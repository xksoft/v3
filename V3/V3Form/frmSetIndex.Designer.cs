namespace V3.V3Form
{
    partial class frmSetIndex
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetIndex));
            this.panelEx3 = new DevExpress.XtraEditors.PanelControl();
            this.label_count = new DevExpress.XtraEditors.LabelControl();
            this.buttonX1 = new DevExpress.XtraEditors.SimpleButton();
            this.panelEx6 = new DevExpress.XtraEditors.PanelControl();
            this.textBoxX1 = new DevExpress.XtraEditors.MemoEdit();
            this.groupPanel1 = new DevExpress.XtraEditors.GroupControl();
            this.switchButton1 = new DevExpress.XtraEditors.ToggleSwitch();
            this.buttonX4 = new DevExpress.XtraEditors.SimpleButton();
            this.textBoxX2 = new DevExpress.XtraEditors.TextEdit();
            this.labelX2 = new DevExpress.XtraEditors.LabelControl();
            this.labelX1 = new DevExpress.XtraEditors.LabelControl();
            this.ipanel = new DevExpress.XtraEditors.PanelControl();
            this.myprocess = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.istate = new DevExpress.XtraEditors.LabelControl();
            this.gridControl_main = new DevExpress.XtraGrid.GridControl();
            this.gridControl_main_view = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx3)).BeginInit();
            this.panelEx3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx6)).BeginInit();
            this.panelEx6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxX1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupPanel1)).BeginInit();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.switchButton1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxX2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ipanel)).BeginInit();
            this.ipanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myprocess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main_view)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx3
            // 
            this.panelEx3.Controls.Add(this.label_count);
            this.panelEx3.Location = new System.Drawing.Point(7, 10);
            this.panelEx3.Name = "panelEx3";
            this.panelEx3.Size = new System.Drawing.Size(416, 30);
            this.panelEx3.TabIndex = 71;
            // 
            // label_count
            // 
            this.label_count.Location = new System.Drawing.Point(5, 8);
            this.label_count.Name = "label_count";
            this.label_count.Size = new System.Drawing.Size(84, 14);
            this.label_count.TabIndex = 0;
            this.label_count.Text = "文章库加载中...";
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Location = new System.Drawing.Point(107, 92);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(87, 27);
            this.buttonX1.TabIndex = 3;
            this.buttonX1.Text = "开始建立";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // panelEx6
            // 
            this.panelEx6.Controls.Add(this.textBoxX1);
            this.panelEx6.Location = new System.Drawing.Point(431, 129);
            this.panelEx6.Name = "panelEx6";
            this.panelEx6.Size = new System.Drawing.Size(408, 219);
            this.panelEx6.TabIndex = 75;
            // 
            // textBoxX1
            // 
            this.textBoxX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxX1.Location = new System.Drawing.Point(2, 2);
            this.textBoxX1.Name = "textBoxX1";
            this.textBoxX1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.textBoxX1.Properties.HideSelection = false;
            this.textBoxX1.Properties.ReadOnly = true;
            this.textBoxX1.Size = new System.Drawing.Size(404, 215);
            this.textBoxX1.TabIndex = 10;
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.switchButton1);
            this.groupPanel1.Controls.Add(this.buttonX4);
            this.groupPanel1.Controls.Add(this.textBoxX2);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.buttonX1);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Location = new System.Drawing.Point(431, -1);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(408, 124);
            this.groupPanel1.TabIndex = 112;
            this.groupPanel1.Text = "生成配置";
            // 
            // switchButton1
            // 
            this.switchButton1.Location = new System.Drawing.Point(107, 59);
            this.switchButton1.Name = "switchButton1";
            this.switchButton1.Properties.OffText = "重新建立资料库";
            this.switchButton1.Properties.OnText = "在现有资料库上追加数据";
            this.switchButton1.Size = new System.Drawing.Size(293, 25);
            this.switchButton1.TabIndex = 2;
            // 
            // buttonX4
            // 
            this.buttonX4.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX4.Location = new System.Drawing.Point(327, 29);
            this.buttonX4.Name = "buttonX4";
            this.buttonX4.Size = new System.Drawing.Size(73, 21);
            this.buttonX4.TabIndex = 1;
            this.buttonX4.Text = "选择";
            this.buttonX4.Click += new System.EventHandler(this.buttonX4_Click);
            // 
            // textBoxX2
            // 
            this.textBoxX2.Location = new System.Drawing.Point(107, 29);
            this.textBoxX2.Name = "textBoxX2";
            this.textBoxX2.Properties.ReadOnly = true;
            this.textBoxX2.Size = new System.Drawing.Size(214, 20);
            this.textBoxX2.TabIndex = 3;
            // 
            // labelX2
            // 
            this.labelX2.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Location = new System.Drawing.Point(15, 32);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(84, 14);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "选择存放位置：";
            // 
            // labelX1
            // 
            this.labelX1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(39, 65);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(60, 14);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "生成选项：";
            // 
            // ipanel
            // 
            this.ipanel.Appearance.BackColor = System.Drawing.Color.White;
            this.ipanel.Appearance.Options.UseBackColor = true;
            this.ipanel.Appearance.Options.UseBorderColor = true;
            this.ipanel.Controls.Add(this.myprocess);
            this.ipanel.Controls.Add(this.istate);
            this.ipanel.Location = new System.Drawing.Point(36, 152);
            this.ipanel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.ipanel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.ipanel.Name = "ipanel";
            this.ipanel.Size = new System.Drawing.Size(365, 80);
            this.ipanel.TabIndex = 113;
            this.ipanel.Visible = false;
            // 
            // myprocess
            // 
            this.myprocess.EditValue = 0;
            this.myprocess.Location = new System.Drawing.Point(22, 48);
            this.myprocess.Name = "myprocess";
            this.myprocess.Size = new System.Drawing.Size(323, 10);
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
            this.gridControl_main.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_main.Location = new System.Drawing.Point(7, 39);
            this.gridControl_main.MainView = this.gridControl_main_view;
            this.gridControl_main.Name = "gridControl_main";
            this.gridControl_main.Size = new System.Drawing.Size(416, 309);
            this.gridControl_main.TabIndex = 0;
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
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(12, 43);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(408, 27);
            this.labelControl2.TabIndex = 115;
            this.labelControl2.Text = "拖动鼠标进行多选，Ctrl+鼠标跨行多选，Shift+鼠标区域多选，Ctrl+A全选";
            // 
            // frmSetIndex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 358);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.ipanel);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.panelEx6);
            this.Controls.Add(this.panelEx3);
            this.Controls.Add(this.gridControl_main);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetIndex";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "语料库生成器";
            this.Load += new System.EventHandler(this.SelectClass_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelEx3)).EndInit();
            this.panelEx3.ResumeLayout(false);
            this.panelEx3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx6)).EndInit();
            this.panelEx6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textBoxX1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupPanel1)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.switchButton1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxX2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ipanel)).EndInit();
            this.ipanel.ResumeLayout(false);
            this.ipanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myprocess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main_view)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelEx3;
        private DevExpress.XtraEditors.SimpleButton buttonX1;
        private DevExpress.XtraEditors.PanelControl panelEx6;private DevExpress.XtraEditors.GroupControl groupPanel1;
        private DevExpress.XtraEditors.SimpleButton buttonX4;
        private DevExpress.XtraEditors.TextEdit textBoxX2;
        private DevExpress.XtraEditors.LabelControl labelX2;
        private DevExpress.XtraEditors.LabelControl labelX1;
        private DevExpress.XtraEditors.MemoEdit textBoxX1;
        private DevExpress.XtraEditors.ToggleSwitch switchButton1;
        private DevExpress.XtraEditors.PanelControl ipanel;
        private DevExpress.XtraEditors.MarqueeProgressBarControl myprocess;
        private DevExpress.XtraEditors.LabelControl istate;
        private DevExpress.XtraEditors.LabelControl label_count;
        private DevExpress.XtraGrid.GridControl gridControl_main;
        private DevExpress.XtraGrid.Views.Grid.GridView gridControl_main_view;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}