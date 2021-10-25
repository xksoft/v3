using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using DevExpress.XtraEditors;

namespace V3.V3Form
{
    public partial class frmTasks_Add : DevExpress.XtraEditors.XtraForm
    {
        public frmTasks_Add()
        {
            InitializeComponent();
        }
        public bool issave = false;
        public Model.Task model = new Model.Task();
        void GetAllWeb() //获取所有站点信息
        {
            PointColl.Clear();
            foreach (KeyValuePair<int, Model.SendPoint> ss in Model.V3Infos.SendPointDb)
            {
                Record r = new Record();
                r.发布点编号 = ss.Value.id;
                r.发布点名称 = ss.Value.name;
                if (ss.Value.GroupTag == "DefaultGroup")
                {
                    r.所属分组 = "默认分组";
                }
                else
                {


                    if (Model.V3Infos.MainDb.GroupList.ContainsKey(ss.Value.GroupTag))
                    {
                        r.所属分组 = Model.V3Infos.MainDb.GroupList[ss.Value.GroupTag];
                    }
                    else
                    {
                        r.所属分组 = "默认分组";
                    }
                }
                PointColl.Add(r);
            }
            gridControl_main.DataSource = PointColl;
            gridControl_main_view.Columns["发布点编号"].Width = 50;
            gridControl_main_view.Columns["发布点编号"].VisibleIndex = 0;
            gridControl_main_view.Columns["发布点名称"].VisibleIndex = 1;
            gridControl_main_view.ClearSelection();

        }
        Thread addthread;

        #region 任务信息
        private void btnSelectArticle_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 0;
            frm.Text = "请选择或新建一个文章库[[[请双击选择您要使用的库，注意了，是“双击”]]]";
            frm.isselectdb = true;
            frm.ShowDialog();
            if (frm.selectedDBid != 0)
            {
                model.ArticleDbId = frm.selectedDBid;
                txtArticleName.Text = "(" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Id + "]" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Name;
            }
        }

        private void btnSelectKeyword_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 1;
            frm.Text = "请选择或新建一个关键词库[[[请双击选择您要使用的库，注意了，是“双击”]]]";
            frm.isselectdb = true;
            frm.ShowDialog();
            if (frm.selectedDBid != 0)
            {
                model.KeywordDbId = frm.selectedDBid;
                txtKeywordName.Text = "(" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Keywords.Count + ")" + "[" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Id + "]" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Name;
            }
        }

        private void btnSelectHash_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 2;
            frm.Text = "请选择或新建一个哈希库[[[请双击选择您要使用的库，注意了，是“双击”]]]";
            frm.isselectdb = true;
            frm.ShowDialog();
            if (frm.selectedDBid != 0)
            {
                model.HashDbId = frm.selectedDBid;
                txtHashName.Text = "(" + Model.V3Infos.HashDb[model.HashDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Id + "]" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Name;
            }
        }

        private void btnSelectReplace_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 3;
            frm.Text = "请选择或新建一个替换库[[[请双击选择您要使用的库，注意了，是“双击”]]]";
            frm.isselectdb = true;
            frm.ShowDialog();
            if (frm.selectedDBid != 0)
            {
                model.ReplaceDbId = frm.selectedDBid;
                txtReplaceName.Text = "(" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Words.Count + ")" + "[" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Id + "]" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Name;
            }
        }

        private void btnSelectLink_Click(object sender, EventArgs e)
        {
            V3Form.DbManage frm = new DbManage();
            frm.dbtype = 4;
            frm.Text = "请选择或新建一个链接库[[[请双击选择您要使用的库，注意了，是“双击”]]]";
            frm.isselectdb = true;
            frm.ShowDialog();
            if (frm.selectedDBid != 0)
            {
                model.LinkDbId = frm.selectedDBid;
                txtLinkName.Text = "(" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Name;
            }
        }

        private void btnAddArticle_Click(object sender, EventArgs e)
        {
            
            if (model.PointId==0) { return; }
            int id = V3.Bll.DbBll.AddDb(1, Model.V3Infos.SendPointDb[model.PointId].name + "-New文章库", Model.V3Infos.SendPointDb[model.PointId].GroupTag);
            if (id != 0)
            {
                model.ArticleDbId = id;
                txtArticleName.Text = "(" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Id + "]" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Name;
            }
        }

        private void btnAddKeyword_Click(object sender, EventArgs e)
        {
            
            if (model.PointId == 0) { return; }
            int id = V3.Bll.DbBll.AddDb(2, Model.V3Infos.SendPointDb[model.PointId].name + "-New关键词库", Model.V3Infos.SendPointDb[model.PointId].GroupTag);
            if (id != 0)
            {
                model.KeywordDbId = id;
                txtKeywordName.Text = "(" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Keywords.Count + ")" + "[" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Id + "]" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Name;
            }
        }

        private void btnAddhash_Click(object sender, EventArgs e)
        {
            if (model.PointId == 0) { return; }
            int id = V3.Bll.DbBll.AddDb(3, Model.V3Infos.SendPointDb[model.PointId].name + "-New哈希库", Model.V3Infos.SendPointDb[model.PointId].GroupTag);
            if (id != 0)
            {
                model.HashDbId = id;
                txtHashName.Text = "(" + Model.V3Infos.HashDb[model.HashDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Id + "]" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Name;
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (model.PointId == 0) { return; }
            int id = V3.Bll.DbBll.AddDb(4, Model.V3Infos.SendPointDb[model.PointId].name + "-New替换库", Model.V3Infos.SendPointDb[model.PointId].GroupTag);
            if (id != 0)
            {
                model.ReplaceDbId = id;
                txtReplaceName.Text = "(" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Words.Count + ")" + "[" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Id + "]" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Name;
            }
        }

        private void btnAddLink_Click(object sender, EventArgs e)
        {
            if (model.PointId == 0) { return; }
            int id = V3.Bll.DbBll.AddDb(5, Model.V3Infos.SendPointDb[model.PointId].name + "-New链接库", Model.V3Infos.SendPointDb[model.PointId].GroupTag);
            if (id != 0)
            {
                model.LinkDbId = id;
                txtLinkName.Text = "(" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Name;
            }
        }

        private void btnArticleDefault_Click(object sender, EventArgs e)
        {
            if (!Model.V3Infos.KeywordDb.ContainsKey(model.ArticleDbId.ToString()))
                return;
            model.ArticleDbId = Model.V3Infos.SendPointDb[model.PointId].ArticleDbID;
            txtArticleName.Text = "(" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Id + "]" + Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Name;
        }

        private void btnKeywordDefault_Click(object sender, EventArgs e)
        {
            if (!Model.V3Infos.KeywordDb.ContainsKey(model.KeywordDbId.ToString()))
                return;
            model.KeywordDbId = Model.V3Infos.SendPointDb[model.PointId].KeywordDbID;
            txtKeywordName.Text = "(" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Keywords.Count + ")" + "[" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Id + "]" + Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Name;

        }

        private void btnHashDefault_Click(object sender, EventArgs e)
        {
            if (!Model.V3Infos.HashDb.ContainsKey(model.HashDbId.ToString()))
                return;
            model.HashDbId = Model.V3Infos.SendPointDb[model.PointId].HashDbID;
            txtHashName.Text = "(" + Model.V3Infos.HashDb[model.HashDbId.ToString()].DataCount + ")" + "[" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Id + "]" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Name;

        }

        private void btnReplaceDefault_Click(object sender, EventArgs e)
        {
            if (!Model.V3Infos.ReplaceDb.ContainsKey(model.ReplaceDbId.ToString()))
                return;
            model.ReplaceDbId = Model.V3Infos.SendPointDb[model.PointId].ReplaceDbid;
            txtReplaceName.Text = "(" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Words.Count + ")" + "[" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Id + "]" + Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Name;

        }

        private void btnLinkDefault_Click(object sender, EventArgs e)
        {
            if (!Model.V3Infos.LinkDb.ContainsKey(model.LinkDbId.ToString()))
                return;
            model.LinkDbId = Model.V3Infos.SendPointDb[model.PointId].LinkDbID;
            txtLinkName.Text = "(" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Links.Count + ")" + "[" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Id + "]" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Name;

        }
   


        private void integerInput1_ValueChanged(object sender, EventArgs e)
        {
            labelX29.Text = "任务批次将以" + integerInput1.Value + "分钟的间隔重复运行！";
        }

        private void IsAutoTask_ValueChanged(object sender, EventArgs e)
        {
          
        }
        #endregion
        #region 数据模型
        public static int ColumnCount = 3;
        public static RecordCollection PointColl = new RecordCollection();
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
            public int 发布点编号 { get { return Convert.ToInt32(values[0]); } set { SetValue(0, value); } }
            public string 发布点名称 { get { return values[1]; } set { SetValue(1, value); } }
            public string 所属分组 { get { return values[2]; } set { SetValue(2, value); } }
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
        #region 任务列表多选
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

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (Model.V3Infos.MainDb.MyModels.ContainsKey(model.GetModel))
            {
                System.Diagnostics.Process.Start("iexplore", Model.V3Infos.MainDb.MyModels[model.GetModel].PlanUrl);
            }
            else
            {
                MessageBox.Show("请先选择一个抓取模块！", "木有选择抓取模块", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private void btnViewArticleDb_Click(object sender, EventArgs e)
        {
            if (Model.V3Infos.ArticleDb.ContainsKey(model.ArticleDbId.ToString()))
            {
                frm_文章库文章列表 dbview = new frm_文章库文章列表();
                dbview.dbid = model.ArticleDbId.ToString();
                dbview.Text = Model.V3Infos.ArticleDb[dbview.dbid].Name + "(文章总数：" + Model.V3Infos.ArticleDb[dbview.dbid].DataCount + "个） - 文章库编辑";
                dbview.ShowDialog();
            }
            else
            {
                MessageBox.Show("请先选择一个文章库！", "木有选择文章库", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnViewKeywordDb_Click(object sender, EventArgs e)
        {
            if (Model.V3Infos.KeywordDb.ContainsKey(model.KeywordDbId.ToString()))
            {
                frm_关键词库 view = new frm_关键词库();
                view.Dbid = model.KeywordDbId.ToString();
                view.Text = Model.V3Infos.KeywordDb[view.Dbid].Name + "(关键词总数：" + Model.V3Infos.KeywordDb[view.Dbid].Keywords.Count + "个） - 关键词库编辑";
                view.ShowDialog();
                if (view.IsSave)
                    Model.V3Infos.KeywordDb[view.Dbid].Keywords = view.WordList;
                Bll.DbBll.SaveDb(2, view.Dbid);
                view.Dispose();
            }
            else
            {
                MessageBox.Show("请先选择一个关键词库！", "木有选择关键词库", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonX1_Click_1(object sender, EventArgs e)
        {
            try
            {
                Model.V3Infos.MainDb.Spiderque[model.GetModel + "-" + Model.V3Infos.TaskDb[model.id].HashDbId].Clear();
                XtraMessageBox.Show( "清空完成，请注意也顺道清空一下哈词库或者换一个，否则还是不会重新爬行的！", "清空完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch { }
        }

        private void buttonX2_Click(object sender, EventArgs e)//网右移动
        {
            
         
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
           
        }
       
        private void switchButton2_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            if (DevExpress.XtraEditors.XtraMessageBox.Show(
                "您确认要为选中的这" + gridControl_main_view.SelectedRowsCount + "个站点创建任务吗？", "确认创建",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                addthread = new Thread(StartPiLiang);
                addthread.Start();
                addthread.IsBackground = true;
            }

        }
        public void ShowLoading()
        {
            ipanel.Location=new Point(0,0);
            ipanel.Width = this.Width;
            ipanel.Height = this.Height;
            ipanel.Visible = true;

          
        }
        public void CloseLoading()
        {
            ipanel.Visible = false;
            if (switchButton2.IsOn == true)//自动创建
            {
                btnArticleDefault.Enabled = false;
                btnHashDefault.Enabled = false;
                btnKeywordDefault.Enabled = false;
                btnLinkDefault.Enabled = false;
                btnReplaceDefault.Enabled = false;

                btnSelectArticle.Enabled = false;
                btnSelectHash.Enabled = false;
                btnSelectLink.Enabled = false;
                btnSelectReplace.Enabled = false;
                btnSelectKeyword.Enabled = false;

                btnAddhash.Enabled = false;
                btnAddKeyword.Enabled = false;
                btnAddLink.Enabled = false;
                btnAddArticle.Enabled = false;
                btnReplace.Enabled = false;

            }
            else//使用现有
            {
                btnArticleDefault.Enabled = true;
                btnHashDefault.Enabled = true;
                btnKeywordDefault.Enabled = true;
                btnLinkDefault.Enabled = true;
                btnReplaceDefault.Enabled = true;

                btnSelectArticle.Enabled = true;
                btnSelectHash.Enabled = true;
                btnSelectLink.Enabled = true;
                btnSelectReplace.Enabled = true;
                btnSelectKeyword.Enabled = true;

                btnAddhash.Enabled = true;
                btnAddKeyword.Enabled = true;
                btnAddLink.Enabled = true;
                btnAddArticle.Enabled = true;
                btnReplace.Enabled = true;
            }
        }

        public void StartPiLiang() //开始生成
        {
            int count = 0;
            if (gridControl_main_view.SelectedRowsCount > 0)
            {
               
             
                    this.Invoke((EventHandler)(delegate
                    {
                        ShowLoading();
                    }));
                    for (int i = 0; i < gridControl_main_view.SelectedRowsCount; i++)
                    {
                        Record r = (Record) gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[i]);
                        this.Invoke((EventHandler) (delegate
                        {
                            istate.Text = "[" + (i + 1) + "/" + gridControl_main_view.SelectedRowsCount + "]正在为" +
                                          r.发布点名称 + "生成任务...";
                        }));
                        if (switchButton2.IsOn == true) //自动创建库
                        {
                            if (
                                Model.V3Infos.MainDb.MyModels.ContainsKey(
                                    Model.V3Infos.SendPointDb[r.发布点编号].GetModel))
                            {

                            }
                            else
                            {
                                V3.Common.Log.LogNewline("[c11]站点" + r.发布点名称 + "未选择抓取模块！跳过该站点！[/c]");

                                continue;
                            }

                            if (
                                !Model.V3Infos.MainDb.MyModels.ContainsKey(
                                    Model.V3Infos.SendPointDb[r.发布点编号].PostModel))
                            {
                                V3.Common.Log.LogNewline("[c11]站点" + r.发布点名称 + "未选择发布模块！跳过该站点！[/c]");

                                continue;
                            }
                            if (model.PointId == 0)
                            {
                                DevExpress.XtraEditors.XtraMessageBox.Show("至少选个站点啊，大哥！", "无法继续",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }



                            string html = "";
                            string result = "";
                            V3.Bll.PostBll bll =
                                new Bll.PostBll(
                                    Model.V3Infos.MainDb.MyModels[
                                        Model.V3Infos.SendPointDb[r.发布点编号].PostModel],
                                    Convert.ToInt32(r.发布点编号), 0);
                            bll.Login(false, ref html);
                            if (
                                Model.V3Infos.MainDb.MyModels[
                                    Model.V3Infos.SendPointDb[r.发布点编号].PostModel]
                                    .Stp2_POST_UsedClass)
                            {

                                result =
                                    bll.RunAction(
                                        Model.V3Infos.MainDb.MyModels[
                                            Model.V3Infos.SendPointDb[r.发布点编号].PostModel]
                                            .Stp2_POST_Get, false, "");
                                string[] jieguo = bll.ClassGet(result);
                                int Success = 0;
                                foreach (string typename in jieguo)
                                {
                                    Model.Task mymodel = new Model.Task();
                                    mymodel = xEngine.Common.XSerializable.CloneObject<Model.Task>(model);
                                    mymodel.GetModel = Model.V3Infos.SendPointDb[r.发布点编号].GetModel;
                                    mymodel.PointId = r.发布点编号;
                                    mymodel.IsAutoTask = switchButton2.IsOn;
                                    mymodel.Jiangetime =Convert.ToInt32(integerInput1.Value);
                                    mymodel.TaskStatusStr = "状态未知...";
                                    mymodel.CountAllGet = 0;
                                    mymodel.CountAllPost = 0;
                                    mymodel.CountThisGet = 0;
                                    mymodel.CountthisPost = 0;
                                    int id = V3.Bll.DbBll.AddDb(1,
                                        Model.V3Infos.SendPointDb[model.PointId].name + "-New文章库",
                                        Model.V3Infos.SendPointDb[mymodel.PointId].GroupTag);
                                    if (id != 0)
                                    {
                                        mymodel.ArticleDbId = id;
                                        V3.Common.Log.LogNewline("[c12]成功添加文章库,编号【“" + id + "”】！[/c]");
                                    }
                                    id = V3.Bll.DbBll.AddDb(3,
                                        Model.V3Infos.SendPointDb[model.PointId].name + "-New哈希库",
                                        Model.V3Infos.SendPointDb[mymodel.PointId].GroupTag);
                                    if (id != 0)
                                    {
                                        V3.Common.Log.LogNewline("[c12]成功添加哈希库,编号【“" + id + "”】！[/c]");
                                        mymodel.HashDbId = id;
                                    }
                                    id = V3.Bll.DbBll.AddDb(4,
                                        Model.V3Infos.SendPointDb[model.PointId].name + "-New替换库",
                                        Model.V3Infos.SendPointDb[mymodel.PointId].GroupTag);
                                    if (id != 0)
                                    {
                                        mymodel.ReplaceDbId = id;
                                        V3.Common.Log.LogNewline("[c12]成功添加替换库,编号【“" + id + "”】！[/c]");
                                    }
                                    id = V3.Bll.DbBll.AddDb(2,
                                        Model.V3Infos.SendPointDb[model.PointId].name + "-New关键词库",
                                        Model.V3Infos.SendPointDb[mymodel.PointId].GroupTag);
                                    if (id != 0)
                                    {
                                        mymodel.KeywordDbId = id;
                                        V3.Common.Log.LogNewline("[c12]成功添加关键词库,编号【“" + id + "”】！[/c]");
                                    }
                                    id = V3.Bll.DbBll.AddDb(5,
                                        Model.V3Infos.SendPointDb[model.PointId].name + "-New链接库",
                                        Model.V3Infos.SendPointDb[mymodel.PointId].GroupTag);
                                    if (id != 0)
                                    {
                                        mymodel.LinkDbId = id;
                                        V3.Common.Log.LogNewline("[c12]成功添加链接库,编号【“" + id + "”】！[/c]");
                                    }

                                    V3.Common.Log.LogNewline("[c12]站点【0" +
                                        Model.V3Infos.SendPointDb[r.发布点编号].name +
                                        "】：成功获取到分类【" + typename.Substring(typename.IndexOf("`") + 1) + "】[/c]");
                                    mymodel.Sendclass = typename.Remove(typename.IndexOf("`"));
                                    mymodel.TaskName = typename.Substring(typename.IndexOf("`") + 1);
                                    if (V3.Bll.TaskBll.Add(mymodel) > 0)
                                    {
                                        Success++;
                                        this.Invoke((EventHandler) (delegate
                                        {
                                            istate.Text = "已经成功添加任务【" + mymodel.TaskName + "】！";
                                        }));
                                        V3.Common.Log.LogNewline(
                                            "[c12]已经成功添加任务【“" + mymodel.TaskName + "”】！[/c]");
                                        count++;
                                        if(Success==spinEdit_MaxCount.Value)
                                        {
                                            V3.Common.Log.LogNewline("[c11]任务【" + mymodel.TaskName + "】成功添加任务数量已达到上限"+spinEdit_MaxCount.Value+"，跳过！”[/c]");
                                            break;
                                        }

                                    }
                                    else
                                    {
                                        V3.Common.Log.LogNewline(
                                            "[c14]任务【" + mymodel.TaskName + "】添加失败！”[/c]");
                                    }
                                }


                            }

                        }
                        else //使用现有
                        {
                            if (Model.V3Infos.ArticleDb.ContainsKey(model.ArticleDbId.ToString()))
                            {
                                txtArticleName.Text = "(" +
                                                      Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].DataCount +
                                                      ")" + "[" +
                                                      Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Id + "]" +
                                                      Model.V3Infos.ArticleDb[model.ArticleDbId.ToString()].Name;
                            }
                            else
                            {

                                txtArticleName.Text = "未设置或已删除！";
                                DevExpress.XtraEditors.XtraMessageBox.Show("您还未选择文章库！", "无法继续",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            if (Model.V3Infos.KeywordDb.ContainsKey(model.KeywordDbId.ToString()))
                            {
                                txtKeywordName.Text = "(" +
                                                      Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Keywords
                                                          .Count + ")" + "[" +
                                                      Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Id + "]" +
                                                      Model.V3Infos.KeywordDb[model.KeywordDbId.ToString()].Name;
                            }
                            else
                            {
                                txtKeywordName.Text = "未设置或已删除！";
                                DevExpress.XtraEditors.XtraMessageBox.Show( "您还未选择关键词库！", "无法继续",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            if (Model.V3Infos.HashDb.ContainsKey(model.HashDbId.ToString()))
                            {
                                txtHashName.Text = "(" + Model.V3Infos.HashDb[model.HashDbId.ToString()].DataCount +
                                                   ")" + "[" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Id +
                                                   "]" + Model.V3Infos.HashDb[model.HashDbId.ToString()].Name;
                            }
                            else
                            {
                                txtHashName.Text = "未设置或已删除！";
                                DevExpress.XtraEditors.XtraMessageBox.Show( "您还未选择哈希库！", "无法继续",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            if (Model.V3Infos.ReplaceDb.ContainsKey(model.ReplaceDbId.ToString()))
                            {
                                txtReplaceName.Text = "(" +
                                                      Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Words
                                                          .Count + ")" + "[" +
                                                      Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Id + "]" +
                                                      Model.V3Infos.ReplaceDb[model.ReplaceDbId.ToString()].Name;
                            }
                            else
                            {
                                txtReplaceName.Text = "未设置或已删除！";
                                DevExpress.XtraEditors.XtraMessageBox.Show( "您还未选择替换库！", "无法继续",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            if (Model.V3Infos.LinkDb.ContainsKey(model.LinkDbId.ToString()))
                            {
                                txtLinkName.Text = "(" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Links.Count +
                                                   ")" + "[" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Id +
                                                   "]" + Model.V3Infos.LinkDb[model.LinkDbId.ToString()].Name;
                            }
                            else
                            {
                                txtLinkName.Text = "未设置或已删除！";
                                DevExpress.XtraEditors.XtraMessageBox.Show( "您还未选择关链接库！", "无法继续",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }



                            string html = "";
                            string result = "";
                            V3.Bll.PostBll bll =
                                new Bll.PostBll(
                                    Model.V3Infos.MainDb.MyModels[
                                        Model.V3Infos.SendPointDb[r.发布点编号].PostModel],
                                    Convert.ToInt32(r.发布点编号), 0);
                            bll.Login(false, ref html);
                            if (
                                Model.V3Infos.MainDb.MyModels[
                                    Model.V3Infos.SendPointDb[r.发布点编号].PostModel]
                                    .Stp2_POST_UsedClass)
                            {

                                result =
                                    bll.RunAction(
                                        Model.V3Infos.MainDb.MyModels[
                                            Model.V3Infos.SendPointDb[r.发布点编号].PostModel]
                                            .Stp2_POST_Get, false, "");
                                string[] jieguo = bll.ClassGet(result);
                                int Success = 0;
                                foreach (string typename in jieguo)
                                {
                                    Model.Task mymodel = new Model.Task();
                                    mymodel = xEngine.Common.XSerializable.CloneObject<Model.Task>(model);
                                    mymodel.GetModel = Model.V3Infos.SendPointDb[r.发布点编号].GetModel;
                                    mymodel.PointId = r.发布点编号;
                                    V3.Common.Log.LogNewline(
                                        "[c12]站点【" +
                                        Model.V3Infos.SendPointDb[r.发布点编号].name +
                                        "】：成功获取到分类【" + typename.Substring(typename.IndexOf("`") + 1) + "】[/c]");
                                    mymodel.PointId = r.发布点编号;
                                    mymodel.Sendclass = typename.Remove(typename.IndexOf("`"));
                                    mymodel.TaskName = typename.Substring(typename.IndexOf("`") + 1);
                                    if (V3.Bll.TaskBll.Add(mymodel) > 0)
                                    {
                                        Success++;
                                        V3.Common.Log.LogNewline(
                                            "[c12]已经成功添加任务【“" + mymodel.TaskName + "”】！[/c]");
                                        count++;
                                        if (Success == spinEdit_MaxCount.Value)
                                        {
                                            V3.Common.Log.LogNewline("[c11]任务【" + mymodel.TaskName + "】成功添加任务数量已达到上限" + spinEdit_MaxCount.Value + "，跳过！”[/c]");
                                            break;
                                        }

                                    }
                                    else
                                    {
                                        V3.Common.Log.LogNewline(
                                            "[c14]任务【" + mymodel.TaskName + "】添加失败！”[/c]");
                                    }
                                }


                            }


                        }



                    }
                    V3.Common.Log.LogNewline("[c12]已经成功添加" + count + "个任务！[/c]");
                    this.Invoke((EventHandler) (delegate
                    {
                        CloseLoading();
                        DevExpress.XtraEditors.XtraMessageBox.Show("已经成功添加" + count + "个任务！", "添加结果",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));




                }
            }



        

        private void buttonquxiao_Click(object sender, EventArgs e)
        {
           CloseLoading();
            if (addthread.ThreadState != ThreadState.Aborted)
            {
                addthread.Abort();
            }
        }

        private void PiLiangTasKFrm_Shown(object sender, EventArgs e)
        {
            GetAllWeb();
        }

        private void frmTasks_Add_Load(object sender, EventArgs e)
        {
            gridControl_main_view.SelectionChanged+=new DevExpress.Data.SelectionChangedEventHandler(gridControl_main_view_SelectionChanged);
        }

        private void gridControl_main_view_SelectionChanged(object sender, EventArgs e)
        {
            label_select.Text = "为" + gridControl_main_view.SelectedRowsCount.ToString() + "个发布点生成任务";
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

        private void IsAutoTask_Toggled(object sender, EventArgs e)
        {
            if (IsAutoTask.IsOn)
                integerInput1.Enabled = true;
            else
                integerInput1.Enabled = false;
        }

        private void switchButton2_Toggled(object sender, EventArgs e)
        {
            if (switchButton2.IsOn == true)//自动创建
            {
                btnArticleDefault.Enabled = false;
                btnHashDefault.Enabled = false;
                btnKeywordDefault.Enabled = false;
                btnLinkDefault.Enabled = false;
                btnReplaceDefault.Enabled = false;

                btnSelectArticle.Enabled = false;
                btnSelectHash.Enabled = false;
                btnSelectLink.Enabled = false;
                btnSelectReplace.Enabled = false;
                btnSelectKeyword.Enabled = false;

                btnAddhash.Enabled = false;
                btnAddKeyword.Enabled = false;
                btnAddLink.Enabled = false;
                btnAddArticle.Enabled = false;
                btnReplace.Enabled = false;

            }
            else//使用现有
            {
                btnArticleDefault.Enabled = true;
                btnHashDefault.Enabled = true;
                btnKeywordDefault.Enabled = true;
                btnLinkDefault.Enabled = true;
                btnReplaceDefault.Enabled = true;

                btnSelectArticle.Enabled = true;
                btnSelectHash.Enabled = true;
                btnSelectLink.Enabled = true;
                btnSelectReplace.Enabled = true;
                btnSelectKeyword.Enabled = true;

                btnAddhash.Enabled = true;
                btnAddKeyword.Enabled = true;
                btnAddLink.Enabled = true;
                btnAddArticle.Enabled = true;
                btnReplace.Enabled = true;
            }
        }

       


     
    }
}
