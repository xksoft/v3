using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Model;
using V3.Common;
using V3Plugin;
using xEngine.Common;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Diagnostics;

namespace V3
{
    static class Program
    {
        public static bool Starting = true;
        public static frmMain f_frmMain;
        public static V3Form.frmTask f_frmTask;
        public static V3Form.frmReplaceTag f_frmReplaceTag;
        public static V3Form.frmPoint f_frmPoint;
        public static V3Form.frmTasks_Modify f_frmTasks_Modify;
        public static  ArrayList TaskRunList=new ArrayList();
        internal static int Level = 9999;
        public static string[] Args = null;
        public static List<string> ProxyList = new List<string>();
        public static List<string> ProxyListSuccess = new List<string>();
        public static long NetWorkUplaod =0;
        public static long NetWorkDownload =0;
        public static string ApplicationPath = Application.ExecutablePath.Remove(Application.ExecutablePath.LastIndexOf("\\"));
     
        public static Dictionary<string, ProcessPlugin> ProcessPluginList = new Dictionary<string, ProcessPlugin>();

        public static void Start()
        {

          
          
#if Final
            try
            {
                //处理未捕获的异常   
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                //处理UI线程异常   
                Application.ThreadException +=
                    new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                //处理非UI线程异常   
                AppDomain.CurrentDomain.UnhandledException +=
                    new System.UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
#endif
                DevExpress.UserSkins.BonusSkins.Register();
                DevExpress.Skins.SkinManager.EnableFormSkins();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-Hans");
                

                V3.Common.Console.VerStart();
                LoadData();
                f_frmMain = new frmMain();
              

           

                Application.Run(f_frmMain);

              


#if Final
            }
            catch (Exception ex)
            {

                if (f_frmMain!=null&&!f_frmMain.isclose)
                {
                    string str = "";
                    string strDateInfo = "出现应用程序未处理的错误：" + DateTime.Now.ToString() + "\r\n";

                    if (ex != null)
                    {
                        str = string.Format(strDateInfo + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                            ex.GetType().Name, ex.Message, ex.StackTrace);
                    }
                    else
                    {
                        str = string.Format("应用程序线程错误:{0}", ex);
                    }

                    if (!str.Contains("has been thrown by the target") || !str.Contains("正在终止线程") ||
                        !str.Contains("操作成功") || !str.Contains("参数无效") || !str.Contains("创建窗口句柄时出错") ||
                        !str.Contains("调用的目标发生了异常") || !str.Contains("Thread was being"))
                    {
                        Log.LogNewline("[c14][系统]系统出现问题导致软件即将关闭，正在紧急保存所有数据，请稍候....[/c]");
                        V3Helper.saveArticleDb();
                        V3Helper.saveHashDb();
                        V3Helper.saveKeywordDb();
                        V3Helper.saveReplaceDb();
                        V3Helper.saveTaskDb();
                        V3Helper.saveSendPointDb();
                        V3Helper.saveLinkDb();
                        V3.Common.V3Helper.saveDBS();
                        Log.LogNewline("[c12][系统]紧急数据保存已完成，现在可以关闭软件了！[/c]");
                        File.WriteAllText(ApplicationPath + "\\Error.txt", str, Encoding.UTF8);
                        MessageBox.Show(
                            str +
                            "\r\n\r\n系统出现错误，可能需要重新启动，所有数据已经成功保存，如果再次启动时数据异常可从备份中手动恢复，现在点击确定可能会退出软件。\r\n\r\n错误日志保存在程序目录下的Error.txt中，请将该文件发送给在线客服帮您解决问题！",
                            "无法继续运行", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
            }
#endif
        }
       
        public static string GetNetWork() 
        {
          
        

            lock ("NetWork")
            {
               
                    string r = "";
                    double Upload = Program.NetWorkUplaod/5/1024;
                    double Download = Program.NetWorkDownload/5/1024;
                    
                    if (Upload > 1024)
                    {
                        Upload = (Upload / 1024);
                        r = "上传" + Upload.ToString("0.0") + "M/S";
                    }
                    else 
                    {
                        r = "上传" + Upload.ToString("0") + "K/S";
                    }
                    if (Download > 1024)
                    {
                        Download = (Download / 1024);
                        r += "    下载" + Download.ToString("0.0") + "M/S";
                    }
                    else
                    {
                        r += "    下载" + Download.ToString("0") + "K/S";
                    }
                    Program.NetWorkUplaod = Program.NetWorkDownload = 0;
                    return r;
                
            }
        
        }

        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.PermissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.AllFlags));
            V3.Common.Console.StartConsole("站群引擎系统初始化中...");
            Start();

        }
      
#if Final
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            if (f_frmMain != null && !f_frmMain.isclose)
            {
                string str = "";
                string strDateInfo = "出现应用程序未处理的异常：" + DateTime.Now.ToString() + "\r\n";
                Exception error = e.Exception as Exception;
                if (error != null)
                {
                    str = string.Format(strDateInfo + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                         error.GetType().Name, error.Message, error.StackTrace);
                }
                else
                {
                    str = string.Format("应用程序线程错误:{0}", e);
                }
                if (!str.Contains("has been thrown by the target") || !str.Contains("正在终止线程") || !str.Contains("参数无效") || !str.Contains("操作成功") || !str.Contains("创建窗口句柄时出错") || !str.Contains("调用的目标发生了异常") || !str.Contains("Thread was being"))
                {
                     Log.LogNewline("[c11][系统]系统出现问题导致软件即将关闭，正在紧急保存所有数据，请稍候....[/c]");
                V3Helper.saveArticleDb();
                V3Helper.saveHashDb();
                V3Helper.saveKeywordDb();
                V3Helper.saveReplaceDb();
                V3Helper.saveTaskDb();
                V3Helper.saveSendPointDb();
                V3Helper.saveLinkDb();
                V3.Common.V3Helper.saveDBS();
                V3Helper.bakdbmain();
                Log.LogNewline("[c12][系统]紧急数据保存已完成！[/c]");
                       File.WriteAllText(ApplicationPath+"\\Error.txt",str,Encoding.UTF8);
                        MessageBox.Show(str + "\r\n\r\n系统出现错误，可能需要重新启动，所有数据已经成功保存，如果再次启动时数据异常可从备份中手动恢复，现在点击确定可能会退出软件。\r\n\r\n错误日志保存在程序目录下的Error.txt中，请将该文件发送给技术支持QQ:24271786，我们将在最短时间内修复！", "可能无法继续运行", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   }
            }
        }
       
        static void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
        {
            if (!f_frmMain.isclose)
            {
                string str = "";
                Exception error = e.ExceptionObject as Exception;
                string strDateInfo = "出现应用程序未处理的异常：" + DateTime.Now.ToString() + "\r\n";
                if (error != null)
                {
                    str = string.Format(strDateInfo + "Application UnhandledException:{0};\n\r堆栈信息:{1}", error.Message, error.StackTrace);
                }
                else
                {
                    str = string.Format("Application UnhandledError:{0}", e);
                }
                if (!str.Contains("has been thrown by the target") || !str.Contains("正在终止线程") || !str.Contains("参数无效") || !str.Contains("操作成功") || !str.Contains("创建窗口句柄时出错") || !str.Contains("调用的目标发生了异常"))
                {
                     Log.LogNewline("[c14][系统]系统出现问题可能导致软件关闭，正在紧急保存所有数据，请稍候....[/c]");
                V3Helper.saveArticleDb();
                V3Helper.saveHashDb();
                V3Helper.saveKeywordDb();
                V3Helper.saveReplaceDb();
                V3Helper.saveTaskDb();
                V3Helper.saveSendPointDb();
                V3Helper.saveLinkDb();
                V3.Common.V3Helper.saveDBS();
                V3Helper.bakdbmain();
                Log.LogNewline("[c12][系统]紧急数据保存已完成！[/c]");
                       File.WriteAllText(ApplicationPath+"\\Error.txt",str,Encoding.UTF8);
                       MessageBox.Show(str + "\r\n\r\n系统出现错误，可能需要重新启动，所有数据已经成功保存，如果再次启动时数据异常可从备份中手动恢复，现在点击确定可能会退出软件。\r\n\r\n错误日志保存在程序目录下的Error.txt中，请将该文件发送给技术支持QQ:24271786，我们将在最短时间内修复！", "可能无法继续运行", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
        }
#endif

        public static void LoadData()
        {

            ThreadPool.SetMaxThreads(15, 15);
            System.Diagnostics.Stopwatch sws = new System.Diagnostics.Stopwatch();
            sws.Start();
            Log.LogNewline("[c11]引擎正在进行本地系统初始化...[/c]");
            V3Helper.CheckDB();
            V3Helper.bakdbmain();
            V3Helper.LoadDbs();
            V3Helper.LoadArticleDb();
            V3Helper.LoadKeywordDb();
            V3Helper.LoadHashDb();
            V3Helper.LoadReplaceDb();
            V3Helper.LoadLinkDb();
            V3Helper.LoadSendPointDb();
            V3Helper.LoadTask();
            sws.Stop();
            Log.LogNewline(
                "[c12]加载了"+Model.V3Infos.MainDb.MyModels.Count+"个模块，" + Model.V3Infos.ArticleDb.Count + "个文章库," + Model.V3Infos.KeywordDb.Count +
                "个关键词库," + Model.V3Infos.HashDb.Count + "个哈希库," +
                Model.V3Infos.ReplaceDb.Count + "个替换库," + Model.V3Infos.LinkDb.Count + "个链接库," +
                Model.V3Infos.SendPointDb.Count + "个发布点和" + Model.V3Infos.TaskDb.Count + "个任务信息[/c]");
            Log.LogNewline("[c12]本地系统初始化完毕，耗时：" + sws.ElapsedMilliseconds + "毫秒[/c]");
            GC.Collect();
            Log.LogNewline("[c11]正在加插件数据，请稍候...[/c]");
            Program.ProcessPluginList = Bll.Plugin.LoadAllPlugins();
            Log.LogNewline("[c12]成功加载到" + Program.ProcessPluginList.Count + "个处理插件！[/c]");
            xEngine.Config.SetHttpTimeout(V3Infos.MainDb.GetTimeOut*1000, V3Infos.MainDb.PostTimeOut*1000,V3Infos.MainDb.GetTimeOut*1000);
        }

      
    }

 

}