namespace V3.V3Form
{
    partial class frm_关键词库
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_关键词库));
            this.keyword = new DevExpress.XtraEditors.MemoEdit();
            this.ipanel = new DevExpress.XtraEditors.PanelControl();
            this.myprocess = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.labelX17 = new DevExpress.XtraEditors.LabelControl();
            this.istate = new DevExpress.XtraEditors.LabelControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btn_save = new DevExpress.XtraBars.BarButtonItem();
            this.btn_cancel = new DevExpress.XtraBars.BarButtonItem();
            this.btn_import = new DevExpress.XtraBars.BarButtonItem();
            this.btn_export = new DevExpress.XtraBars.BarButtonItem();
            this.btn_unique = new DevExpress.XtraBars.BarButtonItem();
            this.btn_tb = new DevExpress.XtraBars.BarButtonItem();
            this.btn_tbb = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.Status = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.keyword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ipanel)).BeginInit();
            this.ipanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myprocess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // keyword
            // 
            this.keyword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keyword.Location = new System.Drawing.Point(0, 32);
            this.keyword.Name = "keyword";
            this.keyword.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.keyword.Properties.NullValuePrompt = "请输入关键词，一行一个...";
            this.keyword.Size = new System.Drawing.Size(1133, 625);
            this.keyword.TabIndex = 2;
            this.keyword.UseOptimizedRendering = true;
            this.keyword.EditValueChanged += new System.EventHandler(this.keyword_EditValueChanged);
            this.keyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyword_KeyDown);
            this.keyword.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.keyword_MouseDoubleClick);
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
            this.ipanel.TabIndex = 69;
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
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btn_save,
            this.btn_cancel,
            this.btn_unique,
            this.btn_import,
            this.btn_export,
            this.btn_tb,
            this.btn_tbb,
            this.Status});
            this.barManager1.MaxItemId = 8;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarItemVertIndent = 5;
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_save),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_cancel),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_import, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_export),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_unique, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_tb),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_tbb)});
            this.bar1.OptionsBar.AllowRename = true;
            this.bar1.OptionsBar.DrawBorder = false;
            this.bar1.Text = "Tools";
            // 
            // btn_save
            // 
            this.btn_save.Caption = "保存";
            this.btn_save.Glyph = global::V3.Properties.Resources.btn_save;
            this.btn_save.Id = 0;
            this.btn_save.Name = "btn_save";
            this.btn_save.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btn_save.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_save_ItemClick);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Caption = "取消";
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
            this.btn_import.Id = 3;
            this.btn_import.Name = "btn_import";
            this.btn_import.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btn_import.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_import_ItemClick);
            // 
            // btn_export
            // 
            this.btn_export.Caption = "导出";
            this.btn_export.Glyph = global::V3.Properties.Resources.btn_module_input;
            this.btn_export.Id = 4;
            this.btn_export.Name = "btn_export";
            this.btn_export.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btn_export.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_export_ItemClick);
            // 
            // btn_unique
            // 
            this.btn_unique.Caption = "去除重复";
            this.btn_unique.Glyph = global::V3.Properties.Resources.btn_unique;
            this.btn_unique.Id = 2;
            this.btn_unique.Name = "btn_unique";
            this.btn_unique.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btn_unique.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_unique_ItemClick);
            // 
            // btn_tb
            // 
            this.btn_tb.Caption = "提纯（包含）";
            this.btn_tb.Glyph = global::V3.Properties.Resources.btn_unique1;
            this.btn_tb.Id = 5;
            this.btn_tb.Name = "btn_tb";
            this.btn_tb.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btn_tb.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_tb_ItemClick);
            // 
            // btn_tbb
            // 
            this.btn_tbb.Caption = "提纯（不包含）";
            this.btn_tbb.Glyph = global::V3.Properties.Resources.btn_unique2;
            this.btn_tbb.Id = 6;
            this.btn_tbb.Name = "btn_tbb";
            this.btn_tbb.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btn_tbb.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_tbb_ItemClick);
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
            this.Status.Caption = "加载中...";
            this.Status.Id = 7;
            this.Status.Name = "Status";
            this.Status.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1133, 32);
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
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 32);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 625);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1133, 32);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 625);
            // 
            // frm_关键词库
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1133, 683);
            this.Controls.Add(this.ipanel);
            this.Controls.Add(this.keyword);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_关键词库";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "替换库";
            this.Load += new System.EventHandler(this.frm_关键词库_Load);
            this.Shown += new System.EventHandler(this.frm_关键词库_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_关键词库_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.keyword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ipanel)).EndInit();
            this.ipanel.ResumeLayout(false);
            this.ipanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myprocess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

   
        private DevExpress.XtraEditors.MemoEdit keyword;
        private DevExpress.XtraEditors.PanelControl ipanel;
        private DevExpress.XtraEditors.MarqueeProgressBarControl myprocess;
        private DevExpress.XtraEditors.LabelControl labelX17;
        private DevExpress.XtraEditors.LabelControl istate;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem btn_save;
        private DevExpress.XtraBars.BarButtonItem btn_cancel;
        private DevExpress.XtraBars.BarButtonItem btn_unique;
        private DevExpress.XtraBars.BarButtonItem btn_import;
        private DevExpress.XtraBars.BarButtonItem btn_export;
        private DevExpress.XtraBars.BarButtonItem btn_tb;
        private DevExpress.XtraBars.BarButtonItem btn_tbb;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarStaticItem Status;

    }
}