namespace V3.V3Form
{
    partial class DbManage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbManage));
            this.panelEx1 = new DevExpress.XtraEditors.PanelControl();
            this.ipanel = new DevExpress.XtraEditors.PanelControl();
            this.myprocess = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.istate = new DevExpress.XtraEditors.LabelControl();
            this.gridControl_main = new DevExpress.XtraGrid.GridControl();
            this.gridControl_main_view = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.labDbStatus = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem_add = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_rename = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_modifygroup = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_delete = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_view = new DevExpress.XtraBars.BarButtonItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.text_page = new DevExpress.XtraEditors.TextEdit();
            this.btn_next = new DevExpress.XtraEditors.SimpleButton();
            this.btn_pre = new DevExpress.XtraEditors.SimpleButton();
            this.btn_view = new DevExpress.XtraEditors.SimpleButton();
            this.btn_delete = new DevExpress.XtraEditors.SimpleButton();
            this.btn_add = new DevExpress.XtraEditors.SimpleButton();
            this.text_search = new DevExpress.XtraEditors.TextEdit();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.comboBox_group = new DevExpress.XtraEditors.LookUpEdit();
            this.popupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).BeginInit();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ipanel)).BeginInit();
            this.ipanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myprocess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main_view)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.text_page.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.text_search.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBox_group.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEx1.Controls.Add(this.ipanel);
            this.panelEx1.Controls.Add(this.gridControl_main);
            this.panelEx1.Location = new System.Drawing.Point(0, 46);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(1133, 611);
            this.panelEx1.TabIndex = 0;
            // 
            // ipanel
            // 
            this.ipanel.Appearance.BackColor = System.Drawing.Color.White;
            this.ipanel.Appearance.Options.UseBackColor = true;
            this.ipanel.Appearance.Options.UseBorderColor = true;
            this.ipanel.Controls.Add(this.myprocess);
            this.ipanel.Controls.Add(this.istate);
            this.ipanel.Location = new System.Drawing.Point(283, 248);
            this.ipanel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.ipanel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.ipanel.Name = "ipanel";
            this.ipanel.Size = new System.Drawing.Size(520, 80);
            this.ipanel.TabIndex = 70;
            this.ipanel.Visible = false;
            // 
            // myprocess
            // 
            this.myprocess.EditValue = 0;
            this.myprocess.Location = new System.Drawing.Point(22, 48);
            this.myprocess.Name = "myprocess";
            this.myprocess.Size = new System.Drawing.Size(479, 11);
            this.myprocess.TabIndex = 4;
            // 
            // istate
            // 
            this.istate.Location = new System.Drawing.Point(22, 21);
            this.istate.Name = "istate";
            this.istate.Size = new System.Drawing.Size(38, 14);
            this.istate.TabIndex = 1;
            this.istate.Text = "labelX2";
            // 
            // gridControl_main
            // 
            this.gridControl_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl_main.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_main.Location = new System.Drawing.Point(0, 2);
            this.gridControl_main.MainView = this.gridControl_main_view;
            this.gridControl_main.MenuManager = this.barManager;
            this.gridControl_main.Name = "gridControl_main";
            this.gridControl_main.Size = new System.Drawing.Size(1133, 608);
            this.gridControl_main.TabIndex = 71;
            this.gridControl_main.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridControl_main_view});
            this.gridControl_main.DoubleClick += new System.EventHandler(this.gridControl_main_DoubleClick);
            this.gridControl_main.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.gridControl_main_MouseDoubleClick);
            this.gridControl_main.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridControl_main_MouseDown);
            this.gridControl_main.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridControl_main_MouseMove);
            this.gridControl_main.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridControl_main_MouseUp);
            // 
            // gridControl_main_view
            // 
            this.gridControl_main_view.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gridControl_main_view.GridControl = this.gridControl_main;
            this.gridControl_main_view.Name = "gridControl_main_view";
            this.gridControl_main_view.OptionsBehavior.Editable = false;
            this.gridControl_main_view.OptionsSelection.MultiSelect = true;
            this.gridControl_main_view.OptionsView.ShowGroupPanel = false;
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem_add,
            this.barButtonItem_rename,
            this.barButtonItem_modifygroup,
            this.barButtonItem_delete,
            this.barButtonItem_view,
            this.labDbStatus});
            this.barManager.MaxItemId = 7;
            this.barManager.StatusBar = this.bar1;
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 2";
            this.bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.labDbStatus)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Custom 2";
            // 
            // labDbStatus
            // 
            this.labDbStatus.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.labDbStatus.Caption = "加载中...";
            this.labDbStatus.Id = 6;
            this.labDbStatus.Name = "labDbStatus";
            this.labDbStatus.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1133, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 657);
            this.barDockControlBottom.Size = new System.Drawing.Size(1133, 26);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 657);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1133, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 657);
            // 
            // barButtonItem_add
            // 
            this.barButtonItem_add.Caption = "新增一个库";
            this.barButtonItem_add.Glyph = global::V3.Properties.Resources.db_add;
            this.barButtonItem_add.Id = 1;
            this.barButtonItem_add.Name = "barButtonItem_add";
            this.barButtonItem_add.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_add_ItemClick);
            // 
            // barButtonItem_rename
            // 
            this.barButtonItem_rename.Caption = "修改这个库的名称";
            this.barButtonItem_rename.Glyph = global::V3.Properties.Resources.btn_modify;
            this.barButtonItem_rename.Id = 2;
            this.barButtonItem_rename.Name = "barButtonItem_rename";
            this.barButtonItem_rename.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_rename_ItemClick);
            // 
            // barButtonItem_modifygroup
            // 
            this.barButtonItem_modifygroup.Caption = "修改这个库的分组";
            this.barButtonItem_modifygroup.Glyph = global::V3.Properties.Resources.group;
            this.barButtonItem_modifygroup.Id = 3;
            this.barButtonItem_modifygroup.Name = "barButtonItem_modifygroup";
            this.barButtonItem_modifygroup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_modifygroup_ItemClick);
            // 
            // barButtonItem_delete
            // 
            this.barButtonItem_delete.Caption = "删除这个库";
            this.barButtonItem_delete.Glyph = global::V3.Properties.Resources.db_delete;
            this.barButtonItem_delete.Id = 4;
            this.barButtonItem_delete.Name = "barButtonItem_delete";
            this.barButtonItem_delete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_delete_ItemClick);
            // 
            // barButtonItem_view
            // 
            this.barButtonItem_view.Caption = "查看这个库信息";
            this.barButtonItem_view.Glyph = global::V3.Properties.Resources.db_view;
            this.barButtonItem_view.Id = 5;
            this.barButtonItem_view.Name = "barButtonItem_view";
            this.barButtonItem_view.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_view_ItemClick);
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.text_page);
            this.panelControl1.Controls.Add(this.btn_next);
            this.panelControl1.Controls.Add(this.btn_pre);
            this.panelControl1.Controls.Add(this.btn_view);
            this.panelControl1.Controls.Add(this.btn_delete);
            this.panelControl1.Controls.Add(this.btn_add);
            this.panelControl1.Controls.Add(this.text_search);
            this.panelControl1.Controls.Add(this.btn_search);
            this.panelControl1.Controls.Add(this.comboBox_group);
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1133, 40);
            this.panelControl1.TabIndex = 45;
            // 
            // text_page
            // 
            this.text_page.EditValue = "1/1";
            this.text_page.Location = new System.Drawing.Point(944, 9);
            this.text_page.Name = "text_page";
            this.text_page.Properties.Appearance.Options.UseTextOptions = true;
            this.text_page.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.text_page.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.text_page.Size = new System.Drawing.Size(87, 20);
            this.text_page.TabIndex = 46;
            // 
            // btn_next
            // 
            this.btn_next.Image = global::V3.Properties.Resources.page_next;
            this.btn_next.Location = new System.Drawing.Point(1037, 8);
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(35, 23);
            this.btn_next.TabIndex = 45;
            this.btn_next.Click += new System.EventHandler(this.btn_next_Click);
            // 
            // btn_pre
            // 
            this.btn_pre.Image = global::V3.Properties.Resources.page_pre;
            this.btn_pre.Location = new System.Drawing.Point(902, 8);
            this.btn_pre.Name = "btn_pre";
            this.btn_pre.Size = new System.Drawing.Size(35, 23);
            this.btn_pre.TabIndex = 44;
            this.btn_pre.Click += new System.EventHandler(this.btn_pre_Click);
            // 
            // btn_view
            // 
            this.btn_view.Image = global::V3.Properties.Resources.db_view;
            this.btn_view.Location = new System.Drawing.Point(771, 8);
            this.btn_view.Name = "btn_view";
            this.btn_view.Size = new System.Drawing.Size(87, 23);
            this.btn_view.TabIndex = 43;
            this.btn_view.Text = "查看";
            this.btn_view.Click += new System.EventHandler(this.btn_view_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Image = global::V3.Properties.Resources.db_delete;
            this.btn_delete.Location = new System.Drawing.Point(677, 8);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(87, 23);
            this.btn_delete.TabIndex = 42;
            this.btn_delete.Text = "删除";
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_add
            // 
            this.btn_add.Image = global::V3.Properties.Resources.db_add;
            this.btn_add.Location = new System.Drawing.Point(582, 8);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(87, 23);
            this.btn_add.TabIndex = 41;
            this.btn_add.Text = "新增";
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // text_search
            // 
            this.text_search.Location = new System.Drawing.Point(6, 9);
            this.text_search.Name = "text_search";
            this.text_search.Properties.AccessibleDescription = "";
            this.text_search.Properties.NullText = "可以输入关键字进行筛选...";
            this.text_search.Size = new System.Drawing.Size(248, 20);
            this.text_search.TabIndex = 39;
            // 
            // btn_search
            // 
            this.btn_search.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_search.Image = global::V3.Properties.Resources.refresh;
            this.btn_search.Location = new System.Drawing.Point(471, 8);
            this.btn_search.Name = "btn_search";
            this.btn_search.ShowToolTips = false;
            this.btn_search.Size = new System.Drawing.Size(104, 23);
            this.btn_search.TabIndex = 38;
            this.btn_search.Text = "刷新/筛选";
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // comboBox_group
            // 
            this.comboBox_group.Location = new System.Drawing.Point(261, 9);
            this.comboBox_group.Name = "comboBox_group";
            this.comboBox_group.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down)});
            this.comboBox_group.Properties.NullText = "";
            this.comboBox_group.Properties.ShowFooter = false;
            this.comboBox_group.Properties.ShowHeader = false;
            this.comboBox_group.Properties.ShowLines = false;
            this.comboBox_group.Size = new System.Drawing.Size(202, 20);
            this.comboBox_group.TabIndex = 40;
            this.comboBox_group.EditValueChanged += new System.EventHandler(this.comboBox_group_EditValueChanged);
            // 
            // popupMenu
            // 
            this.popupMenu.AllowRibbonQATMenu = false;
            this.popupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem_view),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem_add),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem_rename, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem_modifygroup),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem_delete, true)});
            this.popupMenu.Manager = this.barManager;
            this.popupMenu.Name = "popupMenu";
            this.popupMenu.ShowCaption = true;
            // 
            // DbManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1133, 683);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(858, 513);
            this.Name = "DbManage";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "数据库管理器";
            this.Load += new System.EventHandler(this.DbManage_Load);
            this.Shown += new System.EventHandler(this.DbManage_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).EndInit();
            this.panelEx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ipanel)).EndInit();
            this.ipanel.ResumeLayout(false);
            this.ipanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myprocess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main_view)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.text_page.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.text_search.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBox_group.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelEx1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btn_view;
        private DevExpress.XtraEditors.SimpleButton btn_delete;
        private DevExpress.XtraEditors.SimpleButton btn_add;
        private DevExpress.XtraEditors.TextEdit text_search;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DevExpress.XtraEditors.TextEdit text_page;
        private DevExpress.XtraEditors.SimpleButton btn_next;
        private DevExpress.XtraEditors.SimpleButton btn_pre;
        private DevExpress.XtraEditors.PanelControl ipanel;
        private DevExpress.XtraEditors.MarqueeProgressBarControl myprocess;
        private DevExpress.XtraEditors.LabelControl istate;
        private DevExpress.XtraGrid.GridControl gridControl_main;
        private DevExpress.XtraGrid.Views.Grid.GridView gridControl_main_view;
        private DevExpress.XtraBars.PopupMenu popupMenu;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_add;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_rename;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_modifygroup;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_delete;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_view;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarStaticItem labDbStatus;
        private DevExpress.XtraEditors.LookUpEdit comboBox_group;
    }
}