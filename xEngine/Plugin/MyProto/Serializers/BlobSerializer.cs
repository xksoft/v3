using MyProto.Compiler;
using MyProto.Meta;
using xEngine.Plugin.MyProto.Compiler;
#if !NO_RUNTIME
using System;

#if FEAT_COMPILER

#endif

#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
#endif

namespace MyProto.Serializers
{
    internal sealed class BlobSerializer : IProtoSerializer
    {
        public Type ExpectedType
        {
            get { return expectedType; }
        }

#if FEAT_IKVM
        readonly Type expectedType;
#else
        private static readonly Type expectedType = typeof (byte[]);
#endif

        public BlobSerializer(TypeModel model, bool overwriteList)
        {
#if FEAT_IKVM
            expectedType = model.MapType(typeof(byte[]));
#endif
            _overwriteList = overwriteList;
        }

        private readonly bool _overwriteList;
#if !FEAT_IKVM
        public object Read(object value, ProtoReader source)
        {
            return ProtoReader.AppendBytes(_overwriteList ? null : (byte[]) value, source);
        }

        public void Write(object value, ProtoWriter dest)
        {
            ProtoWriter.WriteBytes((byte[]) value, dest);
        }
#endif

        bool IProtoSerializer.RequiresOldValue
        {
            get { return !_overwriteList; }
        }

        bool IProtoSerializer.ReturnsValue
        {
            get { return true; }
        }

#if FEAT_COMPILER
        void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitBasicWrite("WriteBytes", valueFrom);
        }

        void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
        {
            if (_overwriteList)
            {
                ctx.LoadNullRef();
            }
            else
            {
                ctx.LoadValue(valueFrom);
            }
            ctx.LoadReaderWriter();
            ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("AppendBytes"));
        }
#endif
    }
}

#endif