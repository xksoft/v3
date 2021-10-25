using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V3Plugin;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
namespace 视频图片插入
{

    public class App : ProcessPlugin
    {
        public static frmMain frm = new frmMain();
        public static Random r = new Random();
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

            get { return "hurew89432yuhjh-863E-F10E-90CF-BEdw0767"; }
        }
        public string ProcessName
        {
            get { return "视频图片插入"; }
        }
        public Dictionary<int, string> ArticleProcess(Dictionary<int, string> objects)
        {
            string path = Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "");
            path = path.Remove(path.LastIndexOf("/"));
            string imgpath = path + "/图片.txt";
            string videpath = path + "/视频.txt";
            if(!File.Exists(imgpath))
            {
                Console.WriteLine("视频图片插入插件：请把“图片.txt”文件放到插件目录下！");

            }
            else if (!File.Exists(videpath))
            {
                Console.WriteLine("视频图片插入插件：请把“视频.txt”文件放到插件目录下！");
            }
            else 
            {
                int from = 0;
                int to = 0;
                int.TryParse( Parameters[0],out from);
                int.TryParse( Parameters[1],out to);
                objects[1] = Insert(objects[1],from,to,true,objects[0]);
                int.TryParse(Parameters[2], out from);
                int.TryParse(Parameters[3], out to);
                objects[1] = Insert(objects[1], from, to, false,objects[0]);
            }
            return objects;
        }

        public string Insert(string str, int from, int to, bool isimg,string title)
        {

            int count = r.Next(from, to + 1);

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
            {
                string ss = "";
                int index = r.Next(0, str.Length);
                bool ins = false;
                for (int c = 0; c < str.Length; c++)
                {
                    ss = ss + str[c];
                    if (ins == false && ss.Length >= index && (str[c] == 'ん' || dic.Count == 0))
                    {
                        string guid = Guid.NewGuid().ToString();
                        string k = "";
                        if (isimg) { k = GetImgOrVideo(true,title); }
                        else { k = GetImgOrVideo(false,title); }
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
            return str;

        }
        public string GetImgOrVideo(bool isimg,string title)
        {
            lock ("GetImg")
            {
                try
                {
                    string path = Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "");
                    path = path.Remove(path.LastIndexOf("/"));
                    string imgpath = path + "/图片.txt";
                    string videpath = path + "/视频.txt";
                    List<string> imgs = new List<string>();
                    if (isimg) {
                        Console.WriteLine("[视频图片插入]:正在读取图片.txt...");
                        imgs = File.ReadAllLines(imgpath, Encoding.Default).ToList();
                        Console.WriteLine("[视频图片插入]:读取到"+imgs.Count+"个图片");
                    }
                    else {
                        Console.WriteLine("[视频图片插入]:正在读取视频.txt...");
                        imgs = File.ReadAllLines(videpath, Encoding.Default).ToList();
                        Console.WriteLine("[视频图片插入]:读取到" + imgs.Count + "个视频");
                    }
                rget: if (imgs.Count > 0)
                    {
                        string img = imgs[0];
                        if (img.Length > 0)
                        {
                            if (isimg)
                            {
                                img = "<img alt=\""+title+"\" src=\"" + img + "\" />";
                            }
                            else
                            {
                                img = "<embed src=\"" + img + "\" allowFullScreen=\"true\" quality=\"high\" width=\"480\" height=\"400\" align=\"middle\" allowScriptAccess=\"always\" type=\"application/x-shockwave-flash\"></embed>";

                            }
                            imgs.RemoveAt(0);
                            string content = "";
                            foreach (string s in imgs)
                            {
                                content += s + "\r\n";
                            }
                            if (isimg)
                            {
                                File.WriteAllText(imgpath, content, Encoding.Default);
                            }
                            else
                            {
                                File.WriteAllText(videpath, content, Encoding.Default);
                            }
                            return img;
                        }
                        else
                        {
                            imgs.RemoveAt(0);
                            goto rget;
                        }
                    }
                    else
                    {
                        if (isimg)
                        {
                            Console.WriteLine("视频图片插入插件：“图片.txt”文件中已经没有数据，不进行插入！");

                        }
                        else
                        {
                            Console.WriteLine("视频图片插入插件：“视频.txt”文件中已经没有数据，不进行插入！");
                        }
                        return "";
                    }
                }
                catch (Exception error) { return ""; }
            }
           
            

        }

        public object Clone()
        {
            return new App();
            //base.MemberwiseClone();
        }

        public string Author
        {
            get { return ""; }
        }

        public string[] Parameters
        {
            get
            {
                string[] s = new string[] { "1", "2",  "1", "2"};

                s[0] = frm.textBox_img_from.Text;
                s[1] = frm.textBox_img_to.Text;

             
                s[2] = frm.textBox_video_from.Text;
                s[3] = frm.textBox_video_to.Text;

                return s;
            }
            set
            {
                if (value == null)
                {
                    value = new string[] { "1", "2",  "1", "2" };
                }
                if (value.Length == 4)
                {
                   
                    frm.textBox_img_from.Text = value[0];
                    frm.textBox_img_to.Text = value[1];

                  
                    frm.textBox_video_from.Text = value[2];
                    frm.textBox_video_to.Text = value[3];

                   
                }

            }
        }



    }
}
