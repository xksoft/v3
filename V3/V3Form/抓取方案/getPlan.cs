using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using Model;
using Model.抓取相关模型;
using xEngine.Common;

namespace V3.V3Form.抓取模块
{
    public partial class getPlan : DevExpress.XtraEditors.XtraForm
    {
        public getPlan()
        {
            InitializeComponent();
        }

        public bool IsShow = false;
        public void showI(string txt)
        {
            this.Invoke((EventHandler)(delegate
            {
                ipanel.Visible = true;
                istate.Text = txt;
            }));

        }
        public void closeI()
        {
            if (IsShow)
            {
                this.Invoke((EventHandler)(delegate
                {
                    ipanel.Visible = false;
                }));
            } 
        }
   
        public Model.GetPostModel model = new Model.GetPostModel();
        public bool isMymodel = false;
        public bool issave = false;
        void SetInfo()
        {
            model.isPostModel = false;
            if (s1.Checked)
                if (model.IsShareModel)
                    model.PlanModel = 10;
                else
                    model.PlanModel = 1;
            else if (s2.Checked)
                if (model.IsShareModel)
                    model.PlanModel = 20;
                else
                    model.PlanModel = 2;
            else if (s3.Checked)
                if (model.IsShareModel)
                    model.PlanModel = 30;
                else
                    model.PlanModel = 3;
            else if (s4.Checked)
                if (model.IsShareModel)
                    model.PlanModel = 40;
                else
                    model.PlanModel = 4;
            model.UpdateTime = DateTime.Now.ToString();
            model.GetMajia = txtmajia.Text;
            model.UserAgent = txtuseragent.Text;
        }
        void GetInfo()
        {
            if(IsShow){
            labPlanDesign.Text = model.PlanDesignName;
            labPlanName.Text = model.PlanName;
            labPlanReadme.Text = model.PlanReadme;
            labPlanUrl.Text = model.PlanUrl;
            labUpdate.Text = model.UpdateTime;
            txtmajia.Text = model.GetMajia;
            txtuseragent.Text = model.UserAgent;
            if (model.PlanModel == 1 || model.PlanModel == 10)
                s1.Checked = true;
            else if (model.PlanModel == 2 || model.PlanModel == 20)
                s2.Checked = true;
            else if (model.PlanModel == 3 || model.PlanModel == 30)
                s3.Checked = true;
            else if (model.PlanModel == 4 || model.PlanModel == 40)
                s4.Checked = true;
           
            }
        }
        //更新模块的方法
        void ModifyModelBase(object mbb)
        {
            showI("正在提交到服务器...");
            Model.ModelBase mb = (Model.ModelBase)mbb;
            string result = V3.Common.ModelShopBll.ModifyModelBase(mb);
            if (result != "OK")
            {
                issave = false;
                closeI();
                try
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        XtraMessageBox.Show("更新模块失败，原因：" + result, "更新失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                }
                catch { }
            }
            else
            {
                issave = true;
                closeI();
               
                Model.V3Infos.MainDb.MyModels[mb.id] = (Model.GetPostModel)xEngine.Common.XSerializable.BytesToObject<Model.GetPostModel>(Base32.FromBase32String(mb.Data));
                this.Invoke((EventHandler)(delegate
                {
                    this.Close();
                }));
            }


        }
        private void ModifeModel(Model.GetPostModel model)
        {
            Model.ModelBase mbmodel = V3.Common.ModelShopBll.GetModelbase(model);
            mbmodel.id = model.mids;
            this.Activate();
            System.Threading.Thread t = new System.Threading.Thread(ModifyModelBase);
            t.IsBackground = true;
            t.Start(mbmodel);
        }
        //上传方法
        private void AddModelBase(object object1)
        {
            showI("正在提交到服务器，请稍后...");
            Model.GetPostModel tempmodel = (Model.GetPostModel)object1;
            tempmodel.uid = Convert.ToInt32(99);
            if (V3.Common.ModelShopBll.UploadModel(tempmodel) == "OK")
            {
                closeI();
                issave = true;

                this.Invoke((EventHandler)(delegate
                {
                    XtraMessageBox.Show("模块成功上传到服务器，在模块市场中点击“同步模块市场信息”即可在<color=red>我的模块</color>中查看并使用它！", "上传成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information, DefaultBoolean.True);
                    this.Close();
                }));
            }
            else
            {
                closeI();
                this.Invoke((EventHandler)(delegate
                {
                    XtraMessageBox.Show("模块上传失败！\r\n\r\n1：检查自己的网络环境是否通畅并重新提交\r\n2：不要关闭该窗口，联系在线客服帮您解决", "上传失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
                issave = false;
            }
        }

        private void btn_step1_Click(object sender, EventArgs e)
        {
          
            if (s1.Checked)//关键字智能抓取流程一
            {
                Stp1_keywordmodel frm = new Stp1_keywordmodel();
                frm.Model =xEngine.Common.XSerializable.CloneObject<GetPostModel>(model) ;
                frm.ShowDialog();
                if (frm.Issave)
                {
                    model = xEngine.Common.XSerializable.CloneObject<GetPostModel>(frm.Model);
                    GetInfo();
                }
            }
            else if (s2.Checked)//自定义抓取
            {

                Stp1_zidingyimodel frm = new Stp1_zidingyimodel();
                frm.Model =xEngine.Common.XSerializable.CloneObject<GetPostModel>( model);
                frm.ShowDialog();
                if (frm.IsSave)
                {
                    model =xEngine.Common.XSerializable.CloneObject<GetPostModel>( frm.Model);
                    GetInfo();
                }
            }
            else if (s3.Checked)//自定义抓取
            {

                stp1_spidergetmodel frm = new stp1_spidergetmodel();
                frm.Model = xEngine.Common.XSerializable.CloneObject<GetPostModel>(model);
                frm.ShowDialog();
                if (frm.Issave)
                {
                    model = xEngine.Common.XSerializable.CloneObject<GetPostModel>(frm.Model);
                    GetInfo();
                }
            }
            else if (s4.Checked)//自定义抓取
            {

                Stp1_tongbumodel frm = new Stp1_tongbumodel();
                frm.Model = xEngine.Common.XSerializable.CloneObject<GetPostModel>(model);
                frm.ShowDialog();
                if (frm.Issave)
                {
                    model = xEngine.Common.XSerializable.CloneObject<GetPostModel>(frm.Model);
                    GetInfo();
                }
            }
           

        }

        private void btn_step2_Click(object sender, EventArgs e)
        {
         
            if (s1.Checked)
            {
                Stp2_keywordmodel frm = new Stp2_keywordmodel();
                frm.Model = xEngine.Common.XSerializable.CloneObject<GetPostModel>(model);
                frm.ShowDialog();
                if (frm.IsSave)
                {
                    model = xEngine.Common.XSerializable.CloneObject<GetPostModel>(frm.Model);
                }
            }
            else if (s2.Checked)
            {
                Stp2_zidingyidmodel frm = new Stp2_zidingyidmodel();
                frm.Model = model;
                frm.ShowDialog();
                if (frm.IsSave)
                {
                    model = xEngine.Common.XSerializable.CloneObject<GetPostModel>(frm.Model);
                }
            }
          
        }

        private void btn_step3_Click(object sender, EventArgs e)
        {
           
            Stp3_Allmodel frm = new Stp3_Allmodel();
            frm.Model = xEngine.Common.XSerializable.CloneObject<GetPostModel>(model);
            frm.ShowDialog();
            if (frm.IsSave)
            {
                model = xEngine.Common.XSerializable.CloneObject<GetPostModel>(frm.Model);
            }
          
        }


        private void getPlan_Load(object sender, EventArgs e)
        {
            IsShow = true;
            if (model.mids == "")
            {
                model.Stp3_GET_Rules = new Model.抓取相关模型.FinalRules[30];
                for (int i = 0; i < 30; i++)
                {
                    model.Stp3_GET_Rules[i] = new FinalRules();
                    model.Stp3_GET_Rules[i].Rules.GetModel = 1;
                }
                model.Zidingyi_Stp3_GET_Rules = new FinalRules[30];
                for (int i = 0; i < 30; i++)
                {
                    model.Zidingyi_Stp3_GET_Rules[i] = new FinalRules();
                    model.Zidingyi_Stp3_GET_Rules[i].Rules.GetModel = 1;
                }

            }
            GetInfo();
        }


        private void s2_CheckedChanged(object sender, EventArgs e)//自定义抓取选择
        {
            /// 模块模式 1关键词智能提取模式 2自定义抓取模式 3蜘蛛爬行模式 4同步追踪模式
            if (s2.Checked == true)//选择了自定义抓取
            {
                model.PlanModel = 2;
                SetInfo();
                btn_step1.Text = "流程1：获取列表链接";
                btn_step2.Text = "流程2：获取内容链接";
                btn_step2.Enabled = true;
                labelX6.Text = "本模式可以根据一个或者多个入口链接加上页面规则对指定站点进行抓取";
            }
        }

        private void s1_CheckedChanged(object sender, EventArgs e)
        {
            if (s1.Checked == true)//选择了自定义抓取
            {
                model.PlanModel = 1;
                SetInfo();
                btn_step1.Text = "流程1：根链接取得参数";
                btn_step2.Text = "流程2：内容链接取得参数";
                btn_step2.Enabled = true;
                labelX6.Text = "本模式下将可以自动根据关键词来抓取内容相符的文章";

            }
        }

        private void s4_CheckedChanged(object sender, EventArgs e)
        {
            if (s4.Checked == true)//选择了同步追踪
            {
                model.PlanModel = 4;
                SetInfo();
                btn_step1.Text = "流程1：更新列表页面设置";
                btn_step2.Enabled = false;
                btn_step3.Text = "流程3：内容提取参数";
                labelX6.Text = "本模式可以根据一个或者多个链接定时访问并且抓取新内容发布到自己的站点上";
            }
        }

        private void s3_CheckedChanged(object sender, EventArgs e)
        {
            if (s3.Checked == true)
            {
                model.PlanModel = 3;
                SetInfo();
                btn_step1.Text = "流程1：设置蜘蛛参数";
                btn_step2.Enabled = false;
                btn_step3.Text = "流程3：内容提取参数";
                labelX6.Text = "本模式可以根据一个或者多个入口加上页面规则直接爬行抓取文章";
            }
        }



        private void btnGetMajia_Click(object sender, EventArgs e)
        {

            //showI("正在提取马甲...");
            //System.Threading.Thread t = new System.Threading.Thread(GetMajia);
            //t.IsBackground = true;
            //t.Start();
            frmGetCookie f = new frmGetCookie();
            f.ShowDialog();
            if (f.GetSuccess)
            {
                txtmajia.Text = f.CookieStr ;
               
                SetInfo();

            }

        }

   
       
     
        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetInfo();
            if (model.PlanName.Length == 0)
            {
                issave = false;
                XtraMessageBox.Show("请在<color=red>模块基本信息</color>中设置模块名称！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information, DefaultBoolean.True);
                btn_info_ItemClick(null, null);
                return;
            }
            if (Model.V3Infos.MainDb.MyModels.ContainsKey(model.mids))
            {
                Model.V3Infos.MainDb.MyModels[model.mids] = model;
               
            }
            else
            {
                model.mids = Guid.NewGuid().ToString();
                Model.V3Infos.MainDb.MyModels.Add(model.mids,model);
               
            }
            V3.Common.V3Helper.saveDBS();
            issave = true;
                this.Close();
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            issave = false;
            this.Close();
        }

        private void btn_info_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PlanReadme frm = new PlanReadme();
            frm.model = model;
            frm.ShowDialog();
            if (frm.issave)
            {
                model = frm.model;
                GetInfo();
            }
        }

        private void btn_output_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetInfo();
            string localFilePath;
            string result = Base32.ToBase32String(xEngine.Common.XSerializable.ObjectToBytes(model));
            SaveFileDialog saveFileDialog1 = new SaveFileDialog(); //设置文件类型 
            saveFileDialog1.Title = "导出抓取模块";
            saveFileDialog1.Filter = "抓取模块(*.v3GetModel)|*.v3GetModel";//设置默认文件类型显示顺序
            saveFileDialog1.FilterIndex = 2; //保存对话框是否记忆上次打开的目录 
            saveFileDialog1.RestoreDirectory = true; //点了保存按钮进入 
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {  //获得文件路径  //
                localFilePath = saveFileDialog1.FileName.ToString();  //获取文件名，不带路径  //
                System.IO.File.WriteAllText(localFilePath, result.Trim(), Encoding.UTF8);
                XtraMessageBox.Show( "模块已成功导出到：" + localFilePath + "", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_input_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "导入V3抓取模块";
            ofd.FileName = "";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);//为了获取特定的系统文件夹，可以使用System.Environment类的静态方法GetFolderPath()。该方法接受一个Environment.SpecialFolder枚举，其中可以定义要返回路径的哪个系统目录
            ofd.Filter = "V3抓取文件(*.v3GetModel)|*.v3GetModel";
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
                  
                    model = xEngine.Common.XSerializable.CloneObject<GetPostModel>(xEngine.Common.XSerializable.BytesToObject<GetPostModel>(Base32.FromBase32String(result)));
                    model.mids = Guid.NewGuid().ToString();
                    GetInfo();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString());
            }
        }

        private void getPlan_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsShow = false;
        }
    }
}
