using System;
using System.Windows.Forms;
using DevExpress.XtraTab;


namespace ConfigEditor.Menus
{
    public class ECSMenuTabPage : XtraTabPage
    {
        #region Constructors

        public ECSMenuTabPage() : base()
        { }

        public ECSMenuTabPage(Control control) : base()
        {
            // Safe design
            if (control == null) { throw new ArgumentNullException(nameof(control)); }

            control.Dock = DockStyle.Fill;
            Text = control.Text;
            PageVisible = control.Visible;
            Controls.Add(control);
        }

        #endregion Constructors
    }
}