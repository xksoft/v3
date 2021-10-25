using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace V3.V3Form.发布模块
{

    public partial class PostStp2 : DevExpress.XtraEditors.XtraForm
    {
        public bool isShow = false;
        public System.Threading.Thread thread;
        public Model.GetPostModel Model = new Model.GetPostModel();
        public V3.Bll.PostBll Bll;
        public bool Issave = false;
        #region 数据模型

        public static int ColumnCount = 2;
        public static RecordCollection ChannelColl = new RecordCollection();

        public class RecordCollection : CollectionBase, IBindingList, ITypedList
        {
            public Record this[int i]
            {
                get { return (Record) List[i]; }
            }

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
                PropertyDescriptorCollection coll = TypeDescriptor.GetProperties(typeof (Record));
                ArrayList list = new ArrayList(coll);
                list.Sort(new PDComparer());
                PropertyDescriptorCollection res = new PropertyDescriptorCollection(null);
                for (int n = 0; n < ColumnCount; n++)
                {
                    res.Add(list[n] as PropertyDescriptor);
                }
                return res;
            }

            private class PDComparer : IComparer
            {
                int IComparer.Compare(object a, object b)
                {
                    return Comparer.Default.Compare(GetName(a), GetName(b));
                }

                private int GetName(object a)
                {
                    PropertyDescriptor pd = (PropertyDescriptor) a;
                    if (pd.Name.StartsWith("Column")) return Convert.ToInt32(pd.Name.Substring(6));
                    return -1;

                }
            }

            string ITypedList.GetListName(PropertyDescriptor[] accessors)
            {
                return "";
            }

            public object AddNew()
            {
                return null;
            }

            public bool AllowEdit
            {
                get { return true; }
            }

            public bool AllowNew
            {
                get { return false; }
            }

            public bool AllowRemove
            {
                get { return false; }
            }

            private ListChangedEventHandler listChangedHandler;

            public event ListChangedEventHandler ListChanged
            {
                add { listChangedHandler += value; }
                remove { listChangedHandler -= value; }
            }

            public void AddIndex(PropertyDescriptor pd)
            {
                throw new NotSupportedException();
            }

            public void ApplySort(PropertyDescriptor pd, ListSortDirection dir)
            {
                throw new NotSupportedException();
            }

            public int Find(PropertyDescriptor property, object key)
            {
                throw new NotSupportedException();
            }

            public bool IsSorted
            {
                get { return false; }
            }

            public void RemoveIndex(PropertyDescriptor pd)
            {
                throw new NotSupportedException();
            }

            public void RemoveSort()
            {
                throw new NotSupportedException();
            }

            public ListSortDirection SortDirection
            {
                get { throw new NotSupportedException(); }
            }

            public PropertyDescriptor SortProperty
            {
                get { throw new NotSupportedException(); }
            }

            public bool SupportsChangeNotification
            {
                get { return true; }
            }

            public bool SupportsSearching
            {
                get { return false; }
            }

            public bool SupportsSorting
            {
                get { return false; }
            }

        }

        private void SetValue(object data, int row, int column, object val)
        {
            RecordCollection rc = data as RecordCollection;
            rc.SetValue(row, column, val);
        }

        public class Record
        {
            internal int Index = -1;
            internal RecordCollection owner;
            private string[] values = new string[2];

            public string 栏目编号
            {
                get { return values[0]; }
                set { SetValue(0, value); }
            }

            public string 栏目名称
            {
                get { return values[1]; }
                set { SetValue(1, value); }
            }

            public string GetValue(int index)
            {
                return values[index];
            }

            //<label1>
            public void SetValue(int index, object val)
            {
                values[index] = val.ToString();
                if (this.owner != null) this.owner.OnListChanged(this);
            }

            //</label1>
        }

        #endregion

        public PostStp2()
        {
            InitializeComponent();


        }
        public void ShowI(string txt)
        {
            this.Invoke((EventHandler) (delegate
            {
                ipanel.Visible = true;
                istate.Text = txt;
            }));

        }
        public void CloseI()
        {
            if (isShow)
            {
                this.Invoke((EventHandler) (delegate
                {
                    ipanel.Visible = false;
                }));
            }
        }
        private void SetInfo()
        {
            Model.Stp2_POST_GetAddOktag = txtAddOktag.Text;
            Model.Stp2_POST_GetClassRules = txtGetrules.Text;
            Model.Stp2_POST_UsedClass = NeedClass.IsOn;
            Model.Stp2_POST_UsedAddClass = NeedAddclass.IsOn;
        }
        private void GetInfo()
        {
            if (Model == null)
                Model = new Model.GetPostModel();
            txtAddOktag.Text = Model.Stp2_POST_GetAddOktag;
            txtGetrules.Text = Model.Stp2_POST_GetClassRules;
            NeedClass.IsOn = Model.Stp2_POST_UsedClass;
            NeedAddclass.IsOn = Model.Stp2_POST_UsedAddClass;
        }
        private void GetClass()
        {
            ChannelColl.Clear();
            this.Invoke((EventHandler) (delegate
            {
                btnTestStp.Enabled = false;
                btnTestStp.Text = "正在获取";
            }));
          
            string result = Bll.RunAction(Model.Stp2_POST_Get, true, "");
           
            string[] jieguo = Bll.ClassGet(result);
            if (isShow)
            {
                this.Invoke((EventHandler) (delegate
                {

                    for (int i = 0; i < jieguo.Length; i++)
                    {
                        Record r = new Record();

                        string[] temp = jieguo[i].Split('`');
                        r.栏目编号 = temp[0];
                        r.栏目名称 = temp[1];
                        ChannelColl.Add(r);
                    }
                    gridControl_main.DataSource = ChannelColl;
                    gridControl_main_view.RefreshData();
                    panelEx3.Text = "共获取到" + jieguo.Length + "个分类！";
                    htmlEditor2.BodyHtml = result;
                    btnTestStp.Enabled = true;
                    btnTestStp.Text = "测试获取分类";
                }));

            }


            CloseI();

        }
        private void AddClass()
        {
            btnAddClass.Enabled = false;
            btnAddClass.Text = "正在增加";
            string[] classname = textBoxX1.Lines;
            System.Collections.ArrayList ary = new System.Collections.ArrayList();
            foreach (string str in classname)
            {
                if (str.Length > 0)
                    ary.Add(str);
            }
            int count = 0;
            for (int i = 0; i < ary.Count; i++)
            {
                if (Bll.ClassAdd(ary[i].ToString(), true))
                    count++;
            }
            if (isShow)
            {
                this.Invoke((EventHandler) (delegate
                {
                    XtraMessageBox.Show("测试添加" + ary.Count + "个分类，其中" + count + "个分类成功添加，您可以在左侧重新获取分类查看测试！", "新增完成",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnAddClass.Enabled = true;
                    btnAddClass.Text = "测试新增分类";
                }));

            }
            CloseI();

        }
        private void getPlan_Load(object sender, EventArgs e)
        {
            isShow = true;
            GetInfo();
        }
        private void btnstp2get_Click(object sender, EventArgs e)
        {
            V3Form.发布模块.Pget frm = new Pget();
            frm.Action = Model.Stp2_POST_Get;
            frm.Model = this.Model;
            frm.Bll = this.Bll;
            frm.ShowDialog();
            if (frm.IsSave)
            {
                Model.Stp2_POST_Get = frm.Action;
            }
        }

        private void btnTestStp_Click(object sender, EventArgs e)
        {
            SetInfo();
            thread = new System.Threading.Thread(GetClass);
            thread.IsBackground = true;
            thread.Start();
            ShowI("正在获取分类");

        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            SetInfo();
            thread = new System.Threading.Thread(AddClass);
            thread.IsBackground = true;
            thread.Start();
            ShowI("正在新增分类");

        }

        private void btnstp2post_Click(object sender, EventArgs e)
        {
            V3Form.发布模块.Ppost frm = new Ppost();
            frm.Action = Model.Stp2_POST_Post;
            frm.Model = this.Model;
            frm.ShowDialog();
            if (frm.IsSave)
            {
                Model.Stp2_POST_Post = frm.Action;
            }
        }

        private void PostStp2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CloseI();
                btnAddClass.Enabled = true;
                btnAddClass.Text = "测试新增分类";
                btnTestStp.Enabled = true;
                btnTestStp.Text = "测试获取分类";

                if (thread != null)
                {
                    try
                    {
                        thread.Abort();
                    }
                    catch
                    {
                    }
                }

            }
        }

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Issave = true;
            SetInfo();
            this.Close();
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Issave = false;
            this.Close();

        }

        private void PostStp2_FormClosed(object sender, FormClosedEventArgs e)
        {
            isShow = false;
        }

    }
}
