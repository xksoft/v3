using DevExpress.XtraEditors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.IO.MemoryMappedFiles;
using System.Diagnostics;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Web;
using System.Data;

namespace V3.V3Form
{

    public partial class frmGetCookie : XtraForm
    {
        public frmGetCookie()
        {
            InitializeComponent();
        }
        public bool IsShow = false;
        public bool IsStop = false;
        public string MemStr = "";
        public string CookieStr = "";
        public string Url = "";
        public bool GetSuccess = false;
        #region 数据模型
        public static int ColumnCount = 3;
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
            string[] values = new string[3];

            public string 方法 { get { return values[0]; } set { SetValue(0, value); } }
            public string 地址 { get { return values[1]; } set { SetValue(1, value); } }
            public string 马甲 { get { return values[2]; } set { SetValue(2, value); } }
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

        public static T ParseFromJson<T>(string szJson)
        {
            try
            {
                T obj = Activator.CreateInstance<T>();
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                    return (T)serializer.ReadObject(ms);
                }
            }
            catch {
                return default(T);
            }
        }
        public static bool IsProcessExist(string name)
        {
            try
            {

                var pArrayy = System.Diagnostics.Process.GetProcesses();
                if (pArrayy == null || pArrayy.Length == 0)
                {
                    return false;
                }
                foreach (var item in pArrayy)
                {
                    try
                    {
                        if (item.ProcessName.ToLower().Contains(name))
                        {
                            return true;
                        }

                    }
                    catch (Exception er)
                    {

                        continue;
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }


        private string GetCookie(string header)
        {
            string cook = "";
            string[] sss = header.Replace("\r\n", "`").Split('`');
            for (int i = 0; i < sss.Length; i++)
            {
                if (sss[i].Contains("Cookie"))
                    return sss[i].Replace("Cookie:", "");
            }
            return cook;
        }

        public static void StopProcess(string name)
        {
           
            try
            {

                var pArrayy = System.Diagnostics.Process.GetProcesses();
                if (pArrayy == null || pArrayy.Length == 0)
                {
                    return;
                }
                foreach (var item in pArrayy)
                {
                    try
                    {
                        if (item.ProcessName.ToLower().Contains(name))
                        {

                            if (!item.CloseMainWindow()) {
                               
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show("停止抓包工具时出错，若造成不能上网，请再次启动抓包工具并手动点击停止！" + ex.Message,
                                 "停止出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                }
              
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("停止抓包工具时出错，若造成不能上网，请再次启动抓包工具并手动点击停止！"+ex.Message,
                                 "停止出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public void StartSniffer()
        {

            byte[] b = new byte[9999999];


            Thread thread = new Thread(delegate ()
            {
                if (!IsProcessExist("xsniffer"))
                {
                    try
                    {
                        Process pro = Process.Start("XSniffer.exe", "start postandget ^((?!(.*.js|.*.jpg|.*.png|.*.gif|.*.css)).*)");
                    }
                    catch
                    {
                        this.Invoke((EventHandler)(delegate
                        {
                            XtraMessageBox.Show("抓包工具的启动被系统安全软件阻止，请手动启动目录下的“XSniffer.exe”或将V3从阻止列表中移除后再次提取POST参数！",
                                "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Close();
                        }));
                        IsShow = false;

                    }

                }
                Thread.Sleep(1000);
                while (true)
                {
                    if (!IsShow)
                    {
                        break;
                    }
                    MemoryMappedFile mp = null;
                    try
                    {
                        mp = MemoryMappedFile.OpenExisting("XSniffer");
                    }
                    catch
                    {
                        continue;
                    }
                    MemoryMappedViewStream stream = mp.CreateViewStream();
                    stream.Read(b, 0, b.Length);
                    string NewMeMStr = Encoding.UTF8.GetString(b).Replace("\0", "");
                    if (NewMeMStr.Length == 0)
                    {
                        continue;

                    }

                    if (NewMeMStr == MemStr)
                    {
                        continue;
                    }
                    MemStr = NewMeMStr;
                    if (MemStr.Length == 0)
                    {
                        DataColl.Clear();
                        gridControl_main_view.RefreshData();
                        continue;
                    }
                    List<Data> list = ParseFromJson<List<Data>>(MemStr);
                   
                    DataColl.Clear();
                    if (list != null && list.Count > 0)
                    {
                        foreach (var d in list)
                        {
                            Record r = new Record();
                            r.方法 = d.Request_Method;
                            r.地址 = d.Request_Url;
                            r.马甲 = GetCookie(d.Request_Headers);
                            DataColl.Add(r);
                        }
                    }
                    
                    if (IsShow)
                    {
                        this.Invoke((EventHandler)(delegate
                                           {
                                               gridControl_main_view.RefreshData();
                                           }));
                    }
                    else { break; }
                   

                    Thread.Sleep(500);
                }

            });
            thread.IsBackground = true;
            thread.Start();

        }

        public void StopSniffer()
        {

            IsStop = true;
            StopProcess("xsniffer");
            panel_Info.Visible = false;
        }

        private void frmGetCookie_Load(object sender, EventArgs e)
        {
            gridControl_main.DataSource = DataColl;
            gridControl_main_view.Columns["方法"].VisibleIndex = 0;
            gridControl_main_view.Columns["方法"].Width = 15;
            gridControl_main_view.Columns["马甲"].Width = 30;
            gridControl_main_view.Columns["地址"].VisibleIndex = 1;
            IsShow = true;
            StartSniffer();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (gridControl_main_view.SelectedRowsCount > 0)
            {
                Record r = (Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);
                if (r.马甲.Length > 0)
                {
                    CookieStr = r.马甲;
                    Url = r.地址;
                    StopSniffer();
                    GetSuccess = true;
                    this.Close();
                }
                else
                {
                    XtraMessageBox.Show("请选择一有马甲数据的项！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                XtraMessageBox.Show("请选择一有马甲数据的项！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void frmGetCookie_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsShow = false;
            StopSniffer();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridControl_main_view_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            txt_cookie.Text = string.Empty;
            if (gridControl_main_view.SelectedRowsCount > 0)
            {
                Record r = (Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);
                txt_cookie.Text += r.马甲;

            }
        }
    }
    public class Data
    {
        public string Request_Method = "GET";
        public string Request_Url = "";
        public string Request_Body = "";
        public string Request_Headers = "";
    }
}
