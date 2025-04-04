using System;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using ConfigEditor.Utils;

namespace ConfigEditor.Menus
{
    public class ECSMenuItem : NavBarItem
    {
        #region Attributes & Properties
        private readonly Action _clickAction;
        #endregion


        #region Constructors & Destructor
        private ECSMenuItem(string caption, bool enabled) : base(caption)
        {
            base.LinkClicked += Item_LinkClicked;
            base.Enabled = enabled;
        }

        public ECSMenuItem(string caption) : this(caption, false)
        {

        }

        public ECSMenuItem(string caption, Action clickAction) : this(caption, true)
        {
            // Safe design
            if (clickAction == null) { throw new ArgumentNullException(nameof(clickAction)); }

            this._clickAction = clickAction;
        }

        public ECSMenuItem(string caption, string iconName) : this(caption, false)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(iconName)) { throw new ArgumentNullException(nameof(iconName)); }

            this.ImageOptions.SmallImage = ECSImageUtility.GetImage(iconName);
        }

        public ECSMenuItem(string caption, string iconName, Action clickAction) : this(caption, true)
        {
            // Safe design
            //if (String.IsNullOrWhiteSpace(iconName)) { throw new ArgumentNullException(nameof(iconName)); }
            if (clickAction == null) { throw new ArgumentNullException(nameof(clickAction)); }

            this.ImageOptions.SmallImage = ECSImageUtility.GetImage(iconName);
            this._clickAction = clickAction;
        }
        #endregion


        #region Methods
        #endregion


        #region Events
        protected virtual void Item_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            if (_clickAction != null)
            {
                new DialogWaitCommand(() => _clickAction.Invoke()).Execute();
            }
            else
            {
                XtraMessageBox.Show(Caption);
            }
        }
        #endregion
    }
}