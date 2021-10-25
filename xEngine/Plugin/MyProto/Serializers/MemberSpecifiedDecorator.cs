using MyProto.Compiler;
using xEngine.Plugin.MyProto.Compiler;
#if !NO_RUNTIME
using System;
#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#else
using System.Reflection;

#endif

namespace MyProto.Serializers
{
    internal sealed class MemberSpecifiedDecorator : ProtoDecoratorBase
    {
        private readonly MethodInfo _getSpecified, _setSpecified;

        public MemberSpecifiedDecorator(MethodInfo getSpecified, MethodInfo setSpecified, IProtoSerializer tail)
            : base(tail)
        {
            if (getSpecified == null && setSpecified == null) throw new InvalidOperationException();
            _getSpecified = getSpecified;
            _setSpecified = setSpecified;
        }

        public override Type ExpectedType
        {
            get { return Tail.ExpectedType; }
        }

        public override bool RequiresOldValue
        {
            get { return Tail.RequiresOldValue; }
        }

        public override bool ReturnsValue
        {
            get { return Tail.ReturnsValue; }
        }

#if !FEAT_IKVM
        public override void Write(object value, ProtoWriter dest)
        {
            if (_getSpecified == null || (bool) _getSpecified.Invoke(value, null))
            {
                Tail.Write(value, dest);
            }
        }

        public override object Read(object value, ProtoReader source)
        {
            var result = Tail.Read(value, source);
            if (_setSpecified != null) _setSpecified.Invoke(value, new object[] {true});
            return result;
        }
#endif
#if FEAT_COMPILER
        protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            if (_getSpecified == null)
            {
                Tail.EmitWrite(ctx, valueFrom);
                return;
            }
            using (var loc = ctx.GetLocalWithValue(ExpectedType, valueFrom))
            {
                ctx.LoadAddress(loc, ExpectedType);
                ctx.EmitCall(_getSpecified);
                var done = ctx.DefineLabel();
                ctx.BranchIfFalse(done, false);
                Tail.EmitWrite(ctx, loc);
                ctx.MarkLabel(done);
            }
        }

        protected override void EmitRead(CompilerContext ctx, Local valueFrom)
        {
            if (_setSpecified == null)
            {
                Tail.EmitRead(ctx, valueFrom);
                return;
            }
            using (var loc = ctx.GetLocalWithValue(ExpectedType, valueFrom))
            {
                Tail.EmitRead(ctx, loc);
                ctx.LoadAddress(loc, ExpectedType);
                ctx.LoadValue(1); // true
                ctx.EmitCall(_setSpecified);
            }
        }
#endif
    }
}

#endif