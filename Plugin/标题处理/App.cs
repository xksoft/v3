using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using V3Plugin;

namespace 百度原创度识别
{
    public class App : ProcessPlugin
    {
        public static frmMain frm=new frmMain();
        public static bool UseProxy=false;
        public UserControl MainControl
        {
            get
            {
              
                return frm;
            }
        }

        public  string Id
        {

            get { return "FDFB26D8-863E-F10E-90CF-BEC20767"; }
        }
        public  string ProcessName
        {
            get { return "百度原创度识别"; }
        }
        public  Dictionary<int, string> ArticleProcess(Dictionary<int, string> objects)
        {

            int count = GetBaiduCount(objects[0]);
            Console.WriteLine("[百度原创度识别]搜索到" + count + "个结果：" + objects[0]);
            if (count > Convert.ToInt32(Parameters[0]))
            {
                objects[0] = "";
                objects[1] = "";

            }
            else
            {
                objects[24] = count.ToString();
            }

            return objects;
        }


        public string Author
        {
            get { return "侠客小易 QQ：24271786"; }
        }

        public  string[] Parameters
        {
            get
            {
                string[]s=new string[2];
                s[0] = frm.textBox_maxCount.Text;
                s[1] = frm.textBox_proxy.Text;
                return s;
            }
            set
            {
                if (value.Length ==2)
                {
                    frm.textBox_maxCount.Text = value[0];
                    frm.textBox_proxy.Text = value[1];
                }

            }
        }

        public int  GetBaiduCount(string title)
        {
           HttpHelper http=new HttpHelper();
            HttpItem item=new HttpItem();
            int regetCount = 0;
            item.URL = "https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=1&tn=baidu&wd=%22"+HttpUtility.UrlEncode(title)+"%22";
            if (UseProxy)
            {
                item.ProxyIp = GetProxy();
            }
        reget:  string html=  http.GetHtml(item).Html;
            if (html.Contains("超时") || html.Contains("过于频繁") || html.Contains("403 Forbidden") ||
                html.Contains("状态：302") || html.Contains("状态：404") || html.Contains("请求失败") || html.Contains("输入以下验证码"))
            {
                UseProxy = true;
                regetCount++;
                if (regetCount < 5)
                {
                    goto reget;
                }
                else
                {
                    return 9999999;
                }
            }
            else
            {
                html = html.Replace("为您找到相关结果约", "为您找到相关结果");
            }
            Regex r=new Regex("(?<=百度为您找到相关结果).*?(?=个)");
            MatchCollection mc = r.Matches(html);
            if (mc.Count > 0)
            {
                return Convert.ToInt32(mc[0].Value.Replace("约", "").Replace(",", ""));
            }
            else
            {
                return 0;
            }
        }

        public string GetProxy()
        {
            string[] proxys = Parameters[1].Split('\n');
            if (proxys.Length == 0)
            {
                return "";
            }
            else
            {
                Random r=new Random();
                return proxys[r.Next(0,proxys.Length)];
            }
        }
    }
}