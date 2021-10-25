using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using DevExpress.Design.TypePickEditor;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Native;
using DevExpress.XtraTreeList.Nodes;
using xEngine.Common;
using xEngine.Model.Execute.Http;
using xEngine.Plugin.HtmlParsing;
using HtmlDocument = xEngine.Plugin.HtmlParsing.HtmlDocument;

namespace V3.V3Form
{
    public partial class frm_ChildEngRules : DevExpress.XtraEditors.XtraForm
    {
        public bool isShow = false;
        public Thread s;
        public HtmlDocument htmlDoc = new HtmlDocument();
        public Model.抓取相关模型.RulesEngLv2 ruleslv2 = new Model.抓取相关模型.RulesEngLv2();
        public Model.抓取相关模型.RulesEngLv1 ruleslv1 = new Model.抓取相关模型.RulesEngLv1();
        public bool issave = false;
        public string cookietxt = "";
        public int count = 0;
        public frm_ChildEngRules()
        {
            InitializeComponent();
            frm_ChildEngRules.CheckForIllegalCrossThreadCalls = false;
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
            if (isShow)
            {
                this.Invoke((EventHandler) (delegate
                {
                    ipanel.Visible = false;
                }));
            }
        }

        private void loaddocThread()
        {
            this.Invoke((EventHandler)(delegate
            {
                btnreloadcode.Enabled = false;
                btntest.Enabled = false;
                statusbar.Caption = "正在加载网页代码到引擎中...";
            }));
            string result = Gethtml();

            LoadDoc(result, "");

            if (isShow)
            {
                this.Invoke((EventHandler)(delegate
                {
                    statusbar.Caption = "加载完成！";
                    htmlEditor1.BodyHtml = result;
                    btntest.Enabled = true;
                    btnreloadcode.Enabled = true;
                }));
            }


            CloseI();
        }

        private string Gethtml()
        {

            string url = txturl.Text;
            xEngine.Execute.Http execute = new xEngine.Execute.Http();
            execute.CookieAddStr(cookietxt);
            execute.LoadScript(Properties.Resources.get, false);
            execute.Scripts[0].Url = url;
            execute.Scripts[0].Referer = "";
            execute.IsAutoEncoding = true;
            XResponse response = execute.RunRequest(execute.Scripts[0]);
            string result = "源地址：" + execute.Scripts[0].Url + "\r\n\r\n" + Library.HtmlHelper.GetHtmlFromByte(response.BodyData, response.ContentType);
            return result;
        }
        private void SelectGo()
        {
            //loaddocThread();
            SelectNode(xEngine.Common.XSerializable.BytesToObject<string>(Base32.FromBase32String(txtKey.Text)), true);
            CloseI();
        }
       
        public void  GetAllNodes(HtmlDocument htmldoc, string keyword)
        {
            
            count = 0;
            try
            {
                LoadNode(htmldoc.DocumentNode, keyword, treeList_Main.Nodes[0]);
            }
            catch(Exception error)
            {
                V3.Common.Log.LogNewline("加载页面代码是出错："+error.Message);
            
            }
        }
        public void LoadNode(HtmlNode htmlnode, string keyword,TreeListNode parentNode)
        {

            TreeListNode node = treeList_Main.Nodes[0];
            string classname = "";
           
            if (htmlnode.Name == "#document")
            {

                node = treeList_Main.Nodes[0];
                
            }
            else
            {
                this.Invoke((EventHandler) (delegate
                {
                    node =treeList_Main.AppendNode(
                            new object[] {htmlnode.Name + classname + "(长度：" + htmlnode.InnerHtml.Length + ")"},
                            parentNode,
                            htmlnode.XPath);
                }));
            }

            if (htmlnode.ChildNodes.Count > 0)
            {
                for (int i = 0; i < htmlnode.ChildNodes.Count; i++)
                {
                    if (htmlnode.ChildNodes[i].Name.Contains("#comment") || htmlnode.ChildNodes[i].Name.Contains("#text") || !htmlnode.ChildNodes[i].OuterHtml.Contains(keyword))
                        continue;
                    try
                    {
                        LoadNode(htmlnode.ChildNodes[i], keyword, node);
                    }
                    catch (Exception error)
                    {
                        V3.Common.Log.LogNewline("加载页面代码是出错：" + error.Message);

                    }
                    count++;
                }
            }
            if (htmlnode.Attributes.Count > 0)
            {
                bool needadd = false;
                TreeListNode nodeattr=null;
                this.Invoke((EventHandler) (delegate
                {
                      nodeattr =
                        treeList_Main.AppendNode(
                            new object[] {htmlnode.Name + "(属性个数：" + htmlnode.Attributes.Count + ")"}, node,
                            htmlnode.XPath);
                }));

                for (int i = 0; i < htmlnode.Attributes.Count; i++)
                {
                
                    if ((htmlnode.Attributes[i].Name + htmlnode.Attributes[i].Value + htmlnode.Attributes[i].ToString()).Contains(keyword))
                        needadd = true;
                    if ((htmlnode.Attributes[i].Name.ToLower() == "class" ||
                        htmlnode.Attributes[i].Name.ToLower() == "name" || htmlnode.Attributes[i].Name.ToLower() == "id" || htmlnode.Attributes[i].Name.ToLower() == "value") && needadd)
                    {
                        classname += "|" + htmlnode.Attributes[i].Value;
                        this.Invoke((EventHandler) (delegate
                        {
                           node.SetValue(0,htmlnode.Name + classname + "(长度：" + htmlnode.InnerHtml.Length + ")");
                            treeList_Main.AppendNode(
                                new object[]
                                {"名称:" + htmlnode.Attributes[i].Name + "  值:" + htmlnode.Attributes[i].Value},
                                nodeattr, htmlnode.Attributes[i].XPath);
                            count++;
                        }));
                    }
                }
            }
           
        }
        private void LoadDoc(string str, string starchkeyword)
        {
            this.Invoke((EventHandler) (delegate
            {
                barStaticItem1.Caption = "正在将代码加载到结构树中...";
                treeList_Main.Nodes[0].Nodes.Clear();
            }));
            htmlDoc.LoadHtml(str);
           GetAllNodes(htmlDoc, starchkeyword);
            this.Invoke((EventHandler) (delegate
            {
                label_status.Text = "网页结构树（总共获取到" + count + "个内容块)";
                barStaticItem1.Caption = "加载完毕!";
            }));
        }
        private void SetInfo(int g)
        {
            try
            {

                ruleslv1.Bianma = 2;
                ruleslv2.Rulesstr = xEngine.Common.XSerializable.BytesToObject<string>(Base32.FromBase32String(txtKey.Text));
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
        private void GetInfo()
        {

            if (ruleslv2.Rulesstr != null)
                txtKey.Text = Base32.ToBase32String(xEngine.Common.XSerializable.ObjectToBytes(ruleslv2.Rulesstr));
            txturl.Text = ruleslv1.TestUrl;
            txtreadme.Text = ruleslv2.Readme;

        }

        private void SelectNode(string nodepath, bool showmsg)
        {
            HtmlNode firstNavItemNode = htmlDoc.DocumentNode.SelectSingleNode(nodepath);
            if (firstNavItemNode != null)
            {
                if (nodepath.Contains("@"))
                {
                    string abname = nodepath.Split('@')[1].Replace("[1]", "");
                    try
                    {
                        this.Invoke((EventHandler)(delegate
                        {
                            inhtml.BodyHtml = firstNavItemNode.InnerHtml;
                            outhtml.BodyHtml = firstNavItemNode.OuterHtml;
                            txtvaluename.Text = firstNavItemNode.Attributes[abname].Name;
                            txtvalue.Text = firstNavItemNode.Attributes[abname].Value;
                        }));
                    }
                    catch { }
                    if (showmsg)
                        this.Invoke((EventHandler)(delegate
                        {
                            XtraMessageBox.Show("成功根据“属性”KEY提取到结果！", "提示", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }));
                }
                else
                {
                    try
                    {
                        this.Invoke((EventHandler)(delegate
                        {
                            inhtml.BodyHtml = firstNavItemNode.InnerHtml;
                            outhtml.BodyHtml = firstNavItemNode.OuterHtml;
                            txtvaluename.Text = string.Empty;
                            txtvalue.Text = string.Empty;
                        }));
                    }
                    catch { }
                    if (showmsg)
                    {
                        this.Invoke((EventHandler)(delegate
                        {
                            XtraMessageBox.Show("成功根据“内容”KEY提取到结果！", "提示", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }));
                    }
                }
            }
            else
            {
                this.Invoke((EventHandler)(delegate
                {
                    if (showmsg)
                        XtraMessageBox.Show("无法根据特征KEY提取到内容，请重新设置一个筛选特征字符！", "提示", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                }));
            }
        }


     
        private void btnreloadcode_Click(object sender, EventArgs e)
        {
          s = new Thread(loaddocThread);
            s.IsBackground = true;
            s.Start();
            ShowI("正在下载网页数据");
        }
        private void btnsearch_Click(object sender, EventArgs e)
        {
            LoadDoc(htmlEditor1.BodyHtml, txtsearch.Text);
        }
        private void txtKey_DoubleClick(object sender, EventArgs e)
        {
            txtKey.SelectAll();
            txtKey.Copy();
            XtraMessageBox.Show( "当前内容已经保存到剪贴板啦", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btntest_Click(object sender, EventArgs e)
        {
            s = new Thread(SelectGo);
            s.IsBackground = true;
            s.Start();
            ShowI("正在提取");
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
                btntest_Click(sender, e);
            }
        }

        private void txtsearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnsearch_Click(sender, e);
            }
        }


        void ClickSelectLink(object sender, EventArgs e)
        {
           DevExpress.XtraEditors.LabelControl temp =sender as DevExpress.XtraEditors.LabelControl;
           if (temp != null)
           {
               if (temp.Name == "get1_1")
               {
                   Model.抓取相关模型.RulesEngLv2 rule = new Model.抓取相关模型.RulesEngLv2();
                   rule.GetModel = 1;
               }
           }

        }

        private void buttonItem5_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem3_Click(object sender, EventArgs e)
        {
          
        }

        private void buttonItem1_Click(object sender, EventArgs e)
        {
          
        }

        private void btnget1_Click(object sender, EventArgs e)
        {
            SetInfo(1);
            issave = true;
            this.Close();
        }

        private void btnget2_Click(object sender, EventArgs e)
        {
            SetInfo(2);
            issave = true;
            this.Close();
        }

        private void btnget3_Click(object sender, EventArgs e)
        {
            SetInfo(3);
            issave = true;
            this.Close();
        }

        private void btnget4_Click(object sender, EventArgs e)
        {
            SetInfo(4);
            issave = true;
            this.Close();
        }

        private void frm_ChildEngRules_Load(object sender, EventArgs e)
        {
            isShow = true;
            GetInfo();
        }

        private void frm_ChildEngRules_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) 
            {
                if (s !=null)
                {   btntest.Enabled = true;
                    btnreloadcode.Enabled = true;
                    CloseI();
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
            SetInfo(0);
            issave = true;
            this.Close();
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            issave = false;
            this.Close();
        }

        private void treeList_Main_SelectionChanged(object sender, EventArgs e)
        {
           
        }

        private void treeList_Main_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {

                if (treeList_Main.Selection[0].Tag != null)
                {
                    txtKey.Text =Base32.ToBase32String(xEngine.Common.XSerializable.ObjectToBytes(treeList_Main.Selection[0].Tag.ToString()));
                    SelectNode(treeList_Main.Selection[0].Tag.ToString(), false);
                }
                else
                {

                }
            }
            catch { };

        }

        private void treeList_Main_KeyPress(object sender, KeyPressEventArgs e)
        {
          
        }

        private void treeList_Main_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void treeList_Main_KeyDown(object sender, KeyEventArgs e)
        {
          
            if (e.KeyCode == Keys.Right&&treeList_Main.Selection.Count>0)
            {
                if (treeList_Main.Selection[0].Expanded == true && treeList_Main.Selection[0].Nodes.Count>0)
                {
                    treeList_Main.Selection[0].Nodes[0].Selected = true;
                }
                else
                {
                    treeList_Main.Selection[0].Expanded = true;
                }
               
                }
        }

        private void frm_ChildEngRules_FormClosed(object sender, FormClosedEventArgs e)
        {
            isShow = false;
        }
    }
}
