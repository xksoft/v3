using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyProto;

namespace Model
{
    [ProtoContract]
    [Serializable]
    public partial class ModelBase
    {
        #region Model
        private String _id;
        private string _name;
        private int _type;
        private string _readme;
        private string _description;
        private int _uid;
        private decimal _money;
        private string _data;
        private string _url;
        private string _designname;
        private int _userCount = 0;
          [ProtoMember(1)]
        public int UserCount
        {
            get { return _userCount; }
            set { _userCount = value; }
        }
        private DateTime _updatetime;
        /// <summary>
        /// 
        /// </summary>
          [ProtoMember(2)]
        public String id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
          [ProtoMember(3)]
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
          [ProtoMember(4)]
        public int Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
          [ProtoMember(5)]
        public string Readme
        {
            set { _readme = value; }
            get { return _readme; }
        }
        /// <summary>
        /// 
        /// </summary>
          [ProtoMember(6)]
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
          [ProtoMember(7)]
        public int Uid
        {
            set { _uid = value; }
            get { return _uid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(8)]
        public decimal Money
        {
            set { _money = value; }
            get { return _money; }
        }
        /// <summary>
        /// 
        /// </summary>
          [ProtoMember(9)]
        public string Data
        {set { _data = value; }
            get { return _data; }
        }
        /// <summary>
        /// 
        /// </summary>
          [ProtoMember(10)]
        public string Url
        {
            set { _url = value; }
            get { return _url; }
        }
        /// <summary>
        /// 
        /// </summary>
          [ProtoMember(11)]
        public string DesignName
        {
            set { _designname = value; }
            get { return _designname; }
        }
        /// <summary>
        /// 
        /// </summary>
          [ProtoMember(12)]
        public DateTime UpdateTime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        #endregion Model
    }
}
