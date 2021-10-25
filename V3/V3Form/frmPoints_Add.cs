using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Threading;
using DevExpress.XtraEditors;
using xEngine.Common;

namespace V3.V3Form
{
    public partial class frmPoints_Add : DevExpress.XtraEditors.XtraForm
    {
        public frmPoints_Add()
        {
            InitializeComponent();
        }
        public Thread addthread;
        public bool issave = false;
        Model.SendPoint model=new Model.SendPoint();
        public string Group = "DefaultGroup";
        void SetInfo()
        {
            //model.name = txtPointName.Text;
            //model.AdminUrl = txtwebinfos.Text;
            //model.Oneaccount.username = txtAdminName.Text;
            //model.Oneaccount.password = txtAdminPassword.Text;
            //model.Oneaccount.loginvalue1 = txtLoginValue1.Text;
            //model.Oneaccount.loginvalue2 = txtLoginValue2.Text;
            //model.GroupTag = Group;
            //model.IsUseModelBianma = postbianma.Value;
            //model.IsPostUtf8 = bianmaisutf8.Checked;
        }
        void GetInfo()
        {
           

                
        
          
            txtwebinfos.Text = model.name + "|" + model.AdminUrl + "|" + model.Oneaccount.username + "|" + model.Oneaccount.password + "|" + model.Oneaccount.loginvalue1 + "|" + model.Oneaccount.loginvalue2; 
            postbianma.IsOn = !model.IsUseModelBianma;
            if (postbianma.IsOn)
                bianmaisutf8.Enabled = true;
            else
                bianmaisutf8.Enabled = false;
            bianmaisutf8.Checked = model.IsPostUtf8;
            if (Model.V3Infos.ArticleDb.ContainsKey(model.ArticleDbID.ToString()))
            {
                txtArticleName.Text = "(" + Model.V3Infos.ArticleDb[model.ArticleDbID.ToString()].DataCount + ")" + "[" + Model.V3Infos.ArticleDb[model.ArticleDbID.ToString()].Id + "]" + Model.V3Infos.ArticleDb[model.ArticleDbID.ToString()].Name;
            }
            else
            {
                txtArticleName.Text = "未设置或已删除！";
            }
            if (Model.V3Infos.KeywordDb.ContainsKey(model.KeywordDbID.ToString()))
            {
                txtKeywordName.Text = "(" + Model.V3Infos.KeywordDb[model.KeywordDbID.ToString()].Keywords.Count + ")" + "[" + Model.V3Infos.KeywordDb[model.KeywordDbID.ToString()].Id + "]" + Model.V3Infos.KeywordDb[model.KeywordDbID.ToString()].Name;
            }
            else
            {
                txtKeywordName.Text = "未设置或已删除！";
            }
            if (Model.V3Infos.HashDb.ContainsKey(model.HashDbID.ToString()))
            {
                txtHashName.Text = "(" + Model.V3Infos.HashDb[model.HashDbID.ToString()].DataCount + ")" + "[" + Model.V3Infos.HashDb[model.HashDbID.ToString()].Id + "]" + Model.V3Infos.HashDb[model.HashDbID.ToString()].Name;
            }
            else
            {
                txtHashName.Text = "未设置或已删除！";
            }
            if (Model.V3Infos.ReplaceDb.ContainsKey(model.ReplaceDbid.ToString()))
            {
                txtReplaceName.Text = "(" + Model.V3Infos.ReplaceDb[model.ReplaceDbid.ToString()].Words.Count + ")" + "[" + Model.V3Infos.ReplaceDb[model.ReplaceDbid.ToString()].Id + "]" + Model.V3Infos.ReplaceDb[model.ReplaceDbid.ToString()].Name;
            }
            else
            {
                txtReplaceName.Text = "未设置或已删除！";
            }
            if (Model.V3Infos.LinkDb.ContainsKey(model.LinkDbID.ToString()))
            {
                txtLinkName.Text = "(" + Model.V3Infos.LinkDb[model.LinkDbID.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.LinkDbID.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.LinkDbID.ToString()].Name;
            }
            else
            {
                txtLinkName.Text = "未设置或已删除！";
            }

            if (Model.V3Infos.MainDb.MyModels.ContainsKey(model.GetModel))
            {
                txtGetModelName.Text = "[" + Model.V3Infos.MainDb.MyModels[model.GetModel].mids + "]" + Model.V3Infos.MainDb.MyModels[model.GetModel].PlanName;
            }
            else
            {
                txtGetModelName.Text = "未选择抓取模块！";
            }

            if (Model.V3Infos.MainDb.MyModels.ContainsKey(model.PostModel))
            {
                txtPostModelName.Text = "[" + Model.V3Infos.MainDb.MyModels[model.PostModel].mids + "]" + Model.V3Infos.MainDb.MyModels[model.PostModel].PlanName;
            }
            else
            {
                txtPostModelName.Text = "未选择发布模块！";
            }
            if (model.GroupTag.ToString() == "DefaultGroup") { comboBoxGroup.SelectedItem = "默认分组"; }
            else{
                try
                {
                    comboBoxGroup.SelectedItem = Model.V3Infos.MainDb.GroupList[model.GroupTag].ToString();
                }
                catch 
                {
                    comboBoxGroup.SelectedIndex = 0;
                }
            }

        }


        public void StartAddWeb()
        {
            int count = 0;
            string[] allinfo = txtwebinfos.Lines;
            for (int i = 0; i < allinfo.Length; i++)
            {
                if (allinfo[i].Trim().Length > 0)
                {
                    string[] webinfo = allinfo[i].Split('|');
                    if (webinfo.Length < 6)
                    {
                        this.Invoke((EventHandler)(delegate
                        {
                            statetxt.Text = "站点信息格式有误！跳过...";
                        }));

                        V3.Common.Log.LogNewline("[c11]站点信息格式有误！跳过...,继续添加下一个站点[/c]");
                        continue;
                    }
                    Model.SendPoint mymodel = new Model.SendPoint();
                    mymodel.name = webinfo[0];
                    this.Invoke((EventHandler)(delegate
                    {
                        statetxt.Text = "正在添加站点：" + mymodel.name;
                    }));
                    mymodel.AdminUrl = webinfo[1];
                    mymodel.Oneaccount.username = webinfo[2];
                    mymodel.Oneaccount.password = webinfo[3];
                    mymodel.Oneaccount.loginvalue1 = webinfo[4];
                    mymodel.Oneaccount.loginvalue2 = webinfo[5];
                    mymodel.IsUseModelBianma = !postbianma.IsOn;
                    mymodel.IsPostUtf8 = bianmaisutf8.Checked;



                    if (Model.V3Infos.MainDb.MyModels.ContainsKey(model.GetModel))
                    {
                        txtGetModelName.Text = "[" + Model.V3Infos.MainDb.MyModels[model.GetModel].mids + "]" +
                                               Model.V3Infos.MainDb.MyModels[model.GetModel].PlanName;
                    }
                    else
                    {
                        this.Invoke((EventHandler)(delegate
                        {
                            XtraMessageBox.Show("您还未选择抓取模块！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtGetModelName.Text = "未选择抓取模块！";
                        }));
                        break;
                    }

                    if (Model.V3Infos.MainDb.MyModels.ContainsKey(model.PostModel))
                    {
                        this.Invoke((EventHandler)(delegate
                        {
                            txtPostModelName.Text = "[" + Model.V3Infos.MainDb.MyModels[model.PostModel].mids + "]" +
                                                    Model.V3Infos.MainDb.MyModels[model.PostModel].PlanName;
                        }));
                    }
                    else
                    {
                        this.Invoke((EventHandler)(delegate
                        {
                            XtraMessageBox.Show("您还未选择发布模块！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtPostModelName.Text = "未选择发布模块！";
                        }));
                        break;
                    }

                    if (comboBoxGroup.SelectedItem.ToString() == "默认分组") { mymodel.GroupTag = "DefaultGroup"; }
                    else
                    {
                        foreach (KeyValuePair<string, string> value in Model.V3Infos.MainDb.GroupList)
                        {
                            if (comboBoxGroup.SelectedItem.ToString() == value.Value)
                            {
                                mymodel.GroupTag = value.Key;
                            }

                        }

                    }
                    //设置库信息
                    if (switchButtonzidongku.IsOn == true)//全部自动创建
                    {
                        int articleid = V3.Bll.DbBll.AddDb(1, "新建文章库", mymodel.GroupTag);
                        if (articleid != 0)
                        {
                            mymodel.ArticleDbID = articleid;
                            V3.Common.Log.LogNewline("[c12]成功创建一个文章库，编号为：" + articleid+"[/c]");
                        }

                        int hashid = V3.Bll.DbBll.AddDb(3, "新建哈希库", mymodel.GroupTag);
                        if (hashid != 0)
                        {
                            mymodel.HashDbID = hashid;
                            V3.Common.Log.LogNewline("[c12]成功创建一个哈希库，编号为：" + hashid + "[/c]");
                        }

                        int replaceid = V3.Bll.DbBll.AddDb(4, "新建替换库", mymodel.GroupTag);
                        if (replaceid != 0)
                        {
                            mymodel.ReplaceDbid = replaceid;
                            V3.Common.Log.LogNewline("[c12]成功创建一个替换库，编号为：" + replaceid + "[/c]");
                        }
                        int keywordid = V3.Bll.DbBll.AddDb(2, "新建关键词库", mymodel.GroupTag);
                        if (keywordid != 0)
                        {
                            mymodel.KeywordDbID = keywordid;
                            V3.Common.Log.LogNewline("[c12]成功创建一个关键词，编号为：" + keywordid + "[/c]");
                        }

                        int lianjieid = V3.Bll.DbBll.AddDb(5, "新建链接库", mymodel.GroupTag);
                        if (lianjieid != 0)
                        {
                            mymodel.LinkDbID = lianjieid;
                            V3.Common.Log.LogNewline("[c12]成功创建一个链接库，编号为：" + lianjieid + "[/c]");
                        }

                        mymodel.HashDbID = hashid;
                        mymodel.KeywordDbID = keywordid;
                        mymodel.LinkDbID = lianjieid;
                        mymodel.ReplaceDbid = replaceid;


                        mymodel.GetModel = model.GetModel;
                        mymodel.PostModel = model.PostModel;


                        int id = V3.Bll.PointBll.Add(mymodel);
                        if (id > 0)
                        {
                            V3.Common.Log.LogNewline("[c12]成功添加一个站点" + mymodel.name+"[/c]");
                            count++;
                        }
                        else { V3.Common.Log.LogNewline("[c14]站点【" + mymodel.name + "】添加失败！[/c]"); }

                    }
                    else//使用现有
                    {
                        if (Model.V3Infos.ArticleDb.ContainsKey(model.ArticleDbID.ToString()))
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                txtArticleName.Text = "(" + Model.V3Infos.ArticleDb[model.ArticleDbID.ToString()].DataCount +
                                                      ")" + "[" + Model.V3Infos.ArticleDb[model.ArticleDbID.ToString()].Id +
                                                      "]" + Model.V3Infos.ArticleDb[model.ArticleDbID.ToString()].Name;
                            }));
                        }
                        else
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                XtraMessageBox.Show("您还未选择文章库！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtArticleName.Text = "未设置或已删除！";
                            }));
                            break;

                        }
                        if (Model.V3Infos.KeywordDb.ContainsKey(model.KeywordDbID.ToString()))
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                txtKeywordName.Text = "(" +
                                                      Model.V3Infos.KeywordDb[model.KeywordDbID.ToString()].Keywords.Count +
                                                      ")" + "[" + Model.V3Infos.KeywordDb[model.KeywordDbID.ToString()].Id +
                                                      "]" + Model.V3Infos.KeywordDb[model.KeywordDbID.ToString()].Name;
                            }));
                        }
                        else
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                XtraMessageBox.Show("您还未选择关键词库！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtKeywordName.Text = "未设置或已删除！";
                            }));
                            break;
                        }
                        if (Model.V3Infos.HashDb.ContainsKey(model.HashDbID.ToString()))
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                txtHashName.Text = "(" + Model.V3Infos.HashDb[model.HashDbID.ToString()].DataCount + ")" +
                                                   "[" +
                                                   Model.V3Infos.HashDb[model.HashDbID.ToString()].Id + "]" +
                                                   Model.V3Infos.HashDb[model.HashDbID.ToString()].Name;
                            }));
                        }
                        else
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                XtraMessageBox.Show("您还未选择哈希库！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtHashName.Text = "未设置或已删除！";
                            }));
                            break;
                        }
                        if (Model.V3Infos.ReplaceDb.ContainsKey(model.ReplaceDbid.ToString()))
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                txtReplaceName.Text = "(" +
                                                      Model.V3Infos.ReplaceDb[model.ReplaceDbid.ToString()].Words.Count +
                                                      ")" + "[" + Model.V3Infos.ReplaceDb[model.ReplaceDbid.ToString()].Id +
                                                      "]" + Model.V3Infos.ReplaceDb[model.ReplaceDbid.ToString()].Name;
                            }));
                        }
                        else
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                XtraMessageBox.Show("您还未选择替换库！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtReplaceName.Text = "未设置或已删除！";
                            }));
                            break;
                        }
                        if (Model.V3Infos.LinkDb.ContainsKey(model.LinkDbID.ToString()))
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                txtLinkName.Text = "(" + Model.V3Infos.LinkDb[model.LinkDbID.ToString()].Links.Count + ")" +
                                                   "[" +
                                                   Model.V3Infos.LinkDb[model.LinkDbID.ToString()].Id + "]" +
                                                   Model.V3Infos.LinkDb[model.LinkDbID.ToString()].Name;
                            }));
                        }
                        else
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                XtraMessageBox.Show("您还未选择链接库！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtLinkName.Text = "未设置或已删除！";
                            }));
                            break;
                        }
                        mymodel.ArticleDbID = model.ArticleDbID;
                        mymodel.HashDbID = model.HashDbID;
                        mymodel.KeywordDbID = model.KeywordDbID;
                        mymodel.LinkDbID = model.LinkDbID;
                        mymodel.ReplaceDbid = model.ReplaceDbid;
                        mymodel.GetModel = model.GetModel;
                        mymodel.PostModel = model.PostModel;

                        int id = V3.Bll.PointBll.Add(mymodel);
                        if (id > 0)
                        {
                            V3.Common.Log.LogNewline("[c12]成功添加一个站点" + mymodel.name+"[/c]");
                            count++;
                        }
                        else { V3.Common.Log.LogNewline("[c14]站点【" + mymodel.name + "】添加失败！[/c]"); }
                    }





                }
            }
            this.BeginInvoke((EventHandler)(delegate
            {
                CloseStateControls();
                DevExpress.XtraEditors.XtraMessageBox.Show("成功添加" + count + "个站点！", "添加成功", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            }));
            V3.Common.Log.LogNewline("[c12]成功添加" + count + "个站点！[/c]");
            if (addthread.ThreadState != ThreadState.Aborted)
            {
                addthread.Abort();
            }



        }

        public void ShowStateControls()
        {
            ipanel.Visible = true;

        }
        public void CloseStateControls()
        {

            ipanel.Visible = false;

        }


        private void PointFrm_Load(object sender, EventArgs e)
        {
            
            //加载所有分组信息
            comboBoxGroup.Properties.Items.Add("默认分组");
            foreach (KeyValuePair<string, string> value in Model.V3Infos.MainDb.GroupList)
            {

                comboBoxGroup.Properties.Items.AddRange(new object[] { value.Value });
               
            } comboBoxGroup.SelectedIndex = 0;
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
                model.ArticleDbID = frm.selectedDBid;
                txtArticleName.Text = "(" + Model.V3Infos.ArticleDb[model.ArticleDbID.ToString()].DataCount + ")" + "[" + Model.V3Infos.ArticleDb[model.ArticleDbID.ToString()].Id + "]" + Model.V3Infos.ArticleDb[model.ArticleDbID.ToString()].Name;
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
                model.KeywordDbID = frm.selectedDBid;
                txtKeywordName.Text = "(" + Model.V3Infos.KeywordDb[model.KeywordDbID.ToString()].Keywords.Count + ")" + "[" + Model.V3Infos.KeywordDb[model.KeywordDbID.ToString()].Id + "]" + Model.V3Infos.KeywordDb[model.KeywordDbID.ToString()].Name;
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
                model.HashDbID = frm.selectedDBid;
                txtHashName.Text = "(" + Model.V3Infos.HashDb[model.HashDbID.ToString()].DataCount + ")" + "[" + Model.V3Infos.HashDb[model.HashDbID.ToString()].Id + "]" + Model.V3Infos.HashDb[model.HashDbID.ToString()].Name;
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
                model.ReplaceDbid = frm.selectedDBid;
                txtReplaceName.Text = "(" + Model.V3Infos.ReplaceDb[model.ReplaceDbid.ToString()].Words.Count + ")" + "[" + Model.V3Infos.ReplaceDb[model.ReplaceDbid.ToString()].Id + "]" + Model.V3Infos.ReplaceDb[model.ReplaceDbid.ToString()].Name;
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
                model.LinkDbID = frm.selectedDBid;
                txtLinkName.Text = "(" + Model.V3Infos.LinkDb[model.LinkDbID.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.LinkDbID.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.LinkDbID.ToString()].Name;
            }
        }

        private void btnAddArticle_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(1, "新建文章库", Group);
            if (id != 0)
            {
                model.ArticleDbID = id;
                txtArticleName.Text = "(" + Model.V3Infos.ArticleDb[model.ArticleDbID.ToString()].DataCount + ")" + "[" + Model.V3Infos.ArticleDb[model.ArticleDbID.ToString()].Id + "]" + Model.V3Infos.ArticleDb[model.ArticleDbID.ToString()].Name;
            }
        }

        private void btnAddKeyword_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(2, "新建关键词库", Group);
            if (id != 0)
            {
                model.KeywordDbID = id;
                txtKeywordName.Text = "(" + Model.V3Infos.KeywordDb[model.KeywordDbID.ToString()].Keywords.Count + ")" + "[" + Model.V3Infos.KeywordDb[model.KeywordDbID.ToString()].Id + "]" + Model.V3Infos.KeywordDb[model.KeywordDbID.ToString()].Name;
            }
        }

        private void btnAddhash_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(3, "新建哈希库", Group);
            if (id != 0)
            {
                model.HashDbID = id;
                txtHashName.Text = "(" + Model.V3Infos.HashDb[model.HashDbID.ToString()].DataCount + ")" + "[" + Model.V3Infos.HashDb[model.HashDbID.ToString()].Id + "]" + Model.V3Infos.HashDb[model.HashDbID.ToString()].Name;
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(4, "新建替换库", Group);
            if (id != 0)
            {
                model.ReplaceDbid = id;
                txtReplaceName.Text = "(" + Model.V3Infos.ReplaceDb[model.ReplaceDbid.ToString()].Words.Count + ")" + "[" + Model.V3Infos.ReplaceDb[model.ReplaceDbid.ToString()].Id + "]" + Model.V3Infos.ReplaceDb[model.ReplaceDbid.ToString()].Name;
            }
        }

        private void btnAddLink_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(5, "新建链接库", Group);
            if (id != 0)
            {
                model.LinkDbID = id;
                txtLinkName.Text = "(" + Model.V3Infos.LinkDb[model.LinkDbID.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.LinkDbID.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.LinkDbID.ToString()].Name;
            }
        }

        private void btnSelectGetModel_Click(object sender, EventArgs e)
        {
            V3Form.frmModelShop frm = new frmModelShop();
            frm.selectmodel = 1;
            frm.Text = "V3模块市场 [请在“我的模块”中“双击”选择一个您要使用的“抓取模块”]";frm.ShowDialog();
            if (frm.selectedid != "")
            {
                model.GetModel = frm.selectedid;
                txtGetModelName.Text = "[" + Model.V3Infos.MainDb.MyModels[model.GetModel].mids + "]" + Model.V3Infos.MainDb.MyModels[model.GetModel].PlanName;
            }
        }

        private void btnSelectPostModel_Click(object sender, EventArgs e)
        {
            V3Form.frmModelShop frm = new frmModelShop();
            frm.selectmodel = 2;
            frm.Text = "V3模块市场 [请在“我的模块”中“双击”选择一个您要使用的“发布模块”]";
            frm.ShowDialog();
            if (frm.selectedid != "")
            {
                model.PostModel = frm.selectedid;
                txtPostModelName.Text = "[" + Model.V3Infos.MainDb.MyModels[model.PostModel].mids + "]" + Model.V3Infos.MainDb.MyModels[model.PostModel].PlanName;
            }
        }

        private void btnTestLogin_Click(object sender, EventArgs e)//开始添加站点
        {
            ShowStateControls();
            addthread = new Thread(StartAddWeb);
            addthread.Start();
            addthread.IsBackground = true;
            
        }

        private void buttonquxiao_Click(object sender, EventArgs e)//停止
        {
            if (addthread.ThreadState != ThreadState.Aborted)
            {
                addthread.Abort();
            }
            CloseStateControls();

        }

        private void btn_usedefault_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (Model.V3Infos.MainDb.DefaultSendPoint.id == 0) { XtraMessageBox.Show("没有设置默认站点模板！"); }
            //else
            //{
            //    model = xEngine.Common.XSerializable.CloneObject<Model.SendPoint>(Model.V3Infos.MainDb.DefaultSendPoint);

            //    GetInfo();
            //}
        }

        private void postbianma_Toggled(object sender, EventArgs e)
        {
            if (postbianma.IsOn)
                bianmaisutf8.Enabled = true;
            else
                bianmaisutf8.Enabled = false;
        }

        private void switchButtonzidongku_Toggled(object sender, EventArgs e)
        {

            if (switchButtonzidongku.IsOn == true)//创建新库
            {
                btnAddArticle.Enabled = false;
                btnAddhash.Enabled = false;
                btnAddKeyword.Enabled = false;
                btnAddLink.Enabled = false;
                btnReplace.Enabled = false;

                btnSelectArticle.Enabled = false;
                btnSelectHash.Enabled = false;
                btnSelectLink.Enabled = false;
                btnSelectReplace.Enabled = false;
                btnSelectKeyword.Enabled = false;
            }
            else//使用现有库
            {
                btnAddArticle.Enabled = true;
                btnAddhash.Enabled = true;
                btnAddKeyword.Enabled = true;
                btnAddLink.Enabled = true;
                btnReplace.Enabled = true;

                btnSelectArticle.Enabled = true;
                btnSelectHash.Enabled = true;
                btnSelectLink.Enabled = true;
                btnSelectReplace.Enabled = true;
                btnSelectKeyword.Enabled = true;

            }
        }



    }
}
