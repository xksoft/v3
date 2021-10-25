using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using Lucene;
using Lucene.Net;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;
using DevExpress.XtraEditors;

namespace V3.V3Form
{
    public partial class frmSetIndex : DevExpress.XtraEditors.XtraForm
    {
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

        #region 数据模型
        public static int ColumnCount = 3;
        public static RecordCollection DbColl = new RecordCollection();
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
            public int 库编号 { get { return Convert.ToInt32(values[0]); } set { SetValue(0, value); } }
            public string 库名称 { get { return values[1]; } set { SetValue(1, value); } }
            public int 数据量 { get { return Convert.ToInt32(values[2]); } set { SetValue(2, value); } }
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
        public frmSetIndex()
        {
            InitializeComponent();
        }
        public void showI(string txt)
        {
            buttonX1.Enabled = buttonX4.Enabled == false;
            ipanel.Visible = true;
            if (txt == "")
            {
                istate.Text = "正在加载中....";
            }
            else
            {
                istate.Text = txt + ".....";
            }
        }
        public void closeI()
        {
            buttonX1.Enabled = buttonX4.Enabled == true;
            ipanel.Visible = false;
            istate.Text = "正在加载中....";
            ipanel.Parent.Enabled = true;

        }
        Thread t;
        private void buttonX1_Click(object sender, EventArgs e)//开始建立
        {
            totlecount = 0;
            dangcount = 0;
            CheckForIllegalCrossThreadCalls = false;
            showI("准备生成....");
            t = new Thread(StartIndex);
            t.IsBackground = true;
            t.Start();
        }


        private void SelectClass_Load(object sender, EventArgs e)
        {
            DbColl.Clear();

            foreach (KeyValuePair<string, Model.ArticleDB> ss in Model.V3Infos.ArticleDb)
            {
                Record r = new Record();
                r.库编号 = Convert.ToInt32(ss.Value.Id);
                r.库名称 = ss.Value.Name;
                r.数据量 = ss.Value.DataCount;
                DbColl.Add(r);

            }
            gridControl_main.DataSource = DbColl;
            gridControl_main_view.RefreshData();
            gridControl_main_view.Columns["库编号"].Width = 50;
            gridControl_main_view.Columns["库编号"].VisibleIndex = 0;
            gridControl_main_view.Columns["库名称"].VisibleIndex = 1;

            label_count.Text = "共有" + Model.V3Infos.ArticleDb.Count + "个文章库";
        }
        public string formatText(string sourceString)
        {
            string result = sourceString;
            Dictionary<string, string> image = new Dictionary<string, string>();
            Dictionary<string, string> link = new Dictionary<string, string>();
            ArrayList tempList = new ArrayList();
            bool isPart = sourceString != null && sourceString.Trim().Length > 0;
            if (isPart)
            {

                result = result.Replace("\r", "");
                result = result.Replace("\n", "");
                result = result.Replace("\t", "");
                Regex regex = new Regex(@"<script[^>]*?>.*?</script>", RegexOptions.Singleline);//去掉所有的script标签
                result = regex.Replace(result, "");
                result = Regex.Replace(result, "(<head>).*(</head>)", "", RegexOptions.Singleline);
                regex = new Regex(@"<style[^>]*?>.*?</style>", RegexOptions.Singleline);//去掉所有的style标签
                result = regex.Replace(result, "");
                result = Regex.Replace(result, @"<( )*td([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*p([^>])*>|</p>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*div([^>])*>|</div>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*li( )*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*tr([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*br([^>])*>", "\r\r", RegexOptions.IgnoreCase);

                regex = new Regex(@"<.+?>", RegexOptions.Singleline);//去掉所有的标签             
                result = regex.Replace(result, "");     //最终的结果
                result = Regex.Replace(result, @" ( )+", "");
                result = Regex.Replace(result, "(\r)( )+(\r)", "\r\r");
                result = Regex.Replace(result, @"(\r\r)+", "\n");

                result = processStringFromXML(result);
                foreach (KeyValuePair<string, string> kvp in link)
                {
                    result = result.Replace(kvp.Key, kvp.Value);//将链接标签重新返回
                }
                foreach (KeyValuePair<string, string> kvp in image)
                {
                    result = result.Replace(kvp.Key, "");//将图片标签重新返回
                }
            }
            return result;
        }
        public string formatTextUseAiGet(string sourceString)
        {
            string result = sourceString;
            Dictionary<string, string> image = new Dictionary<string, string>();
            Dictionary<string, string> link = new Dictionary<string, string>();
            ArrayList tempList = new ArrayList();
            bool isPart = sourceString != null && sourceString.Trim().Length > 0;
            if (isPart)
            {

                result = result.Replace("\r", "");
                result = result.Replace("\n", "");
                result = result.Replace("\t", "");
                Regex regex = new Regex(@"<script[^>]*?>.*?</script>", RegexOptions.Singleline);//去掉所有的script标签
                result = regex.Replace(result, "");
                result = Regex.Replace(result, "(<head>).*(</head>)", "", RegexOptions.Singleline);
                regex = new Regex(@"<style[^>]*?>.*?</style>", RegexOptions.Singleline);//去掉所有的style标签
                result = regex.Replace(result, "");
                result = Regex.Replace(result, @"<( )*td([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*p([^>])*>|</p>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*div([^>])*>|</div>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*li( )*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*tr([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*br([^>])*>", "\r\r", RegexOptions.IgnoreCase);

                regex = new Regex(@"<.+?>", RegexOptions.Singleline);//去掉所有的标签             
                result = regex.Replace(result, "");     //最终的结果
                result = Regex.Replace(result, @" ( )+", "");
                result = Regex.Replace(result, "(\r)( )+(\r)", "\r\r");
                result = Regex.Replace(result, @"(\r\r)+", "\n");

                result = processStringFromXML(result);
            }
            return result;
        }



        string ReverseByArray(string original)//反转字符串
        {
            char[] c = original.ToCharArray();
            Array.Reverse(c);
            return new string(c);
        }

        string processStringFromXML(string input)
        {
            string output = "";
            output = input.Replace("&lt;", "<");
            output = input.Replace("&nbsp;", " ");
            output = output.Replace("&gt;", ">");
            output = output.Replace("&quot;", "\"");
            output = output.Replace("&apos;", "'");
            output = output.Replace("&amp;", "&");
            return output;
        }

        //全角转半角



        public void IndexDB(ArrayList li)
        {
            int i = 0;

            Regex r = new Regex(@"<a\s+href=(?<url>.+?)>(?<content>.+?)</a>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            TimeSpan tsStart;
            TimeSpan tsEnd; TimeSpan ts;
            for (int lii = 0; lii < li.Count; lii++)
            {
                ArrayList result = new ArrayList();
                i++;
                tsStart = new TimeSpan(DateTime.Now.Ticks);

                V3.Common.Format f = new Common.Format();

                string jieguo = f.formatText(li[lii].ToString(), 0).Replace("<p style=\"line-height:240%;\">", "<p>");
                jieguo = r.Replace(jieguo, "");
                string[] ok = jieguo.Replace("</p>", "`").Split('`');

                for (int ii = 0; ii < ok.Length; ii++)
                {
                    if (ok[ii].Length > 100)
                        result.Add(ok[ii] + "</p>");
                }
                for (int ii = 0; ii < result.Count; ii++)
                {
                    Document doc = new Document();//创建文档，给文档添加字段，并把文档添加到索引书写器里 
                    doc.Add(new Field("content", result[ii].ToString(), Field.Store.YES, Field.Index.TOKENIZED));//存储且索引  
                    writer.AddDocument(doc);




                }
                dangcount++;
                tsEnd = new TimeSpan(DateTime.Now.Ticks);
                ts = tsEnd.Subtract(tsStart).Duration();
                if (dangcount % 500 == 0)
                {
                    textBoxX1.Text += "总共有" + totlecount + "篇文章需要处理,已处理" + dangcount + "篇，预计剩余" + ((totlecount - dangcount) * ts.Milliseconds) / 1000 / 60 + "分钟\r\n";
                }
            }


        }
        static IndexWriter writer;
        static Lucene.Net.Analysis.Standard.StandardAnalyzer a;
        long totlecount = 0;
        static long dangcount = 0;
        public static SqlDataReader re;
        public void StartIndex()
        {
            try
            {
                for (int i = 0; i < gridControl_main_view.SelectedRowsCount; i++)
                {
                    Record r = (Record) gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[i]);
                    totlecount += V3.Common.ArticleBll.GetAllArticleCoun(r.库编号.ToString());


                }
                for (int i = 0; i < gridControl_main_view.SelectedRowsCount; i++)
                {
                    Record r = (Record) gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[i]);

                    if (dangcount == 0)
                    {
                        if (switchButton1.IsOn == false)
                        {
                            istate.Text = "正在处理文章库：" + r.库名称;
                            textBoxX1.Text += "正在处理文章库：" + r.库名称 + "\r\n";
                            ArrayList li = V3.Common.ArticleBll.GetAllArticle(r.库编号.ToString());
                            a = new StandardAnalyzer();
                            writer = new IndexWriter(textBoxX2.Text, a, true);

                            IndexDB(li);
                            writer.Optimize();
                            writer.Close();
                            textBoxX1.Text += "文章库：" + r.库名称 + "处理完毕\r\n";
                        }
                        else
                        {
                            istate.Text = "正在处理文章库：" + r.库名称;
                            textBoxX1.Text += "正在处理文章库：" + r.库名称 + "\r\n";
                            ArrayList li = V3.Common.ArticleBll.GetAllArticle(r.库编号.ToString());
                            a = new StandardAnalyzer();
                            writer = new IndexWriter(textBoxX2.Text, a, false);

                            IndexDB(li);
                            writer.Optimize();
                            writer.Close();
                            textBoxX1.Text += "文章库：" + r.库名称 + "处理完毕\r\n";
                        }

                    }
                    else
                    {
                        istate.Text = "正在处理文章库：" + r.库名称;
                        textBoxX1.Text += "正在处理文章库：" + r.库名称 + "\r\n";

                        ArrayList li = V3.Common.ArticleBll.GetAllArticle(r.库编号.ToString());
                        a = new StandardAnalyzer();
                        writer = new IndexWriter(textBoxX2.Text, a, false);

                        IndexDB(li);
                        writer.Optimize();
                        writer.Close();
                        textBoxX1.Text += "文章库：" + r.库名称 + "处理完毕\r\n";
                    }
                    textBoxX1.Select(textBoxX1.Text.Length,0);
                    textBoxX1.ScrollToCaret();
                }
                closeI();
                textBoxX1.Text += "全部处理完毕\r\n";
                this.Invoke((EventHandler) (delegate
                {
                    XtraMessageBox.Show("生成完毕！请等待大约10秒钟以后再使用该语料库！", "提示", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }));

            }
            catch (Exception err)
            {
                closeI();
                this.Invoke((EventHandler) (delegate
                {
                    XtraMessageBox.Show("生成出错：" + err.Message, "请确认目录文件是否正确", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }));

            }

        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog().ToString() == "OK")
            {
                textBoxX2.Text = f.SelectedPath;
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
        }
    }
}
