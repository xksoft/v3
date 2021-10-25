using MyProto.Compiler;
using MyProto.Meta;
using xEngine.Plugin.MyProto.Compiler;
#if !NO_RUNTIME
using System;

#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#else

#endif

namespace MyProto.Serializers
{
    internal sealed class BooleanSerializer : IProtoSerializer
    {
#if FEAT_IKVM
        readonly Type expectedType;
#else
        private static readonly Type expectedType = typeof (bool);
#endif

        public BooleanSerializer(TypeModel model)
        {
#if FEAT_IKVM
            expectedType = model.MapType(typeof(bool));
#endif
        }

        public Type ExpectedType
        {
            get { return expectedType; }
        }

#if !FEAT_IKVM
        public void Write(object value, ProtoWriter dest)
        {
            ProtoWriter.WriteBoolean((bool) value, dest);
        }

        public object Read(object value, ProtoReader source)
        {
            Helpers.DebugAssert(value == null); // since replaces
            return source.ReadBoolean();
        }
#endif

        bool IProtoSerializer.RequiresOldValue
        {
            get { return false; }
        }

        bool IProtoSerializer.ReturnsValue
        {
            get { return true; }
        }

#if FEAT_COMPILER
        void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitBasicWrite("WriteBoolean", valueFrom);
        }

        void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitBasicRead("ReadBoolean", ExpectedType);
        }
#endif
    }
}

#endif