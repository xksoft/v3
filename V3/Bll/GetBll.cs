using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using V3.Common;
using xEngine.Plugin.HtmlParsing;
using HtmlDocument = xEngine.Plugin.HtmlParsing.HtmlDocument;
namespace V3.Bll
{
    public class GetBll
    {
        HtmlDocument htmlDoc=new HtmlDocument();
        string htmlstr;
        Model.抓取相关模型.RulesEngLv1 rulesl1 = new Model.抓取相关模型.RulesEngLv1();
        public GetBll(string str,Model.抓取相关模型.RulesEngLv1 rules)
        {
            htmlstr = str;
            htmlDoc.LoadHtml(str);
            this.rulesl1 = rules;
        }
        private string _oldurl = "";
        public string oldurl
        {
            get
            {
                return this._oldurl;
            }
            set
            {
                this._oldurl = value;
            }
        }
        public string testEngRules(Model.抓取相关模型.RulesEngLv2 ruleslv2)
        {
            string result = "";
            HtmlNode firstNavItemNode = htmlDoc.DocumentNode.SelectSingleNode(ruleslv2.Rulesstr);
            if (firstNavItemNode != null)
            {

                if (ruleslv2.GetModel == 1)
                {
                    result = firstNavItemNode.InnerHtml;
                }
                else if (ruleslv2.GetModel == 2)
                {
                    result = firstNavItemNode.OuterHtml;
                }
                else if (ruleslv2.GetModel == 3)
                {
                    string[] strs = ruleslv2.Rulesstr.Split('@');
                    if (strs.Length > 1)
                    {
                        string abname = strs[1].Replace("[1]", "");
                        result = firstNavItemNode.Attributes[abname].Name;
                    }
                    else
                        return "";
                }
                else if (ruleslv2.GetModel == 4)
                {
                    string[] strs = ruleslv2.Rulesstr.Split('@');
                    if (strs.Length > 1)
                    {
                        string abname = strs[1].Replace("[1]", "");
                        result = firstNavItemNode.Attributes[abname].Value;
                    }
                    else
                        return "";
                }
            }
            return result;
        }
        public ArrayList testRgxRules(Model.抓取相关模型.RulesEngLv2 ruleslv2)
        {
        
            return Library.RegexHelper.GetArrayList(htmlstr,ruleslv2.Rulesstr);
          
        }
        public ArrayList getAllRules(int taskid)
        {
          
            if (rulesl1.IsuseYuchuli)
            {
                yuchuli();
             
                htmlDoc.LoadHtml(htmlstr);
            }
           
            ArrayList result = new ArrayList();
            V3.Common.Format format = new Format();
            format.oldurls = oldurl;
            if (rulesl1.GetModel == 1)
            { 
            #region 模式1
            if (rulesl1.OutModel == 1)
            {
                result.Add("");
                if (rulesl1.IsRgx)
                {
                    ArrayList temp = new ArrayList();
                    for (int i = 0; i < rulesl1.MyRules.Length; i++)
                    {
                        ArrayList temp2 = testRgxRules(rulesl1.MyRules[i]);
                        foreach (object s in temp2)
                        {
                            temp.Add(s);
                        }
                    }
                    for (int i = 0; i < temp.Count; i++)
                    {
                        if (i != temp.Count - 1)
                        {
                            string tempstr = temp[i].ToString();
                            if (tempstr != "")
                            {
                                result[0] += tempstr + rulesl1.HebingStr;
                            }
                        }
                        else
                        {
                            string tempstr = temp[i].ToString();
                            if (tempstr != "")
                            {
                                result[0] += tempstr;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < rulesl1.MyRules.Length; i++)
                    {
                        if (i != rulesl1.MyRules.Length - 1)
                        {
                            string temp = testEngRules(rulesl1.MyRules[i]);
                            if (temp != "")
                            {
                                result[0] += temp + rulesl1.HebingStr;
                            }
                        }
                        else
                        {
                            string temp = testEngRules(rulesl1.MyRules[i]);
                            if (temp != "")
                            {
                                result[0] += temp;
                            }
                        }

                    }
                }

            }
            #endregion 
            #region 模式2
            else if (rulesl1.OutModel == 2)
            {
               
                if (rulesl1.IsRgx)
                {
                    for (int i = 0; i < rulesl1.MyRules.Length; i++)
                    {
                        ArrayList temp2 = testRgxRules(rulesl1.MyRules[i]);
                        foreach (object s in temp2)
                        {
                            result.Add(s);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < rulesl1.MyRules.Length; i++)
                    {
                        string temp = testEngRules(rulesl1.MyRules[i]);
                        if (temp != "")
                        {
                            result.Add(temp);
                        }
                    }
                }
               
            }
            #endregion
            #region 模式3
            else if (rulesl1.OutModel == 3)
            {
                result.Add("");
                if (rulesl1.IsRgx)
                {
                    ArrayList temp = new ArrayList();
                    for (int i = 0; i < rulesl1.MyRules.Length; i++)
                    {
                        ArrayList temp2 = testRgxRules(rulesl1.MyRules[i]);
                        foreach (object s in temp2)
                        {
                            temp.Add(s);
                        }
                    }
                    for (int i = 0; i < temp.Count; i++)
                    {
                        string tempstr = temp[i].ToString(); ;
                        if (tempstr != "")
                        {
                            if (tempstr.Length > result[0].ToString().Length)
                                result[0] = tempstr;
                        }
                    }
                }
                else
                { 
                for (int i = 0; i < rulesl1.MyRules.Length; i++)
                {
                    string temp = testEngRules(rulesl1.MyRules[i]);
                    if (temp != "")
                    {
                        if (temp.Length > result[0].ToString().Length)
                            result[0] = temp;
                    }
                }
                }
            }
            #endregion
            #region 模式4
            else if (rulesl1.OutModel == 4)
            {
                result.Add("");
                if (rulesl1.IsRgx)
                {
                                        ArrayList temp = new ArrayList();
                    for (int i = 0; i < rulesl1.MyRules.Length; i++)
                    {
                        ArrayList temp2 = testRgxRules(rulesl1.MyRules[i]);
                        foreach (object s in temp2)
                        {
                            temp.Add(s);
                        }
                    }
                    for (int i = 0; i < temp.Count; i++)
                    {
                        string tempstr = testEngRules(rulesl1.MyRules[i]);
                        if (tempstr != "")
                        {
                            if (temp.Contains(rulesl1.HebingStr) && tempstr.Length > result[0].ToString().Length)
                            {
                                result[0] = tempstr;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < rulesl1.MyRules.Length; i++)
                    {
                        string temp = testEngRules(rulesl1.MyRules[i]);
                        if (temp != "")
                        {
                            if (temp.Contains(rulesl1.HebingStr) && temp.Length > result[0].ToString().Length)
                            {
                                result[0] = temp;
                            }
                        }
                    }
                }
            }
            #endregion
            }
            else if (rulesl1.GetModel == 2)
            {
                result = findurl(htmlDoc.DocumentNode);
            }
            else if (rulesl1.GetModel == 3)
            {
                result = findurl(htmlDoc.DocumentNode);
                for (int i = 0; i < result.Count; i++)
                {
                    string newurl = GetFullUrl(oldurl,result[i].ToString(),true);
                    if (newurl != null)
                        result[i] = newurl;
                }
            }
            else if (rulesl1.GetModel == 4)
            {
                for (int i = rulesl1.Toucong; i < rulesl1.Toudao;i=i+rulesl1.Zizeng )
                {
                    result.Add(rulesl1.Shengchengtou + i.ToString());
                }
            }
            //判断必须包含
            if (rulesl1.CheckStr1.Length > 0)
            {
                ArrayList temp=new ArrayList();
                for (int i = 0; i < result.Count; i++)
                {
                    bool have = false;
                    int iii = 0;
                    foreach (string str in rulesl1.CheckStr1)
                    {
                        if (result[i].ToString().Contains(str))
                        {
                            iii++;
                            if (!rulesl1.IsOrorAnd && iii > 0)
                            {
                                have = true;
                                break;
                            }
                            else if (rulesl1.IsOrorAnd && iii == rulesl1.CheckStr1.Length)
                            {
                                have = true;
                                break;
                            }   
                        }
                    }
                    if (have)
                        temp.Add(result[i]);
                }
                result = temp;
            }
            //判断必须不包含
            if (rulesl1.CheckStr2.Length > 0)
            {
                ArrayList temp = new ArrayList();
                for (int i = 0; i < result.Count; i++)
                {
                    foreach (string str in rulesl1.CheckStr2)
                    {
                        if (result[i].ToString().Contains(str))
                        {
                            temp.Add(result[i]);
                            break;
                        }
                    }
                }
                for (int i = 0; i < temp.Count; i++)
                {
                    result.Remove(temp[i]);
                }
            }
            //头部字符
            if (rulesl1.inhead != "")
            {
                for (int i = 0; i < result.Count; i++)
                {
                    result[i] = rulesl1.inhead + result[i].ToString();
                }
            }
            //尾部字符
            if (rulesl1.infoot != "")
            {
                for (int i = 0; i < result.Count; i++)
                {
                    result[i] = result[i].ToString() + rulesl1.infoot;
                }
            }
            //是否进行html化
            if (rulesl1.IsHtml && rulesl1.GetModel == 1)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    result[i] = format.ToHtml(result[i].ToString());
                }
            }
            //是否进行文章正规化
            if (rulesl1.IsFormat&&rulesl1.GetModel==1)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    result[i] = format.formatText(result[i].ToString(),taskid);
                }
            }
            for (int i = 0; i < result.Count; i++)
                result[i] = houchuli(result[i].ToString());
            return result;
        }
        #region 预处理
        public string yuchuli()
        {
            
            if (rulesl1.Istichun)
            {
                if (rulesl1.Tichunrule.GetModel == -1)
                {
                    ArrayList ary = testRgxRules(rulesl1.Tichunrule);
                    if (ary.Count > 0)
                    {
                        htmlstr = ary[0].ToString();
                    }
                    else 
                    {
                        htmlstr = "";
                    }
                }
                else
                    htmlstr = testEngRules(rulesl1.Tichunrule);
            }
           
            if (rulesl1.ReplaceData != null && rulesl1.ReplaceData.Count > 0)
            {
                foreach (KeyValuePair<string, string> ss in rulesl1.ReplaceData)
                {
                    try
                    {
                        htmlstr = Regex.Replace(htmlstr, ss.Key, ss.Value, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    }
                    catch (Exception ex)
                    {
                        htmlstr = "预处理替换时出错：标签" + ss.Key + " 原因" + ex.Message;
                    }
                }
                  
            }
            if (rulesl1.Ischulijuedui)
            {
               
                Dictionary<string, string> image = new Dictionary<string, string>();
                Dictionary<string, string> link = new Dictionary<string, string>();
                htmlstr = processImage(ref image, htmlstr, true, oldurl);//替换掉图片
               
                htmlstr = processLink(ref link, htmlstr, oldurl);//替换掉链接
               
                foreach (KeyValuePair<string, string> kvp in link)
                {
                    htmlstr = htmlstr.Replace(kvp.Key, kvp.Value);//将链接标签重新返回
                }
                foreach (KeyValuePair<string, string> kvp in image)
                {
                    htmlstr = htmlstr.Replace(kvp.Key, kvp.Value);//将图片标签重新返回
                }
            }
            return htmlstr;
        }
        string processImage(ref Dictionary<string, string> imageHash, string sourceString, bool isFormatPicUrl, string oldurls)
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
                    Regex regex2 = new Regex("<(img|IMG) (.*?)(src|SRC)=('|\"|\\\\\"|)(.+?)(\\.jpg|\\.JPG|\\.gif|\\.GIF|\\.png|\\.PNG|\\.bmp|\\.BMP|\\.jpeg|\\.JPEG)(.*?)>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    MatchCollection mc2 = regex2.Matches(m.Value);
                    foreach (Match m2 in mc2)
                    {
                        if (m2.Success)
                        {
                            string oldURL = m2.Groups["5"].Value + m2.Groups["6"].Value;
                            if (isFormatPicUrl && oldurls != "")
                            {
                                string newurl = GetFullUrl(oldurl,oldURL,true);
                                if (newurl != null)
                                    oldURL = newurl;
                            }
                            result = result.Replace(m.Value, "$formatImage" + i + "$");
                            imageHash.Add("$formatImage" + i + "$", "<img src=\"" + oldURL + "\" border=0 >");
                        }
                    }
                }
            }

            return result;
        }
        string processLink(ref Dictionary<string, string> Link, string sourceString, string oldurls)
        {
            string result = sourceString;
            Regex regex1 = new Regex("<a.*?</a>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
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
                            string newurl = GetFullUrl(oldurl,url.Replace("\"", "").Replace("'", ""),true);
                            if (newurl != null)
                                url = newurl;
                            result = result.Replace(m.Value, "$formatLink" + i + "$");
                            Link.Add("$formatLink" + i + "$", "<a href=" + url + " target=\"_blank\">" + content + "</a>");
                        }
                    }
                }
            }

            return result;
        }
        #endregion
        #region 后处理
        string houchuli(string str)
        {
            string result = str;
            if (rulesl1.HouReplaceData!=null&&rulesl1.HouReplaceData.Count > 0)
            {
                foreach (KeyValuePair<string, string> value in rulesl1.HouReplaceData)
                {
                    try
                    {
                        result = Regex.Replace(result, value.Key, value.Value);
                    }
                    catch (Exception ex)
                    {
                        result = "后处理替换时出错：标签" + value.Key + " 原因" + ex.Message;
                    }
                }
            }
            if (rulesl1.Guolvbiaoqian != null && rulesl1.Guolvbiaoqian.Length > 0)
            {
                foreach (string srt in rulesl1.Guolvbiaoqian)
                {
                    try
                    {
                        result = Regex.Replace(result, "(<" + srt + ">|<" + srt + ".+?>|</" + srt + ".?>|</" + srt + " .+?>)", "");
                    }
                    catch (Exception ex)
                    {
                        result = "后处理替换时出错：标签" + str + " 原因" + ex.Message;
                    }
                }
            }
            return result;
        }
        #endregion
        ArrayList findurl(HtmlNode htmlnode)
        {
            ArrayList result = new ArrayList();
            if (htmlnode.ChildNodes.Count > 0)
            {
                for (int i = 0; i < htmlnode.ChildNodes.Count; i++)
                {
                    ArrayList temp = findurl(htmlnode.ChildNodes[i]);
                    foreach (object s in temp)
                    {
                        if (!result.Contains(s.ToString().Replace("&amp;","&")))
                            result.Add(s.ToString().Replace("&amp;", "&"));
                    }
                }
            }
            if (htmlnode.Attributes.Count > 0&&htmlnode.Name.ToLower()=="a")
            {

                for (int i = 0; i < htmlnode.Attributes.Count; i++)
                {

                    if (htmlnode.Attributes[i].Name.ToLower() == "href")
                    {
                        if (!result.Contains(htmlnode.Attributes[i].Value.ToString().Replace("&amp;", "&")))
                            result.Add(htmlnode.Attributes[i].Value.ToString().Replace("&amp;", "&"));
                        break;
                    }
                }
            }

            return result;
        }
        //public  string geturl(string newurl)
        //{
        //    try
        //    {
        //        newurl = newurl.Trim();
        //        if (newurl.Contains("javascript") && newurl.Contains(":"))
        //            return null;
        //        if (newurl.Length > 7 && (newurl.ToLower().Substring(0, 7) == "http://" || newurl.ToLower().Substring(0, 8) == "https://"))
        //            return newurl;
        //        if (newurl == "")
        //            return oldurl;
        //        string url = "";
        //        Uri olduri = new Uri(oldurl);
        //        if (newurl.Substring(0, 1) == "/")
        //        {
        //            url = @"http://" + olduri.Host + newurl;
        //        }
        //        else
        //        {
        //            string path = olduri.PathAndQuery;
        //            if (path.Substring(path.Length - 1, 1) == "/")
        //            {
        //                url = "http://" + olduri.Host + path + newurl;
        //            }
        //            else
        //            {
        //                if (newurl.Length > 1 && newurl.Substring(0, 1) == "?")
        //                {
        //                    url = @"http://" + olduri.Host + olduri.PathAndQuery + newurl;
        //                }
        //                else
        //                {
        //                    if (olduri.PathAndQuery.Contains("/"))
        //                        path = path.Replace(olduri.PathAndQuery.Split('/')[olduri.PathAndQuery.Split('/').Length - 1], "");
        //                    else
        //                        path = "/";
        //                    url = @"http://" + olduri.Host + path + newurl;
        //                }
        //            }
        //        }
        //        Uri testuri = new Uri(url);
        //        return url;
        //    }
        //    catch (Exception ex)
        //    {
        //        V3.Common.Log.LogNewline("[c11]在处理一个URL时出错了，错误：" + ex.Message + ",但并不影响任务执行！[/c]");
        //        return null;
        //    }
        //}
        public static string GetFullUrl(string uri, string OldUrl, bool CrossDomain)
        {

            try
            {
                if (uri.Length==0) { if (OldUrl.ToLower().StartsWith("http://") || OldUrl.ToLower().StartsWith("https://")){ return OldUrl; } else { return ""; } }
                Uri olduri = new Uri(uri);
                OldUrl = OldUrl.Trim();
                if (OldUrl.Contains("javascript") && OldUrl.Contains(":"))
                    return "";
                if ((OldUrl.Contains("http://") || OldUrl.Contains("https://")) && CrossDomain)
                {//允许跨域

                    return ReplaceUrl(OldUrl);


                }
                if (OldUrl.Length > 7 && (OldUrl.ToLower().Substring(0, 7) == "http://" || OldUrl.ToLower().Substring(0, 8) == "https://") && (OldUrl.Contains("http://" + olduri.Host) || OldUrl.Contains("https://" + olduri.Host)))
                    return ReplaceUrl(OldUrl);
                if (OldUrl.Length > 7 && (OldUrl.ToLower().Substring(0, 7) == "http://" || OldUrl.ToLower().Substring(0, 8) == "https://") && (!OldUrl.Contains("http://" + olduri.Host) && !OldUrl.Contains("https://" + olduri.Host)))
                { return ""; }
                if (OldUrl == "")
                    return ReplaceUrl(OldUrl);
                string url = "";


                if (OldUrl.Substring(0, 1) == "/")
                {
                    url = @"http://" + olduri.Host + OldUrl;
                }
                else
                {
                    string path = olduri.PathAndQuery;
                    if (path.Substring(path.Length - 1, 1) == "/")
                    {
                        url = "http://" + olduri.Host + path + OldUrl;
                    }
                    else
                    {
                        if (OldUrl.Length > 1 && OldUrl.Substring(0, 1) == "?")
                        {
                            url = @"http://" + olduri.Host + olduri.AbsolutePath + OldUrl;
                        }
                        else
                        {
                            if (olduri.PathAndQuery.Contains("/"))
                                path = path.Replace(olduri.PathAndQuery.Split('/')[olduri.PathAndQuery.Split('/').Length - 1], "");
                            else
                                path = "/";
                            url = @"http://" + olduri.Host + path + OldUrl;
                        }
                    }
                }

            
                return ReplaceUrl(url);
            }
            catch (Exception ex)
            {

                V3.Common.Log.LogNewline("[c11]在处理一个URL时出错了，错误：" + ex.Message + ",但并不影响任务执行！[/c]");
                return "";
            }

        }
        public static string ReplaceUrl(string url)
        {

            return url.Replace("&amp;", "&");
        }
    }
}
