using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Model.发布相关模型;
using xEngine.Common;
using V3.Common;
using xEngine.Execute;
using xEngine.Model.Execute.Http;
using xEngine.Model;
namespace V3.Common
{
    public class ModelShopBll
    {
     
        //获取模块商店数据
        public static string GetModelShop()
        {
            string result = "OK";
            //xEngine.Model.Command cmd = new xEngine.Model.Command();
            //cmd.Action = "0";
            //cmd.Number1 = 99;

            //try
            //{
            //    cmd = Common.Command.RunCommand(cmd, Program.ServerUrl);
            //    if (cmd.IsOk)
            //    {
            //        bool isNoSave = false;
            //        bool isgetmodel = false;
            //        bool isupgrade = false;
            //        bool isgetshop = false;

            //        frmMain.MyFrmMain.Invoke((EventHandler)(delegate
            //        {
            //            V3.V3Form.frmDownLoadModel frm = new V3Form.frmDownLoadModel();
            //            frm.TopMost = true;
            //            frm.ShowDialog();
            //            if (!frm.issave)
            //                isNoSave = true;
            //            isgetmodel = frm.isgetmymodel.IsOn;
            //            isupgrade = frm.isupdatemymodel.IsOn;
            //            isgetshop = frm.isgetshop.IsOn;
            //        }));
            //        if (isNoSave)
            //            return "OK";
            //        Dictionary<int, Model.ModelBase> temp1 =xEngine.Common.XSerializable.BytesToObject<Dictionary<int, Model.ModelBase>>(cmd.Bytes1);
            //        Dictionary<int, Model.ModelBase> temp2 = xEngine.Common.XSerializable.BytesToObject<Dictionary<int, Model.ModelBase>>(cmd.Bytes2);
            //        Dictionary<int, Model.ModelBase> temp3 = xEngine.Common.XSerializable.BytesToObject<Dictionary<int, Model.ModelBase>>(cmd.Bytes3);
            //        Dictionary<int, Model.ModelBase> temp4 = xEngine.Common.XSerializable.BytesToObject<Dictionary<int, Model.ModelBase>>(cmd.Bytes4);
            //        if (isgetshop)
            //        {
            //            Model.V3Infos.ModelShop.Clear();
            //            Model.V3Infos.ModelShopNewTop100.Clear();
            //            Model.V3Infos.ModelShopHotTop100.Clear();
            //            foreach (KeyValuePair<int, Model.ModelBase> s in temp1)
            //            {
            //                Model.ModelBase temp = new Model.ModelBase();
            //                temp = XSerializable.CloneObject<Model.ModelBase>(s.Value);
            //                Model.V3Infos.ModelShop.Add(s.Key, temp);
            //            }
            //            foreach (KeyValuePair<int, Model.ModelBase> s in temp2)
            //            {
            //                Model.ModelBase temp = new Model.ModelBase();
            //                temp = XSerializable.CloneObject<Model.ModelBase>(s.Value);
            //                Model.V3Infos.ModelShopNewTop100.Add(s.Key, temp);
            //            }
            //            foreach (KeyValuePair<int, Model.ModelBase> s in temp3)
            //            {
            //                Model.ModelBase temp = new Model.ModelBase();
            //                temp = XSerializable.CloneObject<Model.ModelBase>(s.Value);
            //                Model.V3Infos.ModelShopHotTop100.Add(s.Key, temp);
            //            }
            //        }
            //        if (isgetmodel)
            //        {
            //            foreach (KeyValuePair<int, Model.ModelBase> s in temp4)
            //            {
            //                try
            //                {
            //                    if (s.Value == null)
            //                    {
            //                        continue;
            //                    }
            //                    Model.GetPostModel model = GetModelbyBase(s.Value);
            //                    //xEngine.Common.XSerializable.BytesToObject<Model.GetPostModel>(Base32.FromBase32String(s.Value.Data));
            //                    Model.GetPostModel temp = new Model.GetPostModel();
            //                    model.mid = s.Key;
            //                    temp = XSerializable.CloneObject<Model.GetPostModel>(model);
            //                    temp.uid = s.Value.Uid;
            //                    if (Model.V3Infos.MainDb.MyModel.ContainsKey(s.Key))
            //                    {
            //                        if (isupgrade)
            //                        {
            //                            Model.V3Infos.MainDb.MyModel[s.Key] = temp;
            //                        }
            //                        else if (temp.UpdateTime != Model.V3Infos.MainDb.MyModel[s.Key].UpdateTime)
            //                        {

            //                            if (
            //                                XtraMessageBox.Show(
            //                                    "模块“[" + Model.V3Infos.MainDb.MyModel[s.Key].mid + "]" +
            //                                    Model.V3Infos.MainDb.MyModel[s.Key].PlanName + "”，已经有更新，是否同步它？", "确认",
            //                                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            //                            {
            //                                Model.V3Infos.MainDb.MyModel[s.Key] = temp;
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        Model.V3Infos.MainDb.MyModel.Add(s.Key, temp);
            //                    }
            //                }
            //                catch(Exception error)
            //                {
            //                    result += "解析模块["+s.Value.id+":"+s.Value.Name+"]时出错，请联系管理员！\r\n\r\n";
            //                }
            //            }
            //            ReSortMymodel();
            //        }

                  
            //        Model.V3Infos.MainDb.Lastupdatetime = DateTime.Now;
            //        V3Helper.saveDBS();
            //        result ="OK";
            //    }
            //    else
            //    {
            //        result ="获取失败！";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    result ="服务器返回信息错误！详细：" + ex.Message;
            //}
            return result;
        }
       
        //把getpostmodel转成modelbase
        public static Model.ModelBase GetModelbase(Model.GetPostModel tempmodel)
        {
            Model.ModelBase mbmodel = new Model.ModelBase();
            mbmodel.Name = tempmodel.PlanName;
            if (tempmodel.ShareLevel == 0)
                mbmodel.Money = 0;
            else if (tempmodel.ShareLevel == 1)
                mbmodel.Money = 1;
            else if (tempmodel.ShareLevel == 2)
                mbmodel.Money = 10;
            else if (tempmodel.ShareLevel == 3)
                mbmodel.Money = 30;
            else if (tempmodel.ShareLevel == 4)
                mbmodel.Money = 60;
            else
                mbmodel.Money = 100;
            mbmodel.Readme = tempmodel.PlanReadme;
            
            mbmodel.Type = tempmodel.PlanModel;
            mbmodel.Uid = Convert.ToInt32(99);
            mbmodel.UpdateTime = DateTime.Now;
            mbmodel.Url = tempmodel.PlanUrl;
            tempmodel.PlanDesignName = "用户";
            if (tempmodel.PlanDescripton == "")
                tempmodel.PlanDescripton = tempmodel.PlanReadme;
            if (tempmodel.Stp1_POST_LoginAction==null||tempmodel.Stp1_POST_LoginAction.Length == 0)
            {
                tempmodel.Stp1_POST_LoginAction = new[] {new GetPostAction()};
            }
            mbmodel.Data = Base32.ToBase32String(xEngine.Common.XSerializable.ObjectToBytes(tempmodel));
           
            mbmodel.Description = tempmodel.PlanDescripton;
            mbmodel.DesignName = "用户";
            mbmodel.id = tempmodel.mids;
            return mbmodel;
        }
        public static Model.GetPostModel GetModelbyBase(Model.ModelBase mbmodel)
        {
            Model.GetPostModel tempmodel = new Model.GetPostModel();
            tempmodel = xEngine.Common.XSerializable.CloneObject<Model.GetPostModel>(xEngine.Common.XSerializable.BytesToObject<Model.GetPostModel>(Base32.FromBase32String(mbmodel.Data)));
           // tempmodel = (Model.GetPostModel)XiakeApp.Class.xkHelper.getObject((Model.GetPostModel)XiakeApp.Class.ObjectToByte.StrToObject(mbmodel.Data), new Model.GetPostModel());
            tempmodel.PlanName = mbmodel.Name;
            if (mbmodel.Money == 0)
                tempmodel.ShareLevel = 0;
            else if (mbmodel.Money == 1)
                tempmodel.ShareLevel = 1;
            else if (mbmodel.Money == 10)
                tempmodel.ShareLevel = 2;
            else if (mbmodel.Money == 30)
                tempmodel.ShareLevel = 3;
            else if (mbmodel.Money == 60)
                tempmodel.ShareLevel = 4;
            else
                tempmodel.ShareLevel = 5;

            tempmodel.IsShareModel = tempmodel.IsShareModel;
            tempmodel.PlanReadme = mbmodel.Readme;
            tempmodel.PlanDesignName = mbmodel.DesignName;
            tempmodel.PlanModel = mbmodel.Type;
            tempmodel.UpdateTime = mbmodel.UpdateTime.ToString();
            tempmodel.PlanUrl = mbmodel.Url;
            tempmodel.PlanDescripton = mbmodel.Description;
            tempmodel.mids = mbmodel.id;
            return tempmodel;
        }
      
        public  static void ReSortMymodel()
        {
            String[] ids = new string[Model.V3Infos.MainDb.MyModels.Keys.Count];
            Model.V3Infos.MainDb.MyModels.Keys.CopyTo(ids, 0);
            Array.Sort(ids);
            Array.Reverse(ids);
            var tempsss = new Dictionary<String, Model.GetPostModel>();
            for (int i = 0; i < ids.Length; i++)
            {
                if (Model.V3Infos.MainDb.MyModels[ids[i]] != null)
                    tempsss.Add(ids[i], Model.V3Infos.MainDb.MyModels[ids[i]]);
            }
            Model.V3Infos.MainDb.MyModels = tempsss;
            
        }
      
    }
}
