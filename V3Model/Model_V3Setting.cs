using System;
using System.Collections.Generic;

using System.Text;
using System.Collections;
using System.Threading;
using System.Drawing;
using MyProto;

namespace Model
{
    [ProtoContract]
    [Serializable]
    public class Model_V3Setting
    {
        #region 主要信息
        private bool _showdebugmsg = false;
        private int _pagenumber = 100;
        private int _GetTimeOut = 30;
        private int _PostTimeOut = 30;
        private bool _PostOnGzip = false;
        private string _lastTypdId = "";
        private Dictionary<string, string> _SPgroup = new Dictionary<string, string>();
        private bool _selectedquit = false;
        private bool _isautoupdate = false;
        private string laiyuan = "未知";
        private int jieduanbiaoti = 5;
        private string connstr = "";
        private string dbip = "";
        private string dbname = "";
        private string dbuname = "";
        private string dbupass = "";
        private bool iswindow = false;
        private bool isfirst = false;
        private int maxSendThread = 9999;


        private int  isshowad = 1;
          [ProtoMember(1)]
        public int Isshowad
        {
            get { return isshowad; }
            set { isshowad = value; }
        }

        private int _allmodelcount = 0;
        /// <summary>
        /// 所有模块数量
        /// </summary>
          [ProtoMember(2)]
        public int Allmodelcount
        {
            get { return _allmodelcount; }
            set { _allmodelcount = value; }
        }

          [ProtoMember(3)]
        public int MaxSendThread
        {
            get { return maxSendThread; }
            set { maxSendThread = value; }
        }


        /// <summary>
        /// 是否第一次运行
        /// </summary>
          [ProtoMember(4)]
        public bool Isfirst
        {
            get { return isfirst; }
            set { isfirst = value; }
        }
        
        /// <summary>
        /// 数据库IP
        /// </summary>
          [ProtoMember(5)]
        public string Dbip
        {
            get { return dbip; }
            set { dbip = value; }
        }

        /// <summary>
        /// 数据库名称
        /// </summary>
          [ProtoMember(6)]
        public string Dbname
        {
            get { return dbname; }
            set { dbname = value; }
        }

        /// <summary>
        /// 数据库用户名
        /// </summary>
          [ProtoMember(7)]
        public string Dbuname
        {
            get { return dbuname; }
            set { dbuname = value; }
        }

        /// <summary>
        /// 数据库密码
        /// </summary>
          [ProtoMember(8)]
        public string Dbupass
        {
            get { return dbupass; }
            set { dbupass = value; }
        }

        /// <summary>
        /// 是否是windows验证
        /// </summary>
          [ProtoMember(9)]
        public bool Iswindow
        {
            get { return iswindow; }
            set { iswindow = value; }
        }

        /// <summary>
        /// 数据库连接串
        /// </summary>
          [ProtoMember(10)]
        public string Connstr
        {
            get { return connstr; }
            set { connstr = value; }
        }

        /// <summary>
        /// 截断标题长度
        /// </summary>
          [ProtoMember(11)]
        public int Jieduanbiaoti
        {
            get { return jieduanbiaoti; }
            set { jieduanbiaoti = value; }
        }
        private int jieduanzhengwen = 100;
        /// <summary>
        /// 截断正文长度
        /// </summary>
          [ProtoMember(12)]
        public int Jieduanzhengwen
        {
            get { return jieduanzhengwen; }
            set { jieduanzhengwen = value; }
        }
        /// <summary>
        /// 来源
        /// </summary>
          [ProtoMember(13)]
        public string Laiyuan
        {
            get { return laiyuan; }
            set { laiyuan = value; }
        }
        private string zuozhe = "";
        /// <summary>
        /// 作者
        /// </summary>
          [ProtoMember(14)]
        public string Zuozhe
        {
            get { return zuozhe; }
            set { zuozhe = value; }
        }

        /// <summary>
        /// 自动刷新任务信息
        /// </summary>
          [ProtoMember(15)]
        public bool Isautoupdate
        {
            get { return _isautoupdate; }
            set { _isautoupdate = value; }
        }
        /// <summary>
        /// 选中标签后立即退出
        /// </summary>
          [ProtoMember(16)]
        public bool Selectedquit
        {
            get { return _selectedquit; }
            set { _selectedquit = value; }
        }

        /// <summary>
        /// post时压缩数据
        /// </summary>
          [ProtoMember(17)]
        public bool PostOnGzip
        {
            get { return _PostOnGzip; }
            set { _PostOnGzip = value; }
        }
       

        /// <summary>
        /// get超时时间
        /// </summary>
          [ProtoMember(18)]
        public int GetTimeOut
        {
            set { _GetTimeOut = value; }
            get { return _GetTimeOut; }
        }

        /// <summary>
        /// post超时时间
        /// </summary>
          [ProtoMember(19)]
        public int PostTimeOut
        {
            set { _PostTimeOut = value; }
            get { return _PostTimeOut; }
        }


        /// <summary>
        /// 每页数量
        /// </summary>
          [ProtoMember(20)]
        public int PageNumber
        {
            set { _pagenumber = value; }
            get { return _pagenumber; }
        }

        /// <summary>
        /// 最近一次设置的分组
        /// </summary>
          [ProtoMember(21)]
        public string LastTypdId
        {
            set { _lastTypdId = value; }
            get { return _lastTypdId; }
        }



        /// <summary>
        /// 发布点分组信息
        /// </summary>
          [ProtoMember(22)]
        public Dictionary<string, string> GroupList
        {
            set { _SPgroup = value; }
            get { return _SPgroup; }
        }


          private List<string> minganwords = new List<string>();
          [ProtoMember(23)]
          public List<string> Minganwords
        {
            get { return minganwords; }
            set { minganwords = value; }
        }

        private Dictionary<string, Queue> spiderque = new Dictionary<string, Queue>();
        /// <summary>
        /// 蜘蛛队列缓存
        /// </summary>
        
        public Dictionary<string, Queue> Spiderque
        {
            get { return spiderque; }
            set { spiderque = value; }
        }

     

        private Int32 startjiange = 0;
        /// <summary>
        /// 任务启动间隔
        /// </summary>
          [ProtoMember(27)]
        public Int32 Startjiange
        {
            get { return startjiange; }
            set { startjiange = value; }
        }
        #endregion

        private  int lastUid = 0;
        /// <summary>
        /// 上次的用户ID
        /// </summary>
         [ProtoMember(28)]
        public  int LastUid
        {
            get { return lastUid; }
            set { lastUid = value; }
        }

        private Dictionary<int, Model.GetPostModel> myModel = new Dictionary<int, Model.GetPostModel>();
        [ProtoMember(29)]
        public Dictionary<int, Model.GetPostModel> MyModel
        {
            get { return myModel; }
            set { myModel = value; }
        }

        private Dictionary<string, Model.GetPostModel> myModels = new Dictionary<string, Model.GetPostModel>();
        [ProtoMember(48)]
        public Dictionary<string, Model.GetPostModel> MyModels
        {
            get { return myModels; }
            set { myModels = value; }
        }

        private Dictionary<int, Model.ModelBase> myAllShop1 = new Dictionary<int, Model.ModelBase>();
        [ProtoMember(30)]
        public Dictionary<int, Model.ModelBase> MyAllShoplist
        {
            get { return myAllShop1; }
            set { myAllShop1 = value; }
        }

        private Dictionary<int, Model.ModelBase> myModelHot1001 = new Dictionary<int, Model.ModelBase>();
         [ProtoMember(31)]
        public Dictionary<int, Model.ModelBase> MyModelHot100list
        {
            get { return myModelHot1001; }
            set { myModelHot1001 = value; }
        }

        private Dictionary<int, Model.ModelBase> myModelNew1001 = new Dictionary<int, Model.ModelBase>();
        [ProtoMember(32)]
        public Dictionary<int, Model.ModelBase> MyModelNew100list
        {
            get { return myModelNew1001; }
            set { myModelNew1001 = value; }
        }

        private DateTime lastupdatetime = DateTime.Now;
          [ProtoMember(33)]
        public DateTime Lastupdatetime
        {
            get { return lastupdatetime; }
            set { lastupdatetime = value; }
        }



        #region 最大Id
        private int articledbid = 0;
          [ProtoMember(34)]
        public int Articledbid
        {
            get { return articledbid; }
            set { articledbid = value; }
        }

        private int keyworddbid = 0;
          [ProtoMember(35)]
        public int Keyworddbid
        {
            get { return keyworddbid; }
            set { keyworddbid = value; }
        }


        private int hashdbid = 0;
          [ProtoMember(36)]
        public int Hashdbid
        {
            get { return hashdbid; }
            set { hashdbid = value; }
        }


        private int linkdbid = 0;
          [ProtoMember(37)]
        public int Linkdbid
        {
            get { return linkdbid; }
            set { linkdbid = value; }
        }


        private int replacedbid = 0;
          [ProtoMember(38)]
        public int Replacedbid
        {
            get { return replacedbid; }
            set { replacedbid = value; }
        }

        private int pointid = 0;
          [ProtoMember(39)]
        public int Pointid
        {
            get { return pointid; }
            set { pointid = value; }
        }


        private int taskid = 0;
          [ProtoMember(40)]
        public int Taskid
        {
            get { return taskid; }
            set { taskid = value; }
        }

        #endregion


         [ProtoMember(41)]
        public string SkinName = "Office 2013 Light Gray";
         private string _SkinName
         {
             get { return SkinName; }
             set { SkinName = value; }
         }

        [ProtoMember(42)]
         public int ShowConsole = 0;
        private int _ShowConsole
        {
            get { return ShowConsole; }
            set { ShowConsole = value; }
        }

        [ProtoMember(43)]
        public bool AutoStart = false;
        private bool _AutoStart
        {
            get { return AutoStart; }
            set { AutoStart = value; }
        }

        [ProtoMember(44)]
        public bool AutoRunTask = false;
        private bool _AutoRunTask
        {
            get { return AutoRunTask; }
            set { AutoRunTask = value; }
        }


        /// <summary>
        /// 默认发布点模板
        /// </summary>
        private Dictionary<string, Model.SendPoint> defaultSendPoint = new Dictionary<string, SendPoint>();
        [ProtoMember(45)]
        public Dictionary<string, Model.SendPoint> DefaultSendPoints
        {
            get { return defaultSendPoint; }
            set { defaultSendPoint = value; }
        }


        /// <summary>
        /// 默认任务模板
        /// </summary>
        private Dictionary<string, Model.Task> defaultTask1 = new Dictionary<string, Model.Task>();
        [ProtoMember(46)]
        public Dictionary<string, Model.Task> DefaultTasks
        {
            get { return defaultTask1; }
            set { defaultTask1 = value; }
        }

       
        [ProtoMember(47)]
        private string superDefaultTask = "";
        public string SuperDefaultTask
        {
            get
            {
                return superDefaultTask;
            }

            set
            {
                superDefaultTask = value;
            }
        }


    }
}
