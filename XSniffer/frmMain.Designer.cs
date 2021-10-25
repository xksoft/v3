namespace XSniffer
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btn_start = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btn_Type = new System.Windows.Forms.ToolStripDropDownButton();
            this.仅GETToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.仅POSTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Get和PostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.textBox_regex = new System.Windows.Forms.ToolStripTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView_main = new XSniffer.DoubleBufferListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "侠客HTTP抓包工具";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_start,
            this.toolStripButton3,
            this.toolStripSeparator1,
            this.btn_Type,
            this.toolStripLabel1,
            this.textBox_regex});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(0, 2, 1, 0);
            this.toolStrip.Size = new System.Drawing.Size(640, 26);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            this.toolStrip.MouseEnter += new System.EventHandler(this.toolStrip_MouseEnter);
            // 
            // btn_start
            // 
            this.btn_start.AutoToolTip = false;
            this.btn_start.Image = global::XSniffer.Properties.Resources.btn_start;
            this.btn_start.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(76, 21);
            this.btn_start.Text = "开始监听";
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.AutoToolTip = false;
            this.toolStripButton3.Image = global::XSniffer.Properties.Resources.btn_clear;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(76, 21);
            this.toolStripButton3.Text = "清空列表";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 24);
            // 
            // btn_Type
            // 
            this.btn_Type.AutoToolTip = false;
            this.btn_Type.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_Type.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.仅GETToolStripMenuItem,
            this.仅POSTToolStripMenuItem,
            this.Get和PostToolStripMenuItem});
            this.btn_Type.Image = ((System.Drawing.Image)(resources.GetObject("btn_Type.Image")));
            this.btn_Type.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Type.Name = "btn_Type";
            this.btn_Type.Size = new System.Drawing.Size(78, 21);
            this.btn_Type.Text = "Get和Post";
            // 
            // 仅GETToolStripMenuItem
            // 
            this.仅GETToolStripMenuItem.Name = "仅GETToolStripMenuItem";
            this.仅GETToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.仅GETToolStripMenuItem.Text = "仅Get";
            this.仅GETToolStripMenuItem.Click += new System.EventHandler(this.仅GETToolStripMenuItem_Click);
            // 
            // 仅POSTToolStripMenuItem
            // 
            this.仅POSTToolStripMenuItem.Name = "仅POSTToolStripMenuItem";
            this.仅POSTToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.仅POSTToolStripMenuItem.Text = "仅Post";
            this.仅POSTToolStripMenuItem.Click += new System.EventHandler(this.仅POSTToolStripMenuItem_Click);
            // 
            // Get和PostToolStripMenuItem
            // 
            this.Get和PostToolStripMenuItem.Name = "Get和PostToolStripMenuItem";
            this.Get和PostToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.Get和PostToolStripMenuItem.Text = "Get和Post";
            this.Get和PostToolStripMenuItem.Click += new System.EventHandler(this.Get和PostToolStripMenuItem_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(92, 21);
            this.toolStripLabel1.Text = "地址过滤正则：";
            // 
            // textBox_regex
            // 
            this.textBox_regex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_regex.Name = "textBox_regex";
            this.textBox_regex.Size = new System.Drawing.Size(300, 24);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 26);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView_main);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(640, 266);
            this.splitContainer1.SplitterDistance = 122;
            this.splitContainer1.TabIndex = 1;
            // 
            // listView_main
            // 
            this.listView_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_main.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView_main.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView_main.FullRowSelect = true;
            this.listView_main.Location = new System.Drawing.Point(-1, 2);
            this.listView_main.Name = "listView_main";
            this.listView_main.Size = new System.Drawing.Size(642, 118);
            this.listView_main.TabIndex = 0;
            this.listView_main.UseCompatibleStateImageBehavior = false;
            this.listView_main.View = System.Windows.Forms.View.Details;
            this.listView_main.SelectedIndexChanged += new System.EventHandler(this.listView_main_SelectedIndexChanged);
            this.listView_main.MouseEnter += new System.EventHandler(this.listView_main_MouseEnter);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "动作";
            this.columnHeader2.Width = 50;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "地址";
            this.columnHeader3.Width = 500;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.richTextBox);
            this.panel1.Location = new System.Drawing.Point(-1, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(642, 139);
            this.panel1.TabIndex = 0;
            // 
            // richTextBox
            // 
            this.richTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox.Location = new System.Drawing.Point(0, 0);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(640, 137);
            this.richTextBox.TabIndex = 0;
            this.richTextBox.Text = "";
            this.richTextBox.MouseEnter += new System.EventHandler(this.richTextBox_MouseEnter);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 292);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "侠客HTTP抓包工具 - www.xksoft.com";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.MouseEnter += new System.EventHandler(this.frmMain_MouseEnter);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DoubleBufferListView listView_main;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolStripButton btn_start;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox textBox_regex;
        private System.Windows.Forms.ToolStripDropDownButton btn_Type;
        private System.Windows.Forms.ToolStripMenuItem 仅GETToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 仅POSTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Get和PostToolStripMenuItem;
    }
}

