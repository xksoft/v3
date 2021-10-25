﻿#if !NO_RUNTIME
using System;
#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#else
using System.Reflection;

#endif

namespace MyProto.Meta
{
    internal abstract class AttributeMap
    {
#if DEBUG
        [Obsolete("Please use AttributeType instead")]
        public new Type GetType()
        {
            return AttributeType;
        }
#endif
        public abstract bool TryGet(string key, bool publicOnly, out object value);

        public bool TryGet(string key, out object value)
        {
            return TryGet(key, true, out value);
        }

        public abstract Type AttributeType { get; }

        public static AttributeMap[] Create(TypeModel model, Type type, bool inherit)
        {
#if FEAT_IKVM
            Type attribType = model.MapType(typeof(System.Attribute));
            System.Collections.Generic.IList<CustomAttributeData> all = type.__GetCustomAttributes(attribType, inherit);
            AttributeMap[] result = new AttributeMap[all.Count];
            int index = 0;
            foreach (CustomAttributeData attrib in all)
            {
                result[index++] = new AttributeDataMap(attrib);
            }
            return result;
#else
#if WINRT
            Attribute[] all = System.Linq.Enumerable.ToArray(type.GetTypeInfo().GetCustomAttributes(inherit));
#else
            var all = type.GetCustomAttributes(inherit);
#endif
            var result = new AttributeMap[all.Length];
            for (var i = 0; i < all.Length; i++)
            {
                result[i] = new ReflectionAttributeMap((Attribute) all[i]);
            }
            return result;
#endif
        }

        public static AttributeMap[] Create(TypeModel model, MemberInfo member, bool inherit)
        {
#if FEAT_IKVM
            System.Collections.Generic.IList<CustomAttributeData> all = member.__GetCustomAttributes(model.MapType(typeof(Attribute)), inherit);
            AttributeMap[] result = new AttributeMap[all.Count];
            int index = 0;
            foreach (CustomAttributeData attrib in all)
            {
                result[index++] = new AttributeDataMap(attrib);
            }
            return result;
#else
#if WINRT
            Attribute[] all = System.Linq.Enumerable.ToArray(member.GetCustomAttributes(inherit));
#else
            var all = member.GetCustomAttributes(inherit);
#endif
            var result = new AttributeMap[all.Length];
            for (var i = 0; i < all.Length; i++)
            {
                result[i] = new ReflectionAttributeMap((Attribute) all[i]);
            }
            return result;
#endif
        }

        public static AttributeMap[] Create(TypeModel model, Assembly assembly)
        {
#if FEAT_IKVM
            const bool inherit = false;
            System.Collections.Generic.IList<CustomAttributeData> all = assembly.__GetCustomAttributes(model.MapType(typeof(Attribute)), inherit);
            AttributeMap[] result = new AttributeMap[all.Count];
            int index = 0;
            foreach (CustomAttributeData attrib in all)
            {
                result[index++] = new AttributeDataMap(attrib);
            }
            return result;
#else
#if WINRT
            Attribute[] all = System.Linq.Enumerable.ToArray(assembly.GetCustomAttributes());
#else
            const bool inherit = false;
            var all = assembly.GetCustomAttributes(inherit);
#endif
            var result = new AttributeMap[all.Length];
            for (var i = 0; i < all.Length; i++)
            {
                result[i] = new ReflectionAttributeMap((Attribute) all[i]);
            }
            return result;
#endif
        }

#if FEAT_IKVM
        private sealed class AttributeDataMap : AttributeMap
        {
            public override Type AttributeType
            {
                get { return attribute.Constructor.DeclaringType; }
            }
            private readonly CustomAttributeData attribute;
            public AttributeDataMap(CustomAttributeData attribute)
            {
                this.attribute = attribute;
            }
            public override bool TryGet(string key, bool publicOnly, out object value)
            {
                foreach (CustomAttributeNamedArgument arg in attribute.NamedArguments)
                {
                    if (string.Equals(arg.MemberInfo.Name, key, StringComparison.OrdinalIgnoreCase))
                    {
                        value = arg.TypedValue.Value;
                        return true;
                    }
                }

                    
                int index = 0;
                ParameterInfo[] parameters = attribute.Constructor.GetParameters();
                foreach (CustomAttributeTypedArgument arg in attribute.ConstructorArguments)
                {
                    if (string.Equals(parameters[index++].Name, key, StringComparison.OrdinalIgnoreCase))
                    {
                        value = arg.Value;
                        return true;
                    }
                }
                value = null;
                return false;
            }
        }
#else
        public abstract object Target { get; }

        private sealed class ReflectionAttributeMap : AttributeMap
        {
            private readonly Attribute _attribute;

            public ReflectionAttributeMap(Attribute attribute)
            {
                _attribute = attribute;
            }

            public override object Target
            {
                get { return _attribute; }
            }

            public override Type AttributeType
            {
                get { return _attribute.GetType(); }
            }

            public override bool TryGet(string key, bool publicOnly, out object value)
            {
                var members = Helpers.GetInstanceFieldsAndProperties(_attribute.GetType(), publicOnly);
                foreach (var member in members)
                {
#if FX11
                    if (member.Name.ToUpper() == key.ToUpper())
#else
                    if (string.Equals(member.Name, key, StringComparison.OrdinalIgnoreCase))
#endif
                    {
                        var prop = member as PropertyInfo;
                        if (prop != null)
                        {
                            value = prop.GetValue(_attribute, null);
                            return true;
                        }
                        var field = member as FieldInfo;
                        if (field != null)
                        {
                            value = field.GetValue(_attribute);
                            return true;
                        }

                        throw new NotSupportedException(member.GetType().Name);
                    }
                }
                value = null;
                return false;
            }
        }
#endif
    }
}

#endif