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
    internal sealed class FieldDecorator : ProtoDecoratorBase
    {
        private readonly FieldInfo _field;
        private readonly Type _forType;

        public FieldDecorator(Type forType, FieldInfo field, IProtoSerializer tail) : base(tail)
        {
            Helpers.DebugAssert(forType != null);
            Helpers.DebugAssert(field != null);
            _forType = forType;
            _field = field;
        }

        public override Type ExpectedType
        {
            get { return _forType; }
        }

        public override bool RequiresOldValue
        {
            get { return true; }
        }

        public override bool ReturnsValue
        {
            get { return false; }
        }

#if !FEAT_IKVM
        public override void Write(object value, ProtoWriter dest)
        {
            Helpers.DebugAssert(value != null);
            value = _field.GetValue(value);
            if (value != null) Tail.Write(value, dest);
        }

        public override object Read(object value, ProtoReader source)
        {
            Helpers.DebugAssert(value != null);
            var newValue = Tail.Read((Tail.RequiresOldValue ? _field.GetValue(value) : null), source);
            if (newValue != null) _field.SetValue(value, newValue);
            return null;
        }
#endif
#if FEAT_COMPILER
        protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            ctx.LoadAddress(valueFrom, ExpectedType);
            ctx.LoadValue(_field);
            ctx.WriteNullCheckedTail(_field.FieldType, Tail, null);
        }

        protected override void EmitRead(CompilerContext ctx, Local valueFrom)
        {
            using (var loc = ctx.GetLocalWithValue(ExpectedType, valueFrom))
            {
                ctx.LoadAddress(loc, ExpectedType);
                if (Tail.RequiresOldValue)
                {
                    ctx.CopyValue();
                    ctx.LoadValue(_field);
                }
                // value is either now on the stack or not needed
                ctx.ReadNullCheckedTail(_field.FieldType, Tail, null);

                if (Tail.ReturnsValue)
                {
                    if (_field.FieldType.IsValueType)
                    {
                        // stack is now the return value
                        ctx.StoreValue(_field);
                    }
                    else
                    {
                        CodeLabel hasValue = ctx.DefineLabel(), allDone = ctx.DefineLabel();
                        ctx.CopyValue();
                        ctx.BranchIfTrue(hasValue, true); // interpret null as "don't assign"

                        // no value, discard
                        ctx.DiscardValue();
                        ctx.DiscardValue();
                        ctx.Branch(allDone, true);

                        ctx.MarkLabel(hasValue);
                        ctx.StoreValue(_field);
                        ctx.MarkLabel(allDone);
                    }
                }
                else
                {
                    ctx.DiscardValue();
                }
            }
        }
#endif
    }
}

#endif