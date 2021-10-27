using System;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using Model;
using Model.发布相关模型;
using Model.抓取相关模型;
using xEngine.Common;

namespace V3.V3Form.发布模块
{
    public partial class postPlan : DevExpress.XtraEditors.XtraForm
    {
        public postPlan()
        {
            InitializeComponent();
        }
        public Model.GetPostModel Model = new Model.GetPostModel();
        public V3.Bll.PostBll Bll;
        public bool Issave = false;
        public bool IsMymodel = false;
        public void ShowI(string txt)
        {
            this.Invoke((EventHandler)(delegate
            {
                ipanel.Visible = true;
                istate.Text = txt;
            }));

        }
        public void CloseI()
        {
            this.Invoke((EventHandler)(delegate
            {
                ipanel.Visible = false;
            }));
        }
        void SetInfo()
        {
            Model.isPostModel = true;
            Model.PlanModel = 5;
            if (Model.IsShareModel)
                Model.PlanModel = 50;
            else
                Model.PlanModel = 5;
            Model.UpdateTime = DateTime.Now.ToString();
            Model.Testadminurl = txtAdminUrl.Text;
            Model.POST_TestAccount.username = "请填写自己后台用户名(临时使用,不会保存)";
            Model.POST_TestAccount.password = "请填写自己密码(临时使用，不会保存)";
        }
        void GetInfo()
        {
          
            if (Model.Stp1_POST_LoginAction == null)
            {
                Model.Stp1_POST_LoginAction = new GetPostAction[0];
                Model.Stp1_POST_NeedLogin = true;
                Model.Stp1_POST_Truetag = new string[0];
                Model.Stp1_POST_Falsetag = new string[0];
               
            }
            if (Model.Zidingyi_Stp3_GET_Rules == null)
            {
                Model.Zidingyi_Stp3_GET_Rules = new FinalRules[0];
            }
            if (Model.Stp3_POST_SendAction == null)
            {
                Model.Stp3_POST_SendAction = new GetPostAction[0];
                Model.Stp3_POST_Falsetag = new string[0];
                Model.Stp3_POST_Truetag = new string[0];
                Model.Stp3_POST_MakeHtmlUrls = new string[0];
            }
            
            labPlanDesign.Text = Model.PlanDesignName;
            labPlanName.Text = Model.PlanName;
            labPlanReadme.Text = Model.PlanReadme;
            labPlanUrl.Text = Model.PlanUrl;
            labUpdate.Text = Model.UpdateTime;
            txtAdminUrl.Text = Model.Testadminurl;
        }
      
      
      
        private void btn_step1_Click(object sender, EventArgs e)
        {
           
            SetInfo();
            PostStp1 frm = new PostStp1();
       
            frm.Model =this.Model;
            frm.Bll = this.Bll;
            frm.ShowDialog();
            if (frm.IsSave)
            {
                this.Model = frm.Model;
            }
           
        }
        private void btn_step2_Click(object sender, EventArgs e)
        {
           
            SetInfo();
            PostStp2 frm = new PostStp2();
          
            frm.Model =this.Model;
            frm.Bll = this.Bll;
            frm.ShowDialog();
            if (frm.Issave)
            {
                this.Model = frm.Model;
            }
   
        }
        private void btn_step3_Click(object sender, EventArgs e)
        {
          
            SetInfo();
            PostStp3 frm = new PostStp3();
            frm.Model = this.Model;
            frm.Bll = this.Bll;
            frm.ShowDialog();
            if (frm.IsSave)
            {
                this.Model = frm.Model;
            }
        }
        private void buttonItem2_Click(object sender, EventArgs e)
        {
          
        }
        private void getPlan_Load(object sender, EventArgs e)
        {
            GetInfo();
            Bll = new Bll.PostBll(Model, 0, 0);
           
           
        }
      
       
      
        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetInfo();
            if (Model.PlanName.Length == 0)
            {
                Issave = false;
                XtraMessageBox.Show("请在<color=red>模块基本信息</color>中设置模块名称！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information, DefaultBoolean.True);
                btn_modify_ItemClick(null, null);
                return;
            }
            if (global::Model.V3Infos.MainDb.MyModels.ContainsKey(Model.mids))
            {
                global::Model.V3Infos.MainDb.MyModels[Model.mids] = Model;

            }
            else
            {
                Model.mids = Guid.NewGuid().ToString();
                global::Model.V3Infos.MainDb.MyModels.Add(Model.mids, Model);

            }
            V3.Common.V3Helper.saveDBS();
            Issave = true;
            this.Close();
           
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Issave = false;
            this.Close();
        }

        private void btn_modify_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            V3Form.抓取模块.PlanReadme frm = new V3Form.抓取模块.PlanReadme();
            frm.model = Model;
            frm.ShowDialog();
            if (frm.issave)
            {
                Model = frm.model;
                GetInfo();
            }
        }

        private void btn_output_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetInfo();
            string localFilePath;
            string result =Base32.ToBase32String(xEngine.Common.XSerializable.ObjectToBytes(Model));
            SaveFileDialog saveFileDialog1 = new SaveFileDialog(); //设置文件类型 
            saveFileDialog1.Title = "导出发布模块";
            saveFileDialog1.Filter = "发布模块(*.v3PostModel)|*.v3PostModel";//设置默认文件类型显示顺序
            saveFileDialog1.FilterIndex = 2; //保存对话框是否记忆上次打开的目录 
            saveFileDialog1.RestoreDirectory = true; //点了保存按钮进入 
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {  //获得文件路径  //
                localFilePath = saveFileDialog1.FileName.ToString();  //获取文件名，不带路径  //
                System.IO.File.WriteAllText(localFilePath, result.Trim(), Encoding.UTF8);
                XtraMessageBox.Show("模块已经成功导出到：" + localFilePath, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_input_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "导入发布模块";
            ofd.FileName = "";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);//为了获取特定的系统文件夹，可以使用System.Environment类的静态方法GetFolderPath()。该方法接受一个Environment.SpecialFolder枚举，其中可以定义要返回路径的哪个系统目录
            ofd.Filter = "发布模块(*.v3PostModel)|*.v3PostModel";
            ofd.ValidateNames = true;     //文件有效性验证ValidateNames，验证用户输入是否是一个有效的Windows文件名
            ofd.CheckFileExists = true;  //验证路径有效性
            ofd.CheckPathExists = true; //验证文件有效性
            ofd.FilterIndex = 2; //保存对话框是否记忆上次打开的目录 
            string result = null;
            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(ofd.FileName, System.Text.Encoding.Default);
                    result = sr.ReadToEnd();
                    sr.Close();
                 
                    Model = xEngine.Common.XSerializable.CloneObject<GetPostModel>(xEngine.Common.XSerializable.BytesToObject<GetPostModel>(Base32.FromBase32String(result)));
                    Model.mids = Guid.NewGuid().ToString();
                    GetInfo();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString());
            }
        }
      
    }
}
