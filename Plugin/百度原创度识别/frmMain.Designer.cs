namespace 百度原创度识别
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
            this.textBox_proxy = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_maxCount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_proxy
            // 
            this.textBox_proxy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_proxy.Location = new System.Drawing.Point(10, 62);
            this.textBox_proxy.Multiline = true;
            this.textBox_proxy.Name = "textBox_proxy";
            this.textBox_proxy.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_proxy.Size = new System.Drawing.Size(514, 128);
            this.textBox_proxy.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(503, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "请输可用的代理IP列表，被百度屏蔽时将自动调用（一行一个，格式如：134.45.45.67:8080）";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "最大结果数量：";
            // 
            // textBox_maxCount
            // 
            this.textBox_maxCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_maxCount.Location = new System.Drawing.Point(91, 3);
            this.textBox_maxCount.Name = "textBox_maxCount";
            this.textBox_maxCount.Size = new System.Drawing.Size(59, 21);
            this.textBox_maxCount.TabIndex = 4;
            this.textBox_maxCount.Text = "10";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(9, 198);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(515, 60);
            this.label3.TabIndex = 5;
            this.label3.Text = "原理：将标题（[模型值1]）在百度中加双引号搜索，获取到结果数量超过一定范围则认为该文章\r\n\r\n已有大量网站使用，则将标题和内容设置为空触发不保存，所以必须在采集" +
    "时处理，可搭配其他\r\n\r\n标题处理功能使用，模型值25保存的是搜索到的结果数量。";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(156, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(257, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "超过该值即认为文章无效，自动将标题设置为空";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_maxCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_proxy);
            this.Name = "frmMain";
            this.Size = new System.Drawing.Size(561, 290);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox textBox_proxy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox textBox_maxCount;

    }
}
