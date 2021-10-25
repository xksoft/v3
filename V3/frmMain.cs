using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Helpers;
using Model;
using V3.Common;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.Design.TypePickEditor;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTreeList.Native;
using DevExpress.XtraTreeList.Nodes;
using V3.V3Form.发布模块;
using V3.V3Form.抓取模块;

namespace V3
{
    public partial class frmMain : RibbonForm
    {
        public static frmMain MyFrmMain;
        #region 窗体初始化
        TimeSpan Starttime;
        V3Form.frmModelShop frmModelShop = new V3Form.frmModelShop();
        V3Form.DbManage articleDbfrm = new V3Form.DbManage();
        V3Form.DbManage keywordDbfrm = new V3Form.DbManage();
        V3Form.DbManage hashDbfrm = new V3Form.DbManage();
        V3Form.DbManage replaceDbfrm = new V3Form.DbManage();
        V3Form.DbManage linkDbfrm = new V3Form.DbManage();
        V3Form.frmSystem settingfrm = new V3Form.frmSystem();
        #endregion
        private delegate void myDelegate();
        public bool LockTaskList = false;
        public System.Collections.ArrayList Tasks = new System.Collections.ArrayList();
        public frmMain()
        {
            InitializeComponent();
            MyFrmMain = this;

        }

        #region 数据模型
        public static int ColumnCount = 8;
        public static RecordCollection TaskColl = new RecordCollection();
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
            string[] values = new string[8];
            public int 任务ID { get { return Convert.ToInt32(values[0]); } set { SetValue(0, value); } }
            public string 任务名称 { get { return values[1]; } set { SetValue(1, value); } }
            public string 发布点 { get { return values[2]; } set { SetValue(2, value); } }
            public string 分组 { get { return values[3]; } set { SetValue(3, value); } }
            public string 任务运行量 { get { return values[4]; } set { SetValue(4, value); } }
            public string 任务状态 { get { return values[5]; } set { SetValue(5, value); } }
            public int 任务状态编号 { get { return Convert.ToInt32(values[6]); } set { SetValue(6, value); } }
            public string 实时运行 { get { return values[7]; } set { SetValue(7, value); } }
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

            if (gridControl_main_view.SelectedRowsCount > 0)
            {
                if (e.Button == MouseButtons.Right)
                {

                    popupMenu_Task.ShowPopup(Control.MousePosition);
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
        #endregion

       

        #region 启动、关闭相关操作
        public void start()
        {
            
            Log.LogNewline("[c11]正在加载模块数据，请稍候...[/c]");
           
            Log.LogNewline("[c11]系统正在进行窗体初始化，这个过程需要耗时1分钟左右，请稍候...[/c]");
            Program.f_frmTask = new V3Form.frmTask();
            Program.f_frmPoint = new V3Form.frmPoint();
            Program.f_frmReplaceTag = new V3Form.frmReplaceTag();
            Program.f_frmTasks_Modify = new V3Form.frmTasks_Modify();
            Log.LogNewline("[c12]窗体初始化完毕！[/c]");
            Log.LogNewline("[c12]V3系统已经全部初始化完毕，您可以开始使用啦！[/c]");
            setdefault();
           Starttime = new TimeSpan(DateTime.Now.Ticks);
            try
            {
                this.Opacity = 100;
                this.ShowIcon = true;
                this.ShowInTaskbar = true;
                this.Activate();
            }
            catch (Exception ex) { Log.LogNewline("[c14]出错啦：" + ex.Message+"[/c]"); }
            this.Text = "站群引擎V3";
        }
        //关闭时保存数据
        public bool isclose = false;
        void StopThread()
        {
           
            int[] tid = new int[Model.V3Infos.TaskThread.Keys.Count];
            Model.V3Infos.TaskThread.Keys.CopyTo(tid, 0);
            for (int i = 0; i < tid.Length; i++)
            {
                try
                {
                    Log.LogNewline("[c11][系统]正在停止任务线程[" + tid[i] + "]....[/c]");
                    if (Model.V3Infos.TaskCancelToken.ContainsKey(tid[i]))
                    {
                       
                        Model.V3Infos.TaskCancelToken[tid[i]].Cancel();
                    }
                }
                catch { }
            }
            var ks = Model.V3Infos.TaskDb.Keys.ToList();
            foreach (var k in ks)
            {
                if (Model.V3Infos.TaskDb.ContainsKey(k))
                {
                    if (Model.V3Infos.TaskDb[k].TaskStatusId != 4)
                    {
                        Model.V3Infos.TaskDb[k].TaskStatusId = 4;
                        Model.V3Infos.TaskDb[k].TaskStatusStr = "与" + DateTime.Now.ToString() + "退出系统时强制停止";
                    }
                }
                
            }
         
        }
        void SaveLinkDb()
        {
            Log.LogNewline("[c11][系统]正在保存链接库信息，如果链接库较多可能需要较长时间....[/c]");
            string[] tid = new string[Model.V3Infos.LinkDb.Keys.Count];
            Model.V3Infos.LinkDb.Keys.CopyTo(tid, 0);
            for (int i = 0; i < tid.Length; i++)
            {
                    V3Helper.saveLinkDb();
            }
        }
        int chkcount = 0;
        int allgetcount = 0;
        int allpostcount = 0;
        int alldatacount = 0;
        int thisgetcount = 0;
        int thispostcount = 0;
        long bak = 0;
        int timecount = 0;
        int baktime = 0;
        int runs = 0;
        int sleep = 0;
        int stop = 0;
        void setdefault()
        {
            Int32[] taskid = new Int32[Model.V3Infos.TaskDb.Count];
            Model.V3Infos.TaskDb.Keys.CopyTo(taskid, 0);
            allpostcount = 0;
            allgetcount = 0;
            thisgetcount = 0;
            thispostcount = 0;
            for (int i = 0; i < taskid.Length; i++)
            {
                try
                {
                    Model.V3Infos.TaskDb[taskid[i]].CountThisGet = 0;
                    Model.V3Infos.TaskDb[taskid[i]].CountthisPost = 0;
                }
                catch
                {
                    Model.V3Infos.TaskDb.Remove(taskid[i]);
                }
            }
        }

        //启动线程
        #endregion
        public void LoadGroup()
        {
           
            treeList_Main.Nodes[0].Tag = "TasK0";
            treeList_Main.Nodes[0].Nodes[0].Tag = "task1_0";
            treeList_Main.Nodes[0].Nodes[0].Nodes[0].Tag = "task1_1";
            treeList_Main.Nodes[0].Nodes[0].Nodes[1].Tag = "task1_2";
            treeList_Main.Nodes[0].Nodes[0].Nodes[2].Tag = "task1_3";
            treeList_Main.Nodes[0].Nodes[1].Tag = "task2_0";
            treeList_Main.Nodes[0].Nodes[1].Nodes[0].Tag = "task2_1";
            treeList_Main.Nodes[0].Nodes[1].Nodes[1].Tag = "task2_2";
            treeList_Main.Nodes[0].Nodes[1].Nodes[2].Tag = "task2_3";
            treeList_Main.Nodes[0].Nodes[2].Tag = "task3_0";
            treeList_Main.Nodes[0].Nodes[2].Nodes[0].Tag = "task3_1";
            treeList_Main.Nodes[0].Nodes[2].Nodes[1].Tag = "task3_2";
            treeList_Main.Nodes[1].Tag = "mgr";
            treeList_Main.Nodes[1].Nodes[0].Tag = "DefaultGroup";
           
         

            int i = 1;
            foreach (KeyValuePair<string, string> value in Model.V3Infos.MainDb.GroupList)
            {
                treeList_Main.AppendNode(new object[] { value.Value }, treeList_Main.Nodes[1], value.Key.ToString());
                treeList_Main.Nodes[1].Nodes[i].SelectImageIndex = 8;
                treeList_Main.Nodes[1].Nodes[i].StateImageIndex = 8;
                treeList_Main.Nodes[1].Nodes[i].ImageIndex = 8;
                i++;

                
                
            }
        }
        public void LoadPointList()
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            foreach (KeyValuePair<int, Model.SendPoint> ss in Model.V3Infos.SendPointDb)
            {
                list.Add(ss.Value);
            }
            for (int i = 1; i < treeList_Main.Nodes[1].Nodes.Count; i++)
            {
                treeList_Main.Nodes[1].Nodes[i].Nodes.Clear();
                string tag = treeList_Main.Nodes[1].Nodes[i].Tag.ToString();
                {
                    int c = 0;
                    for (int ii = 0; ii < list.Count; ii++)
                    {
                        Model.SendPoint temp = (Model.SendPoint)list[ii];
                        if (temp.GroupTag == tag)
                        {
                            treeList_Main.AppendNode(new object[] { ((Model.SendPoint)list[ii]).name }, treeList_Main.Nodes[1].Nodes[i], "point-" + ((Model.SendPoint)list[ii]).id);
                            treeList_Main.Nodes[1].Nodes[i].Nodes[c].SelectImageIndex = 10;
                            treeList_Main.Nodes[1].Nodes[i].Nodes[c].StateImageIndex = 10;
                            treeList_Main.Nodes[1].Nodes[i].Nodes[c].ImageIndex = 10;
                            list.Remove(list[ii]);
                            c++;
                            ii--;
                        }
                    }
                }
            }
            treeList_Main.Nodes[1].Nodes[0].Nodes.Clear();
            int DefaultIndex = 0;
            for (int ii = 0; ii < list.Count; ii++)
            {
              
                Model.V3Infos.SendPointDb[((Model.SendPoint)list[ii]).id].GroupTag = "DefaultGroup";
                treeList_Main.AppendNode(new object[] { ((Model.SendPoint)list[ii]).name }, treeList_Main.Nodes[1].Nodes[0], "point-" + ((Model.SendPoint)list[ii]).id);
                treeList_Main.Nodes[1].Nodes[0].Nodes[DefaultIndex].SelectImageIndex = 10;
                treeList_Main.Nodes[1].Nodes[0].Nodes[DefaultIndex].StateImageIndex = 10;
                treeList_Main.Nodes[1].Nodes[0].Nodes[DefaultIndex].ImageIndex = 10;
                DefaultIndex++;
            }
            //changstatusstr();
        }
        public void UpdatePoint()
        {
            var ks = Model.V3Infos.TaskDb.Keys.ToList();
            for (int i = 1; i < treeList_Main.Nodes[1].Nodes.Count; i++)
            {
                treeList_Main.Nodes[1].Nodes[i].SetValue(0, "(" + treeList_Main.Nodes[1].Nodes[i].Nodes.Count + ")" + Model.V3Infos.MainDb.GroupList[treeList_Main.Nodes[1].Nodes[i].Tag.ToString()]);
                for (int ii = 0; ii < treeList_Main.Nodes[1].Nodes[i].Nodes.Count; ii++)
                {
                    int taskcount = 0;
                    int pointid = Convert.ToInt32(treeList_Main.Nodes[1].Nodes[i].Nodes[ii].Tag.ToString().Split('-')[1]);
                    foreach (var k in ks)
                    {
                        if (Model.V3Infos.TaskDb.ContainsKey(k))
                        {
                            if (Model.V3Infos.TaskDb[k].PointId == pointid)
                                taskcount++;
                        }
                    }
                    treeList_Main.Nodes[1].Nodes[i].Nodes[ii].SetValue(0,"(" + taskcount + ")[" + Model.V3Infos.SendPointDb[pointid].id + "]" + Model.V3Infos.SendPointDb[pointid].name);
                }
            }
            treeList_Main.Nodes[1].Nodes[0].SetValue(0, "(" + treeList_Main.Nodes[1].Nodes[0].Nodes.Count + ")默认分组");
            for (int ii = 0; ii < treeList_Main.Nodes[1].Nodes[0].Nodes.Count; ii++)
            {
                int taskcount = 0;
                int pointid = Convert.ToInt32(treeList_Main.Nodes[1].Nodes[0].Nodes[ii].Tag.ToString().Split('-')[1]);
                foreach (var k in ks)
                {
                    if (Model.V3Infos.TaskDb.ContainsKey(k))
                    {
                        if (Model.V3Infos.TaskDb[k].PointId == pointid)
                            taskcount++;
                    }
                }
                treeList_Main.Nodes[1].Nodes[0].Nodes[ii].SetValue(0, "(" + taskcount + ")[" + Model.V3Infos.SendPointDb[pointid].id + "]" + Model.V3Infos.SendPointDb[pointid].name);
            }
           
        }
        public void ShowList()
        {
            this.Invoke((EventHandler)(delegate
            {

                gridControl_main.DataSource = TaskColl;
                gridControl_main_view.Columns["任务状态"].Width = 80;
                gridControl_main_view.Columns["分组"].Width = 100;
                gridControl_main_view.Columns["任务ID"].Width = 50;
                gridControl_main_view.Columns["任务名称"].Width = 130;
                gridControl_main_view.Columns["发布点"].Width = 120;
                gridControl_main_view.Columns["任务运行量"].Width = 240;

               gridControl_main_view.Columns["任务状态"].VisibleIndex = 0;
               gridControl_main_view.Columns["分组"].VisibleIndex = 1;
               gridControl_main_view.Columns["任务ID"].VisibleIndex = 2;
               gridControl_main_view.Columns["任务名称"].VisibleIndex = 3;
               gridControl_main_view.Columns["发布点"].VisibleIndex = 4;
               gridControl_main_view.Columns["任务运行量"].VisibleIndex = 5;
               gridControl_main_view.Columns["任务状态编号"].Visible = false;
           

            gridControl_main_view.RefreshData();

            }));
            LockTaskList = false;
        }
        public void LoadTaskList()
        {
        
            TaskColl.Clear();
          
            for (int i = 0; i < Tasks.Count; i++)
            {
               
                string GroupName = string.Empty;
                int taskid = Convert.ToInt32(Tasks[i]);
                if (!Model.V3Infos.SendPointDb.ContainsKey(Model.V3Infos.TaskDb[taskid].PointId))
                {
                    V3.Bll.TaskBll.Delete(taskid);
                    continue;
                }
                if (Model.V3Infos.MainDb.GroupList.ContainsKey(Model.V3Infos.SendPointDb[Model.V3Infos.TaskDb[taskid].PointId].GroupTag))
                    GroupName = Model.V3Infos.MainDb.GroupList[Model.V3Infos.SendPointDb[Model.V3Infos.TaskDb[taskid].PointId].GroupTag];
                else
                    GroupName = "默认分组";

                Record r = new Record();
                r.任务ID = Model.V3Infos.TaskDb[taskid].id;
                r.发布点 = Model.V3Infos.SendPointDb[Model.V3Infos.TaskDb[taskid].PointId].name;
                r.分组 = GroupName;
                r.任务名称 = Model.V3Infos.TaskDb[taskid].TaskName;
                r.任务运行量 = "本次抓取:" + Model.V3Infos.TaskDb[taskid].CountThisGet + " 总抓取:" + Model.V3Infos.TaskDb[taskid].CountAllGet + " 今日发布:" + Model.V3Infos.TaskDb[taskid].CountthisPost + " 总发布:" + Model.V3Infos.TaskDb[taskid].CountAllPost;
                r.任务状态 = "停止中";
                r.实时运行 = Model.V3Infos.TaskDb[taskid].TaskStatusStr;
                if (V3.Bll.TaskBll.getTaskStatus(taskid) == 1)
                {
                    r.任务状态编号 = Model.V3Infos.TaskDb[taskid].TaskStatusId;
                    if (r.任务状态编号 == 0)
                    {
                       r.任务状态 = "等待中...";
                        runs++;

                    }
                    else if (r.任务状态编号 == 1)
                    {
                        r.任务状态 = "抓取中...";

                    }
                    else if (r.任务状态编号 == 2)
                    {
                        r.任务状态 = "发布中...";

                    }
                    else if (r.任务状态编号 == 3)
                    {
                        r.任务状态 = "休眠中";

                    }

                }
                else
                {
                    r.任务状态编号 = 4;
                }
                TaskColl.Add(r);

            }
           
            ShowList();
           
        }
        public void LoadTaskAry()
        {
            LockTaskList = true;
            if (treeList_Main.FocusedNode== null) { return; }
            DevExpress.XtraTreeList.Nodes.TreeListNode node = treeList_Main.FocusedNode;
            if (node != null)
            {
                Tasks.Clear();
                var ks = Model.V3Infos.TaskDb.Keys.ToList();
                #region 按任务状态归类
                if (node.Tag.ToString() == "task1_0")
                {
                   
                    foreach (var k in ks)
                    {
                        if (Model.V3Infos.TaskDb.ContainsKey(k))
                        {
                            Tasks.Add(k);
                        }
                    }
                }
                else if (node.Tag.ToString() == "task1_1")
                {
                    foreach (var k in ks)
                    {
                        if (Model.V3Infos.TaskDb.ContainsKey(k))
                        {
                            if (Model.V3Infos.TaskDb[k].TaskStatusId == 1 || Model.V3Infos.TaskDb[k].TaskStatusId == 2)
                                Tasks.Add(k);
                        }

                    }
                }
                else if (node.Tag.ToString() == "task1_2")
                {
                    foreach (var k in ks)
                    {
                        if (Model.V3Infos.TaskDb.ContainsKey(k))
                        {
                            if (Model.V3Infos.TaskDb[k].TaskStatusId == 3)
                                Tasks.Add(k);
                        }
                    }
                }
                else if (node.Tag.ToString() == "task1_3")
                {
                    foreach (var k in ks)
                    {
                        if (Model.V3Infos.TaskDb.ContainsKey(k))
                        {
                            if (Model.V3Infos.TaskDb[k].TaskStatusId == 4)
                                Tasks.Add(k);
                        }
                    }
                }
                else if (node.Tag.ToString() == "task2_0")
                {
                    foreach (var k in ks)
                    {
                        if (Model.V3Infos.TaskDb.ContainsKey(k))
                        {
                            if (Model.V3Infos.TaskDb[k].IsAutoTask)
                                Tasks.Add(k);
                        }
                    }
                }
                else if (node.Tag.ToString() == "task2_1")
                {
                    foreach (var k in ks)
                    {
                        if (Model.V3Infos.TaskDb.ContainsKey(k))
                        {

                            if (Model.V3Infos.TaskDb[k].IsAutoTask &&
                                (Model.V3Infos.TaskDb[k].TaskStatusId == 1 ||
                                 Model.V3Infos.TaskDb[k].TaskStatusId == 2))
                                Tasks.Add(k);

                        }
                    }
                }
                else if (node.Tag.ToString() == "task2_2")
                {
                    foreach (var k in ks)
                    {
                        if (Model.V3Infos.TaskDb.ContainsKey(k))
                        {
                            if (Model.V3Infos.TaskDb[k].IsAutoTask && Model.V3Infos.TaskDb[k].TaskStatusId == 3)
                                Tasks.Add(k);
                        }
                    }
                }
                else if (node.Tag.ToString() == "task2_3")
                {
                    foreach (var k in ks)
                    {
                        if (Model.V3Infos.TaskDb.ContainsKey(k))
                        {
                            if (Model.V3Infos.TaskDb[k].IsAutoTask && Model.V3Infos.TaskDb[k].TaskStatusId == 4)
                                Tasks.Add(k);
                        }
                    }
                }
                else if (node.Tag.ToString() == "task3_0")
                {
                    foreach (var k in ks)
                    {
                        if (Model.V3Infos.TaskDb.ContainsKey(k))
                        {
                            if (!Model.V3Infos.TaskDb[k].IsAutoTask)
                                Tasks.Add(k);
                        }
                    }
                }
                else if (node.Tag.ToString() == "task3_1")
                {
                    foreach (var k in ks)
                    {
                        if (Model.V3Infos.TaskDb.ContainsKey(k))
                        {
                            if (!Model.V3Infos.TaskDb[k].IsAutoTask &&
                                (Model.V3Infos.TaskDb[k].TaskStatusId == 1 || Model.V3Infos.TaskDb[k].TaskStatusId == 2))
                                Tasks.Add(k);
                        }
                    }
                }
                else if (node.Tag.ToString() == "task3_2")
                {
                    foreach (var k in ks)
                    {
                        if (Model.V3Infos.TaskDb.ContainsKey(k))
                        {
                            if (!Model.V3Infos.TaskDb[k].IsAutoTask && Model.V3Infos.TaskDb[k].TaskStatusId == 4)
                                Tasks.Add(k);
                        }
                    }
                }
                #endregion

                #region 按组别、节点归类
                else if (node.Tag.ToString().ToLower().Contains("group"))
                {
                    string grouptag = node.Tag.ToString();
                    foreach (var k in ks)
                    {
                        if (Model.V3Infos.TaskDb.ContainsKey(k))
                        {
                            if (!Model.V3Infos.SendPointDb.ContainsKey(Model.V3Infos.TaskDb[k].PointId))
                                continue;
                            if (Model.V3Infos.SendPointDb[Model.V3Infos.TaskDb[k].PointId].GroupTag == grouptag)
                                Tasks.Add(k);
                        }
                    }
                }
                else if (node.Tag.ToString().ToLower().Contains("point"))
                {

                    int pointid = Convert.ToInt32(node.Tag.ToString().Split('-')[1]);
                    foreach (var k in ks)
                    {
                        if (Model.V3Infos.TaskDb.ContainsKey(k))
                        {
                            if (Model.V3Infos.TaskDb[k].PointId == pointid)
                                Tasks.Add(k);
                        }
                    }
                }
                else
                {
                    return;
                }
                #endregion

            }
            Tasks.Reverse();
            LoadTaskList();
           
        }
        public void StartUpdateTaskStatus()
        {
           
            Thread thread = new Thread(delegate()
           {
               while (true)
               {
                   UpdateTaskStatus();
                  
                   Thread.Sleep(5000);
               }
           });
           thread.IsBackground = true;
           thread.Start();
        
        
        
        }
        public void UpdateTaskStatus()
        {
            try
            {
                if (isclose || this.IsDisposed || LockTaskList)
                {
                    return;
                }
                if (this.InvokeRequired)
                {
                    try
                    {

                        this.Invoke(new myDelegate(UpdateTaskStatus));
                    }
                    catch
                    {

                    }


                }
                else
                {
                    baktime++;

                  if(baktime>=1440)
                  {
                      V3Helper.bakdbmain();
                      baktime = 0;
                  }


                    gridControl_main.BeginUpdate();
                    timecount = 0;
                    runs = 0;
                    sleep = 0;
                    stop = 0;
                    for (int i = 0; i < TaskColl.Count; i++)
                    {

                        if (LockTaskList)
                        {
                            break;
                        }
                        string GroupName = string.Empty;
                        int taskid = Convert.ToInt32(TaskColl[i].任务ID);
                        if (!Model.V3Infos.SendPointDb.ContainsKey(Model.V3Infos.TaskDb[taskid].PointId))
                        {
                            V3.Bll.TaskBll.Delete(taskid);
                            continue;
                        }
                        if (
                            Model.V3Infos.MainDb.GroupList.ContainsKey(
                                Model.V3Infos.SendPointDb[Model.V3Infos.TaskDb[taskid].PointId].GroupTag))
                            GroupName =
                                Model.V3Infos.MainDb.GroupList[
                                    Model.V3Infos.SendPointDb[Model.V3Infos.TaskDb[taskid].PointId].GroupTag];
                        else
                            GroupName = "默认分组";

                        Record r = TaskColl[i];
                        r.任务ID = Model.V3Infos.TaskDb[taskid].id;
                        r.发布点 = Model.V3Infos.SendPointDb[Model.V3Infos.TaskDb[taskid].PointId].name;
                        r.分组 = GroupName;
                        r.任务名称 = Model.V3Infos.TaskDb[taskid].TaskName;
                        r.任务运行量 = "本次抓取:" + Model.V3Infos.TaskDb[taskid].CountThisGet + " 总抓取:" +
                                  Model.V3Infos.TaskDb[taskid].CountAllGet + " 今日发布:" +
                                  Model.V3Infos.TaskDb[taskid].CountthisPost + " 总发布:" +
                                  Model.V3Infos.TaskDb[taskid].CountAllPost;
                        r.任务状态编号 = Model.V3Infos.TaskDb[taskid].TaskStatusId;
                        r.实时运行 = Model.V3Infos.TaskDb[taskid].TaskStatusStr;
                        if (V3.Bll.TaskBll.getTaskStatus(taskid) == 1)
                        {
                            if (r.任务状态编号 == 0)
                            {
                                r.任务状态 = "等待中...";
                                runs++;

                            }
                            else if (r.任务状态编号 == 1)
                            {
                                r.任务状态 = "抓取中...";
                                runs++;

                            }
                            else if (r.任务状态编号 == 2)
                            {
                                r.任务状态 = "发布中...";

                            }
                            else if (TaskColl[i].任务状态编号 == 3)
                            {
                                r.任务状态 = "休眠中";
                                sleep++;

                            }

                        }
                        else
                        {
                            r.任务状态编号 = 4;
                            r.任务状态 = "停止中";
                            stop++;
                        }


                    }
                    if (!isclose && !this.IsDisposed && !LockTaskList)
                    {

                        gridControl_main.EndUpdate();
                        label_network.Caption = Program.GetNetWork();
                        label_Status.Caption = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] 发布点" +
                                               Model.V3Infos.SendPointDb.Count + "个（总共" +
                                               Model.V3Infos.TaskDb.Count + "个任务，运行中" + runs + "，休眠中" + sleep +
                                               "，停止中" + stop + "）";

                    }

                }
            }
            catch
            {

            }
        }
        public void StartRunTaskList()
        {
            Thread thread = new Thread(delegate()
            {
                while (true)
                {

                    if (Program.TaskRunList.Count > 0)
                    {

                       int task_id = Convert.ToInt32(Program.TaskRunList[0]);
                       V3.Common.TaskRun.Run(task_id);
                       Program.TaskRunList.RemoveAt(0);
                       if (Model.V3Infos.MainDb.Startjiange > 0)
                       { 
                           Common.Log.LogNewline("[c11]"+Model.V3Infos.MainDb.Startjiange + "秒后启动下一个任务，剩余" + Program.TaskRunList.Count + "个任务等待启动...[/c]");
                           Thread.Sleep(Model.V3Infos.MainDb.Startjiange * 1000);
                          
                       }
  
                    }
                    Thread.Sleep(100);
                }  
            });
            thread.IsBackground = true;
            thread.Start();

        }
        public void StartWaitTask()
        {
            Thread thread = new Thread(delegate()
            {
                while (true)
                {
                    List<int>DeleteId=new List<int>();
                    var ks = Model.V3Infos.TaskWaiting.Keys.ToList();
                    foreach (var k in ks)
                    {
                        if (Model.V3Infos.TaskWaiting.ContainsKey(k))
                        {
                            if (Model.V3Infos.TaskWaiting[k] <= DateTime.Now)
                            {
                                DeleteId.Add(k);


                            }

                        }
                       

                    }
                    foreach (var id in DeleteId)
                    {
                        if (Model.V3Infos.TaskWaiting.ContainsKey(id))
                        {
                            Model.V3Infos.TaskWaiting.Remove(id);
                            TaskRun.Run(id);
                        }
                    }
                    Thread.Sleep(500);
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        public void AutoStartTask()
        {
            int count = 0;
            var ks = Model.V3Infos.TaskDb.Keys.ToList();
            foreach (var k in ks)
            {
                if (Model.V3Infos.TaskDb.ContainsKey(k))
                {
                    if (Model.V3Infos.TaskDb[k].IsAutoTask)
                    {
                        if (!Program.TaskRunList.Contains(k))
                        {
                            count++;
                            Program.TaskRunList.Add(k);
                        }
                    }

                }

            }
            if (count>0)
            {
                Common.Log.LogNewline("[c12]已成功自动将"+count+"个任务加入到启动列队！[/c]");
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            start();
            DevExpress.XtraEditors.XtraMessageBox.AllowCustomLookAndFeel = true;
            defaultLookAndFeel.LookAndFeel.SetSkinStyle(V3Infos.MainDb.SkinName);
            SkinHelper.InitSkinGallery(rgbiSkins);
            Program.Starting = false;
            if (Model.V3Infos.MainDb.ShowConsole == 1)
            {

                btn_ShowConsole.Glyph = Properties.Resources.btn_ShowConsole;
               
               

            }
            else
            {
                btn_ShowConsole.Glyph = Properties.Resources.btn_CloseConsole;
                V3.Common.Console.StartConsole("V3运行日志监控窗口");
            }

            treeList_Main.ExpandToLevel(1);
            LoadGroup();
            LoadPointList();
            UpdatePoint();
            treeList_Main.Nodes[0].Nodes[0].Selected = true;
            StartUpdateTaskStatus();
           StartRunTaskList();
            Model.V3Infos.TaskWaiting.Clear();
           StartWaitTask();
            
            gridControl_main_view.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(gridControl_main_view_RowStyle);
            if (Model.V3Infos.MainDb.AutoRunTask)
            {
                AutoStartTask();
            }

         
        }
        private void barButtonItem_TaskStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
          
        }

        private void barButtonItem_AddPoints_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Program.Level < 30)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("您所使用的版本不支持【批量添加站点】功能，请联系客服升级后使用哦！", "升级版本后才能使用哦！", MessageBoxButtons.OKCancel);
                return;
            }

            V3Form.frmPoints_Add p = new V3Form.frmPoints_Add();
            p.ShowDialog();
            LoadPointList();

        }

        private void barButtonItem_AddTasks_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Program.Level < 30)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("您所使用的版本不支持【批量添加任务】功能，请联系客服升级后使用哦！", "升级版本后才能使用哦！", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }
            if (Model.V3Infos.MainDb.DefaultTasks.Count==0)
            { DevExpress.XtraEditors.XtraMessageBox.Show("批量添加任务需要使用任务模板，当前系统没有任务模板！请编辑任一任务，并设为任务模板！", "没有模板", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            else
            {
                V3Form.frmDefaultTasks f = new V3Form.frmDefaultTasks();
                f.ShowDialog();
                if (f.IsOK)
                {
                    V3Form.frmTasks_Add p = new V3Form.frmTasks_Add();
                    p.model = f.DefaultTask;
                    p.ShowDialog();
                }
                
            }
        }

        private void barButtonItem_ModifyTasks_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Program.Level < 30)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("您所使用的版本不支持【批量修改任务参数】功能，请联系客服升级后使用哦！", "升级版本后才能使用哦！", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }
            if (Program.f_frmTasks_Modify.IsDisposed )
                Program.f_frmTasks_Modify = new V3Form.frmTasks_Modify();
            Program.f_frmTasks_Modify.ShowDialog();
        }

        private void barButtonItem_ToSuperx_ItemClick(object sender, ItemClickEventArgs e)
        {
           DevExpress.XtraEditors.XtraMessageBox.Show("请到文章库管理，将文章导出为txt格式即可！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem_DataBase_Article_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (articleDbfrm.IsDisposed)
                articleDbfrm = new V3Form.DbManage();
            articleDbfrm.dbtype = 0;
            articleDbfrm.Text = "文章库管理";
            articleDbfrm.ShowDialog();
            
        }

        private void barButtonItem_DataBase_Keyword_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (keywordDbfrm.IsDisposed)
                keywordDbfrm = new V3Form.DbManage();
            keywordDbfrm.dbtype = 1;
            keywordDbfrm.Text = "关键词库管理";
            keywordDbfrm.ShowDialog();
        }

        private void barButtonItem_DataBase_Hash_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (hashDbfrm.IsDisposed)
                hashDbfrm = new V3Form.DbManage();
            hashDbfrm.dbtype = 2;
            hashDbfrm.Text = "哈希库库管理";
            hashDbfrm.ShowDialog();
        }

        private void barButtonItem_DataBase_Replease_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (replaceDbfrm.IsDisposed)
                replaceDbfrm = new V3Form.DbManage();
            replaceDbfrm.dbtype = 3;
            replaceDbfrm.Text = "替换库管理";
            replaceDbfrm.ShowDialog();
        }

        private void barButtonItem_DataBase_Link_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (linkDbfrm.IsDisposed)
                linkDbfrm = new V3Form.DbManage();
            linkDbfrm.dbtype = 4;
            linkDbfrm.Text = "链接库管理";
            linkDbfrm.ShowDialog();

        }

        private void barButtonItem_DataBase_DangerWord_ItemClick(object sender, ItemClickEventArgs e)
        {
            V3Form.frm_参数输入编辑器 view = new V3Form.frm_参数输入编辑器();
            view.Text = "请输入需要处理的敏感字符";
            view.txttitle.Caption = "一行一个";

            foreach (string m in Model.V3Infos.MainDb.Minganwords)
            {
                Common.V3Helper.GetInnerTextBox(view.txtMainbox).AppendText(m + "\r\n");
            } view.txtStatus.Caption = "总共有敏感字符" + Model.V3Infos.MainDb.Minganwords.Count.ToString() + "个";
            view.ShowDialog();

            if (view.issave)
            {

                string[] temp = view.txtMainbox.Lines;
                Model.V3Infos.MainDb.Minganwords.Clear();
                for (int i = 0; i < temp.Length; i++)
                {
                    string s = temp[i].Trim();

                    if (s.Length > 0 && !Model.V3Infos.MainDb.Minganwords.Contains(s))
                    {


                        Model.V3Infos.MainDb.Minganwords.Add(s);
                    }


                }
                V3Helper.saveDBS();
            }
        }

        private void barButtonItem_DataBase_Article_Word_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Program.Level < 120)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show( "您所使用的版本不支持该功能，请联系客服升级后使用哦！", "升级版本后才能使用哦！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            V3Form.frmSetIndex se = new V3Form.frmSetIndex();
            se.Show();
        }

        private void barButtonItem_Task_System_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (settingfrm.IsDisposed)
                settingfrm = new V3Form.frmSystem();
            settingfrm.ShowDialog();
            settingfrm.Activate();
        }

        private void barButtonItem_Custom_1_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        private void barButtonItem_video_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void barButtonItem_bbs_ItemClick(object sender, ItemClickEventArgs e)
        {
           
        }

        private void barButtonItem_OnlineHelp_ItemClick(object sender, ItemClickEventArgs e)
        {
           
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DevExpress.XtraEditors.XtraMessageBox.Show("您确认要关闭V3吗？\r\n\r\n当前系统正在运行的任务数：" + Model.V3Infos.TaskThread.Count, "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                if (V3Helper.isUseSaveTaskDb || V3Helper.isUseSaveReplaceDb || V3Helper.isUseSavePointDb || V3Helper.isUseSaveLinkDb || V3Helper.isUseSaveKeywordDb || V3Helper.isUseSaveHashDb || V3Helper.isUseSaveDbs || V3Helper.isUseSaveArticleDb)
                {

                    if (DevExpress.XtraEditors.XtraMessageBox.Show("数据后台保存警告！\r\n\r\n您的数据量可能很大，系统正在后台保存您刚才的操作，强制关闭可能会导致数据保存失败！", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        isclose = true;
                        this.Hide();
                        Log.LogNewline("[c11][系统]正在保存数据，请稍候....[/c]");
                        StopThread();
                        V3Helper.saveArticleDb();
                        V3Helper.saveHashDb();
                        V3Helper.saveKeywordDb();
                        V3Helper.saveReplaceDb();
                        V3Helper.saveTaskDb();
                        V3Helper.saveSendPointDb();
                        V3Helper.saveLinkDb();
                        SaveLinkDb();
                        V3.Common.V3Helper.saveDBS();
                        notifyIcon.Dispose();
                    }

                }
                else
                {
                    isclose = true;
                    this.Hide();
                    Log.LogNewline("[c11][系统]正在保存数据，请稍候....[/c]");
                    StopThread();
                    V3Helper.saveArticleDb();
                    V3Helper.saveHashDb();
                    V3Helper.saveKeywordDb();
                    V3Helper.saveReplaceDb();
                    V3Helper.saveTaskDb();
                    V3Helper.saveSendPointDb();
                    V3Helper.saveLinkDb();
                    SaveLinkDb();
                    V3.Common.V3Helper.saveDBS();
                    notifyIcon.Dispose();

                }

            }
            else
                e.Cancel = true;

        }

     
        private void gridControl_main_view_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs  e)
        {
            int hand = e.RowHandle;
            if (hand < 0) return;
            Record r = (Record)this.gridControl_main_view.GetRow(hand);
            if (r!=null)
            {
                if (r.任务状态编号 == 0)
                {
                    e.Appearance.ForeColor = Color.LimeGreen;
                }
                else if (r.任务状态编号 == 1)
                {
                    e.Appearance.ForeColor = Color.Green;
                }
                else if (r.任务状态编号 == 2)
                {
                    e.Appearance.ForeColor = Color.Green;

                }
                else if (r.任务状态编号 == 3)
                {
                    e.Appearance.ForeColor = Color.DarkOrange;
                }
                else
                {
                    e.Appearance.ForeColor = Color.Red;
                }
            }
            

        }

        private void barButton_Shop_ItemClick(object sender, ItemClickEventArgs e)
        {
            V3Form.frmModelShop frmModelShop = new V3Form.frmModelShop();
            frmModelShop.ShowDialog();
        }

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            LoadTaskAry();
        }

        private void treeList_Main_MouseClick(object sender, MouseEventArgs e)
        {
            if (treeList_Main.CalcHitInfo(e.Location).Node != null)
            {
                treeList_Main.CalcHitInfo(e.Location).Node.Selected = true;
            }
            else { return; }
            if (treeList_Main.FocusedNode != null&&e.Button==MouseButtons.Right)
            {

                if (treeList_Main.FocusedNode.Tag.ToString().Contains("point"))
                {
                    barButtonItem_StartAll.Enabled = true;
                    barButtonItem_StopAll.Enabled = true;
                    barButtonItem_Task_Add.Enabled = true;
                    barButtonItem_Point_Add.Enabled = false;
                    barButtonItem_Point_Modify.Enabled = true;
                    barButtonItem_Point_Delete.Enabled = true;
                    barButtonItem_Group_Add.Enabled = false;
                    barButtonItem_Group_Rename.Enabled = false;
                    barButtonItem_Group_Delete.Enabled = false;
                }
                else if (treeList_Main.FocusedNode.Tag.ToString().ToLower().Contains("group"))
                {


                    barButtonItem_StartAll.Enabled = true;
                    barButtonItem_StopAll.Enabled = true;
                    barButtonItem_Task_Add.Enabled = false;
                    barButtonItem_Point_Add.Enabled = true;
                    barButtonItem_Point_Modify.Enabled = false;
                    barButtonItem_Point_Delete.Enabled = false;
                    barButtonItem_Group_Add.Enabled = true;
                    barButtonItem_Group_Rename.Enabled = true;
                    barButtonItem_Group_Delete.Enabled = true;
                }
                else if (treeList_Main.FocusedNode.Tag.ToString().ToLower().Contains("task"))
                {

                    barButtonItem_StartAll.Enabled = true;
                    barButtonItem_StopAll.Enabled = true;
                    barButtonItem_Task_Add.Enabled = false;
                    barButtonItem_Point_Add.Enabled = true;
                    barButtonItem_Point_Modify.Enabled = false;
                    barButtonItem_Point_Delete.Enabled = false;
                    barButtonItem_Group_Add.Enabled = false;
                    barButtonItem_Group_Rename.Enabled = false;
                    barButtonItem_Group_Delete.Enabled = false;
                }
                else if (treeList_Main.FocusedNode.Tag.ToString().ToLower().Contains("mgr"))
                {
                    barButtonItem_StartAll.Enabled = false;
                    barButtonItem_StopAll.Enabled = false;
                    barButtonItem_Task_Add.Enabled = false;
                    barButtonItem_Point_Add.Enabled = false;
                    barButtonItem_Point_Modify.Enabled = false;
                    barButtonItem_Point_Delete.Enabled = false;
                    barButtonItem_Group_Add.Enabled = true;
                    barButtonItem_Group_Rename.Enabled = false;
                    barButtonItem_Group_Delete.Enabled = false;

                }
                else
                {
                    barButtonItem_StartAll.Enabled = false;
                    barButtonItem_StopAll.Enabled = false;
                    barButtonItem_Task_Add.Enabled = false;
                    barButtonItem_Point_Add.Enabled = false;
                    barButtonItem_Point_Modify.Enabled = false;
                    barButtonItem_Point_Delete.Enabled = false;
                    barButtonItem_Group_Add.Enabled = false;
                    barButtonItem_Group_Rename.Enabled = false;
                    barButtonItem_Group_Delete.Enabled = false;
                }

                popupMenu_Point.ShowPopup(Control.MousePosition);

            }
        }

        private void barButtonItem_StartAll_ItemClick(object sender, ItemClickEventArgs e)
        {

            Random r = new Random();
            if (Tasks.Count > 0)
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show("您确认要运行选中的这" + Tasks.Count + "个任务吗？", "确认运行", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                  
                    int count = 0;
                    for (int i = 0; i < Tasks.Count; i++)
                    {
                        if (!Program.TaskRunList.Contains(Convert.ToInt32(Tasks[i])))
                        {
                            count++;
                            Program.TaskRunList.Add(Convert.ToInt32(Tasks[i]));
                        }
                    }
                    if (count > 0)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("恭喜，已经成功将" + count + "个任务添加到启动队列中！", "加入启动队列成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("抱歉，没有将任何任务加入启动队列，可能原因如下：\r\n\r\n1，任务已经在队列中\r\n2，任务参数设置有误", "加入启动队列失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                 
                }
            }
            else
                DevExpress.XtraEditors.XtraMessageBox.Show("当前类目下没有任务，无法启动！", "没有任务", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem_StopAll_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Tasks.Count > 0)
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show("您确认要停止选中的这" + Tasks.Count + "个任务吗？", "确认停止", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < Tasks.Count; i++)
                    {
                        try
                        {
                            V3.Common.TaskRun.Stop(Convert.ToInt32(Tasks[i]));
                        }
                        catch { }
                    }
                }
            }
            else
                DevExpress.XtraEditors.XtraMessageBox.Show("当前项木有要停止的任务！", "木有任务", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem_Task_Add_ItemClick(object sender, ItemClickEventArgs e)
        {
            V3Form.frmTask frm = new V3Form.frmTask();
            frm.model.PointId = Convert.ToInt32(treeList_Main.FocusedNode.Tag.ToString().Split('-')[1]);
            frm.model.ArticleDbId = Model.V3Infos.SendPointDb[frm.model.PointId].ArticleDbID;
            frm.model.KeywordDbId = Model.V3Infos.SendPointDb[frm.model.PointId].KeywordDbID;
            frm.model.HashDbId = Model.V3Infos.SendPointDb[frm.model.PointId].HashDbID;
            frm.model.ReplaceDbId = Model.V3Infos.SendPointDb[frm.model.PointId].ReplaceDbid;
            frm.model.LinkDbId = Model.V3Infos.SendPointDb[frm.model.PointId].LinkDbID;
            frm.model.GetModel = Model.V3Infos.SendPointDb[frm.model.PointId].GetModel;
            frm.ShowDialog();
            if (frm.issave)
            {
                V3.Bll.TaskBll.Add(frm.model);
                LoadTaskAry();
                LoadTaskList();
                UpdatePoint();
            }
        }

        private void barButtonItem_Point_Add_ItemClick(object sender, ItemClickEventArgs e)
        {
            V3Form.frmPoint frm = new V3Form.frmPoint();
            frm.Model.GroupTag = treeList_Main.FocusedNode.Tag.ToString();
            frm.ShowDialog();
            if (frm.IsSave)
            {
                int id = V3.Bll.PointBll.Add(frm.Model);
                if (id != 0)
                {
                    V3.Common.Log.LogNewline("[c12]成功增加了一个新发布点！[/c]");
                   
                    treeList_Main.AppendNode(new object[] { "[" + id + "]" + frm.Model.name }, treeList_Main.FocusedNode, "point-" + id);
                    treeList_Main.FocusedNode.LastNode.SelectImageIndex = 10;
                    treeList_Main.FocusedNode.LastNode.StateImageIndex = 10;
                    treeList_Main.FocusedNode.LastNode.ImageIndex = 10;
                }
                else
                    V3.Common.Log.LogNewline("[c14]增加发布点失败！[/c]");
            }
            UpdatePoint();
        }

        private void barButtonItem_Point_Modify_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Program.f_frmPoint.IsDisposed )
                Program.f_frmPoint = new V3Form.frmPoint();
            Program.f_frmPoint.Model = Model.V3Infos.SendPointDb[Convert.ToInt32(treeList_Main.FocusedNode.Tag.ToString().Split('-')[1])];
            string Group = Program.f_frmPoint.Model.GroupTag;
            Program.f_frmPoint.Group = treeList_Main.FocusedNode.Tag.ToString();
            Program.f_frmPoint.ShowDialog();
            if (Program.f_frmPoint.IsSave)
            {
                if (V3.Bll.PointBll.Update(Program.f_frmPoint.Model))
                {
                    Model.V3Infos.SendPointDb[Program.f_frmPoint.Model.id] = Program.f_frmPoint.Model;
                   treeList_Main.FocusedNode.SetValue( 0,"[" + Program.f_frmPoint.Model.id + "]" + Program.f_frmPoint.Model.name);
                   if (Group != Program.f_frmPoint.Model.GroupTag)
                    {
                       LoadPointList();
                    }
                    UpdatePoint();
                }
            }
        }

        private void barButtonItem_Point_Delete_ItemClick(object sender, ItemClickEventArgs e)
        {
            //遍历所有，判断有没有正在运行的任务
            var ks=Model.V3Infos.TaskDb.Keys.ToList();
            foreach (var k in ks)
            {
                if (Model.V3Infos.TaskDb.ContainsKey(k))
                {
                    if (Model.V3Infos.TaskDb[k].PointId == Convert.ToInt32(treeList_Main.FocusedNode.Tag.ToString().Split('-')[1]) && Model.V3Infos.TaskThread.ContainsKey(k))
                    {
                        if (Model.V3Infos.TaskThread[k].Status == TaskStatus.Running)
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show("请停止所有任务后再删除该发布点！");
                            return;
                        }
                    }
                }
               
            }



            if (DevExpress.XtraEditors.XtraMessageBox.Show("您真的要删除这个发布点吗？", "是否确定删除？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (V3.Bll.PointBll.Delete(Convert.ToInt32(treeList_Main.FocusedNode.Tag.ToString().Split('-')[1])))
                {

                    int pointid = Convert.ToInt32(treeList_Main.FocusedNode.Tag.ToString().Split('-')[1]);
                    Model.V3Infos.SendPointDb.Remove(pointid);
                    int[] dataArray = new int[Model.V3Infos.TaskDb.Keys.Count];
                    Model.V3Infos.TaskDb.Keys.CopyTo(dataArray, 0);
                    for (int i = 0; i < dataArray.Length; i++)
                    {
                        if (Model.V3Infos.TaskDb[dataArray[i]].PointId == pointid)
                            V3.Bll.TaskBll.Delete(dataArray[i]);
                    }
                    treeList_Main.DeleteNode(treeList_Main.FocusedNode);
                   
                }
                UpdatePoint();
            }
        }

        private void barButtonItem_Group_Add_ItemClick(object sender, ItemClickEventArgs e)
        {

            V3.V3Form.frmGroup_Rename view = new V3.V3Form.frmGroup_Rename();
            view.Text = "新增一个分组";
            view.ShowDialog();
            if (view.issave)
            {
                string tag = "groups" + new Random().Next(10000000, 99999999).ToString();
                treeList_Main.AppendNode(new object[] { view.name.Text }, treeList_Main.Nodes[1], tag);
                treeList_Main.Nodes[1].LastNode.SelectImageIndex = 8;
                treeList_Main.Nodes[1].LastNode.StateImageIndex = 8;
                treeList_Main.Nodes[1].LastNode.ImageIndex = 8;
                Model.V3Infos.MainDb.GroupList.Add(tag, view.name.Text);
                V3.Common.Log.LogNewline("[c12][系统]恭喜，已成功添加了一个新的分组" + view.name.Text + "！[/c]");
            }
            UpdatePoint();
        }

        private void barButtonItem_Group_Rename_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (treeList_Main.FocusedNode.Tag.ToString().Contains("groups"))
            {
                V3.V3Form.frmGroup_Rename view = new V3.V3Form.frmGroup_Rename();
                view.Text = "重命名分组";
                view.name.Text = V3Infos.MainDb.GroupList[treeList_Main.FocusedNode.Tag.ToString()];
                view.ShowDialog();
                if (view.issave)
                {
                    treeList_Main.FocusedNode.SetValue( 0,view.name.Text);
                    Model.V3Infos.MainDb.GroupList[treeList_Main.FocusedNode.Tag.ToString()] = view.name.Text;
                    V3Helper.saveDBS();
                    UpdatePoint();
                }
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("默认分组不允许重命名，请选择一个自定义分组！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void barButtonItem_Group_Delete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (treeList_Main.FocusedNode.Tag.ToString().Contains("groups"))
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show("确定要删除这个分组吗？\r\n\r\n注意：删除后该分组下的任务和发布点将归到默认分组下！", "确定删除？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Model.V3Infos.MainDb.GroupList.Remove(treeList_Main.FocusedNode.Tag.ToString());
                    treeList_Main.DeleteNode(treeList_Main.FocusedNode);
                    V3Helper.saveDBS();
                    LoadPointList();
                  
                }
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("请选择要删除的自定义分组！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void barButtonItem_Task_Start_ItemClick(object sender, ItemClickEventArgs e)
        {
            Random r = new Random();
            if (gridControl_main_view.SelectedRowsCount > 0)
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show("您确认要运行选中的这" + gridControl_main_view.SelectedRowsCount + "个任务吗？", "确认运行", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                   
                    int count = 0;
                    for (int i = 0; i < gridControl_main_view.SelectedRowsCount; i++)
                    {
                        int task_id =((Record) gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[i])).任务ID;
                        if (!Program.TaskRunList.Contains(task_id)&&!Model.V3Infos.TaskWaiting.ContainsKey(task_id))
                        {
                            count++;
                            Program.TaskRunList.Add(task_id);
                        }
                    }
                    if (count > 0)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("恭喜，已经成功将" + count + "个任务添加到启动队列中！", "加入启动队列成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("抱歉，没有将任何任务加入启动队列，可能原因如下：\r\n\r\n1.任务已经在运行或休眠中\r\n2.任务参数设置有误", "加入启动队列失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                   
                }

            }
            else
                DevExpress.XtraEditors.XtraMessageBox.Show("请先至少一个您要启动的任务！", "木有选择任务", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem_Task_Stop_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridControl_main_view.SelectedRowsCount > 0)
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show("您确认要停止选中的这" + gridControl_main_view.SelectedRowsCount + "个任务吗？", "确认停止", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < gridControl_main_view.SelectedRowsCount; i++)
                    {
                        V3.Common.TaskRun.Stop(((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[i])).任务ID);
                    }
                }
            }
            else
                DevExpress.XtraEditors.XtraMessageBox.Show("请先至少一个您要停止的任务！", "木有选择任务", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem_Task_Modify_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridControl_main_view.SelectedRowsCount > 0)
            {
                if (Program.f_frmTask.IsDisposed)
                    Program.f_frmTask = new V3Form.frmTask();
                Program.f_frmTask.model = xEngine.Common.XSerializable.CloneObject<Model.Task>(Model.V3Infos.TaskDb[((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0])).任务ID]);
                Program.f_frmTask.ShowDialog();
                //关闭提示
                if (Program.f_frmTask.issave)
                {

                    Model.Task model=xEngine.Common.XSerializable.CloneObject<Model.Task>(Program.f_frmTask.model);
                    Model.V3Infos.TaskDb[((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[0])).任务ID] = model;
                   
                    V3.Bll.TaskBll.SaveTask(Program.f_frmTask.model.id);
                    
                }
            }
            else
                DevExpress.XtraEditors.XtraMessageBox.Show("请先选择一个您要编辑的任务！", "木有选择任务", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void barButtonItem_Task_Delete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridControl_main_view.SelectedRowsCount > 0)
            {

                if (DevExpress.XtraEditors.XtraMessageBox.Show("您确认要删除选中的这" + gridControl_main_view.SelectedRowsCount + "个任务吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < gridControl_main_view.SelectedRowsCount; i++)
                    {
                        int taskid = ((Record)gridControl_main_view.GetRow(gridControl_main_view.GetSelectedRows()[i])).任务ID;
                        if (Model.V3Infos.TaskThread.ContainsKey(taskid))
                        {
                            if (Model.V3Infos.TaskThread[taskid].Status == TaskStatus.Running)
                            {
                                DevExpress.XtraEditors.XtraMessageBox.Show("请停止该任务后再删除！");
                                return;
                            }
                        }
                        V3.Common.TaskRun.Stop(taskid);
                        V3.Bll.TaskBll.Delete(taskid);
                    }
                    LoadTaskAry();
                    LoadTaskList();
                    UpdatePoint();
                }
            }
            else
                DevExpress.XtraEditors.XtraMessageBox.Show("请先至少一个您要删除的任务！", "木有选择任务", MessageBoxButtons.OK, MessageBoxIcon.Information);
          
        }

        private void gridControl_main_DoubleClick(object sender, EventArgs e)
        {
            barButtonItem_Task_Modify_ItemClick(sender,null);
        }

        private void barButtonItem_getmodule_add_ItemClick(object sender, ItemClickEventArgs e)
        {
            V3Form.抓取模块.getPlan frm =new getPlan();
            frm.isMymodel = true;
            frm.ShowDialog();
            if (frm.issave)
            {
               
            }
        }

        private void barButtonItem_postmodule_add_ItemClick(object sender, ItemClickEventArgs e)
        {
            V3Form.发布模块.postPlan frm = new postPlan();
            frm.IsMymodel = true;
            frm.ShowDialog();
            if (frm.Issave)
            {
               

            }
        }

        private void barButtonItem_moduleshop_ItemClick(object sender, ItemClickEventArgs e)
        {
            V3Form.frmModelShop frmModelShop = new V3Form.frmModelShop();
            frmModelShop.ShowDialog();
        }

        private void rgbiSkins_GalleryItemClick(object sender, GalleryItemClickEventArgs e)
        {
            string SkinValue = e.Item.Caption;
            Model.V3Infos.MainDb.SkinName = SkinValue;
          
        }

        private void btn_logo_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.baidu.com");
            }
            catch
            {
                XtraMessageBox.Show("请在您的浏览器中打开网址“<color=red>http://www.baidu.com</color>”", DefaultBoolean.True);
            }
        }

        private void treeList_Main_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (treeList_Main.FocusedNode != null && e.Button == MouseButtons.Left)
            {

                if (treeList_Main.FocusedNode.Tag.ToString().Contains("point"))
                {
                    barButtonItem_Point_Modify_ItemClick(null,null);
                }
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState==FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon.ShowBalloonTip(5,"系统提示","站群V3已经最小化在这里运行！",ToolTipIcon.Info);
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void btn_ShowConsole_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Model.V3Infos.MainDb.ShowConsole == 0)
            {
                Model.V3Infos.MainDb.ShowConsole = 1;
                btn_ShowConsole.Glyph = Properties.Resources.btn_ShowConsole;
              
                V3.Common.Console.StartConsole("V3运行日志监控窗口");
            }
            else
            {
                Model.V3Infos.MainDb.ShowConsole = 0;
                btn_ShowConsole.Glyph = Properties.Resources.btn_CloseConsole;
              
                V3.Common.Console.StartConsole("V3运行日志监控窗口");
            }
        }

        private void barButtonItem_super_ItemClick(object sender, ItemClickEventArgs e)
        {
            V3Form.frmSuper2 frm = new V3Form.frmSuper2();
            frm.ShowDialog();
        }
    }
}