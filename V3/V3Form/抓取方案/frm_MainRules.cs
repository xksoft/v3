using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using DevExpress.XtraEditors;
using xEngine.Model.Execute.Http;

namespace V3.V3Form
{
    public partial class MainRules : DevExpress.XtraEditors.XtraForm
    {
        #region 数据模型

        public static int ColumnCount = 3;
        public static RecordCollection Coll = new RecordCollection();

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
            private object[] values = new object[3];

            public int 编号
            {
                get { return Convert.ToInt32(values[0]); }
                set { SetValue(0, value); }
            }

            public string 规则名称
            {
                get { return values[1].ToString(); }
                set { SetValue(1, value); }
            }

            public Model.抓取相关模型.RulesEngLv2 规则
            {
                get { return (Model.抓取相关模型.RulesEngLv2) values[2]; }
                set { SetValue(2, value); }
            }

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

        public ArrayList resultList=new ArrayList();
        public void showI(string txt)
        {
            this.Invoke((EventHandler) (delegate
            {
                ipanel.Visible = true;
                istate.Text = txt;
            }));

        }

        public void closeI()
        {
            if (isShow)
            {
                this.Invoke((EventHandler) (delegate
                {
                    ipanel.Visible = false;
                }));
            }

        }

        public MainRules()
        {
            InitializeComponent();
        }

        public Model.抓取相关模型.RulesEngLv1 ruleslv1 = new Model.抓取相关模型.RulesEngLv1();
        public string cookietxt = "";
        public bool needmore = false;
        public bool isSave = false;
        public bool isShow = false;

        private void GetInfo()
        {
            try
            {
                Coll.Clear();
                modelck1.Checked = !ruleslv1.IsRgx;
                isOrorAnd.IsOn = ruleslv1.IsOrorAnd;
                textBoxX1.Text = ruleslv1.Shengchengtou;
                textBoxX2.Text = ruleslv1.Toucong.ToString();
                textBoxX3.Text = ruleslv1.Toudao.ToString();
                textBoxX4.Text = ruleslv1.Zizeng.ToString();
                if (ruleslv1.GetModel == 1)
                    chk1_1.Checked = true;
                else if (ruleslv1.GetModel == 2)
                    chk1_2.Checked = true;
                else if (ruleslv1.GetModel == 3)
                    chk1_3.Checked = true;
                else checkBoxX2.Checked = true;
                if (ruleslv1.OutModel == 1)
                    chk3_1.Checked = true;
                else if (ruleslv1.OutModel == 2)
                    chk3_2.Checked = true;
                else if (ruleslv1.OutModel == 3)
                    chk3_3.Checked = true;
                else
                    chk3_4.Checked = true;
                if (ruleslv1.IsRgx)
                    modelck2.Checked = true;
                txtHebingStr.Text = ruleslv1.HebingStr;
                txttesturl.Text = ruleslv1.TestUrl;
                txtrulesname.Text = ruleslv1.RulesName;
                // ruleslv1.CheckStr1 = txtcheckstr1.Text.Trim('|').Split('|');
                // ruleslv1.CheckStr2 = txtcheckstr2.Text.Trim('|').Split('|');
                txtinhead.Text = ruleslv1.inhead;
                txtinfoot.Text = ruleslv1.infoot;
                txtcheckstr1.Text = "";
                txttesturl.Text = ruleslv1.TestUrl;
                for (int i = 0; i < ruleslv1.CheckStr1.Length; i++)
                {
                    if (i != ruleslv1.CheckStr1.Length - 1)
                        txtcheckstr1.Text += ruleslv1.CheckStr1[i] + "|";
                    else
                        txtcheckstr1.Text += ruleslv1.CheckStr1[i];
                }
                txtcheckstr2.Text = "";
                for (int i = 0; i < ruleslv1.CheckStr2.Length; i++)
                {
                    if (i != ruleslv1.CheckStr2.Length - 1)
                        txtcheckstr2.Text += ruleslv1.CheckStr2[i] + "|";
                    else
                        txtcheckstr2.Text += ruleslv1.CheckStr2[i];
                }
                isformat.IsOn = ruleslv1.IsFormat;
                ishtml.IsOn = ruleslv1.IsHtml;

                try
                {
                    this.Invoke((EventHandler) (delegate
                    {

                        for (int i = 0; i < ruleslv1.MyRules.Length; i++)
                        {
                            Record r = new Record();
                            r.编号 = i + 1;
                            r.规则名称 = ruleslv1.MyRules[i].Readme;
                            r.规则 = ruleslv1.MyRules[i];
                            Coll.Add(r);

                        }
                        gridControl_main.DataSource = Coll;
                        gridControl_main_view.RefreshData();
                        gridControl_main_view.Columns["编号"].Width = 50;
                        gridControl_main_view.Columns["规则"].Visible = false;
                        gridControl_main_view.Columns["编号"].VisibleIndex = 0;


                    }));
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString());
            }
        }

        private void SetInfo()
        {
            try
            {

                try
                {
                    ruleslv1.Toucong = Convert.ToInt32(textBoxX2.Text);
                    ruleslv1.Toudao = Convert.ToInt32(textBoxX3.Text);
                    ruleslv1.Zizeng = Convert.ToInt32(textBoxX4.Text);
                }
                catch
                {
                    ruleslv1.Toucong = 0;
                    ruleslv1.Toudao = 0;
                    ruleslv1.Zizeng = 0;
                }
                ruleslv1.Shengchengtou = textBoxX1.Text;
                ruleslv1.IsOrorAnd = isOrorAnd.IsOn;
                ruleslv1.Bianma = 3;
                if (chk1_1.Checked)
                    ruleslv1.GetModel = 1;
                else if (chk1_2.Checked)
                    ruleslv1.GetModel = 2;
                else if (checkBoxX2.Checked)
                    ruleslv1.GetModel = 4;
                else
                    ruleslv1.GetModel = 3;
                if (chk3_1.Checked)
                    ruleslv1.OutModel = 1;
                else if (chk3_2.Checked)
                    ruleslv1.OutModel = 2;
                else if (chk3_3.Checked)
                    ruleslv1.OutModel = 3;
                else
                    ruleslv1.OutModel = 4;
                ruleslv1.HebingStr = txtHebingStr.Text;
                ruleslv1.TestUrl = txttesturl.Text;
                ruleslv1.MyRules = new Model.抓取相关模型.RulesEngLv2[Coll.Count];
                ruleslv1.RulesName = txtrulesname.Text;
                ruleslv1.CheckStr1 = txtcheckstr1.Text.Trim('|').Split('|');
                if (ruleslv1.CheckStr1[0] == "")
                    ruleslv1.CheckStr1 = new string[0];
                ruleslv1.CheckStr2 = txtcheckstr2.Text.Trim('|').Split('|');
                if (ruleslv1.CheckStr2[0] == "")
                    ruleslv1.CheckStr2 = new string[0];
                ruleslv1.IsFormat = isformat.IsOn;
                ruleslv1.IsHtml = ishtml.IsOn;
                ruleslv1.inhead = txtinhead.Text;
                ruleslv1.infoot = txtinfoot.Text;
                if (modelck1.Checked)
                    ruleslv1.IsRgx = false;
                else
                    ruleslv1.IsRgx = true;
                for (int i = 0; i < Coll.Count; i++)
                {
                    ruleslv1.MyRules[i] = Coll[i].规则;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString());
            }
        }


        private void testone()
        {
            if ((gridControl_main_view.SelectedRowsCount > 0 || ruleslv1.GetModel != 1) && ruleslv1.GetModel != 4)
            {
                showI("正在测试指定规则...");
                string result = gethtml();
                statusbar.Caption = "正在加载" + txttesturl.Text + "的网页代码...";
                if (isShow)
                {
                    this.Invoke((EventHandler) (delegate
                    {
                        htmlEditor1.BodyHtml = result;
                    }));
                }
                statusbar.Caption = "网页代码加载完毕！";
                V3.Bll.GetBll get1 = new Bll.GetBll(result, ruleslv1);
                get1.oldurl = txttesturl.Text;
                if (isShow)
                {
                    this.Invoke((EventHandler) (delegate
                    {
                        if (gridControl_main_view.SelectedRowsCount > 0)
                        {
                            Record r = (Record) gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);
                            htmlEditor2.BodyHtml = get1.testEngRules(r.规则);
                        }
                        statusbar.Caption = "单条规则测试完毕！";
                    }));
                }
               
                closeI();
            }

            else
            {
                this.Invoke((EventHandler) (delegate
                {
                    XtraMessageBox.Show("请先选中或添加至少一条子规则！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            }
        }

        private string gethtml()
        {
            statusbar.Caption = "正在加载" + txttesturl.Text + "的源代码...";
            string url = txttesturl.Text;

            xEngine.Execute.Http execute = new xEngine.Execute.Http();
            execute.CookieAddStr(cookietxt);
            execute.LoadScript(Properties.Resources.get, false);
            execute.Scripts[0].Url = url;
            execute.Scripts[0].Referer = "";
            execute.Scripts[0].Encoding = Encoding.Default.WebName;
            execute.IsAutoEncoding = true;
            XResponse response = execute.RunRequest(execute.Scripts[0]);
            string result = "源地址：" + execute.Scripts[0].Url + "\r\n\r\n" + Library.HtmlHelper.GetHtmlFromByte(response.BodyData, response.ContentType);


            statusbar.Caption = "加载完毕!";
            return result;
        }

        private void gethtmlstr()
        {
            if (isShow)
            {
                this.Invoke((EventHandler) (delegate
                {
                    buttonX1.Enabled = false;
                    buttonX2.Enabled = false;
                    buttonX3.Enabled = false;
                    statusbar.Caption = "正在加载" + txttesturl.Text + "的网页代码...";
                }));
            }
            string result = gethtml();
            if (isShow)
            {
                this.Invoke((EventHandler) (delegate
                {
                    htmlEditor1.BodyHtml = result;
                    statusbar.Caption = "网页代码加载完毕！";
                    buttonX1.Enabled = true;
                    buttonX2.Enabled = true;
                    buttonX3.Enabled = true;
                }));
            }
            closeI();

        }


        private void testall()
        {

            if (Coll.Count > 0 || ruleslv1.GetModel != 1)
            {
                showI("正在测试所有规则...");
                SetInfo();
                statusbar.Caption = "正在加载" + txttesturl.Text + "的网页代码...";
                string result = gethtml();

                if (isShow)
                {
                    this.Invoke((EventHandler) (delegate
                    {
                        htmlEditor1.BodyHtml = result;
                        statusbar.Caption = "网页代码加载完毕！";
                    }));
                }
               
                V3.Bll.GetBll get1 = new Bll.GetBll(result, ruleslv1);
                get1.oldurl = txttesturl.Text;
                resultList = new System.Collections.ArrayList();
                resultList = get1.getAllRules(0);
                string jieguo = "";
                for (int i = 0; i < resultList.Count; i++)
                {
                    jieguo += "第" + (i + 1) + "个结果:\r\n" + resultList[i].ToString() + "\r\n\r\n";
                }
                if (isShow)
                {
                    this.Invoke((EventHandler) (delegate
                    {
                        labelX8.Text = "提取结果：共提取到" + resultList.Count + "个结果！";
                        htmlEditor2.BodyHtml = jieguo;
                        statusbar.Caption = "全局规则测试完毕！";
                    }));
                }


                closeI();
            }
            else
            {
                this.Invoke((EventHandler) (delegate
                {
                    XtraMessageBox.Show("请先选中或添加至少一条子规则！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            }

        }


        private System.Threading.Thread s;

        private void buttonX1_Click(object sender, EventArgs e)
        {
            s = new System.Threading.Thread(gethtmlstr);
            s.IsBackground = true;
            s.Start();
            showI("正在下载网页数据");


        }

        private void modelck2_CheckedChanged(object sender, EventArgs e)
        {

            if (modelck2.Checked && ruleslv1.IsRgx == false)
            {
                ruleslv1.IsRgx = modelck2.Checked;

            }
            else if (modelck1.Checked && ruleslv1.IsRgx)
            {
                ruleslv1.IsRgx = false;

            }
            if (Coll.Count > 0)
            {
                if (
                    XtraMessageBox.Show(
                        "当前规则已经包含了" + Coll.Count + "条子规则，如果切换提取引擎，这些规则将被清空哦！\r\n您确定要切换提取引擎吗？", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes)
                {
                    Coll.Clear();
                    gridControl_main_view.RefreshData();
                }
                else
                {
                    //  e.Cancel = true;
                }
            }
        }

        private void modelck2_CheckedChanging(object sender, EventArgs e)
        {

        }

        private void chk1_1_CheckedChanged(object sender, EventArgs e)
        {
            chk3_1.Enabled = true;
            chk3_3.Enabled = true;
            chk3_4.Enabled = true;
            txtHebingStr.Enabled = true;
            if (chk1_1.Checked)
                ruleslv1.GetModel = 1;

        }

        private void chk1_2_CheckedChanged(object sender, EventArgs e)
        {
            chk3_2.Checked = true;
            chk3_2.Enabled = true;
            chk3_1.Enabled = false;
            chk3_3.Enabled = false;
            chk3_4.Enabled = false;
            txtHebingStr.Enabled = false;
            if (chk1_2.Checked)
                ruleslv1.GetModel = 2;
            isformat.IsOn = false;
            ishtml.IsOn = false;
        }

        private void chk1_3_CheckedChanged(object sender, EventArgs e)
        {
            chk3_2.Enabled = true;
            chk3_2.Checked = true;
            chk3_1.Enabled = false;
            chk3_3.Enabled = false;
            chk3_4.Enabled = false;
            txtHebingStr.Enabled = false;
            if (chk1_3.Checked)
                ruleslv1.GetModel = 3;
            isformat.IsOn = false;
            ishtml.IsOn = false;
        }

        private void frm_MainRules_Load(object sender, EventArgs e)
        {
            isShow = true;
            GetInfo();
            if (needmore)
                chk3_2.Checked = true;
        }

        private void buttonItem3_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxX2_CheckedChanged(object sender, EventArgs e)
        {

            ruleslv1.GetModel = 4;
            chk3_2.Enabled = false;
            chk3_1.Enabled = false;
            chk3_3.Enabled = false;
            chk3_4.Enabled = false;

        }

        private void frm_PiLiangLinks_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ipanel.Parent.Enabled = true;
                closeI();
                buttonX1.Enabled = true;
                buttonX2.Enabled = true;
                buttonX3.Enabled = true;

                if (s != null)
                {

                    try
                    {
                        s.Abort();
                    }
                    catch
                    {
                    }

                }
            }



        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            frm_YuChuli frm = new frm_YuChuli();
            frm.cookietxt = cookietxt;
            frm.ruleslv1 = xEngine.Common.XSerializable.CloneObject<Model.抓取相关模型.RulesEngLv1>(ruleslv1);
            frm.ShowDialog();
            if (frm.issave)
            {
                ruleslv1 = frm.ruleslv1;
            }
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            frm_Houchuli frm = new frm_Houchuli();
            frm.cookietxt = cookietxt;
            frm.ruleslv1 = xEngine.Common.XSerializable.CloneObject<Model.抓取相关模型.RulesEngLv1>(ruleslv1);
            frm.ShowDialog();
            if (frm.issave)
            {
                ruleslv1 = frm.ruleslv1;
            }
        }

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetInfo();
            isSave = true;
            if (needmore)
            {
                if (ruleslv1.OutModel == 2)
                {
                    Close();
                }
                else
                {
                    XtraMessageBox.Show("本规则为特殊需求规则，输出结果必须为多结果模式，请修改！", "模式错误", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    isSave = false;
                    return;
                }
            }
            else
            {
                this.Close();
            }
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            isSave = false;
            this.Close();
        }

        private void btn_rule_add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Model.抓取相关模型.RulesEngLv2 temprule = new Model.抓取相关模型.RulesEngLv2();
            if (ruleslv1.IsRgx)
            {
                frm_ChildRgxRules frm = new frm_ChildRgxRules();
                frm.cookietxt = cookietxt;
                frm.ruleslv2 = temprule;
                frm.ruleslv1 = ruleslv1;

                frm.ShowDialog();

                if (frm.issave)
                {
                    Record r = new Record();
                    r.编号 = Coll.Count + 1;
                    r.规则名称 = frm.ruleslv2.Readme;
                    r.规则 = frm.ruleslv2;
                    Coll.Add(r);
                    ruleslv1 = frm.ruleslv1;
                    txttesturl.Text = ruleslv1.TestUrl;
                    SetInfo();
                    GetInfo();
                }
            }
            else
            {
                frm_ChildEngRules frm = new frm_ChildEngRules();
                frm.cookietxt = cookietxt;
                frm.ruleslv2 = temprule;
                frm.ruleslv1 = ruleslv1;

                frm.ShowDialog();
                ;
                if (frm.issave)
                {
                    Record r = new Record();
                    r.编号 = Coll.Count + 1;
                    r.规则名称 = frm.ruleslv2.Readme;
                    r.规则 = frm.ruleslv2;
                    Coll.Add(r);
                    ruleslv1 = frm.ruleslv1;
                    txttesturl.Text = ruleslv1.TestUrl;
                    SetInfo();
                    GetInfo();
                }
            }
        }

        private void btn_rule_edit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridControl_main_view.SelectedRowsCount >= 1)
            {

                Record r = (Record) gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);

                if (ruleslv1.IsRgx)
                {
                    frm_ChildRgxRules frm = new frm_ChildRgxRules();
                    frm.cookietxt = cookietxt;
                    frm.ruleslv1 = ruleslv1;
                    frm.ruleslv2 = r.规则;
                    frm.ShowDialog();

                    if (frm.issave)
                    {
                        r.规则 = frm.ruleslv2;
                        ruleslv1 = frm.ruleslv1;
                        txttesturl.Text = ruleslv1.TestUrl;
                        SetInfo();
                        GetInfo();

                    }
                }
                else
                {
                    frm_ChildEngRules frm = new frm_ChildEngRules();
                    frm.cookietxt = cookietxt;
                    frm.ruleslv1 = ruleslv1;
                    frm.ruleslv2 = r.规则;
                    frm.ShowDialog();
                    if (frm.issave)
                    {
                        r.规则 = frm.ruleslv2;
                        ruleslv1 = frm.ruleslv1;
                        txttesturl.Text = ruleslv1.TestUrl;
                        SetInfo();
                        GetInfo();
                    }
                }

            }
        }

        private void btn_rule_delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Record r = (Record) gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0]);
                Coll.Remove(r);
                gridControl_main_view.RefreshData();
            }
            catch
            {
            }
            ;
        }

        private void btn_rule_test_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            s = new System.Threading.Thread(testone);
            s.IsBackground = true;
            s.Start();

        }

        private void btn_rule_testall_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            s = new System.Threading.Thread(testall);
            s.IsBackground = true;
            s.Start();

        }

        private void gridControl_main_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                popupMenu.ShowPopup(Control.MousePosition);
            }
        }

        private void gridControl_main_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btn_rule_edit_ItemClick(null, null);
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            btn_rule_test_ItemClick(null, null);
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            btn_rule_testall_ItemClick(null, null);
        }

        private void MainRules_FormClosed(object sender, FormClosedEventArgs e)
        {
            isShow = false;
        }

    }
}
