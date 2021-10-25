namespace V3.V3Form
{
    partial class frm_新增库
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_新增库));
            this.groupPanel1 = new DevExpress.XtraEditors.GroupControl();
            this.checkBoxX5 = new DevExpress.XtraEditors.CheckEdit();
            this.checkBoxX4 = new DevExpress.XtraEditors.CheckEdit();
            this.checkBoxX3 = new DevExpress.XtraEditors.CheckEdit();
            this.checkBoxX2 = new DevExpress.XtraEditors.CheckEdit();
            this.checkBoxX1 = new DevExpress.XtraEditors.CheckEdit();
            this.labelX2 = new DevExpress.XtraEditors.LabelControl();
            this.textBoxX1 = new DevExpress.XtraEditors.TextEdit();
            this.labelX1 = new DevExpress.XtraEditors.LabelControl();
            this.cmbGroup = new DevExpress.XtraEditors.LookUpEdit();
            this.buttonX2 = new DevExpress.XtraEditors.SimpleButton();
            this.buttonX1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupPanel1)).BeginInit();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxX5.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxX4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxX3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxX2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxX1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxX1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGroup.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.checkBoxX5);
            this.groupPanel1.Controls.Add(this.checkBoxX4);
            this.groupPanel1.Controls.Add(this.checkBoxX3);
            this.groupPanel1.Controls.Add(this.checkBoxX2);
            this.groupPanel1.Controls.Add(this.checkBoxX1);
            this.groupPanel1.Location = new System.Drawing.Point(19, 83);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(458, 70);
            this.groupPanel1.TabIndex = 4;
            this.groupPanel1.Text = "同时添加以下同名的库";
            // 
            // checkBoxX5
            // 
            this.checkBoxX5.Location = new System.Drawing.Point(366, 32);
            this.checkBoxX5.Name = "checkBoxX5";
            this.checkBoxX5.Properties.Caption = "链接库";
            this.checkBoxX5.Size = new System.Drawing.Size(83, 19);
            this.checkBoxX5.TabIndex = 7;
            // 
            // checkBoxX4
            // 
            this.checkBoxX4.Location = new System.Drawing.Point(278, 32);
            this.checkBoxX4.Name = "checkBoxX4";
            this.checkBoxX4.Properties.Caption = "替换库";
            this.checkBoxX4.Size = new System.Drawing.Size(83, 19);
            this.checkBoxX4.TabIndex = 6;
            // 
            // checkBoxX3
            // 
            this.checkBoxX3.Location = new System.Drawing.Point(191, 32);
            this.checkBoxX3.Name = "checkBoxX3";
            this.checkBoxX3.Properties.Caption = "哈希库";
            this.checkBoxX3.Size = new System.Drawing.Size(83, 19);
            this.checkBoxX3.TabIndex = 5;
            // 
            // checkBoxX2
            // 
            this.checkBoxX2.Location = new System.Drawing.Point(95, 32);
            this.checkBoxX2.Name = "checkBoxX2";
            this.checkBoxX2.Properties.Caption = "关键词库";
            this.checkBoxX2.Size = new System.Drawing.Size(91, 19);
            this.checkBoxX2.TabIndex = 4;
            // 
            // checkBoxX1
            // 
            this.checkBoxX1.Location = new System.Drawing.Point(8, 32);
            this.checkBoxX1.Name = "checkBoxX1";
            this.checkBoxX1.Properties.Caption = "文章库";
            this.checkBoxX1.Size = new System.Drawing.Size(83, 19);
            this.checkBoxX1.TabIndex = 3;
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(19, 49);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(36, 14);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "分组：";
            // 
            // textBoxX1
            // 
            this.textBoxX1.Location = new System.Drawing.Point(61, 18);
            this.textBoxX1.Name = "textBoxX1";
            this.textBoxX1.Properties.NullValuePrompt = "请输入您要新增的库名....";
            this.textBoxX1.Size = new System.Drawing.Size(416, 20);
            this.textBoxX1.TabIndex = 1;
            this.textBoxX1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxX1_KeyDown);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(19, 18);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(36, 14);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "库名：";
            // 
            // cmbGroup
            // 
            this.cmbGroup.Location = new System.Drawing.Point(61, 49);
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down)});
            this.cmbGroup.Properties.NullText = "";
            this.cmbGroup.Properties.NullValuePrompt = "请选择要添加的分组...";
            this.cmbGroup.Properties.PopupSizeable = false;
            this.cmbGroup.Properties.ShowFooter = false;
            this.cmbGroup.Properties.ShowHeader = false;
            this.cmbGroup.Properties.ShowLines = false;
            this.cmbGroup.Size = new System.Drawing.Size(416, 20);
            this.cmbGroup.TabIndex = 10;
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.Image = ((System.Drawing.Image)(resources.GetObject("buttonX2.Image")));
            this.buttonX2.Location = new System.Drawing.Point(390, 163);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(87, 27);
            this.buttonX2.TabIndex = 9;
            this.buttonX2.Text = "取消";
            this.buttonX2.Click += new System.EventHandler(this.buttonX2_Click);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Image = ((System.Drawing.Image)(resources.GetObject("buttonX1.Image")));
            this.buttonX1.Location = new System.Drawing.Point(205, 163);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(87, 27);
            this.buttonX1.TabIndex = 8;
            this.buttonX1.Text = "新增";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // frm_新增库
            // 
            this.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 210);
            this.Controls.Add(this.buttonX2);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.textBoxX1);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.cmbGroup);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_新增库";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "新增库";
            this.Load += new System.EventHandler(this.frm_新增库_Load);
            this.Shown += new System.EventHandler(this.frm_新增库_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.groupPanel1)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxX5.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxX4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxX3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxX2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxX1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxX1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGroup.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupPanel1;
        private DevExpress.XtraEditors.CheckEdit checkBoxX5;
        private DevExpress.XtraEditors.CheckEdit checkBoxX4;
        private DevExpress.XtraEditors.CheckEdit checkBoxX3;
        private DevExpress.XtraEditors.CheckEdit checkBoxX2;
        private DevExpress.XtraEditors.CheckEdit checkBoxX1;
        private DevExpress.XtraEditors.LabelControl labelX2;
        private DevExpress.XtraEditors.TextEdit textBoxX1;
        private DevExpress.XtraEditors.LabelControl labelX1;
        private DevExpress.XtraEditors.SimpleButton buttonX2;
        private DevExpress.XtraEditors.SimpleButton buttonX1;
   
        public DevExpress.XtraEditors.LookUpEdit cmbGroup;
    }
}