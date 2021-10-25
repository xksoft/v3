namespace V3.V3Form.工具窗体
{
    partial class 框
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
            if (disposing && (components != null)){
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
            this.buttonX1 = new DevExpress.XtraEditors.SimpleButton();
            this.buttonX2 = new DevExpress.XtraEditors.SimpleButton();
            this.buttonX3 = new DevExpress.XtraEditors.SimpleButton();
            this.labelX1 = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Location = new System.Drawing.Point(113, 86);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(87, 27);
            this.buttonX1.TabIndex = 0;
            this.buttonX1.Text = "升级";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.Location = new System.Drawing.Point(257, 86);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(87, 27);
            this.buttonX2.TabIndex = 1;
            this.buttonX2.Text = "忽略";
            this.buttonX2.Click += new System.EventHandler(this.buttonX2_Click);
            // 
            // buttonX3
            // 
            this.buttonX3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX3.Location = new System.Drawing.Point(400, 86);
            this.buttonX3.Name = "buttonX3";
            this.buttonX3.Size = new System.Drawing.Size(87, 27);
            this.buttonX3.TabIndex = 2;
            this.buttonX3.Text = "关闭";
            this.buttonX3.Click += new System.EventHandler(this.buttonX3_Click);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(14, 14);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(38, 14);
            this.labelX1.TabIndex = 3;
            this.labelX1.Text = "labelX1";
            // 
            // 框
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 133);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.buttonX3);
            this.Controls.Add(this.buttonX2);
            this.Controls.Add(this.buttonX1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "框";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "框";
            this.Load += new System.EventHandler(this.框_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton buttonX1;
        private DevExpress.XtraEditors.SimpleButton buttonX2;
        private DevExpress.XtraEditors.SimpleButton buttonX3;
        private DevExpress.XtraEditors.LabelControl labelX1;
    }
}