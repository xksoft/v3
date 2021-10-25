using System;
using System.Collections.Generic;
using System.Text;
using V3;
using Model;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using V3.Common;
namespace V3.Bll
{
    public static class PointBll
    {

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static int Add(SendPoint model)
        {
            int i = 0;
            foreach (KeyValuePair<int, SendPoint> sk in Model.V3Infos.SendPointDb)
            {
            
                    if (Convert.ToInt32(sk.Key) > i) { i = Convert.ToInt32(sk.Key); }
            }
            Model.V3Infos.MainDb.Pointid = i;
            V3Infos.MainDb.Pointid++;
            model.id = V3Infos.MainDb.Pointid;
            Model.V3Infos.SendPointDb.Add(model.id, model);
            V3Helper.saveSendPointDb();
            return model.id;
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public static bool Update(Model.SendPoint model)
        {
            try
            {
                V3Infos.SendPointDb[model.id] = model;
                V3Helper.saveSendPointDb();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public static bool Delete(int id)
        {
            try
            {
                V3Infos.SendPointDb.Remove(id);
                V3Helper.saveSendPointDb();
                return true;
            }
            catch { return false; }
        }

    }
}
