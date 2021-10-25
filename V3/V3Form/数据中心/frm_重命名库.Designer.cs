namespace V3.V3Form
{
    partial class frm_重命名库
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_重命名库));
            this.panelEx1 = new DevExpress.XtraEditors.PanelControl();
            this.buttonX1 = new DevExpress.XtraEditors.SimpleButton();
            this.name = new DevExpress.XtraEditors.TextEdit();
            this.labelX1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).BeginInit();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.name.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.Controls.Add(this.buttonX1);
            this.panelEx1.Controls.Add(this.name);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(465, 51);
            this.panelEx1.TabIndex = 0;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Image = ((System.Drawing.Image)(resources.GetObject("buttonX1.Image")));
            this.buttonX1.Location = new System.Drawing.Point(356, 14);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(87, 21);
            this.buttonX1.TabIndex = 2;
            this.buttonX1.Text = "确定";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(64, 14);
            this.name.Name = "name";
            this.name.Properties.NullValuePrompt = "请输入一个新库名....";
            this.name.Size = new System.Drawing.Size(279, 20);
            this.name.TabIndex = 1;
            this.name.KeyDown += new System.Windows.Forms.KeyEventHandler(this.name_KeyDown);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(14, 16);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(48, 14);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "分组名：";
            // 
            // frm_重命名库
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 51);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_重命名库";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "重命名库名称";
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.name.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelEx1;
        private DevExpress.XtraEditors.SimpleButton buttonX1;
        public DevExpress.XtraEditors.TextEdit name;
        private DevExpress.XtraEditors.LabelControl labelX1;
    }
}