using System;

namespace MyProto
{
    /// <summary>
    ///     Indicates that a type is defined for protocol-buffer serialization.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface,
        Inherited = false)]
    public sealed class ProtoContractAttribute : Attribute
    {
        private const byte
            OptionsInferTagFromName = 1,
            OptionsInferTagFromNameHasValue = 2,
            OptionsUseProtoMembersOnly = 4,
            OptionsSkipConstructor = 8,
            OptionsIgnoreListHandling = 16,
            OptionsAsReferenceDefault = 32;

        private byte _flags;
        private int _implicitFirstTag;

        /// <summary>
        ///     Gets or sets the defined name of the type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the fist offset to use with implicit field tags;
        ///     only uesd if ImplicitFields is set.
        /// </summary>
        public int ImplicitFirstTag
        {
            get { return _implicitFirstTag; }
            set
            {
                if (value < 1) throw new ArgumentOutOfRangeException("ImplicitFirstTag");
                _implicitFirstTag = value;
            }
        }

        /// <summary>
        ///     If specified, alternative contract markers (such as markers for XmlSerailizer or DataContractSerializer) are
        ///     ignored.
        /// </summary>
        public bool UseProtoMembersOnly
        {
            get { return HasFlag(OptionsUseProtoMembersOnly); }
            set { SetFlag(OptionsUseProtoMembersOnly, value); }
        }

        /// <summary>
        ///     If specified, do NOT treat this type as a list, even if it looks like one.
        /// </summary>
        public bool IgnoreListHandling
        {
            get { return HasFlag(OptionsIgnoreListHandling); }
            set { SetFlag(OptionsIgnoreListHandling, value); }
        }

        /// <summary>
        ///     Gets or sets the mechanism used to automatically infer field tags
        ///     for members. This option should be used in advanced scenarios only.
        ///     Please review the important notes against the ImplicitFields enumeration.
        /// </summary>
        public ImplicitFields ImplicitFields { get; set; }

        /// <summary>
        ///     Enables/disables automatic tag generation based on the existing name / order
        ///     of the defined members. This option is not used for members marked
        ///     with ProtoMemberAttribute, as intended to provide compatibility with
        ///     WCF serialization. WARNING: when adding new fields you must take
        ///     care to increase the Order for new elements, otherwise data corruption
        ///     may occur.
        /// </summary>
        /// <remarks>If not explicitly specified, the default is assumed from Serializer.GlobalOptions.InferTagFromName.</remarks>
        public bool InferTagFromName
        {
            get { return HasFlag(OptionsInferTagFromName); }
            set
            {
                SetFlag(OptionsInferTagFromName, value);
                SetFlag(OptionsInferTagFromNameHasValue, true);
            }
        }

        /// <summary>
        ///     Has a InferTagFromName value been explicitly set? if not, the default from the type-model is assumed.
        /// </summary>
        internal bool InferTagFromNameHasValue
        {
            // note that this property is accessed via reflection and should not be removed
            get { return HasFlag(OptionsInferTagFromNameHasValue); }
        }

        /// <summary>
        ///     Specifies an offset to apply to [DataMember(Order=...)] markers;
        ///     this is useful when working with mex-generated classes that have
        ///     a different origin (usually 1 vs 0) than the original data-contract.
        ///     This value is added to the Order of each member.
        /// </summary>
        public int DataMemberOffset { get; set; }

        /// <summary>
        ///     If true, the constructor for the type is bypassed during deserialization, meaning any field initializers
        ///     or other initialization code is skipped.
        /// </summary>
        public bool SkipConstructor
        {
            get { return HasFlag(OptionsSkipConstructor); }
            set { SetFlag(OptionsSkipConstructor, value); }
        }

        /// <summary>
        ///     Should this type be treated as a reference by default? Please also see the implications of this,
        ///     as recorded on ProtoMemberAttribute.AsReference
        /// </summary>
        public bool AsReferenceDefault
        {
            get { return HasFlag(OptionsAsReferenceDefault); }
            set { SetFlag(OptionsAsReferenceDefault, value); }
        }

        private bool HasFlag(byte flag)
        {
            return (_flags & flag) == flag;
        }

        private void SetFlag(byte flag, bool value)
        {
            if (value) _flags |= flag;
            else _flags = (byte) (_flags & ~flag);
        }
    }
}