using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 文件下载
{
   public class UrlHelper
    {


        public static string GetFullUrl(string uri, string OldUrl, bool CrossDomain)
        {

            try
            {
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

                Uri testuri = new Uri(url);
                if (url.Contains("?prefix="))
                {
                    string hh = url;

                }
                return ReplaceUrl(url);
            }
            catch (Exception ex)
            {

                return "";
            }

        }

        public static string ReplaceUrl(string url)
        {

            return url.Replace("&amp;", "&");
        }
    }
}
