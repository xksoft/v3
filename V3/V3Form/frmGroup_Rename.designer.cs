namespace V3.V3Form
{
    partial class frmGroup_Rename
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGroup_Rename));
            this.buttonX1 = new DevExpress.XtraEditors.SimpleButton();
            this.name = new DevExpress.XtraEditors.TextEdit();
            this.labelX1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.name.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Image = ((System.Drawing.Image)(resources.GetObject("buttonX1.Image")));
            this.buttonX1.Location = new System.Drawing.Point(375, 27);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(87, 22);
            this.buttonX1.TabIndex = 2;
            this.buttonX1.Text = "确定";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(65, 28);
            this.name.Name = "name";
            this.name.Properties.NullValuePrompt = "请输入一个分组名";
            this.name.Size = new System.Drawing.Size(304, 20);
            this.name.TabIndex = 1;
            this.name.KeyDown += new System.Windows.Forms.KeyEventHandler(this.name_KeyDown);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 31);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(48, 14);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "分组名：";
            // 
            // frmGroup_Rename
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 79);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.name);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGroup_Rename";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "分组编辑";
            ((System.ComponentModel.ISupportInitialize)(this.name.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton buttonX1;
        public DevExpress.XtraEditors.TextEdit name;
        private DevExpress.XtraEditors.LabelControl labelX1;
    }
}