using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace V3.V3Form.工具窗体
{
    public partial class 框 :  DevExpress.XtraEditors.XtraForm
    {
        public 框()
        {
            InitializeComponent();
        }

        public string backstring = "关闭";
        public  string msg = "";
        private void 框_Load(object sender, EventArgs e)
        {
            labelX1.Text = msg;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            backstring = "升级";
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            backstring = "忽略";
            this.Close();

        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            backstring = "关闭";
            this.Close();
        }
    }
}
