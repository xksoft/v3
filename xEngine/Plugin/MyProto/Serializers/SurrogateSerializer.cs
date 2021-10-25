using MyProto.Compiler;
using xEngine.Plugin.MyProto.Compiler;
#if !NO_RUNTIME
using System;
using MyProto.Meta;
#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#else
using System.Reflection;

#endif

namespace MyProto.Serializers
{
    internal sealed class SurrogateSerializer : IProtoTypeSerializer
    {
        private readonly Type _forType, _declaredType;
        private readonly IProtoTypeSerializer _rootTail;
        private readonly MethodInfo _toTail, _fromTail;

        public SurrogateSerializer(Type forType, Type declaredType, IProtoTypeSerializer rootTail)
        {
            Helpers.DebugAssert(forType != null, "forType");
            Helpers.DebugAssert(declaredType != null, "declaredType");
            Helpers.DebugAssert(rootTail != null, "rootTail");
            Helpers.DebugAssert(rootTail.RequiresOldValue, "RequiresOldValue");
            Helpers.DebugAssert(!rootTail.ReturnsValue, "ReturnsValue");
            Helpers.DebugAssert(declaredType == rootTail.ExpectedType ||
                                Helpers.IsSubclassOf(declaredType, rootTail.ExpectedType));
            _forType = forType;
            _declaredType = declaredType;
            _rootTail = rootTail;
            _toTail = GetConversion(true);
            _fromTail = GetConversion(false);
        }

        bool IProtoTypeSerializer.HasCallbacks(TypeModel.CallbackType callbackType)
        {
            return false;
        }

        bool IProtoTypeSerializer.CanCreateInstance()
        {
            return false;
        }

        public bool ReturnsValue
        {
            get { return false; }
        }

        public bool RequiresOldValue
        {
            get { return true; }
        }

        public Type ExpectedType
        {
            get { return _forType; }
        }

        private static bool HasCast(Type type, Type from, Type to, out MethodInfo op)
        {
#if WINRT
            System.Collections.Generic.List<MethodInfo> list = new System.Collections.Generic.List<MethodInfo>();
            foreach (var item in type.GetRuntimeMethods())
            {
                if (item.IsStatic) list.Add(item);
            }
            MethodInfo[] found = list.ToArray();
#else
            const BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            var found = type.GetMethods(flags);
#endif
            for (var i = 0; i < found.Length; i++)
            {
                var m = found[i];
                if ((m.Name != "op_Implicit" && m.Name != "op_Explicit") || m.ReturnType != to)
                {
                    continue;
                }
                var paramTypes = m.GetParameters();
                if (paramTypes.Length == 1 && paramTypes[0].ParameterType == from)
                {
                    op = m;
                    return true;
                }
            }
            op = null;
            return false;
        }

        public MethodInfo GetConversion(bool toTail)
        {
            var to = toTail ? _declaredType : _forType;
            var from = toTail ? _forType : _declaredType;
            MethodInfo op;
            if (HasCast(_declaredType, from, to, out op) || HasCast(_forType, from, to, out op))
            {
                return op;
            }
            throw new InvalidOperationException("No suitable conversion operator found for surrogate: " +
                                                _forType.FullName + " / " + _declaredType.FullName);
        }

#if FEAT_COMPILER
        void IProtoTypeSerializer.EmitCallback(CompilerContext ctx, Local valueFrom, TypeModel.CallbackType callbackType)
        {
        }

        void IProtoTypeSerializer.EmitCreateInstance(CompilerContext ctx)
        {
            throw new NotSupportedException();
        }
#endif
#if !FEAT_IKVM
        object IProtoTypeSerializer.CreateInstance(ProtoReader source)
        {
            throw new NotSupportedException();
        }

        void IProtoTypeSerializer.Callback(object value, TypeModel.CallbackType callbackType,
            SerializationContext context)
        {
        }
#endif
#if !FEAT_IKVM
        public void Write(object value, ProtoWriter writer)
        {
            _rootTail.Write(_toTail.Invoke(null, new[] {value}), writer);
        }

        public object Read(object value, ProtoReader source)
        {
            // convert the incoming value
            object[] args = {value};
            value = _toTail.Invoke(null, args);

            // invoke the tail and convert the outgoing value
            args[0] = _rootTail.Read(value, source);
            return _fromTail.Invoke(null, args);
        }
#endif
#if FEAT_COMPILER
        void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
        {
            Helpers.DebugAssert(valueFrom != null); // don't support stack-head for this
            using (var converted = new Local(ctx, _declaredType)) // declare/re-use local
            {
                ctx.LoadValue(valueFrom); // load primary onto stack
                ctx.EmitCall(_toTail); // static convert op, primary-to-surrogate
                ctx.StoreValue(converted); // store into surrogate local

                _rootTail.EmitRead(ctx, converted); // downstream processing against surrogate local

                ctx.LoadValue(converted); // load from surrogate local
                ctx.EmitCall(_fromTail); // static convert op, surrogate-to-primary
                ctx.StoreValue(valueFrom); // store back into primary
            }
        }

        void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            ctx.LoadValue(valueFrom);
            ctx.EmitCall(_toTail);
            _rootTail.EmitWrite(ctx, null);
        }
#endif
    }
}

#endif