namespace V3.V3Form.发布模块
{
    partial class UserInput
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInput));
            this.panelEx1 = new DevExpress.XtraEditors.PanelControl();
            this.buttonX2 = new DevExpress.XtraEditors.SimpleButton();
            this.buttonX1 = new DevExpress.XtraEditors.SimpleButton();
            this.value2 = new DevExpress.XtraEditors.TextEdit();
            this.value1 = new DevExpress.XtraEditors.TextEdit();
            this.password = new DevExpress.XtraEditors.TextEdit();
            this.username = new DevExpress.XtraEditors.TextEdit();
            this.labelX4 = new DevExpress.XtraEditors.LabelControl();
            this.labelX3 = new DevExpress.XtraEditors.LabelControl();
            this.labelX2 = new DevExpress.XtraEditors.LabelControl();
            this.labelX1 = new DevExpress.XtraEditors.LabelControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar4 = new DevExpress.XtraBars.Bar();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).BeginInit();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.value2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.value1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.password.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.username.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelEx1.Controls.Add(this.buttonX2);
            this.panelEx1.Controls.Add(this.buttonX1);
            this.panelEx1.Controls.Add(this.value2);
            this.panelEx1.Controls.Add(this.value1);
            this.panelEx1.Controls.Add(this.password);
            this.panelEx1.Controls.Add(this.username);
            this.panelEx1.Controls.Add(this.labelX4);
            this.panelEx1.Controls.Add(this.labelX3);
            this.panelEx1.Controls.Add(this.labelX2);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(472, 198);
            this.panelEx1.TabIndex = 0;
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.Image = ((System.Drawing.Image)(resources.GetObject("buttonX2.Image")));
            this.buttonX2.Location = new System.Drawing.Point(364, 149);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(87, 27);
            this.buttonX2.TabIndex = 53;
            this.buttonX2.Text = "取消";
            this.buttonX2.Click += new System.EventHandler(this.buttonX2_Click);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Image = ((System.Drawing.Image)(resources.GetObject("buttonX1.Image")));
            this.buttonX1.Location = new System.Drawing.Point(200, 149);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(87, 27);
            this.buttonX1.TabIndex = 52;
            this.buttonX1.Text = "保存";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // value2
            // 
            this.value2.Location = new System.Drawing.Point(101, 117);
            this.value2.Name = "value2";
            this.value2.Properties.NullValuePrompt = "这里填写附加值2，也可以留空...";
            this.value2.Size = new System.Drawing.Size(350, 20);
            this.value2.TabIndex = 51;
            this.value2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.username_KeyDown);
            // 
            // value1
            // 
            this.value1.Location = new System.Drawing.Point(101, 86);
            this.value1.Name = "value1";
            this.value1.Properties.NullValuePrompt = "这里填写附加值1，也可以留空...";
            this.value1.Size = new System.Drawing.Size(350, 20);
            this.value1.TabIndex = 50;
            this.value1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.username_KeyDown);
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(101, 54);
            this.password.Name = "password";
            this.password.Properties.NullValuePrompt = "这里填写登录密码...";
            this.password.Size = new System.Drawing.Size(350, 20);
            this.password.TabIndex = 49;
            this.password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.username_KeyDown);
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(101, 20);
            this.username.Name = "username";
            this.username.Properties.NullValuePrompt = "这里填写登录账户名...";
            this.username.Size = new System.Drawing.Size(350, 20);
            this.username.TabIndex = 48;
            this.username.KeyDown += new System.Windows.Forms.KeyEventHandler(this.username_KeyDown);
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(40, 120);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(55, 14);
            this.labelX4.TabIndex = 47;
            this.labelX4.Text = "附加值2：";
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(40, 88);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(55, 14);
            this.labelX3.TabIndex = 46;
            this.labelX3.Text = "附加值1：";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(59, 57);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(36, 14);
            this.labelX2.TabIndex = 45;
            this.labelX2.Text = "密码：";
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(47, 23);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(48, 14);
            this.labelX1.TabIndex = 44;
            this.labelX1.Text = "用户名：";
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar4});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barStaticItem1});
            this.barManager1.MaxItemId = 1;
            this.barManager1.StatusBar = this.bar4;
            // 
            // bar4
            // 
            this.bar4.BarName = "Status bar";
            this.bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar4.DockCol = 0;
            this.bar4.DockRow = 0;
            this.bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar4.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1)});
            this.bar4.OptionsBar.AllowQuickCustomization = false;
            this.bar4.OptionsBar.DrawDragBorder = false;
            this.bar4.OptionsBar.UseWholeRow = true;
            this.bar4.Text = "Status bar";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.barStaticItem1.Caption = "提示：附加值可以根据需要填写，亦可留空。";
            this.barStaticItem1.Id = 0;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(472, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 198);
            this.barDockControlBottom.Size = new System.Drawing.Size(472, 26);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 198);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(472, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 198);
            // 
            // UserInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 224);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "输入用户信息";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.UserInput_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.value2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.value1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.password.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.username.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelEx1;
        private DevExpress.XtraEditors.SimpleButton buttonX2;
        private DevExpress.XtraEditors.SimpleButton buttonX1;
        public DevExpress.XtraEditors.TextEdit value2;
        public DevExpress.XtraEditors.TextEdit value1;
        public DevExpress.XtraEditors.TextEdit password;
        public DevExpress.XtraEditors.TextEdit username;
        private DevExpress.XtraEditors.LabelControl labelX4;
        private DevExpress.XtraEditors.LabelControl labelX3;
        private DevExpress.XtraEditors.LabelControl labelX2;
        private DevExpress.XtraEditors.LabelControl labelX1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar4;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
    }
}