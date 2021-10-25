#region

using System;
using MyProto;

#endregion

namespace xEngine.Model.License
{
    /// <summary>
    /// </summary>
    [ProtoContract]
    [Serializable]
    public class MyLicense
    {
        /// <summary>
        /// </summary>
        [ProtoMember(1)]
        public string Custom { get; internal set; }

        /// <summary>
        /// </summary>
        [ProtoMember(2)]
        public string Description { get; internal set; }

        /// <summary>
        /// </summary>
        [ProtoMember(3)]
        public DateTime Expiration { get; internal set; }

        /// <summary>
        /// </summary>
        [ProtoMember(4)]
        public int Id { get; internal set; }

        /// <summary>
        /// </summary>
        [ProtoMember(5)]
        public string Key { get; internal set; }

        /// <summary>
        /// </summary>
        [ProtoMember(6)]
        public int Level { get; internal set; }

        /// <summary>
        /// </summary>
        [ProtoMember(7)]
        public string Name { get; internal set; }
    }
}