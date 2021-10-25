using System.Reflection.Emit;
using MyProto.Compiler;
using xEngine.Plugin.MyProto.Compiler;
#if !NO_RUNTIME
using System;
using MyProto.Meta;

#if FEAT_COMPILER
#if FEAT_IKVM
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;
#else

#endif
#endif

namespace MyProto.Serializers
{
    internal sealed class SubItemSerializer : IProtoTypeSerializer
    {
        private readonly int _key;
        private readonly ISerializerProxy _proxy;
        private readonly bool _recursionCheck;
        private readonly Type _type;

        public SubItemSerializer(Type type, int key, ISerializerProxy proxy, bool recursionCheck)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (proxy == null) throw new ArgumentNullException("proxy");
            _type = type;
            _proxy = proxy;
            _key = key;
            _recursionCheck = recursionCheck;
        }

        bool IProtoTypeSerializer.HasCallbacks(TypeModel.CallbackType callbackType)
        {
            return ((IProtoTypeSerializer) _proxy.Serializer).HasCallbacks(callbackType);
        }

        bool IProtoTypeSerializer.CanCreateInstance()
        {
            return ((IProtoTypeSerializer) _proxy.Serializer).CanCreateInstance();
        }

        Type IProtoSerializer.ExpectedType
        {
            get { return _type; }
        }

        bool IProtoSerializer.RequiresOldValue
        {
            get { return true; }
        }

        bool IProtoSerializer.ReturnsValue
        {
            get { return true; }
        }

#if FEAT_COMPILER
        void IProtoTypeSerializer.EmitCallback(CompilerContext ctx, Local valueFrom, TypeModel.CallbackType callbackType)
        {
            ((IProtoTypeSerializer) _proxy.Serializer).EmitCallback(ctx, valueFrom, callbackType);
        }

        void IProtoTypeSerializer.EmitCreateInstance(CompilerContext ctx)
        {
            ((IProtoTypeSerializer) _proxy.Serializer).EmitCreateInstance(ctx);
        }
#endif
#if !FEAT_IKVM
        void IProtoTypeSerializer.Callback(object value, TypeModel.CallbackType callbackType,
            SerializationContext context)
        {
            ((IProtoTypeSerializer) _proxy.Serializer).Callback(value, callbackType, context);
        }

        object IProtoTypeSerializer.CreateInstance(ProtoReader source)
        {
            return ((IProtoTypeSerializer) _proxy.Serializer).CreateInstance(source);
        }
#endif
#if !FEAT_IKVM
        void IProtoSerializer.Write(object value, ProtoWriter dest)
        {
            if (_recursionCheck)
            {
                ProtoWriter.WriteObject(value, _key, dest);
            }
            else
            {
                ProtoWriter.WriteRecursionSafeObject(value, _key, dest);
            }
        }

        object IProtoSerializer.Read(object value, ProtoReader source)
        {
            return ProtoReader.ReadObject(value, _key, source);
        }
#endif
#if FEAT_COMPILER
        private bool EmitDedicatedMethod(CompilerContext ctx, Local valueFrom, bool read)
        {
#if SILVERLIGHT
            return false;
#else
            var method = ctx.GetDedicatedMethod(_key, read);
            if (method == null) return false;

            using (var token = new Local(ctx, ctx.MapType(typeof (SubItemToken))))
            {
                var rwType = ctx.MapType(read ? typeof (ProtoReader) : typeof (ProtoWriter));
                ctx.LoadValue(valueFrom);
                if (!read) // write requires the object for StartSubItem; read doesn't
                {
                    // (if recursion-check is disabled [subtypes] then null is fine too)
                    if (_type.IsValueType || !_recursionCheck)
                    {
                        ctx.LoadNullRef();
                    }
                    else
                    {
                        ctx.CopyValue();
                    }
                }
                ctx.LoadReaderWriter();
                ctx.EmitCall(rwType.GetMethod("StartSubItem"));
                ctx.StoreValue(token);

                // note: value already on the stack
                ctx.LoadReaderWriter();
                ctx.EmitCall(method);
                // handle inheritance (we will be calling the *base* version of things,
                // but we expect Read to return the "type" type)
                if (read && _type != method.ReturnType) ctx.Cast(_type);
                ctx.LoadValue(token);

                ctx.LoadReaderWriter();
                ctx.EmitCall(rwType.GetMethod("EndSubItem"));
            }
            return true;
#endif
        }

        void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            if (!EmitDedicatedMethod(ctx, valueFrom, false))
            {
                ctx.LoadValue(valueFrom);
                if (_type.IsValueType) ctx.CastToObject(_type);
                ctx.LoadValue(ctx.MapMetaKeyToCompiledKey(_key));
                // re-map for formality, but would expect identical, else dedicated method
                ctx.LoadReaderWriter();
                ctx.EmitCall(
                    ctx.MapType(typeof (ProtoWriter))
                        .GetMethod(_recursionCheck ? "WriteObject" : "WriteRecursionSafeObject"));
            }
        }

        void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
        {
            if (!EmitDedicatedMethod(ctx, valueFrom, true))
            {
                ctx.LoadValue(valueFrom);
                if (_type.IsValueType) ctx.CastToObject(_type);
                ctx.LoadValue(ctx.MapMetaKeyToCompiledKey(_key));
                // re-map for formality, but would expect identical, else dedicated method
                ctx.LoadReaderWriter();
                ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("ReadObject"));
                ctx.CastFromObject(_type);
            }
        }
#endif
    }
}

#endif