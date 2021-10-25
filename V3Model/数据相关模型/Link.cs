using System;
using System.Collections.Generic;

using System.Text;
using MyProto;

namespace Model
{
    [ProtoContract]
    [Serializable]
    public class Link
    {
        /// <summary>
        /// 链接库信息模型
        /// </summary>
        public Link() { }


        private string _Title;
        /// <summary>
        /// 链接的标题
        /// </summary>
          [ProtoMember(1)]
        public string Title
        {
            set { _Title = value; }
            get { return _Title; }
        }

        private string _Url;
        /// <summary>
        /// 链接的地址
        /// </summary>
          [ProtoMember(2)]
        public string Url
        {
            set { _Url = value; }
            get { return _Url; }
        }

        private string _keyword;
        /// <summary>
        /// 链接的关键字
        /// </summary>
          [ProtoMember(3)]
        public string Keyword
        {
            set { _keyword = value; }
            get { return _keyword; }
        }
    }
}
