using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using DevExpress.XtraEditors;
using Model;
using V3.Bll;
using V3.Common;
using System.Xml;
using System.Collections;
using DevExpress.Utils;

namespace V3.V3Form
{
 
    public partial class frmSuper2 : DevExpress.XtraEditors.XtraForm
    {
        public static frmSuper2 myfrmSuper2;
        public string SuperDbconfig = "";
        public string SuperPath = "";
        
        #region v3文章库数据模型

        public static int ColumnCount = 3;
        public static RecordCollection V3DbColl = new RecordCollection();
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
        #region 超站文章库数据模型

        public static int SuperColumnCount = 3;
        public static SuperRecordCollection SuperDbColl = new SuperRecordCollection();
        public class SuperRecordCollection : CollectionBase, IBindingList, ITypedList
        {
            public SuperRecord this[int i] { get { return (SuperRecord)List[i]; } }
            public void Add(SuperRecord record)
            {
                int res = List.Add(record);
                record.owner = this;
                record.Index = res;

            }
            public void SetValue(int row, int col, object val)
            {
                this[row].SetValue(col, val);
            }
            internal void OnListChanged(SuperRecord rec)
            {
                // if (listChangedHandler != null) listChangedHandler(this, new ListChangedEventArgs(ListChangedType.ItemChanged, rec.Index, rec.Index));
            }
            PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] accessors)
            {
                PropertyDescriptorCollection coll = TypeDescriptor.GetProperties(typeof(SuperRecord));
                ArrayList list = new ArrayList(coll);
                list.Sort(new PDComparer());
                PropertyDescriptorCollection res = new PropertyDescriptorCollection(null);
                for (int n = 0; n < SuperColumnCount; n++)
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

        public class SuperRecord
        {
            internal int Index = -1;
            internal SuperRecordCollection owner;
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
        #region v3文章库列表多选
        bool v3isMouseDown = false;
        bool v3isSetStartRow = false;
        private int v3StartRowHandle = -1;
        private int v3CurrentRowHandle = -1;
        private void v3SelectRows(int startRow, int endRow)
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
        #region 超站文章库列表多选
        bool superisMouseDown = false;
        bool superisSetStartRow = false;
        private int superStartRowHandle = -1;
        private int superCurrentRowHandle = -1;
        public Random R = new Random();
        private void superSelectRows(int startRow, int endRow)
        {
            if (startRow > -1 && endRow > -1)
            {

                gridControl_Super_View.BeginSelection();
                gridControl_Super_View.ClearSelection();
                gridControl_Super_View.SelectRange(startRow, endRow);
                gridControl_Super_View.EndSelection();


            }
        }
        #endregion
        public frmSuper2()
        {
            InitializeComponent();
            myfrmSuper2 = this;
        }
       
        public bool Stop = true;
        private void frm_文章导出导入工具_Shown(object sender, EventArgs e)
        {
          
            //text_to.Value = Model.V3Infos.ArticleDb[dbid].DataCount;
            //toggleSwitch.IsOn = isimport;
            //LoadConfig();
            //ribemport.Checked = !isimport;
        }
        public bool CheckLicense()
        {
            string AppPath = Application.StartupPath;
            if (AppPath.Contains("\\"))
            {
                AppPath = AppPath.Remove(AppPath.LastIndexOf("\\"));
                if (AppPath.Contains("\\"))
                {
                    AppPath = AppPath.Remove(AppPath.LastIndexOf("\\"));
                    SuperPath = AppPath + "\\super2";
                    if (Directory.Exists(SuperPath))
                    {
                     
                        string[] Dirs=  Directory.GetDirectories(SuperPath);
                        foreach(string d in Dirs)
                        {
                            DirectoryInfo di = new DirectoryInfo(d);
                            comboBox_Super.Properties.Items.AddRange(new object[] {di.Name});
                        }
                        comboBox_Super.SelectedIndex = 0;
                        return true;
                       
                    }
                }
            }
            return false;
        }
        public void LoadSuperArticleDbList()
        {
            btn_Start.Enabled = false;
            SuperDbColl.Clear();
            DataTable dt = new DataTable();
           
           Library.MySqlHelper.ConnectionStringManager = "server=localhost;database=superx;uid="+textEdit_dbpassword.Text+";pwd="+textEdit_dbuser.Text+ ";CharSet=utf8;";
            try
            {
                dt = Library.MySqlHelper.GetDataSet("select * from tb_articledb").Tables[0];
                btn_Start.Enabled = true;
            }
            catch (Exception error)
            {
                XtraMessageBox.Show("数据库连接失败："+error.Message, "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            for (int i=0;i<dt.Rows.Count;i++)
            {
                SuperRecord r = new SuperRecord();
                r.库编号 = Convert.ToInt32(dt.Rows[i]["t_id"]);
                r.库名称 = dt.Rows[i]["t_name"].ToString();
                r.数据量 =Convert.ToInt32(dt.Rows[i]["t_count"].ToString());
                SuperDbColl.Add(r);


            }
            gridControl_Super.DataSource = SuperDbColl;
            gridControl_Super_View.RefreshData();
            gridControl_Super_View.Columns["库编号"].Width = 80;
            gridControl_Super_View.Columns["数据量"].Width = 80;
            gridControl_Super_View.Columns["库编号"].VisibleIndex = 0;
            gridControl_Super_View.Columns["库名称"].VisibleIndex = 1;
        }
        public void LoadV3ArticleDbList()
        {
            V3DbColl.Clear();

            foreach (KeyValuePair<string, Model.ArticleDB> ss in Model.V3Infos.ArticleDb)
            {
                Record r = new Record();
                r.库编号 = Convert.ToInt32(ss.Value.Id);
                r.库名称 = ss.Value.Name;
                r.数据量 = ss.Value.DataCount;
                if (comboBox_Groups.EditValue.ToString() != "AllGroup")
                {
                    if (ss.Value.Type == comboBox_Groups.EditValue.ToString())
                    {
                        V3DbColl.Add(r);
                    }
                }
                else {
                    V3DbColl.Add(r);
                }
               

            }
            gridControl_main.DataSource = V3DbColl;
            gridControl_main_view.RefreshData();
            gridControl_Super_View.Columns["库编号"].Width = 20;
            gridControl_Super_View.Columns["数据量"].Width = 20;
            gridControl_main_view.Columns["库编号"].VisibleIndex = 0;
            gridControl_main_view.Columns["库名称"].VisibleIndex = 1;

        }
        public void LoadGroups()
        {
            comboBox_Groups.Properties.Columns.Clear();
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Tag", typeof(string));
            dt.Rows.Add(new object[] { "全部分组", "AllGroup" });
            dt.Rows.Add(new object[] { "默认分组", "DefaultGroup" });
            foreach (KeyValuePair<string, string> value in Model.V3Infos.MainDb.GroupList)
            {
                dt.Rows.Add(new object[] { value.Value, value.Key.ToString() });
            }
            comboBox_Groups.EditValue = "Tag";
            comboBox_Groups.Properties.ValueMember = "Tag";
            comboBox_Groups.Properties.DisplayMember = "Name";

            comboBox_Groups.Properties.DataSource = dt;

            comboBox_Groups.Properties.Columns.Add(
                new DevExpress.XtraEditors.Controls.LookUpColumnInfo(comboBox_Groups.Properties.DisplayMember));
            comboBox_Groups.ItemIndex = 0;
        }
        public void LoadConfig()
        {

            if (Model.V3Infos.MainDb.DefaultTasks.ContainsKey(Model.V3Infos.MainDb.SuperDefaultTask))
            {
                linkLabel_DefaultTask.Text = "使用任务模板 [" + Model.V3Infos.MainDb.SuperDefaultTask + "] 的处理配置";
            }
            else
            {
                linkLabel_DefaultTask.Text = "点击选择要使用的处理任务模板";
            }


        }

      

        private void btnClose_Click(object sender, EventArgs e)
        {

          
        }
      
        private void btnStart_Click(object sender, EventArgs e)
        {

           
          
        }

        public void start()
        {
            int success = 0;
            int error = 0;
            Thread thread = new Thread(delegate()
            {

               
                 
                    //int fromID = Convert.ToInt32(text_from.Value);
                    //int toID = Convert.ToInt32(text_to.Value);
                    //Encoding encoding = Encoding.UTF8;
                    //if (toggleSwitch_encoding.IsOn)
                    //{
                    //    encoding = Encoding.GetEncoding("gbk");
                    //}
                    //this.Invoke((EventHandler) (delegate
                    //{
                    //    btn_start.Text = "停止导出";
                    //    progressBar.Visible = label_status.Visible = true;
                    //}));
                    //if (fromID < toID)
                    //{

                    //    this.Invoke((EventHandler) (delegate
                    //    {
                    //        progressBar.Properties.Maximum = toID - fromID;
                    //        label_status.Text = "准备导出...";
                    //    }));
                    //    for (int i = fromID; i < toID + 1; i++)
                    //    {
                    //        if (Stop) { break; }
                    //        Model.Model_Article article = new Model_Article();
                    //        article.Id = i.ToString();
                    //        bool bget = ArticleBll.LoadData(ref article, Model.V3Infos.ArticleDb[dbid]);
                    //        if (article != null & bget)
                    //        {
                    //            if (toggleSwitch_SetArticle.IsOn && Model.V3Infos.MainDb.DefaultTasks.ContainsKey(db.DefaultTask))
                    //            {
                    //                Common.SetArticle s = new Common.SetArticle();
                    //                s.Task = xEngine.Common.XSerializable.CloneObject<Model.Task>(Model.V3Infos.MainDb.DefaultTasks[db.DefaultTask]);
                    //                s.art = article;
                    //                s.chuli();
                    //                s.minganguolv();
                    //            }
                               
                    //            PostBll bll = new PostBll(new GetPostModel(), 0, 0);
                    //            bll.pointid = -1;
                    //            bll.article = xEngine.Common.XSerializable.CloneObject<Model.Model_Article>(article);
                    //            string path = text_path.Text;
                    //            string filename = text_filename.Text;
                    //            string filecontent = text_formate.Text;
                    //            path = bll.ReplaceTag(path, false, "", true);
                    //            filename = bll.ReplaceTag(filename, false, "", true);
                    //            filename = Library.StrHelper.SetFileName(filename);
                    //            this.Invoke((EventHandler)(delegate
                    //            {
                    //                progressBar.Text = i.ToString();
                    //                label_status.Text = "正在导出[" + filename + "]...";
                    //            }));
                    //            filecontent = bll.ReplaceTag(filecontent, false, "", true);
                    //            try
                    //            {
                    //                if (!Directory.Exists(path))
                    //                {
                    //                    Directory.CreateDirectory(path);
                    //                }
                    //                if (filename.Length > 200)
                    //                {
                    //                    filename = filename.Substring(filename.Length - 200, 200);
                    //                }
                    //                File.WriteAllText(path + filename, filecontent, encoding);
                    //                success++;
                    //            }
                    //            catch (Exception err)
                    //            {
                    //                error++;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            error++;
                    //        }
                    //    }
                    //}
                    //else
                    //{

                    //}
                    //this.Invoke((EventHandler) (delegate
                    //{
                    //    Stop = true;
                        
                    //    progressBar.Text = "0";
                    //    progressBar.Visible = label_status.Visible = false;
                    //    btn_start.Text = "开始导出";
                    //    XtraMessageBox.Show("成功导出[" + success + "]篇文章到“" + text_path.Text + "”，失败[" + error + "]篇！",
                    //        "导出完毕", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      

                    //}));
                
            });
            thread.IsBackground = true;
            thread.Start();

        }

        public Model.Model_Article GetArticle(string name, string content, string nameformate, string contentformate)
        {
            Model.Model_Article article = new Model_Article();
            article.DataObject = new Dictionary<int, string>()
            {
            { 0,""},
            { 1,""},
            { 2,""},
            { 3,""},
            { 4,""},
            { 5,""},
            { 6,""},
            { 7,""},
            { 8,""},
            { 9,""},
            { 10,""},
            { 11,""},
            { 12,""},
            { 13,""},
            { 14,""},
            { 15,""},
            { 16,""},
            { 17,""},
            { 18,""},
            { 19,""},
            { 20,""},
            { 21,""},
            { 22,""},
            { 23,""},
            { 24,""},
            { 25,""},
            { 26,""},
            { 27,""},
            { 28,""},
            { 29,""}
            };
            if (nameformate.Length>0)
            {
                FileInfo file=new FileInfo(name);
                int v =Convert.ToInt32(Library.RegexHelper.GetList(nameformate, "【模型值(?<v>\\d+)】")[0])-1;
                article.DataObject[v] = new Regex("\\.(\\w*)$").Replace(file.Name, "");
            }
            content = content.Replace("\r","");
            content = content.Replace("\n", "㊣");
            List<string> vs = Library.RegexHelper.GetList(contentformate, "【模型值(?<v>\\d+)】");
            int vc = 0;
            foreach (var s in vs)
            {
                if (vc == vs.Count-1)
                {
                    contentformate = contentformate.Replace("【模型值" + s + "】", "(?<model" + s + ">.*)");
                }
                else
                {
                    contentformate = contentformate.Replace("【模型值" + s + "】", "(?<model" + s + ">.+?)");
                }
                vc++;
            }
            contentformate = contentformate.Replace("\r","");
            contentformate = contentformate.Replace("\n", "㊣");
            Match mc = new Regex(contentformate,RegexOptions.IgnoreCase).Match(content);
            if (mc.Success )
            {
                Match m = mc.NextMatch();
               for (int i=0;i<30;i++)
                {

                    if (mc.Groups["model" + (i + 1)].Success)
                    {
                        article.DataObject[i] = mc.Groups["model" + (i + 1)].Value.Replace("㊣", "\r\n");
                    }
                }
                return article;
            }
            else
            {
                return null;
            }

        }

     

        private void frm_文章导出导入工具_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.f_frmReplaceTag != null && !Program.f_frmReplaceTag.IsDisposed )
            {
                Program.f_frmReplaceTag.Close();
            }
        }

    
        

        private void frm_文章导出导入工具_Load(object sender, EventArgs e)
        {
            if (CheckLicense())
            {
                LoadConfig();
                LoadGroups();
                LoadV3ArticleDbList();
            } else
            {
                XtraMessageBox.Show("未检测到超级站群引擎授权，无法使用该功能！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

            }
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (Stop) { Stop = false; btn_Start.Text = "停止导出"; }
            else { Stop = true; btn_Start.Text = "开始导出"; }
            bool Delete = toggleSwitch_Delete.IsOn;
            int Success = 0;
            int Error = 0;
            string Title = textEdit_Title.Text;
            string Content = textEdit_Content.Text;
            string Keyword = textEdit_Keyword.Text;
            if (gridControl_main_view.SelectedRowsCount == 0 || gridControl_Super_View.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("请用鼠标选择V3文章库和要导入的超站文章库！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Stop = true; btn_Start.Text = "开始导出";
                return;
            }
            else if (Keyword.Contains("【随机关键词】")&&(!Model.V3Infos.MainDb.DefaultTasks.ContainsKey(Model.V3Infos.MainDb.SuperDefaultTask)||!toggleSwitch_SetArticle.IsOn))
            {
                XtraMessageBox.Show("使用随机关键词要求必须启用并设置处理任务模板！", "无法继续", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Stop = true; btn_Start.Text = "开始导出";
                return;
            }

            Stop = false;
            int[] SelectHandles = gridControl_main_view.GetSelectedRows();
            int[] SelectSuperHandles = gridControl_Super_View.GetSelectedRows();
            List<int> SelectSuperDbIds = new List<int>();
            int TotalCount = 0;
            foreach (int h in SelectSuperHandles)
            {
                SuperRecord r = (SuperRecord)gridControl_Super_View.GetRow(h);
                SelectSuperDbIds.Add(r.库编号);

            }
            foreach (int h in SelectHandles)
            {
               
                Record r = (Record)gridControl_main_view.GetRow(h);
                TotalCount += r.数据量;


            }
            label_status.Visible = true;
            progressBar.Visible = true;
            progressBar.Properties.Maximum = TotalCount;
            label_status.Text = "准备导出...";
           
            Thread thread = new Thread(delegate ()
            {
                foreach (int h in SelectHandles)
                {
                    List<string> DeleteList = new List<string>();
                    int PageIndex = 1;
                    Record r = (Record)gridControl_main_view.GetRow(h);
                    ArticleDB db = Model.V3Infos.ArticleDb[r.库编号.ToString()];
                    ArrayList list = ArticleBll.getpagelist(PageIndex, db);
                    while (list.Count > 0)
                    {
                        if (Stop) { break; }
                        PageIndex++;

                        for (int i = 0; i < list.Count; i++)
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                progressBar.Text = (Success + Error).ToString();
                                label_status.Text = "正在导出[" + (Success+Error)+"/"+TotalCount + "]...";
                            }));
                            if (Stop) { break; }
                            Model.Model_Article article = (Model.Model_Article)list[i];
                            if (article==null) { continue; }
                            int superdbid = SelectSuperDbIds[R.Next(0, SelectSuperDbIds.Count)];
                            int PointId = -1;
                            int TaskId = 0;
                           
                            if (toggleSwitch_SetArticle.IsOn && Model.V3Infos.MainDb.DefaultTasks.ContainsKey(Model.V3Infos.MainDb.SuperDefaultTask))
                            {
                                Common.SetArticle s = new Common.SetArticle();
                                s.Task = xEngine.Common.XSerializable.CloneObject<Model.Task>(Model.V3Infos.MainDb.DefaultTasks[Model.V3Infos.MainDb.SuperDefaultTask]);
                                s.art = article;
                                s.chuli();
                                s.minganguolv();
                                PointId = Model.V3Infos.MainDb.DefaultTasks[Model.V3Infos.MainDb.SuperDefaultTask].PointId;
                                TaskId = s.Task.id;
                            }

                            PostBll bll = new PostBll(new GetPostModel(), PointId, TaskId);
                            bll.pointid = PointId;
                            bll.article = xEngine.Common.XSerializable.CloneObject<Model.Model_Article>(article);

                             Title = textEdit_Title.Text;
                             Content = textEdit_Content.Text;
                             Keyword = textEdit_Keyword.Text;
                            Title = bll.ReplaceTag(Title, false, "", true);
                            Content = bll.ReplaceTag(Content, false, "", true);
                            Keyword = bll.ReplaceTag(Keyword, false, "", true);


                        MySql.Data.MySqlClient.MySqlParameter[] ps = new MySql.Data.MySqlClient.MySqlParameter[] {
                        new MySql.Data.MySqlClient.MySqlParameter("t_dbid",superdbid),
                        new MySql.Data.MySqlClient.MySqlParameter("t_verify",1),
                        new MySql.Data.MySqlClient.MySqlParameter("t_title",Title),
                        new MySql.Data.MySqlClient.MySqlParameter("t_content",Content),
                        new MySql.Data.MySqlClient.MySqlParameter("t_keyword",Keyword),
                        new MySql.Data.MySqlClient.MySqlParameter("t_time",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                        };
                            int su = Library.MySqlHelper.ExecuteNonQuery(CommandType.Text, "insert into tb_articles(t_dbid,t_verify,t_title,t_content,t_keyword,t_time) values(@t_dbid,@t_verify,@t_title,@t_content,@t_keyword,@t_time)", ps);
                            if (su > 0)
                            {
                                Success++;
                                if (Delete) { DeleteList.Add(article.Id); }
                            }
                            else { Error++; }
                        }
                        list = ArticleBll.getpagelist(PageIndex, db);
                    }
                    //删除文章
                    foreach (string s in DeleteList) {  ArticleBll.dbdelete(s, db); }

                }
                this.Invoke((EventHandler)(delegate
                {
                    label_status.Visible = false;
                    progressBar.Visible = false;
                    btn_Start.Text = "开始导出";
                    Stop = true;
                    XtraMessageBox.Show("成功导出" + Success + "篇文章，失败"+Error+"篇！", "导出完毕", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            });
            thread.IsBackground = true;
            thread.Start();
            
        }

    

   

     

     

        private void linkLabel_DefaultTask_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           

            frmDefaultTasks f = new frmDefaultTasks();
            f.ShowDialog();
            if (f.IsOK)
            {
                Model.V3Infos.MainDb.SuperDefaultTask = f.DefaultTaskName;
                LoadConfig();
            }
            else 
            {
                LoadConfig();
            }
        }

        private void comboBox_Super_SelectedIndexChanged(object sender, EventArgs e)
        {
            
           SuperDbconfig = SuperPath+"\\"+comboBox_Super.SelectedItem.ToString() + "\\Dbconfig.xml";

            if (SuperDbconfig.Length > 0)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(SuperDbconfig);
                string dbuser = xmlDoc.SelectSingleNode("/superx/dbuser").Attributes["value"].Value;
                string dbpassword = xmlDoc.SelectSingleNode("/superx/dbpassword").Attributes["value"].Value;
                textEdit_dbpassword.Text = dbuser;
                textEdit_dbuser.Text = dbpassword;
                LoadSuperArticleDbList();
            }
        }

        private void btn_LoadSuperArticleDbList_Click(object sender, EventArgs e)
        {
            LoadSuperArticleDbList();

        }

        private void comboBox_Groups_Properties_EditValueChanged(object sender, EventArgs e)
        {
            LoadV3ArticleDbList();
        }

        private void gridControl_main_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                v3isMouseDown = true;
            }
        }

        private void gridControl_main_MouseMove(object sender, MouseEventArgs e)
        {

            if (v3isMouseDown)
            {
                
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo info = gridControl_main_view.CalcHitInfo(e.X, e.Y);
                //如果鼠标落在单元格里
                if (info.InRow)
                {
                    gridControl_main_view.FocusedRowHandle = info.RowHandle;
                  
                    if (!v3isSetStartRow)
                    {
                        v3StartRowHandle = info.RowHandle;
                        v3isSetStartRow = true;
                    }
                    else
                    {
                        //获得当前的单元格
                        int newRowHandle = info.RowHandle;
                        if (v3CurrentRowHandle != newRowHandle)
                        {
                            v3CurrentRowHandle = newRowHandle;
                            //选定 区域 单元格
                            v3SelectRows(v3StartRowHandle, v3CurrentRowHandle);
                            
                               
                            
                        }
                    }
                }
            }
        }

        private void gridControl_main_MouseUp(object sender, MouseEventArgs e)
        {

            v3StartRowHandle = -1;
            v3CurrentRowHandle = -1;
            v3isMouseDown = false;
            v3isSetStartRow = false;
        }

        private void gridControl_Super_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                superisMouseDown = true;
            }

        }

        private void gridControl_Super_MouseMove(object sender, MouseEventArgs e)
        {
            if (superisMouseDown)
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo info = gridControl_Super_View.CalcHitInfo(e.X, e.Y);
                //如果鼠标落在单元格里
                if (info.InRow)
                {
                    gridControl_Super_View.FocusedRowHandle = info.RowHandle;
                    if (!superisSetStartRow)
                    {
                        superStartRowHandle = info.RowHandle;
                        superisSetStartRow = true;
                    }
                    else
                    {
                        //获得当前的单元格
                        int newRowHandle = info.RowHandle;
                        if (superCurrentRowHandle != newRowHandle)
                        {
                            superCurrentRowHandle = newRowHandle;
                            //选定 区域 单元格
                            superSelectRows(superStartRowHandle, superCurrentRowHandle);

                        }
                    }
                }
            }

        }

        private void gridControl_Super_MouseUp(object sender, MouseEventArgs e)
        {
            superStartRowHandle = -1;
            superCurrentRowHandle = -1;
            superisMouseDown = false;
            superisSetStartRow = false;
        }

        private void textEdit_Title_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (Program.f_frmReplaceTag == null || Program.f_frmReplaceTag.IsDisposed)
                {
                    Program.f_frmReplaceTag = new frmReplaceTag();
                }
                Program.f_frmReplaceTag.referer = "frm_super2_textEdit_Title";
                Program.f_frmReplaceTag.Focus();
                Program.f_frmReplaceTag.Show();
                Program.f_frmReplaceTag.Location = new Point(0, 0);
            }
        }

        private void textEdit_Keyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (Program.f_frmReplaceTag == null || Program.f_frmReplaceTag.IsDisposed)
                {
                    Program.f_frmReplaceTag = new frmReplaceTag();
                }
                Program.f_frmReplaceTag.referer = "frm_super2_textEdit_Keyword";
                Program.f_frmReplaceTag.Focus();
                Program.f_frmReplaceTag.Show();
                Program.f_frmReplaceTag.Location = new Point(0, 0);
            }
        }

        private void textEdit_Content_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (Program.f_frmReplaceTag == null || Program.f_frmReplaceTag.IsDisposed)
                {
                    Program.f_frmReplaceTag = new frmReplaceTag();
                }
                Program.f_frmReplaceTag.referer = "frm_super2_textEdit_Content";
                Program.f_frmReplaceTag.Focus();
                Program.f_frmReplaceTag.Show();
                Program.f_frmReplaceTag.Location = new Point(0, 0);
            }

        }

        private void textEdit_Title_MouseUp(object sender, MouseEventArgs e)
        {
            Program.f_frmReplaceTag.referer = "frm_super2_textEdit_Title";
        }

        private void textEdit_Content_MouseUp(object sender, MouseEventArgs e)
        {
            Program.f_frmReplaceTag.referer = "frm_super2_textEdit_Content";
        }

        private void textEdit_Keyword_MouseUp(object sender, MouseEventArgs e)
        {
            Program.f_frmReplaceTag.referer = "frm_super2_textEdit_Keyword";

        }

        private void linkLabel_models_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
        }
    }
}
