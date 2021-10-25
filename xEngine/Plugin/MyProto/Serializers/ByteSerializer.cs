using MyProto.Compiler;
using MyProto.Meta;
using xEngine.Plugin.MyProto.Compiler;
#if !NO_RUNTIME
using System;

#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
#endif

namespace MyProto.Serializers
{
    internal sealed class ByteSerializer : IProtoSerializer
    {
        public Type ExpectedType
        {
            get { return expectedType; }
        }

#if FEAT_IKVM
        readonly Type expectedType;
#else
        private static readonly Type expectedType = typeof (byte);
#endif

        public ByteSerializer(TypeModel model)
        {
#if FEAT_IKVM
            expectedType = model.MapType(typeof(byte));
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
        public void Write(object value, ProtoWriter dest)
        {
            ProtoWriter.WriteByte((byte) value, dest);
        }

        public object Read(object value, ProtoReader source)
        {
            Helpers.DebugAssert(value == null); // since replaces
            return source.ReadByte();
        }
#endif

#if FEAT_COMPILER
        void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitBasicWrite("WriteByte", valueFrom);
        }

        void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitBasicRead("ReadByte", ExpectedType);
        }
#endif
    }
}

#endif