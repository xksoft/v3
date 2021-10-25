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
    internal sealed class DefaultValueDecorator : ProtoDecoratorBase
    {
        private readonly object _defaultValue;

        public DefaultValueDecorator(TypeModel model, object defaultValue, IProtoSerializer tail) : base(tail)
        {
            if (defaultValue == null) throw new ArgumentNullException("defaultValue");
            var type = model.MapType(defaultValue.GetType());
            if (type != tail.ExpectedType
#if FEAT_IKVM // in IKVM, we'll have the default value as an underlying type
                && !(tail.ExpectedType.IsEnum && type == tail.ExpectedType.GetEnumUnderlyingType())
#endif
                )
            {
                throw new ArgumentException("Default value is of incorrect type", "defaultValue");
            }
            _defaultValue = defaultValue;
        }

        public override Type ExpectedType
        {
            get { return Tail.ExpectedType; }
        }

        public override bool RequiresOldValue
        {
            get { return Tail.RequiresOldValue; }
        }

        public override bool ReturnsValue
        {
            get { return Tail.ReturnsValue; }
        }

#if !FEAT_IKVM
        public override void Write(object value, ProtoWriter dest)
        {
            if (!Equals(value, _defaultValue))
            {
                Tail.Write(value, dest);
            }
        }

        public override object Read(object value, ProtoReader source)
        {
            return Tail.Read(value, source);
        }
#endif
#if FEAT_COMPILER
        protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            var done = ctx.DefineLabel();
            if (valueFrom == null)
            {
                ctx.CopyValue(); // on the stack
                var needToPop = ctx.DefineLabel();
                EmitBranchIfDefaultValue(ctx, needToPop);
                Tail.EmitWrite(ctx, null);
                ctx.Branch(done, true);
                ctx.MarkLabel(needToPop);
                ctx.DiscardValue();
            }
            else
            {
                ctx.LoadValue(valueFrom); // variable/parameter
                EmitBranchIfDefaultValue(ctx, done);
                Tail.EmitWrite(ctx, valueFrom);
            }
            ctx.MarkLabel(done);
        }

        private void EmitBeq(CompilerContext ctx, CodeLabel label, Type type)
        {
            switch (Helpers.GetTypeCode(type))
            {
                case ProtoTypeCode.Boolean:
                case ProtoTypeCode.Byte:
                case ProtoTypeCode.Char:
                case ProtoTypeCode.Double:
                case ProtoTypeCode.Int16:
                case ProtoTypeCode.Int32:
                case ProtoTypeCode.Int64:
                case ProtoTypeCode.SByte:
                case ProtoTypeCode.Single:
                case ProtoTypeCode.UInt16:
                case ProtoTypeCode.UInt32:
                case ProtoTypeCode.UInt64:
                    ctx.BranchIfEqual(label, false);
                    break;
                default:
                    var method = type.GetMethod("op_Equality", BindingFlags.Public | BindingFlags.Static,
                        null, new[] {type, type}, null);
                    if (method == null || method.ReturnType != ctx.MapType(typeof (bool)))
                    {
                        throw new InvalidOperationException(
                            "No suitable equality operator found for default-values of type: " + type.FullName);
                    }
                    ctx.EmitCall(method);
                    ctx.BranchIfTrue(label, false);
                    break;
            }
        }

        private void EmitBranchIfDefaultValue(CompilerContext ctx, CodeLabel label)
        {
            var expected = ExpectedType;
            switch (Helpers.GetTypeCode(expected))
            {
                case ProtoTypeCode.Boolean:
                    if ((bool) _defaultValue)
                    {
                        ctx.BranchIfTrue(label, false);
                    }
                    else
                    {
                        ctx.BranchIfFalse(label, false);
                    }
                    break;
                case ProtoTypeCode.Byte:
                    if ((byte) _defaultValue == 0)
                    {
                        ctx.BranchIfFalse(label, false);
                    }
                    else
                    {
                        ctx.LoadValue((byte) _defaultValue);
                        EmitBeq(ctx, label, expected);
                    }
                    break;
                case ProtoTypeCode.SByte:
                    if ((sbyte) _defaultValue == 0)
                    {
                        ctx.BranchIfFalse(label, false);
                    }
                    else
                    {
                        ctx.LoadValue((sbyte) _defaultValue);
                        EmitBeq(ctx, label, expected);
                    }
                    break;
                case ProtoTypeCode.Int16:
                    if ((short) _defaultValue == 0)
                    {
                        ctx.BranchIfFalse(label, false);
                    }
                    else
                    {
                        ctx.LoadValue((short) _defaultValue);
                        EmitBeq(ctx, label, expected);
                    }
                    break;
                case ProtoTypeCode.UInt16:
                    if ((ushort) _defaultValue == 0)
                    {
                        ctx.BranchIfFalse(label, false);
                    }
                    else
                    {
                        ctx.LoadValue((ushort) _defaultValue);
                        EmitBeq(ctx, label, expected);
                    }
                    break;
                case ProtoTypeCode.Int32:
                    if ((int) _defaultValue == 0)
                    {
                        ctx.BranchIfFalse(label, false);
                    }
                    else
                    {
                        ctx.LoadValue((int) _defaultValue);
                        EmitBeq(ctx, label, expected);
                    }
                    break;
                case ProtoTypeCode.UInt32:
                    if ((uint) _defaultValue == 0)
                    {
                        ctx.BranchIfFalse(label, false);
                    }
                    else
                    {
                        ctx.LoadValue((int) (uint) _defaultValue);
                        EmitBeq(ctx, label, expected);
                    }
                    break;
                case ProtoTypeCode.Char:
                    if ((char) _defaultValue == (char) 0)
                    {
                        ctx.BranchIfFalse(label, false);
                    }
                    else
                    {
                        ctx.LoadValue((char) _defaultValue);
                        EmitBeq(ctx, label, expected);
                    }
                    break;
                case ProtoTypeCode.Int64:
                    ctx.LoadValue((long) _defaultValue);
                    EmitBeq(ctx, label, expected);
                    break;
                case ProtoTypeCode.UInt64:
                    ctx.LoadValue((long) (ulong) _defaultValue);
                    EmitBeq(ctx, label, expected);
                    break;
                case ProtoTypeCode.Double:
                    ctx.LoadValue((double) _defaultValue);
                    EmitBeq(ctx, label, expected);
                    break;
                case ProtoTypeCode.Single:
                    ctx.LoadValue((float) _defaultValue);
                    EmitBeq(ctx, label, expected);
                    break;
                case ProtoTypeCode.String:
                    ctx.LoadValue((string) _defaultValue);
                    EmitBeq(ctx, label, expected);
                    break;
                case ProtoTypeCode.Decimal:
                {
                    var d = (decimal) _defaultValue;
                    ctx.LoadValue(d);
                    EmitBeq(ctx, label, expected);
                }
                    break;
                case ProtoTypeCode.TimeSpan:
                {
                    var ts = (TimeSpan) _defaultValue;
                    if (ts == TimeSpan.Zero)
                    {
                        ctx.LoadValue(typeof (TimeSpan).GetField("Zero"));
                    }
                    else
                    {
                        ctx.LoadValue(ts.Ticks);
                        ctx.EmitCall(ctx.MapType(typeof (TimeSpan)).GetMethod("FromTicks"));
                    }
                    EmitBeq(ctx, label, expected);
                    break;
                }
                case ProtoTypeCode.Guid:
                {
                    ctx.LoadValue((Guid) _defaultValue);
                    EmitBeq(ctx, label, expected);
                    break;
                }
                case ProtoTypeCode.DateTime:
                {
#if FX11
                        ctx.LoadValue(((DateTime)defaultValue).ToFileTime());
                        ctx.EmitCall(typeof(DateTime).GetMethod("FromFileTime"));                      
#else
                    ctx.LoadValue(((DateTime) _defaultValue).ToBinary());
                    ctx.EmitCall(ctx.MapType(typeof (DateTime)).GetMethod("FromBinary"));
#endif

                    EmitBeq(ctx, label, expected);
                    break;
                }
                default:
                    throw new NotSupportedException("Type cannot be represented as a default value: " +
                                                    expected.FullName);
            }
        }

        protected override void EmitRead(CompilerContext ctx, Local valueFrom)
        {
            Tail.EmitRead(ctx, valueFrom);
        }
#endif
    }
}

#endif