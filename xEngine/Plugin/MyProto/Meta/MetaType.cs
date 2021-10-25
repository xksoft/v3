﻿using System.Collections.Generic;
#if !NO_RUNTIME
using System;
using System.Collections;
using System.Text;
using MyProto.Serializers;
#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#if FEAT_COMPILER
using IKVM.Reflection.Emit;
#endif
#else
using System.Reflection;

#if FEAT_COMPILER

#endif
#endif

namespace MyProto.Meta
{
    /// <summary>
    ///     Represents a type at runtime for use with protobuf, allowing the field mappings (etc) to be defined
    /// </summary>
    public class MetaType : ISerializerProxy
    {
        internal sealed class Comparer : IComparer
#if !NO_GENERICS
            , IComparer<MetaType>
#endif
        {
            public static readonly Comparer Default = new Comparer();

            public int Compare(object x, object y)
            {
                return Compare(x as MetaType, y as MetaType);
            }

            public int Compare(MetaType x, MetaType y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (x == null) return -1;
                if (y == null) return 1;

#if FX11
                return string.Compare(x.GetSchemaTypeName(), y.GetSchemaTypeName());
#else
                return string.Compare(x.GetSchemaTypeName(), y.GetSchemaTypeName(), StringComparison.Ordinal);
#endif
            }
        }

        /// <summary>
        ///     Get the name of the type being represented
        /// </summary>
        public override string ToString()
        {
            return _type.ToString();
        }

        IProtoSerializer ISerializerProxy.Serializer
        {
            get { return Serializer; }
        }

        /// <summary>
        ///     Gets the base-type for this type
        /// </summary>
        public MetaType BaseType { get; private set; }

        internal TypeModel Model
        {
            get { return _model; }
        }

        /// <summary>
        ///     When used to compile a model, should public serialization/deserialzation methods
        ///     be included for this type?
        /// </summary>
        public bool IncludeSerializerMethod
        {
            // negated to minimize common-case / initializer
            get { return !HasFlag(OptionsPrivateOnApi); }
            set { SetFlag(OptionsPrivateOnApi, !value, true); }
        }

        /// <summary>
        ///     Should this type be treated as a reference by default?
        /// </summary>
        public bool AsReferenceDefault
        {
            get { return HasFlag(OptionsAsReferenceDefault); }
            set { SetFlag(OptionsAsReferenceDefault, value, true); }
        }

        private BasicList _subTypes;

        private bool IsValidSubType(Type subType)
        {
#if WINRT
            return typeInfo.IsAssignableFrom(subType.GetTypeInfo());
#else
            return _type.IsAssignableFrom(subType);
#endif
        }

        /// <summary>
        ///     Adds a known sub-type to the inheritance model
        /// </summary>
        public MetaType AddSubType(int fieldNumber, Type derivedType)
        {
            return AddSubType(fieldNumber, derivedType, DataFormat.Default);
        }

        /// <summary>
        ///     Adds a known sub-type to the inheritance model
        /// </summary>
        public MetaType AddSubType(int fieldNumber, Type derivedType, DataFormat dataFormat)
        {
            if (derivedType == null) throw new ArgumentNullException("derivedType");
            if (fieldNumber < 1) throw new ArgumentOutOfRangeException("fieldNumber");
#if WINRT
            if (!(typeInfo.IsClass || typeInfo.IsInterface) || typeInfo.IsSealed) {
#else
            if (!(_type.IsClass || _type.IsInterface) || _type.IsSealed)
            {
#endif
                throw new InvalidOperationException("Sub-types can only be added to non-sealed classes");
            }
            if (!IsValidSubType(derivedType))
            {
                throw new ArgumentException(derivedType.Name + " is not a valid sub-type of " + _type.Name,
                    "derivedType");
            }
            var derivedMeta = _model[derivedType];
            ThrowIfFrozen();
            derivedMeta.ThrowIfFrozen();
            var subType = new SubType(fieldNumber, derivedMeta, dataFormat);
            ThrowIfFrozen();

            derivedMeta.SetBaseType(this); // includes ThrowIfFrozen
            if (_subTypes == null) _subTypes = new BasicList();
            _subTypes.Add(subType);
            return this;
        }

#if WINRT
        internal static readonly TypeInfo ienumerable = typeof(IEnumerable).GetTypeInfo();
#else
        internal static readonly Type Ienumerable = typeof (IEnumerable);
#endif

        private void SetBaseType(MetaType baseType)
        {
            if (baseType == null) throw new ArgumentNullException("baseType");
            if (BaseType == baseType) return;
            if (BaseType != null)
                throw new InvalidOperationException("A type can only participate in one inheritance hierarchy");

            var type = baseType;
            while (type != null)
            {
                if (ReferenceEquals(type, this))
                    throw new InvalidOperationException("Cyclic inheritance is not allowed");
                type = type.BaseType;
            }
            BaseType = baseType;
        }

        private CallbackSet _callbacks;

        /// <summary>
        ///     Indicates whether the current type has defined callbacks
        /// </summary>
        public bool HasCallbacks
        {
            get { return _callbacks != null && _callbacks.NonTrivial; }
        }

        /// <summary>
        ///     Indicates whether the current type has defined subtypes
        /// </summary>
        public bool HasSubtypes
        {
            get { return _subTypes != null && _subTypes.Count != 0; }
        }

        /// <summary>
        ///     Returns the set of callbacks defined for this type
        /// </summary>
        public CallbackSet Callbacks
        {
            get
            {
                if (_callbacks == null) _callbacks = new CallbackSet(this);
                return _callbacks;
            }
        }

        private bool IsValueType
        {
            get
            {
#if WINRT
                return typeInfo.IsValueType;
#else
                return _type.IsValueType;
#endif
            }
        }

        /// <summary>
        ///     Assigns the callbacks to use during serialiation/deserialization.
        /// </summary>
        /// <param name="beforeSerialize">The method (or null) called before serialization begins.</param>
        /// <param name="afterSerialize">The method (or null) called when serialization is complete.</param>
        /// <param name="beforeDeserialize">
        ///     The method (or null) called before deserialization begins (or when a new instance is
        ///     created during deserialization).
        /// </param>
        /// <param name="afterDeserialize">The method (or null) called when deserialization is complete.</param>
        /// <returns>The set of callbacks.</returns>
        public MetaType SetCallbacks(MethodInfo beforeSerialize, MethodInfo afterSerialize, MethodInfo beforeDeserialize,
            MethodInfo afterDeserialize)
        {
            var callbacks = Callbacks;
            callbacks.BeforeSerialize = beforeSerialize;
            callbacks.AfterSerialize = afterSerialize;
            callbacks.BeforeDeserialize = beforeDeserialize;
            callbacks.AfterDeserialize = afterDeserialize;
            return this;
        }

        /// <summary>
        ///     Assigns the callbacks to use during serialiation/deserialization.
        /// </summary>
        /// <param name="beforeSerialize">The name of the method (or null) called before serialization begins.</param>
        /// <param name="afterSerialize">The name of the method (or null) called when serialization is complete.</param>
        /// <param name="beforeDeserialize">
        ///     The name of the method (or null) called before deserialization begins (or when a new
        ///     instance is created during deserialization).
        /// </param>
        /// <param name="afterDeserialize">The name of the method (or null) called when deserialization is complete.</param>
        /// <returns>The set of callbacks.</returns>
        public MetaType SetCallbacks(string beforeSerialize, string afterSerialize, string beforeDeserialize,
            string afterDeserialize)
        {
            if (IsValueType) throw new InvalidOperationException();
            var callbacks = Callbacks;
            callbacks.BeforeSerialize = ResolveMethod(beforeSerialize, true);
            callbacks.AfterSerialize = ResolveMethod(afterSerialize, true);
            callbacks.BeforeDeserialize = ResolveMethod(beforeDeserialize, true);
            callbacks.AfterDeserialize = ResolveMethod(afterDeserialize, true);
            return this;
        }

        internal string GetSchemaTypeName()
        {
            if (_surrogate != null) return _model[_surrogate].GetSchemaTypeName();

            if (!Helpers.IsNullOrEmpty(_name)) return _name;

            var typeName = _type.Name;
#if !NO_GENERICS
            if (_type
#if WINRT
                .GetTypeInfo()
#endif
                .IsGenericType)
            {
                var sb = new StringBuilder(typeName);
                var split = typeName.IndexOf('`');
                if (split >= 0) sb.Length = split;
                foreach (var arg in _type
#if WINRT
                    .GetTypeInfo().GenericTypeArguments
#else
                    .GetGenericArguments()
#endif
                    )
                {
                    sb.Append('_');
                    var tmp = arg;
                    var key = _model.GetKey(ref tmp);
                    MetaType mt;
                    if (key >= 0 && (mt = _model[tmp]) != null && mt._surrogate == null)
                        // <=== need to exclude surrogate to avoid chance of infinite loop
                    {
                        sb.Append(mt.GetSchemaTypeName());
                    }
                    else
                    {
                        sb.Append(tmp.Name);
                    }
                }
                return sb.ToString();
            }
#endif
            return typeName;
        }

        private string _name;

        /// <summary>
        ///     Gets or sets the name of this contract.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                ThrowIfFrozen();
                _name = value;
            }
        }

        private MethodInfo _factory;

        /// <summary>
        ///     Designate a factory-method to use to create instances of this type
        /// </summary>
        public MetaType SetFactory(MethodInfo factory)
        {
            _model.VerifyFactory(factory, _type);
            ThrowIfFrozen();
            _factory = factory;
            return this;
        }


        /// <summary>
        ///     Designate a factory-method to use to create instances of this type
        /// </summary>
        public MetaType SetFactory(string factory)
        {
            return SetFactory(ResolveMethod(factory, false));
        }

        private MethodInfo ResolveMethod(string name, bool instance)
        {
            if (Helpers.IsNullOrEmpty(name)) return null;
#if WINRT
            return instance ? Helpers.GetInstanceMethod(typeInfo, name) : Helpers.GetStaticMethod(typeInfo, name);
#else
            return instance ? Helpers.GetInstanceMethod(_type, name) : Helpers.GetStaticMethod(_type, name);
#endif
        }

        private readonly RuntimeTypeModel _model;

        internal static Exception InbuiltType(Type type)
        {
            return
                new ArgumentException(
                    "Data of this type has inbuilt behaviour, and cannot be added to a model in this way: " +
                    type.FullName);
        }

        internal MetaType(RuntimeTypeModel model, Type type, MethodInfo factory)
        {
            _factory = factory;
            if (model == null) throw new ArgumentNullException("model");
            if (type == null) throw new ArgumentNullException("type");

            var coreSerializer = model.TryGetBasicTypeSerializer(type);
            if (coreSerializer != null)
            {
                throw InbuiltType(type);
            }

            _type = type;
#if WINRT
            this.typeInfo = type.GetTypeInfo();
#endif
            _model = model;

            if (Helpers.IsEnum(type))
            {
#if WINRT
                EnumPassthru = typeInfo.IsDefined(typeof(FlagsAttribute), false);
#else
                EnumPassthru = type.IsDefined(model.MapType(typeof (FlagsAttribute)), false);
#endif
            }
        }

#if WINRT
        private readonly TypeInfo typeInfo;
#endif

        /// <summary>
        ///     Throws an exception if the type has been made immutable
        /// </summary>
        protected internal void ThrowIfFrozen()
        {
            if ((_flags & OptionsFrozen) != 0)
                throw new InvalidOperationException(
                    "The type cannot be changed once a serializer has been generated for " + _type.FullName);
        }

        //internal void Freeze() { flags |= OPTIONS_Frozen; }

        private readonly Type _type;

        /// <summary>
        ///     The runtime type that the meta-type represents
        /// </summary>
        public Type Type
        {
            get { return _type; }
        }

        private IProtoTypeSerializer _serializer;

        internal IProtoTypeSerializer Serializer
        {
            get
            {
                if (_serializer == null)
                {
                    var opaqueToken = 0;
                    try
                    {
                        _model.TakeLock(ref opaqueToken);
                        if (_serializer == null)
                        {
                            // double-check, but our main purpse with this lock is to ensure thread-safety with
                            // serializers needing to wait until another thread has finished adding the properties
                            SetFlag(OptionsFrozen, true, false);
                            _serializer = BuildSerializer();
#if FEAT_COMPILER && !FX11
                            if (_model.AutoCompile) CompileInPlace();
#endif
                        }
                    }
                    finally
                    {
                        _model.ReleaseLock(opaqueToken);
                    }
                }
                return _serializer;
            }
        }

        internal bool IsList
        {
            get
            {
                var itemType = IgnoreListHandling ? null : TypeModel.GetListItemType(_model, _type);
                return itemType != null;
            }
        }

        private IProtoTypeSerializer BuildSerializer()
        {
            if (Helpers.IsEnum(_type))
            {
                return new TagDecorator(MyProto.Serializer.ListItemTag, WireType.Variant, false,
                    new EnumSerializer(_type, GetEnumMap()));
            }
            var itemType = IgnoreListHandling ? null : TypeModel.GetListItemType(_model, _type);
            if (itemType != null)
            {
                if (_surrogate != null)
                {
                    throw new ArgumentException(
                        "Repeated data (a list, collection, etc) has inbuilt behaviour and cannot use a surrogate");
                }
                if (_subTypes != null && _subTypes.Count != 0)
                {
                    throw new ArgumentException(
                        "Repeated data (a list, collection, etc) has inbuilt behaviour and cannot be subclassed");
                }
                Type defaultType = null;
                ResolveListTypes(_model, _type, ref itemType, ref defaultType);
                var fakeMember = new ValueMember(_model, MyProto.Serializer.ListItemTag, _type, itemType, defaultType,
                    DataFormat.Default);
                return new TypeSerializer(_model, _type, new[] {MyProto.Serializer.ListItemTag},
                    new[] {fakeMember.Serializer}, null, true, true, null, _constructType, _factory);
            }
            if (_surrogate != null)
            {
                MetaType mt = _model[_surrogate], mtBase;
                while ((mtBase = mt.BaseType) != null)
                {
                    mt = mtBase;
                }
                return new SurrogateSerializer(_type, _surrogate, mt.Serializer);
            }
            if (IsAutoTuple)
            {
                MemberInfo[] mapping;
                var ctor = ResolveTupleConstructor(_type, out mapping);
                if (ctor == null) throw new InvalidOperationException();
                return new TupleSerializer(_model, ctor, mapping);
            }


            _fields.Trim();
            var fieldCount = _fields.Count;
            var subTypeCount = _subTypes == null ? 0 : _subTypes.Count;
            var fieldNumbers = new int[fieldCount + subTypeCount];
            var serializers = new IProtoSerializer[fieldCount + subTypeCount];
            var i = 0;
            if (subTypeCount != 0)
            {
                foreach (SubType subType in _subTypes)
                {
#if WINRT
                    if (!subType.DerivedType.IgnoreListHandling && ienumerable.IsAssignableFrom(subType.DerivedType.Type.GetTypeInfo()))
#else
                    if (!subType.DerivedType.IgnoreListHandling &&
                        _model.MapType(Ienumerable).IsAssignableFrom(subType.DerivedType.Type))
#endif
                    {
                        throw new ArgumentException(
                            "Repeated data (a list, collection, etc) has inbuilt behaviour and cannot be used as a subclass");
                    }
                    fieldNumbers[i] = subType.FieldNumber;
                    serializers[i++] = subType.Serializer;
                }
            }
            if (fieldCount != 0)
            {
                foreach (ValueMember member in _fields)
                {
                    fieldNumbers[i] = member.FieldNumber;
                    serializers[i++] = member.Serializer;
                }
            }

            BasicList baseCtorCallbacks = null;
            var tmp = BaseType;

            while (tmp != null)
            {
                var method = tmp.HasCallbacks ? tmp.Callbacks.BeforeDeserialize : null;
                if (method != null)
                {
                    if (baseCtorCallbacks == null) baseCtorCallbacks = new BasicList();
                    baseCtorCallbacks.Add(method);
                }
                tmp = tmp.BaseType;
            }
            MethodInfo[] arr = null;
            if (baseCtorCallbacks != null)
            {
                arr = new MethodInfo[baseCtorCallbacks.Count];
                baseCtorCallbacks.CopyTo(arr, 0);
                Array.Reverse(arr);
            }
            return new TypeSerializer(_model, _type, fieldNumbers, serializers, arr, BaseType == null, UseConstructor,
                _callbacks, _constructType, _factory);
        }

        [Flags]
        internal enum AttributeFamily
        {
            None = 0,
            ProtoBuf = 1,
            DataContractSerialier = 2,
            XmlSerializer = 4,
            AutoTuple = 8
        }

        private static Type GetBaseType(MetaType type)
        {
#if WINRT
            return type.typeInfo.BaseType;
#else
            return type._type.BaseType;
#endif
        }

        internal static bool GetAsReferenceDefault(RuntimeTypeModel model, Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (Helpers.IsEnum(type)) return false; // never as-ref
            var typeAttribs = AttributeMap.Create(model, type, false);
            for (var i = 0; i < typeAttribs.Length; i++)
            {
                if (typeAttribs[i].AttributeType.FullName == "MyProto.ProtoContractAttribute")
                {
                    object tmp;
                    if (typeAttribs[i].TryGet("AsReferenceDefault", out tmp)) return (bool) tmp;
                }
            }
            return false;
        }

        internal void ApplyDefaultBehaviour()
        {
            var baseType = GetBaseType(this);
            if (baseType != null && _model.FindWithoutAdd(baseType) == null
                && GetContractFamily(_model, baseType, null) != AttributeFamily.None)
            {
                _model.FindOrAddAuto(baseType, true, false, false);
            }

            var typeAttribs = AttributeMap.Create(_model, _type, false);
            var family = GetContractFamily(_model, _type, typeAttribs);
            if (family == AttributeFamily.AutoTuple)
            {
                SetFlag(OptionsAutoTuple, true, true);
            }
            var isEnum = !EnumPassthru && Helpers.IsEnum(_type);
            if (family == AttributeFamily.None && !isEnum) return; // and you'd like me to do what, exactly?
            BasicList partialIgnores = null, partialMembers = null;
            int dataMemberOffset = 0, implicitFirstTag = 1;
            var inferTagByName = _model.InferTagFromNameDefault;
            var implicitMode = ImplicitFields.None;
            string name = null;
            for (var i = 0; i < typeAttribs.Length; i++)
            {
                var item = typeAttribs[i];
                object tmp;
                var fullAttributeTypeName = item.AttributeType.FullName;
                if (!isEnum && fullAttributeTypeName == "MyProto.ProtoIncludeAttribute")
                {
                    var tag = 0;
                    if (item.TryGet("tag", out tmp)) tag = (int) tmp;
                    var dataFormat = DataFormat.Default;
                    if (item.TryGet("DataFormat", out tmp))
                    {
                        dataFormat = (DataFormat) (int) tmp;
                    }
                    Type knownType = null;
                    try
                    {
                        if (item.TryGet("knownTypeName", out tmp))
                            knownType = _model.GetType((string) tmp, _type
#if WINRT
                            .GetTypeInfo()
#endif
                                .Assembly);
                        else if (item.TryGet("knownType", out tmp)) knownType = (Type) tmp;
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Unable to resolve sub-type of: " + _type.FullName, ex);
                    }
                    if (knownType == null)
                    {
                        throw new InvalidOperationException("Unable to resolve sub-type of: " + _type.FullName);
                    }
                    if (IsValidSubType(knownType)) AddSubType(tag, knownType, dataFormat);
                }

                if (fullAttributeTypeName == "MyProto.ProtoPartialIgnoreAttribute")
                {
                    if (item.TryGet("MemberName", out tmp) && tmp != null)
                    {
                        if (partialIgnores == null) partialIgnores = new BasicList();
                        partialIgnores.Add(tmp);
                    }
                }
                if (!isEnum && fullAttributeTypeName == "MyProto.ProtoPartialMemberAttribute")
                {
                    if (partialMembers == null) partialMembers = new BasicList();
                    partialMembers.Add(item);
                }

                if (fullAttributeTypeName == "MyProto.ProtoContractAttribute")
                {
                    if (item.TryGet("Name", out tmp)) name = (string) tmp;
                    if (!isEnum)
                    {
                        if (item.TryGet("DataMemberOffset", out tmp)) dataMemberOffset = (int) tmp;

#if !FEAT_IKVM
                        // IKVM can't access InferTagFromNameHasValue, but conveniently, InferTagFromName will only be returned if set via ctor or property
                        if (item.TryGet("InferTagFromNameHasValue", false, out tmp) && (bool) tmp)
#endif
                        {
                            if (item.TryGet("InferTagFromName", out tmp)) inferTagByName = (bool) tmp;
                        }

                        if (item.TryGet("ImplicitFields", out tmp) && tmp != null)
                        {
                            implicitMode = (ImplicitFields) (int) tmp;
                            // note that this uses the bizarre unboxing rules of enums/underlying-types
                        }

                        if (item.TryGet("SkipConstructor", out tmp)) UseConstructor = !(bool) tmp;
                        if (item.TryGet("IgnoreListHandling", out tmp)) IgnoreListHandling = (bool) tmp;
                        if (item.TryGet("AsReferenceDefault", out tmp)) AsReferenceDefault = (bool) tmp;
                        if (item.TryGet("ImplicitFirstTag", out tmp) && (int) tmp > 0) implicitFirstTag = (int) tmp;
                    }
                }

                if (fullAttributeTypeName == "System.Runtime.Serialization.DataContractAttribute")
                {
                    if (name == null && item.TryGet("Name", out tmp)) name = (string) tmp;
                }
                if (fullAttributeTypeName == "System.Xml.Serialization.XmlTypeAttribute")
                {
                    if (name == null && item.TryGet("TypeName", out tmp)) name = (string) tmp;
                }
            }
            if (!Helpers.IsNullOrEmpty(name)) Name = name;
            if (implicitMode != ImplicitFields.None)
            {
                family &= AttributeFamily.ProtoBuf; // with implicit fields, **only** proto attributes are important
            }
            MethodInfo[] callbacks = null;

            var members = new BasicList();

#if WINRT
            System.Collections.Generic.IEnumerable<MemberInfo> foundList;
            if(isEnum) {
                foundList = type.GetRuntimeFields();
            }
            else
            {
                System.Collections.Generic.List<MemberInfo> list = new System.Collections.Generic.List<MemberInfo>();
                foreach(PropertyInfo prop in type.GetRuntimeProperties()) {
                    MethodInfo getter = Helpers.GetGetMethod(prop, false, false);
                    if(getter != null && !getter.IsStatic) list.Add(prop);
                }
                foreach(FieldInfo fld in type.GetRuntimeFields()) if(fld.IsPublic && !fld.IsStatic) list.Add(fld);
                foreach(MethodInfo mthd in type.GetRuntimeMethods()) if(mthd.IsPublic && !mthd.IsStatic) list.Add(mthd);
                foundList = list;
            }
#else
            var foundList = _type.GetMembers(isEnum
                ? BindingFlags.Public | BindingFlags.Static
                : BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
#endif
            foreach (var member in foundList)
            {
                if (member.DeclaringType != _type) continue;
                if (member.IsDefined(_model.MapType(typeof (ProtoIgnoreAttribute)), true)) continue;
                if (partialIgnores != null && partialIgnores.Contains(member.Name)) continue;

                bool forced = false, isPublic, isField;
                Type effectiveType;


                PropertyInfo property;
                FieldInfo field;
                MethodInfo method;
                if ((property = member as PropertyInfo) != null)
                {
                    if (isEnum) continue; // wasn't expecting any props!

                    effectiveType = property.PropertyType;
                    isPublic = Helpers.GetGetMethod(property, false, false) != null;
                    isField = false;
                    ApplyDefaultBehaviour_AddMembers(_model, family, isEnum, partialMembers, dataMemberOffset,
                        inferTagByName, implicitMode, members, member, ref forced, isPublic, isField, ref effectiveType);
                }
                else if ((field = member as FieldInfo) != null)
                {
                    effectiveType = field.FieldType;
                    isPublic = field.IsPublic;
                    isField = true;
                    if (isEnum && !field.IsStatic)
                    {
                        // only care about static things on enums; WinRT has a __value instance field!
                        continue;
                    }
                    ApplyDefaultBehaviour_AddMembers(_model, family, isEnum, partialMembers, dataMemberOffset,
                        inferTagByName, implicitMode, members, member, ref forced, isPublic, isField, ref effectiveType);
                }
                else if ((method = member as MethodInfo) != null)
                {
                    if (isEnum) continue;
                    var memberAttribs = AttributeMap.Create(_model, method, false);
                    if (memberAttribs != null && memberAttribs.Length > 0)
                    {
                        CheckForCallback(method, memberAttribs, "MyProto.ProtoBeforeSerializationAttribute",
                            ref callbacks, 0);
                        CheckForCallback(method, memberAttribs, "MyProto.ProtoAfterSerializationAttribute",
                            ref callbacks, 1);
                        CheckForCallback(method, memberAttribs, "MyProto.ProtoBeforeDeserializationAttribute",
                            ref callbacks, 2);
                        CheckForCallback(method, memberAttribs, "MyProto.ProtoAfterDeserializationAttribute",
                            ref callbacks, 3);
                        CheckForCallback(method, memberAttribs, "System.Runtime.Serialization.OnSerializingAttribute",
                            ref callbacks, 4);
                        CheckForCallback(method, memberAttribs, "System.Runtime.Serialization.OnSerializedAttribute",
                            ref callbacks, 5);
                        CheckForCallback(method, memberAttribs, "System.Runtime.Serialization.OnDeserializingAttribute",
                            ref callbacks, 6);
                        CheckForCallback(method, memberAttribs, "System.Runtime.Serialization.OnDeserializedAttribute",
                            ref callbacks, 7);
                    }
                }
            }
            var arr = new ProtoMemberAttribute[members.Count];
            members.CopyTo(arr, 0);

            if (inferTagByName || implicitMode != ImplicitFields.None)
            {
                Array.Sort(arr);
                var nextTag = implicitFirstTag;
                foreach (var normalizedAttribute in arr)
                {
                    if (!normalizedAttribute.TagIsPinned) // if ProtoMember etc sets a tag, we'll trust it
                    {
                        normalizedAttribute.Rebase(nextTag++);
                    }
                }
            }

            foreach (var normalizedAttribute in arr)
            {
                var vm = ApplyDefaultBehaviour(isEnum, normalizedAttribute);
                if (vm != null)
                {
                    Add(vm);
                }
            }

            if (callbacks != null)
            {
                SetCallbacks(Coalesce(callbacks, 0, 4), Coalesce(callbacks, 1, 5),
                    Coalesce(callbacks, 2, 6), Coalesce(callbacks, 3, 7));
            }
        }

        private static void ApplyDefaultBehaviour_AddMembers(TypeModel model, AttributeFamily family, bool isEnum,
            BasicList partialMembers, int dataMemberOffset, bool inferTagByName, ImplicitFields implicitMode,
            BasicList members, MemberInfo member, ref bool forced, bool isPublic, bool isField, ref Type effectiveType)
        {
            switch (implicitMode)
            {
                case ImplicitFields.AllFields:
                    if (isField) forced = true;
                    break;
                case ImplicitFields.AllPublic:
                    if (isPublic) forced = true;
                    break;
            }

            // we just don't like delegate types ;p
#if WINRT
            if (effectiveType.GetTypeInfo().IsSubclassOf(typeof(Delegate))) effectiveType = null;
#else
            if (effectiveType.IsSubclassOf(model.MapType(typeof (Delegate)))) effectiveType = null;
#endif
            if (effectiveType != null)
            {
                var normalizedAttribute = NormalizeProtoMember(model, member, family, forced, isEnum,
                    partialMembers,
                    dataMemberOffset, inferTagByName);
                if (normalizedAttribute != null) members.Add(normalizedAttribute);
            }
        }


        private static MethodInfo Coalesce(MethodInfo[] arr, int x, int y)
        {
            var mi = arr[x];
            if (mi == null) mi = arr[y];
            return mi;
        }

        internal static AttributeFamily GetContractFamily(RuntimeTypeModel model, Type type, AttributeMap[] attributes)
        {
            var family = AttributeFamily.None;

            if (attributes == null) attributes = AttributeMap.Create(model, type, false);

            for (var i = 0; i < attributes.Length; i++)
            {
                switch (attributes[i].AttributeType.FullName)
                {
                    case "MyProto.ProtoContractAttribute":
                        var tmp = false;
                        GetFieldBoolean(ref tmp, attributes[i], "UseProtoMembersOnly");
                        if (tmp) return AttributeFamily.ProtoBuf;
                        family |= AttributeFamily.ProtoBuf;
                        break;
                    case "System.Xml.Serialization.XmlTypeAttribute":
                        if (!model.AutoAddProtoContractTypesOnly)
                        {
                            family |= AttributeFamily.XmlSerializer;
                        }
                        break;
                    case "System.Runtime.Serialization.DataContractAttribute":
                        if (!model.AutoAddProtoContractTypesOnly)
                        {
                            family |= AttributeFamily.DataContractSerialier;
                        }
                        break;
                }
            }
            if (family == AttributeFamily.None)
            {
                // check for obvious tuples
                MemberInfo[] mapping;
                if (ResolveTupleConstructor(type, out mapping) != null)
                {
                    family |= AttributeFamily.AutoTuple;
                }
            }
            return family;
        }

        internal static ConstructorInfo ResolveTupleConstructor(Type type, out MemberInfo[] mappedMembers)
        {
            mappedMembers = null;
            if (type == null) throw new ArgumentNullException("type");
#if WINRT
            TypeInfo typeInfo = type.GetTypeInfo();
            if (typeInfo.IsAbstract) return null; // as if!
            ConstructorInfo[] ctors = Helpers.GetConstructors(typeInfo, false);
#else
            if (type.IsAbstract) return null; // as if!
            var ctors = Helpers.GetConstructors(type, false);
#endif
            // need to have an interesting constructor to bother even checking this stuff
            if (ctors.Length == 0 || (ctors.Length == 1 && ctors[0].GetParameters().Length == 0)) return null;

            var fieldsPropsUnfiltered = Helpers.GetInstanceFieldsAndProperties(type, true);
            var memberList = new BasicList();
            for (var i = 0; i < fieldsPropsUnfiltered.Length; i++)
            {
                var prop = fieldsPropsUnfiltered[i] as PropertyInfo;
                if (prop != null)
                {
                    if (!prop.CanRead) return null; // no use if can't read
                    if (prop.CanWrite && Helpers.GetSetMethod(prop, false, false) != null)
                        return null;
                    // don't allow a public set (need to allow non-public to handle Mono's KeyValuePair<,>)
                    memberList.Add(prop);
                }
                else
                {
                    var field = fieldsPropsUnfiltered[i] as FieldInfo;
                    if (field != null)
                    {
                        if (!field.IsInitOnly) return null; // all public fields must be readonly to be counted a tuple
                        memberList.Add(field);
                    }
                }
            }
            if (memberList.Count == 0)
            {
                return null;
            }

            var members = new MemberInfo[memberList.Count];
            memberList.CopyTo(members, 0);

            var mapping = new int[members.Length];
            var found = 0;
            ConstructorInfo result = null;
            mappedMembers = new MemberInfo[mapping.Length];
            for (var i = 0; i < ctors.Length; i++)
            {
                var parameters = ctors[i].GetParameters();

                if (parameters.Length != members.Length) continue;

                // reset the mappings to test
                for (var j = 0; j < mapping.Length; j++) mapping[j] = -1;

                for (var j = 0; j < parameters.Length; j++)
                {
                    var lower = parameters[j].Name.ToLower();
                    for (var k = 0; k < members.Length; k++)
                    {
                        if (members[k].Name.ToLower() != lower) continue;
                        var memberType = Helpers.GetMemberType(members[k]);
                        if (memberType != parameters[j].ParameterType) continue;

                        mapping[j] = k;
                    }
                }
                // did we map all?
                var notMapped = false;
                for (var j = 0; j < mapping.Length; j++)
                {
                    if (mapping[j] < 0)
                    {
                        notMapped = true;
                        break;
                    }
                    mappedMembers[j] = members[mapping[j]];
                }

                if (notMapped) continue;
                found++;
                result = ctors[i];
            }
            return found == 1 ? result : null;
        }

        private static void CheckForCallback(MethodInfo method, AttributeMap[] attributes, string callbackTypeName,
            ref MethodInfo[] callbacks, int index)
        {
            for (var i = 0; i < attributes.Length; i++)
            {
                if (attributes[i].AttributeType.FullName == callbackTypeName)
                {
                    if (callbacks == null)
                    {
                        callbacks = new MethodInfo[8];
                    }
                    else if (callbacks[index] != null)
                    {
#if WINRT || FEAT_IKVM
                        Type reflected = method.DeclaringType;
#else
                        var reflected = method.ReflectedType;
#endif
                        throw new ProtoException("Duplicate " + callbackTypeName + " callbacks on " + reflected.FullName);
                    }
                    callbacks[index] = method;
                }
            }
        }

        private static bool HasFamily(AttributeFamily value, AttributeFamily required)
        {
            return (value & required) == required;
        }

        private static ProtoMemberAttribute NormalizeProtoMember(TypeModel model, MemberInfo member,
            AttributeFamily family, bool forced, bool isEnum, BasicList partialMembers, int dataMemberOffset,
            bool inferByTagName)
        {
            if (member == null || (family == AttributeFamily.None && !isEnum)) return null; // nix
            int fieldNumber = int.MinValue, minAcceptFieldNumber = inferByTagName ? -1 : 1;
            string name = null;
            bool isPacked = false,
                ignore = false,
                done = false,
                isRequired = false,
                asReference = false,
                asReferenceHasValue = false,
                dynamicType = false,
                tagIsPinned = false,
                overwriteList = false;
            var dataFormat = DataFormat.Default;
            if (isEnum) forced = true;
            var attribs = AttributeMap.Create(model, member, true);
            AttributeMap attrib;

            if (isEnum)
            {
                attrib = GetAttribute(attribs, "MyProto.ProtoIgnoreAttribute");
                if (attrib != null)
                {
                    ignore = true;
                }
                else
                {
                    attrib = GetAttribute(attribs, "MyProto.ProtoEnumAttribute");
#if WINRT || PORTABLE || CF || FX11
                    fieldNumber = Convert.ToInt32(((FieldInfo)member).GetValue(null));
#else
                    fieldNumber = Convert.ToInt32(((FieldInfo) member).GetRawConstantValue());
#endif
                    if (attrib != null)
                    {
                        GetFieldName(ref name, attrib, "Name");
#if !FEAT_IKVM // IKVM can't access HasValue, but conveniently, Value will only be returned if set via ctor or property
                        if ((bool) Helpers.GetInstanceMethod(attrib.AttributeType
#if WINRT
                             .GetTypeInfo()
#endif
                            , "HasValue").Invoke(attrib.Target, null))
#endif
                        {
                            object tmp;
                            if (attrib.TryGet("Value", out tmp)) fieldNumber = (int) tmp;
                        }
                    }
                }
                done = true;
            }

            if (!ignore && !done) // always consider ProtoMember 
            {
                attrib = GetAttribute(attribs, "MyProto.ProtoMemberAttribute");
                GetIgnore(ref ignore, attrib, attribs, "MyProto.ProtoIgnoreAttribute");

                if (!ignore && attrib != null)
                {
                    GetFieldNumber(ref fieldNumber, attrib, "Tag");
                    GetFieldName(ref name, attrib, "Name");
                    GetFieldBoolean(ref isRequired, attrib, "IsRequired");
                    GetFieldBoolean(ref isPacked, attrib, "IsPacked");
                    GetFieldBoolean(ref overwriteList, attrib, "OverwriteList");
                    GetDataFormat(ref dataFormat, attrib, "DataFormat");

#if !FEAT_IKVM
                    // IKVM can't access AsReferenceHasValue, but conveniently, AsReference will only be returned if set via ctor or property
                    GetFieldBoolean(ref asReferenceHasValue, attrib, "AsReferenceHasValue", false);
                    if (asReferenceHasValue)
#endif
                    {
                        asReferenceHasValue = GetFieldBoolean(ref asReference, attrib, "AsReference", true);
                    }
                    GetFieldBoolean(ref dynamicType, attrib, "DynamicType");
                    done = tagIsPinned = fieldNumber > 0; // note minAcceptFieldNumber only applies to non-proto
                }

                if (!done && partialMembers != null)
                {
                    foreach (AttributeMap ppma in partialMembers)
                    {
                        object tmp;
                        if (ppma.TryGet("MemberName", out tmp) && (string) tmp == member.Name)
                        {
                            GetFieldNumber(ref fieldNumber, ppma, "Tag");
                            GetFieldName(ref name, ppma, "Name");
                            GetFieldBoolean(ref isRequired, ppma, "IsRequired");
                            GetFieldBoolean(ref isPacked, ppma, "IsPacked");
                            GetFieldBoolean(ref overwriteList, attrib, "OverwriteList");
                            GetDataFormat(ref dataFormat, ppma, "DataFormat");

#if !FEAT_IKVM
                            // IKVM can't access AsReferenceHasValue, but conveniently, AsReference will only be returned if set via ctor or property
                            GetFieldBoolean(ref asReferenceHasValue, attrib, "AsReferenceHasValue", false);
                            if (asReferenceHasValue)
#endif
                            {
                                asReferenceHasValue = GetFieldBoolean(ref asReference, ppma, "AsReference", true);
                            }
                            GetFieldBoolean(ref dynamicType, ppma, "DynamicType");
                            if (done = tagIsPinned = fieldNumber > 0)
                                break; // note minAcceptFieldNumber only applies to non-proto
                        }
                    }
                }
            }

            if (!ignore && !done && HasFamily(family, AttributeFamily.DataContractSerialier))
            {
                attrib = GetAttribute(attribs, "System.Runtime.Serialization.DataMemberAttribute");
                if (attrib != null)
                {
                    GetFieldNumber(ref fieldNumber, attrib, "Order");
                    GetFieldName(ref name, attrib, "Name");
                    GetFieldBoolean(ref isRequired, attrib, "IsRequired");
                    done = fieldNumber >= minAcceptFieldNumber;
                    if (done)
                        fieldNumber += dataMemberOffset;
                    // dataMemberOffset only applies to DCS flags, to allow us to "bump" WCF by a notch
                }
            }
            if (!ignore && !done && HasFamily(family, AttributeFamily.XmlSerializer))
            {
                attrib = GetAttribute(attribs, "System.Xml.Serialization.XmlElementAttribute");
                if (attrib == null) attrib = GetAttribute(attribs, "System.Xml.Serialization.XmlArrayAttribute");
                GetIgnore(ref ignore, attrib, attribs, "System.Xml.Serialization.XmlIgnoreAttribute");
                if (attrib != null && !ignore)
                {
                    GetFieldNumber(ref fieldNumber, attrib, "Order");
                    GetFieldName(ref name, attrib, "ElementName");
                    done = fieldNumber >= minAcceptFieldNumber;
                }
            }
            if (!ignore && !done)
            {
                if (GetAttribute(attribs, "System.NonSerializedAttribute") != null) ignore = true;
            }
            if (ignore || (fieldNumber < minAcceptFieldNumber && !forced)) return null;
            var result = new ProtoMemberAttribute(fieldNumber, forced || inferByTagName);
            result.AsReference = asReference;
            result.AsReferenceHasValue = asReferenceHasValue;
            result.DataFormat = dataFormat;
            result.DynamicType = dynamicType;
            result.IsPacked = isPacked;
            result.OverwriteList = overwriteList;
            result.IsRequired = isRequired;
            result.Name = Helpers.IsNullOrEmpty(name) ? member.Name : name;
            result.Member = member;
            result.TagIsPinned = tagIsPinned;
            return result;
        }

        private ValueMember ApplyDefaultBehaviour(bool isEnum, ProtoMemberAttribute normalizedAttribute)
        {
            MemberInfo member;
            if (normalizedAttribute == null || (member = normalizedAttribute.Member) == null) return null; // nix

            var effectiveType = Helpers.GetMemberType(member);


            Type itemType = null;
            Type defaultType = null;

            // check for list types
            ResolveListTypes(_model, effectiveType, ref itemType, ref defaultType);
            // but take it back if it is explicitly excluded
            if (itemType != null)
            {
                // looks like a list, but double check for IgnoreListHandling
                var idx = _model.FindOrAddAuto(effectiveType, false, true, false);
                if (idx >= 0 && _model[effectiveType].IgnoreListHandling)
                {
                    itemType = null;
                    defaultType = null;
                }
            }
            var attribs = AttributeMap.Create(_model, member, true);
            AttributeMap attrib;

            object defaultValue = null;
            // implicit zero default
            if (_model.UseImplicitZeroDefaults)
            {
                switch (Helpers.GetTypeCode(effectiveType))
                {
                    case ProtoTypeCode.Boolean:
                        defaultValue = false;
                        break;
                    case ProtoTypeCode.Decimal:
                        defaultValue = (decimal) 0;
                        break;
                    case ProtoTypeCode.Single:
                        defaultValue = (float) 0;
                        break;
                    case ProtoTypeCode.Double:
                        defaultValue = (double) 0;
                        break;
                    case ProtoTypeCode.Byte:
                        defaultValue = (byte) 0;
                        break;
                    case ProtoTypeCode.Char:
                        defaultValue = (char) 0;
                        break;
                    case ProtoTypeCode.Int16:
                        defaultValue = (short) 0;
                        break;
                    case ProtoTypeCode.Int32:
                        defaultValue = 0;
                        break;
                    case ProtoTypeCode.Int64:
                        defaultValue = (long) 0;
                        break;
                    case ProtoTypeCode.SByte:
                        defaultValue = (sbyte) 0;
                        break;
                    case ProtoTypeCode.UInt16:
                        defaultValue = (ushort) 0;
                        break;
                    case ProtoTypeCode.UInt32:
                        defaultValue = (uint) 0;
                        break;
                    case ProtoTypeCode.UInt64:
                        defaultValue = (ulong) 0;
                        break;
                    case ProtoTypeCode.TimeSpan:
                        defaultValue = TimeSpan.Zero;
                        break;
                    case ProtoTypeCode.Guid:
                        defaultValue = Guid.Empty;
                        break;
                }
            }
            if ((attrib = GetAttribute(attribs, "System.ComponentModel.DefaultValueAttribute")) != null)
            {
                object tmp;
                if (attrib.TryGet("Value", out tmp)) defaultValue = tmp;
            }
            var vm = ((isEnum || normalizedAttribute.Tag > 0))
                ? new ValueMember(_model, _type, normalizedAttribute.Tag, member, effectiveType, itemType, defaultType,
                    normalizedAttribute.DataFormat, defaultValue)
                : null;
            if (vm != null)
            {
#if WINRT
                TypeInfo finalType = typeInfo;
#else
                var finalType = _type;
#endif
                var prop = Helpers.GetProperty(finalType, member.Name + "Specified", true);
                var getMethod = Helpers.GetGetMethod(prop, true, true);
                if (getMethod == null || getMethod.IsStatic) prop = null;
                if (prop != null)
                {
                    vm.SetSpecified(getMethod, Helpers.GetSetMethod(prop, true, true));
                }
                else
                {
                    var method = Helpers.GetInstanceMethod(finalType, "ShouldSerialize" + member.Name,
                        Helpers.EmptyTypes);
                    if (method != null && method.ReturnType == _model.MapType(typeof (bool)))
                    {
                        vm.SetSpecified(method, null);
                    }
                }
                if (!Helpers.IsNullOrEmpty(normalizedAttribute.Name)) vm.SetName(normalizedAttribute.Name);
                vm.IsPacked = normalizedAttribute.IsPacked;
                vm.IsRequired = normalizedAttribute.IsRequired;
                vm.OverwriteList = normalizedAttribute.OverwriteList;
                if (normalizedAttribute.AsReferenceHasValue)
                {
                    vm.AsReference = normalizedAttribute.AsReference;
                }
                vm.DynamicType = normalizedAttribute.DynamicType;
            }
            return vm;
        }

        private static void GetDataFormat(ref DataFormat value, AttributeMap attrib, string memberName)
        {
            if ((attrib == null) || (value != DataFormat.Default)) return;
            object obj;
            if (attrib.TryGet(memberName, out obj) && obj != null) value = (DataFormat) obj;
        }

        private static void GetIgnore(ref bool ignore, AttributeMap attrib, AttributeMap[] attribs, string fullName)
        {
            if (ignore || attrib == null) return;
            ignore = GetAttribute(attribs, fullName) != null;
        }

        private static void GetFieldBoolean(ref bool value, AttributeMap attrib, string memberName)
        {
            GetFieldBoolean(ref value, attrib, memberName, true);
        }

        private static bool GetFieldBoolean(ref bool value, AttributeMap attrib, string memberName, bool publicOnly)
        {
            if (attrib == null) return false;
            if (value) return true;
            object obj;
            if (attrib.TryGet(memberName, publicOnly, out obj) && obj != null)
            {
                value = (bool) obj;
                return true;
            }
            return false;
        }

        private static void GetFieldNumber(ref int value, AttributeMap attrib, string memberName)
        {
            if (attrib == null || value > 0) return;
            object obj;
            if (attrib.TryGet(memberName, out obj) && obj != null) value = (int) obj;
        }

        private static void GetFieldName(ref string name, AttributeMap attrib, string memberName)
        {
            if (attrib == null || !Helpers.IsNullOrEmpty(name)) return;
            object obj;
            if (attrib.TryGet(memberName, out obj) && obj != null) name = (string) obj;
        }

        private static AttributeMap GetAttribute(AttributeMap[] attribs, string fullName)
        {
            for (var i = 0; i < attribs.Length; i++)
            {
                var attrib = attribs[i];
                if (attrib != null && attrib.AttributeType.FullName == fullName) return attrib;
            }
            return null;
        }

        /// <summary>
        ///     Adds a member (by name) to the MetaType
        /// </summary>
        public MetaType Add(int fieldNumber, string memberName)
        {
            AddField(fieldNumber, memberName, null, null, null);
            return this;
        }

        /// <summary>
        ///     Adds a member (by name) to the MetaType, returning the ValueMember rather than the fluent API.
        ///     This is otherwise identical to Add.
        /// </summary>
        public ValueMember AddField(int fieldNumber, string memberName)
        {
            return AddField(fieldNumber, memberName, null, null, null);
        }

        /// <summary>
        ///     Gets or sets whether the type should use a parameterless constructor (the default),
        ///     or whether the type should skip the constructor completely. This option is not supported
        ///     on compact-framework.
        /// </summary>
        public bool UseConstructor
        {
            // negated to have defaults as flat zero
            get { return !HasFlag(OptionsSkipConstructor); }
            set { SetFlag(OptionsSkipConstructor, !value, true); }
        }

        /// <summary>
        ///     The concrete type to create when a new instance of this type is needed; this may be useful when dealing
        ///     with dynamic proxies, or with interface-based APIs
        /// </summary>
        public Type ConstructType
        {
            get { return _constructType; }
            set
            {
                ThrowIfFrozen();
                _constructType = value;
            }
        }

        private Type _constructType;

        /// <summary>
        ///     Adds a member (by name) to the MetaType
        /// </summary>
        public MetaType Add(string memberName)
        {
            Add(GetNextFieldNumber(), memberName);
            return this;
        }

        private Type _surrogate;

        /// <summary>
        ///     Performs serialization of this type via a surrogate; all
        ///     other serialization options are ignored and handled
        ///     by the surrogate's configuration.
        /// </summary>
        public void SetSurrogate(Type surrogateType)
        {
            if (surrogateType == _type) surrogateType = null;
            if (surrogateType != null)
            {
                // note that BuildSerializer checks the **CURRENT TYPE** is OK to be surrogated
                if (surrogateType != null &&
                    Helpers.IsAssignableFrom(_model.MapType(typeof (IEnumerable)), surrogateType))
                {
                    throw new ArgumentException(
                        "Repeated data (a list, collection, etc) has inbuilt behaviour and cannot be used as a surrogate");
                }
            }
            ThrowIfFrozen();
            _surrogate = surrogateType;
            // no point in offering chaining; no options are respected
        }

        internal MetaType GetSurrogateOrSelf()
        {
            if (_surrogate != null) return _model[_surrogate];
            return this;
        }

        internal MetaType GetSurrogateOrBaseOrSelf(bool deep)
        {
            if (_surrogate != null) return _model[_surrogate];
            var snapshot = BaseType;
            if (snapshot != null)
            {
                if (deep)
                {
                    MetaType tmp;
                    do
                    {
                        tmp = snapshot;
                        snapshot = snapshot.BaseType;
                    } while (snapshot != null);
                    return tmp;
                }
                return snapshot;
            }
            return this;
        }

        private int GetNextFieldNumber()
        {
            var maxField = 0;
            foreach (ValueMember member in _fields)
            {
                if (member.FieldNumber > maxField) maxField = member.FieldNumber;
            }
            if (_subTypes != null)
            {
                foreach (SubType subType in _subTypes)
                {
                    if (subType.FieldNumber > maxField) maxField = subType.FieldNumber;
                }
            }
            return maxField + 1;
        }

        /// <summary>
        ///     Adds a set of members (by name) to the MetaType
        /// </summary>
        public MetaType Add(params string[] memberNames)
        {
            if (memberNames == null) throw new ArgumentNullException("memberNames");
            var next = GetNextFieldNumber();
            for (var i = 0; i < memberNames.Length; i++)
            {
                Add(next++, memberNames[i]);
            }
            return this;
        }


        /// <summary>
        ///     Adds a member (by name) to the MetaType
        /// </summary>
        public MetaType Add(int fieldNumber, string memberName, object defaultValue)
        {
            AddField(fieldNumber, memberName, null, null, defaultValue);
            return this;
        }

        /// <summary>
        ///     Adds a member (by name) to the MetaType, including an itemType and defaultType for representing lists
        /// </summary>
        public MetaType Add(int fieldNumber, string memberName, Type itemType, Type defaultType)
        {
            AddField(fieldNumber, memberName, itemType, defaultType, null);
            return this;
        }

        /// <summary>
        ///     Adds a member (by name) to the MetaType, including an itemType and defaultType for representing lists, returning
        ///     the ValueMember rather than the fluent API.
        ///     This is otherwise identical to Add.
        /// </summary>
        public ValueMember AddField(int fieldNumber, string memberName, Type itemType, Type defaultType)
        {
            return AddField(fieldNumber, memberName, itemType, defaultType, null);
        }

        private ValueMember AddField(int fieldNumber, string memberName, Type itemType, Type defaultType,
            object defaultValue)
        {
            MemberInfo mi = null;
#if WINRT
            mi = Helpers.IsEnum(type) ? type.GetTypeInfo().GetDeclaredField(memberName) : Helpers.GetInstanceMember(type.GetTypeInfo(), memberName);

#else
            var members = _type.GetMember(memberName,
                Helpers.IsEnum(_type)
                    ? BindingFlags.Static | BindingFlags.Public
                    : BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (members != null && members.Length == 1) mi = members[0];
#endif
            if (mi == null) throw new ArgumentException("Unable to determine member: " + memberName, "memberName");

            Type miType;
#if WINRT || PORTABLE
            PropertyInfo pi = mi as PropertyInfo;
            if (pi == null)
            {
                FieldInfo fi = mi as FieldInfo;
                if (fi == null)
                {
                    throw new NotSupportedException(mi.GetType().Name);
                }
                else
                {
                    miType = fi.FieldType;
                }
            }
            else
            {
                miType = pi.PropertyType;
            }
#else
            switch (mi.MemberType)
            {
                case MemberTypes.Field:
                    miType = ((FieldInfo) mi).FieldType;
                    break;
                case MemberTypes.Property:
                    miType = ((PropertyInfo) mi).PropertyType;
                    break;
                default:
                    throw new NotSupportedException(mi.MemberType.ToString());
            }
#endif
            ResolveListTypes(_model, miType, ref itemType, ref defaultType);
            var newField = new ValueMember(_model, _type, fieldNumber, mi, miType, itemType, defaultType,
                DataFormat.Default, defaultValue);
            Add(newField);
            return newField;
        }

        internal static void ResolveListTypes(TypeModel model, Type type, ref Type itemType, ref Type defaultType)
        {
            if (type == null) return;
            // handle arrays
            if (type.IsArray)
            {
                if (type.GetArrayRank() != 1)
                {
                    throw new NotSupportedException("Multi-dimension arrays are supported");
                }
                itemType = type.GetElementType();
                if (itemType == model.MapType(typeof (byte)))
                {
                    defaultType = itemType = null;
                }
                else
                {
                    defaultType = type;
                }
            }
            // handle lists
            if (itemType == null)
            {
                itemType = TypeModel.GetListItemType(model, type);
            }

            // check for nested data (not allowed)
            if (itemType != null)
            {
                Type nestedItemType = null, nestedDefaultType = null;
                ResolveListTypes(model, itemType, ref nestedItemType, ref nestedDefaultType);
                if (nestedItemType != null)
                {
                    throw TypeModel.CreateNestedListsNotSupported();
                }
            }

            if (itemType != null && defaultType == null)
            {
#if WINRT
                TypeInfo typeInfo = type.GetTypeInfo();
                if (typeInfo.IsClass && !typeInfo.IsAbstract && Helpers.GetConstructor(typeInfo, Helpers.EmptyTypes, true) != null)
#else
                if (type.IsClass && !type.IsAbstract && Helpers.GetConstructor(type, Helpers.EmptyTypes, true) != null)
#endif
                {
                    defaultType = type;
                }
                if (defaultType == null)
                {
#if WINRT
                    if (typeInfo.IsInterface)
#else
                    if (type.IsInterface)
#endif
                    {
#if NO_GENERICS
                        defaultType = typeof(ArrayList);
#else
                        Type[] genArgs;
#if WINRT
                        if (typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(System.Collections.Generic.IDictionary<,>)
                            && itemType == typeof(System.Collections.Generic.KeyValuePair<,>).MakeGenericType(genArgs = typeInfo.GenericTypeArguments))
#else
                        if (type.IsGenericType &&
                            type.GetGenericTypeDefinition() == model.MapType(typeof (IDictionary<,>))
                            &&
                            itemType ==
                            model.MapType(typeof (KeyValuePair<,>))
                                .MakeGenericType(genArgs = type.GetGenericArguments()))
#endif
                        {
                            defaultType = model.MapType(typeof (Dictionary<,>)).MakeGenericType(genArgs);
                        }
                        else
                        {
                            defaultType = model.MapType(typeof (List<>)).MakeGenericType(itemType);
                        }
#endif
                    }
                }
                // verify that the default type is appropriate
                if (defaultType != null && !Helpers.IsAssignableFrom(type, defaultType))
                {
                    defaultType = null;
                }
            }
        }

        private void Add(ValueMember member)
        {
            var opaqueToken = 0;
            try
            {
                _model.TakeLock(ref opaqueToken);
                ThrowIfFrozen();
                _fields.Add(member);
            }
            finally
            {
                _model.ReleaseLock(opaqueToken);
            }
        }

        /// <summary>
        ///     Returns the ValueMember that matchs a given field number, or null if not found
        /// </summary>
        public ValueMember this[int fieldNumber]
        {
            get
            {
                foreach (ValueMember member in _fields)
                {
                    if (member.FieldNumber == fieldNumber) return member;
                }
                return null;
            }
        }

        /// <summary>
        ///     Returns the ValueMember that matchs a given member (property/field), or null if not found
        /// </summary>
        public ValueMember this[MemberInfo member]
        {
            get
            {
                if (member == null) return null;
                foreach (ValueMember x in _fields)
                {
                    if (x.Member == member) return x;
                }
                return null;
            }
        }

        private readonly BasicList _fields = new BasicList();

        /// <summary>
        ///     Returns the ValueMember instances associated with this type
        /// </summary>
        public ValueMember[] GetFields()
        {
            var arr = new ValueMember[_fields.Count];
            _fields.CopyTo(arr, 0);
            Array.Sort(arr, ValueMember.Comparer.Default);
            return arr;
        }

        /// <summary>
        ///     Returns the SubType instances associated with this type
        /// </summary>
        public SubType[] GetSubtypes()
        {
            if (_subTypes == null || _subTypes.Count == 0) return new SubType[0];
            var arr = new SubType[_subTypes.Count];
            _subTypes.CopyTo(arr, 0);
            Array.Sort(arr, SubType.Comparer.Default);
            return arr;
        }

#if FEAT_COMPILER && !FX11

        /// <summary>
        ///     Compiles the serializer for this type; this is *not* a full
        ///     standalone compile, but can significantly boost performance
        ///     while allowing additional types to be added.
        /// </summary>
        /// <remarks>An in-place compile can access non-public types / members</remarks>
        public void CompileInPlace()
        {
#if FEAT_IKVM
    // just no nothing, quietely; don't want to break the API
#else
            _serializer = CompiledSerializer.Wrap(Serializer, _model);
#endif
        }
#endif

        internal bool IsDefined(int fieldNumber)
        {
            foreach (ValueMember field in _fields)
            {
                if (field.FieldNumber == fieldNumber) return true;
            }
            return false;
        }

        internal int GetKey(bool demand, bool getBaseKey)
        {
            return _model.GetKey(_type, demand, getBaseKey);
        }


        internal EnumSerializer.EnumPair[] GetEnumMap()
        {
            if (HasFlag(OptionsEnumPassThru)) return null;
            var result = new EnumSerializer.EnumPair[_fields.Count];
            for (var i = 0; i < result.Length; i++)
            {
                var member = (ValueMember) _fields[i];
                var wireValue = member.FieldNumber;
                var value = member.GetRawEnumValue();
                result[i] = new EnumSerializer.EnumPair(wireValue, value, member.MemberType);
            }
            return result;
        }


        /// <summary>
        ///     Gets or sets a value indicating that an enum should be treated directly as an int/short/etc, rather
        ///     than enforcing .proto enum rules. This is useful *in particul* for [Flags] enums.
        /// </summary>
        public bool EnumPassthru
        {
            get { return HasFlag(OptionsEnumPassThru); }
            set { SetFlag(OptionsEnumPassThru, value, true); }
        }

        /// <summary>
        ///     Gets or sets a value indicating that this type should NOT be treated as a list, even if it has
        ///     familiar list-like characteristics (enumerable, add, etc)
        /// </summary>
        public bool IgnoreListHandling
        {
            get { return HasFlag(OptionsIgnoreListHandling); }
            set { SetFlag(OptionsIgnoreListHandling, value, true); }
        }

        internal bool Pending
        {
            get { return HasFlag(OptionsPending); }
            set { SetFlag(OptionsPending, value, false); }
        }

        private const byte
            OptionsPending = 1,
            OptionsEnumPassThru = 2,
            OptionsFrozen = 4,
            OptionsPrivateOnApi = 8,
            OptionsSkipConstructor = 16,
            OptionsAsReferenceDefault = 32,
            OptionsAutoTuple = 64,
            OptionsIgnoreListHandling = 128;

        private volatile byte _flags;

        private bool HasFlag(byte flag)
        {
            return (_flags & flag) == flag;
        }

        private void SetFlag(byte flag, bool value, bool throwIfFrozen)
        {
            if (throwIfFrozen && HasFlag(flag) != value)
            {
                ThrowIfFrozen();
            }
            if (value)
                _flags |= flag;
            else
                _flags = (byte) (_flags & ~flag);
        }

        internal static MetaType GetRootType(MetaType source)
        {
            while (source._serializer != null)
            {
                var tmp = source.BaseType;
                if (tmp == null) return source;
                source = tmp; // else loop until we reach something that isn't generated, or is the root
            }

            // now we get into uncertain territory
            var model = source._model;
            var opaqueToken = 0;
            try
            {
                model.TakeLock(ref opaqueToken);

                MetaType tmp;
                while ((tmp = source.BaseType) != null) source = tmp;
                return source;
            }
            finally
            {
                model.ReleaseLock(opaqueToken);
            }
        }

        internal bool IsPrepared()
        {
#if FEAT_COMPILER && !FEAT_IKVM && !FX11
            return _serializer is CompiledSerializer;
#else
            return false;
            #endif
        }

        internal IEnumerable Fields
        {
            get { return _fields; }
        }

        internal static StringBuilder NewLine(StringBuilder builder, int indent)
        {
            return Helpers.AppendLine(builder).Append(' ', indent*3);
        }

        internal bool IsAutoTuple
        {
            get { return HasFlag(OptionsAutoTuple); }
        }

        internal void WriteSchema(StringBuilder builder, int indent, ref bool requiresBclImport)
        {
            if (_surrogate != null) return; // nothing to write


            var fieldsArr = new ValueMember[_fields.Count];
            _fields.CopyTo(fieldsArr, 0);
            Array.Sort(fieldsArr, ValueMember.Comparer.Default);

            if (IsList)
            {
                var itemTypeName = _model.GetSchemaTypeName(TypeModel.GetListItemType(_model, _type),
                    DataFormat.Default,
                    false, false, ref requiresBclImport);
                NewLine(builder, indent).Append("message ").Append(GetSchemaTypeName()).Append(" {");
                NewLine(builder, indent + 1).Append("repeated ").Append(itemTypeName).Append(" items = 1;");
                NewLine(builder, indent).Append('}');
            }
            else if (IsAutoTuple)
            {
                // Key-value-pair etc
                MemberInfo[] mapping;
                if (ResolveTupleConstructor(_type, out mapping) != null)
                {
                    NewLine(builder, indent).Append("message ").Append(GetSchemaTypeName()).Append(" {");
                    for (var i = 0; i < mapping.Length; i++)
                    {
                        Type effectiveType;
                        if (mapping[i] is PropertyInfo)
                        {
                            effectiveType = ((PropertyInfo) mapping[i]).PropertyType;
                        }
                        else if (mapping[i] is FieldInfo)
                        {
                            effectiveType = ((FieldInfo) mapping[i]).FieldType;
                        }
                        else
                        {
                            throw new NotSupportedException("Unknown member type: " + mapping[i].GetType().Name);
                        }
                        NewLine(builder, indent + 1)
                            .Append("optional ")
                            .Append(
                                _model.GetSchemaTypeName(effectiveType, DataFormat.Default, false, false,
                                    ref requiresBclImport).Replace('.', '_'))
                            .Append(' ').Append(mapping[i].Name).Append(" = ").Append(i + 1).Append(';');
                    }
                    NewLine(builder, indent).Append('}');
                }
            }
            else if (Helpers.IsEnum(_type))
            {
                NewLine(builder, indent).Append("enum ").Append(GetSchemaTypeName()).Append(" {");
                if (fieldsArr.Length == 0 && EnumPassthru)
                {
                    if (_type
#if WINRT
                    .GetTypeInfo()
#endif
                        .IsDefined(_model.MapType(typeof (FlagsAttribute)), false))
                    {
                        NewLine(builder, indent + 1).Append("// this is a composite/flags enumeration");
                    }
                    else
                    {
                        NewLine(builder, indent + 1).Append("// this enumeration will be passed as a raw value");
                    }
                    foreach (var field in
#if WINRT
                        type.GetRuntimeFields()
#else
                        _type.GetFields()
#endif
                        )
                    {
                        if (field.IsStatic && field.IsLiteral)
                        {
                            object enumVal;
#if WINRT || PORTABLE || CF || FX11
                            enumVal = field.GetValue(null);
#else
                            enumVal = field.GetRawConstantValue();
#endif
                            NewLine(builder, indent + 1).Append(field.Name).Append(" = ").Append(enumVal).Append(";");
                        }
                    }
                }
                else
                {
                    foreach (var member in fieldsArr)
                    {
                        NewLine(builder, indent + 1)
                            .Append(member.Name)
                            .Append(" = ")
                            .Append(member.FieldNumber)
                            .Append(';');
                    }
                }
                NewLine(builder, indent).Append('}');
            }
            else
            {
                NewLine(builder, indent).Append("message ").Append(GetSchemaTypeName()).Append(" {");
                foreach (var member in fieldsArr)
                {
                    var ordinality = member.ItemType != null
                        ? "repeated"
                        : member.IsRequired ? "required" : "optional";
                    NewLine(builder, indent + 1).Append(ordinality).Append(' ');
                    if (member.DataFormat == DataFormat.Group) builder.Append("group ");
                    var schemaTypeName = member.GetSchemaTypeName(true, ref requiresBclImport);
                    builder.Append(schemaTypeName).Append(" ")
                        .Append(member.Name).Append(" = ").Append(member.FieldNumber);
                    if (member.DefaultValue != null)
                    {
                        if (member.DefaultValue is string)
                        {
                            builder.Append(" [default = \"").Append(member.DefaultValue).Append("\"]");
                        }
                        else if (member.DefaultValue is bool)
                        {
                            // need to be lower case (issue 304)
                            builder.Append((bool) member.DefaultValue ? " [default = true]" : " [default = false]");
                        }
                        else
                        {
                            builder.Append(" [default = ").Append(member.DefaultValue).Append(']');
                        }
                    }
                    if (member.ItemType != null && member.IsPacked)
                    {
                        builder.Append(" [packed=true]");
                    }
                    builder.Append(';');
                    if (schemaTypeName == "bcl.NetObjectProxy" && member.AsReference && !member.DynamicType)
                        // we know what it is; tell the user
                    {
                        builder.Append(" // reference-tracked ")
                            .Append(member.GetSchemaTypeName(false, ref requiresBclImport));
                    }
                }
                if (_subTypes != null && _subTypes.Count != 0)
                {
                    NewLine(builder, indent + 1)
                        .Append("// the following represent sub-types; at most 1 should have a value");
                    var subTypeArr = new SubType[_subTypes.Count];
                    _subTypes.CopyTo(subTypeArr, 0);
                    Array.Sort(subTypeArr, SubType.Comparer.Default);
                    foreach (var subType in subTypeArr)
                    {
                        var subTypeName = subType.DerivedType.GetSchemaTypeName();
                        NewLine(builder, indent + 1).Append("optional ").Append(subTypeName)
                            .Append(" ").Append(subTypeName).Append(" = ").Append(subType.FieldNumber).Append(';');
                    }
                }
                NewLine(builder, indent).Append('}');
            }
        }
    }
}

#endif