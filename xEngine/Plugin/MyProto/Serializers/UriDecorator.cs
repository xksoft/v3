﻿using MyProto.Compiler;
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
    internal sealed class UriDecorator : ProtoDecoratorBase
    {
#if FEAT_IKVM
        readonly Type expectedType;
#else
        private static readonly Type expectedType = typeof (Uri);
#endif

        public UriDecorator(TypeModel model, IProtoSerializer tail) : base(tail)
        {
#if FEAT_IKVM
            expectedType = model.MapType(typeof(Uri));
#endif
        }

        public override Type ExpectedType
        {
            get { return expectedType; }
        }

        public override bool RequiresOldValue
        {
            get { return false; }
        }

        public override bool ReturnsValue
        {
            get { return true; }
        }


#if !FEAT_IKVM
        public override void Write(object value, ProtoWriter dest)
        {
            Tail.Write(((Uri) value).AbsoluteUri, dest);
        }

        public override object Read(object value, ProtoReader source)
        {
            Helpers.DebugAssert(value == null); // not expecting incoming
            var s = (string) Tail.Read(null, source);
            return s.Length == 0 ? null : new Uri(s);
        }
#endif

#if FEAT_COMPILER
        protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            ctx.LoadValue(valueFrom);
            ctx.LoadValue(typeof (Uri).GetProperty("AbsoluteUri"));
            Tail.EmitWrite(ctx, null);
        }

        protected override void EmitRead(CompilerContext ctx, Local valueFrom)
        {
            Tail.EmitRead(ctx, valueFrom);
            ctx.CopyValue();
            CodeLabel @nonEmpty = ctx.DefineLabel(), @end = ctx.DefineLabel();
            ctx.LoadValue(typeof (string).GetProperty("Length"));
            ctx.BranchIfTrue(@nonEmpty, true);
            ctx.DiscardValue();
            ctx.LoadNullRef();
            ctx.Branch(@end, true);
            ctx.MarkLabel(@nonEmpty);
            ctx.EmitCtor(ctx.MapType(typeof (Uri)), ctx.MapType(typeof (string)));
            ctx.MarkLabel(@end);
        }
#endif
    }
}

#endif