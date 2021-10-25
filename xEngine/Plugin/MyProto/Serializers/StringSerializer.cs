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
    internal sealed class StringSerializer : IProtoSerializer
    {
#if FEAT_IKVM
        readonly Type expectedType;
#else
        private static readonly Type expectedType = typeof (string);
#endif

        public StringSerializer(TypeModel model)
        {
#if FEAT_IKVM
            expectedType = model.MapType(typeof(string));
#endif
        }

        public Type ExpectedType
        {
            get { return expectedType; }
        }

        public void Write(object value, ProtoWriter dest)
        {
            ProtoWriter.WriteString((string) value, dest);
        }

        bool IProtoSerializer.RequiresOldValue
        {
            get { return false; }
        }

        bool IProtoSerializer.ReturnsValue
        {
            get { return true; }
        }

        public object Read(object value, ProtoReader source)
        {
            Helpers.DebugAssert(value == null); // since replaces
            return source.ReadString();
        }

#if FEAT_COMPILER
        void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitBasicWrite("WriteString", valueFrom);
        }

        void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitBasicRead("ReadString", ExpectedType);
        }
#endif
    }
}

#endif