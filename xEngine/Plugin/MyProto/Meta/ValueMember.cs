﻿using System.Collections;
using System.Collections.Generic;
#if !NO_RUNTIME
using System;
using MyProto.Serializers;
using System.Globalization;
#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#else
using System.Reflection;

#endif

namespace MyProto.Meta
{
    /// <summary>
    ///     Represents a member (property/field) that is mapped to a protobuf field
    /// </summary>
    public class ValueMember
    {
        private const byte
            OptionsIsStrict = 1,
            OptionsIsPacked = 2,
            OptionsIsRequired = 4,
            OptionsOverwriteList = 8,
            OptionsSupportNull = 16;

        private readonly Type _defaultType;
        private readonly int _fieldNumber;
        private readonly Type _itemType;
        private readonly MemberInfo _member;
        private readonly Type _memberType;
        private readonly RuntimeTypeModel _model;
        private readonly Type _parentType;
        private bool _asReference;
        private DataFormat _dataFormat;
        private object _defaultValue;
        private bool _dynamicType;
        private byte _flags;
        private MethodInfo _getSpecified;
        private string _name;
        private IProtoSerializer _serializer;
        private MethodInfo _setSpecified;

        /// <summary>
        ///     Creates a new ValueMember instance
        /// </summary>
        public ValueMember(RuntimeTypeModel model, Type parentType, int fieldNumber, MemberInfo member, Type memberType,
            Type itemType, Type defaultType, DataFormat dataFormat, object defaultValue)
            : this(model, fieldNumber, memberType, itemType, defaultType, dataFormat)
        {
            if (member == null) throw new ArgumentNullException("member");
            if (parentType == null) throw new ArgumentNullException("parentType");
            if (fieldNumber < 1 && !Helpers.IsEnum(parentType)) throw new ArgumentOutOfRangeException("fieldNumber");

            _member = member;
            _parentType = parentType;
            if (fieldNumber < 1 && !Helpers.IsEnum(parentType)) throw new ArgumentOutOfRangeException("fieldNumber");
//#if WINRT
            if (defaultValue != null && model.MapType(defaultValue.GetType()) != memberType)
//#else
//            if (defaultValue != null && !memberType.IsInstanceOfType(defaultValue))
//#endif
            {
                defaultValue = ParseDefaultValue(memberType, defaultValue);
            }
            _defaultValue = defaultValue;

            var type = model.FindWithoutAdd(memberType);
            if (type != null)
            {
                _asReference = type.AsReferenceDefault;
            }
            else
            {
                // we need to scan the hard way; can't risk recursion by fully walking it
                _asReference = MetaType.GetAsReferenceDefault(model, memberType);
            }
        }

        /// <summary>
        ///     Creates a new ValueMember instance
        /// </summary>
        internal ValueMember(RuntimeTypeModel model, int fieldNumber, Type memberType, Type itemType, Type defaultType,
            DataFormat dataFormat)
        {
            if (memberType == null) throw new ArgumentNullException("memberType");
            if (model == null) throw new ArgumentNullException("model");
            _fieldNumber = fieldNumber;
            _memberType = memberType;
            _itemType = itemType;
            _defaultType = defaultType;

            _model = model;
            _dataFormat = dataFormat;
        }

        /// <summary>
        ///     The number that identifies this member in a protobuf stream
        /// </summary>
        public int FieldNumber
        {
            get { return _fieldNumber; }
        }

        /// <summary>
        ///     Gets the member (field/property) which this member relates to.
        /// </summary>
        public MemberInfo Member
        {
            get { return _member; }
        }

        /// <summary>
        ///     Within a list / array / etc, the type of object for each item in the list (especially useful with ArrayList)
        /// </summary>
        public Type ItemType
        {
            get { return _itemType; }
        }

        /// <summary>
        ///     The underlying type of the member
        /// </summary>
        public Type MemberType
        {
            get { return _memberType; }
        }

        /// <summary>
        ///     For abstract types (IList etc), the type of concrete object to create (if required)
        /// </summary>
        public Type DefaultType
        {
            get { return _defaultType; }
        }

        /// <summary>
        ///     The type the defines the member
        /// </summary>
        public Type ParentType
        {
            get { return _parentType; }
        }

        /// <summary>
        ///     The default value of the item (members with this value will not be serialized)
        /// </summary>
        public object DefaultValue
        {
            get { return _defaultValue; }
            set
            {
                ThrowIfFrozen();
                _defaultValue = value;
            }
        }

        internal IProtoSerializer Serializer
        {
            get
            {
                if (_serializer == null) _serializer = BuildSerializer();
                return _serializer;
            }
        }

        /// <summary>
        ///     Specifies the rules used to process the field; this is used to determine the most appropriate
        ///     wite-type, but also to describe subtypes <i>within</i> that wire-type (such as SignedVariant)
        /// </summary>
        public DataFormat DataFormat
        {
            get { return _dataFormat; }
            set
            {
                ThrowIfFrozen();
                _dataFormat = value;
            }
        }

        /// <summary>
        ///     Indicates whether this field should follow strict encoding rules; this means (for example) that if a "fixed32"
        ///     is encountered when "variant" is defined, then it will fail (throw an exception) when parsing. Note that
        ///     when serializing the defined type is always used.
        /// </summary>
        public bool IsStrict
        {
            get { return HasFlag(OptionsIsStrict); }
            set { SetFlag(OptionsIsStrict, value, true); }
        }

        /// <summary>
        ///     Indicates whether this field should use packed encoding (which can save lots of space for repeated primitive
        ///     values).
        ///     This option only applies to list/array data of primitive types (int, double, etc).
        /// </summary>
        public bool IsPacked
        {
            get { return HasFlag(OptionsIsPacked); }
            set { SetFlag(OptionsIsPacked, value, true); }
        }

        /// <summary>
        ///     Indicates whether this field should *repace* existing values (the default is false, meaning *append*).
        ///     This option only applies to list/array data.
        /// </summary>
        public bool OverwriteList
        {
            get { return HasFlag(OptionsOverwriteList); }
            set { SetFlag(OptionsOverwriteList, value, true); }
        }

        /// <summary>
        ///     Indicates whether this field is mandatory.
        /// </summary>
        public bool IsRequired
        {
            get { return HasFlag(OptionsIsRequired); }
            set { SetFlag(OptionsIsRequired, value, true); }
        }

        /// <summary>
        ///     Enables full object-tracking/full-graph support.
        /// </summary>
        public bool AsReference
        {
            get { return _asReference; }
            set
            {
                ThrowIfFrozen();
                _asReference = value;
            }
        }

        /// <summary>
        ///     Embeds the type information into the stream, allowing usage with types not known in advance.
        /// </summary>
        public bool DynamicType
        {
            get { return _dynamicType; }
            set
            {
                ThrowIfFrozen();
                _dynamicType = value;
            }
        }

        /// <summary>
        ///     Gets the logical name for this member in the schema (this is not critical for binary serialization, but may be used
        ///     when inferring a schema).
        /// </summary>
        public string Name
        {
            get { return Helpers.IsNullOrEmpty(_name) ? _member.Name : _name; }
        }

        /// <summary>
        ///     Should lists have extended support for null values? Note this makes the serialization less efficient.
        /// </summary>
        public bool SupportNull
        {
            get { return HasFlag(OptionsSupportNull); }
            set { SetFlag(OptionsSupportNull, value, true); }
        }

        internal object GetRawEnumValue()
        {
#if WINRT || PORTABLE || CF || FX11
            object value = ((FieldInfo)member).GetValue(null);
            switch(Helpers.GetTypeCode(Enum.GetUnderlyingType(((FieldInfo)member).FieldType)))
            {
                case ProtoTypeCode.SByte: return (sbyte)value;
                case ProtoTypeCode.Byte: return (byte)value;
                case ProtoTypeCode.Int16: return (short)value;
                case ProtoTypeCode.UInt16: return (ushort)value;
                case ProtoTypeCode.Int32: return (int)value;
                case ProtoTypeCode.UInt32: return (uint)value;
                case ProtoTypeCode.Int64: return (long)value;
                case ProtoTypeCode.UInt64: return (ulong)value;
                default:
                    throw new InvalidOperationException();
            }
#else
            return ((FieldInfo) _member).GetRawConstantValue();
#endif
        }

        private static object ParseDefaultValue(Type type, object value)
        {
            {
                var tmp = Helpers.GetUnderlyingType(type);
                if (tmp != null) type = tmp;
            }
            if (value is string)
            {
                var s = (string) value;
                if (Helpers.IsEnum(type)) return Helpers.ParseEnum(type, s);

                switch (Helpers.GetTypeCode(type))
                {
                    case ProtoTypeCode.Boolean:
                        return bool.Parse(s);
                    case ProtoTypeCode.Byte:
                        return byte.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    case ProtoTypeCode.Char: // char.Parse missing on CF/phone7
                        if (s.Length == 1) return s[0];
                        throw new FormatException("Single character expected: \"" + s + "\"");
                    case ProtoTypeCode.DateTime:
                        return DateTime.Parse(s, CultureInfo.InvariantCulture);
                    case ProtoTypeCode.Decimal:
                        return decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture);
                    case ProtoTypeCode.Double:
                        return double.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture);
                    case ProtoTypeCode.Int16:
                        return short.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture);
                    case ProtoTypeCode.Int32:
                        return int.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture);
                    case ProtoTypeCode.Int64:
                        return long.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture);
                    case ProtoTypeCode.SByte:
                        return sbyte.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    case ProtoTypeCode.Single:
                        return float.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture);
                    case ProtoTypeCode.String:
                        return s;
                    case ProtoTypeCode.UInt16:
                        return ushort.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture);
                    case ProtoTypeCode.UInt32:
                        return uint.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture);
                    case ProtoTypeCode.UInt64:
                        return ulong.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture);
                    case ProtoTypeCode.TimeSpan:
                        return TimeSpan.Parse(s);
                    case ProtoTypeCode.Uri:
                        return s; // Uri is decorated as string
                    case ProtoTypeCode.Guid:
                        return new Guid(s);
                }
            }
#if FEAT_IKVM
            if (Helpers.IsEnum(type)) return value; // return the underlying type instead
            System.Type convertType = null;
            switch(Helpers.GetTypeCode(type))
            {
                case ProtoTypeCode.SByte: convertType = typeof(sbyte); break;
                case ProtoTypeCode.Int16: convertType = typeof(short); break;
                case ProtoTypeCode.Int32: convertType = typeof(int); break;
                case ProtoTypeCode.Int64: convertType = typeof(long); break;
                case ProtoTypeCode.Byte: convertType = typeof(byte); break;
                case ProtoTypeCode.UInt16: convertType = typeof(ushort); break;
                case ProtoTypeCode.UInt32: convertType = typeof(uint); break;
                case ProtoTypeCode.UInt64: convertType = typeof(ulong); break;
                case ProtoTypeCode.Single: convertType = typeof(float); break;
                case ProtoTypeCode.Double: convertType = typeof(double); break;
                case ProtoTypeCode.Decimal: convertType = typeof(decimal); break;
            }
            if(convertType != null) return Convert.ChangeType(value, convertType, CultureInfo.InvariantCulture);
            throw new ArgumentException("Unable to process default value: " + value + ", " + type.FullName);
#else
            if (Helpers.IsEnum(type)) return Enum.ToObject(type, value);
            return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
#endif
        }

        /// <summary>
        ///     Specifies methods for working with optional data members.
        /// </summary>
        /// <param name="getSpecified">
        ///     Provides a method (null for none) to query whether this member should
        ///     be serialized; it must be of the form "bool {Method}()". The member is only serialized if the
        ///     method returns true.
        /// </param>
        /// <param name="setSpecified">
        ///     Provides a method (null for none) to indicate that a member was
        ///     deserialized; it must be of the form "void {Method}(bool)", and will be called with "true"
        ///     when data is found.
        /// </param>
        public void SetSpecified(MethodInfo getSpecified, MethodInfo setSpecified)
        {
            if (getSpecified != null)
            {
                if (getSpecified.ReturnType != _model.MapType(typeof (bool))
                    || getSpecified.IsStatic
                    || getSpecified.GetParameters().Length != 0)
                {
                    throw new ArgumentException("Invalid pattern for checking member-specified", "getSpecified");
                }
            }
            if (setSpecified != null)
            {
                ParameterInfo[] args;
                if (setSpecified.ReturnType != _model.MapType(typeof (void))
                    || setSpecified.IsStatic
                    || (args = setSpecified.GetParameters()).Length != 1
                    || args[0].ParameterType != _model.MapType(typeof (bool)))
                {
                    throw new ArgumentException("Invalid pattern for setting member-specified", "setSpecified");
                }
            }
            ThrowIfFrozen();
            _getSpecified = getSpecified;
            _setSpecified = setSpecified;
        }

        private void ThrowIfFrozen()
        {
            if (_serializer != null)
                throw new InvalidOperationException("The type cannot be changed once a serializer has been generated");
        }

        private IProtoSerializer BuildSerializer()
        {
            var opaqueToken = 0;
            try
            {
                _model.TakeLock(ref opaqueToken); // check nobody is still adding this type
                WireType wireType;
                var finalType = _itemType == null ? _memberType : _itemType;
                var ser = TryGetCoreSerializer(_model, _dataFormat, finalType, out wireType, _asReference,
                    _dynamicType,
                    OverwriteList, true);
                if (ser == null)
                {
                    throw new InvalidOperationException("No serializer defined for type: " + finalType.FullName);
                }

                // apply tags
                if (_itemType != null && SupportNull)
                {
                    if (IsPacked)
                    {
                        throw new NotSupportedException("Packed encodings cannot support null values");
                    }
                    ser = new TagDecorator(NullDecorator.Tag, wireType, IsStrict, ser);
                    ser = new NullDecorator(_model, ser);
                    ser = new TagDecorator(_fieldNumber, WireType.StartGroup, false, ser);
                }
                else
                {
                    ser = new TagDecorator(_fieldNumber, wireType, IsStrict, ser);
                }
                // apply lists if appropriate
                if (_itemType != null)
                {
#if NO_GENERICS
                    Type underlyingItemType = itemType;
#else
                    var underlyingItemType = SupportNull
                        ? _itemType
                        : Helpers.GetUnderlyingType(_itemType) ?? _itemType;
#endif
                    Helpers.DebugAssert(underlyingItemType == ser.ExpectedType,
                        "Wrong type in the tail; expected {0}, received {1}", ser.ExpectedType, underlyingItemType);
                    if (_memberType.IsArray)
                    {
                        ser = new ArrayDecorator(_model, ser, _fieldNumber, IsPacked, wireType, _memberType,
                            OverwriteList, SupportNull);
                    }
                    else
                    {
                        ser = new ListDecorator(_model, _memberType, _defaultType, ser, _fieldNumber, IsPacked, wireType,
                            _member != null && PropertyDecorator.CanWrite(_model, _member), OverwriteList, SupportNull);
                    }
                }
                else if (_defaultValue != null && !IsRequired && _getSpecified == null)
                {
                    // note: "ShouldSerialize*" / "*Specified" / etc ^^^^ take precedence over defaultValue,
                    // as does "IsRequired"
                    ser = new DefaultValueDecorator(_model, _defaultValue, ser);
                }
                if (_memberType == _model.MapType(typeof (Uri)))
                {
                    ser = new UriDecorator(_model, ser);
                }
                if (_member != null)
                {
                    var prop = _member as PropertyInfo;
                    if (prop != null)
                    {
                        ser = new PropertyDecorator(_model, _parentType, (PropertyInfo) _member, ser);
                    }
                    else
                    {
                        var fld = _member as FieldInfo;
                        if (fld != null)
                        {
                            ser = new FieldDecorator(_parentType, (FieldInfo) _member, ser);
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                    if (_getSpecified != null || _setSpecified != null)
                    {
                        ser = new MemberSpecifiedDecorator(_getSpecified, _setSpecified, ser);
                    }
                }
                return ser;
            }
            finally
            {
                _model.ReleaseLock(opaqueToken);
            }
        }

        private static WireType GetIntWireType(DataFormat format, int width)
        {
            switch (format)
            {
                case DataFormat.ZigZag:
                    return WireType.SignedVariant;
                case DataFormat.FixedSize:
                    return width == 32 ? WireType.Fixed32 : WireType.Fixed64;
                case DataFormat.TwosComplement:
                case DataFormat.Default:
                    return WireType.Variant;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static WireType GetDateTimeWireType(DataFormat format)
        {
            switch (format)
            {
                case DataFormat.Group:
                    return WireType.StartGroup;
                case DataFormat.FixedSize:
                    return WireType.Fixed64;
                case DataFormat.Default:
                    return WireType.String;
                default:
                    throw new InvalidOperationException();
            }
        }

        internal static IProtoSerializer TryGetCoreSerializer(RuntimeTypeModel model, DataFormat dataFormat, Type type,
            out WireType defaultWireType,
            bool asReference, bool dynamicType, bool overwriteList, bool allowComplexTypes)
        {
#if !NO_GENERICS
            {
                var tmp = Helpers.GetUnderlyingType(type);
                if (tmp != null) type = tmp;
            }
#endif
            if (Helpers.IsEnum(type))
            {
                if (allowComplexTypes && model != null)
                {
                    // need to do this before checking the typecode; an int enum will report Int32 etc
                    defaultWireType = WireType.Variant;
                    return new EnumSerializer(type, model.GetEnumMap(type));
                }
                // enum is fine for adding as a meta-type
                defaultWireType = WireType.None;
                return null;
            }
            var code = Helpers.GetTypeCode(type);
            switch (code)
            {
                case ProtoTypeCode.Int32:
                    defaultWireType = GetIntWireType(dataFormat, 32);
                    return new Int32Serializer(model);
                case ProtoTypeCode.UInt32:
                    defaultWireType = GetIntWireType(dataFormat, 32);
                    return new UInt32Serializer(model);
                case ProtoTypeCode.Int64:
                    defaultWireType = GetIntWireType(dataFormat, 64);
                    return new Int64Serializer(model);
                case ProtoTypeCode.UInt64:
                    defaultWireType = GetIntWireType(dataFormat, 64);
                    return new UInt64Serializer(model);
                case ProtoTypeCode.String:
                    defaultWireType = WireType.String;
                    if (asReference)
                    {
                        return new NetObjectSerializer(model, model.MapType(typeof (string)), 0,
                            BclHelpers.NetObjectOptions.AsReference);
                    }
                    return new StringSerializer(model);
                case ProtoTypeCode.Single:
                    defaultWireType = WireType.Fixed32;
                    return new SingleSerializer(model);
                case ProtoTypeCode.Double:
                    defaultWireType = WireType.Fixed64;
                    return new DoubleSerializer(model);
                case ProtoTypeCode.Boolean:
                    defaultWireType = WireType.Variant;
                    return new BooleanSerializer(model);
                case ProtoTypeCode.DateTime:
                    defaultWireType = GetDateTimeWireType(dataFormat);
                    return new DateTimeSerializer(model);
                case ProtoTypeCode.Decimal:
                    defaultWireType = WireType.String;
                    return new DecimalSerializer(model);
                case ProtoTypeCode.Byte:
                    defaultWireType = GetIntWireType(dataFormat, 32);
                    return new ByteSerializer(model);
                case ProtoTypeCode.SByte:
                    defaultWireType = GetIntWireType(dataFormat, 32);
                    return new SByteSerializer(model);
                case ProtoTypeCode.Char:
                    defaultWireType = WireType.Variant;
                    return new CharSerializer(model);
                case ProtoTypeCode.Int16:
                    defaultWireType = GetIntWireType(dataFormat, 32);
                    return new Int16Serializer(model);
                case ProtoTypeCode.UInt16:
                    defaultWireType = GetIntWireType(dataFormat, 32);
                    return new UInt16Serializer(model);
                case ProtoTypeCode.TimeSpan:
                    defaultWireType = GetDateTimeWireType(dataFormat);
                    return new TimeSpanSerializer(model);
                case ProtoTypeCode.Guid:
                    defaultWireType = WireType.String;
                    return new GuidSerializer(model);
                case ProtoTypeCode.Uri:
                    defaultWireType = WireType.String;
                    return new StringSerializer(model); // treat as string; wrapped in decorator later
                case ProtoTypeCode.ByteArray:
                    defaultWireType = WireType.String;
                    return new BlobSerializer(model, overwriteList);
                case ProtoTypeCode.Type:
                    defaultWireType = WireType.String;
                    return new SystemTypeSerializer(model);
            }
            IProtoSerializer parseable = model.AllowParseableTypes ? ParseableSerializer.TryCreate(type, model) : null;
            if (parseable != null)
            {
                defaultWireType = WireType.String;
                return parseable;
            }
            if (allowComplexTypes && model != null)
            {
                var key = model.GetKey(type, false, true);
                if (asReference || dynamicType)
                {
                    defaultWireType = dataFormat == DataFormat.Group ? WireType.StartGroup : WireType.String;
                    var options = BclHelpers.NetObjectOptions.None;
                    if (asReference) options |= BclHelpers.NetObjectOptions.AsReference;
                    if (dynamicType) options |= BclHelpers.NetObjectOptions.DynamicType;
                    if (key >= 0)
                    {
                        // exists
                        if (asReference && Helpers.IsValueType(type))
                        {
                            var message = "AsReference cannot be used with value-types";

                            if (type.Name == "KeyValuePair`2")
                            {
                                message += ";";
                            }
                            else
                            {
                                message += ": " + type.FullName;
                            }
                            throw new InvalidOperationException(message);
                        }
                        var meta = model[type];
                        if (asReference && meta.IsAutoTuple) options |= BclHelpers.NetObjectOptions.LateSet;
                        if (meta.UseConstructor) options |= BclHelpers.NetObjectOptions.UseConstructor;
                    }
                    return new NetObjectSerializer(model, type, key, options);
                }
                if (key >= 0)
                {
                    defaultWireType = dataFormat == DataFormat.Group ? WireType.StartGroup : WireType.String;
                    return new SubItemSerializer(type, key, model[type], true);
                }
            }
            defaultWireType = WireType.None;
            return null;
        }

        internal void SetName(string name)
        {
            ThrowIfFrozen();
            _name = name;
        }

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

        internal string GetSchemaTypeName(bool applyNetObjectProxy, ref bool requiresBclImport)
        {
            var effectiveType = ItemType;
            if (effectiveType == null) effectiveType = MemberType;
            return _model.GetSchemaTypeName(effectiveType, DataFormat, applyNetObjectProxy && _asReference,
                applyNetObjectProxy && _dynamicType, ref requiresBclImport);
        }

        internal sealed class Comparer : IComparer
#if !NO_GENERICS
            , IComparer<ValueMember>
#endif
        {
            public static readonly Comparer Default = new Comparer();

            public int Compare(object x, object y)
            {
                return Compare(x as ValueMember, y as ValueMember);
            }

            public int Compare(ValueMember x, ValueMember y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (x == null) return -1;
                if (y == null) return 1;

                return x.FieldNumber.CompareTo(y.FieldNumber);
            }
        }
    }
}

#endif