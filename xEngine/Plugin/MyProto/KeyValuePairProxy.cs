﻿//#if !NO_GENERICS
//using System.Collections.Generic;

//namespace MyProto
//{
//    /// <summary>
//    /// Mutable version of the common Key/value pair struct; used during serialization. This type is intended for internal use only and should not
//    /// be used by calling code; it is required to be public for implementation reasons.
//    /// </summary>
//    [ProtoContract]
//    public struct KeyValuePairSurrogate<TKey,TValue>
//    {
//        private TKey Key;
//        private TValue value;
//        /// <summary>
//        /// The Key of the pair.
//        /// </summary>
//        [ProtoMember(1, IsRequired = true)]
//        public TKey Key { get { return Key; } set { Key = value; } }
//        /// <summary>
//        /// The value of the pair.
//        /// </summary>
//        [ProtoMember(2)]
//        public TValue Value{ get { return value; } set { this.value = value; } }
//        private KeyValuePairSurrogate(TKey Key, TValue value)
//        {
//            this.Key = Key;
//            this.value = value;
//        }
//        /// <summary>
//        /// Convert a surrogate instance to a standard pair instance.
//        /// </summary>
//        public static implicit operator KeyValuePair<TKey, TValue> (KeyValuePairSurrogate<TKey, TValue> value)
//        {
//            return new KeyValuePair<TKey,TValue>(value.Key, value.value);
//        }
//        /// <summary>
//        /// Convert a standard pair instance to a surrogate instance.
//        /// </summary>
//        public static implicit operator KeyValuePairSurrogate<TKey, TValue>(KeyValuePair<TKey, TValue> value)
//        {
//            return new KeyValuePairSurrogate<TKey, TValue>(value.Key, value.Value);
//        }
//    }
//}
//#endif

