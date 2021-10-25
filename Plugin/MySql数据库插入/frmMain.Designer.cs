namespace MySql数据库插入
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
            this.textBox_Sql = new System.Windows.Forms.TextBox();
            this.linkLabel_model = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ConnStr = new System.Windows.Forms.TextBox();
            this.button_test = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_Sql
            // 
            this.textBox_Sql.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_Sql.Location = new System.Drawing.Point(121, 92);
            this.textBox_Sql.Multiline = true;
            this.textBox_Sql.Name = "textBox_Sql";
            this.textBox_Sql.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Sql.Size = new System.Drawing.Size(418, 60);
            this.textBox_Sql.TabIndex = 1;
            this.textBox_Sql.Text = "insert into tb_article values(\'[模型值1]\',\'[模型值2]\')";
            // 
            // linkLabel_model
            // 
            this.linkLabel_model.AutoSize = true;
            this.linkLabel_model.Location = new System.Drawing.Point(119, 166);
            this.linkLabel_model.Name = "linkLabel_model";
            this.linkLabel_model.Size = new System.Drawing.Size(77, 12);
            this.linkLabel_model.TabIndex = 2;
            this.linkLabel_model.TabStop = true;
            this.linkLabel_model.Text = "[模型值1-30]";
            this.linkLabel_model.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_model_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "插入语句格式：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "数据库连接字符串：";
            // 
            // textBox_ConnStr
            // 
            this.textBox_ConnStr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_ConnStr.Location = new System.Drawing.Point(121, 22);
            this.textBox_ConnStr.Multiline = true;
            this.textBox_ConnStr.Name = "textBox_ConnStr";
            this.textBox_ConnStr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_ConnStr.Size = new System.Drawing.Size(345, 64);
            this.textBox_ConnStr.TabIndex = 9;
            this.textBox_ConnStr.Text = "server=服务器地址;database=数据库名;uid=MySql账号;pwd=MySql密码;";
            // 
            // button_test
            // 
            this.button_test.Location = new System.Drawing.Point(472, 20);
            this.button_test.Name = "button_test";
            this.button_test.Size = new System.Drawing.Size(67, 66);
            this.button_test.TabIndex = 10;
            this.button_test.Text = "测试连接";
            this.button_test.UseVisualStyleBackColor = true;
            this.button_test.Click += new System.EventHandler(this.button_test_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(119, 192);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(353, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "请在使用该插件前提前备份好数据库，否则造成的后果与本人无关";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_test);
            this.Controls.Add(this.textBox_ConnStr);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkLabel_model);
            this.Controls.Add(this.textBox_Sql);
            this.Name = "frmMain";
            this.Size = new System.Drawing.Size(578, 228);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox textBox_Sql;
        private System.Windows.Forms.LinkLabel linkLabel_model;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox textBox_ConnStr;
        private System.Windows.Forms.Button button_test;
        private System.Windows.Forms.Label label3;

    }
}
