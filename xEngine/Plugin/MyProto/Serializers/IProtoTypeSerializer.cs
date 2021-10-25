using MyProto.Compiler;
using xEngine.Plugin.MyProto.Compiler;
#if !NO_RUNTIME
using MyProto.Meta;

namespace MyProto.Serializers
{
    internal interface IProtoTypeSerializer : IProtoSerializer
    {
        bool HasCallbacks(TypeModel.CallbackType callbackType);
        bool CanCreateInstance();
#if FEAT_COMPILER
        void EmitCallback(CompilerContext ctx, Local valueFrom, TypeModel.CallbackType callbackType);
#endif
#if FEAT_COMPILER
        void EmitCreateInstance(CompilerContext ctx);
#endif
#if !FEAT_IKVM
        object CreateInstance(ProtoReader source);
        void Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context);
#endif
    }
}

#endif