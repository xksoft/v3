namespace V3.V3Form
{
    partial class frmGetCookie
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGetCookie));
            this.panel_Info = new DevExpress.XtraEditors.PanelControl();
            this.panel_bottom = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_ok = new DevExpress.XtraEditors.SimpleButton();
            this.splitContainerControl2 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridControl_main = new DevExpress.XtraGrid.GridControl();
            this.gridControl_main_view = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txt_cookie = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_Info)).BeginInit();
            this.panel_Info.SuspendLayout();
            this.panel_bottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main_view)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_cookie.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_Info
            // 
            this.panel_Info.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panel_Info.Controls.Add(this.panel_bottom);
            this.panel_Info.Controls.Add(this.splitContainerControl2);
            this.panel_Info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Info.Location = new System.Drawing.Point(0, 0);
            this.panel_Info.Name = "panel_Info";
            this.panel_Info.Size = new System.Drawing.Size(737, 386);
            this.panel_Info.TabIndex = 62;
            // 
            // panel_bottom
            // 
            this.panel_bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel_bottom.Controls.Add(this.label2);
            this.panel_bottom.Controls.Add(this.btn_cancel);
            this.panel_bottom.Controls.Add(this.btn_ok);
            this.panel_bottom.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_bottom.Location = new System.Drawing.Point(0, 0);
            this.panel_bottom.Name = "panel_bottom";
            this.panel_bottom.Size = new System.Drawing.Size(737, 53);
            this.panel_bottom.TabIndex = 50;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(148, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(427, 14);
            this.label2.TabIndex = 46;
            this.label2.Text = "请在抓包工具启动后打开本机浏览器进行操作，注意筛选正确的地址和马甲内容";
            // 
            // btn_cancel
            // 
            this.btn_cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_cancel.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.Image")));
            this.btn_cancel.Location = new System.Drawing.Point(662, 12);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(58, 27);
            this.btn_cancel.TabIndex = 45;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_ok
            // 
            this.btn_ok.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_ok.Image = ((System.Drawing.Image)(resources.GetObject("btn_ok.Image")));
            this.btn_ok.Location = new System.Drawing.Point(16, 12);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(97, 27);
            this.btn_ok.TabIndex = 44;
            this.btn_ok.Text = "使用选中马甲";
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerControl2.Horizontal = false;
            this.splitContainerControl2.Location = new System.Drawing.Point(3, 56);
            this.splitContainerControl2.Name = "splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add(this.gridControl_main);
            this.splitContainerControl2.Panel1.Text = "Panel1";
            this.splitContainerControl2.Panel2.Controls.Add(this.txt_cookie);
            this.splitContainerControl2.Panel2.Text = "Panel2";
            this.splitContainerControl2.Size = new System.Drawing.Size(730, 327);
            this.splitContainerControl2.SplitterPosition = 182;
            this.splitContainerControl2.TabIndex = 3;
            this.splitContainerControl2.Text = "splitContainerControl2";
            // 
            // gridControl_main
            // 
            this.gridControl_main.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_main.Location = new System.Drawing.Point(0, 0);
            this.gridControl_main.MainView = this.gridControl_main_view;
            this.gridControl_main.Name = "gridControl_main";
            this.gridControl_main.Size = new System.Drawing.Size(730, 182);
            this.gridControl_main.TabIndex = 1;
            this.gridControl_main.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridControl_main_view});
            // 
            // gridControl_main_view
            // 
            this.gridControl_main_view.Appearance.Row.Options.UseTextOptions = true;
            this.gridControl_main_view.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridControl_main_view.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.gridControl_main_view.GridControl = this.gridControl_main;
            this.gridControl_main_view.Name = "gridControl_main_view";
            this.gridControl_main_view.OptionsBehavior.Editable = false;
            this.gridControl_main_view.OptionsPrint.PrintHeader = false;
            this.gridControl_main_view.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridControl_main_view.OptionsSelection.MultiSelect = true;
            this.gridControl_main_view.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
            this.gridControl_main_view.OptionsView.ShowGroupPanel = false;
            this.gridControl_main_view.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.gridControl_main_view_SelectionChanged);
            // 
            // txt_cookie
            // 
            this.txt_cookie.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_cookie.Location = new System.Drawing.Point(0, 2);
            this.txt_cookie.Name = "txt_cookie";
            this.txt_cookie.Size = new System.Drawing.Size(730, 135);
            this.txt_cookie.TabIndex = 4;
            // 
            // frmGetCookie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 386);
            this.Controls.Add(this.panel_Info);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmGetCookie";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "马甲提取器";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmGetCookie_FormClosed);
            this.Load += new System.EventHandler(this.frmGetCookie_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panel_Info)).EndInit();
            this.panel_Info.ResumeLayout(false);
            this.panel_bottom.ResumeLayout(false);
            this.panel_bottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).EndInit();
            this.splitContainerControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main_view)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_cookie.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panel_Info;
        private System.Windows.Forms.Panel panel_bottom;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraEditors.SimpleButton btn_ok;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
        private DevExpress.XtraGrid.GridControl gridControl_main;
        private DevExpress.XtraGrid.Views.Grid.GridView gridControl_main_view;
        private DevExpress.XtraEditors.MemoEdit txt_cookie;
    }
}