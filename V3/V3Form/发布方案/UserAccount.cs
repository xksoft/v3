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
    public partial class UserAccount : DevExpress.XtraEditors.XtraForm
    {
        public Model.发布相关模型.UserAccount Account = new Model.发布相关模型.UserAccount();
       public Model.发布相关模型.Account Majiaac = new Model.发布相关模型.Account();
        public bool IsSave = false;
       public System.Threading.Thread t;
        public UserAccount()
        {
            InitializeComponent();
        }
        #region 数据模型
        public static int ColumnCount = 7;
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
            string[] values = new string[7];
            public string 账号 { get { return values[0]; } set { SetValue(0, value); } }
            public string 密码 { get { return values[1]; } set { SetValue(1, value); } }
            public string 附加值1 { get { return values[2]; } set { SetValue(2, value); } }
            public string 附加值2 { get { return values[3]; } set { SetValue(3, value); } }
            public string 最近活动时间 { get { return values[4]; } set { SetValue(4, value); } }
            public string 状态 { get { return values[5]; } set { SetValue(5, value); } }
            public string 异常计数 { get { return values[6]; } set { SetValue(6, value); } }
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
       
        void SetInfo()
        {
            Account.randomget = isRandom.IsOn;
            Account.tryLoginCount =Convert.ToInt32( txtlogin.Value);
            Account.tryLoginFalse = Convert.ToInt32(txtfalse.Value);
        }
        void GetInfo()
        {
            if (Account == null)
                Account = new Model.发布相关模型.UserAccount();
            isRandom.IsOn = Account.randomget;
            txtlogin.Value = Account.tryLoginCount;
            txtfalse.Value = Account.tryLoginFalse;
        }

        public void LoadAccount()
        {
            if (radio_all.Checked)
            {
                Coll.Clear();
                string[] dataArray = new string[Account.AccountTrue.Values.Count];
                Account.AccountTrue.Keys.CopyTo(dataArray, 0);
                for (int i = 0; i < dataArray.Length; i++)
                {
                    if (dataArray[i] == Account.AccountTrue[dataArray[i]].username)
                    {
                        Record r = new Record();
                        r.账号 = Account.AccountTrue[dataArray[i]].username;
                        r.密码 = Account.AccountTrue[dataArray[i]].password;
                        r.附加值1 = Account.AccountTrue[dataArray[i]].loginvalue1;
                        r.附加值2 = Account.AccountTrue[dataArray[i]].loginvalue2;
                        r.最近活动时间 = Account.AccountTrue[dataArray[i]].LastActiveTime.ToString();
                        r.状态 = Account.AccountTrue[dataArray[i]].Status;
                        r.异常计数 = Account.AccountTrue[dataArray[i]].CountFalse.ToString();
                        Coll.Add(r);
                    }
                    else
                    {
                        Model.发布相关模型.Account ac = new Model.发布相关模型.Account();
                        ac.username = Account.AccountTrue[dataArray[i]].username;
                        ac.password = Account.AccountTrue[dataArray[i]].password;
                        ac.loginvalue1 = Account.AccountTrue[dataArray[i]].loginvalue1;
                        ac.loginvalue2 = Account.AccountTrue[dataArray[i]].loginvalue2;
                        ac.Status = "未知";
                        Account.AccountTrue.Remove(dataArray[i]);
                        if (!Account.AccountTrue.ContainsKey(ac.username))
                        {
                            Account.AccountTrue.Add(ac.username, ac);
                            Record r = new Record();
                            r.账号 = ac.username;
                            r.密码 = ac.password;
                            r.附加值1 = ac.loginvalue1;
                            r.附加值2 = ac.loginvalue2;
                            r.最近活动时间 = ac.LastActiveTime.ToString();
                            r.状态 = ac.Status;
                            r.异常计数 = ac.CountFalse.ToString();
                            Coll.Add(r);
                        }
                    }
                }
                dataArray = new string[Account.AccountFalse.Values.Count];
                Account.AccountFalse.Keys.CopyTo(dataArray, 0);
                for (int i = 0; i < dataArray.Length; i++)
                {
                    if (dataArray[i] == Account.AccountFalse[dataArray[i]].username)
                    {
                        Record r = new Record();
                        r.账号 = Account.AccountFalse[dataArray[i]].username;
                        r.密码 = Account.AccountFalse[dataArray[i]].password;
                        r.附加值1 = Account.AccountFalse[dataArray[i]].loginvalue1;
                        r.附加值2 = Account.AccountFalse[dataArray[i]].loginvalue2;
                        r.最近活动时间 = Account.AccountFalse[dataArray[i]].LastActiveTime.ToString();
                        r.状态 = Account.AccountFalse[dataArray[i]].Status;
                        r.异常计数 = Account.AccountFalse[dataArray[i]].CountFalse.ToString();
                        Coll.Add(r);
                    }
                    else
                    {
                        Model.发布相关模型.Account ac = new Model.发布相关模型.Account();
                        ac.username = Account.AccountFalse[dataArray[i]].username;
                        ac.password = Account.AccountFalse[dataArray[i]].password;
                        ac.loginvalue1 = Account.AccountFalse[dataArray[i]].loginvalue1;
                        ac.loginvalue2 = Account.AccountFalse[dataArray[i]].loginvalue2;
                        ac.Status = "未知";
                        Account.AccountFalse.Remove(dataArray[i]);
                        if (!Account.AccountFalse.ContainsKey(ac.username))
                        {
                            Account.AccountFalse.Add(ac.username, ac);
                            Record r = new Record();
                            r.账号 = ac.username;
                            r.密码 = ac.password;
                            r.附加值1 = ac.loginvalue1;
                            r.附加值2 = ac.loginvalue2;
                            r.最近活动时间 = ac.LastActiveTime.ToString();
                            r.状态 = ac.Status;
                            r.异常计数 = ac.CountFalse.ToString();
                            Coll.Add(r);
                        }
                    }
                }
            }
            else if (radio_normal.Checked)
            {
                Coll.Clear();
                string[] dataArray = new string[Account.AccountTrue.Values.Count];
                Account.AccountTrue.Keys.CopyTo(dataArray, 0);
                for (int i = 0; i < dataArray.Length; i++)
                {
                    if (dataArray[i] == Account.AccountTrue[dataArray[i]].username)
                    {
                        Record r = new Record();
                        r.账号 = Account.AccountTrue[dataArray[i]].username;
                        r.密码 = Account.AccountTrue[dataArray[i]].password;
                        r.附加值1 = Account.AccountTrue[dataArray[i]].loginvalue1;
                        r.附加值2 = Account.AccountTrue[dataArray[i]].loginvalue2;
                        r.最近活动时间 = Account.AccountTrue[dataArray[i]].LastActiveTime.ToString();
                        r.状态 = Account.AccountTrue[dataArray[i]].Status;
                        r.异常计数 = Account.AccountTrue[dataArray[i]].CountFalse.ToString();
                        Coll.Add(r);
                    }
                    else
                    {
                        Model.发布相关模型.Account ac = new Model.发布相关模型.Account();
                        ac.username = Account.AccountTrue[dataArray[i]].username;
                        ac.password = Account.AccountTrue[dataArray[i]].password;
                        ac.loginvalue1 = Account.AccountTrue[dataArray[i]].loginvalue1;
                        ac.loginvalue2 = Account.AccountTrue[dataArray[i]].loginvalue2;
                        ac.Status = "未知";
                        Account.AccountTrue.Remove(dataArray[i]);
                        if (!Account.AccountTrue.ContainsKey(ac.username))
                        {
                            Account.AccountTrue.Add(ac.username, ac);
                            Record r = new Record();
                            r.账号 = ac.username;
                            r.密码 = ac.password;
                            r.附加值1 = ac.loginvalue1;
                            r.附加值2 = ac.loginvalue2;
                            r.最近活动时间 = ac.LastActiveTime.ToString();
                            r.状态 = ac.Status;
                            r.异常计数 = ac.CountFalse.ToString();
                            Coll.Add(r);
                        }
                    }
                }
            }
            else if (radio_error.Checked)
            {
                Coll.Clear();
                string[] dataArray = new string[Account.AccountFalse.Values.Count];
                Account.AccountFalse.Keys.CopyTo(dataArray, 0);
                for (int i = 0; i < dataArray.Length; i++)
                {
                    if (dataArray[i] == Account.AccountFalse[dataArray[i]].username)
                    {
                        Record r = new Record();
                        r.账号 = Account.AccountFalse[dataArray[i]].username;
                        r.密码 = Account.AccountFalse[dataArray[i]].password;
                        r.附加值1 = Account.AccountFalse[dataArray[i]].loginvalue1;
                        r.附加值2 = Account.AccountFalse[dataArray[i]].loginvalue2;
                        r.最近活动时间 = Account.AccountFalse[dataArray[i]].LastActiveTime.ToString();
                        r.状态 = Account.AccountFalse[dataArray[i]].Status;
                        r.异常计数 = Account.AccountFalse[dataArray[i]].CountFalse.ToString();
                        Coll.Add(r);
                    }
                    else
                    {
                        Model.发布相关模型.Account ac = new Model.发布相关模型.Account();
                        ac.username = Account.AccountFalse[dataArray[i]].username;
                        ac.password = Account.AccountFalse[dataArray[i]].password;
                        ac.loginvalue1 = Account.AccountFalse[dataArray[i]].loginvalue1;
                        ac.loginvalue2 = Account.AccountFalse[dataArray[i]].loginvalue2;
                        ac.Status = "未知";
                        Account.AccountFalse.Remove(dataArray[i]);
                        if (!Account.AccountFalse.ContainsKey(ac.username))
                        {
                            Account.AccountFalse.Add(ac.username, ac);
                            Record r = new Record();
                            r.账号 = ac.username;
                            r.密码 = ac.password;
                            r.附加值1 = ac.loginvalue1;
                            r.附加值2 = ac.loginvalue2;
                            r.最近活动时间 = ac.LastActiveTime.ToString();
                            r.状态 = ac.Status;
                            r.异常计数 = ac.CountFalse.ToString();
                            Coll.Add(r);
                        }
                    }
                }
            }
            gridControl_main.DataSource = Coll;
            gridControl_main_view.RefreshData();
            gridControl_main_view.Columns["账号"].VisibleIndex = 0;
            gridControl_main_view.Columns["密码"].VisibleIndex = 1;
            gridControl_main_view.Columns["附加值1"].VisibleIndex = 2;
            gridControl_main_view.Columns["附加值2"].VisibleIndex = 3;
            gridControl_main_view.Columns["异常计数"].VisibleIndex = 4;
            gridControl_main_view.Columns["最近活动时间"].VisibleIndex = 5;
            gridControl_main_view.Columns["状态"].VisibleIndex = 6;
            statusbar.Caption = "共有账号" + (Account.AccountTrue.Count + Account.AccountFalse.Count) + "个，其中正常账号" + Account.AccountTrue.Count + "个，异常账号" + Account.AccountFalse.Count + "个，您可以检查下原因把异常账号变为正常账号或删除掉！";
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
        private void GetMajia()
        {
            try
            {
                int mykey = (DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour);
                //实例一个process类
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //设定程序名
                process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\xBrowser.exe";
                process.StartInfo.Arguments = mykey + " " + "http://www.xiake.org";
                //关闭Shell的使用
                process.StartInfo.UseShellExecute = false;
                //重新定向标准输入，输入，错误输出
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                //设置cmd窗口不显示
                process.StartInfo.CreateNoWindow = true;
                //开始
                process.Start();
                string strRst = process.StandardOutput.ReadToEnd();
                string[] strs = strRst.Replace("```", "ヵ").Split('ヵ');
                if (strs.Length != 2)
                    return;
                string cookie = strs[0];
                string url = strs[1].Replace("\r\n", "").Trim();
                Majiaac.Majiaurl = url;
                Majiaac.Majiastr = cookie;
                Majiaac.MyCookie = new xEngine.Model.Execute.Http.XCookieManager();
                xEngine.Execute.Http execute = new xEngine.Execute.Http();
                execute.CookieManager = Majiaac.MyCookie;
                execute.CookieAddStr(cookie);

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("没有能成功提取到马甲，原因：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                CloseI();
            }
        }
        private void getPlan_Load(object sender, EventArgs e)
        {
            GetInfo();
            LoadAccount();
        }
        private void UserAccount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CloseI();
               

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
            IsSave = true;
            SetInfo();
            this.Close();

        }

        private void barButtonItem_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void radio_all_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_all.Checked)
            {
                LoadAccount();
            }
        }

        private void radio_normal_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_normal.Checked)
            {
                LoadAccount();
            }
        }

        private void radio_error_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_error.Checked)
            {
                LoadAccount();
            }
        }

        private void btn_add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            V3.V3Form.frm_参数输入编辑器 frm = new frm_参数输入编辑器();
            frm.Text = "账号批量添加";
            frm.txttitle.Caption = "一行一组账号，账号属性用|间隔";
            frm.txtStatus.Caption = "格式：账号|密码 或 账号|密码|附加值1 或 账号|密码|附加值1|附加值2  一行一组!";
            frm.ShowDialog();
            if (frm.issave)
            {
                string[] temp = frm.txtMainbox.Lines;
                for (int i = 0; i < temp.Length; i++)
                {
                    string[] aaa = temp[i].Split('|');
                    Model.发布相关模型.Account tempac = new Model.发布相关模型.Account();
                    try
                    {
                        tempac.LastActiveTime = DateTime.Now;
                        tempac.Status = "未知";
                        tempac.username = aaa[0];
                        tempac.password = aaa[1];
                        tempac.loginvalue1 = aaa[2];
                        tempac.loginvalue2 = aaa[3];
                    }
                    catch { }
                    if (aaa.Length > 1)
                    {
                        Account.AddorUpdateAccount(tempac, true);
                    }
                }
                LoadAccount();
            }
        }

        private void btn_edit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridControl_main_view.SelectedRowsCount > 0)
            {
                Record r = (Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);
                UserInput frm = new UserInput();
                Model.发布相关模型.Account ac = new Model.发布相关模型.Account();
                if (Account.AccountTrue.ContainsKey(r.账号))
                    ac = Account.AccountTrue[r.账号];
                else
                    ac = Account.AccountFalse[r.账号];
                frm.username.Text = ac.username;
                frm.password.Text = ac.password;
                frm.value1.Text = ac.loginvalue1;
                frm.value2.Text = ac.loginvalue2;
                frm.ShowDialog();
                if (frm.issave)
                {
                    ac.username = frm.username.Text;
                    ac.password = frm.password.Text;
                    ac.loginvalue1 = frm.value1.Text;
                    ac.loginvalue2 = frm.value2.Text;
                    LoadAccount();
                }
            }
        }

        private void btn_delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridControl_main_view.SelectedRowsCount > 0)
            {

                for (int i = 0; i < gridControl_main_view.SelectedRowsCount; i++)
                {
                    Record r = (Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[i]);
                    if (Account.AccountTrue.ContainsKey(r.账号))
                        Account.AccountTrue.Remove(r.账号);
                    if (Account.AccountFalse.ContainsKey(r.账号))
                        Account.AccountFalse.Remove(r.账号);
                }
                LoadAccount();
            }
        }

        private void btn_normal_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridControl_main_view.SelectedRowsCount > 0)
            {
                
                for (int i = 0; i < gridControl_main_view.SelectedRowsCount; i++)
                {Record r = (Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[i]);
                    if (Account.AccountFalse.ContainsKey(r.账号))
                        Account.AddorUpdateAccount(Account.AccountFalse[r.账号], true);
                 
                }
                LoadAccount();
            }
        }

        private void btn_cookie_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetInfo();
            if (gridControl_main_view.SelectedRowsCount > 0)
            {
                Record r = (Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);
                if (Account.AccountTrue.ContainsKey(r.账号))
                    Majiaac = Account.AccountTrue[r.账号];
                if (Account.AccountFalse.ContainsKey(r.账号))
                    Majiaac = Account.AccountFalse[r.账号];
                //ShowI("正在提取马甲");
                //t = new System.Threading.Thread(GetMajia);
                //t.IsBackground = true;
                //t.Start();
                frmGetCookie f = new frmGetCookie();
                f.ShowDialog();
                if (f.GetSuccess)
                {
                    Majiaac.Majiaurl = f.Url;
                    Majiaac.Majiastr = f.CookieStr;
                    Majiaac.MyCookie = new xEngine.Model.Execute.Http.XCookieManager();
                    xEngine.Execute.Http execute = new xEngine.Execute.Http();
                    execute.CookieManager = Majiaac.MyCookie;
                    execute.CookieAddStr(f.CookieStr);

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

        private void gridControl_main_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btn_edit_ItemClick(null, null);
        }

        private void txtfalse_EditValueChanged(object sender, EventArgs e)
        {
            if (txtlogin.Value <= 4)
            {
                XtraMessageBox.Show( "这个值不能小于5！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtlogin.Value = 5;
            }
        }

        private void txtlogin_EditValueChanged(object sender, EventArgs e)
        {
            if (txtlogin.Value > txtfalse.Value)
            {
                XtraMessageBox.Show( "这个值不能大于失败阀值数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtlogin.Value = txtfalse.Value;
            }
            if (txtlogin.Value <= 2)
            {
                XtraMessageBox.Show( "这个值不能小于3！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtlogin.Value = 1;
            }
        }


    }
}
