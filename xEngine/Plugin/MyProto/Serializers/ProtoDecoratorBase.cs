using MyProto.Compiler;
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
    internal abstract class ProtoDecoratorBase : IProtoSerializer
    {
        protected readonly IProtoSerializer Tail;

        protected ProtoDecoratorBase(IProtoSerializer tail)
        {
            Tail = tail;
        }

        public abstract Type ExpectedType { get; }
        public abstract bool ReturnsValue { get; }
        public abstract bool RequiresOldValue { get; }
#if !FEAT_IKVM
        public abstract void Write(object value, ProtoWriter dest);
        public abstract object Read(object value, ProtoReader source);
#endif
#if FEAT_COMPILER
        void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            EmitWrite(ctx, valueFrom);
        }

        protected abstract void EmitWrite(CompilerContext ctx, Local valueFrom);

        void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
        {
            EmitRead(ctx, valueFrom);
        }

        protected abstract void EmitRead(CompilerContext ctx, Local valueFrom);
#endif
    }
}

#endif