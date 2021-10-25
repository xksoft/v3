#region

using System;
using MyProto;

#endregion

namespace xEngine.Model.Execute.Http
{
    /// <summary>
    ///     侠客SDK Cookie模型
    /// </summary>
    [ProtoContract]
    [Serializable]
    public class XCookie
    {
        /// <summary>
        ///     域
        /// </summary>
        [ProtoMember(4)] public string Domain;

        /// <summary>
        ///     名称
        /// </summary>
        [ProtoMember(1)] public string Name;

        /// <summary>
        ///     路径
        /// </summary>
        [ProtoMember(3)] public string Path;

        /// <summary>
        ///     值
        /// </summary>
        [ProtoMember(2)] public string Value;
    }
}