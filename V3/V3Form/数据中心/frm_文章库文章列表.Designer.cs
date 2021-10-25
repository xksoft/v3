namespace V3.V3Form
{
    partial class frm_文章库文章列表
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_文章库文章列表));
            this.gridControl_main = new DevExpress.XtraGrid.GridControl();
            this.gridControl_main_view = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.popupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.btn_edit = new DevExpress.XtraBars.BarButtonItem();
            this.btn_delete = new DevExpress.XtraBars.BarButtonItem();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.btn_add = new DevExpress.XtraBars.BarButtonItem();
            this.btn_cancel = new DevExpress.XtraBars.BarButtonItem();
            this.btn_import = new DevExpress.XtraBars.BarButtonItem();
            this.btn_output = new DevExpress.XtraBars.BarButtonItem();
            this.btn_clear = new DevExpress.XtraBars.BarButtonItem();
            this.btn_pre = new DevExpress.XtraBars.BarButtonItem();
            this.txtpage = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.btn_next = new DevExpress.XtraBars.BarButtonItem();
            this.btn_ref = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.Status = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.ipanel = new DevExpress.XtraEditors.PanelControl();
            this.myprocess = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.labelX17 = new DevExpress.XtraEditors.LabelControl();
            this.istate = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main_view)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ipanel)).BeginInit();
            this.ipanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myprocess.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl_main
            // 
            this.gridControl_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl_main.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_main.Location = new System.Drawing.Point(-1, 36);
            this.gridControl_main.MainView = this.gridControl_main_view;
            this.gridControl_main.Name = "gridControl_main";
            this.gridControl_main.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit1});
            this.gridControl_main.Size = new System.Drawing.Size(1135, 624);
            this.gridControl_main.TabIndex = 112;
            this.gridControl_main.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridControl_main_view});
            this.gridControl_main.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.gridControl_main_MouseDoubleClick);
            this.gridControl_main.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridControl_main_MouseDown);
            this.gridControl_main.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridControl_main_MouseMove);
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
            this.gridControl_main_view.OptionsView.ShowGroupPanel = false;
            this.gridControl_main_view.RowHeight = 22;
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "awfa", 20, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.repositoryItemButtonEdit1.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            // 
            // popupMenu
            // 
            this.popupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_edit),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_delete)});
            this.popupMenu.Manager = this.barManager;
            this.popupMenu.Name = "popupMenu";
            // 
            // btn_edit
            // 
            this.btn_edit.Caption = "编辑文章";
            this.btn_edit.Glyph = global::V3.Properties.Resources.btn_modify;
            this.btn_edit.Id = 9;
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_edit_ItemClick);
            // 
            // btn_delete
            // 
            this.btn_delete.Caption = "删除选中文章";
            this.btn_delete.Glyph = global::V3.Properties.Resources.delete;
            this.btn_delete.Id = 8;
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_delete_ItemClick);
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.bar3});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btn_add,
            this.btn_cancel,
            this.btn_import,
            this.btn_output,
            this.btn_clear,
            this.btn_pre,
            this.txtpage,
            this.btn_next,
            this.btn_delete,
            this.btn_edit,
            this.btn_ref,
            this.Status});
            this.barManager.MainMenu = this.bar2;
            this.barManager.MaxItemId = 12;
            this.barManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1});
            this.barManager.StatusBar = this.bar3;
            // 
            // bar2
            // 
            this.bar2.BarItemVertIndent = 5;
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_add),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_cancel),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_import, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_output),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_clear),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_pre, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.txtpage),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_next),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_ref)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawBorder = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // btn_add
            // 
            this.btn_add.Caption = "新建文章";
            this.btn_add.Glyph = global::V3.Properties.Resources.add;
            this.btn_add.Id = 0;
            this.btn_add.Name = "btn_add";
            this.btn_add.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btn_add.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_add_ItemClick);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Caption = "离开";
            this.btn_cancel.Glyph = global::V3.Properties.Resources.btn_cancel;
            this.btn_cancel.Id = 1;
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btn_cancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_cancel_ItemClick);
            // 
            // btn_import
            // 
            this.btn_import.Caption = "导入";
            this.btn_import.Glyph = global::V3.Properties.Resources.btn_module_input;
            this.btn_import.Id = 2;
            this.btn_import.Name = "btn_import";
            this.btn_import.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btn_import.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_import_ItemClick);
            // 
            // btn_output
            // 
            this.btn_output.Caption = "导出";
            this.btn_output.Glyph = global::V3.Properties.Resources.btn_module_output;
            this.btn_output.Id = 3;
            this.btn_output.Name = "btn_output";
            this.btn_output.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btn_output.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_output_ItemClick);
            // 
            // btn_clear
            // 
            this.btn_clear.Caption = "清空";
            this.btn_clear.Glyph = ((System.Drawing.Image)(resources.GetObject("btn_clear.Glyph")));
            this.btn_clear.Id = 4;
            this.btn_clear.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btn_clear.LargeGlyph")));
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btn_clear.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_clear_ItemClick);
            // 
            // btn_pre
            // 
            this.btn_pre.Caption = "上一页";
            this.btn_pre.Glyph = global::V3.Properties.Resources.page_pre;
            this.btn_pre.Id = 5;
            this.btn_pre.Name = "btn_pre";
            this.btn_pre.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_pre_ItemClick);
            // 
            // txtpage
            // 
            this.txtpage.Caption = "barEditItem1";
            this.txtpage.Edit = this.repositoryItemTextEdit1;
            this.txtpage.Id = 6;
            this.txtpage.Name = "txtpage";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // btn_next
            // 
            this.btn_next.Caption = "下一页";
            this.btn_next.Glyph = global::V3.Properties.Resources.page_next;
            this.btn_next.Id = 7;
            this.btn_next.Name = "btn_next";
            this.btn_next.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_next_ItemClick);
            // 
            // btn_ref
            // 
            this.btn_ref.Caption = "刷新";
            this.btn_ref.Glyph = global::V3.Properties.Resources.refresh;
            this.btn_ref.Id = 10;
            this.btn_ref.Name = "btn_ref";
            this.btn_ref.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ref_ItemClick);
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.Status)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // Status
            // 
            this.Status.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.Status.Caption = "数据加载中...";
            this.Status.Id = 11;
            this.Status.Name = "Status";
            this.Status.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1133, 30);
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
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 30);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 627);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1133, 30);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 627);
            // 
            // ipanel
            // 
            this.ipanel.Appearance.BackColor = System.Drawing.Color.White;
            this.ipanel.Appearance.Options.UseBackColor = true;
            this.ipanel.Appearance.Options.UseBorderColor = true;
            this.ipanel.Controls.Add(this.myprocess);
            this.ipanel.Controls.Add(this.labelX17);
            this.ipanel.Controls.Add(this.istate);
            this.ipanel.Location = new System.Drawing.Point(301, 296);
            this.ipanel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.ipanel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.ipanel.Name = "ipanel";
            this.ipanel.Size = new System.Drawing.Size(520, 80);
            this.ipanel.TabIndex = 117;
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
            // labelX17
            // 
            this.labelX17.Location = new System.Drawing.Point(386, 5);
            this.labelX17.Name = "labelX17";
            this.labelX17.Size = new System.Drawing.Size(114, 14);
            this.labelX17.TabIndex = 3;
            this.labelX17.Text = "按下Esc键终止该操作";
            // 
            // istate
            // 
            this.istate.Location = new System.Drawing.Point(22, 21);
            this.istate.Name = "istate";
            this.istate.Size = new System.Drawing.Size(38, 14);
            this.istate.TabIndex = 1;
            this.istate.Text = "labelX2";
            // 
            // frm_文章库文章列表
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1133, 683);
            this.Controls.Add(this.ipanel);
            this.Controls.Add(this.gridControl_main);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_文章库文章列表";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "文章库[XXX]的文章列表";
            this.Load += new System.EventHandler(this.frm_文章库文章列表_Load);
            this.Shown += new System.EventHandler(this.frmMainDbListview_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main_view)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ipanel)).EndInit();
            this.ipanel.ResumeLayout(false);
            this.ipanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myprocess.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

      
        private DevExpress.XtraGrid.GridControl gridControl_main;
        private DevExpress.XtraGrid.Views.Grid.GridView gridControl_main_view;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraBars.PopupMenu popupMenu;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem btn_add;
        private DevExpress.XtraBars.BarButtonItem btn_cancel;
        private DevExpress.XtraBars.BarButtonItem btn_import;
        private DevExpress.XtraBars.BarButtonItem btn_output;
        private DevExpress.XtraBars.BarButtonItem btn_clear;
        private DevExpress.XtraBars.BarButtonItem btn_pre;
        private DevExpress.XtraBars.BarEditItem txtpage;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarButtonItem btn_next;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btn_delete;
        private DevExpress.XtraBars.BarButtonItem btn_edit;
        private DevExpress.XtraEditors.PanelControl ipanel;
        private DevExpress.XtraEditors.MarqueeProgressBarControl myprocess;
        private DevExpress.XtraEditors.LabelControl labelX17;
        private DevExpress.XtraEditors.LabelControl istate;
        private DevExpress.XtraBars.BarButtonItem btn_ref;
        private DevExpress.XtraBars.BarStaticItem Status;

    }
}