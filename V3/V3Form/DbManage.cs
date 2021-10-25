using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using Model;
using V3.Common;
using V3.V3Form;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Filtering;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace V3.V3Form
{
    public partial class DbManage : DevExpress.XtraEditors.XtraForm
    {
        public DbManage()
        {
            InitializeComponent();
            
        }
        public static RecordCollection coll = new RecordCollection();
        public static DataTable dt=new DataTable();
        public bool isselectdb = false;
        public int selectedDBid = 0;
        public int dbtype = 0;
     
        private void GroupLoad()
        {
          
          comboBox_group.Properties.Columns.Clear();
         dt=new DataTable();
            dt.Columns.Add("Name", typeof (string));
            dt.Columns.Add("Tag", typeof (string));
            dt.Rows.Add(new object[] { "全部分组", "AllGroup" });
            dt.Rows.Add(new object[] {"默认分组", "DefaultGroup"});
            foreach (KeyValuePair<string, string> value in Model.V3Infos.MainDb.GroupList)
            {
                dt.Rows.Add(new object[] {value.Value, value.Key.ToString()});
            }
            comboBox_group.EditValue = "Tag";
            comboBox_group.Properties.ValueMember = "Tag";
            comboBox_group.Properties.DisplayMember = "Name";

            comboBox_group.Properties.DataSource = dt;
            
            comboBox_group.Properties.Columns.Add(
                new DevExpress.XtraEditors.Controls.LookUpColumnInfo(comboBox_group.Properties.DisplayMember));
            comboBox_group.ItemIndex = 0;

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
        public static int ColumnCount = 6;
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
                if (listChangedHandler != null) listChangedHandler(this, new ListChangedEventArgs(ListChangedType.ItemChanged, rec.Index, rec.Index));
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
            string[] values = new string[6];
            public string 库ID { get { return values[0]; } set { SetValue(0, value); } }
            public string 库名称 { get { return values[1]; } set { SetValue(1, value); } }
            public string 分组ID { get { return values[2]; } set { SetValue(2, value); } }
            public string 分组名称 { get { return values[3]; } set { SetValue(3, value); } }
            public string 使用该库的任务数量 { get { return values[4]; } set { SetValue(4, value); } }
            public string 数据量 { get { return values[5]; } set { SetValue(5, value); } }


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
        #region 数据选中
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
        #region 数据加载

        //当前页
        int thispages = 0;
        //把库加载到界面上
        void loaddblist(int pages)
        {
            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(LoadDblist);
            Thread myThread = new Thread(ParStart);
            myThread.IsBackground = true;
            myThread.Start(pages);
        }

        //把库加载到界面上[线程]
        void LoadDblist(object pagenumber)
        {
            coll.Clear();
            int pages = (Int32)pagenumber;
            if (dbtype == 0)
            {
                Bll.DbBll.SaveDb(1, selectedDBid.ToString());
                V3Helper.LoadArticleDb();
                ShowI("文章库加载中...");


                ArrayList dbdata = V3.Bll.DbBll.GetDbPageList(1, text_search.Text.Trim());
                dbdata.Reverse();//反转过来，让后加的库排在前面
                int page = 0;
                page = dbdata.Count / Model.V3Infos.MainDb.PageNumber;
                if ((dbdata.Count % Model.V3Infos.MainDb.PageNumber) > 0)
                    page++;
                if (pages > page)
                    pages = page;
                if (pages < 1)
                    pages = 1;
                thispages = pages;

                for (int i = (pages - 1) * Model.V3Infos.MainDb.PageNumber; i < dbdata.Count; i++)
                {
                    if (i - ((pages - 1) * Model.V3Infos.MainDb.PageNumber) > Model.V3Infos.MainDb.PageNumber)
                        break;
                    ArticleDB tempdb = (ArticleDB)dbdata[i];
                    string GroupName = string.Empty;
                    if (Model.V3Infos.MainDb.GroupList.ContainsKey(tempdb.Type))
                        GroupName = Model.V3Infos.MainDb.GroupList[tempdb.Type];
                    else
                        GroupName = "默认分组";
                    Record row = new Record();
                    row.库ID = tempdb.Id;
                    row.库名称 = tempdb.Name;
                    row.分组ID = tempdb.Type;
                    row.分组名称 = GroupName;
                    row.使用该库的任务数量 = getTaskCount(dbtype, Convert.ToInt32(tempdb.Id)).ToString();
                    row.数据量 = tempdb.DataCount.ToString();
                    coll.Add(row);
                }


                CloseI();
                this.Invoke((EventHandler) (delegate
                {
                    text_page.Text = pages + "/" + page;
                    labDbStatus.Caption = "您总共有" + Model.V3Infos.ArticleDb.Count + "个文章库，系统帮您找到" + dbdata.Count +
                                          "个库（" + Model.V3Infos.MainDb.PageNumber + "个/页)当前是第" + pages + "页，总共有" + page +
                                          "页";
                }));
            }
            else if (dbtype == 1)
            {
                ShowI("关键词库加载中...");
                ArrayList dbdata = V3.Bll.DbBll.GetDbPageList(2, text_search.Text.Trim());
                dbdata.Reverse();//反转过来，让后加的库排在前面
                int page = 0;
                page = dbdata.Count / Model.V3Infos.MainDb.PageNumber;
                if ((dbdata.Count % Model.V3Infos.MainDb.PageNumber) > 0)
                    page++;
                if (pages > page)
                    pages = page;
                if (pages < 1)
                    pages = 1;
                thispages = pages;
                for (int i = (pages - 1) * Model.V3Infos.MainDb.PageNumber; i < dbdata.Count; i++)
                {
                    if (i - ((pages - 1) * Model.V3Infos.MainDb.PageNumber) > Model.V3Infos.MainDb.PageNumber)
                        break;
                    KeywordDB tempdb = (KeywordDB)dbdata[i];

                    string GroupName = string.Empty;
                    if (Model.V3Infos.MainDb.GroupList.ContainsKey(tempdb.Type))
                        GroupName = Model.V3Infos.MainDb.GroupList[tempdb.Type];
                    else
                        GroupName = "默认分组";
                    Record row = new Record();
                    row.库ID = tempdb.Id;
                    row.库名称 = tempdb.Name;
                    row.分组ID = tempdb.Type;
                    row.分组名称 = GroupName;
                    row.使用该库的任务数量 = getTaskCount(dbtype, Convert.ToInt32(tempdb.Id)).ToString();
                    row.数据量 = tempdb.DataCount.ToString();
                    coll.Add(row);


                }

                CloseI();
                this.Invoke((EventHandler) (delegate
                {
                    text_page.Text = pages + "/" + page;
                    labDbStatus.Caption = "您总共有" + Model.V3Infos.KeywordDb.Count + "个关键词库，系统帮您找到" + dbdata.Count +
                                          "个库（" + Model.V3Infos.MainDb.PageNumber + "个/页)当前是第" + pages + "页，总共有" +
                                          page + "页";
                }));
            }
            else if (dbtype == 2)
            {
                ShowI("哈希库加载中...");
                ArrayList dbdata = V3.Bll.DbBll.GetDbPageList(3, text_search.Text.Trim());
                dbdata.Reverse();//反转过来，让后加的库排在前面
                int page = 0;
                page = dbdata.Count / Model.V3Infos.MainDb.PageNumber;
                if ((dbdata.Count % Model.V3Infos.MainDb.PageNumber) > 0)
                    page++;
                if (pages > page)
                    pages = page;
                if (pages < 1)
                    pages = 1;
                thispages = pages;
                for (int i = (pages - 1) * Model.V3Infos.MainDb.PageNumber; i < dbdata.Count; i++)
                {
                    if (i - ((pages - 1) * Model.V3Infos.MainDb.PageNumber) > Model.V3Infos.MainDb.PageNumber)
                        break;
                    HashDB tempdb = (HashDB)dbdata[i];

                    string GroupName = string.Empty;
                    if (Model.V3Infos.MainDb.GroupList.ContainsKey(tempdb.Type))
                        GroupName = Model.V3Infos.MainDb.GroupList[tempdb.Type];
                    else
                        GroupName = "默认分组";
                    Record row = new Record();
                    row.库ID = tempdb.Id;
                    row.库名称 = tempdb.Name;
                    row.分组ID = tempdb.Type;
                    row.分组名称 = GroupName;
                    row.使用该库的任务数量 = getTaskCount(dbtype, Convert.ToInt32(tempdb.Id)).ToString();
                    row.数据量 = tempdb.DataCount.ToString();
                    coll.Add(row);

                }

                CloseI();
                this.Invoke((EventHandler) (delegate
                {
                    text_page.Text = pages + "/" + page;
                    labDbStatus.Caption = "您总共有" + Model.V3Infos.HashDb.Count + "个哈希库，系统帮您找到" + dbdata.Count + "个库（" +
                                          Model.V3Infos.MainDb.PageNumber + "个/页)当前是第" + pages + "页，总共有" + page + "页";
                }));
            }
            else if (dbtype == 3)
            {
                ShowI("替换库加载中...");

                ArrayList dbdata = V3.Bll.DbBll.GetDbPageList(4, text_search.Text.Trim());
                dbdata.Reverse();//反转过来，让后加的库排在前面
                int page = 0;
                page = dbdata.Count / Model.V3Infos.MainDb.PageNumber;
                if ((dbdata.Count % Model.V3Infos.MainDb.PageNumber) > 0)
                    page++;
                if (pages > page)
                    pages = page;
                if (pages < 1)
                    pages = 1;
                thispages = pages;
                for (int i = (pages - 1) * Model.V3Infos.MainDb.PageNumber; i < dbdata.Count; i++)
                {
                    if (i - ((pages - 1) * Model.V3Infos.MainDb.PageNumber) > Model.V3Infos.MainDb.PageNumber)
                        break;
                    ReplaceDB tempdb = (ReplaceDB)dbdata[i];

                    string GroupName = string.Empty;
                    if (Model.V3Infos.MainDb.GroupList.ContainsKey(tempdb.Type))
                        GroupName = Model.V3Infos.MainDb.GroupList[tempdb.Type];
                    else
                        GroupName = "默认分组";

                    Record row = new Record();
                    row.库ID = tempdb.Id;
                    row.库名称 = tempdb.Name;
                    row.分组ID = tempdb.Type;
                    row.分组名称 = GroupName;
                    row.使用该库的任务数量 = getTaskCount(dbtype, Convert.ToInt32(tempdb.Id)).ToString();
                    row.数据量 = tempdb.DataCount.ToString();
                    coll.Add(row);

                }
                CloseI();
                this.Invoke((EventHandler) (delegate
                {
                    text_page.Text = pages + "/" + page;
                    labDbStatus.Caption = "您总共有" + Model.V3Infos.ReplaceDb.Count + "个替换库，系统帮您找到" + dbdata.Count +
                                          "个库（" + Model.V3Infos.MainDb.PageNumber + "个/页)当前是第" + pages + "页，总共有" +
                                          page + "页";

                }));
            }
            else if (dbtype == 4)
            {

                ShowI("链接库加载中...");
                ArrayList dbdata = V3.Bll.DbBll.GetDbPageList(5, text_search.Text.Trim());
                dbdata.Reverse();//反转过来，让后加的库排在前面
                int page = 0;
                page = dbdata.Count / Model.V3Infos.MainDb.PageNumber;
                if ((dbdata.Count % Model.V3Infos.MainDb.PageNumber) > 0)
                    page++;
                if (pages > page)
                    pages = page;
                if (pages < 1)
                    pages = 1;
                thispages = pages;
                for (int i = (pages - 1) * Model.V3Infos.MainDb.PageNumber; i < dbdata.Count; i++)
                {
                    if (i - ((pages - 1) * Model.V3Infos.MainDb.PageNumber) > Model.V3Infos.MainDb.PageNumber)
                        break;
                    LinkDB tempdb = (LinkDB)dbdata[i];

                    string GroupName = string.Empty;
                    if (Model.V3Infos.MainDb.GroupList.ContainsKey(tempdb.Type))
                        GroupName = Model.V3Infos.MainDb.GroupList[tempdb.Type];
                    else
                        GroupName = "默认分组";
                    Record row = new Record();
                    row.库ID = tempdb.Id;
                    row.库名称 = tempdb.Name;
                    row.分组ID = tempdb.Type;
                    row.分组名称 = GroupName;
                    row.使用该库的任务数量 = getTaskCount(dbtype, Convert.ToInt32(tempdb.Id)).ToString();
                    row.数据量 = tempdb.Links.Count.ToString();
                    coll.Add(row);
                } CloseI();

                this.Invoke((EventHandler) (delegate
                {
                    text_page.Text = pages + "/" + page;
                    labDbStatus.Caption = "您总共有" + Model.V3Infos.LinkDb.Count + "个链接库，系统帮您找到" + dbdata.Count + "个库（" +
                                          Model.V3Infos.MainDb.PageNumber + "个/页)当前是第" + pages + "页，总共有" + page + "页";
                }));
            }
            ShowList();
        }

        public void ShowList()
        {
            this.Invoke((EventHandler)(delegate
                  {

                      gridControl_main.DataSource = coll;

                      gridControl_main_view.Columns["库ID"].VisibleIndex = 0;
                      gridControl_main_view.Columns["库名称"].VisibleIndex = 1;
                      gridControl_main_view.Columns["数据量"].VisibleIndex = 2;
                      gridControl_main_view.Columns["使用该库的任务数量"].VisibleIndex = 3;
                      gridControl_main_view.Columns["分组名称"].VisibleIndex = 4;
                      gridControl_main_view.Columns["库名称"].Width = 250;
                      gridControl_main_view.Columns["分组ID"].Visible = false;
                       gridControl_main_view.RefreshData();

                  }));
        }

        int getTaskCount(int dbtype, int dbid)
        {
            int result = 0;
            foreach (KeyValuePair<int, Model.Task> s in Model.V3Infos.TaskDb)
            {
                if (dbtype == 0)
                {
                    if (s.Value.ArticleDbId == dbid)
                        result++;
                }
                else if (dbtype == 1)
                {
                    if (s.Value.KeywordDbId == dbid)
                        result++;
                }
                else if (dbtype == 2)
                {
                    if (s.Value.HashDbId == dbid)
                        result++;
                }
                else if (dbtype == 3)
                {
                    if (s.Value.ReplaceDbId == dbid)
                        result++;
                }
                else if (dbtype == 4)
                {
                    if (s.Value.LinkDbId == dbid)
                        result++;
                }
            }
            return result;
        }





        #endregion
        #region 数据管理
        /// <summary>
        /// 新增库
        /// </summary>
        private void AddDb()
        {
            frm_新增库 frmadd = new frm_新增库();
            DataTable dt = (DataTable) comboBox_group.Properties.DataSource;
          
            frmadd.cmbGroup.Properties.DataSource = dt;

            if (comboBox_group.GetSelectedDataRow() != null)
            {
                frmadd.GroupString = ((DataRowView) comboBox_group.GetSelectedDataRow())["Tag"].ToString();
            }
            if (dbtype == 0)
            {
                frmadd.kutype = 1;
            }
            else if (dbtype == 1)
            {
                frmadd.kutype = 2;
            }
            else if (dbtype == 2)
            {
                frmadd.kutype = 3;
            }
            else if (dbtype == 3)
            {
                frmadd.kutype = 4;
            }
            else if (dbtype == 4)
            {
                frmadd.kutype = 5;
            }
            frmadd.ShowDialog();
            if (frmadd.issave)
            {
                text_search.Text = "";
                if (frmadd.add1)
                {
                    if (V3.Bll.DbBll.AddDb(1, frmadd.kuname, frmadd.kutypeid) != 0)
                    {
                        V3.Common.Log.LogNewline("[c12][系统]恭喜，您成功添加了一个新的文章库“" + frmadd.kuname + "”[/c]");
                        //XtraMessageBox.Show("恭喜，您成功添加了一个新的文章库“" + tempdb.dbName + "”", "增加成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                if (frmadd.add2)
                {
                    if (V3.Bll.DbBll.AddDb(2, frmadd.kuname, frmadd.kutypeid) != 0)
                    {
                        V3.Common.Log.LogNewline("[c12][系统]恭喜，您成功添加了一个新的关键词库“" + frmadd.kuname + "”[/c]");
                        //XtraMessageBox.Show("恭喜，您成功添加了一个新的文章库“" + tempdb.dbName + "”", "增加成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                if (frmadd.add3)
                {
                    if (V3.Bll.DbBll.AddDb(3, frmadd.kuname, frmadd.kutypeid) != 0)
                    {
                        V3.Common.Log.LogNewline("[c12][系统]恭喜，您成功添加了一个新的哈希库“" + frmadd.kuname + "”[/c]");
                        //XtraMessageBox.Show("恭喜，您成功添加了一个新的文章库“" + tempdb.dbName + "”", "增加成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                if (frmadd.add4)
                {
                    if (V3.Bll.DbBll.AddDb(4, frmadd.kuname, frmadd.kutypeid) != 0)
                    {
                        V3.Common.Log.LogNewline("[c12][系统]恭喜，您成功添加了一个新的替换库“" + frmadd.kuname + "”[/c]");
                        //XtraMessageBox.Show("恭喜，您成功添加了一个新的文章库“" + tempdb.dbName + "”", "增加成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                if (frmadd.add5)
                {
                    if (V3.Bll.DbBll.AddDb(5, frmadd.kuname, frmadd.kutypeid) != 0)
                    {
                        V3.Common.Log.LogNewline("[c12][系统]恭喜，您成功添加了一个新的链接库“" + frmadd.kuname + "”[/c]");
                        //XtraMessageBox.Show("恭喜，您成功添加了一个新的文章库“" + tempdb.dbName + "”", "增加成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                loaddblist(1);
            }
        }
        /// <summary>
        /// 删除选定的库
        /// </summary>
        void DelSelectDB()
        {
            DialogResult d = DialogResult.Cancel;

            this.Invoke((EventHandler) (delegate
            {

                d = XtraMessageBox.Show("您确认要删除所选定的“" + gridControl_main_view.SelectedRowsCount + "”个数据库吗？", "确定删除？", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            }));
            if (d != DialogResult.Yes)
            {
                return;
            }
            int[] SelectHandles = gridControl_main_view.GetSelectedRows();
            if (dbtype == 0)
            {
                ShowI("删除中...");
                foreach (int i in SelectHandles)
                {
                    Record r = (Record) gridControl_main_view.GetRow(i);
                    if (V3.Bll.DbBll.DbDelete(1, r.库ID))
                    {
                        V3.Common.Log.LogNewline("[c12][系统]恭喜，已成功删除名为“" + r.库名称 + "”的数据库！[/c]");
                    }
                    else
                    {
                        V3.Common.Log.LogNewline("[c14][系统]系统在删除名为“" + r.库名称 + "”的数据库时失败了，可能该库还有剩余数据或者还有任务在使用它！[/c]");
                    }
                }

                CloseI();
                loaddblist(1);
            }
            else if (dbtype == 1)
            {

                ShowI("删除中...");

                foreach (int i in SelectHandles)
                {
                    Record r = (Record)gridControl_main_view.GetRow(i);
                    if (V3.Bll.DbBll.DbDelete(2, r.库ID))
                    {
                        V3.Common.Log.LogNewline("[c12][系统]恭喜，已成功删除名为“" +r.库名称 + "”的数据库！[/c]");
                    }
                    else
                    {
                        V3.Common.Log.LogNewline("[c14][系统]系统在删除名为“" + r.库名称 + "”的数据库时失败了，可能该库还有剩余数据或者还有任务在使用它！[/c]");
                    }
                }

                CloseI();

                loaddblist(1);
            }
            else if (dbtype == 2)
            {

                ShowI("删除中...");

                foreach (int i in SelectHandles)
                {
                    Record r = (Record)gridControl_main_view.GetRow(i);
                    if (V3.Bll.DbBll.DbDelete(3, r.库ID))
                    {
                        V3.Common.Log.LogNewline("[c12][系统]恭喜，已成功删除名为“" + r.库名称 + "”的数据库！[/c]");
                    }
                    else
                    {
                        V3.Common.Log.LogNewline("[c14][系统]系统在删除名为“" + r.库名称 + "”的数据库时失败了，可能该库还有剩余数据或者还有任务在使用它！[/c]");
                    }
                }
                CloseI();

                loaddblist(1);
            }
            else if (dbtype == 3)
            {

                ShowI("删除中...");
                foreach (int i in SelectHandles)
                {
                    Record r = (Record)gridControl_main_view.GetRow(i);
                    if (V3.Bll.DbBll.DbDelete(4, r.库ID))
                    {
                        V3.Common.Log.LogNewline("[c12][系统]恭喜，已成功删除名为“" + r.库名称 + "”的数据库！[/c]");
                    }
                    else
                    {
                        V3.Common.Log.LogNewline("[c14][系统]系统在删除名为“" + r.库名称 + "”的数据库时失败了，可能该库还有剩余数据或者还有任务在使用它！[/c]");
                    }
                }

                CloseI();
                loaddblist(1);
            }
            else if (dbtype == 4)
            {
               
              ShowI("删除中...");
 
                foreach (int i in SelectHandles)
                {
                    Record r = (Record)gridControl_main_view.GetRow(i);
                    if (V3.Bll.DbBll.DbDelete(5, r.库ID))
                    {
                        V3.Common.Log.LogNewline("[c12][系统]恭喜，已成功删除名为“" + r.库名称 + "”的数据库！[/c]");
                    }
                    else
                    {
                        V3.Common.Log.LogNewline("[c14][系统]啊哦，系统在删除名为“" + r.库名称 + "”的数据库时失败了，可能该库还有剩余数据或者还有任务在使用它！[/c]");
                    }
                }
               
                CloseI();
                
                loaddblist(1);
            }
        }
        /// <summary>
        /// 查看库
        /// </summary>
        private void ViewDb()
        {

            if (gridControl_main_view.SelectedRowsCount > 0)
            {
                Record r = (Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);
                if (dbtype == 0)
                {

                    frm_文章库文章列表 dbview = new frm_文章库文章列表();
                    dbview.dbid =r.库ID;
                    dbview.Text = Model.V3Infos.ArticleDb[dbview.dbid].Name + "(文章总数：" + Model.V3Infos.ArticleDb[dbview.dbid].DataCount + "个） - 文章库编辑";
                    dbview.ShowDialog();
                   r.数据量 = Model.V3Infos.ArticleDb[dbview.dbid].DataCount.ToString();
                   Bll.DbBll.SaveDb(1, dbview.dbid);
                }
                else if (dbtype == 1)
                {
                    frm_关键词库 view = new frm_关键词库();
                    view.Dbid = r.库ID;
                    view.Text = Model.V3Infos.KeywordDb[view.Dbid].Name + "(关键词总数：" + Model.V3Infos.KeywordDb[view.Dbid].Keywords.Count + "个） - 关键词库编辑";
                    view.ShowDialog();
                    if (view.IsSave)
                        Model.V3Infos.KeywordDb[view.Dbid].Keywords = view.WordList;
                  r.数据量 = Model.V3Infos.KeywordDb[view.Dbid].Keywords.Count.ToString();
                    Model.V3Infos.KeywordDb[view.Dbid].DataCount = Model.V3Infos.KeywordDb[view.Dbid].Keywords.Count;
                    Bll.DbBll.SaveDb(2, view.Dbid);
                    view.Dispose();
                }
                else if (dbtype == 2)
                {
                    frm_编辑哈希库 dbview = new frm_编辑哈希库();
                    dbview.Dbid = r.库ID;
                    dbview.Text = Model.V3Infos.HashDb[dbview.Dbid].Name + "(哈希值总数：" + Model.V3Infos.HashDb[dbview.Dbid].DataCount + "个） - 哈希库编辑";
                    dbview.ShowDialog();
                   r.数据量 = Model.V3Infos.HashDb[dbview.Dbid].DataCount.ToString();
                }
                else if (dbtype == 3)
                {
                    frm_替换库 view = new frm_替换库();
                    view.dbid = r.库ID;
                    view.Text = Model.V3Infos.ReplaceDb[view.dbid].Name + "(替换词组总数：" + Model.V3Infos.ReplaceDb[view.dbid].Words.Count + "组） - 替换库编辑";
                    view.ShowDialog();
                    if (view.isSave)
                        Model.V3Infos.ReplaceDb[view.dbid].Words = view.word;
                    r.数据量 = Model.V3Infos.ReplaceDb[view.dbid].Words.Count.ToString();
                   Model.V3Infos.ReplaceDb[view.dbid].DataCount = Model.V3Infos.ReplaceDb[view.dbid].Words.Count;
                    Bll.DbBll.SaveDb(4, view.dbid);
                    view.Dispose();
                }
                else if (dbtype == 4)
                {
                    frm_链接库 view = new frm_链接库();
                    view.dbid = r.库ID;
                    view.Text = Model.V3Infos.LinkDb[view.dbid].Name + "(链接组总数：" + Model.V3Infos.LinkDb[view.dbid].Links.Count + "组） - 链接库编辑";
                    view.ShowDialog();
                    if (view.isSave)
                        Model.V3Infos.LinkDb[view.dbid].Links = view.word;
                 r.数据量 = Model.V3Infos.LinkDb[view.dbid].Links.Count.ToString(); ;
                 Bll.DbBll.SaveDb(5, view.dbid);
                    view.Dispose();
                }

            }
        }
        #endregion


        private void DbManage_Load(object sender, EventArgs e)
        {


        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            loaddblist(thispages);
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            AddDb();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(new ThreadStart(DelSelectDB));
            th.IsBackground = true;
            th.Start();
        }

        private void btn_view_Click(object sender, EventArgs e)
        {
            ViewDb();
        }

        private void btn_pre_Click(object sender, EventArgs e)
        {
            thispages--;
            loaddblist(thispages);
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            thispages++;
            loaddblist(thispages);
        }


        private void gridControl_main_MouseUp(object sender, MouseEventArgs e)
        {
            StartRowHandle = -1;
            CurrentRowHandle = -1;
            isMouseDown = false;
            isSetStartRow = false;

            if (gridControl_main_view.SelectedRowsCount > 0)
            {
                if (e.Button == MouseButtons.Right)
                {

                    popupMenu.ShowPopup(Control.MousePosition);
                }
            }
        }
       

        private void gridControl_main_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                GridHitInfo info = gridControl_main_view.CalcHitInfo(e.X, e.Y);
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

        private void gridControl_main_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
            }

        }

        private void barButtonItem_add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btn_add_Click(sender, e);
        }

        private void barButtonItem_rename_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (gridControl_main_view.SelectedRowsCount >0)
            {
                Record r = (Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);
                frm_重命名库 frm = new frm_重命名库();
                frm.name.Text = r.库名称;
                frm.ShowDialog();
                if (frm.issave)
                {
                    r.库名称 = frm.name.Text;
                    V3.Bll.DbBll.DbRename((dbtype + 1), r.库ID, r.库名称, r.分组ID);
                }
            }
        }

        private void barButtonItem_modifygroup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridControl_main_view.SelectedRowsCount == 1)
            {
                Record r = ((Record) gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]));
                DataTable dt = ((DataTable) comboBox_group.Properties.DataSource);
                frm_重新选择分组 frm = new frm_重新选择分组();
                frm.cmbGroup.Properties.DataSource = dt;

               
               frm.GroupString =r.分组ID ;
                frm.ShowDialog();
                if (frm.issave)
                {
                    string GroupName = string.Empty;
                    if (Model.V3Infos.MainDb.GroupList.ContainsKey(frm.GroupString))
                        GroupName = Model.V3Infos.MainDb.GroupList[frm.GroupString];
                    else
                        GroupName = "默认分组";

                  r.分组名称 = GroupName;
                   r.分组ID = frm.GroupString;
                   V3.Bll.DbBll.DbRename((dbtype + 1), r.库ID, r.库名称, frm.GroupString);
                }
            }
        }

        private void barButtonItem_delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btn_delete_Click(sender, e);
        }

        private void barButtonItem_view_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btn_view_Click(sender, e);
        }
        private void gridControl_main_DoubleClick(object sender, EventArgs e)
        {

            if (gridControl_main_view.SelectedRowsCount > 0)
            {
                Record r = (Record) gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);
                if (isselectdb)
                {
                    selectedDBid = Convert.ToInt32(r.库ID);
                    this.Close();
                }
                else
                    btn_view_Click(sender, e);
            }

        }

        private void gridControl_main_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void comboBox_group_EditValueChanged(object sender, EventArgs e)
        {
            if (comboBox_group.GetSelectedDataRow() != null)
            {
                text_search.Text = "Group:" + ((DataRowView) comboBox_group.GetSelectedDataRow())["Tag"];
                btn_search_Click(sender, e);
            }

        }

        private void DbManage_Shown(object sender, EventArgs e)
        {
           GroupLoad();
        }

    }
}
