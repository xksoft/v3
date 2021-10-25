#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using xEngine.Model.Execute.Http;
using xEngine.Plugin.HtmlParsing;

#endregion

namespace xEngine.Common
{
    /// <summary>
    /// </summary>
    public class GetArticle
    {
        private static List<string> FindDiv(HtmlNode htmlnode)
        {
            var result = new List<string>();
            if (htmlnode.ChildNodes.Count > 0)
            {
                result.AddRange(htmlnode.ChildNodes.SelectMany(FindDiv));
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

        private static string ToTxt(string str)
        {
            if (str == null || str.Equals(""))
            {
                return str;
            }

            var sb = new StringBuilder(str);
            sb.Replace("&nbsp;", " ");
            sb.Replace("<br>", "\r\n");
            sb.Replace("&lt;", "<");
            sb.Replace("&gt;", ">");
            sb.Replace("&amp;", "&");
            sb.Replace("\r", "");
            sb.Replace("\n", "");
            return sb.ToString();
        }

        private static string GetTitle(HtmlNode htmlnode)
        {
            var title = "";
            var oldtitle = "";
            var changdu = 0;
            FindTitle(htmlnode, ref title, ref oldtitle, ref changdu);
            if (title == oldtitle)
            {
                string[] sArray;
                try
                {
                    var result = oldtitle;
                    result = result.Replace("_", "`").Replace("-", "`").Replace("—", "`'").Replace("&nbsp;", " ");
                    sArray = result.Split('`');
                }
                catch
                {
                    return null;
                }
                title = sArray[0];
            }
            return title;
        }

        private static void FindTitle(HtmlNode htmlnode, ref string title, ref string oldtitle, ref int changdu)
        {
            if (htmlnode.ChildNodes.Count > 0)
            {
                foreach (var t in htmlnode.ChildNodes)
                {
                    FindTitle(t, ref title, ref oldtitle, ref changdu);
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
            else if (ToTxt(htmlnode.InnerText) != "" && oldtitle.Contains(ToTxt(htmlnode.InnerText)) &&
                     !(htmlnode.Id.ToLower() == "head" || htmlnode.Name.ToLower() == "head"))
            {
                if (ToTxt(htmlnode.InnerText).Length >= changdu)
                {
                    changdu = ToTxt(htmlnode.InnerText).Length;
                    title = ToTxt(htmlnode.InnerText);
                }
            }
        }

        private static int CompareDinosByChineseLength(string x, string y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            var r = new Regex("[\u4e00-\u9fa5]");
            var xCount = r.Matches(x).Count/(float) x.Length;
            var yCount = r.Matches(y).Count/(float) y.Length;

            var retval = xCount.CompareTo(yCount);

            return retval != 0 ? retval : string.Compare(x, y, StringComparison.Ordinal);
        }

        private static string GetMainContent(HtmlNode node, string find)
        {
            string result;
            try
            {
                //1、获取网页的所有div标签
                var list = FindDiv(node);


                var needToRemove = new List<string>();
                foreach (var s in list)
                {
                    var r = new Regex("[\u4e00-\u9fa5]");
                    //去除汉字少于200字的div
                    if (r.Matches(s).Count < 200)
                    {
                        needToRemove.Add(s);
                    }
                    //去除不包含特征的div
                    if (!s.Contains(find))
                    {
                        needToRemove.Add(s);
                    }
                }
                foreach (var s in needToRemove)
                {
                    list.Remove(s);
                }
                needToRemove.Clear();

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
                return "";
            }

            return result;
        }

        /// <summary>
        ///     智能提取网页源码中的文章和标题
        /// </summary>
        /// <param name="htmlcode">网页源码</param>
        /// <returns>tag的str01是标题，str02是内容</returns>
        public static XTag Get(string htmlcode)
        {
            return Get(htmlcode, "");
        }

        /// <summary>
        ///     智能提取网页源码中的文章和标题
        /// </summary>
        /// <param name="htmlcode">网页源码</param>
        /// <param name="findstr">筛选字符</param>
        /// <returns>tag的str01是标题，str02是内容</returns>
        public static XTag Get(string htmlcode, string findstr)
        {
            var tag = new XTag();
            var htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(htmlcode);
            tag.Str01 = GetTitle(htmldoc.DocumentNode);
            tag.Str02 = GetMainContent(htmldoc.DocumentNode, findstr);
            return tag;
        }
    }
}