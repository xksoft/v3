using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using V3.Common;
using xEngine.Plugin.HtmlParsing;

namespace V3.Common
{
    class AiGet
    {
        static List<string> findDiv(HtmlNode htmlnode)
        {
            List<string> result = new List<string>();
            if (htmlnode.ChildNodes.Count > 0)
            {
                for (int i = 0; i < htmlnode.ChildNodes.Count; i++)
                {
                    List<string> temp = findDiv(htmlnode.ChildNodes[i]);
                    foreach (string s in temp)
                    {
                        result.Add(s);
                    }
                }
            }
            if (htmlnode.Name.ToLower() == "div")
            {
                if (!result.Contains(htmlnode.InnerHtml))
                {
                    result.Add(htmlnode.InnerHtml);
                }
            }

            return result;
        }
        static String ToTxt(String str)
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
            sb.Replace("\r", "");
            sb.Replace("\n", "");
            return sb.ToString();
        }
        public static string getTitle(HtmlNode htmlnode)
        {
            string title = "";
            string oldtitle = "";
            int changdu = 0;
            findTitle(htmlnode, ref title, ref oldtitle, ref changdu);
            if (title == oldtitle)
            {

                string[] sArray = null;
                try
                {
                    string result = oldtitle;
                    result = result.Replace("_", "`").Replace("-", "`").Replace("—", "`'").Replace("&nbsp;", " ");
                    sArray = result.Split('`');
                }
                catch { return null; }
                title = sArray[0];
            }
            return title;
        }
         static void findTitle(HtmlNode htmlnode, ref string title, ref string oldtitle, ref int changdu)
        {
            if (htmlnode.ChildNodes.Count > 0)
            {
                for (int i = 0; i < htmlnode.ChildNodes.Count; i++)
                {
                    findTitle(htmlnode.ChildNodes[i], ref title, ref oldtitle, ref changdu);
                }
            }
            if (htmlnode.Id.ToLower() == "title" || htmlnode.Name.ToLower() == "title")
            {
                if (ToTxt(htmlnode.InnerText).Length >= oldtitle.Length)
                {
                    oldtitle = ToTxt(htmlnode.InnerText);
                    title = ToTxt(htmlnode.InnerText);
                }
            }
            else if (ToTxt(htmlnode.InnerText) != "" && oldtitle.Contains(ToTxt(htmlnode.InnerText)) && !(htmlnode.Id.ToLower() == "head" || htmlnode.Name.ToLower() == "head"))
            {
                if (ToTxt(htmlnode.InnerText).Length >= changdu)
                {
                    changdu = ToTxt(htmlnode.InnerText).Length;
                    title = ToTxt(htmlnode.InnerText);
                }
            }
        }
        public static int CompareDinosByChineseLength(string x, string y)
        {
            V3.Common.Format format=new Format();
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y == null)
                {
                    return 1;
                }
                else
                {
                    Regex r = new Regex("[\u4e00-\u9fa5]");
                    float xCount = (float)(r.Matches(x).Count) / (float)x.Length;
                    float yCount = (float)(r.Matches(y).Count) / (float)y.Length;

                    int retval = xCount.CompareTo(yCount);

                    if (retval != 0)
                    {
                        return retval;
                    }
                    else
                    {
                        return x.CompareTo(y);
                    }
                }
            }
        }
        public static string GetMainContent(HtmlNode node,int taskid)
        {
            string result = "";
            try
            {
                V3.Common.Format ff = new Common.Format();
                //1、获取网页的所有div标签
                List<string> list = findDiv(node);

                //2、去除汉字少于200字的div
                List<string> needToRemove = new List<string>();
                foreach (string s in list)
                {
                    Regex r = new Regex("[\u4e00-\u9fa5]");
                    if (r.Matches(s).Count < 200){
                        needToRemove.Add(s);
                    }
                }
                foreach (string s in needToRemove)
                {
                    list.Remove(s);
                }
                needToRemove.Clear();

                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = ff.formatText(list[i],taskid);
                }
                //3、把剩下的div按汉字比例多少倒序排列,
                list.Sort(CompareDinosByChineseLength);
                if (list.Count < 1)
                {
                    return "";
                }
                result = list[list.Count - 1];
            }
            catch
            {
                return null;
            }
            V3.Common.Format format = new Common.Format();
            return format.formatText(result,taskid);
        }

     
    }
}
