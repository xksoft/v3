namespace 百度翻译
{
    partial class frmMain
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ApiKey = new System.Windows.Forms.TextBox();
            this.textBox_SecretKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButton_cn = new System.Windows.Forms.RadioButton();
            this.radioButton_en = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "API Key：";
            // 
            // textBox_ApiKey
            // 
            this.textBox_ApiKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_ApiKey.Location = new System.Drawing.Point(81, 3);
            this.textBox_ApiKey.Name = "textBox_ApiKey";
            this.textBox_ApiKey.Size = new System.Drawing.Size(413, 21);
            this.textBox_ApiKey.TabIndex = 4;
            this.textBox_ApiKey.Text = "10";
            // 
            // textBox_SecretKey
            // 
            this.textBox_SecretKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_SecretKey.Location = new System.Drawing.Point(81, 46);
            this.textBox_SecretKey.Name = "textBox_SecretKey";
            this.textBox_SecretKey.Size = new System.Drawing.Size(413, 21);
            this.textBox_SecretKey.TabIndex = 6;
            this.textBox_SecretKey.Text = "10";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Secret Key：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "目标语言：";
            // 
            // radioButton_cn
            // 
            this.radioButton_cn.AutoSize = true;
            this.radioButton_cn.Checked = true;
            this.radioButton_cn.Location = new System.Drawing.Point(81, 84);
            this.radioButton_cn.Name = "radioButton_cn";
            this.radioButton_cn.Size = new System.Drawing.Size(47, 16);
            this.radioButton_cn.TabIndex = 8;
            this.radioButton_cn.TabStop = true;
            this.radioButton_cn.Text = "中文";
            this.radioButton_cn.UseVisualStyleBackColor = true;
            // 
            // radioButton_en
            // 
            this.radioButton_en.AutoSize = true;
            this.radioButton_en.Location = new System.Drawing.Point(145, 84);
            this.radioButton_en.Name = "radioButton_en";
            this.radioButton_en.Size = new System.Drawing.Size(47, 16);
            this.radioButton_en.TabIndex = 9;
            this.radioButton_en.Text = "英文";
            this.radioButton_en.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radioButton_en);
            this.Controls.Add(this.radioButton_cn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_SecretKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_ApiKey);
            this.Controls.Add(this.label2);
            this.Name = "frmMain";
            this.Size = new System.Drawing.Size(561, 290);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox textBox_ApiKey;
        public System.Windows.Forms.TextBox textBox_SecretKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.RadioButton radioButton_cn;
        public System.Windows.Forms.RadioButton radioButton_en;
    }
}
