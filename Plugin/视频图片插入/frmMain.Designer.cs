namespace 视频图片插入
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
            this.label_title = new System.Windows.Forms.Label();
            this.textBox_img_from = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_img_to = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_video_to = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_video_from = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.label_title.Location = new System.Drawing.Point(43, 19);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(41, 12);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "图片：";
            // 
            // textBox_title_from
            // 
            this.textBox_img_from.Location = new System.Drawing.Point(89, 17);
            this.textBox_img_from.Name = "textBox_title_from";
            this.textBox_img_from.Size = new System.Drawing.Size(40, 21);
            this.textBox_img_from.TabIndex = 2;
            this.textBox_img_from.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(135, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "到";
            // 
            // textBox_title_to
            // 
            this.textBox_img_to.Location = new System.Drawing.Point(158, 17);
            this.textBox_img_to.Name = "textBox_title_to";
            this.textBox_img_to.Size = new System.Drawing.Size(40, 21);
            this.textBox_img_to.TabIndex = 4;
            this.textBox_img_to.Text = "2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(204, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "个";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(204, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "个";
            // 
            // textBox_keyword_to
            // 
            this.textBox_video_to.Location = new System.Drawing.Point(158, 55);
            this.textBox_video_to.Name = "textBox_keyword_to";
            this.textBox_video_to.Size = new System.Drawing.Size(40, 21);
            this.textBox_video_to.TabIndex = 10;
            this.textBox_video_to.Text = "2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(135, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "到";
            // 
            // textBox_keyword_from
            // 
            this.textBox_video_from.Location = new System.Drawing.Point(89, 55);
            this.textBox_video_from.Name = "textBox_keyword_from";
            this.textBox_video_from.Size = new System.Drawing.Size(40, 21);
            this.textBox_video_from.TabIndex = 8;
            this.textBox_video_from.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(43, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "视频：";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_video_to);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_video_from);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_img_to);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_img_from);
            this.Controls.Add(this.label_title);
            this.Name = "frmMain";
            this.Size = new System.Drawing.Size(433, 241);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox textBox_img_from;
        public System.Windows.Forms.TextBox textBox_img_to;
        public System.Windows.Forms.TextBox textBox_video_to;
        public System.Windows.Forms.TextBox textBox_video_from;
    }
}
