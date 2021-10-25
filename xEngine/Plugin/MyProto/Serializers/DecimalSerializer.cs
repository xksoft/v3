using MyProto.Compiler;
using xEngine.Plugin.MyProto.Compiler;
#if !NO_RUNTIME
using System;
using MyProto.Meta;

#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#else

#endif

namespace MyProto.Serializers
{
    internal sealed class DecimalSerializer : IProtoSerializer
    {
#if FEAT_IKVM
        readonly Type expectedType;
#else
        private static readonly Type expectedType = typeof (decimal);
#endif

        public DecimalSerializer(TypeModel model)
        {
#if FEAT_IKVM
            expectedType = model.MapType(typeof(decimal));
#endif
        }

        public Type ExpectedType
        {
            get { return expectedType; }
        }

        bool IProtoSerializer.RequiresOldValue
        {
            get { return false; }
        }

        bool IProtoSerializer.ReturnsValue
        {
            get { return true; }
        }

#if !FEAT_IKVM
        public object Read(object value, ProtoReader source)
        {
            Helpers.DebugAssert(value == null); // since replaces
            return BclHelpers.ReadDecimal(source);
        }

        public void Write(object value, ProtoWriter dest)
        {
            BclHelpers.WriteDecimal((decimal) value, dest);
        }
#endif
#if FEAT_COMPILER
        void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitWrite(ctx.MapType(typeof (BclHelpers)), "WriteDecimal", valueFrom);
        }

        void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitBasicRead(ctx.MapType(typeof (BclHelpers)), "ReadDecimal", ExpectedType);
        }
#endif
    }
}

#endif