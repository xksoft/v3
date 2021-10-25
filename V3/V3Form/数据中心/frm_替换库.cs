using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.IO;
using DevExpress.XtraEditors;

namespace V3.V3Form
{
    public partial class frm_替换库 : DevExpress.XtraEditors.XtraForm
    {
        public frm_替换库()
        {
            InitializeComponent();
        }
        public bool isSave = false;
        public string dbid = "";
        public List<string> word = new List<string>();

        private void clear()
        {
            string[] temp = keyword.Lines;
           
            for (int i = 0; i < temp.Length; i++)
            {
                if (!word.Contains(temp[i]) && temp[i].Length>0&&temp[i].Contains("→"))
                {
                    word.Add(temp[i]);
                }
            }
          
           
            try
            {
            this.Invoke((EventHandler)(delegate
            {
                if (isSave)
                    this.Close();
            }));
            }
            catch { }

        }

        private void frm_关键词库_Shown(object sender, EventArgs e)
        {
            string result = string.Empty;
            for (int i = 0; i < Model.V3Infos.ReplaceDb[dbid].Words.Count; i++)
            {
                result += Model.V3Infos.ReplaceDb[dbid].Words[i]+"\r\n";
            }
            keyword.Text = result;
            txtStatus.Caption = "共有替换词"+Model.V3Infos.ReplaceDb[dbid].Words.Count+"组";
        }

        private void keyword_TextChanged(object sender, EventArgs e)
        {
            txtStatus.Caption = "共有替换词" + (keyword.Lines.Length - 1) + "组。 说明：词组间使用→作为间隔，支持多组，替换是支持正则表达式的，如果需要使用正则请用[]包括起来即可，详细请参见教程！";
        }


        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            isSave = true;
            Thread s = new Thread(new ThreadStart(clear));
            s.IsBackground = true;
            s.Start();
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            isSave = false;
            this.Close();
        }

        private void btn_import_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "导入站群引擎V3替换库文件";
            ofd.FileName = "";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);//为了获取特定的系统文件夹，可以使用System.Environment类的静态方法GetFolderPath()。该方法接受一个Environment.SpecialFolder枚举，其中可以定义要返回路径的哪个系统目录
            ofd.Filter = "站群引擎V3替换库文件(*.txt)|*.txt";
            ofd.ValidateNames = true;     //文件有效性验证ValidateNames，验证用户输入是否是一个有效的Windows文件名
            ofd.CheckFileExists = true;  //验证路径有效性
            ofd.CheckPathExists = true; //验证文件有效性
            ofd.FilterIndex = 2; //保存对话框是否记忆上次打开的目录 
            string result = null;
            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    StreamReader sr = new StreamReader(ofd.FileName, System.Text.Encoding.Default);
                    result = sr.ReadToEnd();
                    sr.Close();
                    keyword.Text = result;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString());
            }
        }

        private void btn_output_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string localFilePath;
            string result = keyword.Text;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog(); //设置文件类型 
            saveFileDialog1.Title = "导出站群引擎V3替换库文件";
            saveFileDialog1.Filter = "站群引擎V3替换库文件(*.txt)|*.txt";//设置默认文件类型显示顺序
            saveFileDialog1.FilterIndex = 2; //保存对话框是否记忆上次打开的目录 
            saveFileDialog1.RestoreDirectory = true; //点了保存按钮进入 
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {  //获得文件路径  //
                localFilePath = saveFileDialog1.FileName.ToString();  //获取文件名，不带路径  //
                File.WriteAllText(localFilePath, result.Trim(), Encoding.Default);

                XtraMessageBox.Show("数据已经成功导出到" + localFilePath + "！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
