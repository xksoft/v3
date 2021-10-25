
#if !NO_RUNTIME

namespace MyProto.Serializers
{
    internal interface ISerializerProxy
    {
        IProtoSerializer Serializer { get; }
    }
}

#endif