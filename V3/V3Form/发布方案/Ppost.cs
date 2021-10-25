using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Lucene.Net.Analysis;

namespace V3.V3Form.发布模块
{
    public partial class Ppost : DevExpress.XtraEditors.XtraForm
    {
        public static Ppost MyPpost;
        public string MemStr = "";
        public bool IsStop = false;
        public bool IsShow = false;
        System.Threading.Thread t;
        public Model.发布相关模型.GetPostAction Action = new Model.发布相关模型.GetPostAction();
        public Model.GetPostModel Model = new Model.GetPostModel();
        public V3.Bll.PostBll Bll;
        public bool IsSave = false;
        public string OldHtml = string.Empty;
        #region 数据模型
        public static int ColumnCount = 6;
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
            string[] values = new string[7];
            public string 地址 { get { return values[0]; } set { SetValue(0, value); } }
            public string 数据 { get { return values[1]; } set { SetValue(1, value); } }
            public string POST数据 { get { return values[2]; } set { SetValue(2, value); } }
            public string 来路 { get { return values[3]; } set { SetValue(3, value); } }
            public bool POST模型 { get { return Convert.ToBoolean(values[4]); } set { SetValue(4, value); } }
            public string 客户端 { get { return values[5]; } set { SetValue(5, value); } }
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
        public Ppost()
        {
            InitializeComponent();
            Bll = new Bll.PostBll(Model, 0, 0);
            Pget.CheckForIllegalCrossThreadCalls = false;
            MyPpost = this;
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
            if (IsShow)
            {
                this.Invoke((EventHandler) (delegate
                {
                    ipanel.Visible = false;
                }));
            }
        }
        void SetInfo()
        {
            Action.ActionUrl = txturl.Text;
            Action.RefrereUrl = txtrefrere.Text;
            Action.UserAgent = txtUserAgent.Text;
            Action.IsPost = true; ;
            Action.IsUtf8 = bianma.IsOn;
            Action.PostModel = isMutli.IsOn;
            string[] tempstr = txtPostData.Lines;
            Action.IsGetRedirect = chongdingxiang.IsOn;
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            for (int i = 0; i < tempstr.Length; i++)
            {
                if (tempstr[i].Trim().Split('→').Length == 2)
                {
                    list.Add(tempstr[i].Trim());
                }
            }
            Action.PostData.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                string[] temp = list[i].ToString().Split('→');
                if (!Action.PostData.ContainsKey(temp[0]))
                {
                    Action.PostData.Add(temp[0], temp[1]);
                }
                else
                {
                    Action.PostData[temp[0]] = temp[1];
                }
            }
        }
        void GetInfo()
        {
            if (Action == null)
                Action = new Model.发布相关模型.GetPostAction();
            txturl.Text = Action.ActionUrl;
            txtrefrere.Text = Action.RefrereUrl;
            txtUserAgent.Text = Action.UserAgent;
            bianma.IsOn = Action.IsUtf8;
            isMutli.IsOn = Action.PostModel;
            txtPostData.Text = string.Empty;
            chongdingxiang.IsOn = Action.IsGetRedirect;
            txtPostData.Text = "";
            foreach (KeyValuePair<string, string> value in Action.PostData)
            {
                txtPostData.Text += value.Key + "→" + value.Value + "\r\n";
            }
        }
        void Start()
        {
            btnstart.Enabled = false;
            btnstart.Text = "正在测试";
            SetInfo();
            Bll = new Bll.PostBll(Model, 0, 0);
            Model.发布相关模型.Account account = Model.POST_TestAccount;
            string result = Bll.RunAction(Action, true, OldHtml);
            Model.POST_TestAccount = account;
            if (IsShow)
            {
                this.Invoke((EventHandler)(delegate
                {
                    htmlEditor2.BodyHtml = result;
                    btnstart.Enabled = true;
                    btnstart.Text = "开始测试";
                }));
            }

            CloseI();

        }

        private void getPlan_Load(object sender, EventArgs e)
        {
            panel_Info.Visible = false;
            IsShow = true;
            GetInfo();
        }
        private void btnstart_Click(object sender, EventArgs e)
        {
            t = new System.Threading.Thread(Start);
            t.IsBackground = true;
            t.Start();
            ShowI("正在向目标进行POST数据");
            
        }
        private void buttonItem3_Click(object sender, EventArgs e)
        {
           
        }

        private void buttonItem1_Click(object sender, EventArgs e)
        {
         
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            StartSniffer();
            IsShow = true;
            gridControl_main.DataSource = DataColl;
            gridControl_main_view.Columns["数据"].Visible = false;
            gridControl_main_view.Columns["客户端"].Visible = false;
            gridControl_main_view.Columns["来路"].Visible = false;
            gridControl_main_view.Columns["POST模型"].Visible = false;
            gridControl_main_view.Columns["POST数据"].Visible = false;
            
        }
        public static T ParseFromJson<T>(string szJson)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
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
        private string GetReferer(string header)
        {
            string cook = "";
            string[] sss = header.Replace("\r\n", "`").Split('`');
            for (int i = 0; i < sss.Length; i++)
            {
                if (sss[i].ToLower().Contains("referer"))
                    return sss[i].ToLower().Replace("referer:", "");
            }
            return cook;
        }
        private bool GetPostmodel(string header)
        {
            if (header.ToLower().Contains("x-www-form-urlencoded"))
                return false;
            else
                return true;
        }
        private string GetUserAgent(string header)
        {
            string cook = "";
            string[] sss = header.Replace("\r\n", "`").Split('`');
            for (int i = 0; i < sss.Length; i++)
            {
                if (sss[i].Contains("User-Agent"))
                    return sss[i].Replace("User-Agent:", "");
            }
            return cook;
        }
        private string GetDataList(string data)
        {
            try
            {
                List<string> list = new List<string>();
                if (data.Contains("Content-Disposition"))
                {
                    var kvs = data.Replace("Content-Disposition", "≈").Split('≈');
                    var r = new Regex("(?<=name=\").*?(?=\")");
                    foreach (
                        var sps in
                            kvs.Select(kvstr => kvstr.Replace("\r\n", "≌").Split('≌'))
                                .Where(sps => r.Match(sps[0]).Success)
                                .Where(sps => r.Match(sps[0]).Value.Trim() != ""))
                    {
                        var sb = new StringBuilder();
                        for (var i = 2; i < sps.Length - 2; i++)
                        {
                            sb.Append(sps[i]);
                        }
                        list.Add(r.Match(sps[0]).Value.Trim() + "→" + sb.ToString());
                    }
                }
                else
                {
                    var kv = data.Split('&');
                    foreach (var ss in kv.Select(s => s.Split('=')))
                    {
                        if (ss.Length >= 2)
                        {
                            list.Add(ss[0] + "→" + HttpUtility.UrlDecode(ss[1]));
                        }
                        else
                        {
                            list.Add(data);
                        }

                    }
                }
                string str = "";
                foreach (var d in list)
                {
                    str += d + "\r\n";
                }
                return str;
            }
            catch
            {
                return "";
            }
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
                            item.CloseMainWindow();
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

        }
        public void StartSniffer()
        {
            panel_Info.Width = panelEx1.Width;
            panel_Info.Height = panelEx1.Height+bar3.Size.Height;
           panel_Info.Location=new Point(0,0);
            panel_Info.Visible = true;
            byte[] b = new byte[9999999];
            

                Thread thread = new Thread(delegate()
                {
                    if (!IsProcessExist("xsniffer"))
                    {
                        try
                        {
                            Process pro = Process.Start("XSniffer.exe", "start post .*?");
                        }
                        catch
                        {
                            this.Invoke((EventHandler) (delegate
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
                        if (NewMeMStr.Length==0)
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
                        foreach (var d in list)
                        {
                            Record r = new Record();
                            r.POST数据 = GetDataList(d.Request_Body);
                            r.地址 = d.Request_Url;
                            r.数据 = d.Request_Url +
                                   "\r\n\r\n"
                                   + d.Request_Headers +
                                   "\r\n\r\n\r\n"
                                   + r.POST数据;
                            r.客户端 = GetUserAgent(d.Request_Headers);
                            r.POST模型 = GetPostmodel(d.Request_Headers);
                            r.来路 = GetReferer(d.Request_Headers);

                            DataColl.Add(r);
                        }

                        gridControl_main_view.RefreshData();

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
      

        private void txturl_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (Program.f_frmReplaceTag == null || Program.f_frmReplaceTag.IsDisposed )
                {
                    Program.f_frmReplaceTag = new frmReplaceTag(); 
                }
                Program.f_frmReplaceTag.referer = "Ppost_txturl";
                Program.f_frmReplaceTag.Focus();
                Program.f_frmReplaceTag.Show();
                Program.f_frmReplaceTag.Location = new Point(0,0); 
            }
        }

        private void txtrefrere_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (Program.f_frmReplaceTag == null || Program.f_frmReplaceTag.IsDisposed )
                {
                    Program.f_frmReplaceTag = new frmReplaceTag();
                }
                Program.f_frmReplaceTag.referer = "Ppost_txtrefrere";
                Program.f_frmReplaceTag.Focus();
                Program.f_frmReplaceTag.Show();
                Program.f_frmReplaceTag.Location = new Point(0, 0);
            }
        }

        private void txtPostData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (Program.f_frmReplaceTag == null || Program.f_frmReplaceTag.IsDisposed )
                {
                    Program.f_frmReplaceTag = new frmReplaceTag();
                }
                Program.f_frmReplaceTag.referer = "Ppost_txtPostData";
                Program.f_frmReplaceTag.Focus();
                Program.f_frmReplaceTag.Show();
                Program.f_frmReplaceTag.Location = new Point(0, 0);
            }
        }

        private void Ppost_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CloseI();
                btnstart.Enabled = true;
                btnstart.Text = "开始测试";

                if (t != null)
                {
                    try
                    {
                        t.Abort();
                    }
                    catch { }
                }

            }
        }

        private void barButtonItem_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetInfo();
            IsSave = true;
            this.Close();
        }

        private void barButtonItem_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IsSave = false;
            this.Close();
        }

        private void Ppost_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed )
            {
                Program.f_frmReplaceTag.Close();
            }
        }

        private void txturl_MouseUp(object sender, MouseEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed )
            {
                Program.f_frmReplaceTag.referer = "Ppost_txturl";
            }
        }

        private void txtrefrere_MouseUp(object sender, MouseEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed )
            {
                Program.f_frmReplaceTag.referer = "Ppost_txtrefrere";
            }
        }

        private void txtPostData_MouseUp(object sender, MouseEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed)
            {
                Program.f_frmReplaceTag.referer = "Ppost_txtPostData";
            }
        }

        private void Ppost_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsShow = false;
            StopSniffer();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (gridControl_main_view.SelectedRowsCount > 0)
            {
                Record r = (Record) gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);
                if (Model.Testadminurl.Length > 0)
                {
                    txturl.Text = r.地址.Replace(Model.Testadminurl, "【后台地址】");
                }
                else
                {
                    txturl.Text = r.地址;
                }

                isMutli.IsOn = r.POST模型;
                txtUserAgent.Text = r.客户端;
                if (r.来路.Length > 0 )
                {
                    if (Model.Testadminurl.Length > 0)
                    {
                        txtrefrere.Text = r.来路.Replace(Model.Testadminurl, "【后台地址】");
                    }
                    else
                    {
                        txtrefrere.Text = r.来路;
                    }

                }
                txtPostData.Text = r.POST数据;
                StopSniffer();
            }
            else
            {
                XtraMessageBox.Show("请选择一条要使用的POST数据！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            StopSniffer();
        }

        private void gridControl_main_view_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            txtmore.Text = string.Empty;
            if (gridControl_main_view.SelectedRowsCount > 0)
            {
                Record r = (Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);
                txtmore.Text += r.数据;

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
