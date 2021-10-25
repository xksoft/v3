#region

using System;
using MyProto;

#endregion

namespace xEngine.Model.Execute.Http
{
    [ProtoContract]
    [Serializable]
    public class XAccount
    {
        /// <summary>
        ///     账号
        /// </summary>
        [ProtoMember(1)] public string Account;

        /// <summary>
        ///     Cookie容器
        /// </summary>
        [ProtoMember(2)] public XCookieManager Cookie;

        /// <summary>
        ///     编号
        /// </summary>
        [ProtoMember(3)] public int Id;

        /// <summary>
        ///     最后活动时间
        /// </summary>
        [ProtoMember(4)] public DateTime LastActive;

        /// <summary>
        ///     附加值1
        /// </summary>
        [ProtoMember(5)] public string Other1;

        /// <summary>
        ///     附加值2
        /// </summary>
        [ProtoMember(6)] public string Other2;

        /// <summary>
        ///     附加值3
        /// </summary>
        [ProtoMember(7)] public string Other3;

        /// <summary>
        ///     附加值3
        /// </summary>
        [ProtoMember(8)] public string Other4;

        /// <summary>
        ///     附加值3
        /// </summary>
        [ProtoMember(9)] public string Other5;

        /// <summary>
        ///     密码
        /// </summary>
        [ProtoMember(10)] public string Password;

        /// <summary>
        ///     状态值
        /// </summary>
        [ProtoMember(11)] public int Status;

        /// <summary>
        ///     状态文本
        /// </summary>
        [ProtoMember(12)] public string StatusText;
    }
}