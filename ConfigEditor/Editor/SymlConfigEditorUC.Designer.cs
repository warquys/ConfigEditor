namespace ConfigEditor.Editor
{
    partial class SymlConfigEditorUC
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this._panelDetail = new DevExpress.XtraEditors.PanelControl();
            this._panelConfig = new DevExpress.XtraEditors.PanelControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
            ((System.ComponentModel.ISupportInitialize)(this._layoutControl)).BeginInit();
            this._layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._panelDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._panelConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // _layoutControl
            // 
            this._layoutControl.Controls.Add(this._panelDetail);
            this._layoutControl.Controls.Add(this._panelConfig);
            this._layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layoutControl.Location = new System.Drawing.Point(0, 0);
            this._layoutControl.Name = "_layoutControl";
            this._layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(847, 132, 650, 400);
            this._layoutControl.Root = this.Root;
            this._layoutControl.Size = new System.Drawing.Size(800, 400);
            this._layoutControl.TabIndex = 4;
            this._layoutControl.Text = "layoutControl1";
            // 
            // _panelDetail
            // 
            this._panelDetail.Location = new System.Drawing.Point(268, 12);
            this._panelDetail.Name = "_panelDetail";
            this._panelDetail.Size = new System.Drawing.Size(520, 376);
            this._panelDetail.TabIndex = 7;
            // 
            // _panelConfig
            // 
            this._panelConfig.Location = new System.Drawing.Point(12, 12);
            this._panelConfig.Name = "_panelConfig";
            this._panelConfig.Size = new System.Drawing.Size(242, 376);
            this._panelConfig.TabIndex = 6;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem3,
            this.splitterItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(800, 400);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._panelConfig;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(246, 5);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(246, 380);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._panelDetail;
            this.layoutControlItem3.Location = new System.Drawing.Point(256, 0);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(246, 5);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(524, 380);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // splitterItem1
            // 
            this.splitterItem1.Location = new System.Drawing.Point(246, 0);
            this.splitterItem1.Name = "splitterItem1";
            this.splitterItem1.Size = new System.Drawing.Size(10, 380);
            // 
            // SymlConfigEditorUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._layoutControl);
            this.MinimumSize = new System.Drawing.Size(800, 400);
            this.Name = "SymlConfigEditorUC";
            this.Size = new System.Drawing.Size(800, 400);
            this.Controls.SetChildIndex(this._layoutControl, 0);
            ((System.ComponentModel.ISupportInitialize)(this._layoutControl)).EndInit();
            this._layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._panelDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._panelConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl _layoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.PanelControl _panelConfig;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.PanelControl _panelDetail;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.SplitterItem splitterItem1;
    }
}
