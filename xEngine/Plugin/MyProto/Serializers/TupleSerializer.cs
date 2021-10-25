using MyProto.Compiler;
using xEngine.Plugin.MyProto.Compiler;
#if !NO_RUNTIME
using System;
using MyProto.Meta;
#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#else
using System.Reflection;

#endif

namespace MyProto.Serializers
{
    internal sealed class TupleSerializer : IProtoTypeSerializer
    {
        private readonly ConstructorInfo _ctor;
        private readonly MemberInfo[] _members;
        private readonly IProtoSerializer[] _tails;

        public TupleSerializer(RuntimeTypeModel model, ConstructorInfo ctor, MemberInfo[] members)
        {
            if (ctor == null) throw new ArgumentNullException("ctor");
            if (members == null) throw new ArgumentNullException("members");
            _ctor = ctor;
            _members = members;
            _tails = new IProtoSerializer[members.Length];

            var parameters = ctor.GetParameters();
            for (var i = 0; i < members.Length; i++)
            {
                WireType wireType;
                var finalType = parameters[i].ParameterType;

                Type itemType = null, defaultType = null;

                MetaType.ResolveListTypes(model, finalType, ref itemType, ref defaultType);
                var tmp = itemType == null ? finalType : itemType;

                var asReference = false;
                var typeIndex = model.FindOrAddAuto(tmp, false, true, false);
                if (typeIndex >= 0)
                {
                    asReference = model[tmp].AsReferenceDefault;
                }
                IProtoSerializer tail = ValueMember.TryGetCoreSerializer(model, DataFormat.Default, tmp, out wireType,
                    asReference, false, false, true),
                    serializer;
                if (tail == null)
                {
                    throw new InvalidOperationException("No serializer defined for type: " + tmp.FullName);
                }

                tail = new TagDecorator(i + 1, wireType, false, tail);
                if (itemType == null)
                {
                    serializer = tail;
                }
                else
                {
                    if (finalType.IsArray)
                    {
                        serializer = new ArrayDecorator(model, tail, i + 1, false, wireType, finalType, false, false);
                    }
                    else
                    {
                        serializer = new ListDecorator(model, finalType, defaultType, tail, i + 1, false, wireType, true,
                            false, false);
                    }
                }
                _tails[i] = serializer;
            }
        }

        public bool HasCallbacks(TypeModel.CallbackType callbackType)
        {
            return false;
        }

#if FEAT_COMPILER
        public void EmitCallback(CompilerContext ctx, Local valueFrom, TypeModel.CallbackType callbackType)
        {
        }
#endif

        public Type ExpectedType
        {
            get { return _ctor.DeclaringType; }
        }

        public bool RequiresOldValue
        {
            get { return true; }
        }

        public bool ReturnsValue
        {
            get { return false; }
        }

        bool IProtoTypeSerializer.CanCreateInstance()
        {
            return false;
        }

        private Type GetMemberType(int index)
        {
            var result = Helpers.GetMemberType(_members[index]);
            if (result == null) throw new InvalidOperationException();
            return result;
        }

#if !FEAT_IKVM
        void IProtoTypeSerializer.Callback(object value, TypeModel.CallbackType callbackType,
            SerializationContext context)
        {
        }

        object IProtoTypeSerializer.CreateInstance(ProtoReader source)
        {
            throw new NotSupportedException();
        }

        private object GetValue(object obj, int index)
        {
            PropertyInfo prop;
            FieldInfo field;

            if ((prop = _members[index] as PropertyInfo) != null)
            {
                if (obj == null)
                    return Helpers.IsValueType(prop.PropertyType) ? Activator.CreateInstance(prop.PropertyType) : null;
                return prop.GetValue(obj, null);
            }
            if ((field = _members[index] as FieldInfo) != null)
            {
                if (obj == null)
                    return Helpers.IsValueType(field.FieldType) ? Activator.CreateInstance(field.FieldType) : null;
                return field.GetValue(obj);
            }
            throw new InvalidOperationException();
        }

        public object Read(object value, ProtoReader source)
        {
            var values = new object[_members.Length];
            var invokeCtor = false;
            if (value == null)
            {
                invokeCtor = true;
            }
            for (var i = 0; i < values.Length; i++)
                values[i] = GetValue(value, i);
            int field;
            while ((field = source.ReadFieldHeader()) > 0)
            {
                invokeCtor = true;
                if (field <= _tails.Length)
                {
                    var tail = _tails[field - 1];
                    values[field - 1] = _tails[field - 1].Read(tail.RequiresOldValue ? values[field - 1] : null, source);
                }
                else
                {
                    source.SkipField();
                }
            }
            return invokeCtor ? _ctor.Invoke(values) : value;
        }

        public void Write(object value, ProtoWriter dest)
        {
            for (var i = 0; i < _tails.Length; i++)
            {
                var val = GetValue(value, i);
                if (val != null) _tails[i].Write(val, dest);
            }
        }
#endif
#if FEAT_COMPILER
        public void EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            using (var loc = ctx.GetLocalWithValue(_ctor.DeclaringType, valueFrom))
            {
                for (var i = 0; i < _tails.Length; i++)
                {
                    var type = GetMemberType(i);
                    ctx.LoadAddress(loc, ExpectedType);
                    switch (_members[i].MemberType)
                    {
                        case MemberTypes.Field:
                            ctx.LoadValue((FieldInfo) _members[i]);
                            break;
                        case MemberTypes.Property:
                            ctx.LoadValue((PropertyInfo) _members[i]);
                            break;
                    }
                    ctx.WriteNullCheckedTail(type, _tails[i], null);
                }
            }
        }

        void IProtoTypeSerializer.EmitCreateInstance(CompilerContext ctx)
        {
            throw new NotSupportedException();
        }

        public void EmitRead(CompilerContext ctx, Local incoming)
        {
            using (var objValue = ctx.GetLocalWithValue(ExpectedType, incoming))
            {
                var locals = new Local[_members.Length];
                try
                {
                    for (var i = 0; i < locals.Length; i++)
                    {
                        var type = GetMemberType(i);
                        var store = true;
                        locals[i] = new Local(ctx, type);
                        if (!ExpectedType.IsValueType)
                        {
                            // value-types always read the old value
                            if (type.IsValueType)
                            {
                                switch (Helpers.GetTypeCode(type))
                                {
                                    case ProtoTypeCode.Boolean:
                                    case ProtoTypeCode.Byte:
                                    case ProtoTypeCode.Int16:
                                    case ProtoTypeCode.Int32:
                                    case ProtoTypeCode.SByte:
                                    case ProtoTypeCode.UInt16:
                                    case ProtoTypeCode.UInt32:
                                        ctx.LoadValue(0);
                                        break;
                                    case ProtoTypeCode.Int64:
                                    case ProtoTypeCode.UInt64:
                                        ctx.LoadValue(0L);
                                        break;
                                    case ProtoTypeCode.Single:
                                        ctx.LoadValue(0.0F);
                                        break;
                                    case ProtoTypeCode.Double:
                                        ctx.LoadValue(0.0D);
                                        break;
                                    case ProtoTypeCode.Decimal:
                                        ctx.LoadValue(0M);
                                        break;
                                    case ProtoTypeCode.Guid:
                                        ctx.LoadValue(Guid.Empty);
                                        break;
                                    default:
                                        ctx.LoadAddress(locals[i], type);
                                        ctx.EmitCtor(type);
                                        store = false;
                                        break;
                                }
                            }
                            else
                            {
                                ctx.LoadNullRef();
                            }
                            if (store)
                            {
                                ctx.StoreValue(locals[i]);
                            }
                        }
                    }

                    var skipOld = ExpectedType.IsValueType
                        ? new CodeLabel()
                        : ctx.DefineLabel();
                    if (!ExpectedType.IsValueType)
                    {
                        ctx.LoadAddress(objValue, ExpectedType);
                        ctx.BranchIfFalse(skipOld, false);
                    }
                    for (var i = 0; i < _members.Length; i++)
                    {
                        ctx.LoadAddress(objValue, ExpectedType);
                        switch (_members[i].MemberType)
                        {
                            case MemberTypes.Field:
                                ctx.LoadValue((FieldInfo) _members[i]);
                                break;
                            case MemberTypes.Property:
                                ctx.LoadValue((PropertyInfo) _members[i]);
                                break;
                        }
                        ctx.StoreValue(locals[i]);
                    }

                    if (!ExpectedType.IsValueType) ctx.MarkLabel(skipOld);

                    using (var fieldNumber = new Local(ctx, ctx.MapType(typeof (int))))
                    {
                        CodeLabel @continue = ctx.DefineLabel(),
                            processField = ctx.DefineLabel(),
                            notRecognised = ctx.DefineLabel();
                        ctx.Branch(@continue, false);

                        var handlers = new CodeLabel[_members.Length];
                        for (var i = 0; i < _members.Length; i++)
                        {
                            handlers[i] = ctx.DefineLabel();
                        }

                        ctx.MarkLabel(processField);

                        ctx.LoadValue(fieldNumber);
                        ctx.LoadValue(1);
                        ctx.Subtract(); // jump-table is zero-based
                        ctx.Switch(handlers);

                        // and the default:
                        ctx.Branch(notRecognised, false);
                        for (var i = 0; i < handlers.Length; i++)
                        {
                            ctx.MarkLabel(handlers[i]);
                            var tail = _tails[i];
                            var oldValIfNeeded = tail.RequiresOldValue ? locals[i] : null;
                            ctx.ReadNullCheckedTail(locals[i].Type, tail, oldValIfNeeded);
                            if (tail.ReturnsValue)
                            {
                                if (locals[i].Type.IsValueType)
                                {
                                    ctx.StoreValue(locals[i]);
                                }
                                else
                                {
                                    CodeLabel hasValue = ctx.DefineLabel(), allDone = ctx.DefineLabel();

                                    ctx.CopyValue();
                                    ctx.BranchIfTrue(hasValue, true); // interpret null as "don't assign"
                                    ctx.DiscardValue();
                                    ctx.Branch(allDone, true);
                                    ctx.MarkLabel(hasValue);
                                    ctx.StoreValue(locals[i]);
                                    ctx.MarkLabel(allDone);
                                }
                            }
                            ctx.Branch(@continue, false);
                        }

                        ctx.MarkLabel(notRecognised);
                        ctx.LoadReaderWriter();
                        ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("SkipField"));

                        ctx.MarkLabel(@continue);
                        ctx.EmitBasicRead("ReadFieldHeader", ctx.MapType(typeof (int)));
                        ctx.CopyValue();
                        ctx.StoreValue(fieldNumber);
                        ctx.LoadValue(0);
                        ctx.BranchIfGreater(processField, false);
                    }
                    for (var i = 0; i < locals.Length; i++)
                    {
                        ctx.LoadValue(locals[i]);
                    }

                    ctx.EmitCtor(_ctor);
                    ctx.StoreValue(objValue);
                }
                finally
                {
                    for (var i = 0; i < locals.Length; i++)
                    {
                        if (locals[i] != null)
                            locals[i].Dispose(); // release for re-use
                    }
                }
            }
        }
#endif
    }
}

#endif