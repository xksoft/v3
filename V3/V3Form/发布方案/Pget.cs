using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Threading;
using System.Windows.Forms;
using V3.Bll;

namespace V3.V3Form.发布模块
{
    public partial class Pget : DevExpress.XtraEditors.XtraForm
    {
        public static Pget MyPget;
        public bool IsShow = false;
        public Model.发布相关模型.GetPostAction Action = new Model.发布相关模型.GetPostAction();
        public Model.GetPostModel Model = new Model.GetPostModel();
        public V3.Bll.PostBll Bll;
        public bool IsSave = false;
        public string OldHtml = string.Empty;
        public Thread t;
        public Pget()
        {
            InitializeComponent();
            Bll = new Bll.PostBll(Model, 0, 0);
            Pget.CheckForIllegalCrossThreadCalls = false;
            MyPget = this;
        }
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
            if (IsShow)
            {
                this.Invoke((EventHandler) (delegate
                {
                    ipanel.Visible = false;
                }));
            }

        }
        void SetInfo()
        {
            Action.ActionUrl = txturl.Text;
            Action.RefrereUrl = txtrefrere.Text;
            Action.UserAgent = txtUserAgent.Text;
            Action.IsPost = false;
            Action.IsGetRedirect = chongdingxiang.IsOn;
            if (bianma.IsOn)
                Action.IsUtf8 = true;
            else
                Action.IsUtf8 = false;
        }
        void GetInfo()
        {
            if (Action == null)
                Action = new Model.发布相关模型.GetPostAction();
            txturl.Text = Action.ActionUrl;
            txtrefrere.Text = Action.RefrereUrl;
            txtUserAgent.Text = Action.UserAgent;
            chongdingxiang.IsOn=Action.IsGetRedirect;
            if (Action.IsUtf8)
                bianma.IsOn = true;
            else
                bianma.IsOn = false;
        }
        private void Start()
        {
            ShowI("正在下载数据...");

            this.Invoke((EventHandler)(delegate
            {
                btnstart.Enabled = false;
                btnstart.Text = "正在测试";
            }));
            SetInfo();
            Bll = new PostBll(Model, 0, 0);
            string result = Bll.RunAction(Action, true, "");
            if (IsShow)
            {
                this.Invoke((EventHandler)(delegate
                {
                    htmlEditor2.BodyHtml = result;
                    btnstart.Enabled = true;
                    btnstart.Text = "开始测试";
                }));
            }
            CloseI();



        }
        private void getPlan_Load(object sender, EventArgs e)
        {
            IsShow = true;
            GetInfo();
        } 
       
        private void btnstart_Click(object sender, EventArgs e)
        {
           t = new System.Threading.Thread(Start);
            t.IsBackground = true;
            t.Start();
            
           
        }
        private void txturl_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (Program.f_frmReplaceTag == null || Program.f_frmReplaceTag.IsDisposed )
                {
                    Program.f_frmReplaceTag = new frmReplaceTag();
                }
                Program.f_frmReplaceTag.referer = "Pget_txturl";
                Program.f_frmReplaceTag.Focus();
                Program.f_frmReplaceTag.Show();
                Program.f_frmReplaceTag.Location = new Point(0, 0);
            }
        }

        private void txtrefrere_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (Program.f_frmReplaceTag == null || Program.f_frmReplaceTag.IsDisposed )
                {
                    Program.f_frmReplaceTag = new frmReplaceTag();
                }
                Program.f_frmReplaceTag.referer = "Pget_txtrefrere";
                Program.f_frmReplaceTag.Focus();
                Program.f_frmReplaceTag.Show();
                Program.f_frmReplaceTag.Location = new Point(0, 0);
            }

        }

        private void Pget_KeyDown(object sender, KeyEventArgs e)//退出
        {
            if (e.KeyCode == Keys.Escape)
            {

                btnstart.Enabled = true;
                btnstart.Text = "开始测试";
                CloseI();
                if (t != null)
                {
                    try
                    {
                        t.Abort();
                    }
                    catch { }
                }

            }
        }

        private void btn_ok_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetInfo();
            IsSave = true;
            this.Close();
        }

        private void btn_c_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IsSave = false;
            this.Close();
        }

        private void Pget_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed )
            {
                Program.f_frmReplaceTag.Close();
            }
        }

        private void txturl_MouseUp(object sender, MouseEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed )
            {
                Program.f_frmReplaceTag.referer = "Pget_txturl";
            }
        }
        private void txtrefrere_MouseUp(object sender, MouseEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed)
            {
                Program.f_frmReplaceTag.referer = "Pget_txtrefrere";
            }
        }
        private void Pget_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsShow = false;
        }

    }
}
