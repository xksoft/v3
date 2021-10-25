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
    public partial class frmGroup_Rename : DevExpress.XtraEditors.XtraForm
    {
        public frmGroup_Rename()
        {
            InitializeComponent();
        }
        public bool issave = false;

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (name.Text == "")
            {
                XtraMessageBox.Show("分组名必须填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            issave = true;
            this.Close();
        }

        private void name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonX1_Click(sender, e);
            }
        }
    }
}
