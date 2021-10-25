using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Threading;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using xEngine.Common;

namespace V3.V3Form
{
    public partial class frmPoint : DevExpress.XtraEditors.XtraForm
    {
        
        public frmPoint()
        {
            InitializeComponent();
        }
      
        public bool IsSave = false;
        public System.Threading.Thread thread;
        public Model.SendPoint Model = new Model.SendPoint();
        public string Group = "DefaultGroup";
        public bool isShow = false;
        public void ShowI(string txt)
        {
            if (isShow)
            {
                this.Invoke((EventHandler)(delegate
            {
                ipanel.Visible = true;
                istate.Text = txt;
            }));
            }

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

        public void SetInfo()
        {
            Group = "DefaultGroup";
            foreach (KeyValuePair<string, string> k in global::Model.V3Infos.MainDb.GroupList)
            {
                if (comboBox_Group.SelectedItem.ToString()==k.Value)
                {
                    Group = k.Key;
                }
            }
            Model.GroupTag = Group;
            Model.name = txtPointName.Text;
            Model.description = txtDescription.Text;
            Model.AdminUrl = txtAdminUrl.Text;
            Model.Oneaccount.username = txtAdminName.Text;
            Model.Oneaccount.password = txtAdminPassword.Text;
            Model.Oneaccount.loginvalue1 = txtLoginValue1.Text;
            Model.Oneaccount.loginvalue2 = txtLoginValue2.Text;
            Model.AccountModel = btnSwitch_accountModel.IsOn;
            Model.IsUseModelBianma = !btnSwitch_encoding.IsOn;
            Model.IsPostUtf8 = bianmaisutf8.Checked;
            
        }

        public void GetInfo()
        {
            if (Model.id == 0)
            {
                comboBox_Group.Enabled = false;
            }
            else
            {
                comboBox_Group.Enabled = true;
            }
            if (global::Model.V3Infos.MainDb.MyModels.ContainsKey(Model.PostModel))
            {
                txtPostModelName.Text = "[" + global::Model.V3Infos.MainDb.MyModels[Model.PostModel].mids+ "]" + global::Model.V3Infos.MainDb.MyModels[Model.PostModel].PlanName;
             
            }
            else
            {
                txtPostModelName.Text = "未选择发布模块！";
            }
            if (Model.GroupTag == "DefaultGroup" || !global::Model.V3Infos.MainDb.GroupList.ContainsKey(Model.GroupTag))
            {
                comboBox_Group.SelectedItem = "默认分组";
            }
            else
            {
                comboBox_Group.SelectedItem = global::Model.V3Infos.MainDb.GroupList[Model.GroupTag];
            }


            txtPointName.Text = Model.name;
            txtDescription.Text = Model.description;
            txtAdminUrl.Text = Model.AdminUrl;
            txtAdminName.Text = Model.Oneaccount.username;
            txtAdminPassword.Text = Model.Oneaccount.password;
            txtLoginValue1.Text = Model.Oneaccount.loginvalue1;
            txtLoginValue2.Text = Model.Oneaccount.loginvalue2;

            btnSwitch_accountModel.IsOn = Model.AccountModel;

            btnSwitch_encoding.IsOn = !Model.IsUseModelBianma;
            if (btnSwitch_encoding.IsOn)
                bianmaisutf8.Enabled = true;
            else
                bianmaisutf8.Enabled = false;
            bianmaisutf8.Checked = Model.IsPostUtf8;
            btnAccountManage.Text = "(" + (Model.AccountData.AccountTrue.Count + Model.AccountData.AccountFalse.Count) + ")账号库";
            if (global::Model.V3Infos.ArticleDb.ContainsKey(Model.ArticleDbID.ToString()))
            {
                txtArticleName.Text = "(" + global::Model.V3Infos.ArticleDb[Model.ArticleDbID.ToString()].DataCount + ")" + "[" + global::Model.V3Infos.ArticleDb[Model.ArticleDbID.ToString()].Id + "]" + global::Model.V3Infos.ArticleDb[Model.ArticleDbID.ToString()].Name;
            }
            else
            {
                txtArticleName.Text = "未设置或已删除！";
            }
            if (global::Model.V3Infos.KeywordDb.ContainsKey(Model.KeywordDbID.ToString()))
            {
                txtKeywordName.Text = "(" + global::Model.V3Infos.KeywordDb[Model.KeywordDbID.ToString()].Keywords.Count + ")" + "[" + global::Model.V3Infos.KeywordDb[Model.KeywordDbID.ToString()].Id + "]" + global::Model.V3Infos.KeywordDb[Model.KeywordDbID.ToString()].Name;
            }
            else
            {
                txtKeywordName.Text = "未设置或已删除！";
            }
            if (global::Model.V3Infos.HashDb.ContainsKey(Model.HashDbID.ToString()))
            {
                txtHashName.Text = "(" + global::Model.V3Infos.HashDb[Model.HashDbID.ToString()].DataCount + ")" + "[" + global::Model.V3Infos.HashDb[Model.HashDbID.ToString()].Id + "]" + global::Model.V3Infos.HashDb[Model.HashDbID.ToString()].Name;
            }
            else
            {
                txtHashName.Text = "未设置或已删除！";
            }
            if (global::Model.V3Infos.ReplaceDb.ContainsKey(Model.ReplaceDbid.ToString()))
            {
                txtReplaceName.Text = "(" + global::Model.V3Infos.ReplaceDb[Model.ReplaceDbid.ToString()].Words.Count + ")" + "[" + global::Model.V3Infos.ReplaceDb[Model.ReplaceDbid.ToString()].Id + "]" + global::Model.V3Infos.ReplaceDb[Model.ReplaceDbid.ToString()].Name;
            }
            else
            {
                txtReplaceName.Text = "未设置或已删除！";
            }
            if (global::Model.V3Infos.LinkDb.ContainsKey(Model.LinkDbID.ToString()))
            {
                txtLinkName.Text = "(" + global::Model.V3Infos.LinkDb[Model.LinkDbID.ToString()].Links.Count + ")" + "[" + global::Model.V3Infos.LinkDb[Model.LinkDbID.ToString()].Id + "]" + global::Model.V3Infos.LinkDb[Model.LinkDbID.ToString()].Name;
            }
            else
            {
                txtLinkName.Text = "未设置或已删除！";
            }

            if (global::Model.V3Infos.MainDb.MyModels.ContainsKey(Model.GetModel))
            {
                txtGetModelName.Text = "[" + global::Model.V3Infos.MainDb.MyModels[Model.GetModel].mids + "]" + global::Model.V3Infos.MainDb.MyModels[Model.GetModel].PlanName;
            }
            else
            {
                txtGetModelName.Text = "未选择抓取模块！";
            }

            if (global::Model.V3Infos.MainDb.MyModels.ContainsKey(Model.PostModel))
            {
                txtPostModelName.Text = "[" + global::Model.V3Infos.MainDb.MyModels[Model.PostModel].mids+ "]" + global::Model.V3Infos.MainDb.MyModels[Model.PostModel].PlanName;
                if (global::Model.V3Infos.MainDb.MyModels[Model.PostModel].Ismajiamodel)
                {

                    labelX15.Visible = txtLoginValue2.Visible = false;
                    btnGetMajia.Visible = true;

                }
                else
                {

                    labelX15.Visible = txtLoginValue2.Visible = true;
                    btnGetMajia.Visible = false;
                }
            }
            else
            {
                txtPostModelName.Text = "未选择发布模块！";
            }

        }

        bool CheckFrmInfo()
        {
            if (txtPointName.Text.Length > 2)
            {
                if (txtAdminUrl.Text.Length < 2)
                {
                    XtraMessageBox.Show("您还没有填写后台地址信息哦", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAdminUrl.Focus();
                    return false;
                }


                if (Model.ArticleDbID == 0)
                {
                    XtraMessageBox.Show("您必须选择或新建一个文章库", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtArticleName.Focus();
                    return false;
                }
                if (Model.KeywordDbID == 0)
                {
                    XtraMessageBox.Show("您必须选择或新建一个关键词库", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtKeywordName.Focus();
                    return false;
                }
                if (Model.HashDbID == 0)
                {
                    XtraMessageBox.Show("您必须选择或新建一个哈希库", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtHashName.Focus();
                    return false;
                }
                if (Model.ReplaceDbid == 0)
                {
                    XtraMessageBox.Show("您必须选择或新建一个替换库", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtReplaceName.Focus();
                    return false;
                }
                if (Model.LinkDbID == 0)
                {
                    XtraMessageBox.Show("您必须选择或新建一个链接库", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtLinkName.Focus();
                    return false;
                }
                if (Model.GetModel == "")
                {
                    XtraMessageBox.Show("您必须选择一个抓取模块", "请选择抓取模块", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtGetModelName.Focus();
                    return false;
                }
                if (Model.PostModel != "" && global::Model.V3Infos.MainDb.MyModels.ContainsKey(Model.PostModel))
                {
                    if (btnSwitch_accountModel.IsOn)
                    {
                        if (global::Model.V3Infos.MainDb.MyModels[Model.PostModel].Ismajiamodel == false)
                        {
                            if (Model.AccountData.AccountTrue.Count == 0)
                            {
                                XtraMessageBox.Show("您选择的账号模式是多账号模式，但是当前账号库中没有任何有效账号信息哦", "无账号信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                btnAccountManage.Focus();
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (global::Model.V3Infos.MainDb.MyModels[Model.PostModel].Ismajiamodel == false)
                        {
                            if (txtAdminName.Text.Length < 2)
                            {
                                XtraMessageBox.Show("您选择的账号模式是单账号模式，但是您还没有填登录账号哦", "无账号信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtAdminName.Focus();
                                return false;
                            }
                            if (txtAdminPassword.Text.Length < 2)
                            {
                                XtraMessageBox.Show("您选择的账号模式是单账号模式，但是您还没有填登录密码哦", "无账号信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtAdminPassword.Focus();
                                return false;
                            }
                        }

                    }
                }
                else
                {
                    XtraMessageBox.Show("您必须选择一个发布模块", "请选择发布模块", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPostModelName.Focus();
                    return false;
                }
            }
            else
            {
                XtraMessageBox.Show("请填写发布点名称(3字符以上）！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPointName.Focus();
                return false;
            }

            return true;
        }

        private void TestLogin()
        {
            this.Invoke((EventHandler)(delegate
            {
                btnTestLogin.Enabled = false;
                btnClassManage.Enabled = false;
                btnTestSend.Enabled = false;
            }));
            SetInfo();
            if (global::Model.V3Infos.SendPointDb.ContainsKey(999999))
            {
                global::Model.V3Infos.SendPointDb[999999] = Model;
            }
            else
            {
                global::Model.V3Infos.SendPointDb.Add(999999, Model);
            }
            Model.Oneaccount.Status = "未知";
            V3.Bll.PostBll bll = new V3.Bll.PostBll(global::Model.V3Infos.MainDb.MyModels[Model.PostModel], 999999, 0);
            bll.account = Model.Oneaccount;
            string tempstr = "";
            string jieguo = bll.Login(false, ref tempstr);
            CloseI();
            if (jieguo == "LoginOK")
            {
                try
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        XtraMessageBox.Show("<color=green>测试登录成功</color>\r\n\r\n建议测试发布一下看看能否成功发布文章！", "登录成功", MessageBoxButtons.OK, MessageBoxIcon.Information, DefaultBoolean.True);
                    }));
                }
                catch { }
            }
            else
            {
                try
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        if (
                            XtraMessageBox.Show(
                                "测试登录失败,原因：<color=red>" + jieguo +
                                "</color>\r\n\r\n请仔细检查以下设置：\r\n1：后台地址、账号、密码是否填写正确\r\n2：是否选择了与网站系统和版本对应的发布模块\r\n3：模块编码是否和网站编码一致\r\n4：编辑当前发布模块，将网站后台信息填写到模块中测试登录，可以看到服务器返回的详细错误",
                                "登录失败[点击“是”查看服务器返回信息]", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information, DefaultBoolean.True) ==
                            DialogResult.Yes)
                        {
                            frmResponse f = new frmResponse();
                            f.Html = tempstr;
                            f.ShowDialog();
                        }
                    }));
                }
                catch { }
            }
            try
            {
                this.Invoke((EventHandler)(delegate
                {
                    btnTestLogin.Enabled = true;
                    btnClassManage.Enabled = true;
                    btnTestSend.Enabled = true;
                    global::Model.V3Infos.SendPointDb.Remove(999999);
                }));
            }
            catch { }



        }
        private void TestSend()
        {
            if (!CheckFrmInfo())
                return;
            this.Invoke((EventHandler)(delegate
            {
                btnTestLogin.Enabled = false;
                btnClassManage.Enabled = false;
                btnTestSend.Enabled = false;
            }));
            SetInfo();
            if (global::Model.V3Infos.SendPointDb.ContainsKey(999999))
            {
                global::Model.V3Infos.SendPointDb[999999] = Model;
            }
            else
            {
                global::Model.V3Infos.SendPointDb.Add(999999, Model);
            }
            V3.Bll.PostBll bll = new V3.Bll.PostBll(global::Model.V3Infos.MainDb.MyModels[Model.PostModel], 999999, 0);
            V3.Common.Log.LogNewline("[c11]任务：开始进行发布测试...[/c]");
            string html = "";
            string link = "";
            string result = "";
            bll.Login(false, ref html);
            if (global::Model.V3Infos.MainDb.MyModels[Model.PostModel].Stp2_POST_UsedClass)
            {
                V3.Common.Log.LogNewline("[c11]任务：正在获取分类信息...[/c]");
                result = bll.RunAction(global::Model.V3Infos.MainDb.MyModels[Model.PostModel].Stp2_POST_Get, false, "");
                string[] jieguo = bll.ClassGet(result);
                frmSelectChannel frmclass = new frmSelectChannel();
                frmclass.jieguo = jieguo;
                V3.Common.Log.LogNewline("[c11]任务：请选择或指定分类！[/c]");
                this.Invoke((EventHandler)(delegate
                {
                    frmclass.ShowDialog();
                }));
                if (frmclass.issave)
                {
                    bll.fenleistr = frmclass.fenleistr;
                }
            }
            V3.Common.Log.LogNewline("[c11]任务：正在进行发布...[/c]");
            result = bll.Send(false, ref html, ref link);
            if (result.Contains("SendOK"))
            {
                V3.Common.Log.LogNewline("[c12]任务：恭喜发布成功！[/c]");
                try
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        CloseI();
                        XtraMessageBox.Show("恭喜已经成功发布！", "发布成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                }
                catch { }
            }
            else
            {
                V3.Common.Log.LogNewline("[c14]任务：啊哦，没有发布成功，原因：" + result + "[/c]");
                try
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        CloseI();
                        if (
                            XtraMessageBox.Show("发布失败，原因：" + result, "发布失败，点击“是”查看服务器返回信息", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            frmResponse f = new frmResponse();
                            f.Html = html;
                            f.ShowDialog();
                        }



                    }));
                }
                catch { }
            }
            try
            {
                this.Invoke((EventHandler)(delegate
                {
                    btnTestLogin.Enabled = true;
                    btnClassManage.Enabled = true;
                    btnTestSend.Enabled = true;
                    global::Model.V3Infos.SendPointDb.Remove(999999);
                }));
            }
            catch { }
            CloseI();

        }
     




        private void PointFrm_Load(object sender, EventArgs e)
        {
            isShow = true;
            CheckForIllegalCrossThreadCalls = false;
            comboBox_Group.Properties.Items.Clear();
            comboBox_Group.Properties.Items.Add("默认分组");
            foreach (KeyValuePair<string, string> value in global::Model.V3Infos.MainDb.GroupList)
            {

                comboBox_Group.Properties.Items.AddRange(new object[] { value.Value });

            }
            GetInfo();
        }

        private void btnSelectArticle_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 0;
            frm.Text = "请选择或新建一个文章库[请“双击”选择您要使用的库]";
            frm.isselectdb = true;
            frm.ShowDialog();
            if (frm.selectedDBid != 0)
            {
                Model.ArticleDbID = frm.selectedDBid;
                txtArticleName.Text = "(" + global::Model.V3Infos.ArticleDb[Model.ArticleDbID.ToString()].DataCount + ")" + "[" + global::Model.V3Infos.ArticleDb[Model.ArticleDbID.ToString()].Id + "]" + global::Model.V3Infos.ArticleDb[Model.ArticleDbID.ToString()].Name;
            }
        }

        private void btnSelectKeyword_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 1;
            frm.Text = "请选择或新建一个关键词库[请“双击”选择您要使用的库]";
            frm.isselectdb = true;
            frm.ShowDialog();
            if (frm.selectedDBid != 0)
            {
                Model.KeywordDbID = frm.selectedDBid;
                txtKeywordName.Text = "(" + global::Model.V3Infos.KeywordDb[Model.KeywordDbID.ToString()].Keywords.Count + ")" + "[" + global::Model.V3Infos.KeywordDb[Model.KeywordDbID.ToString()].Id + "]" + global::Model.V3Infos.KeywordDb[Model.KeywordDbID.ToString()].Name;
            }
        }

        private void btnSelectHash_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 2;
            frm.Text = "请选择或新建一个哈希库[请“双击”选择您要使用的库]";
            frm.isselectdb = true;
            frm.ShowDialog();
            if (frm.selectedDBid != 0)
            {
                Model.HashDbID = frm.selectedDBid;
                txtHashName.Text = "(" + global::Model.V3Infos.HashDb[Model.HashDbID.ToString()].DataCount + ")" + "[" + global::Model.V3Infos.HashDb[Model.HashDbID.ToString()].Id + "]" + global::Model.V3Infos.HashDb[Model.HashDbID.ToString()].Name;
            }
        }

        private void btnSelectReplace_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 3;
            frm.Text = "请选择或新建一个替换库[请“双击”选择您要使用的库]";
            frm.isselectdb = true;
            frm.ShowDialog();
            if (frm.selectedDBid != 0)
            {
                Model.ReplaceDbid = frm.selectedDBid;
                txtReplaceName.Text = "(" + global::Model.V3Infos.ReplaceDb[Model.ReplaceDbid.ToString()].Words.Count + ")" + "[" + global::Model.V3Infos.ReplaceDb[Model.ReplaceDbid.ToString()].Id + "]" + global::Model.V3Infos.ReplaceDb[Model.ReplaceDbid.ToString()].Name;
            }
        }

        private void btnSelectLink_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 4;
            frm.Text = "请选择或新建一个链接库[请“双击”选择您要使用的库]";
            frm.isselectdb = true;
            frm.ShowDialog();
            if (frm.selectedDBid != 0)
            {
                Model.LinkDbID = frm.selectedDBid;
                txtLinkName.Text = "(" + global::Model.V3Infos.LinkDb[Model.LinkDbID.ToString()].Links.Count + ")" + "[" + global::Model.V3Infos.LinkDb[Model.LinkDbID.ToString()].Id + "]" + global::Model.V3Infos.LinkDb[Model.LinkDbID.ToString()].Name;
            }
        }

        private void btnAddArticle_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(1, "新建文章库", Group);
            if (id != 0)
            {
                Model.ArticleDbID = id;
                txtArticleName.Text = "(" + global::Model.V3Infos.ArticleDb[Model.ArticleDbID.ToString()].DataCount + ")" + "[" + global::Model.V3Infos.ArticleDb[Model.ArticleDbID.ToString()].Id + "]" + global::Model.V3Infos.ArticleDb[Model.ArticleDbID.ToString()].Name;
            }
        }

        private void btnAddKeyword_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(2, "新建关键词库", Group);
            if (id != 0)
            {
                Model.KeywordDbID = id;
                txtKeywordName.Text = "(" + global::Model.V3Infos.KeywordDb[Model.KeywordDbID.ToString()].Keywords.Count + ")" + "[" + global::Model.V3Infos.KeywordDb[Model.KeywordDbID.ToString()].Id + "]" + global::Model.V3Infos.KeywordDb[Model.KeywordDbID.ToString()].Name;
            }
        }

        private void btnAddhash_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(3, "新建哈希库", Group);
            if (id != 0)
            {
                Model.HashDbID = id;
                txtHashName.Text = "(" + global::Model.V3Infos.HashDb[Model.HashDbID.ToString()].DataCount + ")" + "[" + global::Model.V3Infos.HashDb[Model.HashDbID.ToString()].Id + "]" + global::Model.V3Infos.HashDb[Model.HashDbID.ToString()].Name;
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(4, "新建替换库", Group);
            if (id != 0)
            {
                Model.ReplaceDbid = id;
                txtReplaceName.Text = "(" + global::Model.V3Infos.ReplaceDb[Model.ReplaceDbid.ToString()].Words.Count + ")" + "[" + global::Model.V3Infos.ReplaceDb[Model.ReplaceDbid.ToString()].Id + "]" + global::Model.V3Infos.ReplaceDb[Model.ReplaceDbid.ToString()].Name;
            }
        }

        private void btnAddLink_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(5, "新建链接库", Group);
            if (id != 0)
            {
                Model.LinkDbID = id;
                txtLinkName.Text = "(" + global::Model.V3Infos.LinkDb[Model.LinkDbID.ToString()].Links.Count + ")" + "[" + global::Model.V3Infos.LinkDb[Model.LinkDbID.ToString()].Id + "]" + global::Model.V3Infos.LinkDb[Model.LinkDbID.ToString()].Name;
            }
        }

        private void btnSelectGetModel_Click(object sender, EventArgs e)
        {
            V3Form.frmModelShop frm = new frmModelShop();
            frm.selectmodel = 1;
            frm.Text = "模块市场 [请在“我的模块”中“双击”选择一个您要使用的“抓取模块”]";frm.ShowDialog();
            if (frm.selectedid != "")
            {
                Model.GetModel = frm.selectedid;
                txtGetModelName.Text = "[" + global::Model.V3Infos.MainDb.MyModels[Model.GetModel].mids + "]" + global::Model.V3Infos.MainDb.MyModels[Model.GetModel].PlanName;
            }
        }

        private void btnSelectPostModel_Click(object sender, EventArgs e)
        {
            V3Form.frmModelShop frm = new frmModelShop();
            frm.selectmodel = 2;
            frm.Text = "模块市场 [请在“我的模块”中“双击”选择一个您要使用的“发布模块”]";
            frm.ShowDialog();
            if (frm.selectedid != "")
            {
                Model.PostModel = frm.selectedid;
                txtPostModelName.Text = "[" + global::Model.V3Infos.MainDb.MyModels[Model.PostModel].mids + "]" + global::Model.V3Infos.MainDb.MyModels[Model.PostModel].PlanName;
                if (global::Model.V3Infos.MainDb.MyModels[Model.PostModel].Ismajiamodel)
                {

                    labelX15.Visible = txtLoginValue2.Visible = false;
                    btnGetMajia.Visible = true;

                }
                else
                {

                    labelX15.Visible = txtLoginValue2.Visible = true;
                    btnGetMajia.Visible = false;
                }
            }

        }

        private void btnViewReadme_Click(object sender, EventArgs e)
        {
            if (global::Model.V3Infos.MainDb.MyModels.ContainsKey(Model.PostModel))
            {
                try
                {
                    System.Diagnostics.Process.Start(global::Model.V3Infos.MainDb.MyModels[Model.PostModel].PlanUrl);
                }
                catch { }
            }
            else
            {

                XtraMessageBox.Show("请先选择一个发布模块！", "尚未选择发布模块", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnAccountManage_Click(object sender, EventArgs e)
        {
            V3Form.发布模块.UserAccount frm = new 发布模块.UserAccount();
            frm.Account = Model.AccountData;
            frm.ShowDialog();
            if (frm.IsSave)
            {
                Model.AccountData = frm.Account;
            }
            btnAccountManage.Text = "(" + (Model.AccountData.AccountTrue.Count + Model.AccountData.AccountFalse.Count) + ")账号库";
        }
       
        private void btnTestLogin_Click(object sender, EventArgs e)
        {
            if (!CheckFrmInfo())
                return;
            thread = new System.Threading.Thread(TestLogin);
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
            ShowI("正在测试登陆...");
        }
       

        private void btnTestSend_Click(object sender, EventArgs e)
        {
            if (!CheckFrmInfo())
                return;
           thread = new System.Threading.Thread(TestSend);
            thread.IsBackground = true;
            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
            ShowI("正在测试发布");
           
        }

        private void btnClassManage_Click(object sender, EventArgs e)
        {
            SetInfo();
            if (global::Model.V3Infos.SendPointDb.ContainsKey(999999))
            {
                global::Model.V3Infos.SendPointDb[999999] = Model;
            }
            else
            {
                global::Model.V3Infos.SendPointDb.Add(999999, Model);
            }
            frmChannel frm = new frmChannel();
            frm.Model = Model;
            if (!global::Model.V3Infos.MainDb.MyModels.ContainsKey(Model.PostModel))
            {
           XtraMessageBox.Show("发布模块不存在，请选择发布模块！","无法继续",MessageBoxButtons.OK,MessageBoxIcon.Information);
           return;
            }
            V3.Bll.PostBll bll = new V3.Bll.PostBll(global::Model.V3Infos.MainDb.MyModels[Model.PostModel], 999999, 0);
            frm.bll = bll;
            this.Invoke((EventHandler) (delegate
            {
                frm.ShowDialog();
            }));
            global::Model.V3Infos.SendPointDb.Remove(999999);
        }

        private void btnGetMajia_Click(object sender, EventArgs e)
        {
            frmGetCookie f = new frmGetCookie();
            f.CookieStr = Model.Oneaccount.Majiastr;
            f.Url = Model.Oneaccount.Majiaurl;
            f.ShowDialog();
            if (f.GetSuccess)
            {
                Model.Oneaccount.Majiaurl = f.Url;
                Model.Oneaccount.Majiastr = f.CookieStr; ;
                Model.Oneaccount.MyCookie = new xEngine.Model.Execute.Http.XCookieManager();
                xEngine.Execute.Http execute = new xEngine.Execute.Http();
                execute.CookieManager = Model.Oneaccount.MyCookie;
                execute.CookieAddStr(f.CookieStr);
                SetInfo();
            }
            

        }

        private void PointFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CloseI();
                btnTestLogin.Enabled = true;
                btnClassManage.Enabled = true;
                btnTestSend.Enabled = true;
                global::Model.V3Infos.SendPointDb.Remove(999999);
              

                if (thread != null)
                {
                    try
                    {
                        thread.Abort();
                    }
                    catch { }
                }

            }
        }

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetInfo();
            if (!CheckFrmInfo())
                return;
            IsSave = true;
            this.Close();
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IsSave = false;
            this.Close();
        }

        private void btn_setdefault_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            frmDefaultPoints f = new frmDefaultPoints();
            f.DefaultSendPoint = Model;
            f.ShowDialog();
            if (f.IsOK)
            {
                XtraMessageBox.Show("成功保存到发布点模板列表！", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
 
        }

        private void btn_usedefault_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmDefaultPoints f = new frmDefaultPoints();
            f.ShowDialog();
            if (f.IsOK)
            {
                int id = Model.id;

                Model = f.DefaultSendPoint;
                Model.Oneaccount = new Model.发布相关模型.Account();
                Model.Oneaccount.MyCookie = f.DefaultSendPoint.Oneaccount.MyCookie;
                Model.Oneaccount.CountFalse = f.DefaultSendPoint.Oneaccount.CountFalse;
                Model.Oneaccount.CountTrue = f.DefaultSendPoint.Oneaccount.CountTrue;
                Model.Oneaccount.LastActiveTime = f.DefaultSendPoint.Oneaccount.LastActiveTime;
                Model.Oneaccount.loginvalue1 = f.DefaultSendPoint.Oneaccount.loginvalue1;
                Model.Oneaccount.loginvalue2 = f.DefaultSendPoint.Oneaccount.loginvalue2;
                Model.Oneaccount.Majiastr = f.DefaultSendPoint.Oneaccount.Majiastr;
                Model.Oneaccount.Majiaurl = f.DefaultSendPoint.Oneaccount.Majiaurl;
                Model.Oneaccount.password = f.DefaultSendPoint.Oneaccount.password;
                Model.Oneaccount.Status = f.DefaultSendPoint.Oneaccount.Status;
                Model.Oneaccount.username = f.DefaultSendPoint.Oneaccount.username;



                Model.id = id;
                GetInfo();
            }
           
        }

        private void btnSwitch_encoding_Toggled(object sender, EventArgs e)
        {
            if (btnSwitch_encoding.IsOn)
                bianmaisutf8.Enabled = true;
            else
                bianmaisutf8.Enabled = false;
        
           
        }

        private void btnSwitch_accountModel_Toggled(object sender, EventArgs e)
        {
            if (btnSwitch_accountModel.IsOn)
            {
                btnAccountManage.Enabled = true;
                txtAdminName.Enabled = false;
                txtAdminPassword.Enabled = false;
                txtLoginValue1.Enabled = false;
                txtLoginValue2.Enabled = false;
            }
            else
            {
                btnAccountManage.Enabled = false;
                txtAdminName.Enabled = true;
                txtAdminPassword.Enabled = true;
                txtLoginValue1.Enabled = true;
                txtLoginValue2.Enabled = true;
            }
        }

        private void bianmaisutf8_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void frmPoint_FormClosed(object sender, FormClosedEventArgs e)
        {
            isShow = false;
        }
    }
}
