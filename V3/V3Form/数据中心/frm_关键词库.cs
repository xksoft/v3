using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.IO;
using System.Linq;
using DevExpress.XtraEditors;

namespace V3.V3Form
{
    public partial class frm_关键词库 : DevExpress.XtraEditors.XtraForm
    {
        public frm_关键词库()
        {
            InitializeComponent();
        }

        public bool Stop = false;
        public bool IsSave = false;
        public string Dbid = "";
        string[] AllKeywords;
        string[] have;
        public List<string> WordList = new List<string>();
        public Thread tdaochu;
        public void ShowI(string txt)
        {
            this.Invoke((EventHandler)(delegate
            {
                ipanel.Visible = true;
                istate.Text = txt;
            }));

        }
        public void CloseI()
        {
            this.Invoke((EventHandler)(delegate
            {
                ipanel.Visible = false;
            }));
        }
        private void clear()
        {
            Stop = false;
            ShowI("正在进行去重操作，如果关键词较多可能需要一定时间...");
            Dictionary<string, int> keydic = new Dictionary<string, int>();//关键字库
            keydic.Clear();
            string[] strs = keyword.Lines;
            for (int i = 0; i < strs.Length; i++)
            {
                if (Stop)
                {
                    break;
                }
                if (!keydic.ContainsKey(strs[i]) && strs[i].Length > 0)
                    keydic.Add(strs[i], 0);

            }
            string aaa = "";
            WordList.Clear();
            foreach (KeyValuePair<string, int> item in keydic)
            {
                if (Stop)
                {
                    break;
                }
                if (item.Key.Trim() != "")
                {
                    aaa += item.Key.Trim() + "\r\n";
                    WordList.Add(item.Key.Trim());
                }
            }
      
            try
            {
           
                Status.Caption = "总共有" + keyword.Lines.Length + "个关键词";
                CloseI();
                if (IsSave)
                {
                    this.Close();
                }
                else
                keyword.Text = aaa;
            }
            catch { }
            
        }
        public string [] keywords;
        public string filename = "";
        public void daochu() 
        {
            File.WriteAllLines(filename, keywords,Encoding.Default);
            CloseI();
            this.Invoke((EventHandler) (delegate
            {
                XtraMessageBox.Show("成功导出" + keywords.Length + "个关键词！");
            }));

        }
        public void daoru()
        { 
           keywords= File.ReadAllLines(filename,Encoding.Default);
           this.Invoke((EventHandler)(delegate
           {
               istate.Text = "一共有" + keywords.Length.ToString() + "个关键字需要导入，如果关键词较多可能需要一定时间...";
           }));
           StringBuilder sb = new StringBuilder();
           for (int i = 0; i < keywords.Length;i++ )
           {
               if (Stop)
               {
                   break;
               }
               sb.Append(keywords[i]);
               sb.Append("\r\n");
             
               
           }
           keyword.Text = sb.ToString();
           CloseI();

        }
        public void baohan()
        {
            string r = "";
            keyword.Text = "";
            for (int ii = 0; ii < have.Length; ii++)
            {
                if (Stop)
                {
                    break;
                }
                for (int i = 0; i < AllKeywords.Length; i++)
                {
                    if (Stop)
                    {
                        break;
                    }
                    if (AllKeywords[i].Contains(have[ii]) && !WordList.Contains(AllKeywords[i]))
                    {
                        WordList.Add(AllKeywords[i]);
                    }

                }
            }
            foreach (string s in WordList)
            {
                if (Stop)
                {
                    break;
                }
                r += (s + "\r\n");
            }
            keyword.Text = r;
            CloseI();
        }
        public void bubaohan()
        {
            string r = "";
            keyword.Text = ""; for (int i = 0; i < AllKeywords.Length; i++)
            {
                for (int ii = 0; ii < have.Length; ii++)
                {
                    if (Stop)
                    {
                        break;
                    }
                    if (AllKeywords[i].Contains(have[ii]))
                    {
                        break;
                    }
                    WordList.Add(AllKeywords[i]);
                }
            }
            foreach (string s in WordList)
            {
                if (Stop)
                {
                    break;
                }
                r += (s + "\r\n");
            }
            keyword.Text = r;
            CloseI();
        }
        private void frm_关键词库_Shown(object sender, EventArgs e)
        {
           
            
        }

        private void keyword_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            keyword.SelectAll();
            keyword.Copy();
            MessageBox.Show("已经全部复制到剪贴板了哦！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void frm_关键词库_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
               CloseI();
                 Stop = true;
                
            }
        }

        private void keyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CloseI();
                Stop = true;

            }
        }

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IsSave = true;
            Thread s = new Thread(new ThreadStart(clear));
            s.IsBackground = true;
            s.Start();
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IsSave = false;
            this.Close();
        }

        private void btn_import_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog sa = new OpenFileDialog();
            sa.Filter = "本文件(*.txt)|*.txt";

            if (sa.ShowDialog() == DialogResult.OK)
            {
                Stop = false;
                filename = sa.FileName;
                tdaochu = new Thread(daoru);
                tdaochu.IsBackground = true;
                tdaochu.Start();
                ShowI("准备导入...");

            }
        }

        private void btn_export_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog sa = new SaveFileDialog();
            sa.DefaultExt = "txt";
            sa.FileName = DateTime.Now.ToString("yyMMddHHmmss关键词");
            if (sa.ShowDialog() == DialogResult.OK)
            {
               
                Stop = false;
                filename = sa.FileName;
                keywords = keyword.Lines;
                ShowI("有" + keywords.Length.ToString() + "个关键词正在导出中...");
                tdaochu = new Thread(daochu);
                tdaochu.IsBackground = true;
                tdaochu.Start();
               
            }
        }

        private void btn_unique_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IsSave = false;
            Thread s = new Thread(new ThreadStart(clear));
            s.IsBackground = true;
            s.Start();
        }

        private void btn_tb_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Stop = false;
            WordList.Clear();
            frm_参数输入编辑器 c = new frm_参数输入编辑器();
            c.Text = "关键词提纯（包含）";
            c.txttitle.Caption = "关键词提纯（包含）";
            c.txtStatus.Caption = "请输入必须包含的关键词，一行一个...";
            c.ShowDialog();

            AllKeywords = keyword.Lines;
            have = c.txtMainbox.Lines;
            if (have.Length == 0) { return; }
            tdaochu = new Thread(baohan);
            tdaochu.IsBackground = true;
            tdaochu.Start();
            ShowI("提纯中...");
        }

        private void btn_tbb_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            Stop = false;
            WordList.Clear();
            frm_参数输入编辑器 c = new frm_参数输入编辑器();
            c.Text = "关键词提纯（不包含）";
            c.txttitle.Caption = "关键词提纯（不包含）";
            c.txtStatus.Caption = "请输不包含的关键词，一行一个...";
            c.ShowDialog();


            AllKeywords = keyword.Lines;
            have = c.txtMainbox.Lines;
            if (have.Length == 0) { return; }
            tdaochu = new Thread(bubaohan);
            tdaochu.IsBackground = true;
            tdaochu.Start();
            ShowI("正在提纯中...");
        }

        private void frm_关键词库_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            Thread thread = new Thread(delegate()
            {
                ShowI("关键词加载中，如果数量较多可能需要一定时间...");
                StringBuilder sb = new StringBuilder();
                this.Text = Model.V3Infos.KeywordDb[Dbid].Name + " - 关键词管理";

                for (int i = 0; i < Model.V3Infos.KeywordDb[Dbid].Keywords.Count; i++)
                {
                    if (Stop)
                    {
                        break;
                    }
                    sb.AppendLine(Model.V3Infos.KeywordDb[Dbid].Keywords[i].ToString());

                }
                this.Invoke((EventHandler)(delegate
                {
                    keyword.Text = sb.ToString();
                    Status.Caption = "总共有" + Model.V3Infos.KeywordDb[Dbid].Keywords.Count + "个关键词！";
                }));
                CloseI();
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void keyword_EditValueChanged(object sender, EventArgs e)
        {
            Status.Caption = "总共有" + keyword.Lines.Length + "个关键词！";
        }
    }
}
