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
    public class XResponse
    {
        /// <summary>
        ///     返回头部列表
        /// </summary>
        [ProtoMember(7)]
        public readonly List<string> HeaderList = new List<string>();

        /// <summary>
        ///     返回二进制主体
        /// </summary>
        [ProtoMember(1)]
        public byte[] BodyData;

        /// <summary>
        ///     返回内容列表
        /// </summary>
        [ProtoMember(6)]
        public List<string> BodyList = new List<string>();

        /// <summary>
        ///     返回字符串主体
        /// </summary>
        [ProtoMember(5)]
        public string BodyString = "";

        /// <summary>
        ///     内容类型
        /// </summary>
        [ProtoMember(2)]
        public string ContentType = "text/html";

        /// <summary>
        ///     头部信息
        /// </summary>
        [ProtoMember(3)]
        public string Header;

        /// <summary>
        ///     重定向地址
        /// </summary>
        [ProtoMember(9)]
        public string Location;

        /// <summary>
        ///     请求编码
        /// </summary>
        [ProtoMember(8)]
        public string RequestEncoding = "utf-8";

        /// <summary>
        ///     原始数据
        /// </summary>
        [ProtoMember(10)]
        public string ResponseHeader;

        /// <summary>
        ///     状态吗
        /// </summary>
        [ProtoMember(4)]
        public int StatusCode = 0;

        /// <summary>
        ///     状态吗
        /// </summary>
        [ProtoMember(11)]
        public string AllContent = "";
    }
}