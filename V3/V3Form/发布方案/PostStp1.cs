using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Model;

namespace V3.V3Form.发布模块
{
    public partial class PostStp1 : DevExpress.XtraEditors.XtraForm
    {
        public static PostStp1 MyPostStp1;
        public bool IsShow = false;
        public string OldHtml = string.Empty;
        public System.Threading.Thread t;
        public Model.GetPostModel Model = new Model.GetPostModel();
        public V3.Bll.PostBll Bll;
        public bool IsSave = false;
        #region 数据模型
        public static int ColumnCount =4;
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
          
            public void Remove(Record r)
            {
                List.Remove(r);
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
            object[] values = new object[4];
            public int 编号 { get { return Convert.ToInt32(values[0]); } set { SetValue(0, value); } }
            public string 类型 { get { return values[1].ToString(); } set { SetValue(1, value); } }
            public string 地址 { get { return values[2].ToString(); } set { SetValue(2, value); } }
            public Model.发布相关模型.GetPostAction 规则 { get { return (Model.发布相关模型.GetPostAction)values[3]; } set { SetValue(3, value); } }
            public object GetValue(int index)
            {
                return values[index];
            }
            //<label1>
            public void SetValue(int index, object val)
            {
                values[index] = val;
                if (this.owner != null) this.owner.OnListChanged(this);
            }
            //</label1>
        }
        #endregion
        public PostStp1()
        {
            InitializeComponent();
            MyPostStp1 = this;
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
            Model.Ismajiamodel = isMajiaModel.IsOn;
            Model.Stp1_POST_NeedLogin = NeedLogin.IsOn;
            Model.Stp1_POST_NeedLoginMore = NeedLoginmore.IsOn;
            Model.Stp1_POST_VcheckcodeUrl = txtvcheckcodeurl.Text;
            Model.Stp1_POST_LoginAction = new Model.发布相关模型.GetPostAction[Coll.Count];
            for (int i = 0; i < Coll.Count; i++)
            {
                Model.Stp1_POST_LoginAction[i] = Coll[i].规则;
            }
            System.Collections.ArrayList temp = new System.Collections.ArrayList();
            string[] tempstr = txttrue.Lines;
            for (int i = 0; i < tempstr.Length; i++)
            {
                if (tempstr[i].Length > 1)
                    temp.Add(tempstr[i]);
            }
            Model.Stp1_POST_Truetag = new string[temp.Count];
            for (int i = 0; i < temp.Count; i++)
            {
                Model.Stp1_POST_Truetag[i] = temp[i].ToString();
            }

            temp.Clear();
            tempstr = txtfalse.Lines;
            for (int i = 0; i < tempstr.Length; i++)
            {
                if (tempstr[i].Length > 1)
                    temp.Add(tempstr[i]);
            }
            Model.Stp1_POST_Falsetag = new string[temp.Count];
            for (int i = 0; i < temp.Count; i++)
            {
                Model.Stp1_POST_Falsetag[i] = temp[i].ToString();
            }
            Model.Stp1_POST_VcodeModel = swNeedLoginVcode.IsOn;
            Bll.account.Majiastr = Model.Majiastr;
        }
        void GetInfo()
        {

            if (Model == null)
                Model = new Model.GetPostModel();
            isMajiaModel.IsOn = Model.Ismajiamodel;
            NeedLogin.IsOn = Model.Stp1_POST_NeedLogin;
            NeedLoginmore.IsOn = Model.Stp1_POST_NeedLoginMore;
            txtvcheckcodeurl.Text = Model.Stp1_POST_VcheckcodeUrl;

            Coll.Clear();
            if (Model.Stp1_POST_LoginAction!=null)
            {
                for (int i = 0; i < Model.Stp1_POST_LoginAction.Length; i++)
                {
                    Record r = new Record();
                    r.地址 = Model.Stp1_POST_LoginAction[i].ActionUrl;
                    r.编号 = i + 1;
                    r.规则 = Model.Stp1_POST_LoginAction[i];
                    r.类型 = Model.Stp1_POST_LoginAction[i].IsPost ? "POST" : "GET";
                    Coll.Add(r);
                }
            }
           
            gridControl_main.DataSource = Coll;
            gridControl_main_view.RefreshData();
            gridControl_main_view.Columns["规则"].Visible = false;
            gridControl_main_view.Columns["编号"].VisibleIndex = 0;
            gridControl_main_view.Columns["类型"].VisibleIndex = 0;
            gridControl_main_view.Columns["编号"].Width = 20;
            gridControl_main_view.Columns["类型"].Width = 20;
            txttrue.Text = string.Empty;
            if (Model.Stp1_POST_Truetag==null)
            {
                Model.Stp1_POST_Truetag=new string[0];
            }
            for (int i = 0; i < Model.Stp1_POST_Truetag.Length; i++)
            {
                txttrue.Text += Model.Stp1_POST_Truetag[i] + "\r\n";
            }
            txtfalse.Text = string.Empty;
            if (Model.Stp1_POST_Falsetag==null)
            {
                Model.Stp1_POST_Falsetag=new string[0];
            }
            for (int i = 0; i < Model.Stp1_POST_Falsetag.Length; i++)
            {
                txtfalse.Text += Model.Stp1_POST_Falsetag[i] + "\r\n";
            }
            swNeedLoginVcode.IsOn = Model.Stp1_POST_VcodeModel;
        }

        void Start()
        {
            this.Invoke((EventHandler)(delegate
            {
                btnTestAll.Enabled = false;
                btnTestAll.Text = "正在测试";
                btnTestLogin.Enabled = false;
                btnTestLogin.Text = "正在测试";
            }));
            SetInfo();
            string result = Bll.LoginStart(true);
            if (IsShow)
            {
                this.Invoke((EventHandler)(delegate
                {
                    htmlEditor2.BodyHtml = result;
                    btnTestAll.Enabled = true;
                    btnTestAll.Text = "测试全部脚本";
                    btnTestLogin.Enabled = true;
                    btnTestLogin.Text = "测试登录";
                }));
            }


            CloseI();

        }

        void StartLogin()
        {
            this.Invoke((EventHandler)(delegate
            {
                btnTestAll.Enabled = false;
                btnTestAll.Text = "正在测试";
                btnTestLogin.Enabled = false;
                btnTestLogin.Text = "正在测试";
            }));
            SetInfo();
            string html = "";
            string result = Bll.Login(true, ref html);
            if (result.Contains("LoginOK"))
            {

                if (IsShow)
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        XtraMessageBox.Show("登录成功！", "登录成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        htmlEditor2.BodyHtml = html;
                        btnTestAll.Enabled = true;
                        btnTestAll.Text = "测试全部脚本";
                        btnTestLogin.Enabled = true;
                        btnTestLogin.Text = "测试登录";
                    }));
                }
            }
            else
            {

                if (IsShow)
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        XtraMessageBox.Show("登录失败，原因：" + result, "登录失败", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        htmlEditor2.BodyHtml = html;
                        btnTestAll.Enabled = true;
                        btnTestAll.Text = "测试全部脚本";
                        btnTestLogin.Enabled = true;
                        btnTestLogin.Text = "测试登录";
                    }));
                }

            }


            CloseI();

        }
        void GetMajia()
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
               

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("未能提取到马甲，原因：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                CloseI();
            }
        }

        private void getPlan_Load(object sender, EventArgs e)
        {
            IsShow = true;
            GetInfo();
           
        }

        private void btnTestAll_Click(object sender, EventArgs e)
        {
            SetInfo();
            if (Coll.Count == 0)
            {
                XtraMessageBox.Show( "请先添加至少一条脚本！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
             t= new System.Threading.Thread(Start);
            t.IsBackground = true;
            t.Start();
            ShowI("正在下载数据");
           
        }
        private void btnTestLogin_Click(object sender, EventArgs e)
        {
            SetInfo();
            if (Coll.Count == 0)
            {
                XtraMessageBox.Show( "请先添加至少一条脚本！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            t = new System.Threading.Thread(StartLogin);
            t.IsBackground = true;
            t.Start();
            ShowI("正在登陆");
            
        }

        private void txtvcheckcodeurl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (Program.f_frmReplaceTag == null || Program.f_frmReplaceTag.IsDisposed)
                {
                    Program.f_frmReplaceTag = new frmReplaceTag();
                }
                Program.f_frmReplaceTag.referer = "PostStp1_txtvcheckcodeurl";
                Program.f_frmReplaceTag.Focus();
                Program.f_frmReplaceTag.Show();
                Program.f_frmReplaceTag.Location = new Point(0, 0);
            }

           
        }

        private void btnGetMajia_Click(object sender, EventArgs e)
        {
            if (!isMajiaModel.IsOn)
            {
                XtraMessageBox.Show( "当前模式不是马甲模式，不用提取马甲！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                frmGetCookie f = new frmGetCookie();
                f.ShowDialog();
                if (f.GetSuccess)
                {
                    Model.Majiastr = f.CookieStr;
                    Model.Majiaurl = f.Url;

                    Model.POST_TestAccount.MyCookie = new xEngine.Model.Execute.Http.XCookieManager();
                    xEngine.Execute.Http execute = new xEngine.Execute.Http();
                    execute.CookieManager = Model.POST_TestAccount.MyCookie;
                    execute.CookieAddStr(f.CookieStr);
                }
            }

        }
        private void PostStp1_KeyDown(object sender, KeyEventArgs e)//停止
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnTestAll.Enabled = true;
                btnTestAll.Text = "测试全部脚本";
                btnTestLogin.Enabled = true;
                btnTestLogin.Text = "测试登录";
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

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IsSave = true;
            SetInfo();
            this.Close();
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
          
            IsSave = false;
            this.Close();
        }

        private void PostStp1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed )
            {
                Program.f_frmReplaceTag.Close();
            }
          

        }

        private void txtvcheckcodeurl_MouseUp(object sender, MouseEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed )
            {
                Program.f_frmReplaceTag.referer = "PostStp1_txtvcheckcodeurl";
            }
        }

        private void btn_AddGet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
         
            Pget frm = new Pget();
            frm.Model = this.Model;
            frm.OldHtml = this.OldHtml;
            frm.ShowDialog();
            if (frm.IsSave)
            {
                Record r=new Record();
                r.编号 = Coll.Count + 1;
                r.类型 = "GET";
                r.地址 = frm.Action.ActionUrl;
                r.规则 = frm.Action;
                Coll.Add(r);
                SetInfo();
                GetInfo();
              
            }
          

        }

        private void btn_AddPost_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           
            Ppost frm = new Ppost();
            frm.Model = this.Model;
            frm.OldHtml = this.OldHtml;
            frm.ShowDialog();
            if (frm.IsSave)
            {
                Record r = new Record();
                r.编号 = Coll.Count + 1;
                r.类型 = "POST";
                r.地址 = frm.Action.ActionUrl;
                r.规则 = frm.Action;
                Coll.Add(r);
                SetInfo();
                GetInfo();
              
                
            }
           
        }

        private void btn_Edit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridControl_main_view.SelectedRowsCount == 1)
            {
               
                Record r = (Record) gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);
                Model.发布相关模型.GetPostAction temp = r.规则;
                if (temp.IsPost)
                {
                    Ppost frm = new Ppost();
                    frm.Action = temp;
                    frm.OldHtml = OldHtml;
                    frm.Model = this.Model;
                    frm.ShowDialog();
                    if (frm.IsSave)
                    {
                       r.规则 = frm.Action;
                    }
                }
                else
                {
                    Pget frm = new Pget();
                    frm.Action = temp;
                    frm.OldHtml = OldHtml;
                    frm.Model = this.Model;
                    frm.ShowDialog();
                    if (frm.IsSave)
                    {
                        r.规则 = frm.Action;
                    }
                }
                SetInfo();
                GetInfo();
              
            }
            else
            {
                XtraMessageBox.Show( "请先选中至少一条脚本！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            
        }

        private void btn_Delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridControl_main_view.SelectedRowsCount==1)
            {
                Record r = (Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);
                Coll.Remove(r);
                gridControl_main_view.RefreshData();
            }
  

        }

        private void gridControl_main_DoubleClick(object sender, EventArgs e)
        {
            btn_Edit_ItemClick(null,null);
        }

        private void gridControl_main_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Right)
            {
                popupMenu.ShowPopup(Control.MousePosition);
            }
        }

        private void PostStp1_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsShow = false;
        }

        private void barButtonItem_UP_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridControl_main_view.SelectedRowsCount == 1)
            {
                Record r = (Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);
                int oldIndex = r.Index;
                if (r.Index - 1 >= 0)
                {
                    int newIndex = r.Index - 1;
                    Record rUp = Coll[newIndex];
                    int tUp编号 = rUp.编号;
                    int r编号 = r.编号;
                    rUp.编号 = r编号;
                    r.编号 = tUp编号;
                    rUp.Index = oldIndex;
                    r.Index = newIndex;
                    List<Record> list = new List<Record>();
                    IEnumerator IEnum = Coll.GetEnumerator();
                    while (IEnum.MoveNext())
                    {
                        list.Add((Record)IEnum.Current);
                    }
                    Coll.Clear();
                    for (int i = 0; i < list.Count;i++)
                    {
                        if (i == newIndex)
                        {
                            Coll.Add(r);
                        }
                        else if(i==oldIndex)
                        {
                            Coll.Add(rUp);
                        
                        }
                        else 
                        {
                            Coll.Add(list[i]);
                        }
                    }


                }
              
                gridControl_main_view.RefreshData();
            }
        }

        private void barButtonItem_Down_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridControl_main_view.SelectedRowsCount == 1)
            {
                Record r = (Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);
                int oldIndex = r.Index;
                if (r.Index<Coll.Count-1)
                {
                    int newIndex = r.Index + 1;
                    Record rDown = Coll[newIndex];
                    int rDown编号 = rDown.编号;
                    int r编号 = r.编号;
                    rDown.编号 = r编号;
                    r.编号 = rDown编号;
                    rDown.Index = oldIndex;
                    r.Index = newIndex;
                    List<Record> list = new List<Record>();
                    IEnumerator IEnum = Coll.GetEnumerator();
                    while (IEnum.MoveNext())
                    {
                        list.Add((Record)IEnum.Current);
                    }
                    Coll.Clear();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (i == newIndex)
                        {
                            Coll.Add(r);
                        }
                        else if (i == oldIndex)
                        {
                            Coll.Add(rDown);

                        }
                        else
                        {
                            Coll.Add(list[i]);
                        }
                    }


                }

                gridControl_main_view.RefreshData();
            }
        }


    }
}
