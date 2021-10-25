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
    public partial class frmTest : DevExpress.XtraEditors.XtraForm
    {
        public  int taskid;
        public frmTest()
        {
            InitializeComponent();
        }

        private void TestFrm_Load(object sender, EventArgs e)
        {

          
        }

        private void buttonX1_Click(object sender, EventArgs e) //开始处理
        {

            Model.Model_Article art = new Model.Model_Article();
            art.DataObject = new Dictionary<int, string>()
            {
                {0, ""},
                {1, ""},
                {2, ""},
                {3, ""},
                {4, ""},
                {5, ""},
                {6, ""},
                {7, ""},
                {8, ""},
                {9, ""},
                {10, ""},
                {11, ""},
                {12, ""},
                {13, ""},
                {14, ""},
                {15, ""},
                {16, ""},
                {17, ""},
                {18, ""},
                {19, ""},
                {20, ""},
                {21, ""},
                {22, ""},
                {23, ""},
                {24, ""},
                {25, ""},
                {26, ""},
                {27, ""},
                {28, ""},
                {29, ""}
            };

        art.DataObject[0]=textBoxX1.Text;
            art.DataObject[1] = htmlEditor1.BodyHtml;

            try
            {
                MarkNewArticle(ref art, taskid);
            }
            catch (Exception error)
            {
                XtraMessageBox.Show(error.Message, "处理错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            inhtml.BodyHtml = art.DataObject[1];
             textBoxX2.Text = art.DataObject[0];
           
        }   
        static string MarkNewArticle(ref Model.Model_Article art, int taskid)
        {
          
            Common.SetArticle s = new Common.SetArticle();
            s.Task =xEngine.Common.XSerializable.CloneObject<Model.Task>(Model.V3Infos.TaskDb[taskid]);
            s.art = art;
            s.chuli();
            return s.minganguolv();
        }
    }
}
