using MyProto.Compiler;
using xEngine.Plugin.MyProto.Compiler;
#if FEAT_COMPILER && !(FX11 || FEAT_IKVM)
using System;
using MyProto.Meta;


namespace MyProto.Serializers
{
    internal sealed class CompiledSerializer : IProtoTypeSerializer
    {
        private readonly ProtoDeserializer _deserializer;
        private readonly IProtoTypeSerializer _head;
        private readonly ProtoSerializer _serializer;

        private CompiledSerializer(IProtoTypeSerializer head, TypeModel model)
        {
            _head = head;
            _serializer = CompilerContext.BuildSerializer(head, model);
            _deserializer = CompilerContext.BuildDeserializer(head, model);
        }

        bool IProtoTypeSerializer.HasCallbacks(TypeModel.CallbackType callbackType)
        {
            return _head.HasCallbacks(callbackType); // these routes only used when bits of the model not compiled
        }

        bool IProtoTypeSerializer.CanCreateInstance()
        {
            return _head.CanCreateInstance();
        }

        object IProtoTypeSerializer.CreateInstance(ProtoReader source)
        {
            return _head.CreateInstance(source);
        }

        public void Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context)
        {
            _head.Callback(value, callbackType, context); // these routes only used when bits of the model not compiled
        }

        bool IProtoSerializer.RequiresOldValue
        {
            get { return _head.RequiresOldValue; }
        }

        bool IProtoSerializer.ReturnsValue
        {
            get { return _head.ReturnsValue; }
        }

        Type IProtoSerializer.ExpectedType
        {
            get { return _head.ExpectedType; }
        }

        void IProtoSerializer.Write(object value, ProtoWriter dest)
        {
            _serializer(value, dest);
        }

        object IProtoSerializer.Read(object value, ProtoReader source)
        {
            return _deserializer(value, source);
        }

        void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            _head.EmitWrite(ctx, valueFrom);
        }

        void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
        {
            _head.EmitRead(ctx, valueFrom);
        }

        void IProtoTypeSerializer.EmitCallback(CompilerContext ctx, Local valueFrom, TypeModel.CallbackType callbackType)
        {
            _head.EmitCallback(ctx, valueFrom, callbackType);
        }

        void IProtoTypeSerializer.EmitCreateInstance(CompilerContext ctx)
        {
            _head.EmitCreateInstance(ctx);
        }

        public static CompiledSerializer Wrap(IProtoTypeSerializer head, TypeModel model)
        {
            var result = head as CompiledSerializer;
            if (result == null)
            {
                result = new CompiledSerializer(head, model);
                Helpers.DebugAssert(((IProtoTypeSerializer) result).ExpectedType == head.ExpectedType);
            }
            return result;
        }
    }
}

#endif