using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using MyProto;

namespace Model
{
    [ProtoContract]
    [Serializable]
    public partial class Task
    {
        public Task()
        { }
        #region Model

        #region 任务信息
        private string _canshu = "<wenzhangmoweicharu>不启用|0</wenzhangmoweicharu>\r\n<settags>False|1</settags>\r\n<modifytags>False|1</modifytags>\r\n<suijilink>False|11|0|9|<a href=[链接地址]>[关键词]</a></suijilink>\r\n<zidinglink>False|11|2|。|<a href=[链接地址]>[关键词]</a></zidinglink>\r\n<lianjiesetart>False|1</lianjiesetart>";
        /// <summary>
        /// 定制参数
        /// </summary>
         [ProtoMember(1)]
        public string Canshu
        {
            get { return _canshu; }
            set { _canshu = value; }
        }


        private int _id = 0;
        /// <summary>
        /// 任务id
        /// </summary>
        [ProtoMember(2)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string taskName = "";
        /// <summary>
        /// 任务名称
        /// </summary>
         [ProtoMember(3)]
        public string TaskName
        {
            get { return taskName; }
            set { taskName = value; }
        }
        private int _PointId = 0;
        /// <summary>
        /// 任务所在发布点
        /// </summary>
        [ProtoMember(4)]
        public int PointId
        {
            get { return _PointId; }
            set { _PointId = value; }
        }

        private int takmoshi = 0;
         [ProtoMember(5)]
        public int TaskMoshi
        {
            get { return takmoshi; }
            set { takmoshi = value; }
        }
         private bool isAutoTask = false;
        /// <summary>
        /// 是否是挂机任务
        /// </summary>
         [ProtoMember(6)]
        public bool IsAutoTask
        {
            get { return isAutoTask; }
            set { isAutoTask = value; }
        }

         private List<string> lianjiefus = new List<string>();
         [ProtoMember(7)]
         public List<string> Lianjiefus
        {
            get { return lianjiefus; }
            set { lianjiefus = value; }
        }


        private int jiangetime = 1440;
        /// <summary>
        /// 批次运行间隔
        /// </summary>
         [ProtoMember(8)]
        public int Jiangetime
        {
            get { return jiangetime; }
            set { jiangetime = value; }
        }

        private int articleDbId = 0;
        /// <summary>
        /// 任务所使用的文章库
        /// </summary>
         [ProtoMember(9)]
        public int ArticleDbId
        {
            get { return articleDbId; }
            set { articleDbId = value; }
        }
        private int keywordDbId = 0;
        /// <summary>
        /// 任务所使用的关键词库
        /// </summary>
         [ProtoMember(10)]
        public int KeywordDbId
        {
            get { return keywordDbId; }
            set { keywordDbId = value; }
        }
        private int hashDbId = 0;
        /// <summary>
        /// 任务所使用的哈希库
        /// </summary>
        [ProtoMember(11)]
        public int HashDbId
        {
            get { return hashDbId; }
            set { hashDbId = value; }
        }
        private int replaceDbId = 0;
        /// <summary>
        /// 任务所使用的替换库
        /// </summary>
         [ProtoMember(12)]
        public int ReplaceDbId
        {
            get { return replaceDbId; }
            set { replaceDbId = value; }
        }
        private int linkDbId = 0;
        /// <summary>
        /// 任务所使用的链接库
        /// </summary>
        [ProtoMember(13)]
        public int LinkDbId
        {
            get { return linkDbId; }
            set { linkDbId = value; }
        }
        private int juziDbId = 0;
        /// <summary>
        /// 任务所使用的句子库
        /// </summary>
         [ProtoMember(14)]
        public int JuziDbId
        {
            get { return juziDbId; }
            set { juziDbId = value; }
        }

     

        private String getModel = "";
        /// <summary>
        /// 任务所使用的抓取模块id
        /// </summary>
        [ProtoMember(141)]
        public String GetModel
        {
            get { return getModel; }
            set { getModel = value; }
        }

        private string taskStatus = "状态未知...";
        /// <summary>
        /// 任务运行文本状态
        /// </summary>
        [ProtoMember(16)]
        public string TaskStatusStr
        {
            get { return taskStatus; }
            set { taskStatus = value; }
        }
        private DateTime startTime = DateTime.Now;
        /// <summary>
        /// 任务启动时间
        /// </summary>
         [ProtoMember(17)]
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        private int countAllGet = 0;
        /// <summary>
        /// 累计抓取数
        /// </summary>
         [ProtoMember(18)]
        public int CountAllGet
        {
            get { return countAllGet; }
            set { countAllGet = value; }
        }
        private int countAllPost = 0;
        /// <summary>
        /// 累计发布数
        /// </summary>
        [ProtoMember(19)]
        public int CountAllPost
        {
            get { return countAllPost; }
            set { countAllPost = value; }
        }
        private int countThisGet = 0;
        /// <summary>
        /// 本次累计抓取数
        /// </summary>
         [ProtoMember(20)]
        public int CountThisGet
        {
            get { return countThisGet; }
            set { countThisGet = value; }
        }
        private int countthisPost = 0;
        /// <summary>
        /// 本次累计发布数
        /// </summary>
         [ProtoMember(21)]
        public int CountthisPost
        {
            get { return countthisPost; }
            set { countthisPost = value; }
        }
        private int _taskStatus = 1;
        /// <summary>
        /// 任务运行状态 1，运行中抓取任务 2，运行中发布任务 3，休眠状态 4，停止状态
        /// </summary>
         [ProtoMember(22)]
        public int TaskStatusId
        {
            get { return _taskStatus; }
            set { _taskStatus = value; }
        }
        #endregion

        #region 抓取参数
         private bool isrunget = false;
        /// <summary>
        /// 是否运行抓取过程
        /// </summary>
         [ProtoMember(23)]
        public bool Isrunget
        {
            get { return isrunget; }
            set { isrunget = value; }
        }
        private int hashModel = 1;
        /// <summary>
        /// 哈希判断模式 1标题 2内容 3url
        /// </summary>
         [ProtoMember(24)]
        public int HashModel
        {
            get { return hashModel; }
            set { hashModel = value; }
        }
        private int minTitlestr = 00;
        /// <summary>
        /// 最小抓取标题长度
        /// </summary>
        [ProtoMember(25)]
        public int MinTitlestr
        {
            get { return minTitlestr; }
            set { minTitlestr = value; }
        }
        private int minContentstr = 0;
        /// <summary>
        /// 最小抓取内容长度
        /// </summary>
         [ProtoMember(26)]
        public int MinContentstr
        {
            get { return minContentstr; }
            set { minContentstr = value; }
        }
        private bool getRunModel = false;
        /// <summary>
        /// 抓取触发模式
        /// </summary>
         [ProtoMember(27)]
        public bool GetRunModel
        {
            get { return getRunModel; }
            set { getRunModel = value; }
        }

        private int getPiciNumber = 3;
        /// <summary>
        /// 抓取触发批次
        /// </summary>
         [ProtoMember(28)]
        public int GetPiciNumber
        {
            get { return getPiciNumber; }
            set { getPiciNumber = value; }
        }

        #region 关键字智能抓取


        private int get1_jiangetime = 0;
        /// <summary>
        /// 抓取间隔
        /// </summary>
        [ProtoMember(29)]
        public int Get1_jiangetime
        {
            get { return get1_jiangetime; }
            set { get1_jiangetime = value; }
        }

        private int get1_jingduModel = 0;
        /// <summary>
        /// 抓取精度模式
        /// </summary>
        [ProtoMember(30)]
        public int Get1_jingduModel
        {
            get { return get1_jingduModel; }
            set { get1_jingduModel = value; }
        }
        private string[] get1_jingdukeyword = new string[0];
        /// <summary>
        /// 精度种子关键字
        /// </summary>
        [ProtoMember(31)]
        public string[] Get1_jingdukeyword
        {
            get { return get1_jingdukeyword; }
            set { get1_jingdukeyword = value; }
        }

        private bool get1_isdownpic = false;
        /// <summary>
        /// 是否下载图片
        /// </summary>
        [ProtoMember(32)]
        public bool Get1_isdownpic
        {
            get { return get1_isdownpic; }
            set { get1_isdownpic = value; }
        }

        private int get1_getnumber = 999;
        /// <summary>
        /// 每关键词抓取量
        /// </summary>
         [ProtoMember(33)]
        public int Get1_getnumber
        {
            get { return get1_getnumber; }
            set { get1_getnumber = value; }
        }


        #endregion

        #region 自定义抓取


        private int zidingyi_get1_jiangetime = 0;
        /// <summary>
        /// 抓取间隔
        /// </summary>
         [ProtoMember(34)]
        public int zidingyi_Get1_jiangetime
        {
            get { return zidingyi_get1_jiangetime; }
            set { zidingyi_get1_jiangetime = value; }
        }



        private int zidingyi_total_getnumber = 1000;
         [ProtoMember(35)]
        public int Zidingyi_total_getnumber
        {
            get { return zidingyi_total_getnumber; }
            set { zidingyi_total_getnumber = value; }
        }


        private int zidingyi_totalpage = 100;
         [ProtoMember(36)]
        public int Zidingyi_Totalpage
        {
            get { return zidingyi_totalpage; }
            set { zidingyi_totalpage = value; }
        }


        #endregion

        #region 蜘蛛爬行模式
        private int spider_jiange = 0;
        /// <summary>
        /// 蜘蛛模式抓取间隔
        /// </summary>
         [ProtoMember(37)]
        public int Spider_jiange
        {
            get { return spider_jiange; }
            set { spider_jiange = value; }
        }

        private int spider_maxget = 99999999;
        /// <summary>
        /// 蜘蛛抓取模式最大抓取量
        /// </summary>
         [ProtoMember(38)]
        public int Spider_maxget
        {
            get { return spider_maxget; }
            set { spider_maxget = value; }
        }

        #endregion

        #region 同步追踪
        private int tongbu_get1_jiangetime = 0;
        /// <summary>
        /// 抓取间隔
        /// </summary>
        [ProtoMember(39)]
        public int Tongbu_get1_jiangetime
        {
            get { return tongbu_get1_jiangetime; }
            set { tongbu_get1_jiangetime = value; }
        }
        /// <summary>
        /// 每次更新间隔，秒为单位
        /// </summary>
        private int tongbu_ZhiXingJianGe = 600;
         [ProtoMember(40)]
        public int Tongbu_ZhiXingJianGe
        {
            get { return tongbu_ZhiXingJianGe; }
            set { tongbu_ZhiXingJianGe = value; }
        }

        #endregion


        private bool isUseKu = false;
         [ProtoMember(41)]
        public bool IsUseKu
        {
            get { return isUseKu; }
            set { isUseKu = value; }
        }


        private string yuLiaoKuPath = "";
         [ProtoMember(42)]
        public string YuLiaoKuPath
        {
            get { return yuLiaoKuPath; }
            set { yuLiaoKuPath = value; }
        }

        private bool yulLiaoMoShi = false;
         [ProtoMember(43)]
        public bool YulLiaoMoShi
        {
            get { return yulLiaoMoShi; }
            set { yulLiaoMoShi = value; }
        }

        private int yuJuShuLiang = 1;
         [ProtoMember(44)]
        public int YuJuShuLiang
        {
            get { return yuJuShuLiang; }
            set { yuJuShuLiang = value; }
        }

        private int yuLiaoYingShen = 0;
         [ProtoMember(45)]
        public int YuLiaoYingShen
        {
            get { return yuLiaoYingShen; }
            set { yuLiaoYingShen = value; }
        }

        private int YuLiaoMax = 0;
         [ProtoMember(46)]
        public int YuLiaoMax1
        {
            get { return YuLiaoMax; }
            set { YuLiaoMax = value; }
        }

        private bool _getmajiamodel = false;
        /// <summary>
        /// 登录马甲使用模式
        /// </summary>
         [ProtoMember(47)]
        public bool Getmajiamodel
        {
            get { return _getmajiamodel; }
            set { _getmajiamodel = value; }
        }

        private string _getmajia = "";
         [ProtoMember(48)]
        public string Getmajia
        {
            get { return _getmajia; }
            set { _getmajia = value; }
        }
        #endregion

    

        #region 发布参数
        private bool isUseMakeHtml = false;
        /// <summary>
        /// 是否使用生成html功能
        /// </summary>
        [ProtoMember(49)]
        public bool IsUseMakeHtml
        {
            get { return isUseMakeHtml; }
            set { isUseMakeHtml = value; }
        }
        private bool isUseLinkDb = false;
        /// <summary>
        /// 是否使用链轮功能
        /// </summary>
         [ProtoMember(50)]
        public bool IsUseLinkDb
        {
            get { return isUseLinkDb; }
            set { isUseLinkDb = value; }
        }
        private string sendclass = "";
        /// <summary>
        /// 发布分类
        /// </summary>
         [ProtoMember(51)]
        public string Sendclass
        {
            get { return sendclass; }
            set { sendclass = value; }
        }
        private bool moreAccountUseModel = false;
        /// <summary>
        /// 多账号使用模式
        /// </summary>
         [ProtoMember(52)]
        public bool MoreAccountUseModel
        {
            get { return moreAccountUseModel; }
            set { moreAccountUseModel = value; }
        }
        private int dataGetmodel = 3;
        /// <summary>
        /// 数据取值模式
        /// </summary>
         [ProtoMember(53)]
        public int DataGetmodel
        {
            get { return dataGetmodel; }
            set { dataGetmodel = value; }
        }

        private bool isrunsend = false;
        /// <summary>
        /// 是否运行发布过程
        /// </summary>
         [ProtoMember(54)]
        public bool Isrunsend
        {
            get { return isrunsend; }
            set { isrunsend = value; }
        }
        private int picinumber = 30;
        /// <summary>
        /// 每批次发布量
        /// </summary>
         [ProtoMember(55)]
        public int Picinumber
        {
            get { return picinumber; }
            set { picinumber = value; }
        }


        /// <summary>
        /// 敏感字符处理方式
        /// </summary>
        private string mingan = "不过滤";
         [ProtoMember(56)]
        public string Mingan
        {
            get { return mingan; }
            set { mingan = value; }
        }
        /// <summary>
        /// html标签处理方式
        /// </summary>
        private bool iszhenggui = false;
         [ProtoMember(57)]
        public bool Iszhenggui
        {
            get { return iszhenggui; }
            set { iszhenggui = value; }
        }
        /// <summary>
        /// 是否替换内容
        /// </summary>
        private bool istihuan = false;
         [ProtoMember(58)]
        public bool Istihuan
        {
            get { return istihuan; }
            set { istihuan = value; }
        }

        /// <summary>
        /// 是否内容混淆
        /// </summary>
        private bool ishunxiao = false;
         [ProtoMember(59)]
        public bool Ishunxiao
        {
            get { return ishunxiao; }
            set { ishunxiao = value; }
        }

        private string hunxiaofangshi = "内容混淆关键字模式";
         [ProtoMember(60)]
        public string Hunxiaofangshi
        {
            get { return hunxiaofangshi; }
            set { hunxiaofangshi = value; }
        }


        private string hunxiaogeshi = "6-3|<B>[$素材$(1)]</B>";
         [ProtoMember(61)]
        public string Hunxiaogeshi
        {
            get { return hunxiaogeshi; }
            set { hunxiaogeshi = value; }
        }
        private bool _FormatLinkURL = false;
        private bool _FormatPicURL = false;
        private bool _FormatSuojin = false;
        private bool _FormatHanggao = false;
        /// <summary>
        /// 正规化时缩进
        /// </summary>
         [ProtoMember(62)]
        public bool FormatSuojin
        {
            get { return _FormatSuojin; }
            set { _FormatSuojin = value; }
        }

        /// <summary>
        /// 正规化时调整行高
        /// </summary>
        [ProtoMember(63)]
        public bool FormatHanggao
        {
            get { return _FormatHanggao; }
            set { _FormatHanggao = value; }
        }

        /// <summary>
        /// 正规化时处理图片路径
        /// </summary>
         [ProtoMember(64)]
        public bool FormatPicURL
        {
            get { return _FormatPicURL; }
            set { _FormatPicURL = value; }
        }

        /// <summary>
        /// 正规化时处理链接为绝对路径
        /// </summary>
        [ProtoMember(65)]
        public bool FormatLinkURL
        {
            get { return _FormatLinkURL; }
            set { _FormatLinkURL = value; }
        }
        private string maostring = "<a href=[链接地址]>[标题]</a>";
         [ProtoMember(66)]
        public string Maostring
        {
            get { return maostring; }
            set { maostring = value; }
        }
        private bool isMaoTiHuan = false;
         [ProtoMember(67)]
        public bool IsMaoTiHuan
        {
            get { return isMaoTiHuan; }
            set { isMaoTiHuan = value; }
        }

        private int maoshulian = 1;
         [ProtoMember(68)]
        public int Maoshulian
        {
            get { return maoshulian; }
            set { maoshulian = value; }
        }

        private bool maouseDefaultlinkDb = false;
         [ProtoMember(69)]
        public bool MaouseDefaultlinkDb
        {
            get { return maouseDefaultlinkDb; }
            set { maouseDefaultlinkDb = value; }
        }
        private int maoxiangtong = 0;
         [ProtoMember(70)]
        public int Maoxiangtong
        {
            get { return maoxiangtong; }
            set { maoxiangtong = value; }
        }
        private int maolinkDbId = 0;
         [ProtoMember(71)]
        public int MaolinkDbId
        {
            get { return maolinkDbId; }
            set { maolinkDbId = value; }
        }

        /// <summary>
        /// 关键词模式，true：总数限制模式，false：间隔限制模式
        /// </summary>
        private bool guanjiancimoshi = false;
         [ProtoMember(72)]
        public bool Guanjiancimoshi
        {
            get { return guanjiancimoshi; }
            set { guanjiancimoshi = value; }
        }

        /// <summary>
        /// 关键词总数
        /// </summary>
        private int keywordtotal = 10;
         [ProtoMember(73)]
        public int Keywordtotal
        {
            get { return keywordtotal; }
            set { keywordtotal = value; }
        }

        /// <summary>
        /// 间隔数，以字符个数为单位
        /// </summary>
        private int keywordjiange = 300;
         [ProtoMember(74)]
        public int Keywordjiange
        {
            get { return keywordjiange; }
            set { keywordjiange = value; }
        }
        /// <summary>
        /// 是否过滤QQ
        /// </summary>
        private bool noQQ = false;
         [ProtoMember(75)]
        public bool NoQQ
        {
            get { return noQQ; }
            set { noQQ = value; }
        }
        private string myQQ = "";
         [ProtoMember(76)]
        public string MyQQ
        {
            get { return myQQ; }
            set { myQQ = value; }
        }

        /// <summary>
        /// 是否过滤邮箱
        /// </summary>
        private bool noEmail = false;
         [ProtoMember(77)]
        public bool NoEmail
        {
            get { return noEmail; }
            set { noEmail = value; }
        }
        private string myEmail = "";
         [ProtoMember(78)]
        public string MyEmail
        {
            get { return myEmail; }
            set { myEmail = value; }
        }

        private int fabujiange = 0;
         [ProtoMember(79)]
        public int Fabujiange
        {
            get { return fabujiange; }
            set { fabujiange = value; }
        }

        /// <summary>
        /// 是否过滤a标签
        /// </summary>
        private bool noA = false;
         [ProtoMember(80)]
        public bool NoA
        {
            get { return noA; }
            set { noA = value; }
        }
        private string myA = "";
         [ProtoMember(81)]
        public string MyA
        {
            get { return myA; }
            set { myA = value; }
        }

        /// <summary>
        /// 是否过滤手机
        /// </summary>
        private bool noPhone = false;
         [ProtoMember(82)]
        public bool NoPhone
        {
            get { return noPhone; }
            set { noPhone = value; }
        }
        private string myPhone = "";
         [ProtoMember(83)]
        public string MyPhone
        {
            get { return myPhone; }
            set { myPhone = value; }
        }


        /// <summary>
        /// 是否过滤图片
        /// </summary>
        private bool noPic = false;
         [ProtoMember(84)]
        public bool NoPic
        {
            get { return noPic; }
            set { noPic = value; }
        }
        private bool noUrl = false;
         [ProtoMember(85)]
        public bool NoUrl
        {
            get { return noUrl; }
            set { noUrl= value; }
        }
        private string myUrl = "";
         [ProtoMember(86)]
        public string  MyUrl
        {
            get { return myUrl; }
            set { myUrl = value; }
        }
        private string myPic = "";
         [ProtoMember(87)]
        public string MyPic
        {
            get { return myPic; }
            set { myPic = value; }
        }

        /// <summary>
        /// 伪原创度
        /// </summary>
        private string weiyuanchuangDu = "不进行原创处理";
         [ProtoMember(88)]
        public string WeiyuanchuangDu
        {
            get { return weiyuanchuangDu; }
            set { weiyuanchuangDu = value; }
        }

        /// <summary>
        /// 伪原创选项
        /// </summary>
        private bool[] weiyuanchuangXiang;
         [ProtoMember(89)]
        public bool[] WeiyuanchuangXiang
        {
            get { return weiyuanchuangXiang; }
            set { weiyuanchuangXiang = value; }
        }


        /// <summary>
        /// 伪原创标题选项
        /// </summary>
        private string weiyuanchuangTitle = "[原标题]";
         [ProtoMember(90)]
        public string WeiyuanchuangTitle
        {
            get { return weiyuanchuangTitle; }
            set { weiyuanchuangTitle = value; }
        }

        private string fanyiformate = "中文→";
         [ProtoMember(91)]
        public string Fanyiformate
        {
            get { return fanyiformate; }
            set { fanyiformate = value; }
        }

        private bool istitleFanYi = false;
         [ProtoMember(92)]
        public bool IstitleFanYi
        {
            get { return istitleFanYi; }
            set { istitleFanYi = value; }
        }


        private string articleTou = "";
         [ProtoMember(93)]
        public string ArticleTou
        {
            get { return articleTou; }
            set { articleTou = value; }
        }

        private string articleWei = "";
         [ProtoMember(94)]
        public string ArticleWei
        {
            get { return articleWei; }
            set { articleWei = value; }
        }

        /// <summary>
        /// 是否启用链轮
        /// </summary>
        private bool isLianLun = false;
         [ProtoMember(95)]
        public bool IsLianLun
        {
            get { return isLianLun; }
            set { isLianLun = value; }
        }

        /// <summary>
        /// 是否是否
        /// </summary>
        private bool userDefaultLinkDB = false;
         [ProtoMember(96)]
        public bool UserDefaultLinkDB
        {
            get { return userDefaultLinkDB; }
            set { userDefaultLinkDB = value; }
        }


        private int otherLinkDB = 0;
         [ProtoMember(97)]
        public int OtherLinkDB
        {
            get { return otherLinkDB; }
            set { otherLinkDB = value; }
        }

        private bool isliantou = false;
         [ProtoMember(98)]
        public bool Isliantou
        {
            get { return isliantou; }
            set { isliantou = value; }
        }

        private int toushuliang = 1;
         [ProtoMember(99)]
        public int Toushuliang
        {
            get { return toushuliang; }
            set { toushuliang = value; }
        }

        private string toustring = "<a href=[链接地址]>[标题]</a>";
         [ProtoMember(100)]
        public string Toustring
        {
            get { return toustring; }
            set { toustring = value; }
        }


        private bool islianzhong = false;
         [ProtoMember(101)]
        public bool Islianzhong
        {
            get { return islianzhong; }
            set { islianzhong = value; }
        }

        private bool lunjiangemoshi = false;
         [ProtoMember(102)]
        public bool Lunjiangemoshi
        {
            get { return lunjiangemoshi; }
            set { lunjiangemoshi = value; }
        }

        private int lunzongshu = 10;
         [ProtoMember(103)]
        public int Lunzongshu
        {
            get { return lunzongshu; }
            set { lunzongshu = value; }
        }
        private int zhongjiange = 10;
         [ProtoMember(104)]
        public int Zhongjiange
        {
            get { return zhongjiange; }
            set { zhongjiange = value; }
        }
        private string zhongstring = "<a href=[链接地址]>[标题]</a>";
         [ProtoMember(105)]
        public string Zhongstring
        {
            get { return zhongstring; }
            set { zhongstring = value; }
        }


       
        private bool islianwei = false;
         [ProtoMember(106)]
        public bool Islianwei
        {
            get { return islianwei; }
            set { islianwei = value; }
        }

        private int weishuliang =1;
         [ProtoMember(107)]
        public int Weishuliang
        {
            get { return weishuliang; }
            set { weishuliang = value; }
        }

        private string weistring = "<a href=[链接地址]>[标题]</a>";
         [ProtoMember(108)]
        public string Weistring
        {
            get { return weistring; }
            set { weistring = value; }
        }


        private bool iszidingLink = false;
         [ProtoMember(109)]
        public bool IszidingLink
        {
            get { return iszidingLink; }
            set { iszidingLink = value; }
        }

        private int zidingLinkDb = 0;
         [ProtoMember(110)]
        public int ZidingLinkDb
        {
            get { return zidingLinkDb; }
            set { zidingLinkDb = value; }
        }


        private bool zidingtou = false;
         [ProtoMember(111)]
        public bool Zidingtou
        {
            get { return zidingtou; }
            set { zidingtou = value; }
        }

        private int zidingyitoushulian = 1;
         [ProtoMember(112)]
        public int Zidingyitoushulian
        {
            get { return zidingyitoushulian; }
            set { zidingyitoushulian = value; }
        }

        private string zidingtoustring = "<a href=[链接地址]>[标题]</a>";
         [ProtoMember(113)]
        public string Zidingtoustring
        {
            get { return zidingtoustring; }
            set { zidingtoustring = value; }
        }

        private bool zidingwei = false;
         [ProtoMember(114)]
        public bool Zidingwei
        {
            get { return zidingwei; }
            set { zidingwei = value; }
        }

        private int zidingweishuliang = 1;
         [ProtoMember(115)]
        public int Zidingweishuliang
        {
            get { return zidingweishuliang; }
            set { zidingweishuliang = value; }
        }


        private string zidingweistring = "<a href=[链接地址]>[标题]</a>";
         [ProtoMember(116)]
        public string Zidingweistring
        {
            get { return zidingweistring; }
            set { zidingweistring = value; }
        }

        private bool zidingzhong = false;
         [ProtoMember(117)]
        public bool Zidingzhong
        {
            get { return zidingzhong; }
            set { zidingzhong = value; }
        }

        private bool zidingjiangemoshi = false;
         [ProtoMember(118)]
        public bool Zidingjiangemoshi
        {
            get { return zidingjiangemoshi; }
            set { zidingjiangemoshi = value; }
        }

        private int zidingzhongjiange = 100;
         [ProtoMember(119)]
        public int Zidingzhongjiange
        {
            get { return zidingzhongjiange; }
            set { zidingzhongjiange = value; }
        }

        private int zidingzhongshuliang = 1;
         [ProtoMember(120)]
        public int Zidingzhongshuliang
        {
            get { return zidingzhongshuliang; }
            set { zidingzhongshuliang = value; }
        }

        private string zidingzhongstring = "<a href=[链接地址]>[标题]</a>";
         [ProtoMember(121)]
        public string Zidingzhongstring
        {
            get { return zidingzhongstring; }
            set { zidingzhongstring = value; }
        }


        private bool isshuangxiang = false;
         [ProtoMember(122)]
        public bool Isshuangxiang
        {
            get { return isshuangxiang; }
            set { isshuangxiang = value; }
        }
        private bool qianchuorhouchu = false;
         [ProtoMember(123)]
        public bool Qianchuorhouchu
        {
            get { return qianchuorhouchu; }
            set { qianchuorhouchu = value; }
        }
        private bool isjianfan = false;
         [ProtoMember(124)]
        public bool Isjianfan
        {
            get { return isjianfan; }
            set { isjianfan = value; }
        }

        private bool jianfanfangshi = false;
         [ProtoMember(125)]
        public bool Jianfanfangshi
        {
            get { return jianfanfangshi; }
            set { jianfanfangshi = value; }
        }
        /// <summary>
        /// 是否随机插入
        /// </summary>
        private bool issuijilian = false;
         [ProtoMember(126)]
        public bool Issuijilian
        {
            get { return issuijilian; }
            set { issuijilian = value; }
        }

        private int suijimax = 1;
         [ProtoMember(127)]
        public int Suijimax
        {
            get { return suijimax; }
            set { suijimax = value; }
        }

        private int suijimin = 1;
         [ProtoMember(128)]
        public int Suijimin
        {
            get { return suijimin; }
            set { suijimin = value; }
        }



        private int bencifabu = 0;
         [ProtoMember(129)]
        public int Bencifabu
        {
            get { return bencifabu; }
            set { bencifabu = value; }
        }


        private int suijirandom = 0;
         [ProtoMember(130)]
        public int Suijirandom
        {
            get { return suijirandom; }
            set { suijirandom = value; }
        }

         private List<string> links = new List<string>();
         [ProtoMember(131)]
         public List<string> Links
        {
            get { return links; }
            set { links = value; }
        }

   

        #endregion


        #region 任务变量
        private List<string> _rukou = new List<string>();
         [ProtoMember(132)]
        public List<string> RukouUrl
        {
            get { return _rukou; }
            set { _rukou = value; }
        }
        [ProtoMember(133)]
         private List<string> plugins = new List<string>();

         public List<string> Plugins
         {
             get { return plugins; }
             set { plugins = value; }
         }
         private Dictionary<string, string[]> _PluginParameters = new Dictionary<string, string[]>();
         [ProtoMember(134)]
         public Dictionary<string, string[]> PluginParameters
         {
             get { return _PluginParameters; }
             set { _PluginParameters = value; }
         }

         private List<string> plugins_before = new List<string>();
         [ProtoMember(135)]
         public List<string> Plugins_before
         {
             get { return plugins_before; }
             set { plugins_before = value; }
         }

        private int _YuanChuangJianGe = 0;
        [ProtoMember(136)]
        public int YuanChuangJianGe
        {
            get
            {
                return _YuanChuangJianGe;
            }

            set
            {
                _YuanChuangJianGe = value;
            }
        }
        public int YuanChuangJianGe_Count = 0;

        private bool _FormateHtmlCode = false;
        [ProtoMember(137)]
        public bool FormateHtmlCode
        {
            get { return _FormateHtmlCode; }
            set { _FormateHtmlCode = value; }
        }


        private bool _ClearHashDb = false;
        [ProtoMember(140)]
        public bool ClearHashDb
        {
            get { return _ClearHashDb; }
            set { _ClearHashDb = value; }
        }

        #endregion
        #endregion Model
    }
}
