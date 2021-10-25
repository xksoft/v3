using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using DevExpress.XtraEditors;
using Model;
using V3.Bll;
using V3.Common;

namespace V3.V3Form
{
    public partial class frm_文章导入导出工具 : DevExpress.XtraEditors.XtraForm
    {
        public static frm_文章导入导出工具 Myfrm_文章导出导入工具;
        public frm_文章导入导出工具()
        {
            InitializeComponent();
            Myfrm_文章导出导入工具 = this;
        }
        public string dbName = "";
        public bool Stop = true;
        public bool isimport = true;
        public string dbid = "";
        public ArticleDB db = null;
        Thread t;
        public bool needRefresh = false;
        private void frm_文章导出导入工具_Shown(object sender, EventArgs e)
        {
          
            text_to.Value = Model.V3Infos.ArticleDb[dbid].DataCount;
            toggleSwitch.IsOn = isimport;
            LoadConfig();
            //ribemport.Checked = !isimport;
        }

        public void LoadConfig()
        {
            db = Model.V3Infos.ArticleDb[dbid];
            if (toggleSwitch.IsOn)
            {
                //导入
                toggleSwitch_encoding.IsOn = db.InPutEncodingGBK;
                text_path.Text = db.InPutPath;
                text_filename.Text = db.InPutFileName;
                text_formate.Text = db.InPutFileContent;
                toggleSwitch_SetArticle.IsOn = db.SetArticle;
                if (Model.V3Infos.MainDb.DefaultTasks.ContainsKey(db.DefaultTask))
                {
                    linkLabel_DefaultTask.Text = "使用任务模板[" + db.DefaultTask + "]处理";
                }
                else 
                {
                    linkLabel_DefaultTask.Text = "点击选择要使用的处理任务模板";
                }
            }
            else
            {
                toggleSwitch_encoding.IsOn = db.OutPutEncodingGBK;
                text_path.Text = db.OutPutPath;
                text_filename.Text = db.OutPutFileName;
                text_formate.Text = db.OutPutFileContent;
                toggleSwitch_SetArticle.IsOn = db.SetArticle;
                if (Model.V3Infos.MainDb.DefaultTasks.ContainsKey(db.DefaultTask))
                {
                    linkLabel_DefaultTask.Text = "使用任务模板 [" + db.DefaultTask + "] 的处理配置";
                }
                else
                {
                    linkLabel_DefaultTask.Text = "点击选择要使用的处理任务模板";
                }
            }

        }

        public void SaveConfig()
        {
            if (toggleSwitch.IsOn)
            {
                //导入
                Model.V3Infos.ArticleDb[dbid].InPutEncodingGBK = toggleSwitch_encoding.IsOn;
                 Model.V3Infos.ArticleDb[dbid].InPutPath=text_path.Text ;
                  Model.V3Infos.ArticleDb[dbid].InPutFileName=text_filename.Text;
                  Model.V3Infos.ArticleDb[dbid].InPutFileContent=text_formate.Text;
                  Model.V3Infos.ArticleDb[dbid].SetArticle = toggleSwitch_SetArticle.IsOn;
            }
            else
            {
                 Model.V3Infos.ArticleDb[dbid].OutPutEncodingGBK=toggleSwitch_encoding.IsOn;
                Model.V3Infos.ArticleDb[dbid].OutPutPath=text_path.Text ;
                Model.V3Infos.ArticleDb[dbid].OutPutFileName =text_filename.Text;
                Model.V3Infos.ArticleDb[dbid].OutPutFileContent=text_formate.Text;
                Model.V3Infos.ArticleDb[dbid].SetArticle = toggleSwitch_SetArticle.IsOn;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

           if(t!=null)
           {
               t.Abort();
           }
        }
        DialogResult dr ;
        FolderBrowserDialog folderDlg = new FolderBrowserDialog();
        private void btnStart_Click(object sender, EventArgs e)
        {

            dr = folderDlg.ShowDialog();
            t = new Thread(start);
            t.IsBackground = true;
            t.Start();
        }

        public void start()
        {
            int success = 0;
            int error = 0;
            Thread thread = new Thread(delegate()
            {

                if (toggleSwitch.IsOn)
                {
                    string path = text_path.Text;
                    string filename = text_filename.Text;
                    string filecontent = text_formate.Text;
                    Encoding encoding = Encoding.UTF8;
                    if (toggleSwitch_encoding.IsOn)
                    {
                        encoding = Encoding.GetEncoding("gbk");
                    }
                    this.Invoke((EventHandler) (delegate
                    {
                        btn_start.Text = "停止导入";
                        progressBar.Visible = label_status.Visible = true;
                    }));
                    string[] files = Directory.GetFiles(path);
                    this.Invoke((EventHandler) (delegate
                    {
                        progressBar.Properties.Maximum = files.Length;
                        label_status.Text = "准备导入...";
                    }));
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (Stop) { break; }
                        this.Invoke((EventHandler) (delegate
                        {
                            progressBar.Text = i.ToString();
                        }));
                        Model.Model_Article article = GetArticle(files[i], File.ReadAllText(files[i], encoding),
                            text_filename.Text, text_formate.Text);
                        if (article != null)
                        {
                            article.Date = DateTime.Now.ToString("yyyy-M-d HH:mm:ss");
                            article.Db = dbid;

                            if (toggleSwitch_SetArticle.IsOn && Model.V3Infos.MainDb.DefaultTasks.ContainsKey(db.DefaultTask)) 
                            {
                                Common.SetArticle s = new Common.SetArticle();
                                s.Task = xEngine.Common.XSerializable.CloneObject<Model.Task>(Model.V3Infos.MainDb.DefaultTasks[db.DefaultTask]);
                                s.art = article;
                                s.chuli();
                                s.minganguolv();
                            }

                            this.Invoke((EventHandler) (delegate
                            {
                                label_status.Text = "正在导入[" + article.DataObject[0] + "]...";
                            }));
                            if (ArticleBll.addnewdata(Model.V3Infos.ArticleDb[dbid], article))
                            {
                                success++;
                                
                            }
                            else
                            {
                                error++;
                            }
                        }
                        else
                        {
                            error++;
                        }

                    }
                    this.Invoke((EventHandler) (delegate
                    {
                        Stop = true;
                        progressBar.Text = "0";
                        progressBar.Visible = label_status.Visible = false;
                        btn_start.Text = "开始导入";
                        XtraMessageBox.Show("成功导入[" + success + "]篇文章到文章库“" + dbid + "”，失败[" + error + "]篇！", "导入完毕",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (success>0) { needRefresh = true; }
                       
                    }));

                }
                else
                {
                    if (!text_path.Text.EndsWith("\\"))
                    {
                        this.Invoke((EventHandler) (delegate
                        {
                            text_path.Text = text_path.Text + "\\";
                        }));
                    }
                    int fromID = Convert.ToInt32(text_from.Value);
                    int toID = Convert.ToInt32(text_to.Value);
                    Encoding encoding = Encoding.UTF8;
                    if (toggleSwitch_encoding.IsOn)
                    {
                        encoding = Encoding.GetEncoding("gbk");
                    }
                    this.Invoke((EventHandler) (delegate
                    {
                        btn_start.Text = "停止导出";
                        progressBar.Visible = label_status.Visible = true;
                    }));
                    if (fromID < toID)
                    {

                        this.Invoke((EventHandler) (delegate
                        {
                            progressBar.Properties.Maximum = toID - fromID;
                            label_status.Text = "准备导出...";
                        }));
                        for (int i = fromID; i < toID + 1; i++)
                        {
                            if (Stop) { break; }
                            Model.Model_Article article = new Model_Article();
                            article.Id = i.ToString();
                            bool bget = ArticleBll.LoadData(ref article, Model.V3Infos.ArticleDb[dbid]);
                            if (article != null & bget)
                            {
                                if (toggleSwitch_SetArticle.IsOn && Model.V3Infos.MainDb.DefaultTasks.ContainsKey(db.DefaultTask))
                                {
                                    Common.SetArticle s = new Common.SetArticle();
                                    s.Task = xEngine.Common.XSerializable.CloneObject<Model.Task>(Model.V3Infos.MainDb.DefaultTasks[db.DefaultTask]);
                                    s.art = article;
                                    s.chuli();
                                    s.minganguolv();
                                }
                               
                                PostBll bll = new PostBll(new GetPostModel(), 0, 0);
                                bll.pointid = -1;
                                bll.article = xEngine.Common.XSerializable.CloneObject<Model.Model_Article>(article);
                                string path = text_path.Text;
                                string filename = text_filename.Text;
                                string filecontent = text_formate.Text;
                                path = bll.ReplaceTag(path, false, "", true);
                                filename = bll.ReplaceTag(filename, false, "", true);
                                filename = Library.StrHelper.SetFileName(filename);
                                this.Invoke((EventHandler)(delegate
                                {
                                    progressBar.Text = i.ToString();
                                    label_status.Text = "正在导出[" + filename + "]...";
                                }));
                                filecontent = bll.ReplaceTag(filecontent, false, "", true);
                                try
                                {
                                    if (!Directory.Exists(path))
                                    {
                                        Directory.CreateDirectory(path);
                                    }

                                    if (filename.Length > 200)
                                    {
                                        filename = filename.Substring(filename.Length - 200, 200);
                                    }
                                    File.WriteAllText(path + filename, filecontent, encoding);
                                    success++;
                                }
                                catch (Exception err)
                                {
                                    error++;
                                }
                            }
                            else
                            {
                                error++;
                            }
                        }
                    }
                    else
                    {

                    }
                    this.Invoke((EventHandler) (delegate
                    {
                        Stop = true;
                        
                        progressBar.Text = "0";
                        progressBar.Visible = label_status.Visible = false;
                        btn_start.Text = "开始导出";
                        XtraMessageBox.Show("成功导出[" + success + "]篇文章到“" + text_path.Text + "”，失败[" + error + "]篇！",
                            "导出完毕", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      

                    }));
                }
            });
            thread.IsBackground = true;
            thread.Start();

        }

        public Model.Model_Article GetArticle(string name, string content, string nameformate, string contentformate)
        {
            Model.Model_Article article = new Model_Article();
            article.DataObject = new Dictionary<int, string>()
            {
            { 0,""},
            { 1,""},
            { 2,""},
            { 3,""},
            { 4,""},
            { 5,""},
            { 6,""},
            { 7,""},
            { 8,""},
            { 9,""},
            { 10,""},
            { 11,""},
            { 12,""},
            { 13,""},
            { 14,""},
            { 15,""},
            { 16,""},
            { 17,""},
            { 18,""},
            { 19,""},
            { 20,""},
            { 21,""},
            { 22,""},
            { 23,""},
            { 24,""},
            { 25,""},
            { 26,""},
            { 27,""},
            { 28,""},
            { 29,""}
            };
            if (nameformate.Length>0)
            {
                FileInfo file=new FileInfo(name);
                int v =Convert.ToInt32(Library.RegexHelper.GetList(nameformate, "【模型值(?<v>\\d+)】")[0])-1;
                article.DataObject[v] = new Regex("\\.(\\w*)$").Replace(file.Name, "");
            }
            content = content.Replace("\r","");
            content = content.Replace("\n", "㊣");
            List<string> vs = Library.RegexHelper.GetList(contentformate, "【模型值(?<v>\\d+)】");
            int vc = 0;
            foreach (var s in vs)
            {
                if (vc == vs.Count-1)
                {
                    contentformate = contentformate.Replace("【模型值" + s + "】", "(?<model" + s + ">.*)");
                }
                else
                {
                    contentformate = contentformate.Replace("【模型值" + s + "】", "(?<model" + s + ">.+?)");
                }
                vc++;
            }
            contentformate = contentformate.Replace("\r","");
            contentformate = contentformate.Replace("\n", "㊣");
            Match mc = new Regex(contentformate,RegexOptions.IgnoreCase).Match(content);
            if (mc.Success )
            {
                Match m = mc.NextMatch();
               for (int i=0;i<30;i++)
                {

                    if (mc.Groups["model" + (i + 1)].Success)
                    {
                        article.DataObject[i] = mc.Groups["model" + (i + 1)].Value.Replace("㊣", "\r\n");
                    }
                }
                return article;
            }
            else
            {
                return null;
            }

        }

        private void text_path_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (Program.f_frmReplaceTag == null || Program.f_frmReplaceTag.IsDisposed )
                {
                    Program.f_frmReplaceTag = new frmReplaceTag();
                }
                Program.f_frmReplaceTag.referer = "frm_文章导出导入工具_text_path";
                Program.f_frmReplaceTag.Focus();
                Program.f_frmReplaceTag.Show();
                Program.f_frmReplaceTag.Location = new Point(0, 0);
            }
        }

        private void text_formate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (Program.f_frmReplaceTag == null || Program.f_frmReplaceTag.IsDisposed )
                {
                    Program.f_frmReplaceTag = new frmReplaceTag();
                }
                Program.f_frmReplaceTag.referer = "frm_文章导出导入工具_text_formate";
                Program.f_frmReplaceTag.Focus();
                Program.f_frmReplaceTag.Show();
                Program.f_frmReplaceTag.Location = new Point(0, 0);
            }
        }

        private void toggleSwitch_Toggled(object sender, EventArgs e)
        {
            if (toggleSwitch.IsOn)
            {
                label_formate.Text = "内容格式(F1插入标签)：\r\n\r\n只能包含模型值1-30\r\n以及其他分割字符";
                label_path.Text = "                 文件目录：";
                btn_start.Text = "开始导入";
                 text_from.Enabled = text_to.Enabled = false;

               
            }
            else
            {
                label_formate.Text = "内容格式(F1插入标签)：";
                label_path.Text = "保存目录(F1插入标签)：";
                btn_start.Text = "开始导出";
                text_filename.Text = "";
                text_from.Enabled = text_to.Enabled = true;
            }
            LoadConfig();
        }

        private void frm_文章导出导入工具_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed )
            {
                Program.f_frmReplaceTag.Close();
            }
        }

        private void text_filename_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void text_filename_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (Program.f_frmReplaceTag == null || Program.f_frmReplaceTag.IsDisposed )
                {
                    Program.f_frmReplaceTag = new frmReplaceTag();
                }
                Program.f_frmReplaceTag.referer = "frm_文章导出导入工具_text_filename";
                Program.f_frmReplaceTag.Focus();
                Program.f_frmReplaceTag.Show();
                Program.f_frmReplaceTag.Location = new Point(0, 0);
            }
        }

        private void frm_文章导出导入工具_Load(object sender, EventArgs e)
        {

        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (Stop)
            {
                if (toggleSwitch.IsOn)
                {
                    if (text_path.Text.Length == 0)
                    {
                        XtraMessageBox.Show("请选择文章存在的目录！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (!Directory.Exists(text_path.Text))
                    {
                        XtraMessageBox.Show("选择的目录不存在，请重新选择！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (new Regex("【.*】").Matches(text_filename.Text).Count >= 2)
                    {
                        XtraMessageBox.Show("文件名只能导入到一个模型值中！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (text_formate.Text.Length == 0)
                    {
                        XtraMessageBox.Show("请设置要导入的模型值列表格式！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        SaveConfig();
                        start();
                        Stop = false;
                    }
                }
                else
                {
                    if (text_path.Text.Length == 0)
                    {
                        XtraMessageBox.Show("请选择要保存到的目录！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (!text_path.Text.Contains("\\"))
                    {
                        XtraMessageBox.Show("目录必须以\\结尾，请手动补全！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    else if (text_filename.Text.Length == 0)
                    {
                        XtraMessageBox.Show("请设置要导出的文件名格式！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (!text_filename.Text.Contains("."))
                    {
                        XtraMessageBox.Show("文件名格式要写后缀哦，比如【模型值1】.txt", "无法继续", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else if (text_formate.Text.Length == 0)
                    {
                        XtraMessageBox.Show("请设置要导出的文件内容格式！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        SaveConfig();
                        start();
                        Stop = false;
                    }

                }
            }
            else
            {
                Stop = true;
            }
        }

        private void btn_selectpath_Click(object sender, EventArgs e)
        {
         
            FolderBrowserDialog f=new FolderBrowserDialog();
            if(f.ShowDialog()==DialogResult.OK)
            {
                text_path.Text = f.SelectedPath+"\\";
            }
        }

        private void text_path_MouseUp(object sender, MouseEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed )
            {
                Program.f_frmReplaceTag.referer = "frm_文章导出导入工具_text_path";
            }
        }

        private void text_filename_MouseUp(object sender, MouseEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed )
            {
                Program.f_frmReplaceTag.referer = "frm_文章导出导入工具_text_filename";
            }
        }

        private void text_formate_MouseUp(object sender, MouseEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed )
            {
                Program.f_frmReplaceTag.referer = "frm_文章导出导入工具_text_formate";
            }
        }

        private void linkLabel_DefaultTask_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           

            frmDefaultTasks f = new frmDefaultTasks();
            f.ShowDialog();
            if (f.IsOK)
            {
                db.DefaultTask = f.DefaultTaskName;
                SaveConfig();
                LoadConfig();

            }
            else 
            {
                LoadConfig();
            }
        }
    }
}
