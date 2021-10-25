using System;
using System.Collections.Generic;

using System.Text;
using MyProto;

namespace Model.发布相关模型
{
    [ProtoContract]
    [Serializable]
    public class GetPostAction
    {
        /// <summary>
        /// 是否是utf8编码
        /// </summary>
        [ProtoMember(1)]
        public bool IsUtf8 = false;
        /// <summary> 是否是POST动作
        /// </summary>
        [ProtoMember(2)]
        public bool IsPost = false;
        /// <summary>
        /// 目标地址
        /// </summary>
        [ProtoMember(3)]
        public string ActionUrl = "【后台地址】";
        /// <summary>
        /// 来路地址
        /// </summary>
        [ProtoMember(4)]
        public string RefrereUrl = "";
        /// <summary>
        /// 浏览器标头
        /// </summary>
        [ProtoMember(5)]
        public string UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)";

        /// <summary>
        /// post模式
        /// </summary>
        [ProtoMember(6)]
        public bool PostModel = false;

        /// <summary>
        /// post参数
        /// </summary>
        [ProtoMember(7)]
        public Dictionary<string, string> PostData = new Dictionary<string, string>();

        private bool isGetRedirect = false;
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(8)]
        public bool IsGetRedirect
        {
            get { return isGetRedirect; }
            set { isGetRedirect = value; }
        }

    }
}
