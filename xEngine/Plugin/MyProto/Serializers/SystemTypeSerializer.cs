using System;
using MyProto.Compiler;
using MyProto.Meta;
using xEngine.Plugin.MyProto.Compiler;

#if !NO_RUNTIME

#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#else

#endif

namespace MyProto.Serializers
{
    internal sealed class SystemTypeSerializer : IProtoSerializer
    {
#if FEAT_IKVM
        readonly Type expectedType;
#else
        private static readonly Type expectedType = typeof (Type);
#endif

        public SystemTypeSerializer(TypeModel model)
        {
#if FEAT_IKVM
            expectedType = model.MapType(typeof(System.Type));
#endif
        }

        public Type ExpectedType
        {
            get { return expectedType; }
        }

#if !FEAT_IKVM
        void IProtoSerializer.Write(object value, ProtoWriter dest)
        {
            ProtoWriter.WriteType((Type) value, dest);
        }

        object IProtoSerializer.Read(object value, ProtoReader source)
        {
            Helpers.DebugAssert(value == null); // since replaces
            return source.ReadType();
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
            ctx.EmitBasicWrite("WriteType", valueFrom);
        }

        void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitBasicRead("ReadType", ExpectedType);
        }
#endif
    }
}

#endif