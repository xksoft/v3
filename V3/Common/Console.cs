using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace V3.Common
{
    public sealed class Console
    {

        public static void StartConsole(string title)
        {
            if (Model.V3Infos.MainDb.ShowConsole == 0)
            {
               
                xEngine.Log.ShowConsole();
                xEngine.Log.Title = title;
                int MaxWindowWidth = System.Console.LargestWindowWidth;
                int MaxWindowHeigh = System.Console.LargestWindowHeight;
                if (MaxWindowHeigh > 0 && MaxWindowWidth > 0)
                {
                    if (MaxWindowWidth < 150)
                    {
                        System.Console.WindowWidth = MaxWindowWidth;
                    }
                    else
                    {
                        System.Console.WindowWidth = 150;
                    }
                    if (MaxWindowHeigh < 40)
                    {
                        System.Console.WindowHeight = MaxWindowHeigh;
                    }
                    else
                    {
                        System.Console.WindowHeight = 40;
                    }
                }
            }
            else 
            {
                xEngine.Log.CloseConsole();
            }
              
        }
      
        public static void VerStart()
        {
         xEngine.Log.WriteLine("[c13]"+Properties.Resources.logo_str1+"[/c]");
       
          
            xEngine.Log.WriteLine(DateTime.Now.ToString() + "|[c11]软件控制台初始化完毕！系统正在和平台服务器进行通讯，请稍等片刻...[/c]");
            if (xEngine.License.MySoft != null)
            {
                Program.Level = xEngine.License.MyLicense.Level;

                xEngine.Log.WriteLine("[c12]  已成功登录平台！");
                xEngine.Log.WriteLine("");
                xEngine.Log.Write(DateTime.Now.ToString() + "|[c12]当前启动的授权:[/c]");

                xEngine.Log.Write("[c13]" + xEngine.License.MySoft.Name + "[/c]");

                xEngine.Log.Write("[c12]" + "  版本号:" + "[/c]");

                xEngine.Log.WriteLine("[c13]" + xEngine.License.MySoft.Version + "[/c]");
                xEngine.Log.WriteLine("");
                xEngine.Log.Write(DateTime.Now.ToString() + "|[c12]" + "当前授权ID:" + "[/c]");

                xEngine.Log.Write("[c13]" + 99.ToString() + "[/c]");

                xEngine.Log.Write("[c12]" + "  级别:" + "[/c]");
                xEngine.Log.Write("[c13]" + xEngine.License.MyLicense.Name + "[/c]");
                xEngine.Log.Write("[c12]" + "  最多站点数量:" + "[/c]");
                xEngine.Log.Write("[c13]" + xEngine.License.MyLicense.Level.ToString() + "[/c]");

                xEngine.Log.Write("[c12]" + "  到期时间:" + "[/c]");

                xEngine.Log.Write("[c12]" + xEngine.License.MyLicense.Expiration.ToString() + "[/c]");

                xEngine.Log.Write("[c12]" + "  剩余:" + "[/c]");

                xEngine.Log.Write("[c13]" +
                    (new TimeSpan(xEngine.License.MyLicense.Expiration.Ticks).Subtract(new TimeSpan(DateTime.Now.Ticks))
                        .Days + "天" +
                     (new TimeSpan(xEngine.License.MyLicense.Expiration.Ticks).Subtract(new TimeSpan(DateTime.Now.Ticks))
                         .Hours + "小时" +
                      (new TimeSpan(xEngine.License.MyLicense.Expiration.Ticks).Subtract(new TimeSpan(DateTime.Now.Ticks))
                          .Minutes + "分\r\n"))) + "[/c]");

                xEngine.Log.WriteLine("[c12]" + "=============================================================================================================================" + "[/c]");
            }
        }
    }
}
