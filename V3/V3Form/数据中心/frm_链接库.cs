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
    public partial class frm_链接库 : DevExpress.XtraEditors.XtraForm
    {
        public frm_链接库()
        {
            InitializeComponent();
        }
        public bool IsShow = false;
        public bool stop = false;
        public bool isSave = false;
        public string dbid = "";
        public List<Model.Link> word = new List<Model.Link>();
        public Thread tdaochu;
        public string filename = "";
        public string[] links;
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

            public void Remove(Record r)
            {
                List.Remove(r);
            }

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
            string[] values = new string[3];
            public string 关键词 { get { return values[0]; } set { SetValue(0, value); } }
            public string 标题 { get { return values[1]; } set { SetValue(1, value); } }
            public string 链接地址 { get { return values[2]; } set { SetValue(2, value); } }
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
            if (IsShow)
            {
                this.Invoke((EventHandler)(delegate
                {
                    ipanel.Visible = false;
                }));
            }
            
        }
        private void Clear()
        {
            for (int i = 0; i < Coll.Count; i++)
            {
                Model.Link link = new Model.Link();
                if (Coll[i].关键词 != null)
                {
                    link.Keyword = Coll[i].关键词;
                }
                else { link.Keyword = ""; }
                if (Coll[i].标题 != null)
                {
                    link.Title = Coll[i].标题;
                }
                else { link.Title = ""; }
                if (Coll[i].链接地址 != null)
                {
                    link.Url = Coll[i].链接地址;
                }
                else { link.Url = ""; }
                word.Add(link);

            }
            try
            {
                this.Invoke((EventHandler)(delegate
                {
                    txtStatus.Caption = "总共有链接组" + Coll.Count + "条";
                    if (isSave)
                    {
                        this.Close();
                    }

                }));
            }
            catch { }
        }
        public void ShowList()
        {
            this.Invoke((EventHandler) (delegate
            {
                gridControl_main.DataSource = Coll;
                gridControl_main_view.RefreshData();
                gridControl_main_view.Columns["关键词"].VisibleIndex = 0;
                gridControl_main_view.Columns["标题"].VisibleIndex = 1;
                gridControl_main_view.Columns["链接地址"].VisibleIndex = 2;
                txtStatus.Caption = "一共有" + Coll.Count + "个链接";
            }));
        }
        public void Export()
        {
          File.WriteAllLines(filename, links, Encoding.Default);
          CloseI();

            this.Invoke((EventHandler) (delegate
            {
                XtraMessageBox.Show("成导出"+links.Length+"个链接到“"+filename+"”", "导出完成", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }));

        }

        public void Import()
        {
            stop = false;
            ShowI("准备导入...");
            links = File.ReadAllLines(filename, Encoding.Default);
            this.Invoke((EventHandler)(delegate
            {
                istate.Text = "一共有" + links.Length.ToString() + "链接需要导入...";
            }));
            for (int i = 0; i < links.Length; i++)
            {
                if (stop)
                {
                    break;
                }
                string[] li = links[i].Split('|');
                if (li.Length >=3)
                {
                    Record r = new Record();
                    r.关键词 = li[0];
                    r.标题 = li[1];
                    r.链接地址 = li[2];
                    Coll.Add(r);


                }
               

            }
            ShowList();
            CloseI();

        }

        private void frm_关键词库_Shown(object sender, EventArgs e)
        {
            IsShow = true;
           Coll.Clear();
            for (int i = 0; i < Model.V3Infos.LinkDb[dbid].Links.Count; i++)
            {
                Model.Link link = (Model.Link)(Model.V3Infos.LinkDb[dbid].Links[i]);
                Record r = new Record();
                r.关键词 = link.Keyword;
                r.标题 = link.Title;
                r.链接地址 = link.Url;
                Coll.Add(r);
            }
           
           ShowList();
        }

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            isSave = true;
            Thread s = new Thread(new ThreadStart(Clear));
            s.IsBackground = true;
            s.Start();
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            isSave = false;
            this.Close();
        }

        private void btn_add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frm_参数输入编辑器 view = new frm_参数输入编辑器();
            view.Text = "请输入需要添加的链接（支持多组)";
            view.txttitle.Caption = "一行一个,示例：我的电脑|我的电脑坏了，谁来帮帮我呀？|http://www.xxx.com/123.htm";
           // view.txtMainbox.Text = "我的电脑|我的电脑坏了，谁来帮帮我呀？|http://www.xxx.com/123.htm";
            view.ShowDialog();
            if (view.issave)
            {
                string[] temp = view.txtMainbox.Lines;
                for (int i = 0; i < temp.Length; i++)
                {
                    string[] s = temp[i].Split('|');
                    if (s.Length == 3)
                    {
                        Record r = new Record();
                        r.关键词 = s[0];
                        r.标题 = s[1];
                        r.链接地址 = s[2];
                        Coll.Add(r);
                       
                    }
                }
                ShowList();
            }
            txtStatus.Caption = "总共有链接组" + Coll.Count + "条";
        }

        private void btn_import_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog sa = new OpenFileDialog();
            sa.Filter = "本文件(*.txt)|*.txt";
            if (sa.ShowDialog() == DialogResult.OK)
            {
                tdaochu = new Thread(Import);
                tdaochu.IsBackground = true;
                tdaochu.Start();
                filename = sa.FileName;

                ShowI("准备导入...");

            }
        }

        private void btn_output_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            links = new string[Coll.Count];
            for (int i = 0; i < Coll.Count; i++)
            {
                links[i] = (Coll[i].关键词 != null ? Coll[i].关键词 : "") + "|" + Coll[i].标题 + "|" + Coll[i].链接地址;
            }
            SaveFileDialog sa = new SaveFileDialog();
            sa.DefaultExt = "txt";
            sa.FileName = DateTime.Now.ToString("yyyyMMddHHmmss链接库");
            if (sa.ShowDialog() == DialogResult.OK)
            {
                filename = sa.FileName;
                tdaochu = new Thread(Export);
                tdaochu.IsBackground = true;
                tdaochu.Start();
                ShowI("准备导出...");
                istate.Text = "有" + links.Length.ToString() + "个链接正在导出中...";
             
            }
        }

        private void btn_clear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("确定要清空“" + Model.V3Infos.LinkDb[dbid].Name + "”这个文章库吗？", "确定清空？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
               Coll.Clear();
                gridControl_main_view.RefreshData();
            }
            txtStatus.Caption = "总共有链接组" + Coll.Count + "条";
        }

        private void frm_链接库_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CloseI();
                stop = true;

            }
        }

        private void btn_delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            List<Record>rl=new SupportClass.EquatableList<Record>();
            for (int i = 0; i < gridControl_main_view.SelectedRowsCount; i++)
            {
                Record r = ((Record) gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[i]));
                if (r != null)
                {
                 rl.Add(r);
                }
               

            }
            foreach (var r in rl)
            {
                Coll.Remove(r);
            }
        gridControl_main_view.RefreshData();
        gridControl_main_view.ClearSelection();
           
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
            if (e.Button==MouseButtons.Right)
            {
                popupMenu.ShowPopup(Control.MousePosition);
            }
        }

        private void frm_链接库_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsShow = false;
        }

    }
}
