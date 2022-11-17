using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.IO;
using V3Plugin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace 百度翻译
{
    public class App : ProcessPlugin
    {
        public static frmMain frm=new frmMain();
        public class TransResult {
            public string dst = "";
        }
        public class TransData {

            public string from = "";
            public string to = "";
            public string q = "";
            public string termIds = "";
        }
        public UserControl MainControl
        {
            get
            {
              
                return frm;
            }
        }
        public List<string> KeyWords
        {
            get;
            set;
        }
        public  string Id
        {

            get { return "FDFB26D8-863E-F10E-90CF-JUHDHUIYU78236h"; }
        }
        public  string ProcessName
        {
            get { return "百度翻译"; }

        }
        public  Dictionary<int, string> ArticleProcess(Dictionary<int, string> objects)
        {
            if (objects[10]=="zh")
            {
                return objects;
            }
            Console.WriteLine("调用百度翻译");
            string AccessToken = GetBaiDuAccessToken(Parameters[0], Parameters[1]);
            Console.WriteLine("获取到AccessToken：" +AccessToken);
            if (objects[0].Trim().Length>0)
            {
                objects[0] = FanYi(AccessToken, objects[0]);
            }
            if (objects[1].Trim().Length > 0)
            {
                List<string> contents = new List<string>();
                string content = "";
                bool inhtml = true;//如果进入html标签，则不截断
                for (int i = 0; i < objects[1].Length; i++)
                {
                    if (objects[1][i] == '<') {
                        inhtml = true;
                    }
                    else if (objects[1][i] == '>')
                    {
                        inhtml = false;

                    }
                    content += objects[1][i];
                    if (content.Length >= 4000 &&!inhtml&& (
                        content.EndsWith(" ")
                        || content.EndsWith(".")
                        || content.EndsWith("。")
                        || content.EndsWith(",")
                        || content.EndsWith("，")
                        || content.EndsWith("!")
                        || content.EndsWith("！")))
                    {
                        contents.Add(content);
                        content = "";
                    }
                }
                if (content.Length > 0) { contents.Add(content); }
                objects[1] = "";
                foreach (string c in contents)
                {
                    Thread.Sleep(1100);
                    objects[1] += FanYi(AccessToken, c);
                }

                
            }
            objects[10] = "zh";


            return objects;
        }


        public string Author
        {
            get { return ""; }
        }

        public  string[] Parameters
        {
            get
            {
                string[]s=new string[]{"","","en"};
                s[0] = frm.textBox_ApiKey.Text;
                s[1] = frm.textBox_SecretKey.Text;
                s[2]=frm.radioButton_en.Checked?"en":"zh";
                return s;
            }
            set
            {
                if (value == null) {value=new string[]{"","","en"}; }
                if (value.Length ==3)
                {
                    frm.textBox_ApiKey.Text = value[0];
                    frm.textBox_SecretKey.Text = value[1];
                    frm.radioButton_en.Checked =( value[2] == "en");
                    frm.radioButton_cn.Checked = (value[2] != "en");
                }

            }
        }
        public string FanYi(string accesstoken, string content)
        {
            retry: Dictionary<string, string> dics = new Dictionary<string, string>();
            string newcontent = "";
            string oldcontent = content;
            Regex r = new Regex("<.+?>");
            MatchCollection mc = r.Matches(content);
            for (int i = 0; i < mc.Count; i++)
            {
                string htmltag = mc[i].Value;
                oldcontent = oldcontent.Replace(htmltag, "[" + i + "]");
                if (!dics.ContainsKey("[" + i + "]"))
                {
                    dics.Add("[" + i + "]", htmltag);
                }
            }
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem();
            item.Method = "POST";
            item.Encoding = Encoding.UTF8;
            item.PostEncoding = Encoding.UTF8;
            item.URL = "https://aip.baidubce.com/rpc/2.0/mt/texttrans/v1?access_token=" + accesstoken;
            TransData transData = new TransData();
            transData.from = "auto";
            transData.to = Parameters[2];
            transData.q = oldcontent;

            item.Postdata = JsonConvert.SerializeObject(transData);
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            JObject jobject = (JObject)JsonConvert.DeserializeObject(html);
            if (jobject["error_code"] != null)
            {
                if (jobject["error_code"].ToString() == "19")
                {
                    Console.WriteLine("百度翻译插件接口异常：翻译额度已用完，请登录百度控制台购买额度");
                    Thread.Sleep(100000);
                    goto retry;
                }
                if (jobject["error_code"].ToString() == "110" || jobject["error_code"].ToString() == "111")
                {
                    Console.WriteLine("百度翻译插件接口异常：Access Token失效，正在重新获取...");
                    accesstoken = GetBaiDuAccessToken(Parameters[0], Parameters[1]);
                    Console.WriteLine("获取到AccessToken：" + accesstoken);
                    Thread.Sleep(1000);
                    goto retry;
                }
                else
                {
                    Console.WriteLine("百度翻译插件接口异常：" + jobject["error_msg"].ToString());
                    Thread.Sleep(10000);
                    goto retry;
                }

            }
            else
            {

                List<TransResult> transResults = JsonConvert.DeserializeObject<List<TransResult>>(((JArray)jobject["result"]["trans_result"]).ToString());
                newcontent = "";
                foreach (TransResult transResult in transResults)
                {
                    if (transResult.dst.Length > 0)
                    {
                        newcontent += transResult.dst + (transResults.Count > 1 ? "\r\n" : "");
                    }
                }

                foreach (string key in dics.Keys.ToArray())
                {
                    newcontent = newcontent.Replace(key, dics[key]);

                }
                return newcontent;

            }
        }
        
        public string GetBaiDuAccessToken(string apikey, string secretkey)
        {

            string filename = "at.txt";
            string token = "";
            if (File.Exists(filename))
            {
                return File.ReadAllText(filename);
            }
            else
            {
                HttpHelper http = new HttpHelper();
                HttpItem item = new HttpItem();
                item.Method = "GET";
                item.URL = "https://aip.baidubce.com/oauth/2.0/token?grant_type=client_credentials&client_id=" + apikey + "&client_secret=" + secretkey;
                HttpResult result = http.GetHtml(item);
                Regex r = new Regex(@"(?<=access_token"":"").*?(?="")");
                MatchCollection mc = r.Matches(result.Html);
                if (mc.Count > 0)
                {
                    File.WriteAllText(filename, mc[0].Value);
                    return mc[0].Value;
                }
                else { return ""; }




            }

        }
   

    

        public object Clone()
        {
            return new App();
        }
    }
}