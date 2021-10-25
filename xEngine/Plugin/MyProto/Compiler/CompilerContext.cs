using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;
using MyProto;
using MyProto.Compiler;
using MyProto.Meta;
using MyProto.Serializers;

#if FEAT_COMPILER
//#define DEBUG_COMPILE
#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
using IKVM.Reflection.Emit;
#else

#endif

namespace xEngine.Plugin.MyProto.Compiler
{
    internal struct CodeLabel
    {
        public readonly int Index;
        public readonly Label Value;

        public CodeLabel(Label value, int index)
        {
            Value = value;
            Index = index;
        }
    }

    internal sealed class CompilerContext
    {
        public TypeModel Model
        {
            get { return _model; }
        }

#if !(FX11 || FEAT_IKVM)
        private readonly DynamicMethod _method;
        private static int _next;
#endif

        internal CodeLabel DefineLabel()
        {
            var result = new CodeLabel(_il.DefineLabel(), _nextLabel++);
            return result;
        }

        internal void MarkLabel(CodeLabel label)
        {
            _il.MarkLabel(label.Value);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine("#: " + label.Index);
#endif
        }

#if !(FX11 || FEAT_IKVM)
        public static ProtoSerializer BuildSerializer(IProtoSerializer head, TypeModel model)
        {
            var type = head.ExpectedType;
            var ctx = new CompilerContext(type, true, true, model, typeof (object));
            ctx.LoadValue(ctx.InputValue);
            ctx.CastFromObject(type);
            ctx.WriteNullCheckedTail(type, head, null);
            ctx.Emit(OpCodes.Ret);
            return (ProtoSerializer) ctx._method.CreateDelegate(
                typeof (ProtoSerializer));
        }

        /*public static ProtoCallback BuildCallback(IProtoTypeSerializer head)
        {
            Type type = head.ExpectedType;
            CompilerContext ctx = new CompilerContext(type, true, true);
            using (Local typedVal = new Local(ctx, type))
            {
                ctx.LoadValue(Local.InputValue);
                ctx.CastFromObject(type);
                ctx.StoreValue(typedVal);
                CodeLabel[] jumpTable = new CodeLabel[4];
                for(int i = 0 ; i < jumpTable.Length ; i++) {
                    jumpTable[i] = ctx.DefineLabel();
                }
                ctx.LoadReaderWriter();
                ctx.Switch(jumpTable);
                ctx.Return();
                for(int i = 0 ; i < jumpTable.Length ; i++) {
                    ctx.MarkLabel(jumpTable[i]);
                    if (head.HasCallbacks((TypeModel.CallbackType)i))
                    {
                        head.EmitCallback(ctx, typedVal, (TypeModel.CallbackType)i);
                    }
                    ctx.Return();
                }                
            }
            
            ctx.Emit(OpCodes.Ret);
            return (ProtoCallback)ctx.method.CreateDelegate(
                typeof(ProtoCallback));
        }*/

        public static ProtoDeserializer BuildDeserializer(IProtoSerializer head, TypeModel model)
        {
            var type = head.ExpectedType;
            var ctx = new CompilerContext(type, false, true, model, typeof (object));

            using (var typedVal = new Local(ctx, type))
            {
                if (!type.IsValueType)
                {
                    ctx.LoadValue(ctx.InputValue);
                    ctx.CastFromObject(type);
                    ctx.StoreValue(typedVal);
                }
                else
                {
                    ctx.LoadValue(ctx.InputValue);
                    CodeLabel notNull = ctx.DefineLabel(), endNull = ctx.DefineLabel();
                    ctx.BranchIfTrue(notNull, true);

                    ctx.LoadAddress(typedVal, type);
                    ctx.EmitCtor(type);
                    ctx.Branch(endNull, true);

                    ctx.MarkLabel(notNull);
                    ctx.LoadValue(ctx.InputValue);
                    ctx.CastFromObject(type);
                    ctx.StoreValue(typedVal);

                    ctx.MarkLabel(endNull);
                }
                head.EmitRead(ctx, typedVal);

                if (head.ReturnsValue)
                {
                    ctx.StoreValue(typedVal);
                }

                ctx.LoadValue(typedVal);
                ctx.CastToObject(type);
            }
            ctx.Emit(OpCodes.Ret);
            return (ProtoDeserializer) ctx._method.CreateDelegate(
                typeof (ProtoDeserializer));
        }
#endif

        internal void Return()
        {
            Emit(OpCodes.Ret);
        }

        private static bool IsObject(Type type)
        {
#if FEAT_IKVM
            return type.FullName == "System.Object";
#else
            return type == typeof (object);
#endif
        }

        internal void CastToObject(Type type)
        {
            if (IsObject(type))
            {
            }
            else if (type.IsValueType)
            {
                _il.Emit(OpCodes.Box, type);
#if DEBUG_COMPILE
                Helpers.DebugWriteLine(OpCodes.Box + ": " + type);
#endif
            }
            else
            {
                _il.Emit(OpCodes.Castclass, MapType(typeof (object)));
#if DEBUG_COMPILE
                Helpers.DebugWriteLine(OpCodes.Castclass + ": " + type);
#endif
            }
        }

        internal void CastFromObject(Type type)
        {
            if (IsObject(type))
            {
            }
            else if (type.IsValueType)
            {
                switch (MetadataVersion)
                {
                    case IlVersion.Net1:
                        _il.Emit(OpCodes.Unbox, type);
                        _il.Emit(OpCodes.Ldobj, type);
#if DEBUG_COMPILE
                        Helpers.DebugWriteLine(OpCodes.Unbox + ": " + type);
                        Helpers.DebugWriteLine(OpCodes.Ldobj + ": " + type);
#endif
                        break;
                    default:
#if FX11
                        throw new NotSupportedException();
#else
                        _il.Emit(OpCodes.Unbox_Any, type);
#if DEBUG_COMPILE
                        Helpers.DebugWriteLine(OpCodes.Unbox_Any + ": " + type);
#endif
                        break;
#endif
                }
            }
            else
            {
                _il.Emit(OpCodes.Castclass, type);
#if DEBUG_COMPILE
                Helpers.DebugWriteLine(OpCodes.Castclass + ": " + type);
#endif
            }
        }

        private readonly bool _isStatic;
#if !SILVERLIGHT
        private readonly RuntimeTypeModel.SerializerPair[] _methodPairs;

        internal MethodBuilder GetDedicatedMethod(int metaKey, bool read)
        {
            if (_methodPairs == null) return null;
            // but if we *do* have pairs, we demand that we find a match...
            for (var i = 0; i < _methodPairs.Length; i++)
            {
                if (_methodPairs[i].MetaKey == metaKey)
                {
                    return read ? _methodPairs[i].Deserialize : _methodPairs[i].Serialize;
                }
            }
            throw new ArgumentException("Meta-Key not found", "metaKey");
        }

        internal int MapMetaKeyToCompiledKey(int metaKey)
        {
            if (metaKey < 0 || _methodPairs == null) return metaKey; // all meta, or a dummy/wildcard Key

            for (var i = 0; i < _methodPairs.Length; i++)
            {
                if (_methodPairs[i].MetaKey == metaKey) return i;
            }
            throw new ArgumentException("Key could not be mapped: " + metaKey, "metaKey");
        }
#else
        internal int MapMetaKeyToCompiledKey(int metaKey)
        {
            return metaKey;
        }
#endif

        private readonly bool _isWriter;
#if FX11 || FEAT_IKVM
        internal bool NonPublic { get { return false; } }
#else
        private readonly bool _nonPublic;

        internal bool NonPublic
        {
            get { return _nonPublic; }
        }
#endif


        private readonly Local _inputValue;

        public Local InputValue
        {
            get { return _inputValue; }
        }

#if !(SILVERLIGHT || PHONE8)
        private readonly string _assemblyName;

        internal CompilerContext(ILGenerator il, bool isStatic, bool isWriter,
            RuntimeTypeModel.SerializerPair[] methodPairs, TypeModel model, IlVersion metadataVersion,
            string assemblyName, Type inputType)
        {
            if (il == null) throw new ArgumentNullException("il");
            if (methodPairs == null) throw new ArgumentNullException("methodPairs");
            if (model == null) throw new ArgumentNullException("model");
            if (Helpers.IsNullOrEmpty(assemblyName)) throw new ArgumentNullException("assemblyName");
            _assemblyName = assemblyName;
            _isStatic = isStatic;
            _methodPairs = methodPairs;
            _il = il;
            // nonPublic = false; <== implicit
            _isWriter = isWriter;
            _model = model;
            _metadataVersion = metadataVersion;
            if (inputType != null) _inputValue = new Local(null, inputType);
        }
#endif
#if !(FX11 || FEAT_IKVM)
        private CompilerContext(Type associatedType, bool isWriter, bool isStatic, TypeModel model, Type inputType)
        {
            if (model == null) throw new ArgumentNullException("model");
#if FX11
            metadataVersion = ILVersion.Net1;
#else
            _metadataVersion = IlVersion.Net2;
#endif
            _isStatic = isStatic;
            _isWriter = isWriter;
            _model = model;
            _nonPublic = true;
            Type[] paramTypes;
            Type returnType;
            if (isWriter)
            {
                returnType = typeof (void);
                paramTypes = new[] {typeof (object), typeof (ProtoWriter)};
            }
            else
            {
                returnType = typeof (object);
                paramTypes = new[] {typeof (object), typeof (ProtoReader)};
            }
            int uniqueIdentifier;
#if PLAT_NO_INTERLOCKED
            uniqueIdentifier = ++next;
#else
            uniqueIdentifier = Interlocked.Increment(ref _next);
#endif
            _method = new DynamicMethod("proto_" + uniqueIdentifier, returnType, paramTypes,
                associatedType.IsInterface ? typeof (object) : associatedType, true);
            _il = _method.GetILGenerator();
            if (inputType != null) _inputValue = new Local(null, inputType);
        }

#endif
        private readonly ILGenerator _il;

        private void Emit(OpCode opcode)
        {
            _il.Emit(opcode);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(opcode.ToString());
#endif
        }

        public void LoadValue(string value)
        {
            if (value == null)
            {
                LoadNullRef();
            }
            else
            {
                _il.Emit(OpCodes.Ldstr, value);
#if DEBUG_COMPILE
                Helpers.DebugWriteLine(OpCodes.Ldstr + ": " + value);
#endif
            }
        }

        public void LoadValue(float value)
        {
            _il.Emit(OpCodes.Ldc_R4, value);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(OpCodes.Ldc_R4 + ": " + value);
#endif
        }

        public void LoadValue(double value)
        {
            _il.Emit(OpCodes.Ldc_R8, value);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(OpCodes.Ldc_R8 + ": " + value);
#endif
        }

        public void LoadValue(long value)
        {
            _il.Emit(OpCodes.Ldc_I8, value);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(OpCodes.Ldc_I8 + ": " + value);
#endif
        }

        public void LoadValue(int value)
        {
            switch (value)
            {
                case 0:
                    Emit(OpCodes.Ldc_I4_0);
                    break;
                case 1:
                    Emit(OpCodes.Ldc_I4_1);
                    break;
                case 2:
                    Emit(OpCodes.Ldc_I4_2);
                    break;
                case 3:
                    Emit(OpCodes.Ldc_I4_3);
                    break;
                case 4:
                    Emit(OpCodes.Ldc_I4_4);
                    break;
                case 5:
                    Emit(OpCodes.Ldc_I4_5);
                    break;
                case 6:
                    Emit(OpCodes.Ldc_I4_6);
                    break;
                case 7:
                    Emit(OpCodes.Ldc_I4_7);
                    break;
                case 8:
                    Emit(OpCodes.Ldc_I4_8);
                    break;
                case -1:
                    Emit(OpCodes.Ldc_I4_M1);
                    break;
                default:
                    if (value >= -128 && value <= 127)
                    {
                        _il.Emit(OpCodes.Ldc_I4_S, (sbyte) value);
#if DEBUG_COMPILE
                        Helpers.DebugWriteLine(OpCodes.Ldc_I4_S + ": " + value);
#endif
                    }
                    else
                    {
                        _il.Emit(OpCodes.Ldc_I4, value);
#if DEBUG_COMPILE
                        Helpers.DebugWriteLine(OpCodes.Ldc_I4 + ": " + value);
#endif
                    }
                    break;
            }
        }

        private readonly MutableList _locals = new MutableList();

        internal LocalBuilder GetFromPool(Type type)
        {
            var count = _locals.Count;
            for (var i = 0; i < count; i++)
            {
                var item = (LocalBuilder) _locals[i];
                if (item != null && item.LocalType == type)
                {
                    _locals[i] = null; // remove from pool
                    return item;
                }
            }
            var result = _il.DeclareLocal(type);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine("$ " + result + ": " + type);
#endif
            return result;
        }

        //
        internal void ReleaseToPool(LocalBuilder value)
        {
            var count = _locals.Count;
            for (var i = 0; i < count; i++)
            {
                if (_locals[i] == null)
                {
                    _locals[i] = value; // released into existing slot
                    return;
                }
            }
            _locals.Add(value); // create a new slot
        }

        public void LoadReaderWriter()
        {
            Emit(_isStatic ? OpCodes.Ldarg_1 : OpCodes.Ldarg_2);
        }

        public void StoreValue(Local local)
        {
            if (local == InputValue)
            {
                var b = _isStatic ? (byte) 0 : (byte) 1;
                _il.Emit(OpCodes.Starg_S, b);
#if DEBUG_COMPILE
                Helpers.DebugWriteLine(OpCodes.Starg_S + ": $" + b);
#endif
            }
            else
            {
#if !FX11
                switch (local.Value.LocalIndex)
                {
                    case 0:
                        Emit(OpCodes.Stloc_0);
                        break;
                    case 1:
                        Emit(OpCodes.Stloc_1);
                        break;
                    case 2:
                        Emit(OpCodes.Stloc_2);
                        break;
                    case 3:
                        Emit(OpCodes.Stloc_3);
                        break;
                    default:
#endif
                        var code = UseShortForm(local) ? OpCodes.Stloc_S : OpCodes.Stloc;
                        _il.Emit(code, local.Value);
#if DEBUG_COMPILE
                        Helpers.DebugWriteLine(code + ": $" + local.Value);
#endif
#if !FX11
                        break;
                }
#endif
            }
        }

        public void LoadValue(Local local)
        {
            if (local == null)
            {
                /* nothing to do; top of stack */
            }
            else if (local == InputValue)
            {
                Emit(_isStatic ? OpCodes.Ldarg_0 : OpCodes.Ldarg_1);
            }
            else
            {
#if !FX11
                switch (local.Value.LocalIndex)
                {
                    case 0:
                        Emit(OpCodes.Ldloc_0);
                        break;
                    case 1:
                        Emit(OpCodes.Ldloc_1);
                        break;
                    case 2:
                        Emit(OpCodes.Ldloc_2);
                        break;
                    case 3:
                        Emit(OpCodes.Ldloc_3);
                        break;
                    default:
#endif
                        var code = UseShortForm(local) ? OpCodes.Ldloc_S : OpCodes.Ldloc;
                        _il.Emit(code, local.Value);
#if DEBUG_COMPILE
                        Helpers.DebugWriteLine(code + ": $" + local.Value);
#endif
#if !FX11
                        break;
                }
#endif
            }
        }

        public Local GetLocalWithValue(Type type, Local fromValue)
        {
            if (fromValue != null)
            {
                if (fromValue.Type == type) return fromValue.AsCopy();
                // otherwise, load onto the stack and let the default handling (below) deal with it
                LoadValue(fromValue);
                if (!type.IsValueType && (fromValue.Type == null || !type.IsAssignableFrom(fromValue.Type)))
                {
                    // need to cast
                    Cast(type);
                }
            }
            // need to store the value from the stack
            var result = new Local(this, type);
            StoreValue(result);
            return result;
        }

        internal void EmitBasicRead(string methodName, Type expectedType)
        {
            var method = MapType(typeof (ProtoReader)).GetMethod(
                methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (method == null || method.ReturnType != expectedType
                || method.GetParameters().Length != 0) throw new ArgumentException("methodName");
            LoadReaderWriter();
            EmitCall(method);
        }

        internal void EmitBasicRead(Type helperType, string methodName, Type expectedType)
        {
            var method = helperType.GetMethod(
                methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (method == null || method.ReturnType != expectedType
                || method.GetParameters().Length != 1) throw new ArgumentException("methodName");
            LoadReaderWriter();
            EmitCall(method);
        }

        internal void EmitBasicWrite(string methodName, Local fromValue)
        {
            if (Helpers.IsNullOrEmpty(methodName)) throw new ArgumentNullException("methodName");
            LoadValue(fromValue);
            LoadReaderWriter();
            EmitCall(GetWriterMethod(methodName));
        }

        private MethodInfo GetWriterMethod(string methodName)
        {
            var writerType = MapType(typeof (ProtoWriter));
            var methods =
                writerType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            foreach (var method in methods)
            {
                if (method.Name != methodName) continue;
                var pis = method.GetParameters();
                if (pis.Length == 2 && pis[1].ParameterType == writerType) return method;
            }
            throw new ArgumentException("No suitable method found for: " + methodName, "methodName");
        }

        internal void EmitWrite(Type helperType, string methodName, Local valueFrom)
        {
            if (Helpers.IsNullOrEmpty(methodName)) throw new ArgumentNullException("methodName");
            var method = helperType.GetMethod(
                methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (method == null || method.ReturnType != MapType(typeof (void)))
                throw new ArgumentException("methodName");
            LoadValue(valueFrom);
            LoadReaderWriter();
            EmitCall(method);
        }

        public void EmitCall(MethodInfo method)
        {
            Helpers.DebugAssert(method != null);
            CheckAccessibility(method);
            var opcode = (method.IsStatic || method.DeclaringType.IsValueType) ? OpCodes.Call : OpCodes.Callvirt;
            _il.EmitCall(opcode, method, null);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(opcode + ": " + method + " on " + method.DeclaringType);
#endif
        }

        /// <summary>
        ///     Pushes a null reference onto the stack. Note that this should only
        ///     be used to return a null (or set a variable to null); for null-tests
        ///     use BranchIfTrue / BranchIfFalse.
        /// </summary>
        public void LoadNullRef()
        {
            Emit(OpCodes.Ldnull);
        }

        private int _nextLabel;

        internal void WriteNullCheckedTail(Type type, IProtoSerializer tail, Local valueFrom)
        {
            if (type.IsValueType)
            {
                Type underlyingType = null;
#if !FX11
                underlyingType = Helpers.GetUnderlyingType(type);
#endif
                if (underlyingType == null)
                {
                    // not a nullable T; can invoke directly
                    tail.EmitWrite(this, valueFrom);
                }
                else
                {
                    // nullable T; check HasValue
                    using (var valOrNull = GetLocalWithValue(type, valueFrom))
                    {
                        LoadAddress(valOrNull, type);
                        LoadValue(type.GetProperty("HasValue"));
                        var @end = DefineLabel();
                        BranchIfFalse(@end, false);
                        LoadAddress(valOrNull, type);
                        EmitCall(type.GetMethod("GetValueOrDefault", Helpers.EmptyTypes));
                        tail.EmitWrite(this, null);
                        MarkLabel(@end);
                    }
                }
            }
            else
            {
                // ref-type; do a null-check
                LoadValue(valueFrom);
                CopyValue();
                CodeLabel hasVal = DefineLabel(), @end = DefineLabel();
                BranchIfTrue(hasVal, true);
                DiscardValue();
                Branch(@end, false);
                MarkLabel(hasVal);
                tail.EmitWrite(this, null);
                MarkLabel(@end);
            }
        }

        internal void ReadNullCheckedTail(Type type, IProtoSerializer tail, Local valueFrom)
        {
#if !FX11
            Type underlyingType;

            if (type.IsValueType && (underlyingType = Helpers.GetUnderlyingType(type)) != null)
            {
                if (tail.RequiresOldValue)
                {
                    // we expect the input value to be in valueFrom; need to unpack it from T?
                    using (var loc = GetLocalWithValue(type, valueFrom))
                    {
                        LoadAddress(loc, type);
                        EmitCall(type.GetMethod("GetValueOrDefault", Helpers.EmptyTypes));
                    }
                }
                else
                {
                    Helpers.DebugAssert(valueFrom == null); // not expecting a valueFrom in this case
                }
                tail.EmitRead(this, null); // either unwrapped on the stack or not provided
                if (tail.ReturnsValue)
                {
                    // now re-wrap the value
                    EmitCtor(type, underlyingType);
                }
                return;
            }
#endif
            // either a ref-type of a non-nullable struct; treat "as is", even if null
            // (the type-serializer will handle the null case; it needs to allow null
            // inputs to perform the correct type of subclass creation)
            tail.EmitRead(this, valueFrom);
        }

        public void EmitCtor(Type type)
        {
            EmitCtor(type, Helpers.EmptyTypes);
        }

        public void EmitCtor(ConstructorInfo ctor)
        {
            if (ctor == null) throw new ArgumentNullException("ctor");
            CheckAccessibility(ctor);
            _il.Emit(OpCodes.Newobj, ctor);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(OpCodes.Newobj + ": " + ctor.DeclaringType);
#endif
        }

        public void EmitCtor(Type type, params Type[] parameterTypes)
        {
            Helpers.DebugAssert(type != null);
            Helpers.DebugAssert(parameterTypes != null);
            if (type.IsValueType && parameterTypes.Length == 0)
            {
                _il.Emit(OpCodes.Initobj, type);
#if DEBUG_COMPILE
                Helpers.DebugWriteLine(OpCodes.Initobj + ": " + type);
#endif
            }
            else
            {
                var ctor = Helpers.GetConstructor(type, parameterTypes, true);
                if (ctor == null)
                    throw new InvalidOperationException("No suitable constructor found for " + type.FullName);
                EmitCtor(ctor);
            }
        }

#if !(PHONE8 || SILVERLIGHT || FX11)
        private BasicList _knownTrustedAssemblies, _knownUntrustedAssemblies;
#endif

        private bool InternalsVisible(Assembly assembly)
        {
#if PHONE8 || SILVERLIGHT || FX11
            return false;
#else
            if (Helpers.IsNullOrEmpty(_assemblyName)) return false;
            if (_knownTrustedAssemblies != null)
            {
                if (_knownTrustedAssemblies.IndexOfReference(assembly) >= 0)
                {
                    return true;
                }
            }
            if (_knownUntrustedAssemblies != null)
            {
                if (_knownUntrustedAssemblies.IndexOfReference(assembly) >= 0)
                {
                    return false;
                }
            }
            var isTrusted = false;
            var attributeType = MapType(typeof (InternalsVisibleToAttribute));
            if (attributeType == null) return false;
#if FEAT_IKVM
            foreach (CustomAttributeData attrib in assembly.__GetCustomAttributes(attributeType, false))
            {
                if (attrib.ConstructorArguments.Count == 1)
                {
                    string privelegedAssembly = attrib.ConstructorArguments[0].Value as string;
                    if (privelegedAssembly == assemblyName)
                    {
                        isTrusted = true;
                        break;
                    }
                }
            }
#else
            foreach (InternalsVisibleToAttribute attrib in assembly.GetCustomAttributes(attributeType, false))
            {
                if (attrib.AssemblyName == _assemblyName)
                {
                    isTrusted = true;
                    break;
                }
            }
#endif
            if (isTrusted)
            {
                if (_knownTrustedAssemblies == null) _knownTrustedAssemblies = new BasicList();
                _knownTrustedAssemblies.Add(assembly);
            }
            else
            {
                if (_knownUntrustedAssemblies == null) _knownUntrustedAssemblies = new BasicList();
                _knownUntrustedAssemblies.Add(assembly);
            }
            return isTrusted;
#endif
        }

        internal void CheckAccessibility(MemberInfo member)
        {
            if (member == null)
            {
                throw new ArgumentNullException("member");
            }

            var memberType = member.MemberType;
            Type type;
            if (!NonPublic)
            {
                bool isPublic;
                switch (memberType)
                {
                    case MemberTypes.TypeInfo:
                        // top-level type
                        type = (Type) member;
                        isPublic = type.IsPublic || InternalsVisible(type.Assembly);
                        break;
                    case MemberTypes.NestedType:
                        type = (Type) member;
                        do
                        {
                            isPublic = type.IsNestedPublic || type.IsPublic ||
                                       ((type.DeclaringType == null || type.IsNestedAssembly || type.IsNestedFamORAssem) &&
                                        InternalsVisible(type.Assembly));
                        } while (isPublic && (type = type.DeclaringType) != null);
                        // ^^^ !type.IsNested, but not all runtimes have that
                        break;
                    case MemberTypes.Field:
                        var field = ((FieldInfo) member);
                        isPublic = field.IsPublic ||
                                   ((field.IsAssembly || field.IsFamilyOrAssembly) &&
                                    InternalsVisible(field.DeclaringType.Assembly));
                        break;
                    case MemberTypes.Constructor:
                        var ctor = ((ConstructorInfo) member);
                        isPublic = ctor.IsPublic ||
                                   ((ctor.IsAssembly || ctor.IsFamilyOrAssembly) &&
                                    InternalsVisible(ctor.DeclaringType.Assembly));
                        break;
                    case MemberTypes.Method:
                        var method = ((MethodInfo) member);
                        isPublic = method.IsPublic ||
                                   ((method.IsAssembly || method.IsFamilyOrAssembly) &&
                                    InternalsVisible(method.DeclaringType.Assembly));
                        if (!isPublic)
                        {
                            // allow calls to TypeModel protected methods, and methods we are in the process of creating
                            if (
#if !SILVERLIGHT
                                member is MethodBuilder ||
#endif
                                    member.DeclaringType == MapType(typeof (TypeModel))) isPublic = true;
                        }
                        break;
                    case MemberTypes.Property:
                        isPublic = true; // defer to get/set
                        break;
                    default:
                        throw new NotSupportedException(memberType.ToString());
                }
                if (!isPublic)
                {
                    switch (memberType)
                    {
                        case MemberTypes.TypeInfo:
                        case MemberTypes.NestedType:
                            throw new InvalidOperationException(
                                "Non-public type cannot be used with full dll compilation: " +
                                ((Type) member).FullName);
                        default:
                            throw new InvalidOperationException(
                                "Non-public member cannot be used with full dll compilation: " +
                                member.DeclaringType.FullName + "." + member.Name);
                    }
                }
            }
        }

        public void LoadValue(FieldInfo field)
        {
            CheckAccessibility(field);
            var code = field.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld;
            _il.Emit(code, field);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(code + ": " + field + " on " + field.DeclaringType);
#endif
        }

#if FEAT_IKVM
        public void StoreValue(System.Reflection.FieldInfo field)
        {
            StoreValue(MapType(field.DeclaringType).GetField(field.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance));
        }
        public void StoreValue(System.Reflection.PropertyInfo property)
        {
            StoreValue(MapType(property.DeclaringType).GetProperty(property.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance));
        }
        public void LoadValue(System.Reflection.FieldInfo field)
        {
            LoadValue(MapType(field.DeclaringType).GetField(field.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance));
        }
        public void LoadValue(System.Reflection.PropertyInfo property)
        {
            LoadValue(MapType(property.DeclaringType).GetProperty(property.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance));
        }
#endif

        public void StoreValue(FieldInfo field)
        {
            CheckAccessibility(field);
            var code = field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld;
            _il.Emit(code, field);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(code + ": " + field + " on " + field.DeclaringType);
#endif
        }

        public void LoadValue(PropertyInfo property)
        {
            CheckAccessibility(property);
            EmitCall(Helpers.GetGetMethod(property, true, true));
        }

        public void StoreValue(PropertyInfo property)
        {
            CheckAccessibility(property);
            EmitCall(Helpers.GetSetMethod(property, true, true));
        }

        //internal void EmitInstance()
        //{
        //    if (isStatic) throw new InvalidOperationException();
        //    Emit(OpCodes.Ldarg_0);
        //}

        internal static void LoadValue(ILGenerator il, int value)
        {
            switch (value)
            {
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    break;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    break;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    break;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    break;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    break;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    break;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    break;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    break;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    break;
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    break;
                default:
                    il.Emit(OpCodes.Ldc_I4, value);
                    break;
            }
        }

        private bool UseShortForm(Local local)
        {
#if FX11
            return locals.Count < 256;
#else
            return local.Value.LocalIndex < 256;
#endif
        }

#if FEAT_IKVM
        internal void LoadAddress(Local local, System.Type type)
        {
            LoadAddress(local, MapType(type));
        }
#endif

        internal void LoadAddress(Local local, Type type)
        {
            if (type.IsValueType)
            {
                if (local == null)
                {
                    throw new InvalidOperationException("Cannot load the address of a struct at the head of the stack");
                }

                if (local == InputValue)
                {
                    _il.Emit(OpCodes.Ldarga_S, (_isStatic ? (byte) 0 : (byte) 1));
#if DEBUG_COMPILE
                    Helpers.DebugWriteLine(OpCodes.Ldarga_S + ": $" + (isStatic ? 0 : 1));
#endif
                }
                else
                {
                    var code = UseShortForm(local) ? OpCodes.Ldloca_S : OpCodes.Ldloca;
                    _il.Emit(code, local.Value);
#if DEBUG_COMPILE
                    Helpers.DebugWriteLine(code + ": $" + local.Value);
#endif
                }
            }
            else
            {
                // reference-type; already *is* the address; just load it
                LoadValue(local);
            }
        }

        internal void Branch(CodeLabel label, bool @short)
        {
            var code = @short ? OpCodes.Br_S : OpCodes.Br;
            _il.Emit(code, label.Value);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(code + ": " + label.Index);
#endif
        }

        internal void BranchIfFalse(CodeLabel label, bool @short)
        {
            var code = @short ? OpCodes.Brfalse_S : OpCodes.Brfalse;
            _il.Emit(code, label.Value);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(code + ": " + label.Index);
#endif
        }


        internal void BranchIfTrue(CodeLabel label, bool @short)
        {
            var code = @short ? OpCodes.Brtrue_S : OpCodes.Brtrue;
            _il.Emit(code, label.Value);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(code + ": " + label.Index);
#endif
        }

        internal void BranchIfEqual(CodeLabel label, bool @short)
        {
            var code = @short ? OpCodes.Beq_S : OpCodes.Beq;
            _il.Emit(code, label.Value);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(code + ": " + label.Index);
#endif
        }

        //internal void TestEqual()
        //{
        //    Emit(OpCodes.Ceq);
        //}


        internal void CopyValue()
        {
            Emit(OpCodes.Dup);
        }

        internal void BranchIfGreater(CodeLabel label, bool @short)
        {
            var code = @short ? OpCodes.Bgt_S : OpCodes.Bgt;
            _il.Emit(code, label.Value);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(code + ": " + label.Index);
#endif
        }

        internal void BranchIfLess(CodeLabel label, bool @short)
        {
            var code = @short ? OpCodes.Blt_S : OpCodes.Blt;
            _il.Emit(code, label.Value);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(code + ": " + label.Index);
#endif
        }

        internal void DiscardValue()
        {
            Emit(OpCodes.Pop);
        }

        public void Subtract()
        {
            Emit(OpCodes.Sub);
        }


        public void Switch(CodeLabel[] jumpTable)
        {
            var labels = new Label[jumpTable.Length];
#if DEBUG_COMPILE
            StringBuilder sb = new StringBuilder(OpCodes.Switch.ToString());
#endif
            for (var i = 0; i < labels.Length; i++)
            {
                labels[i] = jumpTable[i].Value;
#if DEBUG_COMPILE
                sb.Append("; ").Append(i).Append("=>").Append(jumpTable[i].Index);
#endif
            }

            _il.Emit(OpCodes.Switch, labels);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(sb.ToString());
#endif
        }

        internal void EndFinally()
        {
            _il.EndExceptionBlock();
#if DEBUG_COMPILE
            Helpers.DebugWriteLine("EndExceptionBlock");
#endif
        }

        internal void BeginFinally()
        {
            _il.BeginFinallyBlock();
#if DEBUG_COMPILE
            Helpers.DebugWriteLine("BeginFinallyBlock");
#endif
        }

        internal void EndTry(CodeLabel label, bool @short)
        {
            var code = @short ? OpCodes.Leave_S : OpCodes.Leave;
            _il.Emit(code, label.Value);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(code + ": " + label.Index);
#endif
        }

        internal CodeLabel BeginTry()
        {
            var label = new CodeLabel(_il.BeginExceptionBlock(), _nextLabel++);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine("BeginExceptionBlock: " + label.Index);
#endif
            return label;
        }

#if !FX11
        internal void Constrain(Type type)
        {
            _il.Emit(OpCodes.Constrained, type);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(OpCodes.Constrained + ": " + type);
#endif
        }
#endif

        internal void TryCast(Type type)
        {
            _il.Emit(OpCodes.Isinst, type);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(OpCodes.Isinst + ": " + type);
#endif
        }

        internal void Cast(Type type)
        {
            _il.Emit(OpCodes.Castclass, type);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(OpCodes.Castclass + ": " + type);
#endif
        }

        public IDisposable Using(Local local)
        {
            return new UsingBlock(this, local);
        }

        private sealed class UsingBlock : IDisposable
        {
            private CompilerContext _ctx;
            private CodeLabel _label;
            private Local _local;

            /// <summary>
            ///     Creates a new "using" block (equivalent) around a variable;
            ///     the variable must exist, and note that (unlike in C#) it is
            ///     the variables *final* value that gets disposed. If you need
            ///     *original* disposal, copy your variable first.
            ///     It is the callers responsibility to ensure that the variable's
            ///     scope fully-encapsulates the "using"; if not, the variable
            ///     may be re-used (and thus re-assigned) unexpectedly.
            /// </summary>
            public UsingBlock(CompilerContext ctx, Local local)
            {
                if (ctx == null) throw new ArgumentNullException("ctx");
                if (local == null) throw new ArgumentNullException("local");

                var type = local.Type;
                // check if **never** disposable
                if ((type.IsValueType || type.IsSealed) &&
                    !ctx.MapType(typeof (IDisposable)).IsAssignableFrom(type))
                {
                    return; // nothing to do! easiest "using" block ever
                    // (note that C# wouldn't allow this as a "using" block,
                    // but we'll be generous and simply not do anything)
                }
                _local = local;
                _ctx = ctx;
                _label = ctx.BeginTry();
            }

            public void Dispose()
            {
                if (_local == null || _ctx == null) return;

                _ctx.EndTry(_label, false);
                _ctx.BeginFinally();
                var disposableType = _ctx.MapType(typeof (IDisposable));
                var dispose = disposableType.GetMethod("Dispose");
                var type = _local.Type;
                // remember that we've already (in the .ctor) excluded the case
                // where it *cannot* be disposable
                if (type.IsValueType)
                {
                    _ctx.LoadAddress(_local, type);
                    switch (_ctx.MetadataVersion)
                    {
                        case IlVersion.Net1:
                            _ctx.LoadValue(_local);
                            _ctx.CastToObject(type);
                            break;
                        default:
#if FX11
                            throw new NotSupportedException();
#else
                            _ctx.Constrain(type);
                            break;
#endif
                    }
                    _ctx.EmitCall(dispose);
                }
                else
                {
                    var @null = _ctx.DefineLabel();
                    if (disposableType.IsAssignableFrom(type))
                    {
                        // *known* to be IDisposable; just needs a null-check                            
                        _ctx.LoadValue(_local);
                        _ctx.BranchIfFalse(@null, true);
                        _ctx.LoadAddress(_local, type);
                    }
                    else
                    {
                        // *could* be IDisposable; test via "as"
                        using (var disp = new Local(_ctx, disposableType))
                        {
                            _ctx.LoadValue(_local);
                            _ctx.TryCast(disposableType);
                            _ctx.CopyValue();
                            _ctx.StoreValue(disp);
                            _ctx.BranchIfFalse(@null, true);
                            _ctx.LoadAddress(disp, disposableType);
                        }
                    }
                    _ctx.EmitCall(dispose);
                    _ctx.MarkLabel(@null);
                }
                _ctx.EndFinally();
                _local = null;
                _ctx = null;
                _label = new CodeLabel(); // default
            }
        }

        internal void Add()
        {
            Emit(OpCodes.Add);
        }

        internal void LoadLength(Local arr, bool zeroIfNull)
        {
            Helpers.DebugAssert(arr.Type.IsArray && arr.Type.GetArrayRank() == 1);

            if (zeroIfNull)
            {
                CodeLabel notNull = DefineLabel(), done = DefineLabel();
                LoadValue(arr);
                CopyValue(); // optimised for non-null case
                BranchIfTrue(notNull, true);
                DiscardValue();
                LoadValue(0);
                Branch(done, true);
                MarkLabel(notNull);
                Emit(OpCodes.Ldlen);
                Emit(OpCodes.Conv_I4);
                MarkLabel(done);
            }
            else
            {
                LoadValue(arr);
                Emit(OpCodes.Ldlen);
                Emit(OpCodes.Conv_I4);
            }
        }

        internal void CreateArray(Type elementType, Local length)
        {
            LoadValue(length);
            _il.Emit(OpCodes.Newarr, elementType);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(OpCodes.Newarr + ": " + elementType);
#endif
        }

        internal void LoadArrayValue(Local arr, Local i)
        {
            var type = arr.Type;
            Helpers.DebugAssert(type.IsArray && arr.Type.GetArrayRank() == 1);
            type = type.GetElementType();
            Helpers.DebugAssert(type != null, "Not an array: " + arr.Type.FullName);
            LoadValue(arr);
            LoadValue(i);
            switch (Helpers.GetTypeCode(type))
            {
                case ProtoTypeCode.SByte:
                    Emit(OpCodes.Ldelem_I1);
                    break;
                case ProtoTypeCode.Int16:
                    Emit(OpCodes.Ldelem_I2);
                    break;
                case ProtoTypeCode.Int32:
                    Emit(OpCodes.Ldelem_I4);
                    break;
                case ProtoTypeCode.Int64:
                    Emit(OpCodes.Ldelem_I8);
                    break;

                case ProtoTypeCode.Byte:
                    Emit(OpCodes.Ldelem_U1);
                    break;
                case ProtoTypeCode.UInt16:
                    Emit(OpCodes.Ldelem_U2);
                    break;
                case ProtoTypeCode.UInt32:
                    Emit(OpCodes.Ldelem_U4);
                    break;
                case ProtoTypeCode.UInt64:
                    Emit(OpCodes.Ldelem_I8);
                    break; // odd, but this is what C# does...

                case ProtoTypeCode.Single:
                    Emit(OpCodes.Ldelem_R4);
                    break;
                case ProtoTypeCode.Double:
                    Emit(OpCodes.Ldelem_R8);
                    break;
                default:
                    if (type.IsValueType)
                    {
                        _il.Emit(OpCodes.Ldelema, type);
                        _il.Emit(OpCodes.Ldobj, type);
#if DEBUG_COMPILE
                        Helpers.DebugWriteLine(OpCodes.Ldelema + ": " + type);
                        Helpers.DebugWriteLine(OpCodes.Ldobj + ": " + type);
#endif
                    }
                    else
                    {
                        Emit(OpCodes.Ldelem_Ref);
                    }

                    break;
            }
        }


        internal void LoadValue(Type type)
        {
            _il.Emit(OpCodes.Ldtoken, type);
#if DEBUG_COMPILE
            Helpers.DebugWriteLine(OpCodes.Ldtoken + ": " + type);
#endif
            EmitCall(MapType(typeof (Type)).GetMethod("GetTypeFromHandle"));
        }

        internal void ConvertToInt32(ProtoTypeCode typeCode, bool uint32Overflow)
        {
            switch (typeCode)
            {
                case ProtoTypeCode.Byte:
                case ProtoTypeCode.SByte:
                case ProtoTypeCode.Int16:
                case ProtoTypeCode.UInt16:
                    Emit(OpCodes.Conv_I4);
                    break;
                case ProtoTypeCode.Int32:
                    break;
                case ProtoTypeCode.Int64:
                    Emit(OpCodes.Conv_Ovf_I4);
                    break;
                case ProtoTypeCode.UInt32:
                    Emit(uint32Overflow ? OpCodes.Conv_Ovf_I4_Un : OpCodes.Conv_Ovf_I4);
                    break;
                case ProtoTypeCode.UInt64:
                    Emit(OpCodes.Conv_Ovf_I4_Un);
                    break;
                default:
                    throw new InvalidOperationException("ConvertToInt32 not implemented for: " + typeCode);
            }
        }

        internal void ConvertFromInt32(ProtoTypeCode typeCode, bool uint32Overflow)
        {
            switch (typeCode)
            {
                case ProtoTypeCode.SByte:
                    Emit(OpCodes.Conv_Ovf_I1);
                    break;
                case ProtoTypeCode.Byte:
                    Emit(OpCodes.Conv_Ovf_U1);
                    break;
                case ProtoTypeCode.Int16:
                    Emit(OpCodes.Conv_Ovf_I2);
                    break;
                case ProtoTypeCode.UInt16:
                    Emit(OpCodes.Conv_Ovf_U2);
                    break;
                case ProtoTypeCode.Int32:
                    break;
                case ProtoTypeCode.UInt32:
                    Emit(uint32Overflow ? OpCodes.Conv_Ovf_U4 : OpCodes.Conv_U4);
                    break;
                case ProtoTypeCode.Int64:
                    Emit(OpCodes.Conv_I8);
                    break;
                case ProtoTypeCode.UInt64:
                    Emit(OpCodes.Conv_U8);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        internal void LoadValue(decimal value)
        {
            if (value == 0M)
            {
                LoadValue(typeof (decimal).GetField("Zero"));
            }
            else
            {
                var bits = decimal.GetBits(value);
                LoadValue(bits[0]); // lo
                LoadValue(bits[1]); // mid
                LoadValue(bits[2]); // hi
                LoadValue((int) (((uint) bits[3]) >> 31)); // isNegative (bool, but int for CLI purposes)
                LoadValue((bits[3] >> 16) & 0xFF); // scale (byte, but int for CLI purposes)

                EmitCtor(MapType(typeof (decimal)), MapType(typeof (int)), MapType(typeof (int)), MapType(typeof (int)),
                    MapType(typeof (bool)), MapType(typeof (byte)));
            }
        }

        internal void LoadValue(Guid value)
        {
            if (value == Guid.Empty)
            {
                LoadValue(typeof (Guid).GetField("Empty"));
            }
            else
            {
                // note we're adding lots of shorts/bytes here - but at the IL level they are I4, not I1/I2 (which barely exist)
                var bytes = value.ToByteArray();
                var i = (bytes[0]) | (bytes[1] << 8) | (bytes[2] << 16) | (bytes[3] << 24);
                LoadValue(i);
                var s = (short) ((bytes[4]) | (bytes[5] << 8));
                LoadValue(s);
                s = (short) ((bytes[6]) | (bytes[7] << 8));
                LoadValue(s);
                for (i = 8; i <= 15; i++)
                {
                    LoadValue(bytes[i]);
                }
                EmitCtor(MapType(typeof (Guid)), MapType(typeof (int)), MapType(typeof (short)), MapType(typeof (short)),
                    MapType(typeof (byte)), MapType(typeof (byte)), MapType(typeof (byte)), MapType(typeof (byte)),
                    MapType(typeof (byte)), MapType(typeof (byte)), MapType(typeof (byte)), MapType(typeof (byte)));
            }
        }

        //internal void LoadValue(bool value)
        //{
        //    Emit(value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
        //}

        internal void LoadSerializationContext()
        {
            LoadReaderWriter();
            LoadValue((_isWriter ? typeof (ProtoWriter) : typeof (ProtoReader)).GetProperty("Context"));
        }

        private readonly TypeModel _model;

        internal Type MapType(Type type)
        {
            return _model.MapType(type);
        }

        private readonly IlVersion _metadataVersion;

        public IlVersion MetadataVersion
        {
            get { return _metadataVersion; }
        }

        public enum IlVersion
        {
            Net1,
            Net2
        }

        internal bool AllowInternal(PropertyInfo property)
        {
            return NonPublic ? true : InternalsVisible(property.DeclaringType.Assembly);
        }
    }
}

#endif