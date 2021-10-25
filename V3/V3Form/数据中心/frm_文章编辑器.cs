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
    public partial class frm_文章编辑器 : DevExpress.XtraEditors.XtraForm
    {
        public frm_文章编辑器()
        {
            InitializeComponent();
         
        }
        public Model.Model_Article art = new Model.Model_Article();
        public bool issave = false;



        private void timer1_Tick(object sender, EventArgs e)
        {
            if (art.DataObject.Count!=30)
            {
                art.DataObject = new Dictionary<int, string>()
            {
            { 0,""},
            { 1,""},
            { 2,""},
            { 3,""},
            { 4,""},
            { 5,""},
            { 6,""},
            { 7,""},
            { 8,""},
            { 9,""},
            { 10,""},
            { 11,""},
            { 12,""},
            { 13,""},
            { 14,""},
            { 15,""},
            { 16,""},
            { 17,""},
            { 18,""},
            { 19,""},
            { 20,""},
            { 21,""},
            { 22,""},
            { 23,""},
            { 24,""},
            { 25,""},
            { 26,""},
            { 27,""},
            { 28,""},
            { 29,""}
            };

            }Object1.Text = art.DataObject[0];
            Object2.BodyHtml = art.DataObject[1];
            Object3.Text = art.DataObject[2];
            Object4.Text = art.DataObject[3];
            Object5.Text = art.DataObject[4];
            Object6.Text = art.DataObject[5];
            Object7.Text = art.DataObject[6];
            Object8.Text = art.DataObject[7];
            Object9.Text = art.DataObject[8];
            Object10.Text = art.DataObject[9];
            Object11.Text = art.DataObject[10];
            Object12.Text = art.DataObject[11];
            Object13.Text = art.DataObject[12];
            Object14.Text = art.DataObject[13];
            Object15.Text = art.DataObject[14];
            Object16.Text = art.DataObject[15];
            Object17.Text = art.DataObject[16];
            Object18.Text = art.DataObject[17];
            Object19.Text = art.DataObject[18];
            Object20.Text = art.DataObject[19];
            Object21.Text = art.DataObject[20];
            Object22.Text = art.DataObject[21];
            Object23.Text = art.DataObject[22];
            Object24.Text = art.DataObject[23];
            Object25.Text = art.DataObject[24];
            Object26.Text = art.DataObject[25];
            Object27.Text = art.DataObject[26];
            Object28.Text = art.DataObject[27];
            Object29.Text = art.DataObject[28];
            Object30.Text = art.DataObject[29];
            Object1.Focus();
            timer1.Enabled = false;
        }

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Object1.Text == "" || Object2.BodyHtml == "")
            {
                XtraMessageBox.Show("标题和正文不能为空哦。。。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            issave = true;
            art.DataObject[0] = Object1.Text;
            art.DataObject[1] = Object2.BodyHtml;
            art.DataObject[2] = Object3.Text;
            art.DataObject[3] = Object4.Text;
            art.DataObject[4] = Object5.Text;
            art.DataObject[5] = Object6.Text;
            art.DataObject[6] = Object7.Text;
            art.DataObject[7] = Object8.Text;
            art.DataObject[8] = Object9.Text;
            art.DataObject[9] = Object10.Text;
            art.DataObject[10] = Object11.Text;
            art.DataObject[11] = Object12.Text;
            art.DataObject[12] = Object13.Text;
            art.DataObject[13] = Object14.Text;
            art.DataObject[14] = Object15.Text;
            art.DataObject[15] = Object16.Text;
            art.DataObject[16] = Object17.Text;
            art.DataObject[17] = Object18.Text;
            art.DataObject[18] = Object19.Text;
            art.DataObject[19] = Object20.Text;
            art.DataObject[20] = Object21.Text;
            art.DataObject[21] = Object22.Text;
            art.DataObject[22] = Object23.Text;
            art.DataObject[23] = Object24.Text;
            art.DataObject[24] = Object25.Text;
            art.DataObject[25] = Object26.Text;
            art.DataObject[26] = Object27.Text;
            art.DataObject[27] = Object28.Text;
            art.DataObject[28] = Object29.Text;
            art.DataObject[29] = Object30.Text;
            this.Close();
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            issave = false;
            this.Close();
        }

        private void frm_文章编辑器_Load(object sender, EventArgs e)
        {
           
        }
    }
}
