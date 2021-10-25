using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
namespace V3.V3Form
{
    public partial class frmResponse :  DevExpress.XtraEditors.XtraForm
    {
        public string Html = "";
        public frmResponse()
        {
            InitializeComponent();
        }

        private void frmResponse_Load(object sender, EventArgs e)
        {
            richTextBox_Html.Text = Html;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
