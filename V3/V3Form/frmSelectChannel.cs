using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace V3.V3Form
{
    public partial class frmSelectChannel : DevExpress.XtraEditors.XtraForm
    {
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
        #region 数据模型
        public static int ColumnCount = 2;
        public static RecordCollection ChannelColl = new RecordCollection();
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
            string[] values = new string[2];
            public string 栏目编号 { get { return values[0]; } set { SetValue(0, value); } }
            public string 栏目名称 { get { return values[1]; } set { SetValue(1, value); } }
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
        public frmSelectChannel()
        {
            InitializeComponent();
        }
        public string[] jieguo = new string[0];
        public bool issave = false;
        public string fenleistr = "";
     
        private void buttonX1_Click(object sender, EventArgs e)
        {
            issave = true;
            fenleistr = txtPointName.Text.Trim();
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SelectClass_Load(object sender, EventArgs e)
        {
            ChannelColl.Clear();
            for (int i = 0; i < jieguo.Length; i++)
            {
                string[] temp = jieguo[i].Split('`');
                Record r=new Record();
                r.栏目编号 = temp[0];
                r.栏目名称 = temp[1];
               ChannelColl.Add(r);
            }
            gridControl_main.DataSource = ChannelColl;
            gridControl_main_view.RefreshData();
            label_channel.Text = "共有" + jieguo.Length + "个分类（多选可以按住Ctrl键不放再选择)";
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
        }

        private void gridControl_main_view_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            txtPointName.Text = "";
            for (int i = 0; i < gridControl_main_view.GetSelectedRows().Length; i++)
            {
                if (i != gridControl_main_view.GetSelectedRows().Length - 1)
                {
                    txtPointName.Text += ((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[i])).栏目编号 + "|";
                }
                else
                {
                    txtPointName.Text +=
                        ((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[i])).栏目编号
                            .ToString();
                }
            }
        }
    }
}
