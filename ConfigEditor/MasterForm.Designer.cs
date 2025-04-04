namespace ConfigEditor
{
    partial class MasterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterForm));
            this.DocumentManager = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this._barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this._tabbedView = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this._sidePanel = new DevExpress.XtraEditors.SidePanel();
            this.masterMenuControl1 = new ConfigEditor.Menus.MasterMenuControl();
            ((System.ComponentModel.ISupportInitialize)(this.DocumentManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._tabbedView)).BeginInit();
            this._sidePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // DocumentManager
            // 
            this.DocumentManager.MdiParent = this;
            this.DocumentManager.MenuManager = this._barManager;
            this.DocumentManager.View = this._tabbedView;
            this.DocumentManager.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this._tabbedView});
            // 
            // _barManager
            // 
            this._barManager.DockControls.Add(this.barDockControlTop);
            this._barManager.DockControls.Add(this.barDockControlBottom);
            this._barManager.DockControls.Add(this.barDockControlLeft);
            this._barManager.DockControls.Add(this.barDockControlRight);
            this._barManager.Form = this;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this._barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(1152, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 568);
            this.barDockControlBottom.Manager = this._barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1152, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this._barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 568);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1152, 0);
            this.barDockControlRight.Manager = this._barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 568);
            // 
            // _tabbedView
            // 
            this._tabbedView.DocumentProperties.UseFormIconAsDocumentImage = true;
            // 
            // _sidePanel
            // 
            this._sidePanel.AutoScroll = true;
            this._sidePanel.AutoScrollMinSize = new System.Drawing.Size(20, 0);
            this._sidePanel.Controls.Add(this.masterMenuControl1);
            this._sidePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this._sidePanel.Location = new System.Drawing.Point(0, 0);
            this._sidePanel.MinimumSize = new System.Drawing.Size(20, 0);
            this._sidePanel.Name = "_sidePanel";
            this._sidePanel.Size = new System.Drawing.Size(243, 568);
            this._sidePanel.TabIndex = 1;
            this._sidePanel.Text = "sidePanel1";
            // 
            // masterMenuControl1
            // 
            this.masterMenuControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.masterMenuControl1.Location = new System.Drawing.Point(0, 0);
            this.masterMenuControl1.Name = "masterMenuControl1";
            this.masterMenuControl1.Size = new System.Drawing.Size(242, 568);
            this.masterMenuControl1.TabIndex = 0;
            // 
            // MasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1152, 568);
            this.Controls.Add(this._sidePanel);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("MasterForm.IconOptions.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MasterForm";
            this.Text = "ConfigEditor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.DocumentManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._tabbedView)).EndInit();
            this._sidePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView _tabbedView;
        private DevExpress.XtraEditors.SidePanel _sidePanel;
        public DevExpress.XtraBars.Docking2010.DocumentManager DocumentManager;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarManager _barManager;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private Menus.MasterMenuControl masterMenuControl1;
    }
}