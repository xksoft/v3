
#if FEAT_COMPILER

namespace MyProto.Compiler
{
    internal delegate void ProtoSerializer(object value, ProtoWriter dest);

    internal delegate object ProtoDeserializer(object value, ProtoReader source);
}

#endif