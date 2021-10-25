using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using V3Plugin;

namespace 文件下载
{
    public class App:ProcessPlugin
    {
        public frmMain frm = new frmMain();
        public UserControl MainControl
        {
            get {
               return frm;
            }
        }

        //插件唯一编号，供V3识别，不允许重复，在http://www.xiake.org/guid.html页面获取，一旦设置请不要修改
        public string Id {

            get { return "3242342-252-g-s-4-52-t-fw-e"; }
        }
        public List<string> KeyWords
        {
            get;
            set;
        }
        //插件进程名称，供V3内部代码调用显示
        public string ProcessName
        {
            get { return "文件下载"; }
        }
        //个人信息以及联系方式，插件出现错误即会显示该信息以便用户联系反馈
        public string Author
        {
            get { return "小易 QQ：24271786"; }
        }

        //由V3传入的30个文章模型值，从0-29
        public Dictionary<int, string> ArticleProcess(Dictionary<int, string> objects)
        {
            if (objects.Count == 30)
            {
                for(int i=0;i<30;i++)
                {
                    if (Parameters[0].Contains("[模型值"+(i+1)+"]"))
                    {
                        objects[i] = GetFiles(objects[i],objects[28]);
                    }
                }  
            }
            return objects;
        }
     

        //自定义配置参数，由V3调用时会触发set，在set里面需对参数进行判断。保存V3任务时会触发get，注意对参数初始化，插件二次升级时请做好默认参数判断。
        public string[] Parameters
        {
            get
            {
               string[] s = new string[]{"","","","","","","","","","",""};

                s[0] = frm.textBox_Model.Text;
                if (frm.checkBox_type_all.Checked)
                {
                    if (!s[1].Contains("<all>"))
                    {
                        s[1] = s[1] + "<all>";
                    }
                }
                else
                {
                    s[1] = s[1].Replace("<all>","");
                }

                if (frm.checkBox_type_image.Checked)
                {
                    if (!s[1].Contains("<jpg><bmp><gif><png>"))
                    {
                        s[1] = s[1] + "<jpg><bmp><gif><png>";
                    }
                }
                else
                {
                    s[1] = s[1].Replace("<jpg><bmp><gif><png>", "");  
                }

                if (frm.checkBox_type_video.Checked)
                {
                    if (!s[1].Contains("<mp4><flv><avi><mkv><wma><rmvb><rm><f4v><m4v>"))
                    {
                        s[1] = s[1] + "<mp4><flv><avi><mkv><wma><rmvb><rm><f4v><m4v>";
                    }
                }
                else
                {
                    s[1] = s[1].Replace("<mp4><flv><avi><mkv><wma><rmvb><rm><f4v><m4v>", ""); 
                    
                }
                s[2] = frm.textBox_path.Text;
                s[3] = frm.textBox_DirName.Text;
                s[4] = frm.checkBox_resetUrl.Checked.ToString();
                s[5] = frm.checkBox_upload.Checked.ToString();
                s[6] = frm.textBox_ftpServer.Text;
                s[7] = frm.textBox_ftpUser.Text;
                s[8] = frm.textBox_ftpPassword.Text;
                s[9] = frm.textBox_ftpPath.Text;
                s[10] = frm.checkBox_delete.Checked.ToString();
                return s;
            }
            set
            {
                if (value == null) { value = new string[] { "[模型值2]", "<jpg><bmp><gif><png>", "C:\\V3Data", "images\\[年月日]", "True", "False", "", "", "", "","True" }; }
                if (value.Length == 11)
                {
                    frm.textBox_Model.Text = value[0];
                    frm.checkBox_type_all.Checked = value[1].Contains("<all>");
                    frm.checkBox_type_image.Checked = value[1].Contains("<jpg><bmp><gif><png>");
                    frm.checkBox_type_video.Checked = value[1].Contains("<mp4><flv><avi><mkv><wma><rmvb><rm><f4v><m4v>");
                    frm.textBox_path.Text = value[2];
                    frm.textBox_DirName.Text = value[3];
                    frm.checkBox_resetUrl.Checked = Convert.ToBoolean(value[4]);
                    frm.checkBox_upload.Checked = Convert.ToBoolean(value[5]);
                    frm.textBox_ftpServer.Text = value[6];
                    frm.textBox_ftpUser.Text = value[7];
                    frm.textBox_ftpPassword.Text = value[8];
                    frm.textBox_ftpPath.Text = value[9];
                    frm.checkBox_delete.Checked= Convert.ToBoolean(value[10]);
                }

            }
        }

        public string GetFiles(string html, string article_url)
        {
            List<Model.download> downloadlist = new List<Model.download>();
            Dictionary<string, Model.Url> fileurls = new Dictionary<string, Model.Url>();
            Regex ruri = new Regex(@"(http|ftp|https)://\w.+");
            Regex rurl = new Regex(@"(?:href\s*=|src\s*=)(?:[\s""']*)(?!#|mailto|location.|javascript)(?<PARAM1>.*?)(?:[\s>""'])");
            MatchCollection con = ruri.Matches(html);
            for (int i = 0; i < con.Count; i++)
            {
                string cvalue = new Regex("<.+?>").Replace(con[i].Value, "");

                if (cvalue.Contains("'")) { cvalue = cvalue.Remove(cvalue.IndexOf("'")); }
                if (cvalue.Contains("\"")) { cvalue = cvalue.Remove(cvalue.IndexOf("\"")); }
                if (cvalue.Contains("<")) { cvalue = cvalue.Remove(cvalue.IndexOf("<")); }
                if (cvalue.Contains(">")) { cvalue = cvalue.Remove(cvalue.IndexOf(">")); }
                Model.Url url = new Model.Url();
                if (cvalue.Trim().Length > 0)
                {
                    url.full_url = cvalue.Trim();
                    url.old_url = cvalue.Trim();
                }
                if (!fileurls.ContainsKey(cvalue)) { fileurls.Add(url.full_url, url); }

            }
            con = rurl.Matches(html);
            for (int i = 0; i < con.Count; i++)
            {
                string cvalue = new Regex("<.+?>").Replace(con[i].Value, "");
                if (cvalue.Contains("'")) { cvalue = cvalue.Substring(cvalue.IndexOf("'") + 1); }
                if (cvalue.Contains("\"")) { cvalue = cvalue.Substring(cvalue.IndexOf("\"") + 1); }
                if (cvalue.Contains("'")) { cvalue = cvalue.Remove(cvalue.IndexOf("'")); }
                if (cvalue.Contains("\"")) { cvalue = cvalue.Remove(cvalue.IndexOf("\"")); }
                if (cvalue.Contains("<")) { cvalue = cvalue.Remove(cvalue.IndexOf("<")); }
                if (cvalue.Contains(">")) { cvalue = cvalue.Remove(cvalue.IndexOf(">")); }
                Model.Url url = new Model.Url();

                url.old_url = cvalue.Trim();
                cvalue = UrlHelper.GetFullUrl(article_url, cvalue, true);
                url.full_url = cvalue.Trim();
                if (!fileurls.ContainsKey(cvalue)) { fileurls.Add(url.full_url, url); }

            }
            if (Parameters[1].Contains("<all>"))
            {
                foreach (KeyValuePair<string, Model.Url> u in fileurls)
                {
                    try
                    {
                        string ext = Path.GetExtension(u.Value.full_url);
                        string filename = Path.GetFileNameWithoutExtension(u.Value.full_url);

                        Model.download d = new Model.download();
                        d.url = u.Value;
                        d.ext = ext;
                        d.filename = filename;
                        downloadlist.Add(d);
                    }
                    catch (Exception error)
                    {

                        Console.WriteLine("[文件下载插件]文件路径提取错误：" + error.Message + "");
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, Model.Url> u in fileurls)
                {
                    try
                    {

                        string ext = Path.GetExtension(u.Value.full_url).Replace(".", "");
                        if (ext.Length > 10) { ext = ""; }
                        string filename = Path.GetFileNameWithoutExtension(u.Value.full_url);
                        if (ext.Length == 0)
                        {
                            //没有后缀
                            if (Parameters[1].Contains("<noext>"))
                            {
                                Model.download d = new Model.download();
                                d.url = u.Value;
                                d.ext = "";
                                d.filename = filename;
                                downloadlist.Add(d);
                            }
                        }
                        else if (Parameters[1].Contains("<" + ext + ">"))
                        {
                            Model.download d = new Model.download();
                            d.url = u.Value;
                            d.ext = ext;
                            d.filename = filename;
                            downloadlist.Add(d);
                        }
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine("[文件下载插件]文件路径提取错误：" + error.Message + "");
                    }
                }

            }
            html = DownloadFile(html, downloadlist);
            return html;
        }

        public string DownloadFile(string html, List<Model.download> downloadlist){
            foreach (Model.download d in downloadlist)
            {
                try
                {
                    string DirName =Parameters[3].Replace("[年月日]",DateTime.Now.ToString("yyyyMMdd"));
                    if (!DirName.EndsWith("\\"))
                    {
                        DirName = DirName + "\\";
                    }
                    Console.WriteLine("[文件下载插件]正在下载文件：" + d.url.old_url);

                    DateTime dt = DateTime.Now;
                    string dir = Parameters[2];
                    string filename = d.filename;

                    if (filename.Length == 0) { filename = dt.ToFileTime().ToString(); }
                    System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(d.url.full_url);
                    Myrq.Referer = d.url.full_url;
                    System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                    long totalBytes = myrp.ContentLength;

                    System.IO.Stream st = myrp.GetResponseStream();
                    System.IO.Stream so = null;
                    long totalDownloadedByte = 0;
                    byte[] by = new byte[100000];
                    int osize = st.Read(by, 0, (int)by.Length);
                    int i = 0;
                    string filedir = dir;
                    if (!filedir.EndsWith("\\")) { filedir = filedir + "\\"; }
                    while (osize > 0)
                    {
                        if (i == 0 && d.ext.Length == 0)
                        {
                            byte[] b = new byte[] { by[0], by[1] };
                            string ext = FileHelper.GetExtName(b);
                            if (d.ext.Length == 0)
                            {
                                d.ext = ext;
                            }

                            if (!Directory.Exists(filedir + DirName)) { Directory.CreateDirectory(filedir + DirName); }

                            so = new System.IO.FileStream(filedir + DirName + filename + "." + ext, System.IO.FileMode.Create);
                            if (Convert.ToBoolean(Parameters[4]))
                            {
                                html = html.Replace(d.url.old_url, (DirName+(filename + "." + ext)).Replace("\\", "/").Replace("//","/"));
                            }
                        }
                        else if (i == 0)
                        {
                            if (!Directory.Exists(filedir + DirName)) { Directory.CreateDirectory(filedir + DirName); }

                            so = new System.IO.FileStream(filedir + DirName+filename + "." + d.ext, System.IO.FileMode.Create);
                            if (Convert.ToBoolean(Parameters[4]))
                            {
                                html = html.Replace(d.url.old_url, ("/"+DirName+(filename + "." + d.ext)).Replace("\\", "/").Replace("//", "/"));
                            }
                        }
                        i++;
                        totalDownloadedByte = osize + totalDownloadedByte;
                        System.Windows.Forms.Application.DoEvents();
                        so.Write(by, 0, osize);

                        osize = st.Read(by, 0, (int)by.Length);
                    }
                    so.Close();
                    st.Close();
                    Console.WriteLine("[文件下载插件]成功下载文件:" + d.url.old_url);
                    if (Convert.ToBoolean(Parameters[5]))
                    {
                       
                        try
                        {
                            FTPHelper ftp = new FTPHelper(Parameters[6], Parameters[9], Parameters[7], Parameters[8]);
                            string ftpdir = DirName.Replace("\\", "/");
                            Console.WriteLine("[文件下载插件]正在上传文件:" + filename + "." + d.ext + "到" + Parameters[9] + ftpdir);
                            bool success=  ftp.FtpUpload((Parameters[9]+ftpdir).Replace("//","/"), (filedir + DirName + filename + "." + d.ext).Replace("//","/"));
                            if (success)
                            {
                                if (Convert.ToBoolean(Parameters[10])) 
                                {
                                    string localpath = (filedir + DirName + filename + "." + d.ext).Replace("//", "/");
                                    Console.WriteLine("[文件下载插件]删除本地缓存文件："+localpath);
                                    File.Delete(localpath);
                                }
                                
                                Console.WriteLine("[文件下载插件]上传成功：" + filedir + DirName + filename + "." + d.ext);
                            }
                           
                        }
                        catch (Exception error)
                        {
                            Console.WriteLine("[文件下载插件]文件上传失败：" + error.Message);
                        }
                    }
                }
                catch (Exception error)
                {
                    Console.WriteLine("[文件下载插件]出现错误" + error.Message);

                }
            }
            return html;
        }

        public object Clone()
        {
            return new App();
        }
    }

}
