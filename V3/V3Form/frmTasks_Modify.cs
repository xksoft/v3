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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevExpress.XtraEditors;
using Model;

namespace V3.V3Form
{
    public partial class frmTasks_Modify : DevExpress.XtraEditors.XtraForm
    {
        public static frmTasks_Modify MyfrmTask;
        public frmTasks_Modify()
        {
            InitializeComponent();
            MyfrmTask = this;
           
        }
       
        public bool issave = false;
        public Model.Task model = new Model.Task();
        public int pointid = 0;
        #region 数据模型
        public static int ColumnCount = 4;
        public static RecordCollection Coll = new RecordCollection();
        public class RecordCollection : CollectionBase, IBindingList, ITypedList
        {
            public Record this[int i] { get { return (Record)List[i]; } }
            public void Add(Record record)
            {
                int res = List.Add(record);
                record.owner = this;
                record.Index = res;

            }
            public void SetValue(int row, int col, object val)
            {
                this[row].SetValue(col, val);
            }
            internal void OnListChanged(Record rec)
            {
                // if (listChangedHandler != null) listChangedHandler(this, new ListChangedEventArgs(ListChangedType.ItemChanged, rec.Index, rec.Index));
            }
            PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] accessors)
            {
                PropertyDescriptorCollection coll = TypeDescriptor.GetProperties(typeof(Record));
                ArrayList list = new ArrayList(coll);
                list.Sort(new PDComparer());
                PropertyDescriptorCollection res = new PropertyDescriptorCollection(null);
                for (int n = 0; n < ColumnCount; n++)
                {
                    res.Add(list[n] as PropertyDescriptor);
                }
                return res;
            }
            class PDComparer : IComparer
            {
                int IComparer.Compare(object a, object b)
                {
                    return Comparer.Default.Compare(GetName(a), GetName(b));
                }
                int GetName(object a)
                {
                    PropertyDescriptor pd = (PropertyDescriptor)a;
                    if (pd.Name.StartsWith("Column")) return Convert.ToInt32(pd.Name.Substring(6));
                    return -1;

                }
            }
            string ITypedList.GetListName(PropertyDescriptor[] accessors) { return ""; }
            public object AddNew() { return null; }
            public bool AllowEdit { get { return true; } }
            public bool AllowNew { get { return false; } }
            public bool AllowRemove { get { return false; } }

            private ListChangedEventHandler listChangedHandler;
            public event ListChangedEventHandler ListChanged
            {
                add { listChangedHandler += value; }
                remove { listChangedHandler -= value; }
            }
            public void AddIndex(PropertyDescriptor pd) { throw new NotSupportedException(); }
            public void ApplySort(PropertyDescriptor pd, ListSortDirection dir) { throw new NotSupportedException(); }
            public int Find(PropertyDescriptor property, object key) { throw new NotSupportedException(); }
            public bool IsSorted { get { return false; } }
            public void RemoveIndex(PropertyDescriptor pd) { throw new NotSupportedException(); }
            public void RemoveSort() { throw new NotSupportedException(); }
            public ListSortDirection SortDirection { get { throw new NotSupportedException(); } }
            public PropertyDescriptor SortProperty { get { throw new NotSupportedException(); } }
            public bool SupportsChangeNotification { get { return true; } }
            public bool SupportsSearching { get { return false; } }
            public bool SupportsSorting { get { return false; } }

        }
        void SetValue(object data, int row, int column, object val)
        {
            RecordCollection rc = data as RecordCollection;
            rc.SetValue(row, column, val);
        }
        public class Record
        {
            internal int Index = -1;
            internal RecordCollection owner;
            string[] values = new string[4];
            public int 任务编号 { get { return Convert.ToInt32(values[0]); } set { SetValue(0, value); } }
            public string 任务名称 { get { return values[1]; } set { SetValue(1, value); } }
            public string 所属站点 { get { return values[2]; } set { SetValue(2, value); } }
            public string 所属分组 { get { return values[3]; } set { SetValue(3, value); } }
            public string GetValue(int index) { return values[index]; }
            //<label1>
            public void SetValue(int index, object val)
            {
                values[index] = val.ToString();
                if (this.owner != null) this.owner.OnListChanged(this);
            }
            //</label1>
        }
        #endregion
        #region 任务列表多选
        //用于记录，鼠标是否已按下
        bool isMouseDown = false;

        //用于鼠标拖动多选，标记是否记录开始行
        bool isSetStartRow = false;

        //用于鼠标拖动多选，记录开始行
        private int StartRowHandle = -1;

        //用于鼠标拖动多选，记录现在行
        private int CurrentRowHandle = -1;

        //用于实现鼠标拖动选择多行功能中的一个方法，对单元格区域进行选中
        private void SelectRows(int startRow, int endRow)
        {
            if (startRow > -1 && endRow > -1)
            {

                gridControl_main_view.BeginSelection();
                gridControl_main_view.ClearSelection();
                gridControl_main_view.SelectRange(startRow, endRow);
                gridControl_main_view.EndSelection();


            }
        }

        #endregion
        void SetInfo()
        {
            int count = 0;
            for (int i = 0; i < gridControl_main_view.SelectedRowsCount; i++)
            {
                issave = true;
                int task_id = ((Record) gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[i])).任务编号;
                Model.Task oldTask = new Model.Task();
                oldTask = xEngine.Common.XSerializable.CloneObject<Model.Task>(V3Infos.TaskDb[task_id]);
                if (checkEdit_switchButton_YuLiao.Checked)
                {
                    V3Infos.TaskDb[task_id].IsUseKu = switchButton_YuLiao.IsOn;
                }

              
                if (checkEdit_switchButtonisuijilian.Checked)
                {
                    V3Infos.TaskDb[task_id].Issuijilian = switchButtonisuijilian.IsOn;
                }
                if (checkEdit_txtInt_suijimin.Checked)
                {
                    V3Infos.TaskDb[task_id].Suijimin = Convert.ToInt32(txtInt_suijimin.Value);
                    V3Infos.TaskDb[task_id].Suijimax = Convert.ToInt32(txtInt_suijimax.Value);
                }
               

                if (checkEdit_txtInt_maoxiangtong.Checked)
                {
                    V3Infos.TaskDb[task_id].Maoxiangtong = Convert.ToInt32(txtInt_maoxiangtong.Value);
                }
                if (checkEdit_txtmajia.Checked)
                {
                    V3Infos.TaskDb[task_id].Getmajia = txtmajia.Text;
                }
                if (checkEdit_getmajiamodel.Checked)
                {
                    V3Infos.TaskDb[task_id].Getmajiamodel = getmajiamodel.IsOn;
                }
                if (checkEdit_btn_SetType.Checked)
                {
                    V3Infos.TaskDb[task_id].Qianchuorhouchu = btn_SetType.IsOn;
                }
                if (checkEdit_isjianfan.Checked)
                {
                    V3Infos.TaskDb[task_id].Isjianfan = isjianfan.IsOn;
                }
                if (checkEdit_janfanfangshi.Checked)
                {
                    V3Infos.TaskDb[task_id].Jianfanfangshi = janfanfangshi.IsOn;
                }
                if (checkEdit_comboBoxtaskmoshi.Checked)
                {
                    V3Infos.TaskDb[task_id].TaskMoshi = comboBoxtaskmoshi.SelectedIndex;
                }
                if (checkEdit_formatsuojin.Checked)
                {
                    V3Infos.TaskDb[task_id].FormatSuojin = formatsuojin.IsOn;
                }
                if (checkEdit_formatpic.Checked)
                {
                    V3Infos.TaskDb[task_id].FormatPicURL = formatpic.IsOn;
                }
                if (checkEdit_formatlink.Checked)
                {
                    V3Infos.TaskDb[task_id].FormatLinkURL = formatlink.IsOn;
                }
                if (checkEdit_formathanggao.Checked)
                {
                    V3Infos.TaskDb[task_id].FormatHanggao = formathanggao.IsOn;
                }
                if (checkEdit_textBoxX9.Checked)
                {
                    V3Infos.TaskDb[task_id].Maostring = textBoxX9.Text;
                }
                if (checkEdit_repxiang.Checked)
                {
                    V3Infos.TaskDb[task_id].Isshuangxiang = repxiang.IsOn;
                }
                if (checkEdit_textBoxlianjiefus.Checked)
                {
                    V3Infos.TaskDb[task_id].Lianjiefus.Clear();
                    foreach (string s in textBoxlianjiefus.Lines)
                    {
                        if (!V3Infos.TaskDb[task_id].Lianjiefus.Contains(s) && s.Trim().Length > 0)
                        {
                            V3Infos.TaskDb[task_id].Lianjiefus.Add(s);
                        }
                    }
                   
                }
              
                if (checkEdit_iszidingyitou.Checked)
                {
                    V3Infos.TaskDb[task_id].Zidingtou = iszidingyitou.Checked;
                }
                if (checkEdit_iszidingyizhong.Checked)
                {
                    V3Infos.TaskDb[task_id].Zidingzhong = iszidingyizhong.Checked;
                }
                if (checkEdit_iszidingyiwei.Checked)
                {
                    V3Infos.TaskDb[task_id].Zidingwei = iszidingyiwei.Checked;
                }
                if (checkEdit_integerInput6.Checked)
                {
                    V3Infos.TaskDb[task_id].Zidingyitoushulian = Convert.ToInt32(integerInput6.Value);
                }
                if (checkEdit_txtInt_integerInput8.Checked)
                {
                    V3Infos.TaskDb[task_id].Zidingzhongshuliang = Convert.ToInt32(txtInt_integerInput8.Value);
                }
                if (checkEdit_txtInt_integerInput5.Checked)
                {
                    V3Infos.TaskDb[task_id].Zidingweishuliang = Convert.ToInt32(txtInt_integerInput5.Value);
                }
                if (checkEdit_jiangeorzhongshu.Checked)
                {
                    V3Infos.TaskDb[task_id].Zidingjiangemoshi = !jiangeorzhongshu.IsOn;
                }
                if (checkEdit_txtInt_jiange.Checked)
                {
                    V3Infos.TaskDb[task_id].Zidingzhongjiange = Convert.ToInt32(txtInt_jiange.Value);
                }
                if (checkEdit_textBoxX3.Checked)
                {
                    V3Infos.TaskDb[task_id].Zidingzhongstring = textBoxX3.Text;
                }
                if (checkEdit_textBoxX1.Checked)
                {
                    V3Infos.TaskDb[task_id].Zidingweistring = textBoxX1.Text;
                }
                if (checkEdit_textBoxX4.Checked)
                {
                    V3Infos.TaskDb[task_id].Zidingtoustring = textBoxX4.Text;
                }
                if (checkEdit_switchButton4.Checked)
                {
                    V3Infos.TaskDb[task_id].Lunjiangemoshi = switchButton4.IsOn;
                }
                if (checkEdit_txtInt_integerInput7.Checked)
                {
                    V3Infos.TaskDb[task_id].Lunzongshu = Convert.ToInt32(txtInt_integerInput7.Value);
                }
                if (checkEdit_switchButton2.Checked)
                {
                    V3Infos.TaskDb[task_id].IsLianLun = switchButton2.IsOn;
                }
                if (checkEdit_switchButton3.Checked)
                {
                    V3Infos.TaskDb[task_id].UserDefaultLinkDB = switchButton3.IsOn;
                }
                if (checkEdit_checkBoxX4.Checked)
                {
                    V3Infos.TaskDb[task_id].Isliantou = checkBoxX4.Checked;
                }
                if (checkEdit_txtInt_touzhuliang.Checked)
                {
                    V3Infos.TaskDb[task_id].Toushuliang = Convert.ToInt32(txtInt_touzhuliang.Value + 1);
                }
                if (checkEdit_textBoxXliantou.Checked)
                {
                    V3Infos.TaskDb[task_id].Toustring = textBoxXliantou.Text;
                }
                if (checkEdit_checkBoxX5.Checked)
                {
                    V3Infos.TaskDb[task_id].Islianzhong = checkBoxX5.Checked;
                }
                if (checkEdit_txtInt_zhongjian.Checked)
                {
                    V3Infos.TaskDb[task_id].Zhongjiange = Convert.ToInt32(txtInt_zhongjian.Value);
                }
                if (checkEdit_textBoxXlianzhong.Checked)
                {
                    V3Infos.TaskDb[task_id].Zhongstring = textBoxXlianzhong.Text;
                }
                if (checkEdit_checkBoxX6.Checked)
                {
                    V3Infos.TaskDb[task_id].Islianwei = checkBoxX6.Checked;
                }
                if (checkEdit_txtInt_weishuliang.Checked)
                {
                    V3Infos.TaskDb[task_id].Weishuliang = Convert.ToInt32(txtInt_weishuliang.Value);
                }
                if (checkEdit_textBoxXlianwei.Checked)
                {
                    V3Infos.TaskDb[task_id].Weistring = textBoxXlianwei.Text;
                }
                if (checkEdit_textBoxXtou.Checked)
                {
                    V3Infos.TaskDb[task_id].ArticleTou = textBoxXtou.Text;
                }
                if (checkEdit_textBoxXwei.Checked)
                {
                    V3Infos.TaskDb[task_id].ArticleWei = textBoxXwei.Text;
                }
                if (checkEdit_integerInputtongbuzhuaqujiange.Checked)
                {
                    V3Infos.TaskDb[task_id].Tongbu_get1_jiangetime = Convert.ToInt32(integerInputtongbuzhuaqujiange.Value);
                }
                if (checkEdit_integerInputzhuizongjiange.Checked)
                {
                    V3Infos.TaskDb[task_id].Tongbu_ZhiXingJianGe = Convert.ToInt32(integerInputzhuizongjiange.Value);
                }
                if (checkEdit_txtInt_pagenumber.Checked)
                {
                    V3Infos.TaskDb[task_id].Zidingyi_Totalpage = Convert.ToInt32(txtInt_pagenumber.Value);
                }
                if (checkEdit_integerInputzidingyimaxurl.Checked)
                {
                    V3Infos.TaskDb[task_id].Zidingyi_total_getnumber = Convert.ToInt32(integerInputzidingyimaxurl.Value);
                }
                if (checkEdit_integerInputzidingyijiange.Checked)
                {
                    V3Infos.TaskDb[task_id].zidingyi_Get1_jiangetime = Convert.ToInt32(integerInputzidingyijiange.Value);
                }
                if (checkEdit_txtInt_fabujiange.Checked)
                {
                    V3Infos.TaskDb[task_id].Fabujiange = Convert.ToInt32(txtInt_fabujiange.Value);
                }
                if (checkEdit_IsAutoTask.Checked)
                {
                    V3Infos.TaskDb[task_id].IsAutoTask = IsAutoTask.IsOn;
                }
                if (checkEdit_txtInt_PiCiJianGe.Checked)
                {
                    V3Infos.TaskDb[task_id].Jiangetime = Convert.ToInt32(txtInt_PiCiJianGe.Value);
                }
                if (checkEdit_isRunGet.Checked)
                {
                    V3Infos.TaskDb[task_id].Isrunget = isRunGet.IsOn;
                }
                if (checkEdit_txtInt_mintitlestr.Checked)
                {
                    V3Infos.TaskDb[task_id].MinTitlestr = Convert.ToInt32(txtInt_mintitlestr.Value);
                }
                if (checkEdit_txtInt_mincontentstr.Checked)
                {
                    V3Infos.TaskDb[task_id].MinContentstr = Convert.ToInt32(txtInt_mincontentstr.Value);
                }
                if (checkEdit_hashModel.Checked)
                {
                    if (checkBoxX1.Checked)
                        V3Infos.TaskDb[task_id].HashModel = 1;
                    else if (checkBoxX2.Checked)
                        V3Infos.TaskDb[task_id].HashModel = 2;
                    else
                        V3Infos.TaskDb[task_id].HashModel = 3;
                }
                if (checkEdit_postyou.Checked)
                {
                    if (s1.Checked)
                        V3Infos.TaskDb[task_id].DataGetmodel = 1;
                    else if (s2.Checked)
                        V3Infos.TaskDb[task_id].DataGetmodel = 2;
                    else
                        V3Infos.TaskDb[task_id].DataGetmodel = 3;
                }
                if (checkEdit_txtInt_get1_jiangetime.Checked)
                {
                    V3Infos.TaskDb[task_id].Get1_jiangetime = Convert.ToInt32(txtInt_get1_jiangetime.Value);
                }
                if (checkEdit_get1_jingdu.Checked)
                {
                    V3Infos.TaskDb[task_id].Get1_jingduModel = Convert.ToInt32(get1_jingdu.SelectedIndex);
                }
                if (checkEdit_txtInt_get1_MoreGetNumber.Checked)
                {
                    V3Infos.TaskDb[task_id].Get1_getnumber = Convert.ToInt32(txtInt_get1_MoreGetNumber.Value);
                }
                if (checkEdit_switchButton6.Checked)
                {
                    V3Infos.TaskDb[task_id].IsMaoTiHuan = switchButton6.IsOn;
                }
                if (checkEdit_txtInt_PiciNumber.Checked)
                {
                    V3Infos.TaskDb[task_id].Picinumber = Convert.ToInt32(txtInt_PiciNumber.Value);
                }
                if (checkEdit_IsRunSend.Checked)
                {
                    V3Infos.TaskDb[task_id].Isrunsend = IsRunSend.IsOn;
                }
                if (checkEdit_MoreAccountUseModel1.Checked)
                {
                    V3Infos.TaskDb[task_id].MoreAccountUseModel = MoreAccountUseModel1.IsOn;
                }
               
                if (checkEdit_isUseMakeHtml.Checked)
                {
                    V3Infos.TaskDb[task_id].IsUseMakeHtml = isUseMakeHtml.IsOn;
                }
                if (checkEdit_isUseLinkDb.Checked)
                {
                    V3Infos.TaskDb[task_id].IsUseLinkDb = isUseLinkDb.IsOn;
                }
                if (checkEdit_comboBoxmingan.Checked)
                {
                    V3Infos.TaskDb[task_id].Mingan = comboBoxmingan.SelectedItem.ToString();
                }
                if (checkEdit_switchButtonhtml.Checked)
                {
                    V3Infos.TaskDb[task_id].Iszhenggui = switchButtonhtml.IsOn;
                }
                if (checkEdit_switchButtonistihuan.Checked)
                {
                    V3Infos.TaskDb[task_id].Istihuan = switchButtonistihuan.IsOn;
                }
                if (checkEdit_switchButtonishunxiao.Checked)
                {
                    V3Infos.TaskDb[task_id].Ishunxiao = switchButtonishunxiao.IsOn;
                }
                if (checkEdit_comboBoxhunxiaofangshi.Checked)
                {
                    try
                    {
                        V3Infos.TaskDb[task_id].Hunxiaofangshi = comboBoxhunxiaofangshi.SelectedItem.ToString();

                    }
                    catch
                    {
                        V3Infos.TaskDb[task_id].Hunxiaofangshi = "内容混淆关键字模式";

                    }
                }
                if (checkEdit_textBoxhunxiaogeshi.Checked)
                {
                    V3Infos.TaskDb[task_id].Hunxiaogeshi = textBoxhunxiaogeshi.Text;
                }
                if (checkEdit_switchButtonjiangemoshi.Checked)
                {
                    V3Infos.TaskDb[task_id].Guanjiancimoshi = switchButtonjiangemoshi.IsOn;
                }
                if (checkEdit_switchButtonjiangemoshi.Checked)
                {
                    if (switchButtonjiangemoshi.IsOn == true)
                    {
                        if (checkEdit_txtInt_Xkeywordtotal.Checked)
                        {
                            V3Infos.TaskDb[task_id].Keywordtotal = Convert.ToInt32(txtInt_Xkeywordtotal.Text.Trim());
                            if (V3Infos.TaskDb[task_id].Keywordtotal <= 0)
                            {
                                V3Infos.TaskDb[task_id].Keywordtotal = 1;
                            }
                        }
                    }
                    else
                    {
                        if (checkEdit_txtInt_Xkeyjiange.Checked)
                        {
                            V3Infos.TaskDb[task_id].Keywordjiange = Convert.ToInt32(txtInt_Xkeyjiange.Text);
                            if (V3Infos.TaskDb[task_id].Keywordtotal <= 0)
                            {
                                V3Infos.TaskDb[task_id].Keywordtotal = 1;
                            }
                        }

                    }
                }
                if (checkEdit_switchButtonnoqq.Checked)
                {
                    V3Infos.TaskDb[task_id].NoQQ = switchButtonnoqq.IsOn;
                }
                if (checkEdit_switchButtonnoqq.Checked)
                {
                    V3Infos.TaskDb[task_id].NoPic = switchButtonnoPic.IsOn;
                }
                if (checkEdit_switchButtonnoPhone.Checked)
                {
                    V3Infos.TaskDb[task_id].NoPhone = switchButtonnoPhone.IsOn;
                }
                if (checkEdit_switchButtonnoemail.Checked)
                {
                    V3Infos.TaskDb[task_id].NoEmail = switchButtonnoemail.IsOn;
                }
                if (checkEdit_switchButtonnoA.Checked)
                {
                    V3Infos.TaskDb[task_id].NoA = switchButtonnoA.IsOn;
                }
                if (checkEdit_textBoxXMyqq.Checked)
                {
                    V3Infos.TaskDb[task_id].MyQQ = textBoxXMyqq.Text.Trim();
                }
                if (checkEdit_textBoxXMyPhone.Checked)
                {
                    V3Infos.TaskDb[task_id].MyPhone = textBoxXMyPhone.Text.Trim();
                }
                if (checkEdit_textBoxXMyemail.Checked)
                {
                    V3Infos.TaskDb[task_id].MyEmail = textBoxXMyemail.Text;
                }
                if (checkEdit_textBoxXMya.Checked)
                {
                    V3Infos.TaskDb[task_id].MyA = textBoxXMya.Text.Trim();
                }
                if (checkEdit_comboBoxExWeiDu.Checked)
                {
                    V3Infos.TaskDb[task_id].WeiyuanchuangDu = comboBoxExWeiDu.SelectedItem.ToString();
                }
                if (checkEdit_switchButtonW1.Checked)
                {

                    V3Infos.TaskDb[task_id].WeiyuanchuangXiang[0] = switchButtonW1.IsOn;
                }
                if (checkEdit_switchButtonW2.Checked)
                {
                    V3Infos.TaskDb[task_id].WeiyuanchuangXiang[1] = switchButtonW2.IsOn;
                }
                if (checkEdit_switchButtonW3.Checked)
                {
                    V3Infos.TaskDb[task_id].WeiyuanchuangXiang[2] = switchButtonW3.IsOn;
                }
                if (checkEdit_switchButtonW4.Checked)
                {
                    V3Infos.TaskDb[task_id].WeiyuanchuangXiang[3] = switchButtonW4.IsOn;
                }
                if (checkEdit_switchButtonW5.Checked)
                {
                    V3Infos.TaskDb[task_id].WeiyuanchuangXiang[4] = switchButtonW5.IsOn;
                }
                if (checkEdit_textBoxXTitle.Checked)
                {
                    V3Infos.TaskDb[task_id].WeiyuanchuangTitle = textBoxXTitle.Text;
                }
                if (checkEdit_textBoxXFanYiLiuCheng.Checked)
                {
                    V3Infos.TaskDb[task_id].Fanyiformate = textBoxXFanYiLiuCheng.Text.Trim();
                }
                if (checkEdit_switchButtonnourl.Checked)
                {
                    V3Infos.TaskDb[task_id].NoUrl = switchButtonnourl.IsOn;
                }
                if (checkEdit_textBoxXMyurl.Checked)
                {
                    V3Infos.TaskDb[task_id].MyUrl = textBoxXMyurl.Text.Trim();
                }
                if (checkEdit_switchButtontitleFanYi.Checked)
                {
                    V3Infos.TaskDb[task_id].IstitleFanYi = switchButtontitleFanYi.IsOn;
                }
                if (checkEdit_integerInput2.Checked)
                {
                    V3Infos.TaskDb[task_id].Spider_jiange = Convert.ToInt32(integerInput2.Value);
                }
                if (checkEdit_integerInput3.Checked)
                {
                    V3Infos.TaskDb[task_id].Spider_maxget = Convert.ToInt32(integerInput3.Value);
                }
                if (checkEdit_txtInt_PiCiShu.Checked)
                {
                    V3Infos.TaskDb[task_id].GetPiciNumber = Convert.ToInt32(txtInt_PiCiShu.Value);
                }
                if (checkEdit_switchButton1.Checked)
                {
                    V3Infos.TaskDb[task_id].GetRunModel = switchButton1.IsOn;
                }
                if (checkEdit_switchButton7.Checked)
                {
                    V3Infos.TaskDb[task_id].MaouseDefaultlinkDb = switchButton7.IsOn;
                }
                if (checkEdit_txtInt_integerInput12.Checked)
                {
                    V3Infos.TaskDb[task_id].Maoshulian = Convert.ToInt32(txtInt_integerInput12.Value);

                }
                if (checkEdit_txtrukou.Checked)
                {


                    List<string> bianliang = new List<string>();
                    string[] bl = txtrukou.Text.Trim().Replace("\r\n", "`").Split('`');
                    for (int c = 0; c < bl.Length; c++)
                    {
                        if (bl[c].Length > 0)
                            bianliang.Add(bl[c]);
                    } V3Infos.TaskDb[task_id].RukouUrl = bianliang;}

                if (model.IsUseKu == true)
                {
                    if (!System.IO.File.Exists(txtGetModelName.Text + "\\segments.gen"))
                    {
                        MessageBox.Show("语料库路径无效！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        issave = false;
                        return;
                    }
                    if (checkEdit_txtInt_yuliaoyingshen.Checked)
                    {
                        V3Infos.TaskDb[task_id].YuLiaoYingShen = Convert.ToInt32(txtInt_yuliaoyingshen.Value);
                    }
                    if (checkEdit_integerInput10.Checked)
                    {
                        V3Infos.TaskDb[task_id].YuLiaoMax1 = Convert.ToInt32(integerInput10.Value);
                    }
                    if (checkEdit_switchButtonyuliaomoshi.Checked)
                    {
                        V3Infos.TaskDb[task_id].YulLiaoMoShi = switchButtonyuliaomoshi.IsOn;
                    }
                    if (checkEdit_txtInt_integerInputshuliang.Checked)
                    {
                        V3Infos.TaskDb[task_id].YuJuShuLiang = Convert.ToInt32(txtInt_integerInputshuliang.Value);
                    }
                    if (checkEdit_txtGetModelName.Checked)
                    {
                        V3Infos.TaskDb[task_id].YuLiaoKuPath = txtGetModelName.Text;
                    }
                }
                if (checkEdit_iszidingyilink.Checked)
                {
                    V3Infos.TaskDb[task_id].IszidingLink = iszidingyilink.IsOn;
                }
                if (checkEdit_textBoxX5.Checked)
                {
                    if (iszidingyilink.IsOn == true &&
                        !Model.V3Infos.LinkDb.ContainsKey(model.ZidingLinkDb.ToString()))
                    {
                        textBoxX5.Text = "未设置或已删除！";
                        MessageBox.Show("请先选择一个自定义链接库！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        issave = false;
                        return;
                    }
                    V3Infos.TaskDb[task_id].ZidingLinkDb = model.ZidingLinkDb;
                }
                if (checkEdit_switchButton2.Checked && checkEdit_switchButton3.Checked)
                {
                    if (switchButton2.IsOn == true && switchButton3.IsOn == false) //使用默认库 
                    {
                        //if (Model.V3Infos.LinkDb.ContainsKey(model.LinkDbId.ToString()))
                        //{
                        //    txtLinkName.Text = "(" + Model.V3Infos.LinkDb[V3Infos.TaskDb[task_id].LinkDbId.ToString()].Links.Count + ")" +
                        //                       "[" + Model.V3Infos.LinkDb[V3Infos.TaskDb[task_id].LinkDbId.ToString()].Id + "]" +
                        //                       Model.V3Infos.LinkDb[V3Infos.TaskDb[task_id].LinkDbId.ToString()].Name;
                        //    V3Infos.TaskDb[task_id].LinkDbId = model.LinkDbId;
                        //}
                        //else
                        //{
                        //    issave = false;
                        //    txtLinkName.Text = "未设置或已删除！";
                        //    MessageBox.Show("链轮设置了使用任务默认链接库，请先选择一个默认链接库！", "无法继续", MessageBoxButtons.OK,
                        //        MessageBoxIcon.Information);
                        //    issave = false;
                        //    return;
                        //}
                       
                    }
                    if (switchButton2.IsOn == true && switchButton3.IsOn == true) //使用指定库 
                    {
                        if (Model.V3Infos.LinkDb.ContainsKey(model.OtherLinkDB.ToString()))
                        {

                            textBoxX2.Text = "(" + Model.V3Infos.LinkDb[V3Infos.TaskDb[task_id].OtherLinkDB.ToString()].Links.Count + ")" +
                                             "[" + Model.V3Infos.LinkDb[V3Infos.TaskDb[task_id].OtherLinkDB.ToString()].Id + "]" +
                                             Model.V3Infos.LinkDb[V3Infos.TaskDb[task_id].OtherLinkDB.ToString()].Name;
                            V3Infos.TaskDb[task_id].OtherLinkDB = model.OtherLinkDB;
                        }
                        else
                        {

                            textBoxX2.Text = "未设置或已删除！";
                            MessageBox.Show("链轮设置了使用自定义链接库，请指定要使用的自定义链接库！", "无法继续", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                            issave = false;
                            return;
                        }
                    }
                }


                if (V3Infos.TaskDb[task_id].IsMaoTiHuan == true)
                {
                    if (V3Infos.TaskDb[task_id].MaouseDefaultlinkDb == true) //使用默认库
                    {
                        //if (!Model.V3Infos.LinkDb.ContainsKey(V3Infos.TaskDb[task_id].LinkDbId.ToString()))
                        //{
                        //    issave = false;
                        //    XtraMessageBox.Show("混淆设置为锚文本但是任务没有链接库！请选择一个链接库！", "无法继续", MessageBoxButtons.OK,
                        //        MessageBoxIcon.Information);
                        //    return;

                        //}
                        //else { V3Infos.TaskDb[task_id].LinkDbId}

                    }
                    else //使用自定义链接库 
                    {
                        if (!Model.V3Infos.LinkDb.ContainsKey(V3Infos.TaskDb[task_id].MaolinkDbId.ToString()))
                        {
                            issave = false;
                            XtraMessageBox.Show("混淆设置为锚文本但是没有选择自定义链接库！", "无法继续", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                            return;

                        }
                    }


                }

                if (checkEdit_txtArticleName.Checked)
                {
                    if (Model.V3Infos.ArticleDb.ContainsKey(model.ArticleDbId.ToString()))
                    {
                        txtArticleName.Text = "(" + Model.V3Infos.ArticleDb[V3Infos.TaskDb[task_id].ArticleDbId.ToString()].DataCount + ")" +
                                              "[" + Model.V3Infos.ArticleDb[V3Infos.TaskDb[task_id].ArticleDbId.ToString()].Id + "]" +
                                              Model.V3Infos.ArticleDb[V3Infos.TaskDb[task_id].ArticleDbId.ToString()].Name;
                        V3Infos.TaskDb[task_id].ArticleDbId = model.ArticleDbId;
                    }
                    else
                    {

                        issave = false;
                        txtArticleName.Text = "未设置或已删除！";
                        XtraMessageBox.Show("未设置文章库！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;

                    }
                }

                if (checkEdit_txtKeywordName.Checked)
                {
                    if (Model.V3Infos.KeywordDb.ContainsKey(model.KeywordDbId.ToString()))
                    {
                        txtKeywordName.Text = "(" +
                                              Model.V3Infos.KeywordDb[V3Infos.TaskDb[task_id].KeywordDbId.ToString()].Keywords.Count +
                                              ")" + "[" + Model.V3Infos.KeywordDb[V3Infos.TaskDb[task_id].KeywordDbId.ToString()].Id +
                                              "]" +
                                              Model.V3Infos.KeywordDb[V3Infos.TaskDb[task_id].KeywordDbId.ToString()].Name;
                        V3Infos.TaskDb[task_id].KeywordDbId = model.KeywordDbId;
                    }
                    else
                    {
                        issave = false;
                        txtKeywordName.Text = "未设置或已删除！";
                        XtraMessageBox.Show("未设置关键词库！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (checkEdit_txtHashName.Checked)
                {
                    if (Model.V3Infos.HashDb.ContainsKey(model.HashDbId.ToString()))
                    {
                        txtHashName.Text = "(" + Model.V3Infos.HashDb[V3Infos.TaskDb[task_id].HashDbId.ToString()].DataCount + ")" + "[" +
                                           Model.V3Infos.HashDb[V3Infos.TaskDb[task_id].HashDbId.ToString()].Id + "]" +
                                           Model.V3Infos.HashDb[V3Infos.TaskDb[task_id].HashDbId.ToString()].Name;
                        V3Infos.TaskDb[task_id].HashDbId = model.HashDbId;
                    }
                    else
                    {
                        issave = false;
                        txtHashName.Text = "未设置或已删除！";
                        XtraMessageBox.Show("未设置哈希库！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                if (checkEdit_txtReplaceName.Checked)
                {
                    if (Model.V3Infos.ReplaceDb.ContainsKey(model.ReplaceDbId.ToString()))
                    {
                        txtReplaceName.Text = "(" + Model.V3Infos.ReplaceDb[V3Infos.TaskDb[task_id].ReplaceDbId.ToString()].Words.Count +
                                              ")" + "[" + Model.V3Infos.ReplaceDb[V3Infos.TaskDb[task_id].ReplaceDbId.ToString()].Id +
                                              "]" +
                                              Model.V3Infos.ReplaceDb[V3Infos.TaskDb[task_id].ReplaceDbId.ToString()].Name;
                        V3Infos.TaskDb[task_id].ReplaceDbId = model.ReplaceDbId;
                    }
                    else
                    {
                        issave = false;
                        txtReplaceName.Text = "未设置或已删除！";
                        XtraMessageBox.Show("未设置替换库！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                if (checkEdit_txtLinkName.Checked)
                {
                    if (Model.V3Infos.LinkDb.ContainsKey(model.LinkDbId.ToString()))
                    {
                        txtLinkName.Text = "(" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Links.Count + ")" +
                                           "[" +
                                           Model.V3Infos.LinkDb[V3Infos.TaskDb[task_id].LinkDbId.ToString()].Id + "]" +
                                           Model.V3Infos.LinkDb[V3Infos.TaskDb[task_id].LinkDbId.ToString()].Name;
                        V3Infos.TaskDb[task_id].LinkDbId = model.LinkDbId;
                    }
                    else
                    {
                        issave = false;
                        txtLinkName.Text = "未设置或已删除！";
                        XtraMessageBox.Show("未设置链接库！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;

                    }
                }
                if (checkEdit_txtLinkName.Checked)
                {
                    if (Model.V3Infos.LinkDb.ContainsKey(model.OtherLinkDB.ToString()))
                    {


                        textBoxX2.Text = "(" + Model.V3Infos.LinkDb[V3Infos.TaskDb[task_id].OtherLinkDB.ToString()].Links.Count + ")" +
                                         "[" +
                                         Model.V3Infos.LinkDb[V3Infos.TaskDb[task_id].OtherLinkDB.ToString()].Id + "]" +
                                         Model.V3Infos.LinkDb[V3Infos.TaskDb[task_id].OtherLinkDB.ToString()].Name;
                        V3Infos.TaskDb[task_id].OtherLinkDB = model.OtherLinkDB;
                    }
                    else if (switchButton3.IsOn == true &&
                             !Model.V3Infos.LinkDb.ContainsKey(V3Infos.TaskDb[task_id].OtherLinkDB.ToString()))
                    {
                        issave = false;
                        textBoxX2.Text = "未设置或已删除！";
                        XtraMessageBox.Show("链轮未设置链接库！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                if (checkEdit_txtGetModelName.Checked)
                { 
                    if (Model.V3Infos.MainDb.MyModels.ContainsKey(model.GetModel))
                    {
                       
                        txtGetModelName.Text = "[" + Model.V3Infos.MainDb.MyModels[V3Infos.TaskDb[task_id].GetModel].mids + "]" +
                                               Model.V3Infos.MainDb.MyModels[V3Infos.TaskDb[task_id].GetModel].PlanName;
                        V3Infos.TaskDb[task_id].GetModel = model.GetModel;
                    }
                    else
                    {
                        issave = false;
                        txtGetModelName.Text = "未选择抓取模块！";
                        XtraMessageBox.Show("未设置抓取模块！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                if (checkEdit_txtrukou.Checked)
                {
                    if (Model.V3Infos.MainDb.MyModels[V3Infos.TaskDb[task_id].GetModel].IsuseTaskRukou && V3Infos.TaskDb[task_id].RukouUrl.Count < 1)
                    {
                        issave = false;
                        XtraMessageBox.Show("您选择的抓取模块需要在任务中指定入口地址，否则无法抓取！\r\n\r\n提示：入口在抓取参数2中哦", "无法继续",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                if (issave==false)
                {
                    V3Infos.TaskDb[task_id] = xEngine.Common.XSerializable.CloneObject<Model.Task>(oldTask);
                    break;
                }
                count++;
                V3.Bll.TaskBll.SaveTask(task_id);
            }

            XtraMessageBox.Show("成功修改"+count+"个任务！", "修改完毕", MessageBoxButtons.OK, MessageBoxIcon.Information);



        }
        void GetInfo()
        {
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

                switchButtonW1.IsOn = model.WeiyuanchuangXiang[0];
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

            switchButton_YuLiao.IsOn = model.IsUseKu;
            if (model.IsUseKu == true && !System.IO.File.Exists(model.YuLiaoKuPath + "\\segments.gen"))
            {
                MessageBox.Show("语料库路径无效！", "建议重新选择", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
        void checkdingzhi()
        {
            //文章末尾插入文章功能
           
                wenzhangmoweicharu.Visible = false;
                buttonsettags.Visible = false;
                setart.Visible = false;
            

        }

        public void LoadTaskList()
        {
            Coll.Clear();
            foreach (var t in V3Infos.TaskDb)
            {
                Record r=new Record();
                r.任务编号 = t.Value.id;
                r.任务名称 = t.Value.TaskName;
                if (comboBox_Group.SelectedItem.ToString() == "全部分组")
                {
                    string group = V3Infos.SendPointDb[t.Value.PointId].GroupTag;
                    if (group == "DefaultGroup" || !Model.V3Infos.MainDb.GroupList.ContainsKey(group))
                    {
                        r.所属分组 = "默认分组";
                    }
                    else
                    {
                        r.所属分组 = Model.V3Infos.MainDb.GroupList[V3Infos.SendPointDb[t.Value.PointId].GroupTag].ToString();
                    }

                    r.所属站点 = V3Infos.SendPointDb[t.Value.PointId].name;
                    Coll.Add(r);
                }
                else
                {
                    string group = V3Infos.SendPointDb[t.Value.PointId].GroupTag;
                    if (group == "DefaultGroup" || !Model.V3Infos.MainDb.GroupList.ContainsKey(group))
                    {
                        r.所属分组 = "默认分组";
                    }
                    else
                    {
                        r.所属分组 = Model.V3Infos.MainDb.GroupList[V3Infos.SendPointDb[t.Value.PointId].GroupTag].ToString();
                    }
                    r.所属站点 = V3Infos.SendPointDb[t.Value.PointId].name;
                    if (r.所属分组 == comboBox_Group.SelectedItem.ToString())
                    {
                        Coll.Add(r);
                    }

                }
                
            }
            gridControl_main.DataSource = Coll;
            gridControl_main_view.RefreshData();
            gridControl_main_view.ClearSelection();
            
        }



        private void TasKFrm_Load(object sender, EventArgs e)
        {
            //加载所有分组信息
             comboBox_Group.Properties.Items.Clear();
            comboBox_Group.Properties.Items.Add("全部分组");
            comboBox_Group.Properties.Items.Add("默认分组");
            foreach (KeyValuePair<string, string> value in Model.V3Infos.MainDb.GroupList)
            {

                comboBox_Group.Properties.Items.AddRange(new object[] { value.Value });

            } comboBox_Group.SelectedIndex = 0;
            LoadTaskList();
            pointid = model.PointId;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            checkdingzhi();
            GetInfo();

        }

     

        #region 任务信息
        private void btnSelectArticle_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 0;
            frm.Text = "请选择或新建一个文章库[[[请双击选择您要使用的库，注意了，是“双击”]]]";
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
            frm.Text = "请选择或新建一个关键词库[[[请双击选择您要使用的库，注意了，是“双击”]]]";
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
            frm.Text = "请选择或新建一个哈希库[[[请双击选择您要使用的库，注意了，是“双击”]]]";
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
            frm.Text = "请选择或新建一个替换库[[[请双击选择您要使用的库，注意了，是“双击”]]]";
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
            frm.Text = "请选择或新建一个链接库[[[请双击选择您要使用的库，注意了，是“双击”]]]";
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
            int id = V3.Bll.DbBll.AddDb(1, Model.V3Infos.SendPointDb[model.PointId].name + "-New文章库", Model.V3Infos.SendPointDb[model.PointId].GroupTag);
            if (id != 0)
            {
                model.ArticleDbId = id;
                txtArticleName.Text = "(" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Id + "]" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Name;
            }
        }

        private void btnAddKeyword_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(2, Model.V3Infos.SendPointDb[model.PointId].name + "-New关键词库", Model.V3Infos.SendPointDb[model.PointId].GroupTag);
            if (id != 0)
            {
                model.KeywordDbId = id;
                txtKeywordName.Text = "(" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Keywords.Count + ")" + "[" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Id + "]" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Name;
            }
        }

        private void btnAddhash_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(3, Model.V3Infos.SendPointDb[model.PointId].name + "-New哈希库", Model.V3Infos.SendPointDb[model.PointId].GroupTag);
            if (id != 0)
            {
                model.HashDbId = id;
                txtHashName.Text = "(" + Model.V3Infos.HashDb[model.HashDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Id + "]" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Name;
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(4, Model.V3Infos.SendPointDb[model.PointId].name + "-New替换库", Model.V3Infos.SendPointDb[model.PointId].GroupTag);
            if (id != 0)
            {
                model.ReplaceDbId = id;
                txtReplaceName.Text = "(" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Words.Count + ")" + "[" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Id + "]" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Name;
            }
        }

        private void btnAddLink_Click(object sender, EventArgs e)
        {
            int id = V3.Bll.DbBll.AddDb(5, Model.V3Infos.SendPointDb[model.PointId].name + "-New链接库", Model.V3Infos.SendPointDb[model.PointId].GroupTag);
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
        private void b_Click(object sender, EventArgs e)
        {
           
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
           
        }

        private void integerInput1_ValueChanged(object sender, EventArgs e)
        {
            PiciNumber_ValueChanged(null,null);
        }

        private void IsAutoTask_ValueChanged(object sender, EventArgs e)
        {
           
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
                MessageBox.Show("请先选择一个抓取模块！", "木有选择抓取模块", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSelectGetModel_Click(object sender, EventArgs e)
        {
            if (switchButton_YuLiao.IsOn == false)
            {


                V3Form.frmModelShop frm = new frmModelShop();
                frm.selectmodel = 1;
                frm.Text = "V3模块市场 [[[请在“我的模块”中双击选择一个您要使用的“抓取模块”，注意啦！是“双击”哦]]]";frm.ShowDialog();
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
        //所给路径中所对应的文件大小  

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
                MessageBox.Show("请先选择一个文章库！", "木有选择文章库", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("请先选择一个关键词库！", "木有选择关键词库", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtGetModelName_TextChanged(object sender, EventArgs e)
        {
            if (!Model.V3Infos.MainDb.MyModels.ContainsKey(model.GetModel))
            {
                return;
            }
            try
            {
                if (switchButton_YuLiao.IsOn == true)
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
                XtraMessageBox.Show( "请去站点属性里面选择发布模块！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void switchButtonjiangemoshi_ValueChanged_1(object sender, EventArgs e)
        {
           
        }

        private void buttonX1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!Model.V3Infos.MainDb.MyModels.ContainsKey(model.GetModel) || !Model.V3Infos.TaskDb.ContainsKey(model.id)) 
                {
                    XtraMessageBox.Show( "蜘蛛使用抓取模块或者任务ID有误，清空失败！可以通过换一个hash库实现同样效果！", "清空失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            Model.V3Infos.MainDb.Spiderque[model.GetModel + "-" + Model.V3Infos.TaskDb[model.id].HashDbId].Clear();
            XtraMessageBox.Show( "清空完成，请注意也顺道清空一下哈词库或者换一个，否则还是不会重新爬行的！", "清空完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception err) { XtraMessageBox.Show( err.Message, "清空失败", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void buttonItemdefaultfabu_Click(object sender, EventArgs e)//设为默认任务模板
        {
                   }

        private void buttonItem1_Click(object sender, EventArgs e)//使用默认任务模板
        {
           
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

        private void switchButton3_ValueChanged_1(object sender, EventArgs e)
        {
          
        }

        private void buttonX5_Click_1(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 4;
            frm.Text = "请选择或新建一个链接库[[[请双击选择您要使用的库，注意了，是“双击”]]]";
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
            frm.Text = "请选择或新建一个链接库[[[请双击选择您要使用的库，注意了，是“双击”]]]";
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

        private void jiangeorzhongshu_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void switchButton4_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void linkLabel24_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBoxXTitle.Text += "[随机连接符]";
        }




        private void buttonX7_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 4;
            frm.Text = "请选择或新建一个链接库[[[请双击选择您要使用的库，注意了，是“双击”]]]";
            frm.isselectdb = true;

            frm.ShowDialog();

            if (frm.selectedDBid != 0)
            {
                model.MaouseDefaultlinkDb = false;
                model.MaolinkDbId = frm.selectedDBid;
                textBoxX10.Text = "(" + Model.V3Infos.LinkDb[model.MaolinkDbId.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.MaolinkDbId.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.MaolinkDbId.ToString()].Name;
            }
        }

        private void switchButton7_ValueChanged(object sender, EventArgs e)
        {
          
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
                if ((1440 / txtInt_PiCiJianGe.Value).ToString() == "0")
                {
                    labelX29.Text = "任务在一天内会执行1次，每次发布" + txtInt_PiciNumber.Value + "篇，24小时内约发布" +
                                    txtInt_PiciNumber.Value + "篇";
                }
                else
                {
                    labelX29.Text = "任务在一天内会执行" + Convert.ToInt32(1440 / txtInt_PiCiJianGe.Value).ToString() + "次，每次发布" +
                                    txtInt_PiciNumber.Value + "篇，一天内约发布" +
                                    txtInt_PiciNumber.Value * (1440 / txtInt_PiCiJianGe.Value) + "篇";
                }
            }
            else
            {
                labelX29.Text = "任务每次运行会发布" + txtInt_PiciNumber.Value + "篇";
            }
        }

        private void switchButton5_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void buttonItem2_Click(object sender, EventArgs e)//测试最终发布效果
        {

           
        }

        private void switchButton2_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void iszidingyilink_ValueChanged(object sender, EventArgs e)
        {
          
        }

        private void switchButton6_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void textBoxXtou_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (Program.f_frmReplaceTag == null || Program.f_frmReplaceTag.IsDisposed )
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
                if (Program.f_frmReplaceTag == null || Program.f_frmReplaceTag.IsDisposed )
                {
                    Program.f_frmReplaceTag = new frmReplaceTag();
                }
                Program.f_frmReplaceTag.referer = "frmTask_textBoxXwei";
                Program.f_frmReplaceTag.Focus();
                Program.f_frmReplaceTag.Show();
                Program.f_frmReplaceTag.Location = new Point(0, 0);
            }


        }

        private void switchButton11_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void btnGetMajia_Click(object sender, EventArgs e)
        {
            //showI("正在提取马甲");
            //System.Threading.Thread t = new System.Threading.Thread(GetMajia);
            //t.IsBackground = true;
            //t.Start();
            frmGetCookie f = new frmGetCookie();
            f.ShowDialog();
            if (f.GetSuccess)
            {
                txtmajia.Text = f.CookieStr;
                SetInfo();
            }
        }
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
            this.Invoke((EventHandler)(delegate
            {
                ipanel.Visible = false;
            }));
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
                    if (gonggongkustr.Split('|').Length >=2)
                    {
                        result = gonggongkustr;
                       
                    }
                }
                 if (xEngine.License.MyLicense.Custom.Contains("settags"))
                {
                    string gonggongkustr = geregex(model.Canshu, "(?<=<settags>).*?(?=</settags>)", 1, false);
                    if (gonggongkustr.Split('|').Length >= 2)
                    {
                        result= gonggongkustr;

                    }
                }
                 if (xEngine.License.MyLicense.Custom.Contains("kan"))
                 {
                     string gonggongkustr = geregex(model.Canshu, "(?<=<modifytags>).*?(?=</modifytags>)", 1, false);
                     if (gonggongkustr.Split('|').Length >= 2)
                     {
                         result= gonggongkustr;

                     }
                 }
                 
            }
            catch { }
            return result;

        }
        #endregion
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
            //    Model.V3Infos.MainDb.DefaultTasks.Lianjiefus.Add(s);
            //}
            
            //Model.V3Infos.MainDb.DefaultTasks.WeiyuanchuangXiang = (bool[])model.WeiyuanchuangXiang.Clone();


            //XtraMessageBox.Show( "成功设为默认模板！", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btn_usedefault_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (Model.V3Infos.MainDb.DefaultTasks.TaskName == null || Model.V3Infos.MainDb.DefaultTasks.TaskName.Length == 0)
            //{ XtraMessageBox.Show( "当前系统没有默认任务模板！", "操作终止", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            //else
            //{
            //    Model.V3Infos.MainDb.DefaultTasks.id = model.id;
            //    model = xEngine.Common.XSerializable.CloneObject<Model.Task>(Model.V3Infos.MainDb.DefaultTasks);
            //    model.Lianjiefus=new List<string>();
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
            t.Show();
        }

        private void frmTask_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed )
            {
                Program.f_frmReplaceTag.Close();
            }
        }

        private void textBoxXtou_MouseUp(object sender, MouseEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed )
            {
                Program.f_frmReplaceTag.referer = "frmTask_textBoxXtou";
            }
        }

        private void textBoxXwei_MouseUp(object sender, MouseEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed )
            {
                Program.f_frmReplaceTag.referer = "frmTask_textBoxXwei";
            }
        }

        private void switchButton5_Toggled(object sender, EventArgs e)
        {
            if (switchButton_YuLiao.IsOn == true)
            {
                if (Program.Level < 120)
                {
                    XtraMessageBox.Show( "您所使用的版本不支持【语料库】功能，请联系客服升级后使用哦！", "升级版本后才能使用哦！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    switchButton_YuLiao.IsOn = false;
                    return;
                }
            }
            if (switchButton_YuLiao.IsOn == true)
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

        private void comboBox_Group_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTaskList();
        }

        private void gridControl_main_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
            }
        }

        private void gridControl_main_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo info = gridControl_main_view.CalcHitInfo(e.X, e.Y);
                //如果鼠标落在单元格里
                if (info.InRow)
                {
                    if (!isSetStartRow)
                    {
                        StartRowHandle = info.RowHandle;
                        isSetStartRow = true;
                    }
                    else
                    {
                        //获得当前的单元格
                        int newRowHandle = info.RowHandle;
                        if (CurrentRowHandle != newRowHandle)
                        {
                            CurrentRowHandle = newRowHandle;
                            //选定 区域 单元格
                            SelectRows(StartRowHandle, CurrentRowHandle);

                        }
                    }
                }
                label_selectcount.Text = "已选中"+gridControl_main_view.SelectedRowsCount+"个任务";
            }
        }

        private void gridControl_main_MouseUp(object sender, MouseEventArgs e)
        {
            StartRowHandle = -1;
            CurrentRowHandle = -1;
            isMouseDown = false;
            isSetStartRow = false;
        }

     
      
    }
}
