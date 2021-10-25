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
    public class XRequest
    {
        /// <summary>
        ///     请求延迟
        /// </summary>
        [ProtoMember(1)] public int Delay = 0;

        /// <summary>
        ///     请求编码
        /// </summary>
        [ProtoMember(2)] public string Encoding = "gbk";

        /// <summary>
        ///     更多头部
        /// </summary>
        [ProtoMember(3)] public List<KeyValuePair<string, string>> Header = new List<KeyValuePair<string, string>>();

        /// <summary>
        ///     是否使用侠客云请求
        /// </summary>
        [ProtoMember(4)] public bool IsUseProxy;

        /// <summary>
        ///     需要POST的数据
        /// </summary>
        [ProtoMember(5)] public List<KeyValuePair<string, string>> PostData = new List<KeyValuePair<string, string>>();

        /// <summary>
        ///     需要POST的数据
        /// </summary>
        [ProtoMember(6)] public string PostString = "";

        /// <summary>
        ///     请求的来路
        /// </summary>
        [ProtoMember(7)] public string Referer = "";

        /// <summary>
        ///     触发条件-包含
        /// </summary>
        [ProtoMember(8)] public string Require = "";

        /// <summary>
        ///     User-Agent标记
        /// </summary>
        [ProtoMember(9)] public string UserAgent =
            "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2;)";

        /// <summary>
        /// </summary>
        public XRequest()
        {
            Description = "未命名";
        }

        /// <summary>
        ///     注释文本
        /// </summary>
        [ProtoMember(10)]
        public string Description { get; set; }

        /// <summary>
        ///     请求方式，0=GET 1=POST 2=POST(Multipart)
        /// </summary>
        [ProtoMember(11)]
        public int Method { get; set; }

        /// <summary>
        ///     目标Url
        /// </summary>
        [ProtoMember(12)]
        public string Url { get; set; }
    }
}