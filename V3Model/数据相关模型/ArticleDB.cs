using System;
using System.Collections.Generic;

using System.Text;
using System.Collections;
using System.Configuration;
using MyProto;

namespace Model
{

    [ProtoContract]
    [Serializable]
    public class ArticleDB
    {
        /// <summary>
        /// 文章库模型
        /// </summary>
        public ArticleDB() { }
        private string _dbID;
        /// <summary>
        /// 文章库ID
        /// </summary>
         [ProtoMember(1)]
        public string Id
        {
            set { _dbID = value; }
            get { return _dbID; }
        }


        private string _dbName;
        /// <summary>
        /// 文章库名称
        /// </summary>
         [ProtoMember(2)]
        public string Name
        {
            set { _dbName = value; }
            get { return _dbName; }
        }

        private string _dbType;
        /// <summary>
        /// 文章库组别
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

        private int maxid = 0;
        public int Maxid
        {
            get
            {
                maxid++;
                return maxid;
            }
        }

        private int maxID = 1;
         [ProtoMember(6)]
        public int MaxID
        {
            get
            {
                return maxID;
            }
             set { maxID = value; }
        }

         private bool _OutPutEncodingGBK = false;
        [ProtoMember(7)]
         public bool OutPutEncodingGBK
         {
             get { return _OutPutEncodingGBK; }
             set { _OutPutEncodingGBK = value; }
         }
         private string _OutPutPath = "c:\\V3Data";
        [ProtoMember(8)]
         public string OutPutPath
         {
             get { return _OutPutPath; }
             set { _OutPutPath = value; }
         }
         private string _OutPutFileName = "【模型值1】.txt";
        [ProtoMember(9)]
         public string OutPutFileName
         {
             get { return _OutPutFileName; }
             set { _OutPutFileName = value; }
         }
         private string _OutPutFileContent = "【模型值2】";
        [ProtoMember(10)]
         public string OutPutFileContent
         {
             get { return _OutPutFileContent; }
             set { _OutPutFileContent = value; }
         }

         private bool _InPutEncodingGBK = false;
        [ProtoMember(11)]
         public bool InPutEncodingGBK
         {
             get { return _InPutEncodingGBK; }
             set { _InPutEncodingGBK = value; }
         }
         private string _InPutPath = "c:\\V3Data";
        [ProtoMember(12)]
         public string InPutPath
         {
             get { return _InPutPath; }
             set { _InPutPath = value; }
         }
         private string _InPutFileName = "【模型值1】";
        [ProtoMember(13)]
         public string InPutFileName
         {
             get { return _InPutFileName; }
             set { _InPutFileName = value; }
         }
         private string _InPutFileContent = "【模型值2】";
        [ProtoMember(14)]
         public string InPutFileContent
         {
             get { return _InPutFileContent; }
             set { _InPutFileContent = value; }
         }

        private bool _SetArticle = false;
         [ProtoMember(15)]
        public bool SetArticle
        {
            get { return _SetArticle; }
            set { _SetArticle = value; }
        }

         private string _DefaultTask = "";
         [ProtoMember(16)]
         public string DefaultTask
         {
             get { return _DefaultTask; }
             set { _DefaultTask = value; }
         } 
    }
}
