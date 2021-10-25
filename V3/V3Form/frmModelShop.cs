using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Collections;
using System.Linq;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using Model;
using xEngine.Common;
using xEngine.Execute;
using xEngine.Model.Execute.Http;
using xEngine.Model;
namespace V3.V3Form
{
    public partial class frmModelShop : DevExpress.XtraEditors.XtraForm
    {
        public int selectmodel = 0;
        public String selectedid = "";
       

        public frmModelShop()
        {
            InitializeComponent();
        }
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
        #region 数据模型

        public string TabTag = "my";
        public static int ColumnCount = 8;
        public static RecordCollection ModuleColl = new RecordCollection();
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
            object[] values = new object[8];
            public String 模块编号 { get { return values[0].ToString(); } set { SetValue(0, value); } }
            public string 模块名称 { get { return values[1].ToString(); } set { SetValue(1, value); } }


            public Image 模块类别图片
            {
                get
                {
                    return (Image)values[2];
                    return null;
                }
                set
                {
                    SetValue(2, value);
                }
            }

            public DateTime 更新时间 { get { return DateTime.Parse( values[3].ToString()); } set { SetValue(3, value); } }
            public string 共享积分 { get { return values[4].ToString(); } set { SetValue(4, value); } }
            public int 人气指数 { get { return Convert.ToInt32(values[5]); } set { SetValue(5, value); } }
            public string 作者 { get { return values[6].ToString(); } set { SetValue(6, value); } }
            public string 获取 { get { return values[7].ToString(); } set { SetValue(7, value); } }
            public string GetValue(int index) { return values[index].ToString(); }
            //<label1>
            public void SetValue(int index, object val)
            {
                values[index] = val;
                if (this.owner != null) this.owner.OnListChanged(this);
            }
            //</label1>
        }
        #endregion
        public void ShowList() 
        { 
            this.Invoke((EventHandler)(delegate
            {
               
          
                gridControl_main.DataSource = ModuleColl;
                
                gridControl_main_view.Columns["模块编号"].Width = 60;
                gridControl_main_view.Columns["模块名称"].MinWidth = 190;
                gridControl_main_view.Columns["人气指数"].Width = 60;
                gridControl_main_view.Columns["共享积分"].Width = 60;
                gridControl_main_view.Columns["更新时间"].Width = 120;
                gridControl_main_view.Columns["模块类别图片"].Width = 110;
                gridControl_main_view.Columns["获取"].Width = 100;


                gridControl_main_view.Columns["模块类别图片"].VisibleIndex = 0;
                gridControl_main_view.Columns["模块编号"].VisibleIndex = 1;
                gridControl_main_view.Columns["模块名称"].VisibleIndex = 2;
                gridControl_main_view.Columns["更新时间"].VisibleIndex = 3;
                gridControl_main_view.Columns["共享积分"].VisibleIndex = 4;
                gridControl_main_view.Columns["人气指数"].VisibleIndex = 5;
                gridControl_main_view.Columns["作者"].VisibleIndex = 6;
                gridControl_main_view.Columns["获取"].VisibleIndex = 7;


                gridControl_main_view.OptionsBehavior.Editable = true;

                RepositoryItemButtonEdit btn_GetModule = new RepositoryItemButtonEdit();
                btn_GetModule.TextEditStyle = TextEditStyles.HideTextEditor;
                btn_GetModule.Buttons[0].Kind = ButtonPredefines.Glyph;
                btn_GetModule.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btn_GetModule_Click);//时间委托
                if (TabTag == "my")
                {
                    btn_GetModule.Buttons[0].Caption = "编辑模块";
                }
                else
                {
                    btn_GetModule.Buttons[0].Caption = "获取模块";
                }

                btn_GetModule.Buttons[0].Visible = true;
                gridControl_main_view.Columns["获取"].ColumnEdit = btn_GetModule;
                gridControl_main_view.Columns["获取"].OptionsColumn.AllowEdit = true;
                gridControl_main_view.Columns["模块编号"].OptionsColumn.AllowEdit = false;
                gridControl_main_view.Columns["模块名称"].OptionsColumn.AllowEdit = false;
                gridControl_main_view.Columns["模块类别图片"].OptionsColumn.AllowEdit = false;
                gridControl_main_view.Columns["更新时间"].OptionsColumn.AllowEdit = false;
                gridControl_main_view.Columns["共享积分"].OptionsColumn.AllowEdit = false;
                gridControl_main_view.Columns["人气指数"].OptionsColumn.AllowEdit = false;
                gridControl_main_view.Columns["作者"].OptionsColumn.AllowEdit = false;

                if (TabTag == "my")
                {
                    gridControl_main_view.Columns["人气指数"].Visible = false;
                }
                else
                {
                    gridControl_main_view.Columns["模块类别图片"].Width = 110;
                }

                gridControl_main_view.RowHeight = 22;
                gridControl_main_view.RefreshData();
                gridControl_main.Refresh();
             
             

            }));
           
        }

        public void LoadList()
        {
            //ModuleColl.Clear();
            //KeyValuePair<int, Model.ModelBase>[] list = null;
            //if (TabTag == "shop")
            //{
             
            //  list = Model.V3Infos.ModelShop.ToArray(); 
            //}
            //else if (TabTag == "new")
            //{
            //    list = Model.V3Infos.ModelShopNewTop100.ToArray();
            //}
            //else if (TabTag == "hot")
            //{
            //    list = Model.V3Infos.ModelShopHotTop100.ToArray();

            //}


            //foreach (KeyValuePair<int, Model.ModelBase> s in list)
            //{
            //    bool isshow = true;
            //    Image img = global::V3.Properties.Resources.共享发布模块;
            //    if (s.Value.Type == 10)
            //    {

            //        img = global::V3.Properties.Resources.关键字智能抓取;
            //        if (!checkEdit_module_type_get_keyword.Checked || checkEdit_module_type_post.Checked)
            //            isshow = false;
            //    }
            //    else if (s.Value.Type == 20)
            //    {
            //        img = global::V3.Properties.Resources.自定义抓取;
            //        if (!checkEdit_module_type_get_custom.Checked || checkEdit_module_type_post.Checked)
            //            isshow = false;
            //    }
            //    else if (s.Value.Type == 30)
            //    {
            //        img = global::V3.Properties.Resources.蜘蛛爬行抓取;
            //        if (!checkEdit_module_type_get_spider.Checked || checkEdit_module_type_post.Checked)
            //            isshow = false;
            //    }
            //    else if (s.Value.Type == 40)
            //    {
            //        img = global::V3.Properties.Resources.同步追踪抓取;
            //        if (!checkEdit_module_type_get_tongbu.Checked || checkEdit_module_type_post.Checked)
            //            isshow = false;
            //    }
            //    else
            //    {
            //        //发布模块
            //        if (checkEdit_module_type_get.Checked)
            //            isshow = false;
            //    }



            //    if (text_word.Text.Trim().Length == 0 || (s.Key.ToString().ToLower().Contains(text_word.Text.Trim().ToLower()) ||
            //         s.Value.Name.ToLower().Contains(text_word.Text.Trim().ToLower()) ||
            //         s.Value.Readme.ToLower().Contains(text_word.Text.Trim().ToLower()) ||
            //         s.Value.Description.ToLower().Contains(text_word.Text.Trim().ToLower()) ||
            //         s.Value.DesignName.ToLower().Contains(text_word.Text.Trim().ToLower())))
            //    {
            //        if (isshow)
            //        {
            //            string DesignName = s.Value.DesignName;
            //            if (s.Value.DesignName == "启程" || s.Value.DesignName == "xiaoxia" ||
            //                s.Value.DesignName == "米饭" || s.Value.DesignName == "小胡")
            //            {
            //                DesignName += "【官方】";
            //            }
            //            Record r = new Record();
            //            r.模块编号 = s.Key;
            //            r.模块名称 = s.Value.Name;
            //            r.更新时间 = s.Value.UpdateTime;
            //            r.共享积分 = s.Value.Money.ToString().Replace(".0000", " XB");
            //            r.人气指数 = s.Value.UserCount;
            //            r.获取 = "";
            //            r.作者 = DesignName;
            //            r.模块类别图片 = img;

            //            ModuleColl.Add(r);
            //        }
            //    }
            //}
            //this.Invoke((EventHandler)(delegate
            //{

            //    label_status.Caption = "模块市场模块总数" + Model.V3Infos.ModelShop.Count + "个，当前筛选到" + ModuleColl.Count + "个模块！";
            //}));
         
            //    ShowList();
           
        }

        public void ReloadModel(object bl)
        {
           
             LoadMylist();
          
           ShowList();

          
        }
        public void BuyModel(object mbb)
        {
            ShowI("获取模块中...");
           Model.ModelBase mb = (Model.ModelBase)mbb;
            string result = V3.Common.ModelShopBll.BuyModelBase(mb);
            if (result != "OK")
            {
                CloseI();
                try
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("购买模块失败，原因：" + result, "购买失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                }
                catch { }
            }
            else
            {
                CloseI();
                this.Invoke((EventHandler) (delegate
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("成功购买模块，可在“我的模块”中查看并使用！", "购买成功", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }));ReloadModel(false);
            }
        }
     
        //将我的模块显示到界面
        public void LoadMylist()
        {
            ModuleColl.Clear();
            V3.Common.ModelShopBll.ReSortMymodel();
            try
            {
                List<Model.GetPostModel> list = new List<Model.GetPostModel>();

                foreach (KeyValuePair<string, Model.GetPostModel> s in Model.V3Infos.MainDb.MyModels)
                {

                    if (checkEdit_module_type_all.Checked && (s.Key.ToString().ToLower().Contains(text_word.Text.Trim().ToLower()) || s.Value.PlanName.ToLower().Contains(text_word.Text.Trim().ToLower()) || s.Value.PlanReadme.ToLower().Contains(text_word.Text.Trim().ToLower()) || s.Value.PlanDescripton.ToLower().Contains(text_word.Text.Trim().ToLower()) || s.Value.PlanDesignName.ToLower().Contains(text_word.Text.Trim().ToLower())))
                        list.Add(s.Value);
                    else if (checkEdit_module_type_get.Checked && (s.Value.PlanModel != 5 && s.Value.PlanModel != 50) && (s.Key.ToString().ToLower().Contains(text_word.Text.Trim().ToLower()) || s.Value.PlanName.ToLower().Contains(text_word.Text.Trim().ToLower()) || s.Value.PlanReadme.ToLower().Contains(text_word.Text.Trim().ToLower()) || s.Value.PlanDescripton.ToLower().Contains(text_word.Text.Trim().ToLower()) || s.Value.PlanDesignName.ToLower().Contains(text_word.Text.Trim().ToLower())))
                        list.Add(s.Value);
                    else if (checkEdit_module_type_post.Checked && (s.Value.PlanModel == 5 || s.Value.PlanModel == 50) && (s.Key.ToString().ToLower().Contains(text_word.Text.Trim().ToLower()) || s.Value.PlanName.ToLower().Contains(text_word.Text.Trim().ToLower()) || s.Value.PlanReadme.ToLower().Contains(text_word.Text.Trim().ToLower()) || s.Value.PlanDescripton.ToLower().Contains(text_word.Text.Trim().ToLower()) || s.Value.PlanDesignName.ToLower().Contains(text_word.Text.Trim().ToLower())))
                        list.Add(s.Value);
                }

                label_status.Caption = "您的模块总数" + Model.V3Infos.MainDb.MyModels.Count + "个，当前筛选到" + ModuleColl.Count + "个模块！";
                for (int i = 0; i < list.Count; i++)
                {
                    int money = 0;
                    if (list[i].ShareLevel == 0)
                        money = 0;
                    else if (list[i].ShareLevel == 1)
                        money = 1;
                    else if (list[i].ShareLevel == 2)
                        money = 10;
                    else if (list[i].ShareLevel == 3)
                        money = 30;
                    else if (list[i].ShareLevel == 4)
                        money = 60;
                    else
                        money = 100;

                    Image img = global::V3.Properties.Resources.共享发布模块;
                    if (list[i].PlanModel == 1 || list[i].PlanModel == 10 || list[i].PlanModel == 2 ||
                        list[i].PlanModel == 20 || list[i].PlanModel == 3 || list[i].PlanModel == 30 ||
                        list[i].PlanModel == 4 || list[i].PlanModel == 40)
                    {
                        if (list[i].IsShareModel)
                        {
                            img = global::V3.Properties.Resources.共享抓取模块;
                        }
                        else
                        {
                            img = global::V3.Properties.Resources.私有_抓取模块;
                        }
                    }
                    else
                    {
                        if (list[i].IsShareModel)
                        {
                            img = global::V3.Properties.Resources.共享发布模块;
                        }
                        else
                        {
                            img = global::V3.Properties.Resources.私有发布模块;
                        }
                    }
                    //myModelList.Rows.Add(list[i].mid, list[i].PlanName, img, list[i].UpdateTime, money + " XB", (list[i].PlanDesignName == "启程" || list[i].PlanDesignName == "xiaoxia" || list[i].PlanDesignName == "米饭" || list[i].PlanDesignName == "小胡") ? list[i].PlanDesignName + "【官方】" : list[i].PlanDesignName, "编辑");
                    string DesignName = "【官方】";
                 
                    Record r = new Record();
                    r.模块编号 = list[i].mids;
                    r.模块名称 = list[i].PlanName;
                    r.更新时间 = DateTime.Now;
                    r.共享积分 = money + "元";
                    r.人气指数 = 0;
                    r.获取 = "";
                    r.作者 = DesignName;
                    r.模块类别图片 = img;
                    ModuleColl.Add(r);
                }

                ShowList();

            }
            catch { }
        }//注销我的模块
        //注销我的模块方法
        public void DelMyModel()
        {
            if (gridControl_main_view.GetSelectedRows().Length == 0)
                return;
            ShowI("正在注销模块...");
            string model_id = ((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0])).模块编号;
          
                Model.V3Infos.MainDb.MyModels.Remove(model_id);
                try
                {
                ReloadModel(true);
                }
                catch { }
                CloseI();
            

        }
      
        public void ModifyModelVoid()
        {
            if (gridControl_main_view.GetSelectedRows().Length == 0 || TabTag != "my")
                return;
            this.Opacity = 0;
            string model_id = ((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0])).模块编号;
            Model.GetPostModel model = Model.V3Infos.MainDb.MyModels[model_id];
            if (model.isPostModel)
            {
                V3Form.发布模块.postPlan frm = new 发布模块.postPlan();
                frm.Model = xEngine.Common.XSerializable.CloneObject<GetPostModel>(model);
                if (model.uid != 0 && model.uid == Convert.ToInt32(99))
                    frm.IsMymodel = true;
                frm.ShowDialog();
                if (frm.Issave)
                {
                    model = xEngine.Common.XSerializable.CloneObject<GetPostModel>(frm.Model);
                    int money = 0;
                    if (frm.Model.ShareLevel == 0)
                        money = 0;
                    else if (frm.Model.ShareLevel == 1)
                        money = 1;
                    else if (frm.Model.ShareLevel == 2)
                        money = 10;
                    else if (frm.Model.ShareLevel == 3)
                        money = 30;
                    else if (frm.Model.ShareLevel == 4)
                        money = 60;
                    else
                        money = 100;

                    Image img = global::V3.Properties.Resources.共享发布模块;
                    if (frm.Model.PlanModel == 1 || frm.Model.PlanModel == 10 || frm.Model.PlanModel == 2 || frm.Model.PlanModel == 20 || frm.Model.PlanModel == 3 || frm.Model.PlanModel == 30 || frm.Model.PlanModel == 4 || frm.Model.PlanModel == 40)
                    {
                        if (frm.Model.IsShareModel)
                        {
                            img = global::V3.Properties.Resources.共享抓取模块;
                        }
                        else
                        {
                            img = global::V3.Properties.Resources.私有_抓取模块;
                        }

                    }
                    else
                    {
                        if (frm.Model.IsShareModel)
                        {
                            img = global::V3.Properties.Resources.共享发布模块;
                        }
                        else
                        {
                            img = global::V3.Properties.Resources.私有发布模块;
                        }
                    }

                    ((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0])).模块名称 = frm.Model.PlanName;
                    ((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0])).更新时间 = DateTime.Parse(frm.Model.UpdateTime);
                    ((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0])).共享积分 = money + "元";
                    ((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0])).作者 = frm.Model.PlanDesignName;
                    ((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0])).模块类别图片 = img;
                    ReloadModel(false);
                }
            }
            else
            {
                V3Form.抓取模块.getPlan frm = new 抓取模块.getPlan();
                frm.model = xEngine.Common.XSerializable.CloneObject<GetPostModel>(model);
                if (model.uid != 0 && model.uid == Convert.ToInt32(99))
                    frm.isMymodel = true;
                frm.ShowDialog();
                if (frm.issave)
                {
                    model = xEngine.Common.XSerializable.CloneObject<GetPostModel>(frm.model);
                    int money = 0;
                    if (frm.model.ShareLevel == 0)
                        money = 0;
                    else if (frm.model.ShareLevel == 1)
                        money = 1;
                    else if (frm.model.ShareLevel == 2)
                        money = 10;
                    else if (frm.model.ShareLevel == 3)
                        money = 30;
                    else if (frm.model.ShareLevel == 4)
                        money = 60;
                    else
                        money = 100;

                    Image img = global::V3.Properties.Resources.共享发布模块;
                    if (frm.model.PlanModel == 1 || frm.model.PlanModel == 10 || frm.model.PlanModel == 2 || frm.model.PlanModel == 20 || frm.model.PlanModel == 3 || frm.model.PlanModel == 30 || frm.model.PlanModel == 4 || frm.model.PlanModel == 40)
                    {
                        if (frm.model.IsShareModel)
                        {
                            img = global::V3.Properties.Resources.共享抓取模块;
                        }
                        else
                        {
                            img = global::V3.Properties.Resources.私有_抓取模块;
                        }

                    }
                    else
                    {
                        if (frm.model.IsShareModel)
                        {
                            img = global::V3.Properties.Resources.共享发布模块;
                        }
                        else
                        {
                            img = global::V3.Properties.Resources.私有发布模块;
                        }
                    }


                    ((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0])).模块名称 = frm.model.PlanName;
                    ((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0])).更新时间 = DateTime.Parse(frm.model.UpdateTime);
                    ((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0])).共享积分 = money + "元";
                    ((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0])).作者 = frm.model.PlanDesignName;
                    ((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0])).模块类别图片 = img;
                    ReloadModel(false);
                }
            }

            this.Opacity = 1;
        }


        private void frmModelShop_Load(object sender, EventArgs e)
        {


        }
        private void frmModelShop_SizeChanged(object sender, EventArgs e)
        {}
        private void frmModelShop_Shown(object sender, EventArgs e)
        {
            if (selectmodel==1)
            {
                checkEdit_module_type_get.Checked = true;
            }else if (selectmodel == 2)
            {
                checkEdit_module_type_post.Checked = true;
            }
            this.Activate();
            System.Threading.Thread t = new System.Threading.Thread(ReloadModel);
            t.IsBackground = true;
            t.Start(false);
        }
        private void btn_GetModule_Click(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {  ModifyModelVoid();
          


        }
       
      

       


        private void text_word_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (TabTag == "my")
                {
                    LoadMylist();
                }
                else
                {
                    LoadList();
                }
            }
        }

        private void checkEdit_module_type_get_custom_CheckedChanged(object sender, EventArgs e)
        {
            LoadList();

        }

        private void checkEdit_module_type_get_keyword_CheckedChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        private void checkEdit_module_type_get_spider_CheckedChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        private void checkEdit_module_type_get_tongbu_CheckedChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        private void checkEdit_module_type_all_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit_module_type_all.Checked)
            {
                checkEdit_module_type_get_custom.Visible =
                    checkEdit_module_type_get_spider.Visible =
                        checkEdit_module_type_get_keyword.Visible =
                        checkEdit_module_type_get_tongbu.Visible = false;
                if (TabTag == "my")
                {
                    LoadMylist();
                }
                else
                {
                    LoadList();
                }
            }
        }

        private void checkEdit_module_type_get_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit_module_type_get.Checked && TabTag != "my")
            {

                checkEdit_module_type_get_custom.Visible =
                    checkEdit_module_type_get_spider.Visible =
                        checkEdit_module_type_get_keyword.Visible =
                        checkEdit_module_type_get_tongbu.Visible = true;
            }
            if (TabTag == "my")
            {
                LoadMylist();
            }
            else
            {
                LoadList();
            }
        }

        private void checkEdit_module_type_post_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit_module_type_post.Checked)
            {
                checkEdit_module_type_get_custom.Visible =
                    checkEdit_module_type_get_spider.Visible =
                        checkEdit_module_type_get_keyword.Visible = 
                        checkEdit_module_type_get_tongbu.Visible = false;
                if (TabTag == "my")
                {
                    LoadMylist();
                }
                else
                {
                    LoadList();
                }
            }
        }

        private void gridControl_main_DoubleClick(object sender, EventArgs e)
        {

            if (gridControl_main_view.GetSelectedRows().Length == 0)
                return;
            string model_id = ((Record) gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0])).模块编号;
            if (selectmodel != 0)
            {
                if (TabTag!="my")
                {
                    XtraMessageBox.Show("请切换到<color=red>我的模块</color>中选择，如果想要使用当前模块，可以先点击“获取模块”", "提示", DefaultBoolean.True);
                    return;
                }

                if (selectmodel == 1)
                {
                    if (!Model.V3Infos.MainDb.MyModels[model_id].isPostModel)
                    {
                        if (Program.Level < 30 &&
                            (Model.V3Infos.MainDb.MyModels[model_id].PlanModel == 4 ||
                             Model.V3Infos.MainDb.MyModels[model_id].PlanModel == 40))
                        {
                            XtraMessageBox.Show("您所使用的版本不支持【同步追踪抓取】功能，请联系客服升级后使用哦！", "需要升级", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                            return;
                        }
                        selectedid = model_id;
                        this.Close();
                    }
                    else
                    {
                        XtraMessageBox.Show("请选择一个<color=red>抓取模块</color>", "提示", MessageBoxButtons.OK,
                            MessageBoxIcon.Information,DefaultBoolean.True);
                        return;
                    }
                }
                else if (selectmodel == 2)
                {
                    if (Model.V3Infos.MainDb.MyModels[model_id].isPostModel)
                    {
                        selectedid = model_id;
                        this.Close();
                    }
                    else
                    {
                        XtraMessageBox.Show("请选择一个<color=red>发布模块</color>", "提示", MessageBoxButtons.OK,
                            MessageBoxIcon.Information, DefaultBoolean.True);
                        return;
                    }
                }
            }
            else
            {
                ModifyModelVoid();
            }
        }

        private void barButtonItem_module_give_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
          
        }

        private void barButtonItem_refresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Threading.Thread t = new System.Threading.Thread(ReloadModel);
            t.IsBackground = true;
            t.Start(true);
        }

        private void barButtonItem_getmodule_add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Opacity=0;
            V3Form.抓取模块.getPlan frm = new 抓取模块.getPlan();
            frm.isMymodel = true;
            frm.ShowDialog();
            if (frm.issave)
            {
                ReloadModel(true);
            }
            this.Opacity=1;
        }

        private void barButtonItem_postmodule_add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Opacity = 0;
            V3Form.发布模块.postPlan frm = new 发布模块.postPlan();
            frm.IsMymodel = true;
            frm.ShowDialog();
            if (frm.Issave)
            {
                ReloadModel(true);
            }
            this.Opacity=1;
        }

        private void barButtonItem_delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridControl_main_view.GetSelectedRows().Length == 0)
                return;
             else if (XtraMessageBox.Show("模块注销后将无法恢复，确定要注销？", "确认注销", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
            {
                System.Threading.Thread t = new System.Threading.Thread(DelMyModel);
                t.IsBackground = true;
                t.Start();
            }

        }

      

        private void gridControl_main_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Right&&TabTag=="my")
            {
                popupMenu.ShowPopup(Control.MousePosition);
            }
        }

        private void gridControl_main_view_EndSorting(object sender, EventArgs e)
        {
          
            
        }

    }
}