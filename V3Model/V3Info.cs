using System;
using System.Collections.Generic;
using System.Text;
using Model;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using MyProto;


namespace Model
{
    [ProtoContract]
    [Serializable]
    public  static class V3Infos
    {


        ///// <summary>
        ///// 模块信息
        ///// </summary>
        //private static Dictionary<string, Model.GetPostModel> myModel = new Dictionary<string, Model.GetPostModel>();
        // [ProtoMember(1)]
        //public static Dictionary<string, Model.GetPostModel> MyModel
        //{
        //    get { return myModel; }
        //    set { myModel = value; }
        //}

        /// <summary>
        /// 模块市场
        /// </summary>
        private static Dictionary<int, Model.ModelBase> modelShop = new Dictionary<int, Model.ModelBase>();
         [ProtoMember(2)]
        public static Dictionary<int, Model.ModelBase> ModelShop
        {
            get { return modelShop; }
            set { modelShop = value; }
        }

        /// <summary>
        /// new top100
        /// </summary>
        private static Dictionary<int, Model.ModelBase> modelShopNewTop100 = new Dictionary<int, Model.ModelBase>();
         [ProtoMember(3)]
        public static Dictionary<int, Model.ModelBase> ModelShopNewTop100
        {
            get { return modelShopNewTop100; }
            set { modelShopNewTop100 = value; }
        }

        /// <summary>
        /// hot top100
        /// </summary>
        private static Dictionary<int, Model.ModelBase> modelShopHotTop100 = new Dictionary<int, Model.ModelBase>();
         [ProtoMember(4)]
        public static Dictionary<int, Model.ModelBase> ModelShopHotTop100
        {
            get { return modelShopHotTop100; }
            set { modelShopHotTop100 = value; }
        }

        /// <summary>
        /// 主信息库的内存映像
        /// </summary>
        private static Model_V3Setting _dbsV3 = new Model_V3Setting();
         [ProtoMember(5)]
        public static Model_V3Setting MainDb
        {
            get { return _dbsV3; }
            set { _dbsV3 = value; }
        }

        /// <summary>
        /// 文章库
        /// </summary>
         private static Dictionary<string, Model.ArticleDB> articleDb = new Dictionary<string, ArticleDB>();
         [ProtoMember(6)]
         public static Dictionary<string, Model.ArticleDB> ArticleDb
        {
            get { return articleDb; }
            set { articleDb = value; }
        }

        /// <summary>
        /// 关键词库库
        /// </summary>
        private static Dictionary<string, Model.KeywordDB> keywordDb = new Dictionary<string, KeywordDB>();
         [ProtoMember(7)]
        public static Dictionary<string, Model.KeywordDB> KeywordDb
        {
            get { return keywordDb; }
            set { keywordDb = value; }
        }

        /// <summary>
        /// 哈希库
        /// </summary>
        private static Dictionary<string, Model.HashDB> _hashDb = new Dictionary<string, HashDB>();
         [ProtoMember(8)]
        public static Dictionary<string, Model.HashDB> HashDb
        {
            get { return _hashDb; }
            set { _hashDb = value; }
        }

        /// <summary>
        /// 替换库
        /// </summary>
        private static Dictionary<string, Model.ReplaceDB> replaceDb = new Dictionary<string, ReplaceDB>();
         [ProtoMember(9)]
        public static Dictionary<string, Model.ReplaceDB> ReplaceDb
        {
            get { return replaceDb; }
            set { replaceDb = value; }
        }

        /// <summary>
        /// 链接库
        /// </summary>
        private static Dictionary<string, Model.LinkDB> linkDb = new Dictionary<string, LinkDB>();
         [ProtoMember(10)]
        public static Dictionary<string, Model.LinkDB> LinkDb
        {
            get { return linkDb; }
            set { linkDb = value; }
        }
        /// <summary>

        /// <summary>
        /// 发布点
        /// </summary>
        private static Dictionary<int, Model.SendPoint> sendPointDb = new Dictionary<int, Model.SendPoint>();
         [ProtoMember(11)]
        public static Dictionary<int, Model.SendPoint> SendPointDb
        {
            get { return sendPointDb; }
            set { sendPointDb = value; }
        }

        /// <summary>
        /// 任务库
        /// </summary>
        private static Dictionary<int, Model.Task> taskDb = new Dictionary<int, Model.Task>();
         [ProtoMember(12)]
        public static Dictionary<int, Model.Task> TaskDb
        {
             get
             {
                
                     return taskDb;
                 


             }
             set
             {
                
                     taskDb = value;
                 
             }
        }

        /// <summary>
        /// 任务线程
        /// </summary>
        public static Dictionary<int, System.Threading.Tasks.Task> TaskThread = new Dictionary<int, System.Threading.Tasks.Task>();

        /// <summary>
        /// 任务线程取消标识
        /// </summary>
        public static Dictionary<int, System.Threading.CancellationTokenSource> TaskCancelToken = new Dictionary<int, CancellationTokenSource>();

        /// <summary>
        /// 休眠区任务列表
        /// </summary>
        public static Dictionary<int, DateTime> TaskWaiting = new Dictionary<int, DateTime>();
    }
}
