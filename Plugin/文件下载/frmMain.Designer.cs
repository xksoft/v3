namespace 文件下载
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
            this.label_type = new System.Windows.Forms.Label();
            this.checkBox_type_image = new System.Windows.Forms.CheckBox();
            this.checkBox_type_video = new System.Windows.Forms.CheckBox();
            this.checkBox_type_all = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.button_selectpath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox_resetUrl = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_tip = new System.Windows.Forms.Label();
            this.button_test = new System.Windows.Forms.Button();
            this.textBox_ftpServer = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox_upload = new System.Windows.Forms.CheckBox();
            this.textBox_ftpPath = new System.Windows.Forms.TextBox();
            this.textBox_ftpPassword = new System.Windows.Forms.TextBox();
            this.textBox_ftpUser = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Model = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_DirName = new System.Windows.Forms.TextBox();
            this.linkLabel_ymd = new System.Windows.Forms.LinkLabel();
            this.checkBox_delete = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_type
            // 
            this.label_type.AutoSize = true;
            this.label_type.Location = new System.Drawing.Point(2, 33);
            this.label_type.Name = "label_type";
            this.label_type.Size = new System.Drawing.Size(65, 12);
            this.label_type.TabIndex = 0;
            this.label_type.Text = "文件类型：";
            // 
            // checkBox_type_image
            // 
            this.checkBox_type_image.AutoSize = true;
            this.checkBox_type_image.Checked = true;
            this.checkBox_type_image.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_type_image.Location = new System.Drawing.Point(69, 31);
            this.checkBox_type_image.Name = "checkBox_type_image";
            this.checkBox_type_image.Size = new System.Drawing.Size(48, 16);
            this.checkBox_type_image.TabIndex = 1;
            this.checkBox_type_image.Text = "图片";
            this.checkBox_type_image.UseVisualStyleBackColor = true;
            // 
            // checkBox_type_video
            // 
            this.checkBox_type_video.AutoSize = true;
            this.checkBox_type_video.Location = new System.Drawing.Point(135, 31);
            this.checkBox_type_video.Name = "checkBox_type_video";
            this.checkBox_type_video.Size = new System.Drawing.Size(48, 16);
            this.checkBox_type_video.TabIndex = 2;
            this.checkBox_type_video.Text = "视频";
            this.checkBox_type_video.UseVisualStyleBackColor = true;
            // 
            // checkBox_type_all
            // 
            this.checkBox_type_all.AutoSize = true;
            this.checkBox_type_all.Location = new System.Drawing.Point(201, 31);
            this.checkBox_type_all.Name = "checkBox_type_all";
            this.checkBox_type_all.Size = new System.Drawing.Size(72, 16);
            this.checkBox_type_all.TabIndex = 3;
            this.checkBox_type_all.Text = "所有文件";
            this.checkBox_type_all.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "保存位置：";
            // 
            // textBox_path
            // 
            this.textBox_path.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_path.Location = new System.Drawing.Point(69, 56);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.Size = new System.Drawing.Size(204, 21);
            this.textBox_path.TabIndex = 5;
            this.textBox_path.Text = "C:\\\\V3Data";
            // 
            // button_selectpath
            // 
            this.button_selectpath.Location = new System.Drawing.Point(463, 55);
            this.button_selectpath.Name = "button_selectpath";
            this.button_selectpath.Size = new System.Drawing.Size(72, 23);
            this.button_selectpath.TabIndex = 6;
            this.button_selectpath.Text = "选择目录";
            this.button_selectpath.UseVisualStyleBackColor = true;
            this.button_selectpath.Click += new System.EventHandler(this.button_selectpath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "处理配置：";
            // 
            // checkBox_resetUrl
            // 
            this.checkBox_resetUrl.AutoSize = true;
            this.checkBox_resetUrl.Checked = true;
            this.checkBox_resetUrl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_resetUrl.Location = new System.Drawing.Point(69, 86);
            this.checkBox_resetUrl.Name = "checkBox_resetUrl";
            this.checkBox_resetUrl.Size = new System.Drawing.Size(264, 16);
            this.checkBox_resetUrl.TabIndex = 8;
            this.checkBox_resetUrl.Text = "重构原路径（将原路径替换成新的相对路径）";
            this.checkBox_resetUrl.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_delete);
            this.groupBox1.Controls.Add(this.label_tip);
            this.groupBox1.Controls.Add(this.button_test);
            this.groupBox1.Controls.Add(this.textBox_ftpServer);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.checkBox_upload);
            this.groupBox1.Controls.Add(this.textBox_ftpPath);
            this.groupBox1.Controls.Add(this.textBox_ftpPassword);
            this.groupBox1.Controls.Add(this.textBox_ftpUser);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(4, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(541, 163);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // label_tip
            // 
            this.label_tip.AutoSize = true;
            this.label_tip.ForeColor = System.Drawing.Color.Red;
            this.label_tip.Location = new System.Drawing.Point(81, 146);
            this.label_tip.Name = "label_tip";
            this.label_tip.Size = new System.Drawing.Size(431, 12);
            this.label_tip.TabIndex = 13;
            this.label_tip.Text = "注意：请将FTP指向网站根目录，并保持保存位置的后半部分和网站附件目录一致";
            // 
            // button_test
            // 
            this.button_test.Location = new System.Drawing.Point(459, 28);
            this.button_test.Name = "button_test";
            this.button_test.Size = new System.Drawing.Size(72, 23);
            this.button_test.TabIndex = 12;
            this.button_test.Text = "测试连接";
            this.button_test.UseVisualStyleBackColor = true;
            this.button_test.Click += new System.EventHandler(this.button_test_Click);
            // 
            // textBox_ftpServer
            // 
            this.textBox_ftpServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_ftpServer.Location = new System.Drawing.Point(80, 29);
            this.textBox_ftpServer.Name = "textBox_ftpServer";
            this.textBox_ftpServer.Size = new System.Drawing.Size(373, 21);
            this.textBox_ftpServer.TabIndex = 11;
            this.textBox_ftpServer.Text = "ftp://www.xiake.org";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "服务器：";
            // 
            // checkBox_upload
            // 
            this.checkBox_upload.AutoSize = true;
            this.checkBox_upload.Checked = true;
            this.checkBox_upload.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_upload.Location = new System.Drawing.Point(17, 0);
            this.checkBox_upload.Name = "checkBox_upload";
            this.checkBox_upload.Size = new System.Drawing.Size(228, 16);
            this.checkBox_upload.TabIndex = 9;
            this.checkBox_upload.Text = "自动上传（启用时任务必须进行发布）";
            this.checkBox_upload.UseVisualStyleBackColor = true;
            // 
            // textBox_ftpPath
            // 
            this.textBox_ftpPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_ftpPath.Location = new System.Drawing.Point(80, 121);
            this.textBox_ftpPath.Name = "textBox_ftpPath";
            this.textBox_ftpPath.Size = new System.Drawing.Size(373, 21);
            this.textBox_ftpPath.TabIndex = 8;
            this.textBox_ftpPath.Text = "/";
            // 
            // textBox_ftpPassword
            // 
            this.textBox_ftpPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_ftpPassword.Location = new System.Drawing.Point(80, 91);
            this.textBox_ftpPassword.Name = "textBox_ftpPassword";
            this.textBox_ftpPassword.Size = new System.Drawing.Size(373, 21);
            this.textBox_ftpPassword.TabIndex = 7;
            this.textBox_ftpPassword.Text = "123456";
            // 
            // textBox_ftpUser
            // 
            this.textBox_ftpUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_ftpUser.Location = new System.Drawing.Point(80, 60);
            this.textBox_ftpUser.Name = "textBox_ftpUser";
            this.textBox_ftpUser.Size = new System.Drawing.Size(373, 21);
            this.textBox_ftpUser.TabIndex = 6;
            this.textBox_ftpUser.Text = "user";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "上传目录：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "FTP密码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "FTP账号：";
            // 
            // textBox_Model
            // 
            this.textBox_Model.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_Model.Location = new System.Drawing.Point(69, 0);
            this.textBox_Model.Name = "textBox_Model";
            this.textBox_Model.Size = new System.Drawing.Size(311, 21);
            this.textBox_Model.TabIndex = 12;
            this.textBox_Model.Text = "[模型值2]";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(2, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = "下载模型：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(386, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(149, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "支持[模型值1]-[模型值30]";
            // 
            // textBox_DirName
            // 
            this.textBox_DirName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_DirName.Location = new System.Drawing.Point(279, 56);
            this.textBox_DirName.Name = "textBox_DirName";
            this.textBox_DirName.Size = new System.Drawing.Size(178, 21);
            this.textBox_DirName.TabIndex = 14;
            this.textBox_DirName.Text = "images\\\\[年月日]";
            // 
            // linkLabel_ymd
            // 
            this.linkLabel_ymd.AutoSize = true;
            this.linkLabel_ymd.Location = new System.Drawing.Point(404, 80);
            this.linkLabel_ymd.Name = "linkLabel_ymd";
            this.linkLabel_ymd.Size = new System.Drawing.Size(53, 12);
            this.linkLabel_ymd.TabIndex = 15;
            this.linkLabel_ymd.TabStop = true;
            this.linkLabel_ymd.Text = "[年月日]";
            this.linkLabel_ymd.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_ymd_LinkClicked);
            // 
            // checkBox_delete
            // 
            this.checkBox_delete.AutoSize = true;
            this.checkBox_delete.Location = new System.Drawing.Point(276, 1);
            this.checkBox_delete.Name = "checkBox_delete";
            this.checkBox_delete.Size = new System.Drawing.Size(180, 16);
            this.checkBox_delete.TabIndex = 14;
            this.checkBox_delete.Text = "上传成功后删除本地缓存文件";
            this.checkBox_delete.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkLabel_ymd);
            this.Controls.Add(this.textBox_DirName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox_Model);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBox_resetUrl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_selectpath);
            this.Controls.Add(this.textBox_path);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox_type_all);
            this.Controls.Add(this.checkBox_type_video);
            this.Controls.Add(this.checkBox_type_image);
            this.Controls.Add(this.label_type);
            this.Name = "frmMain";
            this.Size = new System.Drawing.Size(561, 290);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_type;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_selectpath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_test;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.TextBox textBox_Model;
        public System.Windows.Forms.CheckBox checkBox_type_image;
        public System.Windows.Forms.CheckBox checkBox_type_video;
        public System.Windows.Forms.CheckBox checkBox_type_all;
        public System.Windows.Forms.TextBox textBox_path;
        public System.Windows.Forms.CheckBox checkBox_resetUrl;
        public System.Windows.Forms.CheckBox checkBox_upload;
        public System.Windows.Forms.TextBox textBox_ftpPath;
        public System.Windows.Forms.TextBox textBox_ftpPassword;
        public System.Windows.Forms.TextBox textBox_ftpUser;
        public System.Windows.Forms.TextBox textBox_ftpServer;
        public System.Windows.Forms.TextBox textBox_DirName;
        private System.Windows.Forms.LinkLabel linkLabel_ymd;
        private System.Windows.Forms.Label label_tip;
        public System.Windows.Forms.CheckBox checkBox_delete;
    }
}
