using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using DevExpress.XtraEditors;

namespace V3.V3Form
{
    public partial class frmDefaultTasks : DevExpress.XtraEditors.XtraForm
    {
        public frmDefaultTasks()
        {
            InitializeComponent();
        }
        public Model.Task DefaultTask = null;
        public string DefaultTaskName = "";
        public bool IsOK = false;
        #region 数据模型
        public static int ColumnCount = 1;
        public static RecordCollection DataColl = new RecordCollection();
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
            string[] values = new string[1];

            public string 模板名称 { get { return values[0]; } set { SetValue(0, value); } }
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
        private void frmDefaultPoints_Load(object sender, EventArgs e)
        {
            if (DefaultTask != null)
            {
                gridControl_main.Visible = false;
                btn_ok.Text = "保存到任务模板列表";
            }
            else
            {
                label_name.Visible = text_name.Visible = false;
                gridControl_main.DataSource = DataColl;
                LoadDefaultTasks();
            }
           
        }
        public void LoadDefaultTasks()
        {
            DataColl.Clear();
            List<string> list = Model.V3Infos.MainDb.DefaultTasks.Keys.ToList<string>() ;
            foreach (string s in list)
            {
                if (Model.V3Infos.MainDb.DefaultTasks.ContainsKey(s))
                {
                    Record r = new Record();
                    r.模板名称 = s;
                    DataColl.Add(r);
                }
                
            }
            gridControl_main_view.RefreshData();

        }

      
        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (DefaultTask != null)
            {
                if (text_name.Text.Trim().Length == 0)
                {
                    XtraMessageBox.Show("请为模板起一个名字方便以后使用！", "无法保存", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else if (Model.V3Infos.MainDb.DefaultTasks.ContainsKey(text_name.Text.Trim()))
                {
                    XtraMessageBox.Show("模板列表中已经存在名称为“" + text_name.Text.Trim() + "”的模板，请使用其他的名称！", "无法保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Model.Task Task = xEngine.Common.XSerializable.CloneObject<Model.Task>(DefaultTask);
                   
                    Model.V3Infos.MainDb.DefaultTasks.Add(text_name.Text.Trim(), Task);
                    IsOK = true;
                    this.Close();

                }
            }
            else {

                if (gridControl_main_view.SelectedRowsCount > 0)
                {
                    Record r = (Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);
                    if (r != null && Model.V3Infos.MainDb.DefaultTasks.ContainsKey(r.模板名称))
                    {
                       
                        DefaultTask = xEngine.Common.XSerializable.CloneObject<Model.Task>(Model.V3Infos.MainDb.DefaultTasks[r.模板名称]);
                        if (!Model.V3Infos.SendPointDb.ContainsKey(DefaultTask.PointId))
                        {
                            DialogResult dr = XtraMessageBox.Show("当前任务模块所属发布点已经被删除，该模板无法使用，点击“确定”将删除该模板！", "无法继续", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                            if (dr == DialogResult.OK)
                            {
                                Model.V3Infos.MainDb.DefaultTasks.Remove(r.模板名称);
                              
                                LoadDefaultTasks();
                               
                            } DefaultTask = null;
                        }
                        else
                        {  DefaultTaskName = r.模板名称;
                            IsOK = true;
                            this.Close();
                        }
                        
                    }
                    else
                    {
                        XtraMessageBox.Show("选择的模板无效！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    XtraMessageBox.Show("请选择一个任务模板，如果没有模板请编辑一个任务设为模板！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridControl_main_Click(object sender, EventArgs e)
        {
            
        }

        private void gridControl_main_view_RowDeleted(object sender, DevExpress.Data.RowDeletedEventArgs e)
        {
           
        }

        private void gridControl_main_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                Record r = (Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);
                if (r != null && Model.V3Infos.MainDb.DefaultTasks.ContainsKey(r.模板名称))
                {
                    DialogResult dr = XtraMessageBox.Show("确定要删除这个任务模板吗？", "确定删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                    if (dr == DialogResult.OK)
                    {
                        Model.V3Infos.MainDb.DefaultTasks.Remove(r.模板名称);
                        LoadDefaultTasks();
                    }
                }
            }
        }
    }
}
