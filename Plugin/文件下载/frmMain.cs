using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 文件下载
{
    public partial class frmMain : UserControl
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button_selectpath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f=new FolderBrowserDialog();
            if (f.ShowDialog()==DialogResult.OK)
            {
                textBox_path.Text = f.SelectedPath;
            }
        }

        private void linkLabel_ymd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int selectstart = textBox_DirName.SelectionStart;
            textBox_DirName.Focus();
            textBox_DirName.SelectedText = "[年月日]";
            textBox_DirName.Select(selectstart + 6, 0);
        }

        private void button_test_Click(object sender, EventArgs e)
        {
            button_test.Text = "测试中...";
            button_test.Enabled = false;
            Thread thread = new Thread(delegate()
            {
                try
                {
                    FTPHelper ftp = new FTPHelper(textBox_ftpServer.Text, textBox_ftpPath.Text, textBox_ftpUser.Text,
                        textBox_ftpPassword.Text);
                    ftp.GetDirectoryList();
                    this.Invoke(new MethodInvoker(delegate
                    {
                        MessageBox.Show("成功连接服务器！");
                    }));
                }
                catch (Exception error)
                {
                    MessageBox.Show("无法连接服务器，原因：" + error.Message);

                }
                this.Invoke(new MethodInvoker(delegate
                {
                    button_test.Enabled = true;
                    button_test.Text = "测试连接";
                }));

            });
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
