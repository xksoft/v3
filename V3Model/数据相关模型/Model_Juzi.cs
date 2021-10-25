using System;
using System.Collections.Generic;

using System.Text;
using System.Collections;
using MyProto;

namespace Model
{
    [ProtoContract]
    [Serializable]
    public class Model_Juzi
    {
        #region Model
        private int _id;
        private string _juzi;
        /// <summary>
        /// 
        /// </summary>
         [ProtoMember(1)]
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
         [ProtoMember(2)]
        public string juzi
        {
            set { _juzi = value; }
            get { return _juzi; }
        }
        #endregion Model

    }
}
