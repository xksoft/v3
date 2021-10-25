using System;
using System.Collections.Generic;

using System.Text;
using System.Collections;
using MyProto;

namespace Model
{
    [ProtoContract]
    [Serializable]
    public class HashDB
    {
        /// <summary>
        /// 哈希库模型
        /// </summary>
        public HashDB() { }
        private string _dbID;
        /// <summary>
        /// 哈希库库ID
        /// </summary>
         [ProtoMember(1)]
        public string Id
        {
            set { _dbID = value; }
            get { return _dbID; }
        }


        private string _dbName;
        /// <summary>
        /// 哈希库名称
        /// </summary>
        [ProtoMember(2)]
        public string Name
        {
            set { _dbName = value; }
            get { return _dbName; }
        }

        private string _dbType;
        /// <summary>
        /// 哈希库组别
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
    }
}
