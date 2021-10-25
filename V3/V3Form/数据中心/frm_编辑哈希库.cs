using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using V3.Common;
namespace V3.V3Form
{
    public partial class frm_编辑哈希库 : DevExpress.XtraEditors.XtraForm
    {

        public frm_编辑哈希库()
        {
            InitializeComponent();

        }
        public string Dbid = "";

        void Readnumber()
        {
            int file1 = 0;
            try
            {
                file1 = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Dbid + "_T\\").Length;
            }
            catch { }
            { }
            textBoxX1.Text = file1.ToString();

            int file2 = 0;
            try
            {
                file2 = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Dbid + "_C\\").Length;
            }
            catch { }
            textBoxX2.Text = file2.ToString();


            int file3 = 0;
            try
            {
                file3 = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Dbid + "_U\\").Length;
            }
            catch { }
            textBoxX3.Text = file3.ToString();
            Model.V3Infos.HashDb[Dbid].DataCount =Convert.ToInt32( file1+file2+file3);
            
        }
        void Cleardb1()
        {

            try
            {
                System.IO.Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Dbid + "_T\\",true);
            }
            catch (Exception Exception)
            {
               // XtraMessageBox.Show( "清空失败,原因:" + Exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        void Cleardb2()
        {
            try
            {
                System.IO.Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Dbid + "_C\\",true);
            }
            catch (Exception Exception)
            { 
                
              //  XtraMessageBox.Show( "清空失败,原因:" + Exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            
            }
        }
        void Cleardb3()
        {
            try
            {
                System.IO.Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" + Dbid + "_U\\",true);
            }
            catch (Exception Exception) 
            { 
                
               // XtraMessageBox.Show( "清空失败,原因:" + Exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("确定要清空这个哈希库的标题哈希值吗？\r\n\r\n清空后，所使用该库的任务将会重新抓取所有的文章标题（有可能会导致跟之前抓取的重复哦)", "确定清空？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Cleardb1();
                Readnumber();
            }
            this.Text = Model.V3Infos.HashDb[Dbid].Name + "(哈希值总数：" + Model.V3Infos.HashDb[Dbid].DataCount + " - 哈希库编辑";
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("确定要清空这个哈希库的正文哈希值吗？\r\n\r\n清空后，所使用该库的任务将会重新抓取所有的文章正文（有可能会导致跟之前抓取的重复哦)", "确定清空？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Cleardb2();
                Readnumber();
            }
            this.Text = Model.V3Infos.HashDb[Dbid].Name + "(哈希值总数：" + Model.V3Infos.HashDb[Dbid].DataCount + " - 哈希库编辑";
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("确定要清空这个哈希库的地址哈希值吗？\r\n\r\n清空后，所使用该库的任务将会重新抓取所有的文章地址（有可能会导致跟之前抓取的重复哦)", "确定清空？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Cleardb3();
                Readnumber();
            }
            this.Text = Model.V3Infos.HashDb[Dbid].Name + "(哈希值总数：" + Model.V3Infos.HashDb[Dbid].DataCount + " - 哈希库编辑";
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("确定要清空这个哈希库的所有哈希值吗？\r\n\r\n清空后，所使用该库的任务将会重新抓取所有的文章（有可能会导致跟之前抓取的重复哦)", "确定清空？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Cleardb1();
                Cleardb2();
                Cleardb3();
                Readnumber();
            }
            this.Text = Model.V3Infos.HashDb[Dbid].Name + "(哈希值总数：" + Model.V3Infos.HashDb[Dbid].DataCount + " - 哈希库编辑";
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_编辑哈希库_Load(object sender, EventArgs e)
        {
            Readnumber();
        }
    }
}
