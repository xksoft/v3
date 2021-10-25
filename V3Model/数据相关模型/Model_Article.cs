using System;
using System.Collections.Generic;

using System.Text;
using System.Collections;
using MyProto;

namespace Model
{
    [ProtoContract]
    [Serializable]
    public class Model_Article
    {
        /// <summary>
        /// 文章信息模型
        /// </summary>
        private string _DataId="";
        /// <summary>
        /// 文章的数字ID
        /// </summary>
        [ProtoMember(1)]
        public string Id
        {
            set { _DataId = value; }
            get { return _DataId; }
        }

        private string _DATAArticleDbID;
        /// <summary>
        /// 文章所在的数据库的ID
        /// </summary>
         [ProtoMember(2)]
        public string Db
        {
            set { _DATAArticleDbID = value; }
            get { return _DATAArticleDbID; }
        }

        private string _HashDBid = "0";
        /// <summary>
        /// 这篇文章所挂钩的哈希库ID
        /// </summary>
         [ProtoMember(3)]
        public string HashDB
        {
            set { _HashDBid = value; }
            get { return _HashDBid; }
        }


        private string  _DataTime;
        /// <summary>
        /// 这篇文章的最后修改时间
        /// </summary>
         [ProtoMember(4)]
        public string Date
        {
            set { _DataTime = value; }
            get { return _DataTime; }

            
        }

        [ProtoMember(5)] 
        public Dictionary<int, string> DataObject = new Dictionary<int, string>
        {
           
        };


    }
}
