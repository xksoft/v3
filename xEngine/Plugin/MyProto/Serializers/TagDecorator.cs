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
    internal sealed class TagDecorator : ProtoDecoratorBase, IProtoTypeSerializer
    {
        private readonly int _fieldNumber;
        private readonly bool _strict;
        private readonly WireType _wireType;

        public TagDecorator(int fieldNumber, WireType wireType, bool strict, IProtoSerializer tail)
            : base(tail)
        {
            _fieldNumber = fieldNumber;
            _wireType = wireType;
            _strict = strict;
        }

        private bool NeedsHint
        {
            get { return ((int) _wireType & ~7) != 0; }
        }

        public bool HasCallbacks(TypeModel.CallbackType callbackType)
        {
            var pts = Tail as IProtoTypeSerializer;
            return pts != null && pts.HasCallbacks(callbackType);
        }

        public bool CanCreateInstance()
        {
            var pts = Tail as IProtoTypeSerializer;
            return pts != null && pts.CanCreateInstance();
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
        public object CreateInstance(ProtoReader source)
        {
            return ((IProtoTypeSerializer) Tail).CreateInstance(source);
        }

        public void Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context)
        {
            var pts = Tail as IProtoTypeSerializer;
            if (pts != null) pts.Callback(value, callbackType, context);
        }
#endif
#if FEAT_COMPILER
        public void EmitCallback(CompilerContext ctx, Local valueFrom, TypeModel.CallbackType callbackType)
        {
            // we only expect this to be invoked if HasCallbacks returned true, so implicitly Tail
            // **must** be of the correct type
            ((IProtoTypeSerializer) Tail).EmitCallback(ctx, valueFrom, callbackType);
        }

        public void EmitCreateInstance(CompilerContext ctx)
        {
            ((IProtoTypeSerializer) Tail).EmitCreateInstance(ctx);
        }
#endif
#if !FEAT_IKVM
        public override object Read(object value, ProtoReader source)
        {
            Helpers.DebugAssert(_fieldNumber == source.FieldNumber);
            if (_strict)
            {
                source.Assert(_wireType);
            }
            else if (NeedsHint)
            {
                source.Hint(_wireType);
            }
            return Tail.Read(value, source);
        }

        public override void Write(object value, ProtoWriter dest)
        {
            ProtoWriter.WriteFieldHeader(_fieldNumber, _wireType, dest);
            Tail.Write(value, dest);
        }
#endif
#if FEAT_COMPILER
        protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            ctx.LoadValue(_fieldNumber);
            ctx.LoadValue((int) _wireType);
            ctx.LoadReaderWriter();
            ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("WriteFieldHeader"));
            Tail.EmitWrite(ctx, valueFrom);
        }

        protected override void EmitRead(CompilerContext ctx, Local valueFrom)
        {
            if (_strict || NeedsHint)
            {
                ctx.LoadReaderWriter();
                ctx.LoadValue((int) _wireType);
                ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod(_strict ? "Assert" : "Hint"));
            }
            Tail.EmitRead(ctx, valueFrom);
        }
#endif
    }
}

#endif