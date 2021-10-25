using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace V3.V3Form.发布模块
{
    public partial class UserInput : DevExpress.XtraEditors.XtraForm
    {
        public UserInput()
        {
            InitializeComponent();
        }
        public bool issave = false;

        private void buttonX1_Click(object sender, EventArgs e)
        {
            issave = true;
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            issave = false;
            this.Close();
        }

        private void username_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonX1_Click(sender, e);
            }
        }

        private void UserInput_Load(object sender, EventArgs e)
        {

        }


    }
}
