using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace V3.V3Form
{
    public partial class frm_新增库 : DevExpress.XtraEditors.XtraForm
    {
        public string GroupString = "";
        /// <summary>
        /// 库类别 1文章库 2关键词库 3哈希库 4替换库 5链接库
        /// </summary>
        public int kutype = 1;
        public string kuname = "";
        public string kutypeid = "";
        public bool issave=false;
        public bool add1 = false;
        public bool add2 = false;
        public bool add3 = false;
        public bool add4 = false;
        public bool add5 = false;
        public frm_新增库()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (textBoxX1.Text.Trim() == "")
            {
                XtraMessageBox.Show( "请输入库名称！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            kuname = textBoxX1.Text.Trim();
            kutypeid = ((DataRowView)cmbGroup.GetSelectedDataRow())["Tag"].ToString();
            issave = true;
            add1 = checkBoxX1.Checked;
            add2 = checkBoxX2.Checked;
            add3 = checkBoxX3.Checked;
            add4 = checkBoxX4.Checked;
            add5 = checkBoxX5.Checked;
            Model.V3Infos.MainDb.LastTypdId = kutypeid;
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            issave = false;
            this.Close();
        }

        private void textBoxX1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonX1_Click(sender, e);
            }
        }

        private void frm_新增库_Shown(object sender, EventArgs e)
        {
            if (kutype == 1)
            {
                checkBoxX1.Checked = true;
                checkBoxX1.Enabled = false;
                this.Text = "新增一个文章库";
            }
            else if (kutype == 2)
            {
                checkBoxX2.Checked = true;
                checkBoxX2.Enabled = false;
                this.Text = "新增一个关键词库";
            }
            else if (kutype == 3)
            {
                checkBoxX3.Checked = true;
                checkBoxX3.Enabled = false;
                this.Text = "新增一个哈希库";
            }
            else if (kutype == 4)
            {
                checkBoxX4.Checked = true;
                checkBoxX4.Enabled = false;
                this.Text = "新增一个替换库";
            }
            else if (kutype == 5)
            {
                checkBoxX5.Checked = true;
                checkBoxX5.Enabled = false;
                this.Text = "新增一个链接库";
            }
            textBoxX1.Text = DateTime.Now.ToString("yyyyMMddHHmmss") + "新建库";
        }

        private void frm_新增库_Load(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)cmbGroup.Properties.DataSource;
            cmbGroup.EditValue = "Tag";
            cmbGroup.Properties.ValueMember = "Tag";
            cmbGroup.Properties.DisplayMember = "Name";
            cmbGroup.Properties.Columns.Add(
              new DevExpress.XtraEditors.Controls.LookUpColumnInfo(cmbGroup.Properties.DisplayMember));
            cmbGroup.ItemIndex = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Tag"].ToString() == GroupString)
                {
                    cmbGroup.ItemIndex = i;
                    break;
                }
            }
        }
    }
}
