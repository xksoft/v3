using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace V3.V3Form.抓取模块
{
    public partial class Stp2_keywordmodel : DevExpress.XtraEditors.XtraForm
    {
        public bool IsShow = false;
        public Model.GetPostModel Model = new Model.GetPostModel();
        public bool IsSave = false;
        public System.Threading.Thread s;
        public Stp2_keywordmodel()
        {
            InitializeComponent();
            Stp2_keywordmodel.CheckForIllegalCrossThreadCalls = false;
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
                this.Invoke((EventHandler)(delegate
                {
                    ipanel.Visible = false;
                }));
            }
           
        }

        void SetInfo()
        {
            Model.Stp2_GET_Keyword_TestUrl = txturl.Text;
            Model.Stp2_GET_NoNeedStp2 = Noneddstp2.IsOn;
        }
        void GetInfo()
        {
            txturl.Text = Model.Stp2_GET_Keyword_TestUrl;
            btnrules.Text = Model.Stp2_GET_Keyword_Rules.Rules.RulesName;
            Noneddstp2.IsOn = Model.Stp2_GET_NoNeedStp2;
        }

        private string Gethtml()
        {
            this.Invoke((EventHandler)(delegate
            {
                statusbar.Caption = "正在加载" + txturl.Text + "的源代码...";
            }));

            string url = txturl.Text;
            xEngine.Execute.Http execute = new xEngine.Execute.Http();
            execute.LoadScript(Properties.Resources.get, false);
            execute.CookieAddStr(Model.GetMajia);
            execute.Scripts[0].Url = url;
            execute.Scripts[0].UserAgent = Model.UserAgent == "" ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)" : Model.UserAgent;
        
            execute.Scripts[0].Referer = "";
            execute.IsAutoEncoding = true;
            string result = execute.RunRequest(execute.Scripts[0]).BodyString;
            return result;
        }
        private void StratTest()
        {
            this.Invoke((EventHandler)(delegate
            {
                btntest.Enabled = false;
                btntest.Text = "正在测试";
            }));
            string result = Gethtml();

            if (IsShow)
            {
                this.Invoke((EventHandler)(delegate
                {
                    htmlEditor1.BodyHtml = result;
                    statusbar.Caption = "网页代码加载完毕！";
                }));
            }

            V3.Bll.GetBll get1 = new Bll.GetBll(result, Model.Stp2_GET_Keyword_Rules.Rules);
            get1.oldurl = txturl.Text;
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            list = get1.getAllRules(0);
            string jieguo = "";
            for (int i = 0; i < list.Count; i++)
            {
                jieguo += "第" + (i + 1) + "个结果:\r\n" + list[i].ToString() + "\r\n\r\n";
            }

            if (IsShow)
            {
                this.Invoke((EventHandler)(delegate
                {
                    groupPanel2.Text = "结果(共获取到" + list.Count + "个结果)";
                    htmlEditor2.BodyHtml = jieguo;
                    statusbar.Caption = "测试完毕！";
                    btntest.Enabled = true;
                    btntest.Text = "开始测试";
                }));
            }

            CloseI();

        }


        private void Stp2_keywordmodel_Load(object sender, EventArgs e)
        {
            IsShow = true;
            GetInfo();
        }

        private void btnrules_Click(object sender, EventArgs e)
        {
            Model.Stp2_GET_Keyword_Rules.Rules.TestUrl = Model.Stp2_GET_Keyword_TestUrl;
            V3Form.MainRules frm = new MainRules();
            frm.cookietxt = Model.GetMajia;
            frm.ruleslv1 = Model.Stp2_GET_Keyword_Rules.Rules;
            frm.needmore = true;
            frm.txtRead.Caption = "请生成一个返回内容页目标链接的“返回多结果”规则！";
            frm.txtRead.Appearance.ForeColor = Color.Red;
            frm.ShowDialog();
            if (frm.isSave)
            {
                if (frm.resultList.Count>0)
                {
                    Model.Stp3_GET_TestUrl = frm.resultList[0].ToString();
                }
                Model.Stp2_GET_Keyword_Rules.Rules = frm.ruleslv1;
                GetInfo();
            }
      
        }

        private void btntest_Click(object sender, EventArgs e)
        {
           s = new System.Threading.Thread(StratTest);
            s.IsBackground = true;
            s.Start();
            ShowI("正在下载网页数据...");
           
        }
        private void Stp2_keywordmodel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                

                    btntest.Enabled = true;
                CloseI();
                if (s != null)
                {
                    try
                    {
                        s.Abort();
                    }
                    catch { }
                }

            }
        }

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IsSave = true;
            SetInfo();
           
            this.Close();
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IsSave = false;
            this.Close();
        }

        private void Stp2_keywordmodel_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsShow = false;
        }
    }
}
