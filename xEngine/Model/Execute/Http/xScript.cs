#region

using System;
using System.Collections.Generic;
using MyProto;

#endregion

namespace xEngine.Model.Execute.Http
{
    /// <summary>
    /// </summary>
    [ProtoContract]
    [Serializable]
    public class XScript
    {
        /// <summary>
        ///     账号
        /// </summary>
        [ProtoMember(4)] public XAccount Account;

        /// <summary>
        ///     文件数据
        /// </summary>
        [ProtoMember(5)] public ByteData ByteData;

        /// <summary>
        ///     cookie管理容器
        /// </summary>
        [ProtoMember(1)] public XCookieManager CookieManager = new XCookieManager();

        /// <summary>
        ///     所有脚本
        /// </summary>
        [ProtoMember(2)] public List<XRequest> Scripts = new List<XRequest>();

        /// <summary>
        ///     变量
        /// </summary>
        [ProtoMember(3)] public XTag Tag;
    }
}