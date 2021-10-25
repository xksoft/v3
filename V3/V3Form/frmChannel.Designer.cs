namespace V3.V3Form
{
    partial class frmChannel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChannel));
            this.panelEx3 = new DevExpress.XtraEditors.PanelControl();
            this.label1 = new System.Windows.Forms.Label();
            this.panelEx2 = new DevExpress.XtraEditors.PanelControl();
            this.ipanel = new DevExpress.XtraEditors.PanelControl();
            this.myprocess = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.istate = new DevExpress.XtraEditors.LabelControl();
            this.gridControl_main = new DevExpress.XtraGrid.GridControl();
            this.gridControl_main_view = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.buttonX1 = new DevExpress.XtraEditors.SimpleButton();
            this.buttonX2 = new DevExpress.XtraEditors.SimpleButton();
            this.txtPointName = new DevExpress.XtraEditors.TextEdit();
            this.panelEx5 = new DevExpress.XtraEditors.PanelControl();
            this.label2 = new System.Windows.Forms.Label();
            this.panelEx6 = new DevExpress.XtraEditors.PanelControl();
            this.textBoxX1 = new DevExpress.XtraEditors.MemoEdit();
            this.buttonX3 = new DevExpress.XtraEditors.SimpleButton();
            this.buttonX5 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx3)).BeginInit();
            this.panelEx3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx2)).BeginInit();
            this.panelEx2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ipanel)).BeginInit();
            this.ipanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myprocess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main_view)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPointName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx5)).BeginInit();
            this.panelEx5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx6)).BeginInit();
            this.panelEx6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxX1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx3
            // 
            this.panelEx3.Controls.Add(this.label1);
            this.panelEx3.Location = new System.Drawing.Point(14, 14);
            this.panelEx3.Name = "panelEx3";
            this.panelEx3.Size = new System.Drawing.Size(412, 30);
            this.panelEx3.TabIndex = 71;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(268, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "共有0个分类（多选可以按住 Ctrl 键不放再选择）";
            // 
            // panelEx2
            // 
            this.panelEx2.Controls.Add(this.ipanel);
            this.panelEx2.Controls.Add(this.gridControl_main);
            this.panelEx2.Location = new System.Drawing.Point(14, 43);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(412, 347);
            this.panelEx2.TabIndex = 70;
            // 
            // ipanel
            // 
            this.ipanel.Appearance.BackColor = System.Drawing.Color.White;
            this.ipanel.Appearance.Options.UseBackColor = true;
            this.ipanel.Appearance.Options.UseBorderColor = true;
            this.ipanel.Controls.Add(this.myprocess);
            this.ipanel.Controls.Add(this.labelControl1);
            this.ipanel.Controls.Add(this.istate);
            this.ipanel.Location = new System.Drawing.Point(13, 89);
            this.ipanel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.ipanel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.ipanel.Name = "ipanel";
            this.ipanel.Size = new System.Drawing.Size(385, 80);
            this.ipanel.TabIndex = 77;
            this.ipanel.Visible = false;
            // 
            // myprocess
            // 
            this.myprocess.EditValue = 0;
            this.myprocess.Location = new System.Drawing.Point(22, 48);
            this.myprocess.Name = "myprocess";
            this.myprocess.Size = new System.Drawing.Size(360, 10);
            this.myprocess.TabIndex = 4;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(386, 5);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(114, 14);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "按下Esc键终止该操作";
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
            this.gridControl_main.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_main.Location = new System.Drawing.Point(0, 0);
            this.gridControl_main.MainView = this.gridControl_main_view;
            this.gridControl_main.Name = "gridControl_main";
            this.gridControl_main.Size = new System.Drawing.Size(412, 347);
            this.gridControl_main.TabIndex = 111;
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
            this.buttonX1.Location = new System.Drawing.Point(673, 431);
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
            this.buttonX2.Location = new System.Drawing.Point(770, 431);
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
            this.txtPointName.Location = new System.Drawing.Point(14, 396);
            this.txtPointName.Name = "txtPointName";
            this.txtPointName.Properties.NullValuePrompt = "可以选择多个分类，亦可手工在这里输入，多个分类请用|间隔";
            this.txtPointName.Size = new System.Drawing.Size(412, 20);
            this.txtPointName.TabIndex = 2;
            // 
            // panelEx5
            // 
            this.panelEx5.Controls.Add(this.label2);
            this.panelEx5.Location = new System.Drawing.Point(433, 14);
            this.panelEx5.Name = "panelEx5";
            this.panelEx5.Size = new System.Drawing.Size(424, 30);
            this.panelEx5.TabIndex = 76;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(247, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "当前模块支持新增分类，请一行一个进行添加";
            // 
            // panelEx6
            // 
            this.panelEx6.Controls.Add(this.textBoxX1);
            this.panelEx6.Location = new System.Drawing.Point(433, 43);
            this.panelEx6.Name = "panelEx6";
            this.panelEx6.Size = new System.Drawing.Size(424, 373);
            this.panelEx6.TabIndex = 75;
            // 
            // textBoxX1
            // 
            this.textBoxX1.Location = new System.Drawing.Point(0, 0);
            this.textBoxX1.Name = "textBoxX1";
            this.textBoxX1.Size = new System.Drawing.Size(424, 373);
            this.textBoxX1.TabIndex = 0;
            this.textBoxX1.UseOptimizedRendering = true;
            // 
            // buttonX3
            // 
            this.buttonX3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX3.Image = ((System.Drawing.Image)(resources.GetObject("buttonX3.Image")));
            this.buttonX3.Location = new System.Drawing.Point(433, 431);
            this.buttonX3.Name = "buttonX3";
            this.buttonX3.Size = new System.Drawing.Size(87, 27);
            this.buttonX3.TabIndex = 78;
            this.buttonX3.Text = "新增分类";
            this.buttonX3.Click += new System.EventHandler(this.buttonX3_Click);
            // 
            // buttonX5
            // 
            this.buttonX5.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX5.Image = ((System.Drawing.Image)(resources.GetObject("buttonX5.Image")));
            this.buttonX5.Location = new System.Drawing.Point(339, 431);
            this.buttonX5.Name = "buttonX5";
            this.buttonX5.Size = new System.Drawing.Size(87, 27);
            this.buttonX5.TabIndex = 80;
            this.buttonX5.Text = "刷新分类";
            this.buttonX5.Click += new System.EventHandler(this.buttonX5_Click);
            // 
            // frmChannel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 473);
            this.Controls.Add(this.txtPointName);
            this.Controls.Add(this.buttonX5);
            this.Controls.Add(this.buttonX3);
            this.Controls.Add(this.panelEx5);
            this.Controls.Add(this.panelEx6);
            this.Controls.Add(this.buttonX2);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.panelEx3);
            this.Controls.Add(this.panelEx2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChannel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "发布分类";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmChannel_FormClosed);
            this.Load += new System.EventHandler(this.SelectClass_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ClassManage_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.panelEx3)).EndInit();
            this.panelEx3.ResumeLayout(false);
            this.panelEx3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx2)).EndInit();
            this.panelEx2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ipanel)).EndInit();
            this.ipanel.ResumeLayout(false);
            this.ipanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myprocess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main_view)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPointName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx5)).EndInit();
            this.panelEx5.ResumeLayout(false);
            this.panelEx5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx6)).EndInit();
            this.panelEx6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textBoxX1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelEx3;
        private DevExpress.XtraEditors.PanelControl panelEx2;
        private DevExpress.XtraEditors.SimpleButton buttonX1;
        private DevExpress.XtraEditors.SimpleButton buttonX2;
        private DevExpress.XtraEditors.TextEdit txtPointName;
        private DevExpress.XtraEditors.PanelControl panelEx5;
        private DevExpress.XtraEditors.PanelControl panelEx6;
        private DevExpress.XtraEditors.MemoEdit textBoxX1;
        private DevExpress.XtraEditors.SimpleButton buttonX3;
        private DevExpress.XtraEditors.SimpleButton buttonX5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraGrid.GridControl gridControl_main;
        private DevExpress.XtraGrid.Views.Grid.GridView gridControl_main_view;
        private DevExpress.XtraEditors.PanelControl ipanel;
        private DevExpress.XtraEditors.MarqueeProgressBarControl myprocess;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl istate;
    }
}