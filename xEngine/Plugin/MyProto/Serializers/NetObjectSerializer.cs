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
    internal sealed class NetObjectSerializer : IProtoSerializer
    {
        private readonly int _key;
        private readonly BclHelpers.NetObjectOptions _options;
        private readonly Type _type;

        public NetObjectSerializer(TypeModel model, Type type, int key, BclHelpers.NetObjectOptions options)
        {
            var dynamicType = (options & BclHelpers.NetObjectOptions.DynamicType) != 0;
            _key = dynamicType ? -1 : key;
            _type = dynamicType ? model.MapType(typeof (object)) : type;
            _options = options;
        }

        public Type ExpectedType
        {
            get { return _type; }
        }

        public bool ReturnsValue
        {
            get { return true; }
        }

        public bool RequiresOldValue
        {
            get { return true; }
        }

#if !FEAT_IKVM
        public object Read(object value, ProtoReader source)
        {
            return BclHelpers.ReadNetObject(value, source, _key, _type == typeof (object) ? null : _type, _options);
        }

        public void Write(object value, ProtoWriter dest)
        {
            BclHelpers.WriteNetObject(value, dest, _key, _options);
        }
#endif
#if FEAT_COMPILER
        public void EmitRead(CompilerContext ctx, Local valueFrom)
        {
            ctx.LoadValue(valueFrom);
            ctx.CastToObject(_type);
            ctx.LoadReaderWriter();
            ctx.LoadValue(ctx.MapMetaKeyToCompiledKey(_key));
            if (_type == ctx.MapType(typeof (object))) ctx.LoadNullRef();
            else ctx.LoadValue(_type);
            ctx.LoadValue((int) _options);
            ctx.EmitCall(ctx.MapType(typeof (BclHelpers)).GetMethod("ReadNetObject"));
            ctx.CastFromObject(_type);
        }

        public void EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            ctx.LoadValue(valueFrom);
            ctx.CastToObject(_type);
            ctx.LoadReaderWriter();
            ctx.LoadValue(ctx.MapMetaKeyToCompiledKey(_key));
            ctx.LoadValue((int) _options);
            ctx.EmitCall(ctx.MapType(typeof (BclHelpers)).GetMethod("WriteNetObject"));
        }
#endif
    }
}

#endif