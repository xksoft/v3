using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Threading;


namespace V3.Common
{
    public class Log
    {

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="log">日志内容</param>
        /// <param name="type">颜色1 正常（黑色） 2系统消息1（蓝色） 3系统消息  2（绿色） 4轻度异常（粉红） 5中度异常（橙色） 6重度异常（红色）</param>
        public static  void LogNewline(string log)
        {
            Thread.Sleep(20);
            lock ("LogNewline")
            {


                if (!work && (Model.V3Infos.MainDb.ShowConsole == 0 || Program.Starting))
                {
                    work = true;
                    try
                    {
                        xEngine.Log.WriteLine(DateTime.Now.ToString() + "|" + log);
                        try
                        {
                            String path = getDirectory() + getFilename();
                            File.AppendAllText(path, DateTime.Now.ToString() + "|" + log.Replace("[/c]", "") + "\r\n");

                        }
                        catch { }
                    }
                    catch
                    {

                    }
                    finally
                    {
                        work = false;
                    }
                }
                else if(!work)
                {
                    work = true;
                    try
                    {
                        
                        try
                        {
                            String path = getDirectory() + getFilename();
                            File.AppendAllText(path, DateTime.Now.ToString() + "|" + log.Replace("[/c]", "") + "\r\n");

                        }
                        catch { }
                    }
                    catch
                    {

                    }
                    finally
                    {
                        work = false;
                    }
                }
            }
        }
        public static void LogNewlineNosave(string log)
        {
            lock ("LogNewline")
            {
                if (!work && (Model.V3Infos.MainDb.ShowConsole == 0 || Program.Starting))
                {
                    if (!work)
                    {
                        work = true;
                        try
                        {

                            xEngine.Log.WriteLine(DateTime.Now.ToString() + "|" + log);

                        }
                        catch { }
                        finally { work = false; }
                    }
                }
            }
        }

        static bool work = false;
        private static string getDirectory()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
   
            path += @"\运行日志\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
        private static string getFilename()
        {
            string filename = DateTime.Now.ToString("yyyyMMdd") + ".txt";
            return filename;
        }
        public static void LogSendError(string log)
        {
            try
            {
                String path = getDirectory() + getFilename();
             V3.Common.Log.LogNewline((DateTime.Now.ToString() + "|" + "[c14]发布文章时遇到一个无法识别的返回信息，请将" + path + "/里的返回信息提供给发布模块设计者，让它完善该模块的返回信息！[/c]"));
                File.AppendAllText(path, "\n" + DateTime.Now.ToString() + "|" + ("发布失败返回信息如下：\r\n\r\n" + log)+"\r\n");
                Thread.Sleep(20);
            }
            catch { }
        }
    }
}
