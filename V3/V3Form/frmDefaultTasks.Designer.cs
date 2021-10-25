namespace V3.V3Form
{
    partial class frmDefaultTasks
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDefaultTasks));
            this.gridControl_main = new DevExpress.XtraGrid.GridControl();
            this.gridControl_main_view = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel_Info = new DevExpress.XtraEditors.PanelControl();
            this.panel_bottom = new System.Windows.Forms.Panel();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_ok = new DevExpress.XtraEditors.SimpleButton();
            this.label_name = new DevExpress.XtraEditors.LabelControl();
            this.text_name = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main_view)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_Info)).BeginInit();
            this.panel_Info.SuspendLayout();
            this.panel_bottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.text_name.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl_main
            // 
            this.gridControl_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl_main.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_main.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridControl_main.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridControl_main.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridControl_main.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.gridControl_main.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridControl_main.EmbeddedNavigator.Buttons.First.Enabled = false;
            this.gridControl_main.EmbeddedNavigator.Buttons.First.Visible = false;
            this.gridControl_main.EmbeddedNavigator.Buttons.Last.Visible = false;
            this.gridControl_main.EmbeddedNavigator.Buttons.Next.Visible = false;
            this.gridControl_main.EmbeddedNavigator.Buttons.NextPage.Visible = false;
            this.gridControl_main.EmbeddedNavigator.Buttons.Prev.Visible = false;
            this.gridControl_main.EmbeddedNavigator.Buttons.PrevPage.Visible = false;
            this.gridControl_main.EmbeddedNavigator.TextStringFormat = "";
            this.gridControl_main.Location = new System.Drawing.Point(0, 0);
            this.gridControl_main.MainView = this.gridControl_main_view;
            this.gridControl_main.Name = "gridControl_main";
            this.gridControl_main.Size = new System.Drawing.Size(443, 197);
            this.gridControl_main.TabIndex = 1;
            this.gridControl_main.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridControl_main_view});
            this.gridControl_main.Click += new System.EventHandler(this.gridControl_main_Click);
            this.gridControl_main.KeyUp += new System.Windows.Forms.KeyEventHandler(this.gridControl_main_KeyUp);
            // 
            // gridControl_main_view
            // 
            this.gridControl_main_view.Appearance.Row.Options.UseTextOptions = true;
            this.gridControl_main_view.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridControl_main_view.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gridControl_main_view.GridControl = this.gridControl_main;
            this.gridControl_main_view.Name = "gridControl_main_view";
            this.gridControl_main_view.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridControl_main_view.OptionsBehavior.Editable = false;
            this.gridControl_main_view.OptionsPrint.PrintHeader = false;
            this.gridControl_main_view.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridControl_main_view.OptionsSelection.MultiSelect = true;
            this.gridControl_main_view.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
            this.gridControl_main_view.OptionsView.ShowGroupPanel = false;
            this.gridControl_main_view.RowDeleted += new DevExpress.Data.RowDeletedEventHandler(this.gridControl_main_view_RowDeleted);
            // 
            // panel_Info
            // 
            this.panel_Info.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panel_Info.Controls.Add(this.panel_bottom);
            this.panel_Info.Controls.Add(this.label_name);
            this.panel_Info.Controls.Add(this.text_name);
            this.panel_Info.Controls.Add(this.gridControl_main);
            this.panel_Info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Info.Location = new System.Drawing.Point(0, 0);
            this.panel_Info.Name = "panel_Info";
            this.panel_Info.Size = new System.Drawing.Size(443, 250);
            this.panel_Info.TabIndex = 63;
            // 
            // panel_bottom
            // 
            this.panel_bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel_bottom.Controls.Add(this.btn_cancel);
            this.panel_bottom.Controls.Add(this.btn_ok);
            this.panel_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_bottom.Location = new System.Drawing.Point(0, 197);
            this.panel_bottom.Name = "panel_bottom";
            this.panel_bottom.Size = new System.Drawing.Size(443, 53);
            this.panel_bottom.TabIndex = 50;
            // 
            // btn_cancel
            // 
            this.btn_cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_cancel.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.Image")));
            this.btn_cancel.Location = new System.Drawing.Point(368, 12);
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
            this.btn_ok.Location = new System.Drawing.Point(168, 12);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(154, 27);
            this.btn_ok.TabIndex = 44;
            this.btn_ok.Text = "使用选中的任务模板";
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // label_name
            // 
            this.label_name.Location = new System.Drawing.Point(16, 84);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(60, 14);
            this.label_name.TabIndex = 48;
            this.label_name.Text = "模板名称：";
            // 
            // text_name
            // 
            this.text_name.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.text_name.Location = new System.Drawing.Point(82, 81);
            this.text_name.Name = "text_name";
            this.text_name.Size = new System.Drawing.Size(344, 20);
            this.text_name.TabIndex = 49;
            // 
            // frmDefaultTasks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 250);
            this.Controls.Add(this.panel_Info);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDefaultTasks";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "任务模板管理 (Delete键删除选中项)";
            this.Load += new System.EventHandler(this.frmDefaultPoints_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_main_view)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panel_Info)).EndInit();
            this.panel_Info.ResumeLayout(false);
            this.panel_Info.PerformLayout();
            this.panel_bottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.text_name.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl_main;
        private DevExpress.XtraGrid.Views.Grid.GridView gridControl_main_view;
        private DevExpress.XtraEditors.PanelControl panel_Info;
        private System.Windows.Forms.Panel panel_bottom;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraEditors.SimpleButton btn_ok;
        private DevExpress.XtraEditors.TextEdit text_name;
        private DevExpress.XtraEditors.LabelControl label_name;
    }
}