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
namespace V3.V3Form
{
    public partial class frm_YuChuli : DevExpress.XtraEditors.XtraForm
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
                this.Invoke((EventHandler) (delegate
                {
                    ipanel.Visible = false;
                }));
            }

        }
        public frm_YuChuli()
        {
            InitializeComponent();
            frm_ChildEngRules.CheckForIllegalCrossThreadCalls = false;
        }
        public Model.抓取相关模型.RulesEngLv1 ruleslv1 = new Model.抓取相关模型.RulesEngLv1();
        public bool issave = false;
        public string cookietxt = "";


        private void btnreloadcode_Click(object sender, EventArgs e)
        {
         s = new Thread(loaddocThread);
            s.IsBackground = true;
            s.Start();
            showI("正在下载网页数据");
           
        }
     
        private void loaddocThread()
        {
            this.Invoke((EventHandler) (delegate
            {
                btnreloadcode.Enabled = false;
                btntest.Enabled = false;
                statusbar.Caption = "正在下载网页代码...";
            }));
            string result = "";
            if (checkBoxX1.Checked)
                result = gethtml();
            else
                result = htmlEditor1.BodyHtml;

            if (isShow)
            {
                this.Invoke((EventHandler) (delegate
                {
                    statusbar.Caption = "加载完成！";
                    htmlEditor1.BodyHtml = result;
                    btntest.Enabled = true;
                    btnreloadcode.Enabled = true;
                }));
            }

            closeI();
           
        }

        private string gethtml()
        {
            
            string url = txturl.Text;
            xEngine.Execute.Http execute = new xEngine.Execute.Http();
            execute.LoadScript(Properties.Resources.get,false);
            execute.CookieAddStr(cookietxt);
            execute.Scripts[0].Url = url;
            execute.Scripts[0].Referer = "";
            execute.IsAutoEncoding = true;
            string result = execute.RunRequest(execute.Scripts[0]).BodyString;
            return result;
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

        /// <summary>
        /// 保存信息到类
        /// </summary>
        private void SetInfo()
        {
            try
            {
                
                    ruleslv1.Bianma = 2;
                ruleslv1.TestUrl = txturl.Text;
                ruleslv1.IsuseYuchuli = switchButton1.IsOn;
                ruleslv1.Ischulijuedui = isformat.IsOn;
                ruleslv1.Istichun = switchButton3.IsOn;
                ruleslv1.IsuseEngreplace = switchButton2.IsOn;
                string[] tempstr = txtReplacedata.Lines;
                System.Collections.ArrayList list = new System.Collections.ArrayList();
                for (int i = 0; i < tempstr.Length; i++)
                {
                    if (tempstr[i].Trim().Split('→').Length == 2)
                    {
                        list.Add(tempstr[i].Trim());
                    }
                }
                ruleslv1.ReplaceData.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    string[] temp = list[i].ToString().Split('→');
                    if (!ruleslv1.ReplaceData.ContainsKey(temp[0]))
                    {
                        ruleslv1.ReplaceData.Add(temp[0], temp[1]);
                    }
                    else
                    {
                        ruleslv1.ReplaceData[temp[0]] = temp[1];
                    }
                }
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
               
                txturl.Text = ruleslv1.TestUrl;
                switchButton1.IsOn = ruleslv1.IsuseYuchuli;
                isformat.IsOn = ruleslv1.Ischulijuedui;
                switchButton3.IsOn = ruleslv1.Istichun;
                switchButton2.IsOn = ruleslv1.IsuseEngreplace;
                txtReplacedata.Text = "";
                foreach (KeyValuePair<string, string> value in ruleslv1.ReplaceData)
                {
                    txtReplacedata.Text += value.Key + "→" + value.Value + "\r\n";
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString());
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
            isShow = true;
            GetInfo();
        }
        Thread s;
        private void btntest_Click(object sender, EventArgs e)
        {
            SetInfo();
            s= new Thread(testgo);
            s.IsBackground = true;
            s.Start();
            showI("正在提取");
          
        }
        void testgo()
        {
            loaddocThread();
            Thread.Sleep(50);
            V3.Bll.GetBll bll = new Bll.GetBll(htmlEditor1.BodyHtml, ruleslv1);
            if (isShow)
            {
                this.Invoke((EventHandler) (delegate
                {
                    bll.oldurl = txturl.Text;
                    outhtml.BodyHtml = bll.yuchuli();
                }));
            }
            closeI();
           
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtReplacedata.SelectedText = "→";
            txtReplacedata.Focus();
        }



        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (switchButton2.IsOn)
            {
                frm_ChildEngRules frm = new frm_ChildEngRules();
                frm.cookietxt = cookietxt;
                frm.ruleslv2 = ruleslv1.Tichunrule;
                frm.ruleslv1 = ruleslv1;
                frm.ShowDialog();
                if (frm.issave)
                    ruleslv1.Tichunrule = frm.ruleslv2;
            }
            else
            {
                frm_ChildRgxRules frm = new frm_ChildRgxRules();
                frm.cookietxt = cookietxt;
                frm.ruleslv2 = ruleslv1.Tichunrule;
                frm.ruleslv1 = ruleslv1;
                frm.ShowDialog();
                if (frm.issave)
                    ruleslv1.Tichunrule = frm.ruleslv2;
            }
        }

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetInfo();
            issave = true;
            this.Close();
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            issave = false;
            this.Close();
        }

        private void frm_YuChuli_FormClosed(object sender, FormClosedEventArgs e)
        {
            isShow = false;
        }
    }
}
