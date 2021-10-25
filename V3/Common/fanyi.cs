using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace V3.Common
{
    class fanyi
    {
        #region http操作
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开
            return true;
        }
        public string SendDataByPost(string Url, string postDataStr, string refrere, bool isutf8)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                HttpWebRequest request = (HttpWebRequest)WebRequest.CreateDefault(new Uri(Url));
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Referer = refrere;
                request.AllowAutoRedirect = true;
                request.Accept = "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, */*";
                request.Headers.Add("Accept-Language", "zh-cn");
                request.Headers.Add("Cache-Control", "no-cache");
                request.Headers.Add("UA-CPU", "x86");
                request.UserAgent = " Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; Creative AutoUpdate v1.40.04)";
                Stream myRequestStream = request.GetRequestStream();
                if (isutf8)
                {
                    byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(postDataStr);
                    using (Stream writer = myRequestStream)
                    {
                        writer.Write(requestBytes, 0, requestBytes.Length);
                    }
                }
                else
                {
                    byte[] requestBytes = System.Text.Encoding.Default.GetBytes(postDataStr);
                    using (Stream writer = myRequestStream)
                    {
                        writer.Write(requestBytes, 0, requestBytes.Length);
                    }
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                string retString = null;
                if (isutf8)
                {
                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                    retString = myStreamReader.ReadToEnd();
                    myStreamReader.Close();
                    myResponseStream.Close();
                }
                else
                {
                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.Default);
                    retString = myStreamReader.ReadToEnd();
                    myStreamReader.Close();
                    myResponseStream.Close();
                }
                return retString;
            }
            catch (Exception ex)
            { return ex.Message; }
        }
        #endregion
        Regex r = new Regex("(?<=\"tgt\":\").*?(?=\"})");
        MatchCollection conn;
         //中英文自动互译
        //public string ZhongYingZiDong(string str)
        //{
        //   System.Collections.ArrayList ar=new System.Collections.ArrayList();
        //    Random rm = new Random();
        //    Regex rt = new Regex("<.+?>");

        //    Dictionary<string, string> keydic = new Dictionary<string, string>();

        //    conn = rt.Matches(str);

        //    for (int i = 0; i < conn.Count; i++)
        //    {

        //            string key =  rm.Next(10000, 99999).ToString();
        //            str = str.Replace(conn[i].Groups[0].Value, key);
        //            if (!keydic.ContainsKey(key)) 
                    
        //            { 
        //            keydic.Add(key, conn[i].Groups[0].Value);
        //        ar.Add(key);
        //            }
                    

        //    }

        //    string res = "";
        //    string html = SendDataByPost("http://fanyi.youdao.com/translate?smartresult=dict&smartresult=rule&smartresult=ugc&sessionFrom=http://www.baidu.com/s?wd=%D3%D0%B5%C0&rsv_bp=0&rsv_spt=3&inputT=4801", "type=AUTO&i=" +System.Web.HttpUtility.UrlEncode( str,Encoding.UTF8) + "&doctype=json&xmlVersion=1.4&keyfrom=fanyi.web&ue=UTF-8&typoResult=true&flag=false", "http://translate.google.com/#", true);
        // conn = r.Matches(html);
        //    for (int i = 0; i < conn.Count;i++ )
        //    {
        //       res+= conn[i].Groups[0].Value;
        //    }
        //    res = res.Replace("\\", "").Replace("�", "");
        //    for (int i = 0; i < keydic.Count;i++ )
        //    {
        //        res = res.Replace(ar[i].ToString(),keydic[ar[i].ToString()].ToString());
        //    }

        //   return res;
        //}


        /// <summary>
        /// 中文变英文 GG
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string zhongtoying(string str)
        {
        

            return regex(SendDataByPost("http://translate.google.cn//translate_a/t", "client=t&text=" + HttpUtility.UrlEncode(str, Encoding.UTF8) + "&hl=zh-CN&sl=zh-CN&tl=en&multires=1&pc=0&sc=1", "http://translate.google.com/#", true), "(?<=\\[\").*?(?=\")").Replace("\\r\\n", "\r\n").Replace("\\n", "\n");
        }
        /// <summary>
        /// 英文变中文 GG
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string yingtozhong(string str)
        {
            return regex(SendDataByPost("http://translate.google.cn//translate_a/t", "client=t&text=" + HttpUtility.UrlEncode(str, Encoding.UTF8) + "&hl=zh-CN&sl=en&tl=zh-CN&multires=1&pc=0&sc=1", "http://translate.google.com/#", true), "(?<=\\[\").*?(?=\")").Replace("\\r\\n", "\r\n").Replace("\\n", "\n").Replace("\\", "");
        }
        string regex(string str, string rex)
        {
            string resulte = "";
            MatchCollection okok = Regex.Matches(unitostr(str), rex, RegexOptions.IgnoreCase);
            Dictionary<string, int> cf = new Dictionary<string, int>();
            if (okok.Count > 0)
            {
                for (int i = 0; i < okok.Count; i++)
                {
                    resulte += okok[i].Value;
                }
            }
            return resulte;
        }
        string unitostr(string str)
        {
            string text = str;
            string strPattern = "(?<code>\\\\u[A-F0-9]{4})";
            do
            {
                Match m = Regex.Match(text, strPattern, RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    string strValue = m.Groups["code"].Value;
                    int i = System.Int32.Parse(strValue.Substring(2, 4), System.Globalization.NumberStyles.HexNumber);
                    char ch = Convert.ToChar(i);
                    text = text.Replace(strValue, ch.ToString());
                }
                else
                {
                    break;
                }
            }
            while (true);
            return text;
        }

    }
}
