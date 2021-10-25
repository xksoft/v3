using System.Collections;
using System.Collections.Generic;
#if !NO_RUNTIME
using System;
using MyProto.Serializers;

namespace MyProto.Meta
{
    /// <summary>
    ///     Represents an inherited type in a type hierarchy.
    /// </summary>
    public sealed class SubType
    {
        private readonly DataFormat _dataFormat;
        private readonly MetaType _derivedType;
        private readonly int _fieldNumber;
        private IProtoSerializer _serializer;

        /// <summary>
        ///     Creates a new SubType instance.
        /// </summary>
        /// <param name="fieldNumber">
        ///     The field-number that is used to encapsulate the data (as a nested
        ///     message) for the derived dype.
        /// </param>
        /// <param name="derivedType">The sub-type to be considered.</param>
        /// <param name="format">
        ///     Specific encoding style to use; in particular, Grouped can be used to avoid buffering, but is not
        ///     the default.
        /// </param>
        public SubType(int fieldNumber, MetaType derivedType, DataFormat format)
        {
            if (derivedType == null) throw new ArgumentNullException("derivedType");
            if (fieldNumber <= 0) throw new ArgumentOutOfRangeException("fieldNumber");
            _fieldNumber = fieldNumber;
            _derivedType = derivedType;
            _dataFormat = format;
        }

        /// <summary>
        ///     The field-number that is used to encapsulate the data (as a nested
        ///     message) for the derived dype.
        /// </summary>
        public int FieldNumber
        {
            get { return _fieldNumber; }
        }

        /// <summary>
        ///     The sub-type to be considered.
        /// </summary>
        public MetaType DerivedType
        {
            get { return _derivedType; }
        }

        internal IProtoSerializer Serializer
        {
            get
            {
                if (_serializer == null) _serializer = BuildSerializer();
                return _serializer;
            }
        }

        private IProtoSerializer BuildSerializer()
        {
            // note the caller here is MetaType.BuildSerializer, which already has the sync-lock
            var wireType = WireType.String;
            if (_dataFormat == DataFormat.Group) wireType = WireType.StartGroup; // only one exception

            IProtoSerializer ser = new SubItemSerializer(_derivedType.Type, _derivedType.GetKey(false, false),
                _derivedType, false);
            return new TagDecorator(_fieldNumber, wireType, false, ser);
        }

        internal sealed class Comparer : IComparer
#if !NO_GENERICS
            , IComparer<SubType>
#endif
        {
            public static readonly Comparer Default = new Comparer();

            public int Compare(object x, object y)
            {
                return Compare(x as SubType, y as SubType);
            }

            public int Compare(SubType x, SubType y)
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