using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using V3Plugin;

namespace V3.V3Form
{
    public partial class frmTask : DevExpress.XtraEditors.XtraForm
    {
        public static frmTask MyfrmTask;
        public frmTask()
        {
            InitializeComponent();
            MyfrmTask = this;
           
        }


        public Dictionary<string, ProcessPlugin> ProcessPluginList = new Dictionary<string, ProcessPlugin>();
        public bool isShow = false;
        public bool issave = false;
        public Model.Task model = new Model.Task();
        public int pointid = 0;

        void SetInfo()
        {
            if (txtTaskName.Text.Length==0)
            {
              
                XtraMessageBox.Show("请为任务起一个名字！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                issave = false;
                return;
            }
            foreach (var p in ProcessPluginList)
            {
                if (model.PluginParameters.ContainsKey(p.Key))
                {
                    model.PluginParameters[p.Key] = p.Value.Parameters;
                    p.Value.Parameters = null;
                }
            }
            model.Issuijilian = switchButtonisuijilian.IsOn;
            model.Suijimin =Convert.ToInt32( txtInt_suijimin.Value);
            model.Suijimax = Convert.ToInt32(txtInt_suijimax.Value);

            model.ClearHashDb = toggleSwitch_ClearHashDb.IsOn;
            model.Maoxiangtong = Convert.ToInt32(txtInt_maoxiangtong.Value);
            model.Getmajia = txtmajia.Text;
            model.Getmajiamodel = getmajiamodel.IsOn;
            model.Qianchuorhouchu = btn_SetType.IsOn;
            model.IsUseKu = switchButton5.IsOn;
            if (model.IsUseKu == true)
            {
                if (!System.IO.File.Exists(txtGetModelName.Text + "\\segments.gen"))
                {

                    XtraMessageBox.Show("语料库路径无效！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    issave = false;
                    return;
                }
                model.YuLiaoYingShen = Convert.ToInt32(txtInt_yuliaoyingshen.Value);
                model.YuLiaoMax1 = Convert.ToInt32(integerInput10.Value);
                model.YulLiaoMoShi = switchButtonyuliaomoshi.IsOn;
                model.YuJuShuLiang = Convert.ToInt32(txtInt_integerInputshuliang.Value);

                model.YuLiaoKuPath = txtGetModelName.Text;
            }
            model.Isjianfan = isjianfan.IsOn;
            model.Jianfanfangshi = janfanfangshi.IsOn;
            model.TaskMoshi = comboBoxtaskmoshi.SelectedIndex;
            model.FormatSuojin = formatsuojin.IsOn;
            model.FormateHtmlCode = toggleSwitch_FormateHtmlCode.IsOn;
            model.FormatPicURL = formatpic.IsOn;
            model.FormatLinkURL = formatlink.IsOn;
            model.FormatHanggao = formathanggao.IsOn;


            model.Maostring = textBoxX9.Text;
            model.Isshuangxiang = repxiang.IsOn;
            model.Lianjiefus.Clear();
            foreach (string s in textBoxlianjiefus.Lines)
            {
                if (!model.Lianjiefus.Contains(s) && s.Trim().Length > 0)
                {
                    model.Lianjiefus.Add(s);
                }
            }
            if (txtSendClass.Text.Trim().Length == 0 && IsRunSend.IsOn == true)
            {
                issave = false; XtraMessageBox.Show("请在发布参数中设置或选择要发布到的分类！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }
            model.IszidingLink = iszidingyilink.IsOn;
            if (iszidingyilink.IsOn == true && !Model.V3Infos.LinkDb.ContainsKey(model.ZidingLinkDb.ToString()))
            {
                textBoxX5.Text = "未设置或已删除！";
                XtraMessageBox.Show("请先选择一个自定义链接库！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                issave = false;
                return;
            }
            model.Zidingtou = iszidingyitou.Checked;
            model.Zidingzhong = iszidingyizhong.Checked;
            model.Zidingwei = iszidingyiwei.Checked;
            model.Zidingyitoushulian = Convert.ToInt32(integerInput6.Value);
            model.Zidingzhongshuliang = Convert.ToInt32(txtInt_integerInput8.Value);
            model.Zidingweishuliang = Convert.ToInt32(txtInt_integerInput5.Value);
            model.Zidingjiangemoshi = !jiangeorzhongshu.IsOn;
            model.Zidingzhongjiange = Convert.ToInt32(txtInt_jiange.Value);
            model.Zidingzhongstring = textBoxX3.Text;
            model.Zidingweistring = textBoxX1.Text;
            model.Zidingtoustring = textBoxX4.Text;
            model.Lunjiangemoshi = switchButton4.IsOn;
            model.Lunzongshu = Convert.ToInt32(txtInt_integerInput7.Value);
            if (switchButton2.IsOn == true && switchButton3.IsOn == false)//使用默认库 
            {
                if (Model.V3Infos.LinkDb.ContainsKey(model.LinkDbId.ToString()))
                {
                    txtLinkName.Text = "(" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Name;
                }
                else
                {
                    issave = false;
                    txtLinkName.Text = "未设置或已删除！";
                    XtraMessageBox.Show("链轮设置了使用任务默认链接库，请先选择一个默认链接库！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    issave = false;
                    return;
                }
            }
            if (switchButton2.IsOn == true && switchButton3.IsOn == true)//使用指定库 
            {
                if (Model.V3Infos.LinkDb.ContainsKey(model.OtherLinkDB.ToString()))
                {

                    textBoxX2.Text = "(" + Model.V3Infos.LinkDb[model.OtherLinkDB.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.OtherLinkDB.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.OtherLinkDB.ToString()].Name;
                }
                else
                {

                    textBoxX2.Text = "未设置或已删除！";
                    XtraMessageBox.Show("链轮设置了使用自定义链接库，请指定要使用的自定义链接库！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    issave = false;
                    return;
                }
            }
            model.IsLianLun = switchButton2.IsOn;
            model.UserDefaultLinkDB = switchButton3.IsOn;

            model.Isliantou = checkBoxX4.Checked;
            model.Toushuliang = Convert.ToInt32(txtInt_touzhuliang.Value+1);
            model.Toustring = textBoxXliantou.Text;

            model.Islianzhong = checkBoxX5.Checked;
            model.Zhongjiange = Convert.ToInt32(txtInt_zhongjian.Value);
            model.Zhongstring = textBoxXlianzhong.Text;

            model.Islianwei = checkBoxX6.Checked;
            model.Weishuliang = Convert.ToInt32(txtInt_weishuliang.Value);
            model.Weistring = textBoxXlianwei.Text;

            model.ArticleTou = textBoxXtou.Text;
            model.ArticleWei = textBoxXwei.Text;
            model.Tongbu_get1_jiangetime = Convert.ToInt32(integerInputtongbuzhuaqujiange.Value);
            model.Tongbu_ZhiXingJianGe = Convert.ToInt32(integerInputzhuizongjiange.Value);
            model.Zidingyi_Totalpage =Convert.ToInt32( txtInt_pagenumber.Value);
            model.Zidingyi_total_getnumber = Convert.ToInt32(integerInputzidingyimaxurl.Value);
            model.zidingyi_Get1_jiangetime = Convert.ToInt32(integerInputzidingyijiange.Value);
            model.Fabujiange = Convert.ToInt32(txtInt_fabujiange.Value);
            model.TaskName = txtTaskName.Text;
            model.IsAutoTask = IsAutoTask.IsOn;
            model.Jiangetime = Convert.ToInt32(txtInt_PiCiJianGe.Value);
            model.Isrunget = isRunGet.IsOn;

            model.MinTitlestr = Convert.ToInt32(txtInt_mintitlestr.Value);
            model.MinContentstr = Convert.ToInt32(txtInt_mincontentstr.Value);
            if (checkBoxX1.Checked)
                model.HashModel = 1;
            else if (checkBoxX2.Checked)
                model.HashModel = 2;
            else
                model.HashModel = 3;
            if (s1.Checked)
                model.DataGetmodel = 1;
            else if (s2.Checked)
                model.DataGetmodel = 2;
            else
                model.DataGetmodel = 3;
            model.Get1_jiangetime = Convert.ToInt32(txtInt_get1_jiangetime.Value);
            model.Get1_jingduModel = Convert.ToInt32(get1_jingdu.SelectedIndex);
            model.Get1_getnumber = Convert.ToInt32(txtInt_get1_MoreGetNumber.Value);
            model.IsMaoTiHuan = switchButton6.IsOn;
            model.Picinumber = Convert.ToInt32(txtInt_PiciNumber.Value);
            model.Isrunsend = IsRunSend.IsOn;
            model.MoreAccountUseModel = MoreAccountUseModel1.IsOn;
            model.Sendclass = txtSendClass.Text;
            model.IsUseMakeHtml = isUseMakeHtml.IsOn;
            model.IsUseLinkDb = isUseLinkDb.IsOn;
            model.Mingan = comboBoxmingan.SelectedItem.ToString();
            model.Iszhenggui = switchButtonhtml.IsOn;
            model.Istihuan = switchButtonistihuan.IsOn;
            model.Ishunxiao = switchButtonishunxiao.IsOn;
            try
            {
                model.Hunxiaofangshi = comboBoxhunxiaofangshi.SelectedItem.ToString();

            }
            catch
            {
                model.Hunxiaofangshi = "内容混淆关键字模式";

            }
            model.Hunxiaogeshi = textBoxhunxiaogeshi.Text;
            model.Guanjiancimoshi = switchButtonjiangemoshi.IsOn;
            if (switchButtonjiangemoshi.IsOn == true)
            {
                model.Keywordtotal = Convert.ToInt32(txtInt_Xkeywordtotal.Text.Trim());
                if(  model.Keywordtotal<=0){model.Keywordtotal=1;}
            }
            else
            {
                model.Keywordjiange = Convert.ToInt32(txtInt_Xkeyjiange.Text);
                if (model.Keywordtotal <= 0) { model.Keywordtotal = 1; }

            }
            model.NoQQ = switchButtonnoqq.IsOn;
            model.NoPic = switchButtonnoPic.IsOn;
            model.NoPhone = switchButtonnoPhone.IsOn;
            model.NoEmail = switchButtonnoemail.IsOn;
            model.NoA = switchButtonnoA.IsOn;

            model.MyQQ = textBoxXMyqq.Text.Trim();
            model.MyPhone = textBoxXMyPhone.Text.Trim();
            model.MyEmail = textBoxXMyemail.Text;
            model.MyA = textBoxXMya.Text.Trim();
            model.YuanChuangJianGe =Convert.ToInt32( spinEdit_YuanChuangJianGe.Value);
            model.WeiyuanchuangDu = comboBoxExWeiDu.SelectedItem.ToString();

            model.WeiyuanchuangXiang[0] = toggleSwitch_TitleYuanChuang.IsOn;
            model.WeiyuanchuangXiang[1] = switchButtonW2.IsOn;
            model.WeiyuanchuangXiang[2] = switchButtonW3.IsOn;
            model.WeiyuanchuangXiang[3] = switchButtonW4.IsOn;
            model.WeiyuanchuangXiang[4] = switchButtonW5.IsOn;

            model.WeiyuanchuangTitle = textBoxXTitle.Text;
            model.Fanyiformate = textBoxXFanYiLiuCheng.Text.Trim();
            model.NoUrl = switchButtonnourl.IsOn;
            model.MyUrl = textBoxXMyurl.Text.Trim();
            model.IstitleFanYi = switchButtontitleFanYi.IsOn;
            model.Spider_jiange = Convert.ToInt32(integerInput2.Value);
            model.Spider_maxget = Convert.ToInt32(integerInput3.Value);

            model.GetPiciNumber = Convert.ToInt32(txtInt_PiCiShu.Value);
            model.GetRunModel = switchButton1.IsOn;
            model.MaouseDefaultlinkDb = switchButton7.IsOn;
            if (model.IsMaoTiHuan == true)
            {
                if (model.MaouseDefaultlinkDb == true)//使用默认库
                {
                    if (!Model.V3Infos.LinkDb.ContainsKey(model.LinkDbId.ToString()))
                    {
                        issave = false;
                        XtraMessageBox.Show("混淆设置为锚文本但是任务没有链接库！请选择一个链接库！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;

                    }

                }
                else//使用自定义链接库 
                {
                    if (!Model.V3Infos.LinkDb.ContainsKey(model.MaolinkDbId.ToString()))
                    {
                        issave = false;
                        XtraMessageBox.Show( "混淆设置为锚文本但是没有选择自定义链接库！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;

                    }
                }


            }
            model.Maoshulian = Convert.ToInt32(txtInt_integerInput12.Value);
            if (Model.V3Infos.ArticleDb.ContainsKey(model.ArticleDbId.ToString()))
            {
                txtArticleName.Text = "(" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Id + "]" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Name;
            }


            else
            {

                issave = false;
                txtArticleName.Text = "未设置或已删除！";
                XtraMessageBox.Show("未设置文章库！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;

            }
            if (Model.V3Infos.KeywordDb.ContainsKey(model.KeywordDbId.ToString()))
            {
                txtKeywordName.Text = "(" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Keywords.Count + ")" + "[" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Id + "]" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Name;
            }
            else
            {
                issave = false;
                txtKeywordName.Text = "未设置或已删除！";
                XtraMessageBox.Show( "未设置关键词库！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Model.V3Infos.HashDb.ContainsKey(model.HashDbId.ToString()))
            {
                txtHashName.Text = "(" + Model.V3Infos.HashDb[model.HashDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Id + "]" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Name;
            }
            else
            {
                issave = false;
                txtHashName.Text = "未设置或已删除！";
                XtraMessageBox.Show("未设置哈希库！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Model.V3Infos.ReplaceDb.ContainsKey(model.ReplaceDbId.ToString()))
            {
                txtReplaceName.Text = "(" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Words.Count + ")" + "[" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Id + "]" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Name;
            }
            else
            {
                issave = false;
                txtReplaceName.Text = "未设置或已删除！";
                XtraMessageBox.Show("未设置替换库！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Model.V3Infos.LinkDb.ContainsKey(model.LinkDbId.ToString()))
            {
                txtLinkName.Text = "(" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Name;
            }
            else
            {
                issave = false;
                txtLinkName.Text = "未设置或已删除！";
                XtraMessageBox.Show("未设置链接库！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;

            }
            if (Model.V3Infos.LinkDb.ContainsKey(model.OtherLinkDB.ToString()))
            {

                textBoxX2.Text = "(" + Model.V3Infos.LinkDb[model.OtherLinkDB.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.OtherLinkDB.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.OtherLinkDB.ToString()].Name;
            }
            else if (switchButton3.IsOn == true && !Model.V3Infos.LinkDb.ContainsKey(model.OtherLinkDB.ToString()))
            {
                issave = false;
                textBoxX2.Text = "未设置或已删除！";
                XtraMessageBox.Show("链轮未设置链接库！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Model.V3Infos.MainDb.MyModels.ContainsKey(model.GetModel))
            {
                txtGetModelName.Text = "[" + Model.V3Infos.MainDb.MyModels[model.GetModel].mids + "]" + Model.V3Infos.MainDb.MyModels[model.GetModel].PlanName;
            }
            else
            {
                issave = false;
                txtGetModelName.Text = "未选择抓取模块！";
                XtraMessageBox.Show( "未设置抓取模块！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            #region 处理任务变量
            List<string> bianliang = new List<string>();
            string[] bl = txtrukou.Text.Trim().Replace("\r\n", "`").Split('`');
            for (int i = 0; i < bl.Length; i++)
            {
                if (bl[i].Length > 0)
                    bianliang.Add(bl[i]);
            }
            model.RukouUrl = bianliang;
            #endregion

            if (Model.V3Infos.MainDb.MyModels[model.GetModel].IsuseTaskRukou && model.RukouUrl.Count < 1)
            {
                XtraMessageBox.Show( "您选择的抓取模块需要在任务中指定入口地址，否则无法抓取！\r\n\r\n提示：入口在“入口/登录抓取”中设置", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                issave = false;
                return;
            }
            model.Canshu = dingzhicanshu.Text;
            issave = true;
        }
        void GetInfo()
        {
            ProcessPluginList.Clear();
            foreach (KeyValuePair<string, ProcessPlugin> kv in Program.ProcessPluginList)
            {
                ProcessPluginList.Add(kv.Key,(ProcessPlugin)kv.Value.Clone());
            }
            if(model.TaskName.Length==0){
                model.MinTitlestr = 5;
                model.MinContentstr = 10;
            model.TaskName = Model.V3Infos.SendPointDb[model.PointId].name + "的新建任务";
            }
            if(model.WeiyuanchuangXiang==null){model.WeiyuanchuangXiang=new bool[5];}
            foreach (var p in model.PluginParameters)
            {
                if (ProcessPluginList.ContainsKey(p.Key))
                {
                    ProcessPluginList[p.Key].Parameters = p.Value;
                }
            }
            ShowProcessPlugin();
            if (model.id==0&&model.TaskName.Length==0)
            {
                model.Isrunget = true;
                model.Isrunsend = true;
                model.WeiyuanchuangXiang = new bool[5];
            }
            toggleSwitch_ClearHashDb.IsOn = model.ClearHashDb;
            switchButtonisuijilian.IsOn = model.Issuijilian;
           txtInt_suijimin.Value = model.Suijimin  ;
           txtInt_suijimax.Value= model.Suijimax  ;

            textBoxlianjiefus.Text = "";
            txtInt_maoxiangtong.Value = model.Maoxiangtong;
            txtGetModelName.Text = "";
            txtmajia.Text = model.Getmajia;
            getmajiamodel.IsOn = model.Getmajiamodel;
            btn_SetType.IsOn = model.Qianchuorhouchu;
            isjianfan.IsOn = model.Isjianfan;
            janfanfangshi.IsOn = model.Jianfanfangshi;
            comboBoxtaskmoshi.SelectedIndex = model.TaskMoshi;
            formatsuojin.IsOn = model.FormatSuojin;
           toggleSwitch_FormateHtmlCode.IsOn=model.FormateHtmlCode ;
            formatpic.IsOn = model.FormatPicURL;
            formatlink.IsOn = model.FormatLinkURL;
            formathanggao.IsOn = model.FormatHanggao;
            textBoxX9.Text = model.Maostring;
            if (model.IsMaoTiHuan == true)
            {
                switchButton6.IsOn = true;
                if (model.MaouseDefaultlinkDb == false)//使用默认库
                {
                    switchButton7.IsOn = false;
                    if (Model.V3Infos.LinkDb.ContainsKey(model.MaolinkDbId.ToString()))
                    {

                        textBoxX10.Text = "(" + Model.V3Infos.LinkDb[model.MaolinkDbId.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.MaolinkDbId.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.MaolinkDbId.ToString()].Name;

                    }
                    else
                    {

                        textBoxX10.Text = "未设置或已删除！";
                    }

                }
                else {
                    switchButton7.IsOn = true;
                }



            }
            else { textBoxX10.Text = ""; }
            txtInt_integerInput12.Value = model.Maoshulian;



            repxiang.IsOn = model.Isshuangxiang;
            textBoxlianjiefus.Text = "";
            foreach (string s in model.Lianjiefus)
            {
                Common.V3Helper.GetInnerTextBox(textBoxlianjiefus).AppendText(s + "\r\n");
            }
            switchButton4.IsOn = model.Lunjiangemoshi;
            txtInt_integerInput7.Value = model.Lunzongshu;
            iszidingyilink.IsOn = model.IszidingLink;
            if (!Model.V3Infos.LinkDb.ContainsKey(model.ZidingLinkDb.ToString()))
            {
                textBoxX5.Text = "未设置或已删除！";

            }
            else { textBoxX5.Text = "(" + Model.V3Infos.LinkDb[model.ZidingLinkDb.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.ZidingLinkDb.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.ZidingLinkDb.ToString()].Name; }
            iszidingyitou.Checked = model.Zidingtou;
            iszidingyizhong.Checked = model.Zidingzhong;
            iszidingyiwei.Checked = model.Zidingwei;
            integerInput6.Value = model.Zidingyitoushulian;
            txtInt_integerInput8.Value = model.Zidingzhongshuliang;
            txtInt_integerInput5.Value = model.Zidingweishuliang;
            jiangeorzhongshu.IsOn = !model.Zidingjiangemoshi;
            txtInt_jiange.Value = model.Zidingzhongjiange;
            textBoxX3.Text = model.Zidingzhongstring;
            textBoxX1.Text = model.Zidingweistring;
            textBoxX4.Text = model.Zidingtoustring;


            switchButton2.IsOn = model.IsLianLun;
            switchButton3.IsOn = model.UserDefaultLinkDB;

            checkBoxX4.Checked = model.Isliantou;
            txtInt_touzhuliang.Value = model.Toushuliang-1;
            textBoxXliantou.Text = model.Toustring;

            checkBoxX5.Checked = model.Islianzhong;
            txtInt_zhongjian.Value = model.Zhongjiange;
            textBoxXlianzhong.Text = model.Zhongstring;

            checkBoxX6.Checked = model.Islianwei;
            txtInt_weishuliang.Value = model.Weishuliang;
            textBoxXlianwei.Text = model.Weistring;

            textBoxXtou.Text = model.ArticleTou;
            textBoxXwei.Text = model.ArticleWei;
            integerInputtongbuzhuaqujiange.Value = model.Tongbu_get1_jiangetime;
            integerInputzhuizongjiange.Value = model.Tongbu_ZhiXingJianGe;
            txtInt_pagenumber.Value = model.Zidingyi_Totalpage;
            integerInputzidingyimaxurl.Value = model.Zidingyi_total_getnumber;
            integerInputzidingyijiange.Value = model.zidingyi_Get1_jiangetime;
            txtInt_fabujiange.Value = model.Fabujiange;
            labelX91.Text = "提示：启用翻译后请在标题伪原创格式里面去掉【文章中的一句话】该标记。该功能即可用于多层伪原创，也可用于发布为其他语言内\n\n容，默认中文则代表不进行翻译！请按照正常逻辑设置流程~";
            txtTaskName.Text = model.TaskName;

            IsAutoTask.IsOn = model.IsAutoTask;
            txtInt_PiCiJianGe.Value = model.Jiangetime;
            isRunGet.IsOn = model.Isrunget;


            txtInt_mintitlestr.Value = model.MinTitlestr;
            txtInt_mincontentstr.Value = model.MinContentstr;
            if (model.HashModel == 1)
                checkBoxX1.Checked = true;
            else if (model.HashModel == 2)
                checkBoxX2.Checked = true;
            else
                checkBoxX3.Checked = true;
            if (model.DataGetmodel == 1)
                s1.Checked = true;
            else if (model.DataGetmodel == 2)
                s2.Checked = true;
            else
                s3.Checked = true;

            txtInt_get1_jiangetime.Value = model.Get1_jiangetime;
            get1_jingdu.SelectedIndex = model.Get1_jingduModel;
            txtInt_get1_MoreGetNumber.Value = model.Get1_getnumber;
            txtInt_PiciNumber.Value = model.Picinumber;
            IsRunSend.IsOn = model.Isrunsend;
            MoreAccountUseModel1.IsOn = model.MoreAccountUseModel;

            if (Model.V3Infos.SendPointDb.ContainsKey(model.PointId))
            {
                string s = Model.V3Infos.SendPointDb[model.PointId].GroupTag;



                txtSendClass.Text = model.Sendclass;
                isUseMakeHtml.IsOn = model.IsUseMakeHtml;
                isUseLinkDb.IsOn = model.IsUseLinkDb;


                comboBoxmingan.Text = model.Mingan;
                switchButtonhtml.IsOn = model.Iszhenggui;
                switchButtonistihuan.IsOn = model.Istihuan;
                switchButtonishunxiao.IsOn = model.Ishunxiao;
                try
                {
                    comboBoxhunxiaofangshi.Text = model.Hunxiaofangshi;
                }
                catch { comboBoxhunxiaofangshi.SelectedIndex = 0; }
                textBoxhunxiaogeshi.Text = model.Hunxiaogeshi;
                switchButtonjiangemoshi.IsOn = model.Guanjiancimoshi;
                txtInt_Xkeywordtotal.Text = model.Keywordtotal.ToString();
                txtInt_Xkeyjiange.Text = model.Keywordjiange.ToString();
                switchButtonnoqq.IsOn = model.NoQQ;
                switchButtonnoPic.IsOn = model.NoPic;
                switchButtonnoPhone.IsOn = model.NoPhone;
                switchButtonnoemail.IsOn = model.NoEmail;
                switchButtonnoA.IsOn = model.NoA;

                textBoxXMyqq.Text = model.MyQQ;
                textBoxXMyPhone.Text = model.MyPhone;
                textBoxXMyemail.Text = model.MyEmail;
                textBoxXMya.Text = model.MyA;

                comboBoxExWeiDu.Text = model.WeiyuanchuangDu;
                spinEdit_YuanChuangJianGe.Value = model.YuanChuangJianGe;
                toggleSwitch_TitleYuanChuang.IsOn = model.WeiyuanchuangXiang[0];
                switchButtonW2.IsOn = model.WeiyuanchuangXiang[1];
                switchButtonW3.IsOn = model.WeiyuanchuangXiang[2];
                switchButtonW4.IsOn = model.WeiyuanchuangXiang[3];
                switchButtonW5.IsOn = model.WeiyuanchuangXiang[4];


                weicount.Text = Model.Model_WeiYuanChuang.words.Count.ToString();
                textBoxXTitle.Text = model.WeiyuanchuangTitle;
                textBoxXFanYiLiuCheng.Text = model.Fanyiformate;
                textBoxXMyurl.Text = model.MyUrl;
                switchButtonnourl.IsOn = model.NoUrl;
                switchButtontitleFanYi.IsOn = model.IstitleFanYi;
                txtInt_PiCiShu.Value = model.GetPiciNumber;
                switchButton1.IsOn = model.GetRunModel;
                labelX11.Text = model.StartTime.ToString();
                labelX2.Text = Model.V3Infos.TaskThread.ContainsKey(model.id) && Model.V3Infos.TaskThread[model.id].Status==TaskStatus.Running ? "运行中" : "停止中";
                TimeSpan total = new TimeSpan(new TimeSpan(DateTime.Now.Ticks).CompareTo(new TimeSpan(model.StartTime.Ticks)));
                labelX19.Text = total.Days + "天" + total.Hours + "小时" + total.Minutes + "分" + total.Seconds + "秒";
                labelX13.Text = model.CountAllGet.ToString();
                labelX24.Text = model.CountThisGet.ToString();
                labelX15.Text = model.CountAllPost.ToString();
                labelX17.Text = model.CountthisPost.ToString();
                txtTaskStatus.Text = model.TaskStatusStr;

                integerInput2.Value = model.Spider_jiange;
                integerInput3.Value = model.Spider_maxget;

                if (Model.V3Infos.ArticleDb.ContainsKey(model.ArticleDbId.ToString()))
                {
                    txtArticleName.Text = "(" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Id + "]" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Name;
                }


                else
                {

                    txtArticleName.Text = "未设置或已删除！";


                }
                if (Model.V3Infos.KeywordDb.ContainsKey(model.KeywordDbId.ToString()))
                {
                    txtKeywordName.Text = "(" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Keywords.Count + ")" + "[" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Id + "]" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Name;
                }
                else
                {
                    txtKeywordName.Text = "未设置或已删除！";

                }
                if (Model.V3Infos.HashDb.ContainsKey(model.HashDbId.ToString()))
                {
                    txtHashName.Text = "(" + Model.V3Infos.HashDb[model.HashDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Id + "]" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Name;
                }
                else
                {
                    txtHashName.Text = "未设置或已删除！";

                }
                if (Model.V3Infos.ReplaceDb.ContainsKey(model.ReplaceDbId.ToString()))
                {
                    txtReplaceName.Text = "(" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Words.Count + ")" + "[" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Id + "]" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Name;
                }
                else
                {
                    txtReplaceName.Text = "未设置或已删除！";

                }
                if (Model.V3Infos.LinkDb.ContainsKey(model.LinkDbId.ToString()))
                {
                    txtLinkName.Text = "(" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Name;
                }
                else
                {
                    txtLinkName.Text = "未设置或已删除！";

                }
                if (Model.V3Infos.LinkDb.ContainsKey(model.OtherLinkDB.ToString()))
                {

                    textBoxX2.Text = "(" + Model.V3Infos.LinkDb[model.OtherLinkDB.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.OtherLinkDB.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.OtherLinkDB.ToString()].Name;
                }
                else
                {
                    textBoxX2.Text = "未设置或已删除！";
                }
                if (Model.V3Infos.MainDb.MyModels.ContainsKey(model.GetModel))
                {
                    txtGetModelName.Text = "[" + Model.V3Infos.MainDb.MyModels[model.GetModel].mids + "]" + Model.V3Infos.MainDb.MyModels[model.GetModel].PlanName;
                }
                else
                {
                    txtGetModelName.Text = "未选择抓取模块！";

                }

                if (Model.V3Infos.MainDb.MyModels.ContainsKey(Model.V3Infos.SendPointDb[model.PointId].PostModel))
                {
                    labelX58.Text = "[" + Model.V3Infos.MainDb.MyModels[Model.V3Infos.SendPointDb[model.PointId].PostModel].mids + "]" + Model.V3Infos.MainDb.MyModels[Model.V3Infos.SendPointDb[model.PointId].PostModel].PlanName + "(发布模块只能在发布点管理中修改、选择)";
                    if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.SendPointDb[model.PointId].PostModel].Stp2_POST_UsedClass)
                        labelX60.Enabled = true;
                    if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.SendPointDb[model.PointId].PostModel].Stp2_POST_UsedAddClass)
                        labelX61.Enabled = true;
                    if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.SendPointDb[model.PointId].PostModel].Stp3_POST_NeedMakeHtml)
                        labelX62.Enabled = true;
                    if (Model.V3Infos.MainDb.MyModels[Model.V3Infos.SendPointDb[model.PointId].PostModel].Stp3_POST_SupportLinkDb)
                        labelX63.Enabled = true;
                }
                else
                {
                    labelX58.Text = "未选择发布模块！" + "(发布模块只能在发布点管理中修改、选择)";
                }
            }
            PiciNumber_ValueChanged(null,null);

            switchButton5.IsOn = model.IsUseKu;
            if (model.IsUseKu == true && !System.IO.File.Exists(model.YuLiaoKuPath + "\\segments.gen"))
            {
                XtraMessageBox.Show("语料库路径无效！", "请重新选择", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else if (model.IsUseKu == true)
            {

                indexcount.Text = "语料库文件大小：" + GetDirectoryLength(model.YuLiaoKuPath) / 1024 / 1024 + "M";
            }
            else { indexcount.Text = ""; }
            if (model.IsUseKu == true)
            {
                txtGetModelName.Text = model.YuLiaoKuPath;

                groupKeyword.Visible = false;
                groupNONE.Visible = false;
                groupSpdier.Visible = false;
                groupZidingyi.Visible = false;
                groupPaneltongbu.Visible = false;
                groupPanelyuliao.Visible = true;
                switchButtonyuliaomoshi.IsOn = model.YulLiaoMoShi;
                txtInt_integerInputshuliang.Value = model.YuJuShuLiang;
                txtInt_yuliaoyingshen.Value = model.YuLiaoYingShen;
                integerInput10.Value = model.YuLiaoMax1;
            }
            #region 任务变量处理
            txtrukou.Text = "";
            for (int i = 0; i < model.RukouUrl.Count; i++)
            {
                if (i != model.RukouUrl.Count - 1)
                    txtrukou.Text += model.RukouUrl[i].ToString() + "\r\n";
                else
                    txtrukou.Text += model.RukouUrl[i].ToString();
            }
            #endregion

            dingzhicanshu.Text = model.Canshu;

        }

        public void ShowProcessPlugin()
        {
            ListBox_plugin.Items.Clear();
            foreach (KeyValuePair<string, ProcessPlugin> p in ProcessPluginList)
            {
                ListBox_plugin.Items.Add(p.Value);
            }
            if (ProcessPluginList.Count == 0)
            {
                ListBox_plugin.Visible = GroupControl_plugin.Visible = panel_plugin_enabled.Visible = false;
            }
        }
        public static long FileSize(string filePath)
        {

            //定义一个FileInfo对象，是指与filePath所指向的文件相关联，以获取其大小  

            FileInfo fileInfo = new FileInfo(filePath);

            return fileInfo.Length;

        }


        public static long GetDirectoryLength(string dirPath)
        {

            long len = 0;

            //判断该路径是否存在（是否为文件夹）  

            if (!Directory.Exists(dirPath))
            {

                //查询文件的大小  

                len = FileSize(dirPath);

            }

            else
            {

                //定义一个DirectoryInfo对象  

                DirectoryInfo di = new DirectoryInfo(dirPath);



                //通过GetFiles方法，获取di目录中的所有文件的大小  

                foreach (FileInfo fi in di.GetFiles())
                {
                    if (fi.Extension == ".cfs" || fi.Extension == ".gen" || fi.FullName.Contains("segments"))
                    {
                        len += fi.Length;
                    }

                }

                //获取di中所有的文件夹，并存到一个新的对象数组中，以进行递归  

                DirectoryInfo[] dis = di.GetDirectories();

                if (dis.Length > 0)
                {

                    for (int i = 0; i < dis.Length; i++)
                    {

                        len += GetDirectoryLength(dis[i].FullName);

                    }

                }

            }

            return len;

        }
        void checkdingzhi()
        {
            //文章末尾插入文章功能
            //if (!xEngine.License.MyLicense.Custom.Contains("wenzhangmoweicharu")) 
            //{
            //    wenzhangmoweicharu.Visible = false;


            //}
            //if (!xEngine.License.MyLicense.Custom.Contains("settags")) 
            //{

            //    buttonsettags.Visible = false;
            //}
            //if (!xEngine.License.MyLicense.Custom.Contains("kan")) { modifytags.Visible = false; }
            //if (!xEngine.License.MyLicense.Custom.Contains("lianjie"))
            //{
            //    setart.Visible = false;
            //}

        }
        public void showI(string txt)
        {
            if (isShow) {
                this.Invoke((EventHandler)(delegate
                {
                    ipanel.Visible = true;
                    istate.Text = txt;
                }));
            }

        }
        public void closeI()
        {
            if (isShow)
            {
                this.Invoke((EventHandler)(delegate
            {
                ipanel.Visible = false;
            }));
            }
        }
       

        /// <summary>
        /// 用户定制内容处理方面的功能
        /// </summary>
        /// <returns></returns>
        #region
        string geregex(string html, string regex, int num, bool msg)
        {
            string result = "";
            try
            {

                MatchCollection mccc = Regex.Matches(html, regex, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                for (int i = 0; i < mccc.Count; i++)
                {
                    result = mccc[i].Groups[0].Value;
                    if (i == num)
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }
        string weibusuijicharu()
        {


            string result = "";
            try
            {
                if (xEngine.License.MyLicense.Custom.Contains("wenzhangmoweicharu"))
                {
                    string gonggongkustr = geregex(model.Canshu, "(?<=<wenzhangmoweicharu>).*?(?=</wenzhangmoweicharu>)", 1, false);
                    if (gonggongkustr.Split('|').Length >= 2)
                    {
                        result = gonggongkustr;

                    }
                }
                if (xEngine.License.MyLicense.Custom.Contains("settags"))
                {
                    string gonggongkustr = geregex(model.Canshu, "(?<=<settags>).*?(?=</settags>)", 1, false);
                    if (gonggongkustr.Split('|').Length >= 2)
                    {
                        result = gonggongkustr;

                    }
                }
                if (xEngine.License.MyLicense.Custom.Contains("kan"))
                {
                    string gonggongkustr = geregex(model.Canshu, "(?<=<modifytags>).*?(?=</modifytags>)", 1, false);
                    if (gonggongkustr.Split('|').Length >= 2)
                    {
                        result = gonggongkustr;

                    }
                }

            }
            catch { }
            return result;

        }
        #endregion
        private void TasKFrm_Load(object sender, EventArgs e)
        {
            //检查处理方面的定制功能
            pointid = model.PointId;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            checkdingzhi();
            GetInfo();
            isShow = true;
            buttonX1.Text = "开始清理";
            buttonX1.Enabled = true;

        }
       

        #region 任务信息
        private void btnSelectArticle_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 0;
            frm.Text = "请选择或新建一个文章库[请“双击”选择您要使用的库]";
            frm.isselectdb = true;
            frm.ShowDialog();
            if (frm.selectedDBid != 0)
            {
                model.ArticleDbId = frm.selectedDBid;
                txtArticleName.Text = "(" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Id + "]" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Name;
            }
        }

        private void btnSelectKeyword_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 1;
            frm.Text = "请选择或新建一个关键词库[请“双击”选择您要使用的库]";
            frm.isselectdb = true;
            frm.ShowDialog();
            if (frm.selectedDBid != 0)
            {
                model.KeywordDbId = frm.selectedDBid;
                txtKeywordName.Text = "(" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Keywords.Count + ")" + "[" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Id + "]" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Name;
            }
        }

        private void btnSelectHash_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 2;
            frm.Text = "请选择或新建一个哈希库[请“双击”选择您要使用的库]";
            frm.isselectdb = true;
            frm.ShowDialog();
            if (frm.selectedDBid != 0)
            {
                model.HashDbId = frm.selectedDBid;
                txtHashName.Text = "(" + Model.V3Infos.HashDb[model.HashDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Id + "]" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Name;
            }
        }

        private void btnSelectReplace_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 3;
            frm.Text = "请选择或新建一个替换库[请“双击”选择您要使用的库]";
            frm.isselectdb = true;
            frm.ShowDialog();
            if (frm.selectedDBid != 0)
            {
                model.ReplaceDbId = frm.selectedDBid;
                txtReplaceName.Text = "(" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Words.Count + ")" + "[" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Id + "]" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Name;
            }
        }

        private void btnSelectLink_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 4;
            frm.Text = "请选择或新建一个链接库[请“双击”选择您要使用的库]";
            frm.isselectdb = true;
            frm.ShowDialog();
            if (frm.selectedDBid != 0)
            {
                model.LinkDbId = frm.selectedDBid;
                txtLinkName.Text = "(" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Name;
            }
        }

        private void btnAddArticle_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(1, Model.V3Infos.SendPointDb[model.PointId].name + "-新建文章库", Model.V3Infos.SendPointDb[model.PointId].GroupTag);
            if (id != 0)
            {
                model.ArticleDbId = id;
                txtArticleName.Text = "(" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Id + "]" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Name;
            }
        }

        private void btnAddKeyword_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(2, Model.V3Infos.SendPointDb[model.PointId].name + "-新建关键词库", Model.V3Infos.SendPointDb[model.PointId].GroupTag);
            if (id != 0)
            {
                model.KeywordDbId = id;
                txtKeywordName.Text = "(" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Keywords.Count + ")" + "[" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Id + "]" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Name;
            }
        }

        private void btnAddhash_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(3, Model.V3Infos.SendPointDb[model.PointId].name + "-新建哈希库", Model.V3Infos.SendPointDb[model.PointId].GroupTag);
            if (id != 0)
            {
                model.HashDbId = id;
                txtHashName.Text = "(" + Model.V3Infos.HashDb[model.HashDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Id + "]" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Name;
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(4, Model.V3Infos.SendPointDb[model.PointId].name + "-新建替换库", Model.V3Infos.SendPointDb[model.PointId].GroupTag);
            if (id != 0)
            {
                model.ReplaceDbId = id;
                txtReplaceName.Text = "(" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Words.Count + ")" + "[" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Id + "]" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Name;
            }
        }

        private void btnAddLink_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(5, Model.V3Infos.SendPointDb[model.PointId].name + "-新建链接库", Model.V3Infos.SendPointDb[model.PointId].GroupTag);
            if (id != 0)
            {
                model.LinkDbId = id;
                txtLinkName.Text = "(" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Name;
            }
        }

        private void btnArticleDefault_Click(object sender, EventArgs e)
        {
            if (!Model.V3Infos.KeywordDb.ContainsKey(model.ArticleDbId.ToString()))
                return;
            model.ArticleDbId = Model.V3Infos.SendPointDb[model.PointId].ArticleDbID;
            txtArticleName.Text = "(" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Id + "]" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Name;
        }

        private void btnKeywordDefault_Click(object sender, EventArgs e)
        {
            if (!Model.V3Infos.KeywordDb.ContainsKey(model.KeywordDbId.ToString()))
                return;
            model.KeywordDbId = Model.V3Infos.SendPointDb[model.PointId].KeywordDbID;
            txtKeywordName.Text = "(" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Keywords.Count + ")" + "[" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Id + "]" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Name;

        }

        private void btnHashDefault_Click(object sender, EventArgs e)
        {
            if (!Model.V3Infos.HashDb.ContainsKey(model.HashDbId.ToString()))
                return;
            model.HashDbId = Model.V3Infos.SendPointDb[model.PointId].HashDbID;
            txtHashName.Text = "(" + Model.V3Infos.HashDb[model.HashDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Id + "]" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Name;

        }

        private void btnReplaceDefault_Click(object sender, EventArgs e)
        {
            if (!Model.V3Infos.ReplaceDb.ContainsKey(model.ReplaceDbId.ToString()))
                return;
            model.ReplaceDbId = Model.V3Infos.SendPointDb[model.PointId].ReplaceDbid;
            txtReplaceName.Text = "(" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Words.Count + ")" + "[" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Id + "]" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Name;

        }

        private void btnLinkDefault_Click(object sender, EventArgs e)
        {
            if (!Model.V3Infos.LinkDb.ContainsKey(model.LinkDbId.ToString()))
                return;
            model.LinkDbId = Model.V3Infos.SendPointDb[model.PointId].LinkDbID;
            txtLinkName.Text = "(" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Name;

        }
      
        private void integerInput1_ValueChanged(object sender, EventArgs e)
        {
            PiciNumber_ValueChanged(null,null);
        }

        #endregion



        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (Model.V3Infos.MainDb.MyModels.ContainsKey(model.GetModel))
            {
                System.Diagnostics.Process.Start("iexplore", Model.V3Infos.MainDb.MyModels[model.GetModel].PlanUrl);
            }
            else
            {
                XtraMessageBox.Show("请先选择一个抓取模块！", "未选择抓取模块", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSelectGetModel_Click(object sender, EventArgs e)
        {
            if (switchButton5.IsOn == false)
            {


                V3Form.frmModelShop frm = new frmModelShop();
                frm.TabTag = "my";
                frm.selectmodel = 1;
                frm.Text = "V3模块市场 [请在“我的模块”中“双击”选择一个您要使用的“抓取模块”]";frm.ShowDialog();
                if (frm.selectedid != "")
                {
                    model.GetModel = frm.selectedid;
                    txtGetModelName.Text = "[" + Model.V3Infos.MainDb.MyModels[model.GetModel].mids + "]" + Model.V3Infos.MainDb.MyModels[model.GetModel].PlanName;
                }
            }
            else
            {

                FolderBrowserDialog f = new FolderBrowserDialog();
                if (f.ShowDialog().ToString() == "OK")
                {
                    txtGetModelName.Text = f.SelectedPath;
                    model.YuLiaoKuPath = f.SelectedPath;
                    groupPanelyuliao.Visible = true;
                    if (System.IO.File.Exists(f.SelectedPath + "/segments.gen"))
                    {

                        indexcount.Text = "语料库文件大小：" + GetDirectoryLength(f.SelectedPath) / 1024 / 1024 + "M";
                    }
                    else
                    {
                        indexcount.Text = "语料库路径无效！";
                    }
                }
            }
        }
      

        private void btnViewArticleDb_Click(object sender, EventArgs e)
        {
            if (Model.V3Infos.ArticleDb.ContainsKey(model.ArticleDbId.ToString()))
            {
                frm_文章库文章列表 dbview = new frm_文章库文章列表();
                dbview.dbid = model.ArticleDbId.ToString();
                dbview.Text = Model.V3Infos.ArticleDb[dbview.dbid].Name + "(文章总数：" + Model.V3Infos.ArticleDb[dbview.dbid].DataCount + "个） - 文章库编辑";
                dbview.ShowDialog();
            }
            else
            {
                XtraMessageBox.Show("请先选择一个文章库！", "未选择文章库", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnViewKeywordDb_Click(object sender, EventArgs e)
        {
            if (Model.V3Infos.KeywordDb.ContainsKey(model.KeywordDbId.ToString()))
            {
                frm_关键词库 view = new frm_关键词库();
                view.Dbid = model.KeywordDbId.ToString();
                view.Text = Model.V3Infos.KeywordDb[view.Dbid].Name + "(关键词总数：" + Model.V3Infos.KeywordDb[view.Dbid].Keywords.Count + "个） - 关键词库编辑";

                view.ShowDialog();
                if (view.IsSave)
                    Model.V3Infos.KeywordDb[view.Dbid].Keywords = view.WordList;
                Bll.DbBll.SaveDb(2, view.Dbid);
                view.Dispose();
            }
            else
            {
                XtraMessageBox.Show("请先选择一个关键词库！", "未选择关键词库", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtGetModelName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (switchButton5.IsOn == true)
                {
                    groupKeyword.Visible = false;
                    groupNONE.Visible = false;
                    groupSpdier.Visible = false;
                    groupZidingyi.Visible = false;
                    groupPaneltongbu.Visible = false;
                    groupPanelyuliao.Visible = true;
                }
                else if (Model.V3Infos.MainDb.MyModels[model.GetModel].PlanModel == 1 || Model.V3Infos.MainDb.MyModels[model.GetModel].PlanModel == 10)
                {
                    groupKeyword.Visible = true;
                    groupNONE.Visible = false;
                    groupSpdier.Visible = false;
                    groupPanelyuliao.Visible = false;
                    groupZidingyi.Visible = false;
                    groupPaneltongbu.Visible = false;
                }
                else if (Model.V3Infos.MainDb.MyModels[model.GetModel].PlanModel == 2 || Model.V3Infos.MainDb.MyModels[model.GetModel].PlanModel == 20)
                {
                    groupKeyword.Visible = false;
                    groupNONE.Visible = false;
                    groupSpdier.Visible = false;
                    groupPanelyuliao.Visible = false;
                    groupZidingyi.Visible = true;
                    groupPaneltongbu.Visible = false;
                }
                else if (Model.V3Infos.MainDb.MyModels[model.GetModel].PlanModel == 3 || Model.V3Infos.MainDb.MyModels[model.GetModel].PlanModel == 30)
                {
                    groupKeyword.Visible = false;
                    groupNONE.Visible = false;
                    groupPanelyuliao.Visible = false;
                    groupSpdier.Visible = true;
                    groupZidingyi.Visible = false;
                    groupPaneltongbu.Visible = false;
                }
                else if (Model.V3Infos.MainDb.MyModels[model.GetModel].PlanModel == 4 || Model.V3Infos.MainDb.MyModels[model.GetModel].PlanModel == 40)
                {

                    groupKeyword.Visible = false;
                    groupPanelyuliao.Visible = false;
                    groupNONE.Visible = false;
                    groupSpdier.Visible = false;
                    groupZidingyi.Visible = false;
                    groupPaneltongbu.Visible = true;
                }

                else
                {
                    groupKeyword.Visible = false;
                    groupNONE.Visible = true;
                    groupSpdier.Visible = false;
                    groupZidingyi.Visible = false;
                    groupZidingyi.Visible = false;
                    groupPanelyuliao.Visible = false;

                }
            }
            catch { }
        }

        private void btnGetSendClass_Click(object sender, EventArgs e)
        {
            if (!Model.V3Infos.MainDb.MyModels.ContainsKey(Model.V3Infos.SendPointDb[model.PointId].PostModel))
            {
                XtraMessageBox.Show( "请去站点属性里面选择发布模块！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!Model.V3Infos.MainDb.MyModels[Model.V3Infos.SendPointDb[model.PointId].PostModel].Stp2_POST_UsedClass)
            {
                XtraMessageBox.Show( "任务使用的发布模块不支持获取分类，您可以手工填写分类ID", "不支持哦", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            frmChannel frm = new frmChannel();
            frm.Model = Model.V3Infos.SendPointDb[model.PointId];
            V3.Bll.PostBll bll = new V3.Bll.PostBll(Model.V3Infos.MainDb.MyModels[Model.V3Infos.SendPointDb[model.PointId].PostModel], model.PointId, 0);
            frm.bll = bll;

            frm.ShowDialog();

            if (frm.issave)
            {
                txtSendClass.Text = frm.fenleistr;
            }
        }
        private void linkLabel_RandomKeyword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBoxXTitle.Text += "[" + linkLabel_RandomKeyword.Text + "]";
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//标题设置，随机关键字
        {
            textBoxXTitle.Text += "[" + linkLabel1.Text + "]";
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//标题设置，主关键字
        {
            textBoxXTitle.Text += "[" + linkLabel3.Text + "]";
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//文章中的一句话
        {
            textBoxXTitle.Text += "[" + linkLabel2.Text + "]";
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//中文
        {
            textBoxXFanYiLiuCheng.Text += linkLabel4.Text + "→";
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBoxXFanYiLiuCheng.Text += linkLabel5.Text + "→";
        }

        private void textBoxXFanYiLiuCheng_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxXFanYiLiuCheng.Text.Trim() == "")
                {
                    textBoxXFanYiLiuCheng.Text = "中文→";

                }
                else if (!textBoxXFanYiLiuCheng.Text.Substring(0, 1).Contains("中"))
                {
                    textBoxXFanYiLiuCheng.Text = textBoxXFanYiLiuCheng.Text.Remove(textBoxXFanYiLiuCheng.Text.IndexOf("→"));
                    textBoxXFanYiLiuCheng.Text = "中文→" + textBoxXFanYiLiuCheng.Text;
                }
            }
            catch { textBoxXFanYiLiuCheng.Text = "中文→英文"; }
            
        }

        private void buttonX1_Click_1(object sender, EventArgs e)
        { 
            if (XtraMessageBox.Show("该操作会同时清空蜘蛛列队和<color=red>当前任务的地址Hash库</color>，可能会抓取到重复的内容，但不会保存，确定要清空吗！", "确定清空", MessageBoxButtons.YesNo, MessageBoxIcon.Information, DefaultBoolean.True) == DialogResult.Yes)
            {
                buttonX1.Text = "正在清理";
                buttonX1.Enabled = false;
                try
                {
                    if (!Model.V3Infos.MainDb.MyModels.ContainsKey(model.GetModel) || !Model.V3Infos.TaskDb.ContainsKey(model.id))
                    {
                        XtraMessageBox.Show("蜘蛛使用抓取模块或者任务ID有误，清空失败！可以通过换一个hash库实现同样效果！", "清空失败", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        return;
                    }
                    Thread thread = new Thread(delegate()
                    {
                        if ( Model.V3Infos.MainDb.Spiderque.ContainsKey(model.GetModel + "-" +Model.V3Infos.TaskDb[model.id].HashDbId))
                        {
                            Model.V3Infos.MainDb.Spiderque[ model.GetModel + "-" + Model.V3Infos.TaskDb[model.id].HashDbId].Clear();
                        }
                        try
                        {
                            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" +
                                Model.V3Infos.TaskDb[model.id].HashDbId + "_T\\"))
                            {
                                System.IO.Directory.Delete(
                                                               AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" +
                                                               Model.V3Infos.TaskDb[model.id].HashDbId + "_T\\", true);
                            }
                            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" +
                                                 Model.V3Infos.TaskDb[model.id].HashDbId + "_C\\"))
                            {
                                System.IO.Directory.Delete(
                                    AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" +
                                    Model.V3Infos.TaskDb[model.id].HashDbId + "_C\\", true);
                            }
                            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" +
                                                 Model.V3Infos.TaskDb[model.id].HashDbId + "_U\\"))
                            {
                                System.IO.Directory.Delete(
                                    AppDomain.CurrentDomain.BaseDirectory + "\\系统数据\\HashDb\\" +
                                    Model.V3Infos.TaskDb[model.id].HashDbId + "_U\\", true);
                            }
                            if (isShow)
                            {
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    buttonX1.Text = "开始清理";
                                    buttonX1.Enabled = true;
                                    XtraMessageBox.Show("成功清空列队和Hash库，现在可以重新开始采集了！", "清空完毕", MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                                }));
                            }
                        }
                        catch(Exception error)
                        {

                            if (isShow)
                            {
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    buttonX1.Text = "开始清理";
                                    buttonX1.Enabled = true;
                                    XtraMessageBox.Show("清理出现异常，如果确定文件已删除即可正常采集："+error.Message, "清空完毕", MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                                }));
                            }
                        }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                catch (Exception err)
                {
                    XtraMessageBox.Show(err.Message, "清空失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }  
        }

        private void linkLabel11_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)//头关键词
        {
            if (textBoxXliantou.Text.Contains("[关键词]"))
            {
                XtraMessageBox.Show( "已经添加了[关键词]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxXliantou.Text = textBoxXliantou.Text + "[关键词]";
            }
        }

        private void linkLabel10_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)//头标题
        {

            if (textBoxXliantou.Text.Contains("[标题]"))
            {
                XtraMessageBox.Show( "已经添加了[标题]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxXliantou.Text = textBoxXliantou.Text + "[标题]";
            }

        }

        private void linkLabel9_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)//头链接地址
        {
            if (textBoxXliantou.Text.Contains("[链接地址]"))
            {
                XtraMessageBox.Show( "已经添加了[链接地址]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxXliantou.Text = textBoxXliantou.Text + "[链接地址]";
            }
        }

        private void linkLabel14_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)//中关键词
        {
            if (textBoxXlianzhong.Text.Contains("[关键词]"))
            {
                XtraMessageBox.Show( "已经添加了[关键词]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxXlianzhong.Text = textBoxXlianzhong.Text + "[关键词]";
            }
        }

        private void linkLabel13_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)//中标题
        {
            if (textBoxXlianzhong.Text.Contains("[标题]"))
            {
                XtraMessageBox.Show( "已经添加了[标题]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxXlianzhong.Text = textBoxXlianzhong.Text + "[标题]";
            }
        }

        private void linkLabel12_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)//中连接地址
        {
            if (textBoxXlianzhong.Text.Contains("[链接地址]"))
            {
                XtraMessageBox.Show( "已经添加了[链接地址]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxXlianzhong.Text = textBoxXlianzhong.Text + "[链接地址]";
            }
        }

        private void linkLabel17_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)//尾关键词
        {
            if (textBoxXlianwei.Text.Contains("[关键词]"))
            {
                XtraMessageBox.Show( "已经添加了[关键词]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxXlianwei.Text = textBoxXlianwei.Text + "[关键词]";
            }
        }

        private void linkLabel16_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)//尾标题
        {
            if (textBoxXlianwei.Text.Contains("[标题]"))
            {
                XtraMessageBox.Show( "已经添加了[标题]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxXlianwei.Text = textBoxXlianwei.Text + "[标题]";
            }
        }

        private void linkLabel15_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)//尾链接地址
        {
            if (textBoxXlianwei.Text.Contains("[链接地址]"))
            {
                XtraMessageBox.Show( "已经添加了[链接地址]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxXlianwei.Text = textBoxXlianwei.Text + "[链接地址]";
            }

        }

        private void btn_get1JingduKeyword_Click_1(object sender, EventArgs e)//编辑种子词
        {
            frm_参数输入编辑器 frm = new frm_参数输入编辑器();
            frm.Text = "请输入种子关键字";
            frm.txtStatus.Caption = "一行一个关键字";
            frm.txttitle.Caption = "一行一个关键字";
            string str = "";
            for (int i = 0; i < model.Get1_jingdukeyword.Length; i++)
            {
                str += model.Get1_jingdukeyword[i] + "\r\n";
            }
            frm.txtMainbox.Text = str;

            frm.ShowDialog();

            if (frm.issave)
            {
                string[] aa = frm.txtMainbox.Lines;
                System.Collections.ArrayList ary = new System.Collections.ArrayList();
                for (int i = 0; i < aa.Length; i++)
                {
                    if (aa[i].Length > 0)
                        ary.Add(aa[i]);
                }
                string[] jieguo = new string[ary.Count];
                for (int i = 0; i < ary.Count; i++)
                {
                    jieguo[i] = ary[i].ToString();
                }
                model.Get1_jingdukeyword = jieguo;
            }
        }

        private void buttonX5_Click_1(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 4;
            frm.Text = "请选择或新建一个链接库[请“双击”选择您要使用的库]";
            frm.isselectdb = true;

            frm.ShowDialog();

            if (frm.selectedDBid != 0)
            {
                model.OtherLinkDB = frm.selectedDBid;
                textBoxX2.Text = "(" + Model.V3Infos.LinkDb[model.OtherLinkDB.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.OtherLinkDB.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.OtherLinkDB.ToString()].Name;
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)//选择一个链接库
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 4;
            frm.Text = "请选择或新建一个链接库[请“双击”选择您要使用的库]";
            frm.isselectdb = true;

            frm.ShowDialog();

            if (frm.selectedDBid != 0)
            {
                model.ZidingLinkDb = frm.selectedDBid;
                textBoxX5.Text = "(" + Model.V3Infos.LinkDb[model.ZidingLinkDb.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.ZidingLinkDb.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.ZidingLinkDb.ToString()].Name;
            }
        }

        private void linkLabel23_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//头关键词
        {
            if (textBoxX4.Text.Contains("[关键词]"))
            {
                XtraMessageBox.Show( "已经添加了[关键词]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxX4.Text = textBoxX4.Text + "[关键词]";
            }
        }

        private void linkLabel22_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//头标题
        {
            if (textBoxX4.Text.Contains("[标题]"))
            {
                XtraMessageBox.Show( "已经添加了[标题]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxX4.Text = textBoxX4.Text + "[标题]";
            }
        }

        private void linkLabel21_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//头连接
        {
            if (textBoxX4.Text.Contains("[链接地址]"))
            {
                XtraMessageBox.Show( "已经添加了[链接地址]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxX4.Text = textBoxX4.Text + "[链接地址]";
            }
        }

        private void linkLabel20_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//中关键词
        {
            if (textBoxX3.Text.Contains("[关键词]"))
            {
                XtraMessageBox.Show( "已经添加了[关键词]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxX3.Text = textBoxX3.Text + "[关键词]";
            }
        }

        private void linkLabel19_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//中标题
        {
            if (textBoxX3.Text.Contains("[标题]"))
            {
                XtraMessageBox.Show( "已经添加了[标题]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxX3.Text = textBoxX3.Text + "[标题]";
            }
        }

        private void linkLabel18_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//中链接地址
        {
            if (textBoxX3.Text.Contains("[链接地址]"))
            {
                XtraMessageBox.Show( "已经添加了[链接地址]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxX3.Text = textBoxX3.Text + "[链接地址]";
            }
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//为关键词
        {
            if (textBoxX1.Text.Contains("[关键词]"))
            {
                XtraMessageBox.Show( "已经添加了[关键词]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxX1.Text = textBoxX1.Text + "[关键词]";
            }
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//为标题
        {
            if (textBoxX1.Text.Contains("[标题]"))
            {
                XtraMessageBox.Show( "已经添加了[标题]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxX1.Text = textBoxX1.Text + "[标题]";
            }
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//尾链接地址
        {
            if (textBoxX1.Text.Contains("[链接地址]"))
            {
                XtraMessageBox.Show( "已经添加了[链接地址]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxX1.Text = textBoxX1.Text + "[链接地址]";
            }
        }

        private void linkLabel24_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBoxXTitle.Text += "[随机连接符]";
        }

        private void buttonX7_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 4;
            frm.Text = "请选择或新建一个链接库[请“双击”选择您要使用的库]";
            frm.isselectdb = true;

            frm.ShowDialog();

            if (frm.selectedDBid != 0)
            {
                model.MaouseDefaultlinkDb = false;
                model.MaolinkDbId = frm.selectedDBid;
                textBoxX10.Text = "(" + Model.V3Infos.LinkDb[model.MaolinkDbId.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.MaolinkDbId.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.MaolinkDbId.ToString()].Name;
            }
        }

      

        private void linkLabel33_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//关键词
        {
            if (textBoxX9.Text.Contains("[关键词]"))
            {
                XtraMessageBox.Show( "已经添加了[关键词]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxX9.Text = textBoxXliantou.Text + "[关键词]";
            }
        }

        private void linkLabel32_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//标题
        {
            if (textBoxX9.Text.Contains("[标题]"))
            {
                XtraMessageBox.Show( "已经添加了[标题]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxX9.Text = textBoxX4.Text + "[标题]";
            }
        }

        private void linkLabel31_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//链接弟子
        {
            if (textBoxX9.Text.Contains("[链接地址]"))
            {
                XtraMessageBox.Show( "已经添加了[链接地址]项哦！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                textBoxX9.Text = textBoxX9.Text + "[链接地址]";
            }
        }

        private void PiciNumber_ValueChanged(object sender, EventArgs e)
        {
            if (IsAutoTask.IsOn)
            {
                if ((1440/txtInt_PiCiJianGe.Value).ToString() == "0")
                {
                    labelX29.Text = "任务在一天内会执行1次，每次发布" + txtInt_PiciNumber.Value + "篇，24小时内约发布" +
                                    txtInt_PiciNumber.Value + "篇";
                }
                else
                {
                    labelX29.Text = "任务在一天内会执行" + Convert.ToInt32(1440/txtInt_PiCiJianGe.Value).ToString() + "次，每次发布" +
                                    txtInt_PiciNumber.Value + "篇，一天内约发布" +
                                    txtInt_PiciNumber.Value*(1440/txtInt_PiCiJianGe.Value) + "篇";
                }
            }
            else
            {
                labelX29.Text = "任务每次运行会发布" + txtInt_PiciNumber.Value + "篇";
            }
        }

        private void textBoxXtou_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (Program.f_frmReplaceTag == null || Program.f_frmReplaceTag.IsDisposed)
                {
                    Program.f_frmReplaceTag = new frmReplaceTag();
                }
                Program.f_frmReplaceTag.referer = "frmTask_textBoxXtou";
                Program.f_frmReplaceTag.Focus();
                Program.f_frmReplaceTag.Show();
                Program.f_frmReplaceTag.Location = new Point(0, 0);
            }

        }

        private void textBoxXwei_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (Program.f_frmReplaceTag == null || Program.f_frmReplaceTag.IsDisposed)
                {
                    Program.f_frmReplaceTag = new frmReplaceTag();
                }
                Program.f_frmReplaceTag.referer = "frmTask_textBoxXwei";
                Program.f_frmReplaceTag.Focus();
                Program.f_frmReplaceTag.Show();
                Program.f_frmReplaceTag.Location = new Point(0, 0);
            }


        }
        private void btnGetMajia_Click(object sender, EventArgs e)
        {
            //showI("正在提取马甲");
            //System.Threading.Thread t = new System.Threading.Thread(GetMajia);
            //t.IsBackground = true;
            //t.Start();
            frmGetCookie f = new frmGetCookie();
            f.CookieStr = txtmajia.Text;
            f.ShowDialog();
            if (f.GetSuccess)
            {
                txtmajia.Text = f.CookieStr;
            }
            

        }

      
        private void wenzhangmoweicharu_Click(object sender, EventArgs e)
        {
           
           

        }

        private void buttonsettags_Click(object sender, EventArgs e)
        {
            
        }

        private void modifytags_Click(object sender, EventArgs e)
        {
           


        }

        private void serart_Click(object sender, EventArgs e)
        {
           



        }

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetInfo();
            if (issave == true)
            {
                this.Close();
            }
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            issave = false;
            this.Close();
        }

        private void btn_setdefault_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Model.V3Infos.MainDb.DefaultTasks = xEngine.Common.XSerializable.CloneObject<Model.Task>(model);
            //Model.V3Infos.MainDb.DefaultTasks.Lianjiefus =new List<string>();
            //foreach (var s in model.Lianjiefus)
            //{
            //    Model.V3Infos.MainDb.DefaultTasks.Lianjiefus .Add(s);
            //}
            //Model.V3Infos.MainDb.DefaultTasks.WeiyuanchuangXiang = (bool[])model.WeiyuanchuangXiang.Clone();
            frmDefaultTasks f = new frmDefaultTasks();
            f.DefaultTask = model;
            f.ShowDialog();
            if (f.IsOK)
            {
                XtraMessageBox.Show("成功加入到任务模板列表！", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            

        }

        private void btn_usedefault_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (Model.V3Infos.MainDb.DefaultTasks.TaskName == null || Model.V3Infos.MainDb.DefaultTasks.TaskName.Length == 0)
            //{ XtraMessageBox.Show( "当前系统没有默认任务模板！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            //else
            //{
            //    Model.V3Infos.MainDb.DefaultTasks.id = model.id;
            //    model = xEngine.Common.XSerializable.CloneObject<Model.Task>(Model.V3Infos.MainDb.DefaultTasks);
            //    model.Lianjiefus =new List<string>();
            //    foreach (var s in Model.V3Infos.MainDb.DefaultTasks.Lianjiefus)
            //    {
            //         model.Lianjiefus.Add(s);
            //    }

            //    model.WeiyuanchuangXiang = (bool[])Model.V3Infos.MainDb.DefaultTasks.WeiyuanchuangXiang.Clone();
            //    model.PointId = pointid;
            //    GetInfo();

            //}
            int task_id = model.id;

            frmDefaultTasks f = new frmDefaultTasks();
            f.ShowDialog();
            if (f.IsOK)
            {
                model = f.DefaultTask;
                model.id = task_id;
                model.PointId = pointid;
                GetInfo();

            }
        }

        private void btn_test_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmTest t = new frmTest();
            t.taskid = model.id;
            t.ShowDialog();
        }

        private void frmTask_FormClosing(object sender, FormClosingEventArgs e)
        {
            isShow = false;
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed )
            {
                Program.f_frmReplaceTag.Close();
            }
        }

        private void textBoxXtou_MouseUp(object sender, MouseEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed)
            {
                Program.f_frmReplaceTag.referer = "frmTask_textBoxXtou";
            }
        }

        private void textBoxXwei_MouseUp(object sender, MouseEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed)
            {
                Program.f_frmReplaceTag.referer = "frmTask_textBoxXwei";
            }
        }

        private void switchButton5_Toggled(object sender, EventArgs e)
        {
            if (switchButton5.IsOn == true)
            {
                if (Program.Level < 120)
                {
                    XtraMessageBox.Show( "您所使用的版本不支持【语料库】功能，请联系客服升级后使用哦！", "升级版本后才能使用哦！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    switchButton5.IsOn = false;
                    return;
                }
            }
            if (switchButton5.IsOn == true)
            {
                btnSelectGetModel.Text = "选择语料库位置";
                btnOpenGetmoldeUrl.Visible = false;
                model.IsUseKu = true;
                groupKeyword.Visible = false;
                groupNONE.Visible = false;
                groupSpdier.Visible = false;
                groupZidingyi.Visible = false;
                groupPaneltongbu.Visible = false;
                groupPanelyuliao.Visible = true;
            }
            else
            {
                btnSelectGetModel.Text = "选择模块";
                btnOpenGetmoldeUrl.Visible = true;

                if (Model.V3Infos.ArticleDb.ContainsKey(model.ArticleDbId.ToString()))
                {
                    txtArticleName.Text = "(" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Id + "]" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Name;
                }


                else
                {

                    txtArticleName.Text = "未设置或已删除！";


                }
                if (Model.V3Infos.KeywordDb.ContainsKey(model.KeywordDbId.ToString()))
                {
                    txtKeywordName.Text = "(" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Keywords.Count + ")" + "[" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Id + "]" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Name;
                }
                else
                {
                    txtKeywordName.Text = "未设置或已删除！";

                }
                if (Model.V3Infos.HashDb.ContainsKey(model.HashDbId.ToString()))
                {
                    txtHashName.Text = "(" + Model.V3Infos.HashDb[model.HashDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Id + "]" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Name;
                }
                else
                {
                    txtHashName.Text = "未设置或已删除！";

                }
                if (Model.V3Infos.ReplaceDb.ContainsKey(model.ReplaceDbId.ToString()))
                {
                    txtReplaceName.Text = "(" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Words.Count + ")" + "[" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Id + "]" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Name;
                }
                else
                {
                    txtReplaceName.Text = "未设置或已删除！";

                }
                if (Model.V3Infos.LinkDb.ContainsKey(model.LinkDbId.ToString()))
                {
                    txtLinkName.Text = "(" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Name;
                }
                else
                {
                    txtLinkName.Text = "未设置或已删除！";

                }
                if (Model.V3Infos.LinkDb.ContainsKey(model.OtherLinkDB.ToString()))
                {

                    textBoxX2.Text = "(" + Model.V3Infos.LinkDb[model.OtherLinkDB.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.OtherLinkDB.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.OtherLinkDB.ToString()].Name;
                }
                else
                {
                    textBoxX2.Text = "未设置或已删除！";
                }
                if (Model.V3Infos.MainDb.MyModels.ContainsKey(model.GetModel))
                {
                    txtGetModelName.Text = "[" + Model.V3Infos.MainDb.MyModels[model.GetModel].mids + "]" + Model.V3Infos.MainDb.MyModels[model.GetModel].PlanName;
                }
                else
                {
                    txtGetModelName.Text = "未选择抓取模块！";

                }

            }
        }

        private void IsAutoTask_Toggled(object sender, EventArgs e)
        {
            PiciNumber_ValueChanged(null, null);
            if (IsAutoTask.IsOn)
            {
                txtInt_PiCiJianGe.Enabled = true;

            }
            else
            {
                txtInt_PiCiJianGe.Enabled = false;

            }
        }

        private void switchButtonjiangemoshi_Toggled(object sender, EventArgs e)
        {
            if (switchButtonjiangemoshi.IsOn == true)//总数模式
            {
                txtInt_Xkeywordtotal.Enabled = true;
                txtInt_Xkeyjiange.Enabled = false;
                txtInt_Xkeyjiange.Text = "";
                txtInt_Xkeywordtotal.Text = "10";

            }
            else
            {
                txtInt_Xkeywordtotal.Enabled = false;
                txtInt_Xkeyjiange.Enabled = true;
                txtInt_Xkeywordtotal.Text = "";
                txtInt_Xkeyjiange.Text = "300";

            }
        }

        private void switchButtonW3_Toggled(object sender, EventArgs e)
        {
          
        }

        private void switchButton3_Toggled(object sender, EventArgs e)
        {
            if (switchButton3.IsOn == true)
            {
                textBoxX2.Enabled = buttonX5.Enabled = true;
            }
            else
            {
                textBoxX2.Enabled = buttonX5.Enabled = false;
            }
        }

        private void jiangeorzhongshu_Toggled(object sender, EventArgs e)
        {
            if (jiangeorzhongshu.IsOn == true)
            {
                txtInt_integerInput8.Enabled = true;
                txtInt_jiange.Enabled = false;
            }
            else
            {
                txtInt_integerInput8.Enabled = false;
                txtInt_jiange.Enabled = true;
            }
        }

        private void switchButton4_Toggled(object sender, EventArgs e)
        {
            if (switchButton4.IsOn == true)
            {
                txtInt_integerInput7.Enabled = true;
                txtInt_zhongjian.Enabled = false;
            }
            else
            {
                txtInt_integerInput7.Enabled = false;
                txtInt_zhongjian.Enabled = true;
            }
        }

        private void switchButton7_Toggled(object sender, EventArgs e)
        {
            if (switchButton7.IsOn)
            {
                buttonX7.Enabled = false;
            }
            else
            {
                buttonX7.Enabled = true;
            }
        }

        private void switchButton2_Toggled(object sender, EventArgs e)
        {
            if (switchButton2.IsOn == true)
            {
                if (Program.Level < 30)
                {
                    XtraMessageBox.Show( "您所使用的版本不支持【自动链轮】功能，请联系客服升级后使用哦！", "升级版本后才能使用哦！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    switchButton2.IsOn = false;
                    return;
                }
            }
        }

        private void iszidingyilink_Toggled(object sender, EventArgs e)
        {
            if (iszidingyilink.IsOn == true)
            {
                if (Program.Level < 30)
                {
                    XtraMessageBox.Show( "您所使用的版本不支持【自定义链接插入】功能，请联系客服升级后使用哦！", "升级版本后才能使用哦！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    iszidingyilink.IsOn = false;
                    return;
                }
            }
        }

        private void switchButton6_Toggled(object sender, EventArgs e)
        {
            if (switchButton6.IsOn == true)
            {
                if (Program.Level < 30)
                {
                    XtraMessageBox.Show( "您所使用的版本不支持【锚文本替换】功能，请联系客服升级后使用哦！", "升级版本后才能使用哦！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    switchButton6.IsOn = false;
                    return;
                }
            }
        }

        private void getmajiamodel_Toggled(object sender, EventArgs e)
        {
            if (getmajiamodel.IsOn)
            {
                txtmajia.Enabled = true;
                btnGetMajia.Enabled = true;
            }
            else
            {
                txtmajia.Enabled = false;
                btnGetMajia.Enabled = false;
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (Model.V3Infos.MainDb.MyModels.ContainsKey(Model.V3Infos.SendPointDb[model.PointId].PostModel))
            {
                System.Diagnostics.Process.Start(Model.V3Infos.MainDb.MyModels[ Model.V3Infos.SendPointDb[model.PointId].PostModel].PlanUrl);
            }
            else
            {
                XtraMessageBox.Show("请先修改站点并选择一个发布模块！", "未设置发布模块", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

  

        private void ListBox_plugin_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (ListBox_plugin.SelectedItems.Count > 0)
            {
                panel_plugin.Controls.Clear();
                ProcessPlugin p = (ProcessPlugin)ListBox_plugin.SelectedItems[0];
                panel_plugin.Controls.Add(ProcessPluginList[p.Id].MainControl);
                if (model.Plugins.Contains(p.Id))
                {
                    toggleSwitch_plugin.IsOn = true;
                }
                else
                {
                    toggleSwitch_plugin.IsOn = false;
                }
                if (model.Plugins_before.Contains(p.Id))
                {
                    toggleSwitch_UseType.IsOn = true;
                }
                else
                {
                    toggleSwitch_UseType.IsOn = false;
                }
                toggleSwitch_plugin_Toggled(null,null);
            }
        }

        private void toggleSwitch_plugin_Toggled(object sender, EventArgs e)
        {
            if (ListBox_plugin.SelectedItems.Count > 0)
            {
                ProcessPlugin p = (ProcessPlugin)ListBox_plugin.SelectedItems[0];
                if (toggleSwitch_plugin.IsOn)
                {
                    if (!model.Plugins.Contains(p.Id))
                    {
                        model.Plugins.Add(p.Id);
                     
                    }
                    if (!model.PluginParameters.ContainsKey(p.Id))
                    {
                        model.PluginParameters.Add(p.Id, p.Parameters);
                    }
                    else
                    {
                        
                        model.PluginParameters[p.Id] = p.Parameters;
                    }
                    panel_plugin.Enabled = true;
                }
                else
                {
                   model.Plugins.Remove(p.Id);
                   // model.PluginParameters.Remove(p.Id);
                    panel_plugin.Enabled = false;

                }
            }
        }

        private void switchButtonisuijilian_Toggled(object sender, EventArgs e)
        {
            txtInt_suijimin.Enabled = txtInt_suijimax.Enabled = switchButtonisuijilian.IsOn;
        }

        private void frmTask_FormClosed(object sender, FormClosedEventArgs e)
        {
            isShow = false;
        }

        private void toggleSwitch_UseType_Toggled(object sender, EventArgs e)
        {
            ProcessPlugin p = (ProcessPlugin)ListBox_plugin.SelectedItems[0];
            if (toggleSwitch_UseType.IsOn)
            {
                if (!model.Plugins_before.Contains(p.Id))
                {
                    model.Plugins_before.Add(p.Id);
                }
            }
            else
            {
                if (model.Plugins_before.Contains(p.Id))
                {
                    model.Plugins_before.Remove(p.Id);
                }
            }
        }

       
    }
}
