using System;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Layout;
using Microsoft.Win32;
using Model;
using V3.Common;

namespace V3.V3Form
{
    public partial class frmSystem : XtraForm
    {
        public frmSystem()
        {
            InitializeComponent();
        }
        public bool Issave = false;
        void SetInfo()
        {
            V3Infos.MainDb.PageNumber = Convert.ToInt32(txtPageNumber.Value);
            V3Infos.MainDb.GetTimeOut = Int32.Parse(txtGettimeout.Text);
            V3Infos.MainDb.PostTimeOut = Int32.Parse(txtPosttimeout.Text);
            V3Infos.MainDb.PostOnGzip = toggleSwitch_iszip.IsOn;
            V3Infos.MainDb.Zuozhe = txtZuozhe.Text;
            V3Infos.MainDb.Laiyuan = txtLaiyuan.Text;
            V3Infos.MainDb.Jieduanbiaoti =Convert.ToInt32( jieduanbiaoti.Value);
            V3Infos.MainDb.Jieduanzhengwen = Convert.ToInt32( jieduanzhengwen.Value);

            V3Infos.MainDb.Startjiange = Convert.ToInt32(StartJianGe.Value);
            V3Infos.MainDb.AutoRunTask = toggleSwitch_AutoRunTask.IsOn;
            V3Infos.MainDb.AutoStart = toggleSwitch_AutoStart.IsOn;
        }
        void GetInfo()
        {
            if (V3Infos.MainDb == null)
                V3Infos.MainDb = new Model_V3Setting();
            txtPageNumber.Text = V3Infos.MainDb.PageNumber.ToString();
          
            txtGettimeout.Text = V3Infos.MainDb.GetTimeOut.ToString();
            txtPosttimeout.Text = V3Infos.MainDb.PostTimeOut.ToString();
            toggleSwitch_iszip.IsOn = V3Infos.MainDb.PostOnGzip;
            txtLaiyuan.Text = V3Infos.MainDb.Laiyuan;
            txtZuozhe.Text = V3Infos.MainDb.Zuozhe;
            jieduanbiaoti.Value = V3Infos.MainDb.Jieduanbiaoti;
            jieduanzhengwen.Value = V3Infos.MainDb.Jieduanzhengwen;
            StartJianGe.Value = V3Infos.MainDb.Startjiange;
           toggleSwitch_AutoRunTask.IsOn= V3Infos.MainDb.AutoRunTask;
            toggleSwitch_AutoStart.IsOn= V3Infos.MainDb.AutoStart;

        }

        private void Setting_Load(object sender, EventArgs e)
        {
           
            GetInfo();
        }

        private void btn_save_ItemClick(object sender, ItemClickEventArgs e)
        {
            SetInfo();
            V3Helper.saveDBS();
            this.Close();
        }

        private void btn_cancel_ItemClick(object sender, ItemClickEventArgs e)
        {

            this.Close();
        }

        private void toggleSwitch_iszip_Toggled(object sender, EventArgs e)
        {
         
        }

        private void toggleSwitch_AutoStart_Toggled(object sender, EventArgs e)
        {
            if (toggleSwitch_AutoStart.IsOn)
            {

                SetAutoRun(Application.ExecutablePath,true);
            }
            else
            {
                SetAutoRun(Application.ExecutablePath, false);
            }
        }
        public static void SetAutoRun(string fileName, bool isAutoRun)
        {
            RegistryKey reg = null;
            try
            {

                string args = "";
                foreach (var a in Program.Args)
                {
                    args += a+" ";
                }
                String name = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (reg == null)
                    reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (isAutoRun)
                    reg.SetValue(name, fileName+" "+args);
                else
                    reg.SetValue(name, false);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("无法设置启动项，如果有安全软件拦截请允许！\r\n\r\n"+ex.Message, "设置失败", MessageBoxButtons.OK);
              
            }
            finally
            {
                if (reg != null)
                    reg.Close();
            }

        }

        private void groupControl_proxy_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}
