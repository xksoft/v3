using System;
using System.Collections.Generic;

using System.Text;
using System.Collections;
using MyProto;

namespace Model
{

    [ProtoContract]
    [Serializable]
    public class KeywordDB
    {
        /// <summary>
        /// 关键词库模型
        /// </summary>
        public KeywordDB() { }
        private string _dbID;
        /// <summary>
        /// 关键词库ID
        /// </summary>
        [ProtoMember(1)]
        public string Id
        {
            set { _dbID = value; }
            get { return _dbID; }
        }


        private string _dbName;
        /// <summary>
        /// 关键词库名称
        /// </summary>
         [ProtoMember(2)]
        public string Name
        {
            set { _dbName = value; }
            get { return _dbName; }
        }

        private string _dbType;
        /// <summary>
        /// 关键词库组别
        /// </summary>
         [ProtoMember(3)]
        public string Type
        {
            set { _dbType = value; }
            get { return _dbType; }
        }

        private int _TaskCount;
        /// <summary>
        /// 使用该库的任务数
        /// </summary>
        [ProtoMember(4)]
        public int TaskCount
        {
            set { _TaskCount = value; }
            get { return _TaskCount; }
        }

        private int _DataCount;
        /// <summary>
        /// 该库的所有数据量
        /// </summary>
        [ProtoMember(5)]
        public int DataCount
        {
            set { _DataCount = value; }
            get { return _DataCount; }
        }

        public List<string> _Keywords = new List<string>();
        /// <summary>
        /// 所有的关键词
        /// </summary>
         [ProtoMember(6)]
        public List<string> Keywords
        {
            set { _Keywords = value; }
            get { return _Keywords; }
        }
    }
}
