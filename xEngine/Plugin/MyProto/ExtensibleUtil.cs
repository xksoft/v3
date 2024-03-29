﻿using System;
using System.Collections;
using MyProto.Meta;
#if !NO_GENERICS
using System.Collections.Generic;

#endif

namespace MyProto
{
    /// <summary>
    ///     This class acts as an internal wrapper allowing us to do a dynamic
    ///     methodinfo invoke; an't put into Serializer as don't want on public
    ///     API; can't put into Serializer&lt;T&gt; since we need to invoke
    ///     accross classes, which isn't allowed in Silverlight)
    /// </summary>
    internal
#if FX11
    sealed
#else
        static
#endif
        class ExtensibleUtil
    {
#if FX11
        private ExtensibleUtil() { } // not a static class for C# 1.2 reasons
#endif

#if !NO_RUNTIME && !NO_GENERICS
        /// <summary>
        ///     All this does is call GetExtendedValuesTyped with the correct type for "instance";
        ///     this ensures that we don't get issues with subclasses declaring conflicting types -
        ///     the caller must respect the fields defined for the type they pass in.
        /// </summary>
        internal static IEnumerable<TValue> GetExtendedValues<TValue>(IExtensible instance, int tag, DataFormat format,
            bool singleton, bool allowDefinedTag)
        {
            foreach (
                TValue value in
                    GetExtendedValues(RuntimeTypeModel.Default, typeof (TValue), instance, tag, format, singleton,
                        allowDefinedTag))
            {
                yield return value;
            }
        }
#endif

        /// <summary>
        ///     All this does is call GetExtendedValuesTyped with the correct type for "instance";
        ///     this ensures that we don't get issues with subclasses declaring conflicting types -
        ///     the caller must respect the fields defined for the type they pass in.
        /// </summary>
        internal static IEnumerable GetExtendedValues(TypeModel model, Type type, IExtensible instance, int tag,
            DataFormat format, bool singleton, bool allowDefinedTag)
        {
#if FEAT_IKVM
            throw new NotSupportedException();
#else

            if (instance == null) throw new ArgumentNullException("instance");
            if (tag <= 0) throw new ArgumentOutOfRangeException("tag");
            var extn = instance.GetExtensionObject(false);

            if (extn == null)
            {
#if FX11
                return new object[0];
#else
                yield break;
#endif
            }

#if FX11
            BasicList result = new BasicList();
#endif
            var stream = extn.BeginQuery();
            object value = null;
            try
            {
                var ctx = new SerializationContext();
                using (var reader = new ProtoReader(stream, model, ctx))
                {
                    while (
                        model.TryDeserializeAuxiliaryType(reader, format, tag, type, ref value, true, false, false,
                            false) && value != null)
                    {
                        if (!singleton)
                        {
#if FX11
                            result.Add(value);
#else
                            yield return value;
#endif
                            value = null; // fresh item each time
                        }
                    }
                }
                if (singleton && value != null)
                {
#if FX11
                    result.Add(value);
#else
                    yield return value;
#endif
                }
#if FX11
                object[] resultArr = new object[result.Count];
                result.CopyTo(resultArr, 0);
                return resultArr;
#endif
            }
            finally
            {
                extn.EndQuery(stream);
            }
#endif
        }

        internal static void AppendExtendValue(TypeModel model, IExtensible instance, int tag, DataFormat format,
            object value)
        {
#if FEAT_IKVM
            throw new NotSupportedException();
#else
            if (instance == null) throw new ArgumentNullException("instance");
            if (value == null) throw new ArgumentNullException("value");

            // TODO
            //model.CheckTagNotInUse(tag);

            // obtain the extension object and prepare to write
            var extn = instance.GetExtensionObject(true);
            if (extn == null)
                throw new InvalidOperationException("No extension object available; appended data would be lost.");
            var commit = false;
            var stream = extn.BeginAppend();
            try
            {
                using (var writer = new ProtoWriter(stream, model, null))
                {
                    model.TrySerializeAuxiliaryType(writer, null, format, tag, value, false);
                    writer.Close();
                }
                commit = true;
            }
            finally
            {
                extn.EndAppend(stream, commit);
            }
#endif
        }

//#if !NO_GENERICS
//        /// <summary>
//        /// Stores the given value into the instance's stream; the serializer
//        /// is inferred from TValue and format.
//        /// </summary>
//        /// <remarks>Needs to be public to be callable thru reflection in Silverlight</remarks>
//        public static void AppendExtendValueTyped<TSource, TValue>(
//            TypeModel model, TSource instance, int tag, DataFormat format, TValue value)
//            where TSource : class, IExtensible
//        {
//            AppendExtendValue(model, instance, tag, format, value);
//        }
//#endif
    }
}