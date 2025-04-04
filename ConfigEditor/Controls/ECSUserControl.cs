using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Container;
using DevExpress.XtraLayout;
using ConfigEditor.Forms;
using ConfigEditor.Utils;

namespace ConfigEditor.Controls
{
    public class ECSUserControl : XtraUserControl
    {
        #region Properties
        public virtual int EditHashCode
        {
            get { return 0; }
        }

        private bool _ecsDisplayAsDialog = false;
        [Browsable(true)]
        public bool EcsDisplayAsDialog
        {
            get { return _ecsDisplayAsDialog; }
            set { _ecsDisplayAsDialog = value; }
        }

        /// <summary>
        /// This is the text that appears in the title bar of the control
        /// </summary>
        [Browsable(true)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public IButtonControl AcceptButton { get; protected set; }

        public IButtonControl CancelButton { get; protected set; }

        #endregion Properties


        #region Constructors

        public ECSUserControl()
        {
        }

        #endregion Constructors


        #region Methods

        /// <summary>
        /// Execute logic on the control, after it has been displayed to the user and data has been loaded
        /// Ex: Selecting all lines of a list.
        /// </summary>
        public virtual void Shown()
        { }


        protected void AddShortCut(Keys key, Action action)
        {
            // Safe design
            if (action == null) { throw new ArgumentNullException(nameof(action)); }
            this.Load += (s, e) =>
            {
                if ((this.FindForm() as ECSChildForm) != null)
                {
                    (this.FindForm() as ECSChildForm).AddShortCut(key, action);
                }
            };
        }

        protected virtual void Close()
        {
            ParentForm?.Close();
        }
        public override int GetHashCode()
        {
            return this.GetType().GetHashCode() ^ EditHashCode;
        }

        public bool ControlHaveFocus()
        {
            bool result = false;
            foreach (Control controlChild in Controls)
            {
                result = result || ControlHaveFocus(controlChild);
            }
            return result;
        }

        private bool ControlHaveFocus(Control control)
        {
            if (control is LayoutControl)
            {
                bool result = false;
                foreach (Control controlChild in control.Controls)
                {
                    result = result || ControlHaveFocus(controlChild);
                }
                return result;
            }
            else if (control is BaseEdit)
            {
                return (control as BaseEdit).EditorContainsFocus;
            }
            else
            {
                return control.Focused;
            }
        }

        public virtual void RefreshTranData()
        {
            // Override this method and provide your own implementation
        }


        #endregion Methods

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ECSUserControl
            // 
            this.Name = "ECSUserControl";
            this.ResumeLayout(false);

        }
    }
}
