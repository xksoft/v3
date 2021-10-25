using MyProto;
using System;
using System.Collections.Generic;

using System.Text;


namespace Model
{
    [ProtoContract]
    [Serializable]
    public partial class SendPoint
    {
        public SendPoint()
        { }
        #region Model
        private int _id=0;
        private string _name="";
        private string _description="";
        private bool _accountmodel=false;
        private int _articledbid = 0;
        private int _hashdbid=0;
        private int _keyworddbid=0;
        private int _linkdbid=0;
        private int _ReplaceDbid=0;
        private string _adminurl="";
        private string _username="";
        private string _password="";
        private string _loginvalue1="";
        private string _loginvalue2="";
        private Model.发布相关模型.UserAccount _accountdata = new 发布相关模型.UserAccount();
        private Model.发布相关模型.Account oneaccount = new 发布相关模型.Account();
        private bool isUseModelBianma = false;
        private bool isPostUtf8 = false;
        private string majiaurl = "";


        /// <summary>
        /// 是否是用utf8编码发布
        /// </summary>
         [ProtoMember(1)]
        public bool IsPostUtf8
        {
            get { return isPostUtf8; }
            set { isPostUtf8 = value; }
        }

        /// <summary>
        /// 是否使用模块自身的编码
        /// </summary>
         [ProtoMember(2)]
        public bool IsUseModelBianma
        {
            get { return isUseModelBianma; }
            set { isUseModelBianma = value; }
        }


        /// <summary>
        /// 单一账号
        /// </summary>
         [ProtoMember(3)]
        public Model.发布相关模型.Account Oneaccount
        {
            get { return oneaccount; }
            set { oneaccount = value; }
        }

    
        private string _getmodel = "";
        private string  _postmodel = "";
        private string _grouptag = "DefaultGroup";
        /// <summary>
        /// 发布点id
        /// </summary>
        [ProtoMember(4)]
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 发布点名称
        /// </summary>
         [ProtoMember(5)]
        public string name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 发布点描述
        /// </summary>
         [ProtoMember(6)]
        public string description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 账号使用方式
        /// </summary>
         [ProtoMember(7)]
        public bool AccountModel
        {
            set { _accountmodel = value; }
            get { return _accountmodel; }
        }
        /// <summary>
        /// 发布点使用的文章库id
        /// </summary>
         [ProtoMember(8)]
        public int ArticleDbID
        {
            set { _articledbid = value; }
            get { return _articledbid; }
        }
        /// <summary>
        /// 发布点使用的哈希库id
        /// </summary>
         [ProtoMember(9)]
        public int HashDbID
        {
            set { _hashdbid = value; }
            get { return _hashdbid; }
        }
        /// <summary>
        /// 发布点使用的关键字库id
        /// </summary>
         [ProtoMember(10)]
        public int KeywordDbID
        {
            set { _keyworddbid = value; }
            get { return _keyworddbid; }
        }
        /// <summary>
        /// 发布点使用的链接库id
        /// </summary>
         [ProtoMember(11)]
        public int LinkDbID
        {
            set { _linkdbid = value; }
            get { return _linkdbid; }
        }
        /// <summary>
        /// 
        /// </summary>
         [ProtoMember(12)]
        public int ReplaceDbid
        {
            set { _ReplaceDbid = value; }
            get { return _ReplaceDbid; }
        }
        /// <summary>
        /// 发布点后台地址
        /// </summary>
         [ProtoMember(13)]
        public string AdminUrl
        {
            set { _adminurl = value; }
            get { return _adminurl; }
        }
        /// <summary>
        /// 发布点后台账号
        /// </summary>
         [ProtoMember(14)]
        public string Username
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 发布点后台密码
        /// </summary>
         [ProtoMember(15)]
        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 登录附加值1
        /// </summary>
         [ProtoMember(16)]
        public string loginvalue1
        {
            set { _loginvalue1 = value; }
            get { return _loginvalue1; }
        }
        /// <summary>
        /// 登录附加值2
        /// </summary>
         [ProtoMember(17)]
        public string loginvalue2
        {
            set { _loginvalue2 = value; }
            get { return _loginvalue2; }
        }
        /// <summary>
        /// 账号信息
        /// </summary>
         [ProtoMember(18)]
        public Model.发布相关模型.UserAccount AccountData
        {
            set { _accountdata = value; }
            get { return _accountdata; }
        }
     

        /// <summary>
        /// 抓取模块id
        /// </summary>
        [ProtoMember(22)]
        public String GetModel
        {
            set { _getmodel = value; }
            get { return _getmodel; }
        }
        /// <summary>
        /// 发布模块id
        /// </summary>
        [ProtoMember(23)]
        public String PostModel
        {
            set { _postmodel = value; }
            get { return _postmodel; }
        }

        /// <summary>
        /// 分组标签
        /// </summary>
        [ProtoMember(21)]
        public string GroupTag
        {
            set { _grouptag = value; }
            get { return _grouptag; }
        }
        #endregion Model
    }
}
