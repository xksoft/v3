using System;
using System.Collections.Generic;

using System.Text;
using System.Collections;
using MyProto;

namespace Model
{

    [ProtoContract]
    [Serializable]
    public class Model_JUziDB
    {
        /// <summary>
        /// 句子库模型
        /// </summary>
        public Model_JUziDB() { }
        private string _dbID;
        /// <summary>
        /// 句子库ID
        /// </summary>
         [ProtoMember(1)]
        public string dbID
        {
            set { _dbID = value; }
            get { return _dbID; }
        }


        private string _dbName;
        /// <summary>
        /// 句子库名称
        /// </summary>
        [ProtoMember(2)]
        public string dbName
        {
            set { _dbName = value; }
            get { return _dbName; }
        }

        private string _dbType;
        /// <summary>
        /// 句子库组别
        /// </summary>
        [ProtoMember(3)]
        public string dbType
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
