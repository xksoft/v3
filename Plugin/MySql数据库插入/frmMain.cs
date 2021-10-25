using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace MySql数据库插入
{
    public partial class frmMain : UserControl
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void linkLabel_model_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int selectstart = textBox_Sql.SelectionStart;
            textBox_Sql.Focus();
            textBox_Sql.SelectedText = "[模型值1]";
            textBox_Sql.Select(selectstart + 6, 0);
        }

        private void button_test_Click(object sender, EventArgs e)
        {
            button_test.Text = "测试中...";
            button_test.Enabled = false;
            Thread thread = new Thread(delegate()
            {
                try
                {
                    MySqlHelper.ConnectionStringManager = textBox_ConnStr.Text;
                    MySqlHelper.ExecuteNonQuery(
                        "select table_name from information_schema.tables where table_schema='csdb' and table_type='base table'");
                    this.Invoke(new MethodInvoker(delegate
                    {
                        MessageBox.Show("成功连接数据库！");
                    }));


                }
                catch (Exception error)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        MessageBox.Show("数据库连接失败：" + error.Message);
                    }));
                }
                this.Invoke(new MethodInvoker(delegate
                {
                    button_test.Text = "测试连接";
                    button_test.Enabled = true;
                }));

            });
            thread.IsBackground = false;
            thread.Start();



        }
    }
}
