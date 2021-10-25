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
    internal sealed class SingleSerializer : IProtoSerializer
    {
#if FEAT_IKVM
        readonly Type expectedType;
#else
        private static readonly Type expectedType = typeof (float);
#endif

        public Type ExpectedType
        {
            get { return expectedType; }
        }

        public SingleSerializer(TypeModel model)
        {
#if FEAT_IKVM
            expectedType = model.MapType(typeof(float));
#endif
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
            return source.ReadSingle();
        }

        public void Write(object value, ProtoWriter dest)
        {
            ProtoWriter.WriteSingle((float) value, dest);
        }
#endif

#if FEAT_COMPILER
        void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitBasicWrite("WriteSingle", valueFrom);
        }

        void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitBasicRead("ReadSingle", ExpectedType);
        }
#endif
    }
}

#endif