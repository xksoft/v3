using System;
using System.Collections.Generic;

using System.Text;
using MyProto;

namespace Model.抓取相关模型
{
    [ProtoContract]
    [Serializable]
    public class RulesEngLv1
    {

        #region 预处理
        private bool _isuseYuchuli = false;
        /// <summary>
        /// 是否使用预处理
        /// </summary>
        [ProtoMember(1)]
        public bool IsuseYuchuli
        {
            get { return _isuseYuchuli; }
            set { _isuseYuchuli = value; }
        }

        private bool _ischulijuedui = false;
        /// <summary>
        /// 是否处理绝对路径
        /// </summary>
        [ProtoMember(2)]
        public bool Ischulijuedui
        {
            get { return _ischulijuedui; }
            set { _ischulijuedui = value; }
        }

        private Dictionary<string, string> _replaceData = new Dictionary<string, string>();
        /// <summary>
        /// 替换数据
        /// </summary>
        [ProtoMember(3)]
        public Dictionary<string, string> ReplaceData
        {
            get { return _replaceData; }
            set { _replaceData = value; }
        }

        private bool _istichun = false;
        /// <summary>
        /// 是否启用提纯
        /// </summary>
        [ProtoMember(4)]
        public bool Istichun
        {
            get { return _istichun; }
            set { _istichun = value; }
        }

        private bool _isuseEngreplace = false;
        /// <summary>
        /// 是否使用视觉方式提纯
        /// </summary>
        [ProtoMember(5)]
        public bool IsuseEngreplace
        {
            get { return _isuseEngreplace; }
            set { _isuseEngreplace = value; }
        }

        private RulesEngLv2 tichunrule = new RulesEngLv2();
        /// <summary>
        /// 提纯规则
        /// </summary>
        [ProtoMember(6)]
        public RulesEngLv2 Tichunrule
        {
            get { return tichunrule; }
            set { tichunrule = value; }
        }
        #endregion 

        #region 主处理


        /// <summary>
        /// 规则
        /// </summary>
        public RulesEngLv1() { }
        private string  rulestname="未命名规则";
        [ProtoMember(7)]
        public string RulesName
        {
            set { rulestname = value; }
            get { return rulestname; }
        }

        private int _bianma=3;
        /// <summary>
        /// 抓取编码
        /// </summary>
         [ProtoMember(8)]
        public int Bianma
        {
            set { _bianma = value; }
            get { return _bianma; }
        }
        private int _GetModel=3;
        /// <summary>
        /// 提取模式 1,规则提取 2，提取链接（默认） 3，提取链接（绝对路径）
        /// </summary>
         [ProtoMember(9)]
        public int GetModel
        {
            set { _GetModel = value; }
            get { return _GetModel; }
        }

        private int _OutModel = 1;
        /// <summary>
        /// 输出模式 1，合并所有结果 2，多结果输出 3，取大 4，包含特征字符的
        /// </summary>
        [ProtoMember(10)]
        public int OutModel
        {
            set { _OutModel = value; }
            get { return _OutModel; }
        }

        private string hebingstr = "<BR>";
        /// <summary>
        /// 合并间隔符
        /// </summary>
        [ProtoMember(11)]
        public string HebingStr
        {
            set { hebingstr = value; }
            get { return hebingstr; }
        }

        private RulesEngLv2[] _rules=new RulesEngLv2[0];
         [ProtoMember(12)]
        public RulesEngLv2[] MyRules
        {
            set { _rules = value; }
            get { return _rules; }
        }

        private string testurl = "";
        /// <summary>
        /// 测试地址
        /// </summary>
        [ProtoMember(13)]
        public string TestUrl
        {
            set { testurl = value; }
            get { return testurl; }
        }

        private bool isrgx = false;
         [ProtoMember(14)]
        public bool IsRgx
        {
            set { isrgx = value; }
            get { return isrgx; }
        }

        private bool isformat = false;
        /// <summary>
        /// 是否进行正规化处理
        /// </summary>
         [ProtoMember(15)]
        public bool IsFormat
        {
            set { isformat = value; }
            get { return isformat; }
        }
        private string shengchengtou = "";
         [ProtoMember(16)]
        public string Shengchengtou
        {
            get { return shengchengtou; }
            set { shengchengtou = value; }
        }
        private int toucong = 0;
         [ProtoMember(17)]
        public int Toucong
        {
            get { return toucong; }
            set { toucong = value; }
        }
        private int toudao = 0;
         [ProtoMember(18)]
        public int Toudao
        {
            get { return toudao; }
            set { toudao = value; }
        }
        private int zizeng = 1;
         [ProtoMember(19)]
        public int Zizeng
        {
            get { return zizeng; }
            set { zizeng = value; }
        }
        private bool ishtml = false;
        /// <summary>
        /// 是否将内容转成html
        /// </summary>
         [ProtoMember(20)]
        public bool IsHtml
        {
            set { ishtml = value; }
            get { return ishtml; }
        }
        private bool _isOrorAnd = false;
         [ProtoMember(21)]
        public bool IsOrorAnd
        {
            get { return _isOrorAnd; }
            set { _isOrorAnd = value; }
        }



        private string[] checkstr1 = new string[0];
         [ProtoMember(22)]
        public string[] CheckStr1
        {
            set { checkstr1 = value; }
            get { return checkstr1; }
        }

        private string[] checkstr2=new string[0];
         [ProtoMember(23)]
        public string[] CheckStr2
        {
            set { checkstr2 = value; }
            get { return checkstr2; }
        }

        private string _inhead = "";
         [ProtoMember(24)]
        public string inhead
        {
            get { return _inhead; }
            set { _inhead = value; }
        }

        private string _infoot = "";
         [ProtoMember(25)]
        public string infoot
        {
            get { return _infoot; }
            set { _infoot = value; }
        }
        #endregion

        #region 后处理 
        private Dictionary<string, string> _houreplaceData = new Dictionary<string, string>();
        /// <summary>
        /// 替换数据(后处理)
        /// </summary>
         [ProtoMember(26)]
        public Dictionary<string, string> HouReplaceData
        {
            get { return _houreplaceData; }
            set { _houreplaceData = value; }
        }

        private string[] _guolvbiaoqian = new string[0];
        /// <summary>
        /// 后处理过滤标签
        /// </summary>
         [ProtoMember(27)]
        public string[] Guolvbiaoqian
        {
            get { return _guolvbiaoqian; }
            set { _guolvbiaoqian = value; }
        }
        #endregion

    }
    [ProtoContract]
    [Serializable]
    public class RulesEngLv2
    {
        /// <summary>
        /// 规则
        /// </summary>
        private string _rulestr="";
        /// <summary>
        /// 提取规则
        /// </summary>
         [ProtoMember(28)]
        public string Rulesstr
        {
            set { _rulestr = value; }
            get { return _rulestr; }
        }
        private int _getmodel=1;
        /// <summary>
        /// 提取模式
        /// </summary>
         [ProtoMember(29)]
        public int GetModel
        {
            set { _getmodel = value; }
            get { return _getmodel; }
        }

        private string readme = "未命名子规则";
        /// <summary>
        /// 规则描述
        /// </summary>
         [ProtoMember(30)]
        public string Readme
        {
            set { readme = value; }
            get { return readme; }
        }
    }
    [ProtoContract]
    [Serializable]
    public class FinalRules
    {
        /// <summary>
        /// 规则
        /// </summary>
        private RulesEngLv1 _Rules = new RulesEngLv1();
         [ProtoMember(31)]
        public RulesEngLv1 Rules
        {
            get { return _Rules; }
            set { _Rules = value; }
        }
        /// <summary>
        /// 所取的值
        /// </summary>
        private int _selectedValue = 0;
         [ProtoMember(32)]
        public int selectedValue
        {
            get { return _selectedValue; }
            set { _selectedValue = value; }
        }
        /// <summary>
        /// 是否是取公共规则
        /// </summary>
        private bool _isGetPublicRules = false;
         [ProtoMember(33)]
        public bool isGetPublicRules
        {
            get { return _isGetPublicRules; }
            set { _isGetPublicRules = value; }
        }

  
    }
}
