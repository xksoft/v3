namespace V3.V3Form
{
    partial class frm_参数输入编辑器
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_参数输入编辑器));
            this.txtMainbox = new DevExpress.XtraEditors.MemoEdit();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.btn_save = new DevExpress.XtraBars.BarButtonItem();
            this.btn_cancel = new DevExpress.XtraBars.BarButtonItem();
            this.txttitle = new DevExpress.XtraBars.BarStaticItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.txtStatus = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtMainbox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtMainbox
            // 
            this.txtMainbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMainbox.Location = new System.Drawing.Point(0, 30);
            this.txtMainbox.Name = "txtMainbox";
            this.txtMainbox.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtMainbox.Properties.NullValuePrompt = "请按照指定格式输入...";
            this.txtMainbox.Size = new System.Drawing.Size(1133, 625);
            this.txtMainbox.TabIndex = 2;
            this.txtMainbox.EditValueChanged += new System.EventHandler(this.txtMainbox_EditValueChanged);
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btn_save,
            this.btn_cancel,
            this.txttitle,
            this.txtStatus});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 4;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar2
            // 
            this.bar2.BarItemVertIndent = 5;
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_save),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_cancel),
            new DevExpress.XtraBars.LinkPersistInfo(this.txttitle, true)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawBorder = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
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
            // txttitle
            // 
            this.txttitle.Caption = "格式说明";
            this.txttitle.Id = 2;
            this.txttitle.ItemAppearance.Normal.ForeColor = System.Drawing.Color.Red;
            this.txttitle.ItemAppearance.Normal.Options.UseForeColor = true;
            this.txttitle.Name = "txttitle";
            this.txttitle.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.txtStatus)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // txtStatus
            // 
            this.txtStatus.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtStatus.Caption = "就绪";
            this.txtStatus.Id = 3;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.TextAlignment = System.Drawing.StringAlignment.Near;
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
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 655);
            this.barDockControlBottom.Size = new System.Drawing.Size(1133, 28);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 30);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 625);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1133, 30);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 625);
            // 
            // frm_参数输入编辑器
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1133, 683);
            this.Controls.Add(this.txtMainbox);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_参数输入编辑器";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "参数输入编辑器";
            ((System.ComponentModel.ISupportInitialize)(this.txtMainbox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

   
        public DevExpress.XtraEditors.MemoEdit txtMainbox;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem btn_save;
        private DevExpress.XtraBars.BarButtonItem btn_cancel;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        public DevExpress.XtraBars.BarStaticItem txttitle;
        public DevExpress.XtraBars.BarStaticItem txtStatus;


    }
}