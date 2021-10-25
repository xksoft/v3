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
    public class MySoft
    {
        /// <summary>
        ///     编号
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; internal set; }

        /// <summary>
        ///     唯一识别码
        /// </summary>
        [ProtoMember(2)]
        public string Key { get; internal set; }

        /// <summary>
        ///     软件名称
        /// </summary>
        [ProtoMember(3)]
        public string Name { get; internal set; }

        /// <summary>
        ///     版本号
        /// </summary>
        [ProtoMember(4)]
        public string Version { get; internal set; }
    }
}