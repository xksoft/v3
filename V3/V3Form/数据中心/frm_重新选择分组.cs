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
    public partial class frm_重新选择分组 : DevExpress.XtraEditors.XtraForm
    {
        public frm_重新选择分组()
        {
            InitializeComponent();
        }
        public bool issave = false;
        public string GroupString = "";
        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (GroupString == "")
            {
                XtraMessageBox.Show("请选择一个分组！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            issave = true;
            this.Close();
        }



        private void frm_重新选择分组_Load(object sender, EventArgs e)
        {
            DataTable dt = (DataTable) cmbGroup.Properties.DataSource;
            cmbGroup.EditValue = "Tag";
            cmbGroup.Properties.ValueMember = "Tag";
            cmbGroup.Properties.DisplayMember = "Name";
            cmbGroup.Properties.Columns.Add(
              new DevExpress.XtraEditors.Controls.LookUpColumnInfo(cmbGroup.Properties.DisplayMember));
            for (int i=0;i<dt.Rows.Count;i++)
            {
                if (dt.Rows[i]["Tag"].ToString()==GroupString)
                {
                    cmbGroup.ItemIndex = i;
                    break;
                }
            }
        }

        private void cmbGroup_EditValueChanged(object sender, EventArgs e)
        {
            if (cmbGroup.GetSelectedDataRow() != null)
            {
                GroupString = ((DataRowView) cmbGroup.GetSelectedDataRow())["Tag"].ToString();
            }

        }



    }
}
