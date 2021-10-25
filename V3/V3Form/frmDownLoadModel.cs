using System;
namespace V3.V3Form
{
    public partial class frmDownLoadModel :  DevExpress.XtraEditors.XtraForm
    {
        public frmDownLoadModel()
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

        private void frmDownLoadModel_Load(object sender, EventArgs e)
        {
           
        }

    }
}
