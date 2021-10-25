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
using System.Threading;
using xEngine.Model.Execute.Http;

namespace V3.V3Form.抓取模块
{
    public partial class stp1_spidergetmodel : DevExpress.XtraEditors.XtraForm
    {
        public bool Issave = false;
        public bool IsShow = false;
        public Model.GetPostModel Model = new Model.GetPostModel();
        public stp1_spidergetmodel()
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
           Model.Stp1_spider_getencoding = 3;
            List<string> li = new List<string>();
            for (int i = 0; i < txturl.Lines.Length;i++ )
            {
                if (!li.Contains(txturl.Lines[i]) && txturl.Lines[i].Trim().Length>0)
            {
                li.Add(txturl.Lines[i]);
            }
            
            }
            Model.Stp1_spider_mainurl = li;
            Model.Stp1_spider_refererurl = txtStp1RefrereUrl.Text;
            Model.PlanModel = 3;
            Model.IsuseTaskRukou = isuserukou.IsOn;
        
        }
        void GetInfo()
        {
            txturl.Text = "";
            foreach (string s in Model.Stp1_spider_mainurl)
            {
                txturl.Text+=(s + "\r\n");
            }
            txtStp1RefrereUrl.Text = Model.Stp1_spider_refererurl;
            isuserukou.IsOn = Model.IsuseTaskRukou;

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
                    if (IsShow)
                    {
                        this.Invoke((EventHandler)(delegate
                        {
                            htmlEditor1.BodyHtml = result;
                        }));
                    }

                    if (result.Trim().ToLower().Contains("<title>"))
                    {
                        if (IsShow)
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
                        if (IsShow)
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


        private void btngo_Click(object sender, EventArgs e)
        {
            Check();
        }

        private void Stp1_keywordmodel_Load(object sender, EventArgs e)
        {
           
            IsShow = true;
            GetInfo();
        }

        private void pagrRules_Click(object sender, EventArgs e)
        {
          
            SetInfo();
            if (Model.Stp1_spider_mainurl.Count>0)
            {
                Model.Stp1_spider_rules.TestUrl = Model.Stp1_spider_mainurl[0];
            }
            V3Form.MainRules frm = new MainRules();
            frm.cookietxt = Model.GetMajia;
            frm.ruleslv1 = Model.Stp1_spider_rules;
            frm.needmore = true;
            frm.txtRead.Caption = "请生成一个返回内容页目标链接的“返回多结果”规则！";
            frm.txtRead.Appearance.ForeColor = Color.Red;
            frm.ShowDialog();
            if (frm.isSave)
            {
                Model.Stp1_spider_rules = frm.ruleslv1;
                GetInfo();
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

        private void stp1_spidergetmodel_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsShow = false;
        }

       

 
    }
}
