using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using V3.V3Form.发布模块;

namespace V3.V3Form
{
    public partial class frmReplaceTag : DevExpress.XtraEditors.XtraForm
    {
        public frmReplaceTag()
        {
            InitializeComponent();
         
        }
        public bool issave = false;
        public string referer = "";
        public string tag = "";

        private void txtname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tag = txtname.Text;
                issave = true;
                this.Close();
            }
        }

        private void txtreadme_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tag = txtname.Text;
                issave = true;
                this.Close();
            }
        }

        private void barButtonItem_login_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           
            DevExpress.XtraBars.BarButtonItem btn = e.Item as DevExpress.XtraBars.BarButtonItem;
            if (btn != null)
            {
                txtname.Text = btn.Caption;
                txtreadme.Text = btn.Tag.ToString();
               
            }
         
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel btn = sender as LinkLabel;
            if (btn != null)
            {
                if (!btn.Text.Contains("〖"))
                {
                    txtname.Text = "【"+btn.Text+"】";
                }
                else
                {
                    txtname.Text = btn.Text;
                }

                txtreadme.Text = btn.Tag.ToString();
                tag = txtname.Text;
                if (referer == "Ppost_txturl")
                {
                    Ppost.MyPpost.txturl.SelectedText = txtname.Text;
                    Ppost.MyPpost.txturl.Focus();
                }
                else if (referer == "Ppost_txtrefrere")
                {
                    Ppost.MyPpost.txtrefrere.SelectedText = txtname.Text;
                    Ppost.MyPpost.txtrefrere.Focus();
                }
                else if (referer == "Ppost_txtPostData")
                {
                    Ppost.MyPpost.txtPostData.SelectedText = txtname.Text;
                    Ppost.MyPpost.txtPostData.Focus();
                }
                else if (referer == "Pget_txturl")
                {
                    Pget.MyPget.txturl.SelectedText = txtname.Text;
                    Pget.MyPget.txturl.Focus();
                }
                else if (referer == "Pget_txtrefrere")
                {
                    Pget.MyPget.txtrefrere.SelectedText = txtname.Text;
                    Pget.MyPget.txtrefrere.Focus();
                }
                else if (referer == "PostStp1_txtvcheckcodeurl")
                {
                    PostStp1.MyPostStp1.txtvcheckcodeurl.SelectedText = txtname.Text;
                    PostStp1.MyPostStp1.txtvcheckcodeurl.Focus();
                }
                else if (referer == "PostStp3_txtvcheckcodeurl")
                {
                    PostStp3.MyPostStp3.txtvcheckcodeurl.SelectedText = txtname.Text;
                    PostStp3.MyPostStp3.txtvcheckcodeurl.Focus();
                }
                else if (referer == "PostStp3_GetLinkUrl")
                {
                    PostStp3.MyPostStp3.GetLinkUrl.SelectedText = txtname.Text;
                    PostStp3.MyPostStp3.GetLinkUrl.Focus();
                }
                else if (referer == "frmTask_textBoxXtou")
                {
                    frmTask.MyfrmTask.textBoxXtou.SelectedText = txtname.Text;
                    frmTask.MyfrmTask.textBoxXtou.Focus();
                }
                else if (referer == "frmTask_textBoxXwei")
                {
                    frmTask.MyfrmTask.textBoxXwei.SelectedText = txtname.Text;
                    frmTask.MyfrmTask.textBoxXwei.Focus();
                }
                else if (referer == "frm_文章导出导入工具_text_path")
                {
                    frm_文章导入导出工具.Myfrm_文章导出导入工具.text_path.SelectedText = txtname.Text;
                    frm_文章导入导出工具.Myfrm_文章导出导入工具.text_path.Focus();
                }
                else if (referer == "frm_文章导出导入工具_text_formate")
                {
                    frm_文章导入导出工具.Myfrm_文章导出导入工具.text_formate.SelectedText = txtname.Text;
                    frm_文章导入导出工具.Myfrm_文章导出导入工具.text_formate.Focus();
                }
                else if (referer == "frm_文章导出导入工具_text_filename")
                {
                    frm_文章导入导出工具.Myfrm_文章导出导入工具.text_filename.SelectedText = txtname.Text;
                    frm_文章导入导出工具.Myfrm_文章导出导入工具.text_filename.Focus();
                }
                else if (referer== "frm_super2_textEdit_Title")
                {
                    frmSuper2.myfrmSuper2.textEdit_Title.SelectedText = txtname.Text;
                    frmSuper2.myfrmSuper2.textEdit_Title.Focus();
                }
                else if (referer == "frm_super2_textEdit_Keyword")
                {
                    frmSuper2.myfrmSuper2.textEdit_Keyword.SelectedText = txtname.Text;
                    frmSuper2.myfrmSuper2.textEdit_Keyword.Focus();
                }
                else if (referer == "frm_super2_textEdit_Content")
                {
                    frmSuper2.myfrmSuper2.textEdit_Content.SelectedText = txtname.Text;
                    frmSuper2.myfrmSuper2.textEdit_Content.Focus();
                }

            }
        }

        private void frmReplaceTag_Load(object sender, EventArgs e)
        {
            if (referer == "frm_文章导出导入工具_text_path" || referer == "frm_文章导出导入工具_text_formate" ||
                referer == "frm_文章导出导入工具_text_filename")
            {
                groupControl1.Enabled = groupControl3.Enabled = linkLabel1.Enabled = linkLabel9.Enabled = linkLabel10.Enabled = linkLabel17.Enabled = false;
            }
            else
            {
                groupControl1.Enabled = groupControl3.Enabled = linkLabel1.Enabled = linkLabel9.Enabled = linkLabel10.Enabled = linkLabel17.Enabled = true;
            }
        }
    }
}
