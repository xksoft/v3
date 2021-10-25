namespace V3.V3Form
{
    partial class frmDownLoadModel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDownLoadModel));
            this.isupdatemymodel = new DevExpress.XtraEditors.ToggleSwitch();
            this.isgetshop = new DevExpress.XtraEditors.ToggleSwitch();
            this.isgetmymodel = new DevExpress.XtraEditors.ToggleSwitch();
            this.labelX2 = new DevExpress.XtraEditors.LabelControl();
            this.labelX1 = new DevExpress.XtraEditors.LabelControl();
            this.panel_bottom = new System.Windows.Forms.Panel();
            this.buttonX2 = new DevExpress.XtraEditors.SimpleButton();
            this.buttonX1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.isupdatemymodel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.isgetshop.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.isgetmymodel.Properties)).BeginInit();
            this.panel_bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // isupdatemymodel
            // 
            this.isupdatemymodel.Location = new System.Drawing.Point(62, 57);
            this.isupdatemymodel.Name = "isupdatemymodel";
            this.isupdatemymodel.Properties.OffText = "如果远程模块已更新，则同步到最新";
            this.isupdatemymodel.Properties.OnText = "弹出窗口询问我是否要更新模块";
            this.isupdatemymodel.Size = new System.Drawing.Size(382, 25);
            this.isupdatemymodel.TabIndex = 48;
            // 
            // isgetshop
            // 
            this.isgetshop.EditValue = true;
            this.isgetshop.Location = new System.Drawing.Point(256, 27);
            this.isgetshop.Name = "isgetshop";
            this.isgetshop.Properties.OffText = "不获取模块市场信息";
            this.isgetshop.Properties.OnText = "获取模块市场信息";
            this.isgetshop.Size = new System.Drawing.Size(180, 25);
            this.isgetshop.TabIndex = 47;
            // 
            // isgetmymodel
            // 
            this.isgetmymodel.EditValue = true;
            this.isgetmymodel.Location = new System.Drawing.Point(62, 27);
            this.isgetmymodel.Name = "isgetmymodel";
            this.isgetmymodel.Properties.OffText = "不获取我的远程模块";
            this.isgetmymodel.Properties.OnText = "获取我的远程模块";
            this.isgetmymodel.Size = new System.Drawing.Size(188, 25);
            this.isgetmymodel.TabIndex = 46;
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(20, 62);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(36, 14);
            this.labelX2.TabIndex = 42;
            this.labelX2.Text = "更新：";
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(20, 32);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(36, 14);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "选项：";
            // 
            // panel_bottom
            // 
            this.panel_bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel_bottom.Controls.Add(this.buttonX2);
            this.panel_bottom.Controls.Add(this.buttonX1);
            this.panel_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_bottom.Location = new System.Drawing.Point(0, 105);
            this.panel_bottom.Name = "panel_bottom";
            this.panel_bottom.Size = new System.Drawing.Size(448, 53);
            this.panel_bottom.TabIndex = 49;
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.Image = ((System.Drawing.Image)(resources.GetObject("buttonX2.Image")));
            this.buttonX2.Location = new System.Drawing.Point(350, 14);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(86, 27);
            this.buttonX2.TabIndex = 45;
            this.buttonX2.Text = "取消同步";
            this.buttonX2.Click += new System.EventHandler(this.buttonX2_Click);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Image = ((System.Drawing.Image)(resources.GetObject("buttonX1.Image")));
            this.buttonX1.Location = new System.Drawing.Point(220, 14);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(86, 27);
            this.buttonX1.TabIndex = 44;
            this.buttonX1.Text = "确定同步";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // frmDownLoadModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 158);
            this.Controls.Add(this.isupdatemymodel);
            this.Controls.Add(this.panel_bottom);
            this.Controls.Add(this.isgetshop);
            this.Controls.Add(this.isgetmymodel);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Office 2013 Light Gray";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDownLoadModel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择要同步的模块类型";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmDownLoadModel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.isupdatemymodel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.isgetshop.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.isgetmymodel.Properties)).EndInit();
            this.panel_bottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelX1;
        private DevExpress.XtraEditors.LabelControl labelX2;
        private DevExpress.XtraEditors.SimpleButton buttonX2;
        private DevExpress.XtraEditors.SimpleButton buttonX1;
        public DevExpress.XtraEditors.ToggleSwitch isupdatemymodel;
        public DevExpress.XtraEditors.ToggleSwitch isgetshop;
        public DevExpress.XtraEditors.ToggleSwitch isgetmymodel;
        private System.Windows.Forms.Panel panel_bottom;
    }
}