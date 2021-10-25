namespace V3.V3Form
{
    partial class frmResponse
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmResponse));
            this.buttonX1 = new DevExpress.XtraEditors.SimpleButton();
            this.panelEx1 = new DevExpress.XtraEditors.PanelControl();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.richTextBox_Html = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).BeginInit();
            this.panelEx1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonX1.Image = ((System.Drawing.Image)(resources.GetObject("buttonX1.Image")));
            this.buttonX1.Location = new System.Drawing.Point(916, 546);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(87, 27);
            this.buttonX1.TabIndex = 52;
            this.buttonX1.Text = "确定";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // panelEx1
            // 
            this.panelEx1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelEx1.Controls.Add(this.label1);
            this.panelEx1.Controls.Add(this.panel1);
            this.panelEx1.Controls.Add(this.buttonX1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(1016, 585);
            this.panelEx1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(12, 552);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(427, 14);
            this.label1.TabIndex = 54;
            this.label1.Text = "请根据服务器返回的内容查看具体问题然后修改模块或发布点账号、编码等信息";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.richTextBox_Html);
            this.panel1.Location = new System.Drawing.Point(-1, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1018, 532);
            this.panel1.TabIndex = 53;
            // 
            // richTextBox_Html
            // 
            this.richTextBox_Html.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox_Html.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_Html.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_Html.Name = "richTextBox_Html";
            this.richTextBox_Html.Size = new System.Drawing.Size(1016, 530);
            this.richTextBox_Html.TabIndex = 0;
            this.richTextBox_Html.Text = "";
            // 
            // frmResponse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 585);
            this.Controls.Add(this.panelEx1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmResponse";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "服务器返回信息";
            this.Load += new System.EventHandler(this.frmResponse_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton buttonX1;
        private DevExpress.XtraEditors.PanelControl panelEx1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox richTextBox_Html;
        private System.Windows.Forms.Label label1;

    }
}