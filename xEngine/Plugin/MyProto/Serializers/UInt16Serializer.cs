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
    internal class UInt16Serializer : IProtoSerializer
    {
#if FEAT_IKVM
        readonly Type expectedType;
#else
        private static readonly Type expectedType = typeof (ushort);
#endif

        public UInt16Serializer(TypeModel model)
        {
#if FEAT_IKVM
            expectedType = model.MapType(typeof(ushort));
#endif
        }

        public virtual Type ExpectedType
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
        public virtual object Read(object value, ProtoReader source)
        {
            Helpers.DebugAssert(value == null); // since replaces
            return source.ReadUInt16();
        }

        public virtual void Write(object value, ProtoWriter dest)
        {
            ProtoWriter.WriteUInt16((ushort) value, dest);
        }
#endif
#if FEAT_COMPILER
        void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitBasicWrite("WriteUInt16", valueFrom);
        }

        void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitBasicRead("ReadUInt16", ctx.MapType(typeof (ushort)));
        }
#endif
    }
}

#endif