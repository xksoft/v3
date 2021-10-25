using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Linq;
using System.Threading;
using xEngine.Model.Execute.Http;

namespace V3.V3Form.抓取模块
{
    public partial class Stp1_tongbumodel : DevExpress.XtraEditors.XtraForm
    {
        public bool isShow=false;
        public Model.GetPostModel Model = new Model.GetPostModel();
        public bool Issave = false;
        public Stp1_tongbumodel()
        {
            InitializeComponent();
        }

        public void ShowI(string txt)
        {
            this.Invoke((EventHandler)(delegate
            {
                ipanel.Visible = true;
                istate.Text = txt;
            }));

        }
        void Check()
        {
            ShowI("正在下载数据...");
            Thread t = new Thread(delegate()
            {
                for (int i = 0; i < txturl.Lines.Length; i++)
                {
                    string url = txturl.Lines[i].ToString();
                    xEngine.Execute.Http execute = new xEngine.Execute.Http();
                    execute.CookieAddStr(Model.GetMajia);
                    execute.LoadScript(Properties.Resources.get, false);
                    execute.Scripts[0].Url = url;
                    execute.Scripts[0].Referer = "";
                    execute.Scripts[0].UserAgent = Model.UserAgent == "" ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)" : Model.UserAgent;
        
                    execute.IsAutoEncoding = true;
                    XResponse response = execute.RunRequest(execute.Scripts[0]);
                    string result = Library.HtmlHelper.GetHtmlFromByte(response.BodyData, response.ContentType);
                    if (isShow)
                    {
                        this.Invoke((EventHandler)(delegate
                        {
                            htmlEditor1.BodyHtml = result;
                        }));

                    }

                    if (result.Trim().ToLower().Contains("<title>"))
                    {
                        if (isShow)
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                htmlEditor1.BodyHtml = "访问正常！\r\n\r\n" + htmlEditor1.BodyHtml;
                            }));
                        }
                        break;
                    }
                    else
                    {
                        if (isShow)
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                htmlEditor1.BodyHtml = "所有地址都无法访问正常，请检查入口地址！\r\n\r\n" + htmlEditor1.BodyHtml;
                            }));
                        }
                    }
                }
                CloseI();
            });
            t.IsBackground = true;
            t.Start();
        }
        public void CloseI()
        {
            if (isShow)
            {
                this.Invoke((EventHandler) (delegate
                {
                    ipanel.Visible = false;
                }));
            }
        }
        void SetInfo()
        {
           
                Model.Tongbu_stp1_GetEncoding = 3;
                List<string> li = new List<string>();
            for (int i = 0; i < txturl.Lines.Length;i++ )
            {
                if (!li.Contains(txturl.Lines[i]) && txturl.Lines[i].Length>0)
            {
                li.Add(txturl.Lines[i]);
            }
            
            }
            Model.Tongbu_stp1_RuKouUrls = li;
            Model.Tongbu_stp1_refrereurl = txtStp1RefrereUrl.Text;
            Model.PlanModel = 4;
            Model.IsuseTaskRukou = isuserukou.IsOn;

        }
        void GetInfo()
        {
            txturl.Text = "";
           
            foreach(string s in Model.Tongbu_stp1_RuKouUrls)
            {
               Common.V3Helper.GetInnerTextBox( txturl).AppendText( s+"\n");
            }
            txtStp1RefrereUrl.Text = Model.Tongbu_stp1_refrereurl;
            isuserukou.IsOn = Model.IsuseTaskRukou;
        }


        private void btngo_Click(object sender, EventArgs e)
        {
            Check();
        }

        private void buttonItem1_Click(object sender, EventArgs e)
        {
          
        }

        private void buttonItem3_Click(object sender, EventArgs e)
        {
           
        }

        private void Stp1_keywordmodel_Load(object sender, EventArgs e)
        {
            isShow = true;
            GetInfo();
        }

        private void pagrRules_Click_1(object sender, EventArgs e)
        {
            SetInfo();
            if (Model.Tongbu_stp1_RuKouUrls.Count>0)
            {
                Model.Tongbu_Stp2_Rules.Rules.TestUrl = Model.Tongbu_stp1_RuKouUrls[0];
            }
            V3Form.MainRules frm = new MainRules();
            frm.cookietxt = Model.GetMajia;
            frm.ruleslv1 = Model.Tongbu_Stp2_Rules.Rules;
            frm.needmore = true;
            frm.txtRead.Caption = "请生成一个返回内容页目标链接的“返回多结果”规则！";
            frm.txtRead.Appearance.ForeColor = Color.Red;
            frm.ShowDialog();
            if (frm.isSave)
            {
                Model.Tongbu_Stp2_Rules.Rules = frm.ruleslv1;
                if (frm.resultList.Count>0)
                {
                    Model.Stp3_GET_TestUrl = frm.resultList[0].ToString();
                }
            }
           
        }

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Issave = true;
            SetInfo();
            this.Close();
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Issave = false;
            this.Close();
        }

        private void Stp1_tongbumodel_FormClosed(object sender, FormClosedEventArgs e)
        {
            isShow = false;
        }

       

 
    }
}
