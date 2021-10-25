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
    public class XCookieManager
    {

        /// <summary>
        ///     cookie容器
        /// </summary>
        [ProtoMember(1)]
        public Dictionary<string, XCookie> Cookies = new Dictionary<string, XCookie>();

        /// <summary>
        ///     cookie属性
        /// </summary>
        public Dictionary<string, string> AttributeDictionary = new Dictionary<string, string>();
    }
}