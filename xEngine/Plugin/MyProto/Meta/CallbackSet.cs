﻿using System.Runtime.Serialization;
#if !NO_RUNTIME
using System;
#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#else
using System.Reflection;

#endif

namespace MyProto.Meta
{
    /// <summary>
    ///     Represents the set of serialization callbacks to be used when serializing/deserializing a type.
    /// </summary>
    public class CallbackSet
    {
        private readonly MetaType _metaType;
        private MethodInfo _afterDeserialize;
        private MethodInfo _afterSerialize, _beforeDeserialize;
        private MethodInfo _beforeSerialize;

        internal CallbackSet(MetaType metaType)
        {
            if (metaType == null) throw new ArgumentNullException("metaType");
            _metaType = metaType;
        }

        internal MethodInfo this[TypeModel.CallbackType callbackType]
        {
            get
            {
                switch (callbackType)
                {
                    case TypeModel.CallbackType.BeforeSerialize:
                        return _beforeSerialize;
                    case TypeModel.CallbackType.AfterSerialize:
                        return _afterSerialize;
                    case TypeModel.CallbackType.BeforeDeserialize:
                        return _beforeDeserialize;
                    case TypeModel.CallbackType.AfterDeserialize:
                        return _afterDeserialize;
                    default:
                        throw new ArgumentException("Callback type not supported: " + callbackType, "callbackType");
                }
            }
        }

        /// <summary>Called before serializing an instance</summary>
        public MethodInfo BeforeSerialize
        {
            get { return _beforeSerialize; }
            set { _beforeSerialize = SanityCheckCallback(_metaType.Model, value); }
        }

        /// <summary>Called before deserializing an instance</summary>
        public MethodInfo BeforeDeserialize
        {
            get { return _beforeDeserialize; }
            set { _beforeDeserialize = SanityCheckCallback(_metaType.Model, value); }
        }

        /// <summary>Called after serializing an instance</summary>
        public MethodInfo AfterSerialize
        {
            get { return _afterSerialize; }
            set { _afterSerialize = SanityCheckCallback(_metaType.Model, value); }
        }

        /// <summary>Called after deserializing an instance</summary>
        public MethodInfo AfterDeserialize
        {
            get { return _afterDeserialize; }
            set { _afterDeserialize = SanityCheckCallback(_metaType.Model, value); }
        }

        /// <summary>
        ///     True if any callback is set, else False
        /// </summary>
        public bool NonTrivial
        {
            get
            {
                return _beforeSerialize != null || _beforeDeserialize != null
                       || _afterSerialize != null || _afterDeserialize != null;
            }
        }

        internal static bool CheckCallbackParameters(TypeModel model, MethodInfo method)
        {
            var args = method.GetParameters();
            for (var i = 0; i < args.Length; i++)
            {
                var paramType = args[i].ParameterType;
                if (paramType == model.MapType(typeof (SerializationContext)))
                {
                }
                else if (paramType == model.MapType(typeof (Type)))
                {
                }
#if PLAT_BINARYFORMATTER
                else if (paramType == model.MapType(typeof (StreamingContext)))
                {
                }
#endif
                else return false;
            }
            return true;
        }

        private MethodInfo SanityCheckCallback(TypeModel model, MethodInfo callback)
        {
            _metaType.ThrowIfFrozen();
            if (callback == null) return callback; // fine
            if (callback.IsStatic) throw new ArgumentException("Callbacks cannot be static", "callback");
            if (callback.ReturnType != model.MapType(typeof (void))
                || !CheckCallbackParameters(model, callback))
            {
                throw CreateInvalidCallbackSignature(callback);
            }
            return callback;
        }

        internal static Exception CreateInvalidCallbackSignature(MethodInfo method)
        {
            return
                new NotSupportedException("Invalid callback signature in " + method.DeclaringType.FullName + "." +
                                          method.Name);
        }
    }
}

#endif