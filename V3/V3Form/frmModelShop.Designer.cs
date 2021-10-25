namespace V3.V3Form
{
    partial class frmModelShop
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
            this.gridControl_main = new DevExpress.XtraGrid.GridControl();
            this.gridControl_main_view = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panelEx1 = new DevExpress.XtraEditors.PanelControl();
            this.ipanel = new DevExpress.XtraEditors.PanelControl();
            this.myprocess = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.istate = new DevExpress.XtraEditors.LabelControl();
            this.text_word = new DevExpress.XtraEditors.TextEdit();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar6 = new DevExpress.XtraBars.Bar();
            this.label_status = new DevExpress.XtraBars.BarStaticItem();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barButtonItem_getmodule_add = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_postmodule_add = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_refresh = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barToggleSwitchItem1 = new DevExpress.XtraBars.BarToggleSwitchItem();
            this.barLargeButtonItem1 = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemRadioGroup1 = new DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_delete = new DevExpress.XtraBars.BarButtonItem();
            this.checkEdit_module_type_post = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit_module_type_get = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit_module_type_all = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.checkEdit_module_type_get_tongbu = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit_module_type_get_spider = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit_module_type_get_custom = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit_module_type_get_keyword = new DevExpress.XtraEditors.CheckEdit();
            this.popupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main_view)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).BeginInit();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ipanel)).BeginInit();
            this.ipanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myprocess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.text_word.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRadioGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_module_type_post.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_module_type_get.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_module_type_all.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_module_type_get_tongbu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_module_type_get_spider.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_module_type_get_custom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_module_type_get_keyword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl_main
            // 
            this.gridControl_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl_main.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_main.Location = new System.Drawing.Point(12, 6);
            this.gridControl_main.MainView = this.gridControl_main_view;
            this.gridControl_main.Name = "gridControl_main";
            this.gridControl_main.Size = new System.Drawing.Size(984, 598);
            this.gridControl_main.TabIndex = 111;
            this.gridControl_main.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridControl_main_view});
            this.gridControl_main.DoubleClick += new System.EventHandler(this.gridControl_main_DoubleClick);
            this.gridControl_main.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridControl_main_MouseUp);
            // 
            // gridControl_main_view
            // 
            this.gridControl_main_view.Appearance.Row.Options.UseTextOptions = true;
            this.gridControl_main_view.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridControl_main_view.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.gridControl_main_view.GridControl = this.gridControl_main;
            this.gridControl_main_view.Name = "gridControl_main_view";
            this.gridControl_main_view.OptionsBehavior.Editable = false;
            this.gridControl_main_view.OptionsPrint.PrintHeader = false;
            this.gridControl_main_view.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridControl_main_view.OptionsSelection.MultiSelect = true;
            this.gridControl_main_view.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
            this.gridControl_main_view.RowHeight = 22;
            this.gridControl_main_view.EndSorting += new System.EventHandler(this.gridControl_main_view_EndSorting);
            // 
            // panelEx1
            // 
            this.panelEx1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelEx1.Controls.Add(this.ipanel);
            this.panelEx1.Controls.Add(this.text_word);
            this.panelEx1.Controls.Add(this.checkEdit_module_type_post);
            this.panelEx1.Controls.Add(this.checkEdit_module_type_get);
            this.panelEx1.Controls.Add(this.checkEdit_module_type_all);
            this.panelEx1.Controls.Add(this.labelControl1);
            this.panelEx1.Controls.Add(this.checkEdit_module_type_get_tongbu);
            this.panelEx1.Controls.Add(this.checkEdit_module_type_get_spider);
            this.panelEx1.Controls.Add(this.checkEdit_module_type_get_custom);
            this.panelEx1.Controls.Add(this.checkEdit_module_type_get_keyword);
            this.panelEx1.Controls.Add(this.gridControl_main);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 32);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(1006, 613);
            this.panelEx1.TabIndex = 51;
            // 
            // ipanel
            // 
            this.ipanel.Appearance.BackColor = System.Drawing.Color.White;
            this.ipanel.Appearance.Options.UseBackColor = true;
            this.ipanel.Appearance.Options.UseBorderColor = true;
            this.ipanel.Controls.Add(this.myprocess);
            this.ipanel.Controls.Add(this.istate);
            this.ipanel.Location = new System.Drawing.Point(243, 286);
            this.ipanel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.ipanel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.ipanel.Name = "ipanel";
            this.ipanel.Size = new System.Drawing.Size(520, 80);
            this.ipanel.TabIndex = 124;
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
            // text_word
            // 
            this.text_word.EditValue = "";
            this.text_word.Location = new System.Drawing.Point(331, 13);
            this.text_word.MenuManager = this.barManager1;
            this.text_word.Name = "text_word";
            this.text_word.Properties.NullText = "输入关键词，回车进行搜索";
            this.text_word.Properties.NullValuePrompt = "输入关键词，回车进行搜索";
            this.text_word.Size = new System.Drawing.Size(279, 20);
            this.text_word.TabIndex = 123;
            this.text_word.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_word_KeyUp);
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar6,
            this.bar2});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barToggleSwitchItem1,
            this.barLargeButtonItem1,
            this.barEditItem1,
            this.barButtonItem_getmodule_add,
            this.barButtonItem_postmodule_add,
            this.barButtonItem3,
            this.label_status,
            this.barButtonItem_refresh,
            this.barButtonItem_delete});
            this.barManager1.MaxItemId = 11;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemRadioGroup1});
            this.barManager1.StatusBar = this.bar6;
            // 
            // bar6
            // 
            this.bar6.BarName = "Status bar";
            this.bar6.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar6.DockCol = 0;
            this.bar6.DockRow = 0;
            this.bar6.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar6.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.label_status)});
            this.bar6.OptionsBar.AllowQuickCustomization = false;
            this.bar6.OptionsBar.DrawDragBorder = false;
            this.bar6.OptionsBar.UseWholeRow = true;
            this.bar6.Text = "Status bar";
            this.bar6.Visible = false;
            // 
            // label_status
            // 
            this.label_status.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.label_status.Caption = "模块加载中...";
            this.label_status.Id = 6;
            this.label_status.Name = "label_status";
            // 
            // bar2
            // 
            this.bar2.BarItemVertIndent = 5;
            this.bar2.BarName = "Custom 3";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem_getmodule_add),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem_postmodule_add),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem_refresh)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawBorder = false;
            this.bar2.Text = "Custom 3";
            // 
            // barButtonItem_getmodule_add
            // 
            this.barButtonItem_getmodule_add.Caption = "新建抓取模块";
            this.barButtonItem_getmodule_add.Id = 3;
            this.barButtonItem_getmodule_add.ImageOptions.Image = global::V3.Properties.Resources.btn_getmodule_add;
            this.barButtonItem_getmodule_add.Name = "barButtonItem_getmodule_add";
            this.barButtonItem_getmodule_add.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barButtonItem_getmodule_add.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_getmodule_add_ItemClick);
            // 
            // barButtonItem_postmodule_add
            // 
            this.barButtonItem_postmodule_add.Caption = "新建发布模块";
            this.barButtonItem_postmodule_add.Id = 4;
            this.barButtonItem_postmodule_add.ImageOptions.Image = global::V3.Properties.Resources.btn_postmodule_add;
            this.barButtonItem_postmodule_add.Name = "barButtonItem_postmodule_add";
            this.barButtonItem_postmodule_add.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barButtonItem_postmodule_add.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_postmodule_add_ItemClick);
            // 
            // barButtonItem_refresh
            // 
            this.barButtonItem_refresh.Caption = "刷新模块";
            this.barButtonItem_refresh.Id = 8;
            this.barButtonItem_refresh.ImageOptions.Image = global::V3.Properties.Resources.refresh;
            this.barButtonItem_refresh.Name = "barButtonItem_refresh";
            this.barButtonItem_refresh.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barButtonItem_refresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_refresh_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1006, 32);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 645);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1006, 25);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 32);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 613);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1006, 32);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 613);
            // 
            // barToggleSwitchItem1
            // 
            this.barToggleSwitchItem1.Caption = "barToggleSwitchItem1";
            this.barToggleSwitchItem1.Id = 0;
            this.barToggleSwitchItem1.Name = "barToggleSwitchItem1";
            // 
            // barLargeButtonItem1
            // 
            this.barLargeButtonItem1.Caption = "barLargeButtonItem1";
            this.barLargeButtonItem1.Id = 1;
            this.barLargeButtonItem1.Name = "barLargeButtonItem1";
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "barEditItem1";
            this.barEditItem1.Edit = this.repositoryItemRadioGroup1;
            this.barEditItem1.Id = 2;
            this.barEditItem1.Name = "barEditItem1";
            // 
            // repositoryItemRadioGroup1
            // 
            this.repositoryItemRadioGroup1.Name = "repositoryItemRadioGroup1";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "新建抓取模块";
            this.barButtonItem3.Id = 5;
            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // barButtonItem_delete
            // 
            this.barButtonItem_delete.Caption = "注销这个模块";
            this.barButtonItem_delete.Id = 9;
            this.barButtonItem_delete.Name = "barButtonItem_delete";
            this.barButtonItem_delete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_delete_ItemClick);
            // 
            // checkEdit_module_type_post
            // 
            this.checkEdit_module_type_post.Location = new System.Drawing.Point(204, 15);
            this.checkEdit_module_type_post.MenuManager = this.barManager1;
            this.checkEdit_module_type_post.Name = "checkEdit_module_type_post";
            this.checkEdit_module_type_post.Properties.Caption = "发布模块";
            this.checkEdit_module_type_post.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.checkEdit_module_type_post.Properties.RadioGroupIndex = 1;
            this.checkEdit_module_type_post.Size = new System.Drawing.Size(87, 19);
            this.checkEdit_module_type_post.TabIndex = 117;
            this.checkEdit_module_type_post.TabStop = false;
            this.checkEdit_module_type_post.CheckedChanged += new System.EventHandler(this.checkEdit_module_type_post_CheckedChanged);
            // 
            // checkEdit_module_type_get
            // 
            this.checkEdit_module_type_get.Location = new System.Drawing.Point(111, 15);
            this.checkEdit_module_type_get.MenuManager = this.barManager1;
            this.checkEdit_module_type_get.Name = "checkEdit_module_type_get";
            this.checkEdit_module_type_get.Properties.Caption = "抓取模块";
            this.checkEdit_module_type_get.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.checkEdit_module_type_get.Properties.RadioGroupIndex = 1;
            this.checkEdit_module_type_get.Size = new System.Drawing.Size(87, 19);
            this.checkEdit_module_type_get.TabIndex = 116;
            this.checkEdit_module_type_get.TabStop = false;
            this.checkEdit_module_type_get.CheckedChanged += new System.EventHandler(this.checkEdit_module_type_get_CheckedChanged);
            // 
            // checkEdit_module_type_all
            // 
            this.checkEdit_module_type_all.EditValue = true;
            this.checkEdit_module_type_all.Location = new System.Drawing.Point(18, 15);
            this.checkEdit_module_type_all.MenuManager = this.barManager1;
            this.checkEdit_module_type_all.Name = "checkEdit_module_type_all";
            this.checkEdit_module_type_all.Properties.Caption = "所有类型";
            this.checkEdit_module_type_all.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.checkEdit_module_type_all.Properties.RadioGroupIndex = 1;
            this.checkEdit_module_type_all.Size = new System.Drawing.Size(76, 19);
            this.checkEdit_module_type_all.TabIndex = 115;
            this.checkEdit_module_type_all.CheckedChanged += new System.EventHandler(this.checkEdit_module_type_all_CheckedChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(18, 13);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(291, 22);
            this.labelControl1.TabIndex = 122;
            this.labelControl1.Text = "a                             ";
            // 
            // checkEdit_module_type_get_tongbu
            // 
            this.checkEdit_module_type_get_tongbu.EditValue = true;
            this.checkEdit_module_type_get_tongbu.Location = new System.Drawing.Point(908, 13);
            this.checkEdit_module_type_get_tongbu.MenuManager = this.barManager1;
            this.checkEdit_module_type_get_tongbu.Name = "checkEdit_module_type_get_tongbu";
            this.checkEdit_module_type_get_tongbu.Properties.Appearance.Options.UseTextOptions = true;
            this.checkEdit_module_type_get_tongbu.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.checkEdit_module_type_get_tongbu.Properties.Caption = "同步追踪";
            this.checkEdit_module_type_get_tongbu.Size = new System.Drawing.Size(75, 19);
            this.checkEdit_module_type_get_tongbu.TabIndex = 121;
            this.checkEdit_module_type_get_tongbu.Visible = false;
            this.checkEdit_module_type_get_tongbu.CheckedChanged += new System.EventHandler(this.checkEdit_module_type_get_tongbu_CheckedChanged);
            // 
            // checkEdit_module_type_get_spider
            // 
            this.checkEdit_module_type_get_spider.EditValue = true;
            this.checkEdit_module_type_get_spider.Location = new System.Drawing.Point(827, 13);
            this.checkEdit_module_type_get_spider.MenuManager = this.barManager1;
            this.checkEdit_module_type_get_spider.Name = "checkEdit_module_type_get_spider";
            this.checkEdit_module_type_get_spider.Properties.Appearance.Options.UseTextOptions = true;
            this.checkEdit_module_type_get_spider.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.checkEdit_module_type_get_spider.Properties.Caption = "蜘蛛爬行";
            this.checkEdit_module_type_get_spider.Size = new System.Drawing.Size(75, 19);
            this.checkEdit_module_type_get_spider.TabIndex = 120;
            this.checkEdit_module_type_get_spider.Visible = false;
            this.checkEdit_module_type_get_spider.CheckedChanged += new System.EventHandler(this.checkEdit_module_type_get_spider_CheckedChanged);
            // 
            // checkEdit_module_type_get_custom
            // 
            this.checkEdit_module_type_get_custom.EditValue = true;
            this.checkEdit_module_type_get_custom.Location = new System.Drawing.Point(619, 13);
            this.checkEdit_module_type_get_custom.MenuManager = this.barManager1;
            this.checkEdit_module_type_get_custom.Name = "checkEdit_module_type_get_custom";
            this.checkEdit_module_type_get_custom.Properties.Appearance.Options.UseTextOptions = true;
            this.checkEdit_module_type_get_custom.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.checkEdit_module_type_get_custom.Properties.Caption = "自定义抓取";
            this.checkEdit_module_type_get_custom.Size = new System.Drawing.Size(83, 19);
            this.checkEdit_module_type_get_custom.TabIndex = 119;
            this.checkEdit_module_type_get_custom.Visible = false;
            this.checkEdit_module_type_get_custom.CheckedChanged += new System.EventHandler(this.checkEdit_module_type_get_custom_CheckedChanged);
            // 
            // checkEdit_module_type_get_keyword
            // 
            this.checkEdit_module_type_get_keyword.EditValue = true;
            this.checkEdit_module_type_get_keyword.Location = new System.Drawing.Point(708, 13);
            this.checkEdit_module_type_get_keyword.MenuManager = this.barManager1;
            this.checkEdit_module_type_get_keyword.Name = "checkEdit_module_type_get_keyword";
            this.checkEdit_module_type_get_keyword.Properties.Appearance.Options.UseTextOptions = true;
            this.checkEdit_module_type_get_keyword.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.checkEdit_module_type_get_keyword.Properties.Caption = "关键词智能抓取";
            this.checkEdit_module_type_get_keyword.Size = new System.Drawing.Size(113, 19);
            this.checkEdit_module_type_get_keyword.TabIndex = 118;
            this.checkEdit_module_type_get_keyword.Visible = false;
            this.checkEdit_module_type_get_keyword.CheckedChanged += new System.EventHandler(this.checkEdit_module_type_get_keyword_CheckedChanged);
            // 
            // popupMenu
            // 
            this.popupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem_delete)});
            this.popupMenu.Manager = this.barManager1;
            this.popupMenu.Name = "popupMenu";
            // 
            // frmModelShop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 670);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(891, 458);
            this.Name = "frmModelShop";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " 模块市场";
            this.Load += new System.EventHandler(this.frmModelShop_Load);
            this.Shown += new System.EventHandler(this.frmModelShop_Shown);
            this.SizeChanged += new System.EventHandler(this.frmModelShop_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main_view)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).EndInit();
            this.panelEx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ipanel)).EndInit();
            this.ipanel.ResumeLayout(false);
            this.ipanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myprocess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.text_word.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRadioGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_module_type_post.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_module_type_get.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_module_type_all.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_module_type_get_tongbu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_module_type_get_spider.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_module_type_get_custom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit_module_type_get_keyword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private DevExpress.XtraEditors.PanelControl panelEx1;
        private DevExpress.XtraGrid.GridControl gridControl_main;
        private DevExpress.XtraGrid.Views.Grid.GridView gridControl_main_view;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar6;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_getmodule_add;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_postmodule_add;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarToggleSwitchItem barToggleSwitchItem1;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItem1;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup repositoryItemRadioGroup1;
        private DevExpress.XtraEditors.CheckEdit checkEdit_module_type_post;
        private DevExpress.XtraEditors.CheckEdit checkEdit_module_type_get;
        private DevExpress.XtraEditors.CheckEdit checkEdit_module_type_all;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit checkEdit_module_type_get_tongbu;
        private DevExpress.XtraEditors.CheckEdit checkEdit_module_type_get_spider;
        private DevExpress.XtraEditors.CheckEdit checkEdit_module_type_get_custom;
        private DevExpress.XtraEditors.CheckEdit checkEdit_module_type_get_keyword;
        private DevExpress.XtraEditors.TextEdit text_word;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarStaticItem label_status;
        private DevExpress.XtraEditors.PanelControl ipanel;
        private DevExpress.XtraEditors.MarqueeProgressBarControl myprocess;
        private DevExpress.XtraEditors.LabelControl istate;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_refresh;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_delete;
        private DevExpress.XtraBars.PopupMenu popupMenu;
    }
}