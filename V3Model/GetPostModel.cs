using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using MyProto;

namespace Model
{
    [ProtoContract]
    [Serializable]
    public class GetPostModel
    {
        #region 模块信息
        /// <summary>
        /// 模块名称
        /// </summary>
        private string _PlanName = "";
        [ProtoMember(1)]
        public string PlanName
        {
            get { return _PlanName; }
            set { _PlanName = value; }
        }
        /// <summary>
        /// 模块说明
        /// </summary>
        private string _PlanReadme = "";
         [ProtoMember(2)]
        public string PlanReadme
        {
            get { return _PlanReadme; }
            set { _PlanReadme = value; }
        }
        /// <summary>
        /// 模块说明
        /// </summary>
        private string _PlanDescripton = "";
         [ProtoMember(3)]
        public string PlanDescripton
        {
            get { return _PlanDescripton; }
            set { _PlanDescripton = value; }
        }
        /// <summary>
        /// 模块设计者名称
        /// </summary>
        private string _PlanDesignName = "";
         [ProtoMember(4)]
        public string PlanDesignName
        {
            get { return _PlanDesignName; }
            set { _PlanDesignName = value; }
        }
        /// <summary>
        /// 模块交流地址
        /// </summary>
         private string _PlanUrl = "http://www.baidu.com";
         [ProtoMember(5)]
        public string PlanUrl
        {
            get { return _PlanUrl; }
            set { _PlanUrl = value; }
        }
        /// <summary>
        /// 模块模式 1关键词智能提取模式 2自定义抓取模式 3蜘蛛爬行模式 4同步追踪模式
        /// </summary>
        private int _PlanModel = 1;
         [ProtoMember(6)]
        public int PlanModel
        {
            get { return _PlanModel; }
            set { _PlanModel = value; }
        }
        /// <summary>
        /// 是否是共享模块
        /// </summary>
        private bool _IsShareModel = false;
         [ProtoMember(7)]
        public bool IsShareModel
        {
            get { return _IsShareModel; }
            set { _IsShareModel = value; }
        }
        /// <summary>
        /// 共享收费等级
        /// </summary>
        private int _ShareLevel = 0;
         [ProtoMember(8)]
        public int ShareLevel
        {
            get { return _ShareLevel; }
            set { _ShareLevel = value; }
        }
        /// <summary>
        /// 最近修改时间
        /// </summary>
        private string _UpdateTime = "未知";
         [ProtoMember(9)]
        public string UpdateTime
        {
            get { return _UpdateTime; }
            set { _UpdateTime = value; }
        }
        /// <summary>
        /// 是否是发布模块
        /// </summary>
        private bool _isPostModel = false;
         [ProtoMember(10)]
        public bool isPostModel
        {
            get { return _isPostModel; }
            set { _isPostModel = value; }
        }
        /// <summary>
        /// 设计者UID
        /// </summary>
        private int _uid = 0;
         [ProtoMember(11)]
        public int uid
        {
            get { return _uid; }
            set { _uid = value; }
        }
        ///// <summary>
        ///// 模块ID
        ///// </summary>
        //private int _mid = 0;
        // [ProtoMember(12)]
        //public int mid
        //{
        //    get { return _mid; }
        //    set { _mid = value; }
        //}

        /// <summary>
        /// 模块唯一ID
        /// </summary>
        private string _mids = "";
        [ProtoMember(87)]
        public string mids
        {
            get { return _mids; }
            set { _mids = value; }
        }

        private string testadminurl = "";
        /// <summary>
        /// 测试发布后台地址
        /// </summary>
        [ProtoMember(13)]
        public string Testadminurl
        {
            get { return testadminurl; }
            set { testadminurl = value; }
        }

        private string userAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
         [ProtoMember(14)]
        public string UserAgent
        {
            get { return userAgent; }
            set { userAgent = value; }
        }

        #endregion

        #region 抓取模块
        private string _GetMajia = "c=1";
         [ProtoMember(15)]
        public string GetMajia
        {
            get { return _GetMajia; }
            set { _GetMajia = value; }
        }

        private bool _isuseTaskRukou = false;
        /// <summary>
        /// 是否使用任务参数里提供的入口
        /// </summary>
         [ProtoMember(16)]
        public bool IsuseTaskRukou
        {
            get { return _isuseTaskRukou; }
            set { _isuseTaskRukou = value; }
        }

        #region 第一步
        #region 关键词智能提取模式
        private string _stp1_refrereurl = "";
        /// <summary>
        /// 第一步来路页地址
        /// </summary>
         [ProtoMember(17)]
        public string Stp1_GET_refrereurl
        {
            get { return _stp1_refrereurl; }
            set { _stp1_refrereurl = value; }
        }
        /// <summary>
        /// 提取编码 1gbk 2utf8 3auto
        /// </summary>
        private int _Stp1_Keyword_GetEncoding = 3;
         [ProtoMember(18)]
        public int Stp1_GET_Keyword_GetEncoding
        {
            get { return _Stp1_Keyword_GetEncoding; }
            set { _Stp1_Keyword_GetEncoding = value; }
        }
        /// <summary>
        /// 测试地址
        /// </summary>
         private string _Stp1_Keyword_TestUrl = "https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=0&rsv_idx=1&tn=baidu&wd=[关键词UTF8编码]";
         [ProtoMember(19)]
        public string Stp1_GET_Keyword_TestUrl
        {
            get { return _Stp1_Keyword_TestUrl; }
            set { _Stp1_Keyword_TestUrl = value; }
        }
        /// <summary>
        /// 测试关键字
        /// </summary>
        private string _Stp1_Keyword_TestKeyword = "手机";
         [ProtoMember(20)]
        public string Stp1_GET_Keyword_TestKeyword
        {
            get { return _Stp1_Keyword_TestKeyword; }
            set { _Stp1_Keyword_TestKeyword = value; }
        }

        private bool _Stp1_NeedGetPage = false;
        /// <summary>
        /// 是否需要获取分页
        /// </summary>
         [ProtoMember(21)]
        public bool Stp1_NeedGetPage
        {
            get { return _Stp1_NeedGetPage; }
            set { _Stp1_NeedGetPage = value; }
        }

        private Model.抓取相关模型.RulesEngLv1 _Stp1_PageRules = new 抓取相关模型.RulesEngLv1();

        /// <summary>
        /// 提取分页规则
        /// </summary>
        [ProtoMember(22)]
        public Model.抓取相关模型.RulesEngLv1 Stp1_PageRules
        {
            get { return _Stp1_PageRules; }
            set { _Stp1_PageRules = value; }
        }

        private Int32 _Stp1_PageNumber = 9999;
        /// <summary>
        /// 分页提取数量
        /// </summary>
         [ProtoMember(23)]
        public Int32 Stp1_PageNumber
        {
            get { return _Stp1_PageNumber; }
            set { _Stp1_PageNumber = value; }
        }

        #endregion

        #region 自定义抓取
        //来路
        private string zidingyi_stp1_refrereurl = "";
         [ProtoMember(24)]
        public string Zidingyi_stp1_refrereurl
        {
            get { return zidingyi_stp1_refrereurl; }
            set { zidingyi_stp1_refrereurl = value; }
        }

        /// <summary>
        /// 提取编码 1gbk 2utf8 3auto
        /// </summary>
        private int zidingyi_stp1_GetEncoding = 3;
         [ProtoMember(25)]
        public int Zidingyi_stp1_GetEncoding
        {
            get { return zidingyi_stp1_GetEncoding; }
            set { zidingyi_stp1_GetEncoding = value; }
        }

        /// <summary>
        /// 入口链接列表
        /// </summary>
        private List<string> zidingyi_stp1_RuKouUrls = new List<string>();
         [ProtoMember(26)]
        public List<string> Zidingyi_stp1_RuKouUrls
        {
            get { return zidingyi_stp1_RuKouUrls; }
            set { zidingyi_stp1_RuKouUrls = value; }
        }



        private Model.抓取相关模型.RulesEngLv1 zidingyi_Stp1_PageRules = new 抓取相关模型.RulesEngLv1();
         [ProtoMember(27)]
        public Model.抓取相关模型.RulesEngLv1 Zidingyi_Stp1_PageRules
        {
            get { return zidingyi_Stp1_PageRules; }
            set { zidingyi_Stp1_PageRules = value; }
        }

  

        #endregion

        #region 同步追踪
        //来路
        private string tongbu_stp1_refrereurl = "";
         [ProtoMember(28)]
        public string Tongbu_stp1_refrereurl
        {
            get { return tongbu_stp1_refrereurl; }
            set { tongbu_stp1_refrereurl = value; }
        }

       
        /// <summary>
        /// 提取编码 1gbk 2utf8 3auto
        /// </summary>
        private int tongbu_stp1_GetEncoding = 3;
         [ProtoMember(29)]
        public int Tongbu_stp1_GetEncoding
        {
            get { return tongbu_stp1_GetEncoding; }
            set { tongbu_stp1_GetEncoding = value; }
        }

       
        /// <summary>
        /// 入口链接列表
        /// </summary>
         private List<string> tongbu_stp1_RuKouUrls = new List<string>();
         [ProtoMember(30)]
         public List<string> Tongbu_stp1_RuKouUrls
        {
            get { return tongbu_stp1_RuKouUrls; }
            set { tongbu_stp1_RuKouUrls = value; }
        }

        private Model.抓取相关模型.FinalRules tongbu_Stp2_Rules = new Model.抓取相关模型.FinalRules();
         [ProtoMember(31)]
        public Model.抓取相关模型.FinalRules Tongbu_Stp2_Rules
        {
            get { return tongbu_Stp2_Rules; }
            set { tongbu_Stp2_Rules = value; }
        }

        private string tongbu_stp3_RefrereURL = "";
        /// <summary>
        /// 结果页来路页
        /// </summary>
        [ProtoMember(32)]
        public string Tongbu_Stp3_RefrereURL
        {
            get { return tongbu_stp3_RefrereURL; }
            set { tongbu_stp3_RefrereURL = value; }
        }

 
     


        #endregion

        #region 蜘蛛爬行模式
        private int stp1_spider_getencoding = 3;
        /// <summary>
        /// 蜘蛛爬行模式编码
        /// </summary>
        [ProtoMember(33)]
        public int Stp1_spider_getencoding
        {
            get { return stp1_spider_getencoding; }
            set { stp1_spider_getencoding = value; }
        }

        private List<string> stp1_spider_mainurl = new List<string>();

        /// <summary>
        /// 初始入口地址
        /// </summary>
        [ProtoMember(34)]
        public List<string> Stp1_spider_mainurl
        {
            get { return stp1_spider_mainurl; }
            set { stp1_spider_mainurl = value; }
        }

        private Model.抓取相关模型.RulesEngLv1 stp1_spider_rules = new 抓取相关模型.RulesEngLv1();
        /// <summary>
        /// 内容提取规则
        /// </summary>
        [ProtoMember(35)]
        public Model.抓取相关模型.RulesEngLv1 Stp1_spider_rules
        {
            get { return stp1_spider_rules; }
            set { stp1_spider_rules = value; }
        }
        private string stp1_spider_refererurl = "";
        /// <summary>
        /// 来路页地址
        /// </summary>
        [ProtoMember(36)]
        public string Stp1_spider_refererurl
        {
            get { return stp1_spider_refererurl; }
            set { stp1_spider_refererurl = value; }
        }

        #endregion
        #endregion

        #region 第二步
        #region 关键词智能提取模式
        /// <summary>
        /// 测试提取地址
        /// </summary>
        private string _Stp2_Keyword_TestUrl = "";
         [ProtoMember(37)]
        public string Stp2_GET_Keyword_TestUrl
        {
            get { return _Stp2_Keyword_TestUrl; }
            set { _Stp2_Keyword_TestUrl = value; }
        }
        private Model.抓取相关模型.FinalRules _Stp2_Keyword_Rules = new Model.抓取相关模型.FinalRules();
         [ProtoMember(38)]
        public Model.抓取相关模型.FinalRules Stp2_GET_Keyword_Rules
        {
            get { return _Stp2_Keyword_Rules; }
            set { _Stp2_Keyword_Rules = value; }
        }
        private bool _Stp2_NoNeedStp2 = false;
         [ProtoMember(39)]
        public bool Stp2_GET_NoNeedStp2
        {
            get { return _Stp2_NoNeedStp2; }
            set { _Stp2_NoNeedStp2 = value; }
        }
        #endregion

        #region  自定义抓取模式
        private string zidingyi_Stp2_Keyword_TestUrl = "";
         [ProtoMember(40)]
        public string Zidingyi_Stp2_GET_Keyword_TestUrl
        {
            get { return zidingyi_Stp2_Keyword_TestUrl; }
            set { zidingyi_Stp2_Keyword_TestUrl = value; }
        }
        private Model.抓取相关模型.FinalRules zidingyi_Stp2_Rules = new Model.抓取相关模型.FinalRules();
         [ProtoMember(41)]
        public Model.抓取相关模型.FinalRules Zidingyi_Stp2_GET_Rules
        {
            get { return zidingyi_Stp2_Rules; }
            set { zidingyi_Stp2_Rules = value; }
        }  
        #endregion
        #endregion

        #region 第三步
        /// <summary>
        /// 获取内容的编码
        /// </summary>
        private int getstup3Enc = 3;
         [ProtoMember(42)]
        public int Getstup3Enc
        {
            get { return getstup3Enc; }
            set { getstup3Enc = value; }
        }
        private string _stp3_RefrereURL = "";
        /// <summary>
        /// 结果页来路页
        /// </summary>
        [ProtoMember(43)]
        public string Stp3_RefrereURL
        {
            get { return _stp3_RefrereURL; }
            set { _stp3_RefrereURL = value; }
        }
        /// <summary>
        /// 测试提取地址
        /// </summary>
        private string _Sep3_TestUrl = "";
         [ProtoMember(44)]
        public string Stp3_GET_TestUrl
        {
            get { return _Sep3_TestUrl; }
            set { _Sep3_TestUrl = value; }
        }
        /// <summary>
        /// 提取模式 1规则 2智能
        /// </summary>
        private int _Stp3_GetModel = 2;
         [ProtoMember(45)]
        public int Stp3_GET_GetModel
        {
            get { return _Stp3_GetModel; }
            set { _Stp3_GetModel = value; }
        }
        private Model.抓取相关模型.FinalRules[] _Stp3Rules;
        [ProtoMember(46)]
        public Model.抓取相关模型.FinalRules[] Stp3_GET_Rules
        {
            get { return _Stp3Rules; }
            set { _Stp3Rules = value; }
        }
        private Model.抓取相关模型.RulesEngLv1 _Stp3PublicRules = new Model.抓取相关模型.RulesEngLv1();
        [ProtoMember(47)]
        public Model.抓取相关模型.RulesEngLv1 Stp3_GET_PublicRules
        {
            get { return _Stp3PublicRules; }
            set { _Stp3PublicRules = value; }
        }

        private bool stp3_neemorepage = false;
        /// <summary>
        /// 是否提取分页
        /// </summary>
        [ProtoMember(48)]
        public bool Stp3_neemorepage
        {
            get { return stp3_neemorepage; }
            set { stp3_neemorepage = value; }
        }

        private Model.抓取相关模型.RulesEngLv1 stp3_pagerule = new 抓取相关模型.RulesEngLv1();
         [ProtoMember(49)]
        public Model.抓取相关模型.RulesEngLv1 Stp3_pagerule
        {
            get { return stp3_pagerule; }
            set { stp3_pagerule = value; }
        }
        #endregion

        #region 自定义抓取
        private string zidingyi_stp3_RefrereURL = "";
        /// <summary>
        /// 结果页来路页
        /// </summary>
       [ProtoMember(50)]
        public string Zidingyi_Stp3_RefrereURL
        {
            get { return zidingyi_stp3_RefrereURL; }
            set { zidingyi_stp3_RefrereURL = value; }
        }
        /// <summary>
        /// 测试提取地址
        /// </summary>
        private string zidingyi_Sep3_TestUrl = "";
         [ProtoMember(51)]
        public string Zidingyi_Sep3_GET_TestUrl
        {
            get { return zidingyi_Sep3_TestUrl; }
            set { zidingyi_Sep3_TestUrl = value; }
        }
        /// <summary>
        /// 提取模式 1规则 2智能
        /// </summary>
        private int zidingyi_Stp3_GetModel = 1;
         [ProtoMember(52)]
        public int Zidingyi_Stp3_GET_GetModel
        {
            get { return zidingyi_Stp3_GetModel; }
            set { zidingyi_Stp3_GetModel = value; }
        }

        private Model.抓取相关模型.FinalRules[] zidingyi_Stp3Rules;
         [ProtoMember(53)]
        public Model.抓取相关模型.FinalRules[] Zidingyi_Stp3_GET_Rules
        {
            get { return zidingyi_Stp3Rules; }
            set { zidingyi_Stp3Rules = value; }
        }
        private Model.抓取相关模型.RulesEngLv1 zidingyi_Stp3PublicRules = new Model.抓取相关模型.RulesEngLv1();
         [ProtoMember(54)]
        public Model.抓取相关模型.RulesEngLv1 Zidingyi_Stp3_GET_PublicRules
        {
            get { return zidingyi_Stp3PublicRules; }
            set { zidingyi_Stp3PublicRules = value; }
        }
        #endregion

        #endregion

        #region 发布模块

        #region 登录
        private bool ismajiamodel = false;
        /// <summary>
        /// 是否是马甲模式
        /// </summary>
        [ProtoMember(55)]
        public bool Ismajiamodel
        {
            get { return ismajiamodel; }
            set { ismajiamodel = value; }
        }

        private string majisstr = "";
        /// <summary>
        /// 马甲代码
        /// </summary>
       [ProtoMember(56)]
        public string Majiastr
        {
            get { return majisstr; }
            set { majisstr = value; }
        }

        private string majiaurl = "";
         [ProtoMember(57)]
        public string Majiaurl
        {
            get { return majiaurl; }
            set { majiaurl = value; }
        }

        private Model.发布相关模型.GetPostAction[] _Stp1_LoginAction;
       [ProtoMember(58)]
        public Model.发布相关模型.GetPostAction[] Stp1_POST_LoginAction
        {
            get { return _Stp1_LoginAction; }
            set { _Stp1_LoginAction = value; }
        }

        private bool _Stp1_NeedLogin = false;
        [ProtoMember(59)]
        public bool Stp1_POST_NeedLogin
        {
            get { return _Stp1_NeedLogin; }
            set { _Stp1_NeedLogin = value; }
        }

        private bool _Stp1_NeedLoginMore = false;
        [ProtoMember(60)]
        public bool Stp1_POST_NeedLoginMore
        {
            get { return _Stp1_NeedLoginMore; }
            set { _Stp1_NeedLoginMore = value; }
        }

        private string _Stp1_VcheckcodeUrl = "";
        [ProtoMember(61)]
        public string Stp1_POST_VcheckcodeUrl
        {
            get { return _Stp1_VcheckcodeUrl; }
            set { _Stp1_VcheckcodeUrl = value; }
        }

        private string[] _Stp1_Truetag;
        [ProtoMember(62)]
        public string[] Stp1_POST_Truetag
        {
            get { return _Stp1_Truetag; }
            set { _Stp1_Truetag = value; }
        }

        private string[] _Stp1_Falsetag;
        [ProtoMember(63)]
        public string[] Stp1_POST_Falsetag
        {
            get { return _Stp1_Falsetag; }
            set { _Stp1_Falsetag = value; }
        }

        private Model.发布相关模型.Account _TestAccount = new Model.发布相关模型.Account();

         [ProtoMember(64)]
        public Model.发布相关模型.Account POST_TestAccount
        {
            get { return _TestAccount; }
            set { _TestAccount = value; }
        }

        private bool _Stp1_VcodeModel = false;

        [ProtoMember(65)]
        public bool Stp1_POST_VcodeModel
        {
            get { return _Stp1_VcodeModel; }
            set { _Stp1_VcodeModel = value; }
        }
        #endregion

        #region 分类
        private bool _Stp2_UsedClass = false;
         [ProtoMember(66)]
        public bool Stp2_POST_UsedClass
        {
            get { return _Stp2_UsedClass; }
            set { _Stp2_UsedClass = value; }
        }

         private bool _Stp2_UsedAddClass = false;
         [ProtoMember(67)]
        public bool Stp2_POST_UsedAddClass
        {
            get { return _Stp2_UsedAddClass; }
            set { _Stp2_UsedAddClass = value; }
        }
        private string _Stp2_GetClassRules = "外部标记(?<typeid>.+?)内部标记(?<typename>.+?)尾部标记";
         [ProtoMember(68)]
        public string Stp2_POST_GetClassRules
        {
            get { return _Stp2_GetClassRules; }
            set { _Stp2_GetClassRules = value; }
        }
        private string _Stp2_GetAddOktag = "成功";
         [ProtoMember(69)]
        public string Stp2_POST_GetAddOktag
        {
            get { return _Stp2_GetAddOktag; }
            set { _Stp2_GetAddOktag = value; }
        }
        private Model.发布相关模型.GetPostAction _Stp2_Get = new Model.发布相关模型.GetPostAction();
         [ProtoMember(70)]
        public Model.发布相关模型.GetPostAction Stp2_POST_Get
        {
            get { return _Stp2_Get; }
            set { _Stp2_Get = value; }
        }
        private Model.发布相关模型.GetPostAction _Stp2_Post = new Model.发布相关模型.GetPostAction();
         [ProtoMember(71)]
        public Model.发布相关模型.GetPostAction Stp2_POST_Post
        {
            get { return _Stp2_Post; }
            set { _Stp2_Post = value; }
        }
        #endregion

        #region 发布
        private string _Stp3_VcodeUrl = "";
         [ProtoMember(72)]
        public string Stp3_POST_VcodeUrl
        {
            get { return _Stp3_VcodeUrl; }
            set { _Stp3_VcodeUrl = value; }
        }

        private string[] _Stp3_Truetag;
         [ProtoMember(73)]
        public string[] Stp3_POST_Truetag
        {
            get { return _Stp3_Truetag; }
            set { _Stp3_Truetag = value; }
        }
        private string[] _Stp3_Falsetag ;
         [ProtoMember(74)]
        public string[] Stp3_POST_Falsetag
        {
            get { return _Stp3_Falsetag; }
            set { _Stp3_Falsetag = value; }
        }

        private Model.发布相关模型.GetPostAction[] _Stp3_SendAction;
         [ProtoMember(75)]
        public Model.发布相关模型.GetPostAction[] Stp3_POST_SendAction
        {
            get { return _Stp3_SendAction; }
            set { _Stp3_SendAction = value; }
        }
        private bool _Stp3_NeedMakeHtml = false;
         [ProtoMember(76)]
        public bool Stp3_POST_NeedMakeHtml
        {
            get { return _Stp3_NeedMakeHtml; }
            set { _Stp3_NeedMakeHtml = value; }
        }
        private int _Stp3_makeHtmlCount = 1;
         [ProtoMember(77)]
        public int Stp3_POST_makeHtmlCount
        {
            get { return _Stp3_makeHtmlCount; }
            set { _Stp3_makeHtmlCount = value; }
        }
        private bool _Stp3_SupportLinkDb = false;
         [ProtoMember(78)]
        public bool Stp3_POST_SupportLinkDb
        {
            get { return _Stp3_SupportLinkDb; }
            set { _Stp3_SupportLinkDb = value; }
        }
        private bool _Stp3_LinkGetModel = false;
         [ProtoMember(79)]
        public bool Stp3_POST_LinkGetModel
        {
            get { return _Stp3_LinkGetModel; }
            set { _Stp3_LinkGetModel = value; }
        }
        private string _Stp3_LinkGetUrl = "";
         [ProtoMember(80)]
        public string Stp3_POST_LinkGetUrl
        {
            get { return _Stp3_LinkGetUrl; }
            set { _Stp3_LinkGetUrl = value; }
        }
        private Model.抓取相关模型.RulesEngLv1 _Stp3_GetLinkrules = new Model.抓取相关模型.RulesEngLv1();
         [ProtoMember(81)]
        public Model.抓取相关模型.RulesEngLv1 Stp3_POST_GetLinkrules
        {
            get { return _Stp3_GetLinkrules; }
            set { _Stp3_GetLinkrules = value; }
        }

        private string[] _Stp3_MakeHtmlUrls;
         [ProtoMember(82)]
        public string[] Stp3_POST_MakeHtmlUrls
        {
            get { return _Stp3_MakeHtmlUrls; }
            set { _Stp3_MakeHtmlUrls = value; }
        }
        private bool _Stp3_VcodeModel = false;
         [ProtoMember(83)]
        public bool Stp3_POST_VcodeModel
        {
            get { return _Stp3_VcodeModel; }
            set { _Stp3_VcodeModel = value; }
        }
        #endregion

        #endregion
    }
}
