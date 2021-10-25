using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace V3.V3Form
{
    public partial class frm_参数输入编辑器 : DevExpress.XtraEditors.XtraForm
    {
        public frm_参数输入编辑器()
        {
            InitializeComponent();
        }
        public bool issave = false;
        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            issave = true;
            this.Close();
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            issave = false;
            this.Close();
        }

        private void txtMainbox_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
