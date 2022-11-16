using System;
using System.Collections.Generic;
using System.Text;
using V3;
using Model;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms.VisualStyles;
using V3.V3Form;

namespace V3.Common
{
    public static class V3Helper
    {
        #region 首次启动需要执行的方法


        #region 加载数据库
        public static void CheckDB() 
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\"))
            {
                V3.Common.Log.LogNewline("[c11]首次启动数据初始化中，请稍后...[/c]");
                isUseSaveArticleDb = isUseSaveDbs = isUseSaveHashDb = isUseSaveKeywordDb = isUseSaveLinkDb = isUseSavePointDb = isUseSaveReplaceDb = isUseSaveTaskDb = false;
                saveDBS();
                saveTaskDb();
                saveSendPointDb();
                saveArticleDb();
                saveLinkDb();
                saveKeywordDb();
                saveHashDb();
                saveReplaceDb();
               
           
            }
        
        }
        /// <summary>
        /// 从文件中读取V3设置信息
        /// </summary>
        /// <returns></returns>
        public static bool LoadDbs()
        {
            bool result = false;
            try
            {

                object temp =(xEngine.Common.XSerializable.BytesToObject<Model.Model_V3Setting>(V3.Common.xFile.ReadFile(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", "dbsv3")));
                if (temp == null)
                {

                    Model.V3Infos.MainDb = new Model_V3Setting();
                    saveDBS();
                }
                else
                {
                    Model.V3Infos.MainDb = xEngine.Common.XSerializable.CloneObject<Model.Model_V3Setting>(temp);
                    if ( Model.V3Infos.MainDb.MyModel.Count>0) 
                    {
                        for (int i=0;i< Model.V3Infos.MainDb.MyModel.Count;i++) {
                            var model = Model.V3Infos.MainDb.MyModel[Model.V3Infos.MainDb.MyModel.Keys.ToList()[i]];
                            model.mids = Model.V3Infos.MainDb.MyModel.Keys.ToList()[i].ToString();
                            if (!Model.V3Infos.MainDb.MyModels.ContainsKey(model.mids)) 
                            { 
                                Model.V3Infos.MainDb.MyModels.Add(model.mids, model);
                            }
                           
                        }
                        Model.V3Infos.MainDb.MyModel.Clear();
                    }
                }
                V3.Common.Log.LogNewline("[c11]正在加载伪原创库...[/c]");

             
                Model.Model_WeiYuanChuang.words = xEngine.Common.XSerializable.CloneObject<List<string>>(xEngine.Common.XSerializable.BytesToObject<List<string>>(Properties.Resources.sck));
                V3.Common.Log.LogNewline("[c12]成功加载到伪原创词组" + Model.Model_WeiYuanChuang.words.Count + "个！[/c]");


            }
            catch(Exception error)
            {
                V3.Common.Log.LogNewline("[c14]系统数据加载出错："+error+",请立刻备份站点数据并关闭V3联系在线客服以防数据丢失！[/c]");
                result = false;
            }
        
            return result;
        }
        public static void CopyDirectory(string srcdir, string desdir)
        {
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);

            string desfolderdir = desdir + "\\" + folderName;

            if (desdir.LastIndexOf("\\") == (desdir.Length - 1))
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {

                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }

                    CopyDirectory(file, desfolderdir);
                }

                else // 否则直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);

                    srcfileName = desfolderdir + "\\" + srcfileName;


                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }


                    File.Copy(file, srcfileName);
                }
            }//foreach
        }//function end
        public static void bakdbmain()
        {
            try
            {
                string Path = AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Bak\\" + DateTime.Now.Year.ToString() +"年"+ DateTime.Now.Month.ToString()+"月" + DateTime.Now.Day.ToString()+"日" ;
                if (!Directory.Exists(Path))
                {
                    V3.Common.Log.LogNewline("[c11]检测到今天还没有备份站点数据，开始自动备份...[/c]");
                    CopyDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", Path);
                    V3.Common.Log.LogNewline("[c12]站点数据已备份到“"+Path+"”！[/c]");

                }
                else
                {
                    Path = Path + DateTime.Now.Hour.ToString()+"点"+DateTime.Now.Minute+"分";
                    if (!Directory.Exists(Path))
                    {
                        V3.Common.Log.LogNewline("[c11]检测到今天已经备份，自动备份数据到当前时间！2小时后再次检查！[/c]");

                        CopyDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", Path);
                        V3.Common.Log.LogNewline("[c12]站点数据已备份到“" + Path + "”！[/c]");
                    }
                }
            }
            catch (Exception err)
            {
                V3.Common.Log.LogNewline("[c14]站点数据备份出错！原因：" + err.Message+"[/c]");
            }

        }
        /// <summary>
        /// 从数据库中加载文章库信息
        /// </summary>
        /// <returns></returns>
       public static bool LoadArticleDb()
        {
         
            bool result = false;
            try
            {

                object temp = xEngine.Common.XSerializable.BytesToObject<Dictionary<string, ArticleDB>>(xFile.ReadFile(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", "ArticleDb"));
                if (temp != null)
                    Model.V3Infos.ArticleDb = (Dictionary<string, ArticleDB>)temp;
                else
                {
                    Model.V3Infos.ArticleDb = new Dictionary<string, ArticleDB>();
                    saveArticleDb();
                }
            }
            catch { result = false; }
            if (Model.V3Infos.ArticleDb.Count > 0)
            {
                Dictionary<string, ArticleDB> newDbs = new Dictionary<string, ArticleDB>();
                int[] ks = new int[Model.V3Infos.ArticleDb.Count];
                int c = 0;
                foreach (KeyValuePair<string, ArticleDB> k in Model.V3Infos.ArticleDb)
                {
                    ks[c] = int.Parse(k.Key);
                    c++;
                }
                Array.Sort(ks);
                foreach (int i in ks)
                {
                    newDbs.Add(i.ToString(), Model.V3Infos.ArticleDb[i.ToString()]);
                }
                Model.V3Infos.ArticleDb = newDbs;
            }
            foreach (KeyValuePair<string, ArticleDB> kv in Model.V3Infos.ArticleDb)
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + kv.Value.Id + "\\"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + kv.Value.Id + "\\");
                kv.Value.DataCount = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\ArticleDb\\" + kv.Value.Id + "\\").Length;
            }

            return result;
        }
        /// <summary>
        /// 从数据库中加载关键词库信息
        /// </summary>
        /// <returns></returns>
       public static bool LoadKeywordDb()
        {
            bool result = false;
            try
            {
                object temp = xEngine.Common.XSerializable.BytesToObject<Dictionary<string, Model.KeywordDB>>(xFile.ReadFile(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", "KeywordDb"));
                if (temp != null)
                    Model.V3Infos.KeywordDb = (Dictionary<string, Model.KeywordDB>)temp;
                else
                {
                    Model.V3Infos.KeywordDb = new Dictionary<string, Model.KeywordDB>();
                    saveKeywordDb();
                }
            }
            catch { result = false; }
            if (Model.V3Infos.KeywordDb.Count > 0)
            {
                Dictionary<string, KeywordDB> newDbs = new Dictionary<string, KeywordDB>();
                int[] ks = new int[Model.V3Infos.KeywordDb.Count];
                int c = 0;
                foreach (KeyValuePair<string, KeywordDB> k in Model.V3Infos.KeywordDb)
                {
                    ks[c] = int.Parse(k.Key);
                    c++;
                }
                Array.Sort(ks);
                foreach (int i in ks)
                {
                    newDbs.Add(i.ToString(), Model.V3Infos.KeywordDb[i.ToString()]);
                }
                Model.V3Infos.KeywordDb = newDbs;
            }
            return result;
        }

        /// <summary>
        /// 从数据库中加载哈希库信息
        /// </summary>
        /// <returns></returns>
       public static bool LoadHashDb()
        {
            bool result = false;
            try
            {
                object temp = xEngine.Common.XSerializable.BytesToObject<Dictionary<string, Model.HashDB>>(xFile.ReadFile(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", "HashDb"));
                if (temp != null)
                    Model.V3Infos.HashDb = (Dictionary<string, Model.HashDB>)temp;
                else
                {
                    Model.V3Infos.HashDb = new Dictionary<string, Model.HashDB>();
                    saveHashDb();
                }
            }
            catch { result = false; }
            if (Model.V3Infos.HashDb.Count > 0)
            {
                Dictionary<string, HashDB> newDbs = new Dictionary<string, HashDB>();
                int[] ks = new int[Model.V3Infos.HashDb.Count];
                int c = 0;
                foreach (KeyValuePair<string, HashDB> k in Model.V3Infos.HashDb)
                {
                    ks[c] = int.Parse(k.Key);
                    c++;
                }
                Array.Sort(ks);
                foreach (int i in ks)
                {
                    newDbs.Add(i.ToString(), Model.V3Infos.HashDb[i.ToString()]);
                }
                Model.V3Infos.HashDb = newDbs;
            }
            return result;
        }

        /// <summary>
        /// 从数据库中加载替换库信息
        /// </summary>
        /// <returns></returns>
       public static bool LoadReplaceDb()
        {
            bool result = false;
            try
            {
                object temp = xEngine.Common.XSerializable.BytesToObject<Dictionary<string, Model.ReplaceDB>>(xFile.ReadFile(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", "ReplaceDb"));
                if (temp != null)
                    Model.V3Infos.ReplaceDb = (Dictionary<string, Model.ReplaceDB>)temp;
                else
                {
                    Model.V3Infos.ReplaceDb = new Dictionary<string, Model.ReplaceDB>();
                    saveReplaceDb();
                }
            }
            catch { result = false; }
            if (Model.V3Infos.ReplaceDb.Count>0)
            {
                Dictionary<string, ReplaceDB> newDbs = new Dictionary<string, ReplaceDB>();
                int[] ks = new int[Model.V3Infos.ReplaceDb.Count];
                int c = 0;
                foreach (KeyValuePair<string, ReplaceDB> k in Model.V3Infos.ReplaceDb)
                {
                    ks[c]=int.Parse(k.Key);
                    c++;
                }
                Array.Sort(ks);
                foreach (int i in ks)
                {
                    newDbs.Add(i.ToString(), Model.V3Infos.ReplaceDb[i.ToString()]);
                }
               Model.V3Infos.ReplaceDb = newDbs;
            }
           
            return result;
        }
        /// <summary>
        /// 从数据库中加载链接库信息
        /// </summary>
        /// <returns></returns>
       public static bool LoadLinkDb()
        {
            bool result = false;
            try
            {
                object temp = xEngine.Common.XSerializable.BytesToObject<Dictionary<string, Model.LinkDB>>(xFile.ReadFile(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", "LinkDb"));
                if (temp != null)
                    Model.V3Infos.LinkDb = (Dictionary<string, Model.LinkDB>)temp;
                else
                {
                    Model.V3Infos.LinkDb = new Dictionary<string, Model.LinkDB>();
                    saveLinkDb();
                }
            }
            catch { result = false; }
            if (Model.V3Infos.LinkDb.Count > 0)
            {
                Dictionary<string, LinkDB> newDbs = new Dictionary<string, LinkDB>();
                int[] ks = new int[Model.V3Infos.LinkDb.Count];
                int c = 0;
                foreach (KeyValuePair<string, LinkDB> k in Model.V3Infos.LinkDb)
                {
                    ks[c] = int.Parse(k.Key);
                    c++;
                }
                Array.Sort(ks);
                foreach (int i in ks)
                {
                    newDbs.Add(i.ToString(), Model.V3Infos.LinkDb[i.ToString()]);
                }
                Model.V3Infos.LinkDb = newDbs;
            }
            return result;
        }

        /// <summary>
        /// 从数据库中加载发布点信息
        /// </summary>
        /// <returns></returns>
       public static bool LoadSendPointDb()
        {
         

            bool result = true;
            try
            {
                object temp = xEngine.Common.XSerializable.BytesToObject<Dictionary<int, Model.SendPoint>>(xFile.ReadFile(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", "SendPointDb"));
                if (temp != null)
                    Model.V3Infos.SendPointDb = (Dictionary<int, Model.SendPoint>) temp;
                else
                { 
                    Model.V3Infos.SendPointDb=new Dictionary<int, SendPoint>();
                    saveSendPointDb();
                }
                int[] ids = new int[Model.V3Infos.SendPointDb.Keys.Count];
                Model.V3Infos.SendPointDb.Keys.CopyTo(ids, 0);
                for (int i = 0; i < ids.Length; i++)
                {
                    if (Model.V3Infos.SendPointDb[ids[i]].id > 100000||ids[i]>100000)
                        Model.V3Infos.SendPointDb.Remove(ids[i]);
                }

                if (Model.V3Infos.SendPointDb.Count > Program.Level)
                {
                    V3Form.工具窗体.框 k = new V3Form.工具窗体.框();
                    k.msg = "您添加的站点数量已经超出授权数量，点击确定进入官网直接联系客服升级即可，点击忽略\n\n则会自动忽略多出的站点，点击取消退出程序！";
                    k.Text = "站点超出数量";
                    k.ShowDialog();
                    if (k.backstring == "忽略")
                    {
                        int more = Model.V3Infos.SendPointDb.Count - Program.Level;
                        ids = new int[Model.V3Infos.SendPointDb.Keys.Count];
                        Model.V3Infos.SendPointDb.Keys.CopyTo(ids, 0);
                        int i = 0;
                        while (Model.V3Infos.SendPointDb.Count > Program.Level)
                        {
                            Model.V3Infos.SendPointDb.Remove(ids[i]);
                            i++;
                        }
                        V3.Common.Log.LogNewline("[c11][系统]系统自动忽略掉" + more.ToString() + "个站点，已经成功加载了" + Model.V3Infos.SendPointDb.Count + "个站点信息！[/c]");
                    }
                    else if (k.backstring == "升级")
                    {
                        System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://www.xiake.org/xiakezhanqun");
                        Environment.Exit(0);
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }
               
                V3.Common.Log.LogNewline("[c12][系统]成功加载" + Model.V3Infos.SendPointDb.Count + "个站点信息[/c]");

            }
            catch { result = false; }
            return result;

        }

        /// <summary>
        /// 从数据库中加载任务信息
        /// </summary>
        /// <returns></returns>
       public static bool LoadTask()
        {

            bool result = true;
            try
            {
                object temp = xEngine.Common.XSerializable.BytesToObject<Dictionary<int, Model.Task>>(xFile.ReadFile(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", "TaskDb"));
                if (temp != null)
                    Model.V3Infos.TaskDb = (Dictionary<int, Model.Task>) temp;
                else
                {
                     Model.V3Infos.TaskDb = new Dictionary<int, Task>();
                    saveTaskDb();
                }
                int more = 0;

                int num = 10;
                if (Program.Level < 3)//初级版
                {
                    num = Program.Level * 3;

                }
                if (Program.Level < 30)//初级版
                {
                    num = 10 * 3;

                }
                else if (Program.Level >= 30 && (Program.Level < 120))//高级版
                    num = 20 * 30;
                else if (Program.Level >= 120 && (Program.Level < 9999))//精英版
                    num = 30 * 120;
                else
                    num = 99999999;
                if (Model.V3Infos.TaskDb.Count > num)//超出任务限制
                {
                    V3Form.工具窗体.框 k = new V3Form.工具窗体.框();
                    k.msg = "您添加的任务数量已经超出授权数量，点击确定进入官网直接联系客服升级即可，点击忽略\n\n则会自动忽略多出的任务，点击取消退出程序！";
                    k.Text = "任务超出数量";
                    DialogResult d = k.ShowDialog();

                    if (k.backstring == "忽略")
                    {

                        int[] ids = new int[Model.V3Infos.TaskDb.Keys.Count];
                        Model.V3Infos.TaskDb.Keys.CopyTo(ids, 0);
                        int i = 0;
                        more = Model.V3Infos.TaskDb.Count - num;
                        while (Model.V3Infos.TaskDb.Count > num)
                        {
                            Model.V3Infos.TaskDb.Remove(ids[i]);
                            i++;
                        }

                    }
                    else if (k.backstring == "升级")
                    {
                        System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://www.xiake.org/xiakezhanqun");
                        Environment.Exit(0);


                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                    V3.Common.Log.LogNewline("[c12][系统]系统自动忽略" + more + "个任务，已经成功加载了" + Model.V3Infos.TaskDb.Count + "个任务信息[/c]");
                }

                result = true;
              
                V3.Common.Log.LogNewline("[c12][系统]系统已经成功加载了" + Model.V3Infos.TaskDb.Count + "个任务信息[/c]");

            }

            catch { result = false; }
            return result;


        }
        #endregion

        #region 存储数据库
        public static bool isUseSaveDbs = false;
        /// <summary>
        /// 把主数据库保存到数据库中
        /// </summary>
        /// <returns></returns>
        public static void saveDBS()
        {
            if (isUseSaveDbs)
                return;
            try
            {
                lock ("savedbs")
                {
                    isUseSaveDbs = true;
                    byte[] b = xEngine.Common.XSerializable.ObjectToBytes(V3Infos.MainDb);
                    xFile.SaveFile(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", "dbsv3",b );
                }
            }
            catch { }
            finally { isUseSaveDbs = false; }
        }
        public static void saveDBSThread()
        {
            System.Threading.Thread t = new Thread(saveDBS);
            t.IsBackground = true;
            t.Start();
        }
      


        #region 保存文章库
        public static bool isUseSaveArticleDb = false;
     
        public static void saveArticleDb()
        {
            if (isUseSaveArticleDb)
                return;
            try
            {
                
                    lock ("saveartdb")
                    { isUseSaveArticleDb = true;
                     xFile.SaveFile(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", "ArticleDb",xEngine.Common.XSerializable.ObjectToBytes(V3Infos.ArticleDb));
                        saveDBS();
                    }
                
            }
            catch { }
            finally { isUseSaveArticleDb = false; }
        }
        #endregion


        #region 保存关键词库
        public static bool isUseSaveKeywordDb = false;

      public  static void saveKeywordDb() 
        {
            if (isUseSaveKeywordDb)
                return;
            try
            {


                lock ("savekeyworddb")
                {
                    isUseSaveKeywordDb = true;
                    xFile.SaveFile(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", "KeywordDb", xEngine.Common.XSerializable.ObjectToBytes(V3Infos.KeywordDb));
                    saveDBS();
                }

            }
            catch { }
            finally { isUseSaveKeywordDb = false; }
        
        }
        #endregion

        #region 保存hash库
        public static bool isUseSaveHashDb = false;

       public static void saveHashDb()
        {
            if (isUseSaveHashDb)
                return;
            try
            {


                lock ("savehashdb")
                {
                    isUseSaveHashDb = true;
                   xFile.SaveFile(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", "HashDb",xEngine.Common.XSerializable.ObjectToBytes(V3Infos.HashDb));
                    saveDBS();
                }

            }
            catch { }
            finally { isUseSaveHashDb = false; }
        }
        #endregion

        #region 保存替换库
        public static bool isUseSaveReplaceDb = false;

       public static void saveReplaceDb() 
        {
            if (isUseSaveReplaceDb)
                return;
            try
            {


                lock ("savereplacedb")
                {
                    isUseSaveReplaceDb = true;
                    xFile.SaveFile(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", "ReplaceDb", xEngine.Common.XSerializable.ObjectToBytes(V3Infos.ReplaceDb));
                    saveDBS();
                }

            }
            catch { }
            finally { isUseSaveReplaceDb = false; }
        }
        #endregion

        #region 保存链接库
        public static bool isUseSaveLinkDb = false;

       public static void saveLinkDb() 
        {
            if (isUseSaveLinkDb)
                return;
            try
            {
                
                lock ("savelinkdb")
                {
                    isUseSaveLinkDb = true;
                    xFile.SaveFile(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", "LinkDb", xEngine.Common.XSerializable.ObjectToBytes(V3Infos.LinkDb));
                    saveDBS();
                }

            }
            catch { }
            finally { isUseSaveLinkDb = false; }
        }
        #endregion

        #region 保存站点
        public static bool isUseSavePointDb = false;
       
       public static void saveSendPointDb()
        {
            if (isUseSavePointDb)
                return;
            try
            {


                lock ("savepointdb")
                {
                    isUseSavePointDb = true;
                xFile.SaveFile(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", "SendPointDb", xEngine.Common.XSerializable.ObjectToBytes(V3Infos.SendPointDb));
                    saveDBS();
                }

            }
            catch { }
            finally { isUseSavePointDb = false; }
        }
        #endregion

        #region 保存任务
        public static bool isUseSaveTaskDb = false;

       public static void saveTaskDb() 
        {
            if (isUseSaveTaskDb)
                return;
            try
            {


                lock ("savetaskdb")
                {
                    isUseSaveTaskDb = true;
                    Dictionary<int,Model.Task> TaskList=new Dictionary<int, Task>();
                    var ks = V3Infos.TaskDb.Keys;
                    foreach (var k in ks)
                    {
                        if (V3Infos.TaskDb.ContainsKey(k))
                        {
                            TaskList.Add(k, V3Infos.TaskDb[k]);
                        }
                    }
                    xFile.SaveFile(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\Main\\", "TaskDb", xEngine.Common.XSerializable.ObjectToBytes(TaskList));
                    saveDBS();
                }

            }
            catch { }
            finally { isUseSaveTaskDb = false; }
        }
        #endregion



        #endregion
        #endregion



        #region 常见系统全局对象

        #region Sql计时方法
        static System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//sql执行计时器
        /// <summary>
        /// 开始计时
        /// </summary>
        public static void swstart()
        {
            sw.Reset();
            sw.Start();
        }
        /// <summary>
        /// 停止计时
        /// </summary>
        public static void swstop()
        {
            sw.Stop();
        }
        /// <summary>
        /// 获取计时读数
        /// </summary>
        /// <returns></returns>
        public static string swvalue()
        {
            return sw.ElapsedMilliseconds.ToString();
        }
        #endregion
        public static Random r = new Random();//随机对象
        #endregion

        #region DES加解密
        //DES解密
        public static string Decrypt(string pToDecrypt)
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(my());
                des.IV = ASCIIEncoding.ASCII.GetBytes(my());
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                    cs.Dispose();
                }

                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                ms.Dispose();
                return str;
            }
        }


        //得到加密密钥
        static string my()
        {
            byte[] mStrs = new byte[8];
            mStrs[0] = 0x21;
            mStrs[1] = 0x21;
            mStrs[2] = 0x21;
            mStrs[3] = 0x40;
            mStrs[4] = 0x40;
            mStrs[5] = 0x40;
            mStrs[6] = 0x23;
            mStrs[7] = 0x23;

            string mString_mStrs = System.Text.Encoding.UTF8.GetString(mStrs);
            return mString_mStrs;
        }

        //加密
        public static string Encrypt(string pToEncrypt)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(my());
                des.IV = ASCIIEncoding.ASCII.GetBytes(my());
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                    cs.Dispose();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                ms.Dispose();
                return str;
            }
        }
        #endregion

        public static TextBox GetInnerTextBox(DevExpress.XtraEditors.TextEdit editor)
        {

            if (editor != null)

                foreach (Control control in editor.Controls)

                    if (control is TextBox)

                        return (TextBox)control;

            return null;

        }
    }

    public class Format
    {
        /// <summary>
        /// 将字符串格式化成HTML代码
        /// </summary>
        /// <postdata name="str">要格式化的字符串</postdata>
        /// <returns>格式化后的字符串</returns>
        public String ToHtml(string str)
        {
            if (str == null || str.Equals(""))
            {
                return str;
            }
            StringBuilder sb = new StringBuilder(str);
            sb.Replace("\r\n", "<br>");
            sb.Replace("\n", "<br>");
            sb.Replace("\t", " ");
            // sb.Replace(" ", "&nbsp;");
            return sb.ToString();
        }
        public string oldurls = "";
        public string alt = "";

        public static String ToTxt(String str)
        {
            if (str == null || str.Equals(""))
            {
                return str;
            }

            StringBuilder sb = new StringBuilder(str);
            sb.Replace("&nbsp;", " ");
            sb.Replace("<br>", "\r\n");
            sb.Replace("&lt;", "<");
            sb.Replace("&gt;", ">");
            sb.Replace("&amp;", "&");
            return sb.ToString();
        }

        public string formatText(string sourceString, int taskid)
        {
            string result = sourceString;
            Dictionary<string, string> image = new Dictionary<string, string>();
            Dictionary<string, string> link = new Dictionary<string, string>();
            ArrayList tempList = new ArrayList();
            bool isPart = sourceString != null && sourceString.Trim().Length > 0;
            if (isPart)
            {
                result = ToTxt(result);
                result = ToHtml(result);
                result = result.Replace("\r", "");
                result = result.Replace("\n", "");
                result = result.Replace("\t", "");
                Regex regex = new Regex(@"<script[^>]*?>.*?</script>", RegexOptions.Singleline);//去掉所有的script标签
                result = regex.Replace(result, "");
                result = Regex.Replace(result, "(<head>).*(</head>)", "", RegexOptions.Singleline);
                regex = new Regex(@"<style[^>]*?>.*?</style>", RegexOptions.Singleline);//去掉所有的style标签
                result = regex.Replace(result, "");
                result = Regex.Replace(result, @"<( )*td([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*p([^>])*>|</p>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*div([^>])*>|</div>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*li( )*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*tr([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*br([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = processImage(ref image, result, Model.V3Infos.TaskDb.ContainsKey(taskid) ? Model.V3Infos.TaskDb[taskid].FormatPicURL : true);//替换掉图片
                result = processLink(ref link, result, taskid);//替换掉链接
                regex = new Regex(@"<.+?>", RegexOptions.Singleline);//去掉所有的标签             
                result = regex.Replace(result, "");     //最终的结果

                result = Regex.Replace(result, @" ( )+", "");
                result = Regex.Replace(result, "(\r)( )+(\r)", "\r\r");
                result = Regex.Replace(result, @"(\r\r)+", "\n");

                result = processFormatText(result, Model.V3Infos.TaskDb.ContainsKey(taskid) ? Model.V3Infos.TaskDb[taskid].FormatSuojin : false, Model.V3Infos.TaskDb.ContainsKey(taskid) ? Model.V3Infos.TaskDb[taskid].FormatHanggao : true);  //格式化
                result = processStringFromXML(result);
                foreach (KeyValuePair<string, string> kvp in link)
                {
                    result = result.Replace(kvp.Key, kvp.Value);//将链接标签重新返回
                }
                foreach (KeyValuePair<string, string> kvp in image)
                {
                    result = result.Replace(kvp.Key, kvp.Value);//将图片标签重新返回
                }
            }
            return result;
        }

      
        string processImage(ref Dictionary<string, string> imageHash, string sourceString, bool isFormatPicUrl)
        {
            string result = sourceString;
            Regex regex1 = new Regex("<img.*?>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            MatchCollection mc = regex1.Matches(result);
            int i = 0;
            foreach (Match m in mc)
            {
                if (m.Success)
                {
                    i++;
                    // Regex regex2 = new Regex("<(img|IMG) (.*?)(src|SRC)=('|\"|\\\\\"|)(.+?)(\\.jpg|\\.JPG|\\.gif|\\.GIF|\\.png|\\.PNG|\\.bmp|\\.BMP|\\.jpeg|\\.JPEG)(.*?)>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    Regex regex2 = new Regex("<(img|IMG) (.*?)(src|SRC)=('|\"|\\\\\"|)(.+?)(.*?)>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    MatchCollection mc2 = regex2.Matches(m.Value);
                    foreach (Match m2 in mc2)
                    {
                        if (m2.Success)
                        {
                            string oldURL = m2.Groups["5"].Value + m2.Groups["6"].Value;
                            if (oldURL.Contains("\""))
                            {
                                oldURL = oldURL.Remove(oldURL.IndexOf("\""));
                            }
                            if (isFormatPicUrl && oldurls != "")
                            {
                                string newurl = geturl(oldURL); ;
                                if (newurl != null)
                                    oldURL = newurl;
                            }
                            result = result.Replace(m.Value, "$formatImage" + i + "$");
                            imageHash.Add("$formatImage" + i + "$", "<img src=\"" + oldURL + "\" border=0 alt=\"" + alt + "\">");
                        }
                    }
                }
            }

            return result;
        }
        string processLink(ref Dictionary<string, string> Link, string sourceString, int taskid)
        {
            string result = sourceString;
            Regex regex1 = new Regex("<a.*?(</a>)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            MatchCollection mc = regex1.Matches(result);
            int i = 0;
            foreach (Match m in mc)
            {
                if (m.Success)
                {
                    i++;
                    Regex regex2 = new Regex(@"<a\s+href=(?<url>.+?)>(?<content>.+?)</a>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    MatchCollection mc2 = regex2.Matches(m.Value);
                    foreach (Match m2 in mc2)
                    {
                        if (m2.Success)
                        {
                            string url = m2.Groups["url"].Value;
                            string content = m2.Groups["content"].Value;
                            if (taskid == 0)//使用
                            {
                                if (oldurls != "")
                                {
                                    string newurl = geturl(url);
                                    if (newurl != null)
                                        url = newurl;
                                }
                            }
                            else if (taskid == 1)//不使用
                            {

                            }
                            else
                            {
                                if (Model.V3Infos.TaskDb[taskid].FormatLinkURL && oldurls != "")
                                {
                                    string newurl = geturl(url);
                                    if (newurl != null)
                                        url = newurl;
                                }
                            }
                            result = result.Replace(m.Value, "$formatLink" + i + "$");
                            Link.Add("$formatLink" + i + "$", "<a href=" + url + " target=\"_blank\">" + content + "</a>");
                        }
                    }
                }
            }

            return result;
        }
        public string processFormatText(string oldString, bool issuojin, bool ishanggao)
        {
            string newString = oldString;
            string[] temps = Regex.Split(newString, "\n");
            string result = "";
            for (int i = 0; i < temps.Length; i++)
            {
                string aaa = ReverseByArray(temps[i]);
                aaa = aaa.Trim();
                aaa = ReverseByArray(aaa);
                if (aaa.Trim().Length == 0)
                    continue;
                string cssstr = "";
                if (issuojin)
                    cssstr += "text-indent:2em; padding:0px; margin:0px;";
                if (ishanggao)
                    cssstr += "line-height:240%;";
                if (cssstr != "")
                    result += "<p style=\"" + cssstr + "\">" + aaa.Trim() + "</p>";
                else
                    result += "<p>" + aaa.Trim() + "</p>";
            }
            return result;
        }
        string ReverseByArray(string original)//反转字符串
        {
            char[] c = original.ToCharArray();
            Array.Reverse(c);
            return new string(c);
        }

        string processStringFromXML(string input)
        {
            string output = "";
            output = input.Replace("&lt;", "<");
            output = input.Replace("&nbsp;", " ");
            output = output.Replace("&gt;", ">");
            output = output.Replace("&quot;", "\"");
            output = output.Replace("&apos;", "'");
            output = output.Replace("&amp;", "&");
            return output;
        }

        //全角转半角
        string charConverter(string source)
        {
            if (source == "")
                return "";
            System.Text.StringBuilder result = new System.Text.StringBuilder(source.Length, source.Length);
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] >= 65281 && source[i] <= 65373)
                {
                    result.Append((char)(source[i] - 65248));//全角转半角减62548；半角转全角加62548
                }
                else if (source[i] == 12288)
                {
                    result.Append(' ');
                }
                else
                {
                    result.Append(source[i]);
                }
            }
            return result.ToString();
        }
        public string geturl(string newurl)
        {
            try
            {
                newurl = newurl.Trim();
                if (newurl.Contains("javascript") && newurl.Contains(":"))
                    return null;
                if (newurl.Length > 7 && (newurl.ToLower().Substring(0, 7) == "http://" || newurl.ToLower().Substring(0, 8) == "https://"))
                    return newurl;
                if (newurl == "")
                    return oldurls;
                string url = "";
                Uri olduri = new Uri(oldurls);
                if (newurl.Substring(0, 1) == "/")
                {
                    url = @"http://" + olduri.Host + newurl;
                }
                else
                {
                    string path = olduri.PathAndQuery;
                    if (path.Substring(path.Length - 1, 1) == "/")
                    {
                        url = "http://" + olduri.Host + path + newurl;
                    }
                    else
                    {
                        if (newurl.Length > 1 && newurl.Substring(0, 1) == "?")
                        {
                            url = @"http://" + olduri.Host + olduri.PathAndQuery + newurl;
                        }
                        else
                        {
                            if (olduri.PathAndQuery.Contains("/"))
                                path = path.Replace(olduri.PathAndQuery.Split('/')[olduri.PathAndQuery.Split('/').Length - 1], "");
                            else
                                path = "/";
                            url = @"http://" + olduri.Host + path + newurl;
                        }
                    }
                    Uri testuri = new Uri(url);
                }
                return url;
            }
            catch (Exception ex)
            {
                V3.Common.Log.LogNewline("[c14]在处理一个URL时出错了，错误：" + ex.Message + ",但并不影响任务执行！[/c]");
                return null;
            }
        }
    }
}
