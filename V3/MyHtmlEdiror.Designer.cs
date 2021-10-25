namespace V3
{
    partial class MyHtmlEdiror
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panelEx1 = new DevExpress.XtraEditors.PanelControl();
            this.myRichTextBox1 = new xEngine.UI.MyControl.MyRichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).BeginInit();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelEx1.Controls.Add(this.myRichTextBox1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Padding = new System.Windows.Forms.Padding(1);
            this.panelEx1.Size = new System.Drawing.Size(150, 150);
            this.panelEx1.TabIndex = 0;
            // 
            // myRichTextBox1
            // 
            this.myRichTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(251)))), ((int)(((byte)(251)))));
            this.myRichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.myRichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myRichTextBox1.Location = new System.Drawing.Point(1, 1);
            this.myRichTextBox1.Name = "myRichTextBox1";
            this.myRichTextBox1.Size = new System.Drawing.Size(148, 148);
            this.myRichTextBox1.TabIndex = 0;
            this.myRichTextBox1.Text = "";
            // 
            // MyHtmlEdiror
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEx1);
            this.Name = "MyHtmlEdiror";
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelEx1;
        private xEngine.UI.MyControl.MyRichTextBox myRichTextBox1;
    }
}
