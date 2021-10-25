using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V3Plugin;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
namespace 模型插入关键词
{
    public class App : ProcessPlugin
    {
        public static frmMain frm = new frmMain();
        public static bool UseProxy = false;
        public static  Random r = new Random();
        public static string KeyWord = "";
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
        public string Id
        {

            get { return "F75B26D8-863E-F10E-90CF-BEdw0767"; }
        }
        public string ProcessName
        {
            get { return "模型插入关键词"; }
        }
        public Dictionary<int, string> ArticleProcess(Dictionary<int, string> objects)
        {
          
            if (KeyWords.Count == 0) { Console.WriteLine("模型插入关键词：任务关键词库无可用关键词，不插入！"); return objects; }
            KeyWord = GetKeyword();
            objects[0] = InsertKeywords(objects[0], Parameters[0], Convert.ToInt32(Parameters[1]), Convert.ToInt32(Parameters[2]));
            objects[29] = InsertKeywords(objects[29], Parameters[3], Convert.ToInt32(Parameters[4]), Convert.ToInt32(Parameters[5]));
            objects[2] = InsertKeywords(objects[2], Parameters[6], Convert.ToInt32(Parameters[7]), Convert.ToInt32(Parameters[8]));
            objects[1] = InsertKeywords(objects[1], Parameters[9], Convert.ToInt32(Parameters[10]), Convert.ToInt32(Parameters[11]));
            return objects;
        }

        public string InsertKeywords(string str,string loc,int from,int to)
        {
         
            int count = r.Next(from,to+1);
            if (loc == "开头")
            {
                for (int i = 0; i < count;i++ )
                {
                    str =KeyWord + str;
                }
            }
            else if (loc == "结尾")
            {
                for (int i = 0; i < count; i++)
                {
                    str = str + KeyWord;
                }
            }
            else if (loc == "中间")
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                Regex reg = new Regex("<.+?>");
                MatchCollection mc = reg.Matches(str);
                for (int i = 0; i < mc.Count; i++)
                {
                    string guid = Guid.NewGuid().ToString();
                    dic.Add("⊙" + guid + "ん",mc[i].Value);
                    str = str.Replace(mc[i].Value, "⊙" + guid + "ん");
                }
               
                for (int i = 0; i < count;i++ )
                { 
                    string ss = "";
                    int z=str.Length/2;
                    bool ins = false;
                    for (int c = 0; c < str.Length;c++ ) 
                    {
                        ss =ss+str[c];
                        if (ins == false && ss.Length >= z && (str[c] == 'ん' || dic.Count == 0))
                        {
                            string guid = Guid.NewGuid().ToString();
                            string k = KeyWord;
                            ss = ss + "⊙"+guid+"ん";
                            dic.Add("⊙" + guid + "ん", k);
                            ins = true;
                        }
                    }
                    str = ss;
                }

                foreach( KeyValuePair<string,string> k in dic)
                {
                    str = str.Replace(k.Key,k.Value);
                }
            }
            else if (loc == "随机")
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                Regex reg = new Regex("<.+?>");
                MatchCollection mc = reg.Matches(str);
                for (int i = 0; i < mc.Count; i++)
                {
                    string guid = Guid.NewGuid().ToString();
                    dic.Add("⊙" + guid + "ん", mc[i].Value);
                    str = str.Replace(mc[i].Value, "⊙" + guid + "ん");
                }
              
                for (int i = 0; i < count; i++)
                {  string ss = "";
                    
                    int index = r.Next(0,str.Length);
                    bool ins=false;
                    for (int c = 0; c < str.Length; c++)
                    {
                        ss = ss + str[c];
                        if (ins==false&&ss.Length >= index && (str[c] == 'ん' || dic.Count == 0))
                        {
                            string guid = Guid.NewGuid().ToString();
                            string k = KeyWord;
                            ss = ss + "⊙" + guid + "ん";
                            dic.Add("⊙" + guid + "ん", k);
                            ins = true;
                        }
                    }
                    str = ss;
                }

                foreach (KeyValuePair<string, string> k in dic)
                {
                    str = str.Replace(k.Key, k.Value);
                }
            }
            return str;
        
        }
        public string GetKeyword()
        {
           
            return KeyWords[r.Next(0, KeyWords.Count)];
        }

        public object Clone()
        {
            return new App();
        }

        public string Author
        {
            get { return "小易 QQ：24271786"; }
        }

        public string[] Parameters
        {
            get
            {
                string[] s = new string[] { "开头", "1", "2", "开头", "1", "2", "开头", "1", "2", "开头", "1", "2" };
                //frm.comboBox_title.SelectedIndex = 0;
                //frm.comboBox_keyword.SelectedIndex = 0;
                //frm.comboBox_des.SelectedIndex = 0;
                //frm.comboBox_content.SelectedIndex = 0;

                s[0] = frm.comboBox_title.SelectedItem.ToString();
                s[1] = frm.textBox_title_from.Text;
                s[2] = frm.textBox_title_to.Text;

                s[3] = frm.comboBox_keyword.SelectedItem.ToString();
                s[4] = frm.textBox_keyword_from.Text;
                s[5] = frm.textBox_keyword_to.Text;

                s[6] = frm.comboBox_des.SelectedItem.ToString();
                s[7] = frm.textBox_des_from.Text;
                s[8] = frm.textBox_des_to.Text;

                s[9] = frm.comboBox_content.SelectedItem.ToString();
                s[10] = frm.textBox_content_from.Text;
                s[11] = frm.textBox_content_to.Text;
                return s;
            }
            set
            {
               if(value==null)
               {
                   value = new string[] { "开头", "1", "2", "开头", "1", "2", "开头", "1", "2", "开头", "1", "2" };
               }
                if (value.Length == 12)
                {
                    frm.comboBox_title.SelectedItem = value[0];
                    frm.textBox_title_from.Text = value[1];
                    frm.textBox_title_to.Text = value[2];

                    frm.comboBox_keyword.SelectedItem = value[3];
                    frm.textBox_keyword_from.Text = value[4];
                    frm.textBox_keyword_to.Text = value[5];

                    frm.comboBox_des.SelectedItem = value[6];
                    frm.textBox_des_from.Text = value[7];
                    frm.textBox_des_to.Text = value[8];

                    frm.comboBox_content.SelectedItem = value[9];
                    frm.textBox_content_from.Text = value[10];
                    frm.textBox_content_to.Text = value[11];
                }

            }
        }



    }
}
