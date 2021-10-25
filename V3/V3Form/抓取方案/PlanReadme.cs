using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace V3.V3Form.抓取模块
{
    public partial class PlanReadme : DevExpress.XtraEditors.XtraForm
    {
        public PlanReadme()
        {
            InitializeComponent();
        }

        public Model.GetPostModel model = new Model.GetPostModel();
        public bool issave = false;
        void SetInfo()
        {
            model.PlanName = txtPlanname.Text;
            model.PlanUrl = txtPlanUrl.Text;
            model.PlanReadme = txtReadme.Text;
            model.PlanDescripton = txtDescription.Text;
            if (s2.Checked)
                model.IsShareModel = true;
            else
                model.IsShareModel = false;
            model.ShareLevel = cmbmoney.SelectedIndex;
        }
        void GetInfo()
        {
            txtPlanname.Text = model.PlanName;
            txtPlanUrl.Text = model.PlanUrl;
            txtReadme.Text = model.PlanReadme;
            txtDescription.Text = model.PlanDescripton;
            if (model.IsShareModel)
                s2.Checked = true;
            else
                s1.Checked = true;
            cmbmoney.SelectedIndex = model.ShareLevel;
        }


        private void s1_CheckedChanged(object sender, EventArgs e)
        {
            if (s2.Checked)
            {
                cmbmoney.Enabled = true;
            }
            else
            {
                cmbmoney.Enabled = false;
            }
        }

        private void buttonItem3_Click(object sender, EventArgs e)
        {
           
        }

        private void buttonItem1_Click(object sender, EventArgs e)
        {
          
        }

        private void PlanReadme_Load(object sender, EventArgs e)
        {
            GetInfo();
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

        private void cmbmoney_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbmoney.SelectedIndex != model.ShareLevel)
            {
                XtraMessageBox.Show("在线模块交易将在近期开放，您可以先将模块设置为私有或者免费！","暂时不能设置模块价格",  MessageBoxButtons.OK);
                cmbmoney.SelectedIndex = model.ShareLevel;
            }
        }
    }
}
