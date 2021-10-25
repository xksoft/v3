namespace V3.V3Form
{
    partial class frm_重新选择分组
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_重新选择分组));
            this.panelEx1 = new DevExpress.XtraEditors.PanelControl();
            this.buttonX1 = new DevExpress.XtraEditors.SimpleButton();
            this.labelX1 = new DevExpress.XtraEditors.LabelControl();
            this.cmbGroup = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).BeginInit();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGroup.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.Controls.Add(this.buttonX1);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Controls.Add(this.cmbGroup);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(465, 51);
            this.panelEx1.TabIndex = 0;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Image = ((System.Drawing.Image)(resources.GetObject("buttonX1.Image")));
            this.buttonX1.Location = new System.Drawing.Point(354, 12);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(87, 21);
            this.buttonX1.TabIndex = 2;
            this.buttonX1.Text = "确定";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(14, 15);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(48, 14);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "分组名：";
            // 
            // cmbGroup
            // 
            this.cmbGroup.Location = new System.Drawing.Point(67, 12);
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down)});
            this.cmbGroup.Properties.NullText = "";
            this.cmbGroup.Properties.NullValuePrompt = "请选择要添加的分组...";
            this.cmbGroup.Properties.PopupSizeable = false;
            this.cmbGroup.Properties.ShowFooter = false;
            this.cmbGroup.Properties.ShowHeader = false;
            this.cmbGroup.Properties.ShowLines = false;
            this.cmbGroup.Size = new System.Drawing.Size(279, 20);
            this.cmbGroup.TabIndex = 11;
            this.cmbGroup.EditValueChanged += new System.EventHandler(this.cmbGroup_EditValueChanged);
            // 
            // frm_重新选择分组
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 51);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_重新选择分组";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选择分组";
            this.Load += new System.EventHandler(this.frm_重新选择分组_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGroup.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelEx1;
        private DevExpress.XtraEditors.SimpleButton buttonX1;
        private DevExpress.XtraEditors.LabelControl labelX1;
        public DevExpress.XtraEditors.LookUpEdit cmbGroup;
    }
}