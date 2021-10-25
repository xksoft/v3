namespace V3.V3Form
{
    partial class frmSelectChannel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectChannel));
            this.panelEx3 = new DevExpress.XtraEditors.PanelControl();
            this.label_channel = new System.Windows.Forms.Label();
            this.gridControl_main = new DevExpress.XtraGrid.GridControl();
            this.gridControl_main_view = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.buttonX1 = new DevExpress.XtraEditors.SimpleButton();
            this.buttonX2 = new DevExpress.XtraEditors.SimpleButton();
            this.txtPointName = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx3)).BeginInit();
            this.panelEx3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main_view)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPointName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx3
            // 
            this.panelEx3.Controls.Add(this.label_channel);
            this.panelEx3.Location = new System.Drawing.Point(14, 14);
            this.panelEx3.Name = "panelEx3";
            this.panelEx3.Size = new System.Drawing.Size(439, 30);
            this.panelEx3.TabIndex = 71;
            // 
            // label_channel
            // 
            this.label_channel.AutoSize = true;
            this.label_channel.Location = new System.Drawing.Point(15, 8);
            this.label_channel.Name = "label_channel";
            this.label_channel.Size = new System.Drawing.Size(79, 14);
            this.label_channel.TabIndex = 0;
            this.label_channel.Text = "分类加载中...";
            // 
            // gridControl_main
            // 
            this.gridControl_main.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_main.Location = new System.Drawing.Point(14, 43);
            this.gridControl_main.MainView = this.gridControl_main_view;
            this.gridControl_main.Name = "gridControl_main";
            this.gridControl_main.Size = new System.Drawing.Size(439, 248);
            this.gridControl_main.TabIndex = 112;
            this.gridControl_main.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridControl_main_view});
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
            this.gridControl_main_view.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.gridControl_main_view_SelectionChanged);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Image = ((System.Drawing.Image)(resources.GetObject("buttonX1.Image")));
            this.buttonX1.Location = new System.Drawing.Point(190, 328);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(87, 27);
            this.buttonX1.TabIndex = 72;
            this.buttonX1.Text = "确定";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.Image = ((System.Drawing.Image)(resources.GetObject("buttonX2.Image")));
            this.buttonX2.Location = new System.Drawing.Point(366, 328);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(87, 27);
            this.buttonX2.TabIndex = 73;
            this.buttonX2.Text = "取消";
            this.buttonX2.Click += new System.EventHandler(this.buttonX2_Click);
            // 
            // txtPointName
            // 
            this.txtPointName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPointName.Location = new System.Drawing.Point(14, 296);
            this.txtPointName.Name = "txtPointName";
            this.txtPointName.Properties.NullValuePrompt = "可以选择多个分类，亦可手工在这里输入，多个分类请用|间隔";
            this.txtPointName.Size = new System.Drawing.Size(439, 20);
            this.txtPointName.TabIndex = 2;
            // 
            // frmSelectChannel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 365);
            this.Controls.Add(this.gridControl_main);
            this.Controls.Add(this.txtPointName);
            this.Controls.Add(this.buttonX2);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.panelEx3);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Office 2013 Light Gray";
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(483, 403);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(483, 403);
            this.Name = "frmSelectChannel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "发布分类";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SelectClass_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelEx3)).EndInit();
            this.panelEx3.ResumeLayout(false);
            this.panelEx3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main_view)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPointName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelEx3;
        private DevExpress.XtraEditors.SimpleButton buttonX1;
        private DevExpress.XtraEditors.SimpleButton buttonX2;
        private DevExpress.XtraEditors.TextEdit txtPointName;
        private System.Windows.Forms.Label label_channel;
        private DevExpress.XtraGrid.GridControl gridControl_main;
        private DevExpress.XtraGrid.Views.Grid.GridView gridControl_main_view;
    }
}