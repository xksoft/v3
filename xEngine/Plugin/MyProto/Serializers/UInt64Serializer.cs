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
    internal sealed class UInt64Serializer : IProtoSerializer
    {
#if FEAT_IKVM
        readonly Type expectedType;
#else
        private static readonly Type expectedType = typeof (ulong);
#endif

        public UInt64Serializer(TypeModel model)
        {
#if FEAT_IKVM
            expectedType = model.MapType(typeof(ulong));
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
            return source.ReadUInt64();
        }

        public void Write(object value, ProtoWriter dest)
        {
            ProtoWriter.WriteUInt64((ulong) value, dest);
        }
#endif

#if FEAT_COMPILER
        void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitBasicWrite("WriteUInt64", valueFrom);
        }

        void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitBasicRead("ReadUInt64", ExpectedType);
        }
#endif
    }
}

#endif