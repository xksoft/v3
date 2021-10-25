using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

using DevExpress.XtraEditors;


namespace V3.V3Form
{
    public partial class frm_文章库文章列表 : DevExpress.XtraEditors.XtraForm
    {
        public frm_文章库文章列表()
        {
            InitializeComponent();
        }
        #region 数据模型
        public static int ColumnCount = 3;
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
            object[] values = new object[3];
            public int 编号 { get { return Convert.ToInt32(values[0]); } set { SetValue(0, value); } }
            public string 标题 { get { return values[1].ToString(); } set { SetValue(1, value); } }
            public string 修改时间 { get { return values[2].ToString(); } set { SetValue(2, value); } }
           
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
        #region 列表多选
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
        }public void ShowList()
        {
            this.Invoke((EventHandler)(delegate
            {


                gridControl_main.DataSource = Coll;

                gridControl_main_view.Columns["编号"].Width = 60;
                gridControl_main_view.Columns["标题"].MinWidth = 190;
                gridControl_main_view.Columns["修改时间"].Width = 150;


                gridControl_main_view.Columns["编号"].VisibleIndex = 0;
                gridControl_main_view.Columns["标题"].VisibleIndex = 1;
                gridControl_main_view.Columns["修改时间"].VisibleIndex = 2;


                gridControl_main_view.RefreshData();
                gridControl_main.Refresh();
                gridControl_main_view.ClearSelection();


            }));

        }
        public string dbid = "";

        private void btn_Article_Click(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

        }

        private void frmMainDbListview_Shown(object sender, EventArgs e)
        {
            this.Text = "文章库[" + Model.V3Infos.ArticleDb[dbid].Name+ "]的文章列表";
            this.Update();
            this.Refresh();
            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(loadData);
            Thread myThread = new Thread(ParStart);
            myThread.IsBackground = true;
            myThread.Start(1);
        }
        int thispages = 0;
        void loadData(object pages)
        {
           showI("数据加载中...");
               Coll.Clear();

            int page = 0;
            page = Model.V3Infos.ArticleDb[dbid].DataCount / Model.V3Infos.MainDb.PageNumber;
            if ((Model.V3Infos.ArticleDb[dbid].DataCount % Model.V3Infos.MainDb.PageNumber) > 0)
                page++;
            if ((int)pages > page)
                pages = page;
            if ((int)pages < 1)
                pages = 1;
            thispages = (int)pages;

            ArrayList tempdb = V3.Common.ArticleBll.getpagelist((int)pages, Model.V3Infos.ArticleDb[dbid]);
            for (int i = 0; i < tempdb.Count; i++)
            {
               
                    Model.Model_Article tempmodel = (Model.Model_Article)tempdb[i];
                    Record r=new Record();
                    r.编号 = Convert.ToInt32(tempmodel.Id);
                    r.标题 = tempmodel.DataObject[0];
                    r.修改时间 = tempmodel.Date;
                    Coll.Add(r);
               
            }
            ShowList();
            try
            {
            this.Invoke((EventHandler)(delegate
            {
              closeI();Status.Caption= "总共有" + Model.V3Infos.ArticleDb[dbid].DataCount + "篇文章，当前是第" + pages + "页，共有" + page + "页（" + Model.V3Infos.MainDb.PageNumber + "个/页)";
              txtpage.EditValue = pages + "/" + page;
            }));
            }
            catch { }
      
        }

        private void pre_Click(object sender, EventArgs e)
        {
            thispages--;
            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(loadData);
            Thread myThread = new Thread(ParStart);
            myThread.IsBackground = true;
            myThread.Start(thispages);
        }

        private void next_Click(object sender, EventArgs e)
        {
            thispages++;
            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(loadData);
            Thread myThread = new Thread(ParStart);
            myThread.IsBackground = true;
            myThread.Start(thispages);
        }

        private void clear_Click(object sender, EventArgs e)
        {if (XtraMessageBox.Show("确定要清空“" + Model.V3Infos.ArticleDb[dbid].Name + "”这个文章库吗？", "确定清空？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (V3.Common.ArticleBll.clearDB(Model.V3Infos.ArticleDb[dbid]))
                {
                    V3.Common.Log.LogNewline("[c12][系统]恭喜，系统已经成功的帮您清空了“" + Model.V3Infos.ArticleDb[dbid].Name + "”这个文章库！[/c]");
                    ParameterizedThreadStart ParStart = new ParameterizedThreadStart(loadData);
                    Thread myThread = new Thread(ParStart);
                    myThread.IsBackground = true;
                    myThread.Start(thispages);
                }
                else
                {
                    V3.Common.Log.LogNewline("[c14][系统]啊哦，系统在帮您清空“" + Model.V3Infos.ArticleDb[dbid].Name + "”这个文章库的时候遇到问题了，没有清空成功，详细请看日志！[/c]");
                }
            }
        }

        private void DelSelectData()
        {
            int[] SelectHandles = gridControl_main_view.GetSelectedRows();
            foreach (int h in SelectHandles)
            {
                if (
                   V3.Common.ArticleBll.dbdelete(
                       ((Record)gridControl_main_view.GetRow(h)).编号.ToString(),
                       Model.V3Infos.ArticleDb[dbid]))
                {
                    //Coll.RemoveAt(
                    //    ((Record)gridControl_main_view.GetRow(h)).Index);
                }
                else
                {
                    V3.Common.Log.LogNewline("[c14][系统]文章“" + ((Record)gridControl_main_view.GetRow(h)).标题 + "”删除失败！[/c]");

                }
                
            }
          

            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(loadData);
            Thread myThread = new Thread(ParStart);
            myThread.IsBackground = true;
            myThread.Start(thispages);
        }

        private void view_Click(object sender, EventArgs e)
        {
           
        }
        void edit(string id)
        {
            Model.Model_Article tempmodel = new Model.Model_Article();
            tempmodel.Id = id;
            if (Common.ArticleBll.LoadData(ref tempmodel, Model.V3Infos.ArticleDb[dbid]))
            {
                frm_文章编辑器 aedit = new frm_文章编辑器();
                aedit.Text = "为“" + Model.V3Infos.ArticleDb[dbid].Name + "”增加新文章";
                aedit.art = tempmodel;
                aedit.ShowDialog();
                if (aedit.issave)
                {
                    if (V3.Common.ArticleBll.SaveData(aedit.art, Model.V3Infos.ArticleDb[dbid]))
                    {
                        V3.Common.Log.LogNewline("[c12][系统]恭喜您，系统成功保存id为" + tempmodel.Id + "的数据的修改！[/c]");
                        ParameterizedThreadStart ParStart = new ParameterizedThreadStart(loadData);
                        Thread myThread = new Thread(ParStart);
                        myThread.IsBackground = true;
                        myThread.Start(thispages);
                    }
                }
            }
            else
            {
                V3.Common.Log.LogNewline("[c14][系统]啊哦，系统在加载id为" + tempmodel.Id + "的数据时失败了[/c]");
            }
        }

        private void datalist_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           }

        private void datalist_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

 

        private void btnAdd_Click(object sender, EventArgs e)
        {
           
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
           
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

           
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
           
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
         
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
           
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
           
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
          
        }

        private void frm_文章库文章列表_Load(object sender, EventArgs e)
        {

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
            }
        }

        private void gridControl_main_MouseUp(object sender, MouseEventArgs e)
        {
            StartRowHandle = -1;
            CurrentRowHandle = -1;
            isMouseDown = false;
            isSetStartRow = false;
            if (e.Button == MouseButtons.Right)
            {
                popupMenu.ShowPopup(Control.MousePosition);
            }
        }

        private void btn_add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frm_文章编辑器 aedit = new frm_文章编辑器();
            aedit.Text = "为“" + Model.V3Infos.ArticleDb[dbid].Name + "”增加新文章";
            aedit.ShowDialog();
            if (aedit.issave)
            {
                aedit.art.Date = DateTime.Now.ToString();
                if (V3.Common.ArticleBll.addnewdata(Model.V3Infos.ArticleDb[dbid], aedit.art))
                {
                    
                }
                ParameterizedThreadStart ParStart = new ParameterizedThreadStart(loadData);
                Thread myThread = new Thread(ParStart);
                myThread.IsBackground = true;
                myThread.Start(thispages);
            }
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btn_import_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frm_文章导入导出工具 import = new frm_文章导入导出工具();
            import.isimport = true;
            import.dbid = dbid;
            import.ShowDialog();
            if (import.needRefresh)
            {
                btn_ref_ItemClick(null, null);
            }
        }

        private void btn_output_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Program.Level < 3)
            {
                XtraMessageBox.Show( "您所使用的版本不支持【文章导出】功能，请联系客服升级后使用哦！", "升级版本后才能使用哦！", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }
            frm_文章导入导出工具 import = new frm_文章导入导出工具();
            import.isimport = false;
            import.dbid = dbid;
            import.ShowDialog();
           
        }

        private void btn_clear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("您确定要清空“" + Model.V3Infos.ArticleDb[dbid].Name + "”这个文章库吗？", "是否确认清空？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (V3.Common.ArticleBll.clearDB(Model.V3Infos.ArticleDb[dbid]))
                {
                    V3.Common.Log.LogNewline("[c12][系统]恭喜，系统已经成功的帮您清空了“" + Model.V3Infos.ArticleDb[dbid].Name + "”这个文章库！[/c]");
                    ParameterizedThreadStart ParStart = new ParameterizedThreadStart(loadData);
                    Thread myThread = new Thread(ParStart);
                    myThread.IsBackground = true;
                    myThread.Start(1);
                }
                else
                {
                    V3.Common.Log.LogNewline("[c14][系统]啊哦，系统在帮您清空“" + Model.V3Infos.ArticleDb[dbid].Name + "”这个文章库的时候遇到问题了，木有清空成功，详细请看日志！[/c]");
                }
            }
        }

        private void btn_pre_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            thispages--;
            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(loadData);
            Thread myThread = new Thread(ParStart);
            myThread.IsBackground = true;
            myThread.Start(thispages);
        }

        private void btn_next_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            thispages++;
            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(loadData);
            Thread myThread = new Thread(ParStart);
            myThread.IsBackground = true;
            myThread.Start(thispages);
        }

        private void btn_delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("您确定要删除所选的这" +gridControl_main_view.SelectedRowsCount+ "篇文章吗？", "是否确认删除？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Thread th = new Thread(new ThreadStart(DelSelectData));
                th.IsBackground = true;
                th.Start();
            }
        }

        private void btn_edit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridControl_main_view.SelectedRowsCount > 0)
                edit(((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0])).编号.ToString());
        }

        private void gridControl_main_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btn_edit_ItemClick(null,null);
        }

        private void btn_ref_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(loadData);
            Thread myThread = new Thread(ParStart);
            myThread.IsBackground = true;
            myThread.Start(thispages);
        }


    }
}
