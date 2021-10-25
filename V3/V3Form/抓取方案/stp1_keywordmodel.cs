using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using DevExpress.Data.Helpers;
using xEngine.Model.Execute.Http;

namespace V3.V3Form.抓取模块
{
    public partial class Stp1_keywordmodel : DevExpress.XtraEditors.XtraForm
    {
        public bool isShow = false;
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
            if (isShow)
            {
                this.Invoke((EventHandler)(delegate
                {
                    ipanel.Visible = false;
                }));
            }
            
        }
        public Stp1_keywordmodel()
        {
            InitializeComponent();
        }
        public Model.GetPostModel Model = new Model.GetPostModel();
        public bool Issave = false;
        void SetInfo()
        {
            
            Model.Stp1_GET_Keyword_GetEncoding = 3;
            Model.Stp1_GET_Keyword_TestKeyword = txttestkeyword.Text;
            Model.Stp1_GET_Keyword_TestUrl = txturl.Text;
            Model.Stp1_GET_refrereurl = txtStp1RefrereUrl.Text;
            Model.Stp1_NeedGetPage = needpage.IsOn;
            Model.Stp1_PageNumber=Convert.ToInt32(pagenumber.Value);
            Model.Stp2_GET_Keyword_TestUrl = Geturl();
        }
        void GetInfo()
        {
           
            txturl.Text = Model.Stp1_GET_Keyword_TestUrl;
            txttestkeyword.Text = Model.Stp1_GET_Keyword_TestKeyword;
            txtStp1RefrereUrl.Text = Model.Stp1_GET_refrereurl;
            needpage.IsOn = Model.Stp1_NeedGetPage;
            pagenumber.Value = Model.Stp1_PageNumber;
        }

        void Check()
        {
            ShowI("正在下载数据...");
            Thread t = new Thread(delegate()
            {
                string url = Geturl();
                xEngine.Execute.Http execute = new xEngine.Execute.Http();
                execute.CookieAddStr(Model.GetMajia);
                execute.LoadScript(Properties.Resources.get, false);
                execute.Scripts[0].Url = url;
                execute.Scripts[0].Referer = "";
                execute.IsAutoEncoding = true;
                XResponse response = execute.RunRequest(execute.Scripts[0]);
                string result = "源地址：" + execute.Scripts[0].Url + "\r\n\r\n" + Library.HtmlHelper.GetHtmlFromByte(response.BodyData, response.ContentType);
                if (isShow)
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        htmlEditor1.BodyHtml = result;
                    }));
                }
                CloseI();
            });
            t.IsBackground = true;
            t.Start();
        }
        string Geturl()
        {
            return txturl.Text.Replace("[关键词]", txttestkeyword.Text).Replace("[关键词UTF8编码]", System.Web.HttpUtility.UrlEncode(txttestkeyword.Text, Encoding.UTF8)).Replace("[关键词GBK编码]", System.Web.HttpUtility.UrlEncode(txttestkeyword.Text, Encoding.Default));
        }
        

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txturl.SelectedText = "[关键词]";
            txturl.Focus();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txturl.SelectedText = "[关键词UTF8编码]";
            txturl.Focus();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txturl.SelectedText = "[关键词GBK编码]";
            txturl.Focus();
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

        private void pagrRules_Click(object sender, EventArgs e)
        {
            
            Model.Stp1_PageRules.TestUrl = Geturl();
            SetInfo();
            V3Form.MainRules frm = new MainRules();
            frm.cookietxt = Model.GetMajia;
            frm.ruleslv1 = Model.Stp1_PageRules;
            
            frm.needmore = true;
            frm.txtRead.Caption = "请生成一个返回分页地址的“返回多结果”规则！";
            frm.ShowDialog();
            if (frm.isSave)
            {
                Model.Stp1_PageRules = frm.ruleslv1;
                GetInfo();
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

        private void Stp1_keywordmodel_FormClosed(object sender, FormClosedEventArgs e)
        {
            isShow = false;
        }

 
    }
}
