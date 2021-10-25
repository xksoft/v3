using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using DevExpress.XtraEditors;
using xEngine.Model.Execute.Http;

namespace V3.V3Form
{
    public partial class frm_ChildRgxRules : DevExpress.XtraEditors.XtraForm
    {
        public bool isShow = false;
        public void showI(string txt)
        {
            this.Invoke((EventHandler)(delegate
            {
                ipanel.Visible = true;
                istate.Text = txt;
            }));

        }
        public void closeI()
        {
            if (isShow)
            {
                this.Invoke((EventHandler)(delegate
                {
                    ipanel.Visible = false;
                }));
            }
          
        }
        public frm_ChildRgxRules()
        {
            InitializeComponent();
            frm_ChildEngRules.CheckForIllegalCrossThreadCalls = false;
        }
        public Model.抓取相关模型.RulesEngLv2 ruleslv2 = new Model.抓取相关模型.RulesEngLv2();
        public Model.抓取相关模型.RulesEngLv1 ruleslv1 = new Model.抓取相关模型.RulesEngLv1();
        public bool issave = false;
        public string cookietxt = "";


     
     
        private void loaddocThread()
        {
            this.Invoke((EventHandler) (delegate
            {
                btnreloadcode.Enabled = false;
                btntest.Enabled = false;
                statusbar.Caption = "正在下载网页代码...";
            }));
            string  result = gethtml();
            
            try
            {
            this.Invoke((EventHandler)(delegate
            {statusbar.Caption = "加载完成！";
                htmlEditor1.BodyHtml = result;
                btntest.Enabled = true;
            btnreloadcode.Enabled = true;
            }));
            }
            catch { }
            
            closeI();
           
        }

        private string gethtml()
        {
            statusbar.Caption = "正在加载" + txturl.Text + "的源代码...";
            string url = txturl.Text;
            xEngine.Execute.Http execute = new xEngine.Execute.Http();
            execute.LoadScript(Properties.Resources.get,false);
            execute.CookieAddStr(cookietxt);
            execute.Scripts[0].Url = url;
            execute.Scripts[0].Referer = "";
            execute.IsAutoEncoding = true;
            XResponse response = execute.RunRequest(execute.Scripts[0]);
            string result = "源地址：" + execute.Scripts[0].Url + "\r\n\r\n" + Library.HtmlHelper.GetHtmlFromByte(response.BodyData, response.ContentType);
            statusbar.Caption = "加载完毕!";
            return result;}

        /// <summary>
        /// 保存信息到类
        /// </summary>
        private void SetInfo(int g)
        {
            try
            {

                ruleslv1.Bianma = 2;
                ruleslv2.Rulesstr = txtKey.Text;
                ruleslv2.Readme = txtreadme.Text;
                ruleslv1.TestUrl = txturl.Text;
                if (g != 0)
                    ruleslv2.GetModel = g;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString());
            }

        }
        /// <summary>
        /// 从类读取信息
        /// </summary>
        private void GetInfo()
        {
            try
            {

                if (ruleslv2.Rulesstr != null)
                    txtKey.Text = ruleslv2.Rulesstr;
                txturl.Text = ruleslv1.TestUrl;
                txtreadme.Text = ruleslv2.Readme;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString());
            }
        }
        void testgo()
        {
            //loaddocThread();
            Thread.Sleep(50);
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            list = Library.RegexHelper.GetArrayList(htmlEditor1.BodyHtml.Replace("\n", "\r\n"), txtKey.Text);
            if (list.Count == 0)
            {
                if (isShow)
                {
                    this.Invoke((EventHandler) (delegate
                    {
                        outhtml.BodyHtml = "没有提取到结果哦。。。";
                        statusbar.Caption = "没有提取到结果哦。。。";
                    }));
                }

            }
            else
            {
                string tempstr = "";
                for (int i = 0; i < list.Count; i++)
                {
                    tempstr += "第" + (i + 1) + "个结果：\r\n" + list[i].ToString() + "\r\n\r\n";
                }
                if (isShow)
                {
                    this.Invoke((EventHandler) (delegate
                    {
                        outhtml.BodyHtml = tempstr;
                        statusbar.Caption = "共提取到" + list.Count + "个结果！";
                    }));
                }
            }
            closeI();

        }

        private void btnreloadcode_Click(object sender, EventArgs e)
        {
            s = new Thread(loaddocThread);
            s.IsBackground = true;
            s.Start();
            showI("正在下载网页数据");

        }
        private void btnsearch_Click(object sender, EventArgs e)
        {

        }
        private void txtKey_DoubleClick(object sender, EventArgs e)
        {
            txtKey.SelectAll();
            txtKey.Copy();
            XtraMessageBox.Show("当前内容已经保存到剪贴板啦", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void txturl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnreloadcode_Click(sender, e);
            }
        }

        private void txtKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

            }
        }

        private void txtsearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnsearch_Click(sender, e);
            }
        }

        private void buttonItem3_Click(object sender, EventArgs e)
        {
          
        }

        private void buttonItem1_Click(object sender, EventArgs e)
        {
          
        }


        private void frm_ChildEngRules_Load(object sender, EventArgs e)
        {
            GetInfo();
            isShow = true;
        }
        Thread s;
        private void btntest_Click(object sender, EventArgs e)
        {
           s= new Thread(testgo);
            s.IsBackground = true;
            s.Start();
            showI("正在提取");
          
        }
 

        private void frm_ChildRgxRules_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (s != null)
                {
                    btntest.Enabled = true;
                    btnreloadcode.Enabled = true;
                    closeI();
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
            SetInfo(-1);
            issave = true;
            this.Close();
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            issave = false;
            this.Close();
        }

        private void frm_ChildRgxRules_FormClosed(object sender, FormClosedEventArgs e)
        {
            isShow = false;
        }
    }
}
