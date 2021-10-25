using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Net;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using xEngine.Model.Execute.Http;
using HtmlDocument = xEngine.Plugin.HtmlParsing.HtmlDocument;
namespace V3.V3Form.抓取模块
{
    public partial class Stp3_Allmodel : DevExpress.XtraEditors.XtraForm
    {
        public bool IsShow = false;
        public System.Threading.Thread s;
        public Model.GetPostModel Model = new Model.GetPostModel();
        public bool IsSave = false;
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
        public Stp3_Allmodel()
        {
            InitializeComponent();
            Stp2_keywordmodel.CheckForIllegalCrossThreadCalls = false;
        }
        void SetInfo()
        {
           
            Model.Zidingyi_Sep3_GET_TestUrl = txturl.Text;
            Model.Tongbu_Stp3_RefrereURL = txtStp3RefrereUrl.Text;
            Model.Stp3_GET_TestUrl = txturl.Text;
            if (chk3_1.Checked)
                Model.Stp3_GET_GetModel = 1;
            else 
                Model.Stp3_GET_GetModel = 2;
            Model.Stp3_RefrereURL = txtStp3RefrereUrl.Text;
            Model.Stp3_neemorepage = isNeedMorePage.IsOn;
        }
        void GetInfo()
        {
           
            txturl.Text = Model.Stp3_GET_TestUrl;
            if (Model.Stp3_GET_GetModel == 1)
            {
                chk3_1.Checked = true;
                comboBox_models.Enabled = comboBox_valueindex.Enabled = button_models.Enabled = true;
            }

            else
            {
                chk3_2.Checked = true;
                comboBox_models.Enabled = comboBox_valueindex.Enabled = button_models.Enabled = false;
            }

            txtStp3RefrereUrl.Text = Model.Stp3_RefrereURL;
            isNeedMorePage.IsOn = Model.Stp3_neemorepage;
        }
        private void StratTest()
        {
            this.Invoke((EventHandler)(delegate
            {
                btntest.Enabled = false;
                btntest.Text = "正在测试";
            }));
            string result = GetHtml();

            if (IsShow)
            {
                this.Invoke((EventHandler)(delegate
                {
                    htmlEditor1.BodyHtml = result;
                    statusbar.Caption = "网页代码加载完毕！";
                }));
            }


            Model.Model_Article art = new Model.Model_Article();
            art.DataObject = new Dictionary<int, string>()
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
            if (Model.Stp3_GET_GetModel == 1)//按规则提取
            {
                for (int i = 0; i < 30; i++)
                {
                    if (Model.Stp3_GET_Rules[i] != null)
                    {
                        V3.Bll.GetBll get = new Bll.GetBll(result, Model.Stp3_GET_Rules[i].isGetPublicRules ? Model.Stp3_GET_PublicRules : Model.Stp3_GET_Rules[i].Rules);
                        get.oldurl = txturl.Text;
                        System.Collections.ArrayList list = new System.Collections.ArrayList();
                        list = get.getAllRules(0);



                        try
                        {
                            art.DataObject[i] = list[Model.Stp3_GET_Rules[i].selectedValue].ToString();
                        }
                        catch { }
                    }
                }
                
            }
            else
            {
                Library.HtmlHelper.Article article = Library.HtmlHelper.HtmlToArticle.GetArticle(result);
                if (article != null)
                {
                    art.DataObject[0] = article.Title;
                    art.DataObject[1] = article.ContentWithTags;
                }
                else
                {
                    HtmlDocument htmldoc = new HtmlDocument();
                    htmldoc.LoadHtml(result);
                    string title = V3.Common.AiGet.getTitle(htmldoc.DocumentNode);
                    string content = V3.Common.AiGet.GetMainContent(htmldoc.DocumentNode, 0);
                    art.DataObject[0] = title;
                    art.DataObject[1] = content;
                }


            }
            art.DataObject[29] = "测试时无关键词";
            art.DataObject[28] = "测试时无源地址";
            if (IsShow)
            {

                this.Invoke((EventHandler)(delegate
                {
                    statusbar.Caption = "测试完毕！";
                    btntest.Enabled = true;
                    btntest.Text = "开始测试";
                    frm_文章编辑器 frm = new frm_文章编辑器();
                    frm.Text = "内容测试提取";
                    frm.art = art;

                    frm.ShowDialog();

                }));
            }
            CloseI();

        }
        private void StratTestFenYe()
        {
            int pagecout = 0;
            Dictionary<string, int> chk = new Dictionary<string, int>();
            string s = "";
            if (isNeedMorePage.IsOn == true)
            {
                Queue que = new Queue();

                chk.Add(txturl.Text.Trim(), 0);
                string url = "";
                string html = "";
                string urls = txturl.Text;


                xEngine.Execute.Http execute = new xEngine.Execute.Http();
                execute.LoadScript(Properties.Resources.get, false);
                execute.CookieAddStr(Model.GetMajia);
                execute.Scripts[0].Url = urls;
                execute.Scripts[0].Referer = txtStp3RefrereUrl.Text;
                execute.IsAutoEncoding = true;
                string FinalHtml = execute.RunRequest(execute.Scripts[0]).BodyString;

                V3.Bll.GetBll get = new V3.Bll.GetBll(FinalHtml, Model.Stp3_pagerule);
                if (txturl.Text.Trim().Contains("http://"))
                    get.oldurl = txturl.Text.Trim();
                System.Collections.ArrayList list = new System.Collections.ArrayList();
                list = get.getAllRules(0);
                if (list.Count > 0)
                {
                    for (int iii = 0; iii < list.Count; iii++)
                    {
                        if (!chk.ContainsKey(list[iii].ToString()))
                        {
                            chk.Add(list[iii].ToString(), 0);
                            que.Enqueue(list[iii].ToString());
                        }
                    }
                }
                while (que.Count > 0)
                {
                    url = que.Dequeue().ToString();
                    xEngine.Execute.Http execute1 = new xEngine.Execute.Http();
                    execute1.LoadScript(Properties.Resources.get, false);
                    execute1.CookieAddStr(Model.GetMajia);
                    execute1.Scripts[0].Url = url;
                    execute1.Scripts[0].Referer = txtStp3RefrereUrl.Text;
                    execute1.IsAutoEncoding = true;
                    html = execute.RunRequest(execute1.Scripts[0]).BodyString;
                    get = new V3.Bll.GetBll(html, Model.Stp3_pagerule);
                    if (txturl.Text.ToString().Contains("http://"))
                        get.oldurl = txturl.Text.ToString();
                    list = new System.Collections.ArrayList();
                    list = get.getAllRules(0);
                    if (list.Count > 0)
                    {
                        for (int iii = 0; iii < list.Count; iii++)
                        {
                            if (!chk.ContainsKey(list[iii].ToString()))
                            {
                                chk.Add(list[iii].ToString(), 0);
                                que.Enqueue(list[iii].ToString());
                                pagecout++;
                                if (pagecout > 50)
                                {
                                    this.Invoke((EventHandler) (delegate
                                    {
                                        XtraMessageBox.Show("获取到的内容分页已经超过50页，内容分页地址提取规则有问题，请检查分页规则！", "停止获取",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        buttonX3.Text = "测试分页规则";
                                        buttonX3.Enabled = true;
                                        CloseI();
                                       
                                    }));
                                    break;
                                }
                            }
                        }
                    }
                    if (pagecout > 50)
                    { break; }

                }
            }
            //输出结果
            int c = 0;
            foreach (string o in chk.Keys)
            {
                c++;
                s += "第" + c.ToString() + "页：" + o.ToString() + "\n\n";

            }
            this.Invoke((EventHandler) (delegate
            {
                htmlEditor1.BodyHtml = s;
                buttonX3.Text = "测试分页规则";
                buttonX3.Enabled = true;
                CloseI();
            }));


        }
        private string GetHtml()
        {
            statusbar.Caption = "正在加载" + txturl.Text + "的源代码...";
            xEngine.Execute.Http execute = new xEngine.Execute.Http();
            execute.LoadScript(Properties.Resources.get, false);
            execute.CookieAddStr(Model.GetMajia);
            execute.Scripts[0].Url = txturl.Text;
            execute.Scripts[0].UserAgent = Model.UserAgent == "" ? "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.2)" : Model.UserAgent;
            execute.Scripts[0].Referer = "";
            execute.IsAutoEncoding = true;
            XResponse response = execute.RunRequest(execute.Scripts[0]);
            string result = "源地址：" + execute.Scripts[0].Url + "\r\n\r\n" + Library.HtmlHelper.GetHtmlFromByte(response.BodyData, response.ContentType);
            statusbar.Caption = "加载完毕！";
            return result;
        }


        private void Stp3_Allmodel_Load(object sender, EventArgs e)
        {
            comboBox_models.SelectedIndex = 0;
            IsShow = true;
            GetInfo();
            
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (Model.Stp3_GET_Rules[comboBox_models.SelectedIndex] == null)
            {
                Model.Stp3_GET_Rules[comboBox_models.SelectedIndex] = new Model.抓取相关模型.FinalRules();
            }
            button_models.Text = Model.Stp3_GET_Rules[comboBox_models.SelectedIndex].Rules.RulesName;
            comboBox_valueindex.SelectedIndex = Model.Stp3_GET_Rules[comboBox_models.SelectedIndex].selectedValue;

            if (comboBox_models.SelectedIndex == 28 || comboBox_models.SelectedIndex == 29)
            {
               comboBox_valueindex.Enabled = button_models.Enabled = false;
            }
            else
            {
               comboBox_valueindex.Enabled = button_models.Enabled = true;
            }

        }

        private void chk3_1_CheckedChanged(object sender, EventArgs e)
        {
            if (chk3_1.Checked)
            {
                Model.Stp3_GET_GetModel = 1;
                comboBox_models.Enabled = comboBox_valueindex.Enabled = button_models.Enabled = true;
            }
            else
            {
                Model.Stp3_GET_GetModel = 2;
                comboBox_models.Enabled = comboBox_valueindex.Enabled = button_models.Enabled = false;
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            V3Form.MainRules frm = new MainRules();
            frm.cookietxt = Model.GetMajia;
            frm.ruleslv1 = Model.Stp3_GET_PublicRules;
    
            frm.ShowDialog();
 
            if (frm.isSave)
            {
                Model.Stp3_GET_PublicRules = frm.ruleslv1;
      
                button_models.Text = Model.Stp3_GET_Rules[comboBox_models.SelectedIndex].Rules.RulesName;

                    Model.Stp3_GET_Rules[comboBox_models.SelectedIndex].isGetPublicRules = false;
                    Model.Stp3_GET_Rules[comboBox_models.SelectedIndex].selectedValue = comboBox_valueindex.SelectedIndex;

            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
          
            V3Form.MainRules frm = new MainRules();
            frm.cookietxt = Model.GetMajia;
            frm.ruleslv1 = Model.Stp3_GET_Rules[comboBox_models.SelectedIndex].Rules;
      
            frm.ShowDialog();
       
            if (frm.isSave)
            {
                Model.Stp3_GET_Rules[comboBox_models.SelectedIndex].Rules = frm.ruleslv1;
                button_models.Text = Model.Stp3_GET_Rules[comboBox_models.SelectedIndex].Rules.RulesName;

                    Model.Stp3_GET_Rules[comboBox_models.SelectedIndex].isGetPublicRules = false;
                    Model.Stp3_GET_Rules[comboBox_models.SelectedIndex].selectedValue = comboBox_valueindex.SelectedIndex;
            }
            
        }

        private void cmbselect1_SelectedIndexChanged(object sender, EventArgs e)
        {

                Model.Stp3_GET_Rules[comboBox_models.SelectedIndex].isGetPublicRules = false;
                Model.Stp3_GET_Rules[comboBox_models.SelectedIndex].selectedValue = comboBox_valueindex.SelectedIndex;
        }
       
        private void btntest_Click(object sender, EventArgs e)
        {
           s = new System.Threading.Thread(StratTest);
            s.IsBackground = true;
            s.Start();
            ShowI("正在提取文章...");
           
        }

        
        private void btnStp3Rules_Click(object sender, EventArgs e)
        {
            Model.Stp3_pagerule.TestUrl = txturl.Text;
            V3Form.MainRules frm = new MainRules();
            frm.cookietxt = Model.GetMajia;
            frm.ruleslv1 = Model.Stp3_pagerule;
            frm.needmore = true;
            frm.txtRead.Caption = "请生成一个返回分页链接的“返回多结果”规则！";
            frm.txtRead.Appearance.ForeColor = Color.Red;
    
            frm.ShowDialog();
  
            if (frm.isSave)
            {
                Model.Stp3_pagerule = frm.ruleslv1;
            }
           
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (isNeedMorePage.IsOn == false) { XtraMessageBox.Show( "请启用分页！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else 
            {
                buttonX3.Text = "正在测试";
                buttonX3.Enabled = false;
              s = new System.Threading.Thread(StratTestFenYe);
                s.IsBackground = true;
                s.Start();
                ShowI("正在测试分页规则...");
               

            }
        }

        private void Stp3_Allmodel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CloseI();
                btntest.Enabled = true;
                btntest.Text = "开始测试";
                buttonX3.Enabled = true;
                buttonX3.Text = "测试分页规则";
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

        private void Stp3_Allmodel_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsShow = false;
        }
    
    }
}
