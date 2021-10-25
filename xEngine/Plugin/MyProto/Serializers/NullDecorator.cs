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
    internal sealed class NullDecorator : ProtoDecoratorBase
    {
        public const int Tag = 1;
        private readonly Type _expectedType;

        public NullDecorator(TypeModel model, IProtoSerializer tail) : base(tail)
        {
            if (!tail.ReturnsValue)
                throw new NotSupportedException("NullDecorator only supports implementations that return values");

            var tailType = tail.ExpectedType;
            if (Helpers.IsValueType(tailType))
            {
#if NO_GENERICS
                throw new NotSupportedException("NullDecorator cannot be used with a struct without generics support");
#else
                _expectedType = model.MapType(typeof (Nullable<>)).MakeGenericType(tailType);
#endif
            }
            else
            {
                _expectedType = tailType;
            }
        }

        public override Type ExpectedType
        {
            get { return _expectedType; }
        }

        public override bool ReturnsValue
        {
            get { return true; }
        }

        public override bool RequiresOldValue
        {
            get { return true; }
        }

#if FEAT_COMPILER
        protected override void EmitRead(CompilerContext ctx, Local valueFrom)
        {
            using (var oldValue = ctx.GetLocalWithValue(_expectedType, valueFrom))
            using (var token = new Local(ctx, ctx.MapType(typeof (SubItemToken))))
            using (var field = new Local(ctx, ctx.MapType(typeof (int))))
            {
                ctx.LoadReaderWriter();
                ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("StartSubItem"));
                ctx.StoreValue(token);

                CodeLabel next = ctx.DefineLabel(), processField = ctx.DefineLabel(), end = ctx.DefineLabel();

                ctx.MarkLabel(next);

                ctx.EmitBasicRead("ReadFieldHeader", ctx.MapType(typeof (int)));
                ctx.CopyValue();
                ctx.StoreValue(field);
                ctx.LoadValue(Tag); // = 1 - process
                ctx.BranchIfEqual(processField, true);
                ctx.LoadValue(field);
                ctx.LoadValue(1); // < 1 - exit
                ctx.BranchIfLess(end, false);

                // default: skip
                ctx.LoadReaderWriter();
                ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("SkipField"));
                ctx.Branch(next, true);

                // process
                ctx.MarkLabel(processField);
                if (Tail.RequiresOldValue)
                {
                    if (_expectedType.IsValueType)
                    {
                        ctx.LoadAddress(oldValue, _expectedType);
                        ctx.EmitCall(_expectedType.GetMethod("GetValueOrDefault", Helpers.EmptyTypes));
                    }
                    else
                    {
                        ctx.LoadValue(oldValue);
                    }
                }
                Tail.EmitRead(ctx, null);
                // note we demanded always returns a value
                if (_expectedType.IsValueType)
                {
                    ctx.EmitCtor(_expectedType, Tail.ExpectedType); // re-nullable<T> it
                }
                ctx.StoreValue(oldValue);
                ctx.Branch(next, false);

                // outro
                ctx.MarkLabel(end);

                ctx.LoadValue(token);
                ctx.LoadReaderWriter();
                ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("EndSubItem"));
                ctx.LoadValue(oldValue); // load the old value
            }
        }

        protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            using (var valOrNull = ctx.GetLocalWithValue(_expectedType, valueFrom))
            using (var token = new Local(ctx, ctx.MapType(typeof (SubItemToken))))
            {
                ctx.LoadNullRef();
                ctx.LoadReaderWriter();
                ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("StartSubItem"));
                ctx.StoreValue(token);

                if (_expectedType.IsValueType)
                {
                    ctx.LoadAddress(valOrNull, _expectedType);
                    ctx.LoadValue(_expectedType.GetProperty("HasValue"));
                }
                else
                {
                    ctx.LoadValue(valOrNull);
                }
                var @end = ctx.DefineLabel();
                ctx.BranchIfFalse(@end, false);
                if (_expectedType.IsValueType)
                {
                    ctx.LoadAddress(valOrNull, _expectedType);
                    ctx.EmitCall(_expectedType.GetMethod("GetValueOrDefault", Helpers.EmptyTypes));
                }
                else
                {
                    ctx.LoadValue(valOrNull);
                }
                Tail.EmitWrite(ctx, null);

                ctx.MarkLabel(@end);

                ctx.LoadValue(token);
                ctx.LoadReaderWriter();
                ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("EndSubItem"));
            }
        }
#endif
#if !FEAT_IKVM
        public override object Read(object value, ProtoReader source)
        {
            var tok = ProtoReader.StartSubItem(source);
            int field;
            while ((field = source.ReadFieldHeader()) > 0)
            {
                if (field == Tag)
                {
                    value = Tail.Read(value, source);
                }
                else
                {
                    source.SkipField();
                }
            }
            ProtoReader.EndSubItem(tok, source);
            return value;
        }

        public override void Write(object value, ProtoWriter dest)
        {
            var token = ProtoWriter.StartSubItem(null, dest);
            if (value != null)
            {
                Tail.Write(value, dest);
            }
            ProtoWriter.EndSubItem(token, dest);
        }
#endif
    }
}

#endif