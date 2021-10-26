using System.Runtime.Serialization;
using MyProto.Compiler;
using xEngine.Plugin.MyProto.Compiler;
#if !NO_RUNTIME
using System;
using MyProto.Meta;
#if FEAT_COMPILER
#endif
#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#else
using System.Reflection;

#endif

namespace MyProto.Serializers
{
    internal sealed class TypeSerializer : IProtoTypeSerializer
    {
        public bool HasCallbacks(TypeModel.CallbackType callbackType)
        {
            if (_callbacks != null && _callbacks[callbackType] != null) return true;
            for (var i = 0; i < _serializers.Length; i++)
            {
                if (_serializers[i].ExpectedType != _forType &&
                    ((IProtoTypeSerializer) _serializers[i]).HasCallbacks(callbackType)) return true;
            }
            return false;
        }

        private readonly Type _forType, _constructType;
#if WINRT
        private readonly TypeInfo typeInfo;
#endif

        public Type ExpectedType
        {
            get { return _forType; }
        }

        private readonly IProtoSerializer[] _serializers;
        private readonly int[] _fieldNumbers;
        private readonly bool _isRootType, _useConstructor, _isExtensible, _hasConstructor;
        private readonly CallbackSet _callbacks;
        private readonly MethodInfo[] _baseCtorCallbacks;
        private readonly MethodInfo _factory;

        public TypeSerializer(TypeModel model, Type forType, int[] fieldNumbers, IProtoSerializer[] serializers,
            MethodInfo[] baseCtorCallbacks, bool isRootType, bool useConstructor, CallbackSet callbacks,
            Type constructType, MethodInfo factory)
        {
            Helpers.DebugAssert(forType != null);
            Helpers.DebugAssert(fieldNumbers != null);
            Helpers.DebugAssert(serializers != null);
            Helpers.DebugAssert(fieldNumbers.Length == serializers.Length);

            Helpers.Sort(fieldNumbers, serializers);
            var hasSubTypes = false;
            for (var i = 1; i < fieldNumbers.Length; i++)
            {
                if (fieldNumbers[i] == fieldNumbers[i - 1])
                    throw new InvalidOperationException("Duplicate field-number detected; " +
                                                        fieldNumbers[i] + " on: " + forType.FullName);
                if (!hasSubTypes && serializers[i].ExpectedType != forType)
                {
                    hasSubTypes = true;
                }
            }
            _forType = forType;
            _factory = factory;
#if WINRT
            this.typeInfo = forType.GetTypeInfo();
#endif
            if (constructType == null)
            {
                constructType = forType;
            }
            else
            {
#if WINRT
                if (!typeInfo.IsAssignableFrom(constructType.GetTypeInfo()))
#else
                if (!forType.IsAssignableFrom(constructType))
#endif
                {
                    throw new InvalidOperationException(forType.FullName + " cannot be assigned from " +
                                                        constructType.FullName);
                }
            }
            _constructType = constructType;
            _serializers = serializers;
            _fieldNumbers = fieldNumbers;
            _callbacks = callbacks;
            _isRootType = isRootType;
            _useConstructor = useConstructor;

            if (baseCtorCallbacks != null && baseCtorCallbacks.Length == 0) baseCtorCallbacks = null;
            _baseCtorCallbacks = baseCtorCallbacks;
#if !NO_GENERICS
            if (Helpers.GetUnderlyingType(forType) != null)
            {
                throw new ArgumentException("Cannot create a TypeSerializer for nullable types", "forType");
            }
#endif

#if WINRT
            if (iextensible.IsAssignableFrom(typeInfo))
            {
                if (typeInfo.IsValueType || !isRootType || hasSubTypes)
#else
            if (model.MapType(Iextensible).IsAssignableFrom(forType))
            {
                if (forType.IsValueType || !isRootType || hasSubTypes)
#endif
                {
                    throw new NotSupportedException(
                        "IExtensible is not supported in structs or classes with inheritance");
                }
                _isExtensible = true;
            }
#if WINRT
            TypeInfo constructTypeInfo = constructType.GetTypeInfo();
            hasConstructor = !constructTypeInfo.IsAbstract && Helpers.GetConstructor(constructTypeInfo, Helpers.EmptyTypes, true) != null;
#else
            _hasConstructor = !constructType.IsAbstract &&
                              Helpers.GetConstructor(constructType, Helpers.EmptyTypes, true) != null;
#endif
            if (constructType != forType && useConstructor && !_hasConstructor)
            {
                throw new ArgumentException(
                    "The supplied default implementation cannot be created: " + constructType.FullName, "constructType");
            }
        }

#if WINRT
        private static readonly TypeInfo iextensible = typeof(IExtensible).GetTypeInfo();
#else
        private static readonly Type Iextensible = typeof (IExtensible);
#endif

        private bool CanHaveInheritance
        {
            get
            {
#if WINRT
                return (typeInfo.IsClass || typeInfo.IsInterface) && !typeInfo.IsSealed;
#else
                return (_forType.IsClass || _forType.IsInterface) && !_forType.IsSealed;
#endif
            }
        }

        bool IProtoTypeSerializer.CanCreateInstance()
        {
            return true;
        }

#if !FEAT_IKVM
        object IProtoTypeSerializer.CreateInstance(ProtoReader source)
        {
            return CreateInstance(source, false);
        }

        public void Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context)
        {
            if (_callbacks != null) InvokeCallback(_callbacks[callbackType], value, context);
            var ser = (IProtoTypeSerializer) GetMoreSpecificSerializer(value);
            if (ser != null) ser.Callback(value, callbackType, context);
        }

        private IProtoSerializer GetMoreSpecificSerializer(object value)
        {
            if (!CanHaveInheritance) return null;
            var actualType = value.GetType();
            if (actualType == _forType) return null;

            for (var i = 0; i < _serializers.Length; i++)
            {
                var ser = _serializers[i];
                if (ser.ExpectedType != _forType && Helpers.IsAssignableFrom(ser.ExpectedType, actualType))
                {
                    return ser;
                }
            }
            if (actualType == _constructType)
                return null; // needs to be last in case the default concrete type is also a known sub-type
            TypeModel.ThrowUnexpectedSubtype(_forType, actualType); // might throw (if not a proxy)
            return null;
        }

        public void Write(object value, ProtoWriter dest)
        {
            if (_isRootType) Callback(value, TypeModel.CallbackType.BeforeSerialize, dest.Context);
            // write inheritance first
            var next = GetMoreSpecificSerializer(value);
            if (next != null) next.Write(value, dest);

            // write all actual fields
            //Helpers.DebugWriteLine(">> Writing fields for " + forType.FullName);
            for (var i = 0; i < _serializers.Length; i++)
            {
                var ser = _serializers[i];
                if (ser.ExpectedType == _forType)
                {
                    //Helpers.DebugWriteLine(": " + ser.ToString());
                    ser.Write(value, dest);
                }
            }
            //Helpers.DebugWriteLine("<< Writing fields for " + forType.FullName);
            if (_isExtensible) ProtoWriter.AppendExtensionData((IExtensible) value, dest);
            if (_isRootType) Callback(value, TypeModel.CallbackType.AfterSerialize, dest.Context);
        }

        public object Read(object value, ProtoReader source)
        {
            if (_isRootType && value != null)
            {
                Callback(value, TypeModel.CallbackType.BeforeDeserialize, source.Context);
            }
            int fieldNumber, lastFieldNumber = 0, lastFieldIndex = 0;
            bool fieldHandled;

            //Helpers.DebugWriteLine(">> Reading fields for " + forType.FullName);
            while ((fieldNumber = source.ReadFieldHeader()) > 0)
            {
                fieldHandled = false;
                if (fieldNumber < lastFieldNumber)
                {
                    lastFieldNumber = lastFieldIndex = 0;
                }
                for (var i = lastFieldIndex; i < _fieldNumbers.Length; i++)
                {
                    if (_fieldNumbers[i] == fieldNumber)
                    {
                      
                        var ser = _serializers[i];
                        //Helpers.DebugWriteLine(": " + ser.ToString());
                        var serType = ser.ExpectedType;
                        if (value == null)
                        {
                            if (serType == _forType) value = CreateInstance(source, true);
                        }
                        else
                        {
                            if (serType != _forType && ((IProtoTypeSerializer) ser).CanCreateInstance()
                                && serType
#if WINRT
                                .GetTypeInfo()
#endif
                                    .IsSubclassOf(value.GetType()))
                            {
                                value = ProtoReader.Merge(source, value,
                                    ((IProtoTypeSerializer) ser).CreateInstance(source));
                            }
                        }

                        if (ser.ReturnsValue)
                        {
                            value = ser.Read(value, source);
                        }
                        else
                        {
                            // pop
                            ser.Read(value, source);
                        }
  //Console.WriteLine("序列化："+fieldNumber.ToString());
                        lastFieldIndex = i;
                        lastFieldNumber = fieldNumber;
                        fieldHandled = true;
                        break;
                    }
                }
                if (!fieldHandled)
                {
                    //Helpers.DebugWriteLine(": [" + fieldNumber + "] (unknown)");
                    if (value == null) value = CreateInstance(source, true);
                    if (_isExtensible)
                    {
                        source.AppendExtensionData((IExtensible) value);
                    }
                    else
                    {
                        source.SkipField();
                    }
                }
            }
            //Helpers.DebugWriteLine("<< Reading fields for " + forType.FullName);
            if (value == null) value = CreateInstance(source, true);
            if (_isRootType)
            {
                Callback(value, TypeModel.CallbackType.AfterDeserialize, source.Context);
            }
            return value;
        }


        private object InvokeCallback(MethodInfo method, object obj, SerializationContext context)
        {
            object result = null;
            object[] args;
            if (method != null)
            {
                // pass in a streaming context if one is needed, else null
                bool handled;
                var parameters = method.GetParameters();
                switch (parameters.Length)
                {
                    case 0:
                        args = null;
                        handled = true;
                        break;
                    default:
                        args = new object[parameters.Length];
                        handled = true;
                        for (var i = 0; i < args.Length; i++)
                        {
                            object val;
                            var paramType = parameters[i].ParameterType;
                            if (paramType == typeof (SerializationContext)) val = context;
                            else if (paramType == typeof (Type)) val = _constructType;
#if PLAT_BINARYFORMATTER || (SILVERLIGHT && NET_4_0)
                            else if (paramType == typeof (StreamingContext)) val = (StreamingContext) context;
#endif
                            else
                            {
                                val = null;
                                handled = false;
                            }
                            args[i] = val;
                        }
                        break;
                }
                if (handled)
                {
                    result = method.Invoke(obj, args);
                }
                else
                {
                    throw CallbackSet.CreateInvalidCallbackSignature(method);
                }
            }
            return result;
        }

        private object CreateInstance(ProtoReader source, bool includeLocalCallback)
        {
            //Helpers.DebugWriteLine("* creating : " + forType.FullName);
            object obj;
            if (_factory != null)
            {
                obj = InvokeCallback(_factory, null, source.Context);
            }
            else if (_useConstructor)
            {
                if (!_hasConstructor) TypeModel.ThrowCannotCreateInstance(_constructType);
                obj = Activator.CreateInstance(_constructType
#if !CF && !SILVERLIGHT && !WINRT && !PORTABLE
                    , true
#endif
                    );
            }
            else
            {
                obj = BclHelpers.GetUninitializedObject(_constructType);
            }
            ProtoReader.NoteObject(obj, source);
            if (_baseCtorCallbacks != null)
            {
                for (var i = 0; i < _baseCtorCallbacks.Length; i++)
                {
                    InvokeCallback(_baseCtorCallbacks[i], obj, source.Context);
                }
            }
            if (includeLocalCallback && _callbacks != null)
                InvokeCallback(_callbacks.BeforeDeserialize, obj, source.Context);
            return obj;
        }
#endif

        bool IProtoSerializer.RequiresOldValue
        {
            get { return true; }
        }

        bool IProtoSerializer.ReturnsValue
        {
            get { return false; }
        } // updates field directly
#if FEAT_COMPILER
        void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            var expected = ExpectedType;
            using (var loc = ctx.GetLocalWithValue(expected, valueFrom))
            {
                // pre-callbacks
                EmitCallbackIfNeeded(ctx, loc, TypeModel.CallbackType.BeforeSerialize);

                var startFields = ctx.DefineLabel();
                // inheritance
                if (CanHaveInheritance)
                {
                    for (var i = 0; i < _serializers.Length; i++)
                    {
                        var ser = _serializers[i];
                        var serType = ser.ExpectedType;
                        if (serType != _forType)
                        {
                            CodeLabel ifMatch = ctx.DefineLabel(), nextTest = ctx.DefineLabel();
                            ctx.LoadValue(loc);
                            ctx.TryCast(serType);
                            ctx.CopyValue();
                            ctx.BranchIfTrue(ifMatch, true);
                            ctx.DiscardValue();
                            ctx.Branch(nextTest, true);
                            ctx.MarkLabel(ifMatch);
                            ser.EmitWrite(ctx, null);
                            ctx.Branch(startFields, false);
                            ctx.MarkLabel(nextTest);
                        }
                    }


                    if (_constructType != null && _constructType != _forType)
                    {
                        using (var actualType = new Local(ctx, ctx.MapType(typeof (Type))))
                        {
                            // would have jumped to "fields" if an expected sub-type, so two options:
                            // a: *exactly* that type, b: an *unexpected* type
                            ctx.LoadValue(loc);
                            ctx.EmitCall(ctx.MapType(typeof (object)).GetMethod("GetType"));
                            ctx.CopyValue();
                            ctx.StoreValue(actualType);
                            ctx.LoadValue(_forType);
                            ctx.BranchIfEqual(startFields, true);

                            ctx.LoadValue(actualType);
                            ctx.LoadValue(_constructType);
                            ctx.BranchIfEqual(startFields, true);
                        }
                    }
                    else
                    {
                        // would have jumped to "fields" if an expected sub-type, so two options:
                        // a: *exactly* that type, b: an *unexpected* type
                        ctx.LoadValue(loc);
                        ctx.EmitCall(ctx.MapType(typeof (object)).GetMethod("GetType"));
                        ctx.LoadValue(_forType);
                        ctx.BranchIfEqual(startFields, true);
                    }
                    // unexpected, then... note that this *might* be a proxy, which
                    // is handled by ThrowUnexpectedSubtype
                    ctx.LoadValue(_forType);
                    ctx.LoadValue(loc);
                    ctx.EmitCall(ctx.MapType(typeof (object)).GetMethod("GetType"));
                    ctx.EmitCall(ctx.MapType(typeof (TypeModel)).GetMethod("ThrowUnexpectedSubtype",
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static));
                }
                // fields

                ctx.MarkLabel(startFields);
                for (var i = 0; i < _serializers.Length; i++)
                {
                    var ser = _serializers[i];
                    if (ser.ExpectedType == _forType) ser.EmitWrite(ctx, loc);
                }

                // extension data
                if (_isExtensible)
                {
                    ctx.LoadValue(loc);
                    ctx.LoadReaderWriter();
                    ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("AppendExtensionData"));
                }
                // post-callbacks
                EmitCallbackIfNeeded(ctx, loc, TypeModel.CallbackType.AfterSerialize);
            }
        }

        private static void EmitInvokeCallback(CompilerContext ctx, MethodInfo method, bool copyValue,
            Type constructType, Type type)
        {
            if (method != null)
            {
                if (copyValue)
                    ctx.CopyValue(); // assumes the target is on the stack, and that we want to *retain* it on the stack
                var parameters = method.GetParameters();
                var handled = true;

                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameterType = parameters[0].ParameterType;
                    if (parameterType == ctx.MapType(typeof (SerializationContext)))
                    {
                        ctx.LoadSerializationContext();
                    }
                    else if (parameterType == ctx.MapType(typeof (Type)))
                    {
                        var tmp = constructType;
                        if (tmp == null) tmp = type; // no ?? in some C# profiles
                        ctx.LoadValue(tmp);
                    }
#if PLAT_BINARYFORMATTER
                    else if (parameterType == ctx.MapType(typeof (StreamingContext)))
                    {
                        ctx.LoadSerializationContext();
                        var op = ctx.MapType(typeof (SerializationContext))
                            .GetMethod("op_Implicit", new[] {ctx.MapType(typeof (SerializationContext))});
                        if (op != null)
                        {
                            // it isn't always! (framework versions, etc)
                            ctx.EmitCall(op);
                            handled = true;
                        }
                    }
#endif
                    else
                    {
                        handled = false;
                    }
                }
                if (handled)
                {
                    ctx.EmitCall(method);
                    if (constructType != null)
                    {
                        if (method.ReturnType == ctx.MapType(typeof (object)))
                        {
                            ctx.CastFromObject(type);
                        }
                    }
                }
                else
                {
                    throw CallbackSet.CreateInvalidCallbackSignature(method);
                }
            }
        }

        private void EmitCallbackIfNeeded(CompilerContext ctx, Local valueFrom, TypeModel.CallbackType callbackType)
        {
            Helpers.DebugAssert(valueFrom != null);
            if (_isRootType && HasCallbacks(callbackType))
            {
                ((IProtoTypeSerializer) this).EmitCallback(ctx, valueFrom, callbackType);
            }
        }

        void IProtoTypeSerializer.EmitCallback(CompilerContext ctx, Local valueFrom, TypeModel.CallbackType callbackType)
        {
            var actuallyHasInheritance = false;
            if (CanHaveInheritance)
            {
                for (var i = 0; i < _serializers.Length; i++)
                {
                    var ser = _serializers[i];
                    if (ser.ExpectedType != _forType && ((IProtoTypeSerializer) ser).HasCallbacks(callbackType))
                    {
                        actuallyHasInheritance = true;
                    }
                }
            }

            Helpers.DebugAssert(HasCallbacks(callbackType), "Shouldn't be calling this if there is nothing to do");
            var method = _callbacks == null ? null : _callbacks[callbackType];
            if (method == null && !actuallyHasInheritance)
            {
                return;
            }
            ctx.LoadAddress(valueFrom, ExpectedType);
            EmitInvokeCallback(ctx, method, actuallyHasInheritance, null, _forType);

            if (actuallyHasInheritance)
            {
                var @break = ctx.DefineLabel();
                for (var i = 0; i < _serializers.Length; i++)
                {
                    var ser = _serializers[i];
                    IProtoTypeSerializer typeser;
                    var serType = ser.ExpectedType;
                    if (serType != _forType &&
                        (typeser = (IProtoTypeSerializer) ser).HasCallbacks(callbackType))
                    {
                        CodeLabel ifMatch = ctx.DefineLabel(), nextTest = ctx.DefineLabel();
                        ctx.CopyValue();
                        ctx.TryCast(serType);
                        ctx.CopyValue();
                        ctx.BranchIfTrue(ifMatch, true);
                        ctx.DiscardValue();
                        ctx.Branch(nextTest, false);
                        ctx.MarkLabel(ifMatch);
                        typeser.EmitCallback(ctx, null, callbackType);
                        ctx.Branch(@break, false);
                        ctx.MarkLabel(nextTest);
                    }
                }
                ctx.MarkLabel(@break);
                ctx.DiscardValue();
            }
        }

        void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
        {
            var expected = ExpectedType;
            Helpers.DebugAssert(valueFrom != null);

            using (var loc = ctx.GetLocalWithValue(expected, valueFrom))
            using (var fieldNumber = new Local(ctx, ctx.MapType(typeof (int))))
            {
                // pre-callbacks
                if (HasCallbacks(TypeModel.CallbackType.BeforeDeserialize))
                {
                    if (ExpectedType.IsValueType)
                    {
                        EmitCallbackIfNeeded(ctx, loc, TypeModel.CallbackType.BeforeDeserialize);
                    }
                    else
                    {
                        // could be null
                        var callbacksDone = ctx.DefineLabel();
                        ctx.LoadValue(loc);
                        ctx.BranchIfFalse(callbacksDone, false);
                        EmitCallbackIfNeeded(ctx, loc, TypeModel.CallbackType.BeforeDeserialize);
                        ctx.MarkLabel(callbacksDone);
                    }
                }

                CodeLabel @continue = ctx.DefineLabel(), processField = ctx.DefineLabel();
                ctx.Branch(@continue, false);

                ctx.MarkLabel(processField);
                foreach (BasicList.Group group in BasicList.GetContiguousGroups(_fieldNumbers, _serializers))
                {
                    var tryNextField = ctx.DefineLabel();
                    var groupItemCount = group.Items.Count;
                    if (groupItemCount == 1)
                    {
                        // discreet group; use an equality test
                        ctx.LoadValue(fieldNumber);
                        ctx.LoadValue(group.First);
                        var processThisField = ctx.DefineLabel();
                        ctx.BranchIfEqual(processThisField, true);
                        ctx.Branch(tryNextField, false);
                        WriteFieldHandler(ctx, expected, loc, processThisField, @continue,
                            (IProtoSerializer) group.Items[0]);
                    }
                    else
                    {
                        // implement as a jump-table-based switch
                        ctx.LoadValue(fieldNumber);
                        ctx.LoadValue(group.First);
                        ctx.Subtract(); // jump-tables are zero-based
                        var jmp = new CodeLabel[groupItemCount];
                        for (var i = 0; i < groupItemCount; i++)
                        {
                            jmp[i] = ctx.DefineLabel();
                        }
                        ctx.Switch(jmp);
                        // write the default...
                        ctx.Branch(tryNextField, false);
                        for (var i = 0; i < groupItemCount; i++)
                        {
                            WriteFieldHandler(ctx, expected, loc, jmp[i], @continue, (IProtoSerializer) group.Items[i]);
                        }
                    }
                    ctx.MarkLabel(tryNextField);
                }

                EmitCreateIfNull(ctx, loc);
                ctx.LoadReaderWriter();
                if (_isExtensible)
                {
                    ctx.LoadValue(loc);
                    ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("AppendExtensionData"));
                }
                else
                {
                    ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("SkipField"));
                }

                ctx.MarkLabel(@continue);
                ctx.EmitBasicRead("ReadFieldHeader", ctx.MapType(typeof (int)));
                ctx.CopyValue();
                ctx.StoreValue(fieldNumber);
                ctx.LoadValue(0);
                ctx.BranchIfGreater(processField, false);

                EmitCreateIfNull(ctx, loc);
                // post-callbacks
                EmitCallbackIfNeeded(ctx, loc, TypeModel.CallbackType.AfterDeserialize);

                if (valueFrom != null && !loc.IsSame(valueFrom))
                {
                    ctx.LoadValue(loc);
                    ctx.Cast(valueFrom.Type);
                    ctx.StoreValue(valueFrom);
                }
            }
        }

        private void WriteFieldHandler(
            CompilerContext ctx, Type expected, Local loc,
            CodeLabel handler, CodeLabel @continue, IProtoSerializer serializer)
        {
            ctx.MarkLabel(handler);
            var serType = serializer.ExpectedType;
            if (serType == _forType)
            {
                EmitCreateIfNull(ctx, loc);
                serializer.EmitRead(ctx, loc);
            }
            else
            {
                var rtm = (RuntimeTypeModel) ctx.Model;
                if (((IProtoTypeSerializer) serializer).CanCreateInstance())
                {
                    var allDone = ctx.DefineLabel();

                    ctx.LoadValue(loc);
                    ctx.BranchIfFalse(allDone, false); // null is always ok

                    ctx.LoadValue(loc);
                    ctx.TryCast(serType);
                    ctx.BranchIfTrue(allDone, false); // not null, but of the correct type

                    // otherwise, need to convert it
                    ctx.LoadReaderWriter();
                    ctx.LoadValue(loc);
                    ((IProtoTypeSerializer) serializer).EmitCreateInstance(ctx);
                    ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("Merge"));
                    ctx.Cast(expected);
                    ctx.StoreValue(loc); // Merge always returns a value

                    // nothing needs doing
                    ctx.MarkLabel(allDone);
                }
                ctx.LoadValue(loc);
                ctx.Cast(serType);
                serializer.EmitRead(ctx, null);
            }

            if (serializer.ReturnsValue)
            {
                // update the variable
                ctx.StoreValue(loc);
            }
            ctx.Branch(@continue, false); // "continue"
        }

        void IProtoTypeSerializer.EmitCreateInstance(CompilerContext ctx)
        {
            // different ways of creating a new instance
            var callNoteObject = true;
            if (_factory != null)
            {
                EmitInvokeCallback(ctx, _factory, false, _constructType, _forType);
            }
            else if (!_useConstructor)
            {
                // DataContractSerializer style
                ctx.LoadValue(_constructType);
                ctx.EmitCall(ctx.MapType(typeof (BclHelpers)).GetMethod("GetUninitializedObject"));
                ctx.Cast(_forType);
            }
            else if (_constructType.IsClass && _hasConstructor)
            {
                // XmlSerializer style
                ctx.EmitCtor(_constructType);
            }
            else
            {
                ctx.LoadValue(ExpectedType);
                ctx.EmitCall(ctx.MapType(typeof (TypeModel)).GetMethod("ThrowCannotCreateInstance",
                    BindingFlags.Static | BindingFlags.Public));
                ctx.LoadNullRef();
                callNoteObject = false;
            }
            if (callNoteObject)
            {
                // track root object creation
                ctx.CopyValue();
                ctx.LoadReaderWriter();
                ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("NoteObject",
                    BindingFlags.Static | BindingFlags.Public));
            }
            if (_baseCtorCallbacks != null)
            {
                for (var i = 0; i < _baseCtorCallbacks.Length; i++)
                {
                    EmitInvokeCallback(ctx, _baseCtorCallbacks[i], true, null, _forType);
                }
            }
        }

        private void EmitCreateIfNull(CompilerContext ctx, Local storage)
        {
            Helpers.DebugAssert(storage != null);
            if (!ExpectedType.IsValueType)
            {
                var afterNullCheck = ctx.DefineLabel();
                ctx.LoadValue(storage);
                ctx.BranchIfTrue(afterNullCheck, false);

                ((IProtoTypeSerializer) this).EmitCreateInstance(ctx);

                if (_callbacks != null) EmitInvokeCallback(ctx, _callbacks.BeforeDeserialize, true, null, _forType);
                ctx.StoreValue(storage);
                ctx.MarkLabel(afterNullCheck);
            }
        }
#endif
    }
}

#endif