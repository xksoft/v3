namespace V3.V3Form.抓取模块
{
    partial class PlanReadme
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlanReadme));
            this.panelEx1 = new DevExpress.XtraEditors.PanelControl();
            this.groupPanel2 = new DevExpress.XtraEditors.GroupControl();
            this.txtDescription = new DevExpress.XtraEditors.MemoEdit();
            this.groupPanel1 = new DevExpress.XtraEditors.GroupControl();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbmoney = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelX5 = new DevExpress.XtraEditors.LabelControl();
            this.s1 = new DevExpress.XtraEditors.CheckEdit();
            this.s2 = new DevExpress.XtraEditors.CheckEdit();
            this.txtPlanUrl = new DevExpress.XtraEditors.TextEdit();
            this.txtReadme = new DevExpress.XtraEditors.TextEdit();
            this.txtPlanname = new DevExpress.XtraEditors.TextEdit();
            this.labelX4 = new DevExpress.XtraEditors.LabelControl();
            this.labelX2 = new DevExpress.XtraEditors.LabelControl();
            this.labelX1 = new DevExpress.XtraEditors.LabelControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.btn_save = new DevExpress.XtraBars.BarButtonItem();
            this.btn_cancel = new DevExpress.XtraBars.BarButtonItem();
            this.bar4 = new DevExpress.XtraBars.Bar();
            this.statusbar = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).BeginInit();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupPanel2)).BeginInit();
            this.groupPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupPanel1)).BeginInit();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbmoney.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlanUrl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReadme.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlanname.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelEx1.Controls.Add(this.groupPanel2);
            this.panelEx1.Controls.Add(this.groupPanel1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 30);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(741, 379);
            this.panelEx1.TabIndex = 0;
            // 
            // groupPanel2
            // 
            this.groupPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPanel2.Controls.Add(this.txtDescription);
            this.groupPanel2.Location = new System.Drawing.Point(12, 175);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(717, 185);
            this.groupPanel2.TabIndex = 46;
            this.groupPanel2.Text = "详细介绍";
            // 
            // txtDescription
            // 
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.Location = new System.Drawing.Point(2, 22);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtDescription.Properties.NullValuePrompt = "采集模块：使用注意事项，例如文章中是否有html代码，是否有图片等以及建议发布时如何处理等。\r\n发布模块：支持的网站编码，是否需要登录附加值以及各个附加值的含义，" +
    "是否需要验证码等。";
            this.txtDescription.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtDescription.Size = new System.Drawing.Size(713, 161);
            this.txtDescription.TabIndex = 0;
            this.txtDescription.UseOptimizedRendering = true;
            // 
            // groupPanel1
            // 
            this.groupPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPanel1.Controls.Add(this.label1);
            this.groupPanel1.Controls.Add(this.cmbmoney);
            this.groupPanel1.Controls.Add(this.labelX5);
            this.groupPanel1.Controls.Add(this.s1);
            this.groupPanel1.Controls.Add(this.s2);
            this.groupPanel1.Controls.Add(this.txtPlanUrl);
            this.groupPanel1.Controls.Add(this.txtReadme);
            this.groupPanel1.Controls.Add(this.txtPlanname);
            this.groupPanel1.Controls.Add(this.labelX4);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Location = new System.Drawing.Point(12, 3);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(717, 164);
            this.groupPanel1.TabIndex = 45;
            this.groupPanel1.Text = "基本信息";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(395, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(307, 14);
            this.label1.TabIndex = 19;
            this.label1.Text = "在线模块交易将在近期开放，您可以先将模块设置为私有";
            // 
            // cmbmoney
            // 
            this.cmbmoney.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbmoney.Enabled = false;
            this.cmbmoney.Location = new System.Drawing.Point(282, 124);
            this.cmbmoney.Name = "cmbmoney";
            this.cmbmoney.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down)});
            this.cmbmoney.Properties.Items.AddRange(new object[] {
            "免费",
            "1元",
            "10元",
            "30元",
            "60元",
            "100元"});
            this.cmbmoney.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbmoney.Size = new System.Drawing.Size(95, 20);
            this.cmbmoney.TabIndex = 18;
            this.cmbmoney.SelectedIndexChanged += new System.EventHandler(this.cmbmoney_SelectedIndexChanged);
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(32, 127);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(60, 14);
            this.labelX5.TabIndex = 13;
            this.labelX5.Text = "模块类型：";
            // 
            // s1
            // 
            this.s1.EditValue = true;
            this.s1.Location = new System.Drawing.Point(98, 125);
            this.s1.Name = "s1";
            this.s1.Properties.Caption = "私有模块";
            this.s1.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.s1.Properties.RadioGroupIndex = 1;
            this.s1.Size = new System.Drawing.Size(94, 19);
            this.s1.TabIndex = 11;
            this.s1.CheckedChanged += new System.EventHandler(this.s1_CheckedChanged);
            // 
            // s2
            // 
            this.s2.Location = new System.Drawing.Point(197, 125);
            this.s2.Name = "s2";
            this.s2.Properties.Caption = "共享模块";
            this.s2.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.s2.Properties.RadioGroupIndex = 1;
            this.s2.Size = new System.Drawing.Size(75, 19);
            this.s2.TabIndex = 12;
            this.s2.TabStop = false;
            this.s2.CheckedChanged += new System.EventHandler(this.s1_CheckedChanged);
            // 
            // txtPlanUrl
            // 
            this.txtPlanUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPlanUrl.Location = new System.Drawing.Point(99, 92);
            this.txtPlanUrl.Name = "txtPlanUrl";
            this.txtPlanUrl.Properties.NullValuePrompt = "如需共享模块，请务必到论坛模块专区发帖，并记录下帖子地址填写在此。。。";
            this.txtPlanUrl.Size = new System.Drawing.Size(603, 20);
            this.txtPlanUrl.TabIndex = 10;
            // 
            // txtReadme
            // 
            this.txtReadme.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReadme.Location = new System.Drawing.Point(99, 61);
            this.txtReadme.Name = "txtReadme";
            this.txtReadme.Properties.NullValuePrompt = "一句话描述模块，例如采集的目标栏目和文章类型或者发布类型，是否需要验证码等";
            this.txtReadme.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtReadme.Size = new System.Drawing.Size(603, 20);
            this.txtReadme.TabIndex = 8;
            // 
            // txtPlanname
            // 
            this.txtPlanname.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPlanname.Location = new System.Drawing.Point(99, 29);
            this.txtPlanname.Name = "txtPlanname";
            this.txtPlanname.Properties.NullValuePrompt = "模块采集的目标网站名称或支持发布的网站系统以及版本";
            this.txtPlanname.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtPlanname.Size = new System.Drawing.Size(603, 20);
            this.txtPlanname.TabIndex = 7;
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(21, 95);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(72, 14);
            this.labelX4.TabIndex = 6;
            this.labelX4.Text = "模块交流贴：";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(9, 64);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(84, 14);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "模块简短说明：";
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(33, 32);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(60, 14);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "模块名称：";
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar3,
            this.bar4});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.statusbar,
            this.btn_save,
            this.btn_cancel});
            this.barManager1.MainMenu = this.bar3;
            this.barManager1.MaxItemId = 3;
            this.barManager1.StatusBar = this.bar4;
            // 
            // bar3
            // 
            this.bar3.BarItemVertIndent = 5;
            this.bar3.BarName = "Main menu";
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_save),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_cancel)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawBorder = false;
            this.bar3.OptionsBar.MultiLine = true;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Main menu";
            // 
            // btn_save
            // 
            this.btn_save.Caption = "保存数据";
            this.btn_save.Glyph = global::V3.Properties.Resources.btn_save;
            this.btn_save.Id = 1;
            this.btn_save.Name = "btn_save";
            this.btn_save.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btn_save.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_save_ItemClick);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Caption = "放弃修改";
            this.btn_cancel.Glyph = global::V3.Properties.Resources.btn_cancel;
            this.btn_cancel.Id = 2;
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btn_cancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_cancel_ItemClick);
            // 
            // bar4
            // 
            this.bar4.BarName = "Status bar";
            this.bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar4.DockCol = 0;
            this.bar4.DockRow = 0;
            this.bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar4.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.statusbar)});
            this.bar4.OptionsBar.AllowQuickCustomization = false;
            this.bar4.OptionsBar.DrawDragBorder = false;
            this.bar4.OptionsBar.UseWholeRow = true;
            this.bar4.Text = "Status bar";
            // 
            // statusbar
            // 
            this.statusbar.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.statusbar.Caption = "如果作为共享模块，请仔细填写更新信息以及使用方法以及注意事项！共享价格可以随时修改，但只对新用户有效！";
            this.statusbar.Id = 0;
            this.statusbar.Name = "statusbar";
            this.statusbar.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(741, 30);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 409);
            this.barDockControlBottom.Size = new System.Drawing.Size(741, 26);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 30);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 379);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(741, 30);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 379);
            // 
            // PlanReadme
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 435);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "PlanReadme";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "模块信息设置";
            this.Load += new System.EventHandler(this.PlanReadme_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).EndInit();
            this.panelEx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupPanel2)).EndInit();
            this.groupPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupPanel1)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbmoney.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlanUrl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReadme.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlanname.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelEx1;
        private DevExpress.XtraEditors.GroupControl groupPanel1;
        private DevExpress.XtraEditors.TextEdit txtPlanUrl;
        private DevExpress.XtraEditors.TextEdit txtReadme;
        private DevExpress.XtraEditors.TextEdit txtPlanname;
        private DevExpress.XtraEditors.LabelControl labelX4;
        private DevExpress.XtraEditors.LabelControl labelX2;
        private DevExpress.XtraEditors.LabelControl labelX1;
        private DevExpress.XtraEditors.LabelControl labelX5;
        private DevExpress.XtraEditors.CheckEdit s1;
        private DevExpress.XtraEditors.CheckEdit s2;
        private DevExpress.XtraEditors.GroupControl groupPanel2;
        private DevExpress.XtraEditors.MemoEdit txtDescription;
        private DevExpress.XtraEditors.ComboBoxEdit cmbmoney;
        
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarButtonItem btn_save;
        private DevExpress.XtraBars.BarButtonItem btn_cancel;
        private DevExpress.XtraBars.Bar bar4;
        private DevExpress.XtraBars.BarStaticItem statusbar;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private System.Windows.Forms.Label label1;
    }
}