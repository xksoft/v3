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
    public partial class frm_重命名库 : DevExpress.XtraEditors.XtraForm
    {
        public frm_重命名库()
        {
            InitializeComponent();
        }
        public bool issave = false;

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (name.Text == "")
            {
                XtraMessageBox.Show("请填写库名称！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
