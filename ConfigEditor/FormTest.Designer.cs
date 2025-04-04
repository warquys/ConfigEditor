namespace ConfigEditor
{
    partial class FormTest
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
            this.masterMenuControl1 = new ConfigEditor.Menus.MasterMenuControl();
            this.SuspendLayout();
            // 
            // masterMenuControl1
            // 
            this.masterMenuControl1.Location = new System.Drawing.Point(168, -17);
            this.masterMenuControl1.Name = "masterMenuControl1";
            this.masterMenuControl1.Size = new System.Drawing.Size(178, 492);
            this.masterMenuControl1.TabIndex = 0;
            // 
            // FormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.masterMenuControl1);
            this.Name = "FormTest";
            this.Text = "FormTest";
            this.ResumeLayout(false);

        }

        #endregion

        private Menus.MasterMenuControl masterMenuControl1;
    }
}