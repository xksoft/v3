using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace V3.V3Form.发布模块
{
    public partial class VcodeInput : DevExpress.XtraEditors.XtraForm
    {
        public VcodeInput()
        {
            InitializeComponent();
        }
        public bool issave = false;

        private void txtvcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                issave = true;
                this.Close();
            }
        }
    }
}
