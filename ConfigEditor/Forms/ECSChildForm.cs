using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraLayout;
using DevExpress.XtraSplashScreen;
using ConfigEditor.Controls;
using ConfigEditor.Utils;
using ConfigEditor.Interfaces;

namespace ConfigEditor.Forms
{
    public partial class ECSChildForm : DevExpress.XtraEditors.XtraForm, ISavable
    {
        #region Properties

        private Dictionary<Keys, Action> _dictShortcut = new Dictionary<Keys, Action>();

        private bool _modal;
        public bool IsModal { get { return _modal; } }

        public bool NeedToSave
        {
            get
            {
                if (_control is ISavable)
                {
                    return (_control as ISavable).NeedToSave;
                }
                return false;
            }
        }
        public bool CancelClose
        {
            get
            {
                if (_control is ISavable)
                {
                    return (_control as ISavable).CancelClose;
                }
                return false;
            }
        }
        public int EditHashCode
        {
            get
            {
                return _control != null ? _control.EditHashCode : 0;
            }
        }

        private ECSUserControl _control;

        #endregion Properties


        #region Constructors

        public ECSChildForm() : base()
        {
            InitializeComponent();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;

            this.Shown += (o, e) =>
            {
                _control?.Shown();
            };
        }


        public ECSChildForm(string title, Control control, bool modal) : this()
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(title)) { throw new ArgumentNullException(nameof(title)); }
            if (control == null) { throw new ArgumentNullException(nameof(control)); }

            _modal = modal;

            InitForm(title, control);
        }

        public ECSChildForm(string title, Control control, Icon icon = null) : this()
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(title)) { throw new ArgumentNullException(nameof(title)); }
            if (control == null) { throw new ArgumentNullException(nameof(control)); }

            if (icon != null)
            {
                this.Icon = icon;
            }
            InitForm(title, control);
        }

        public ECSChildForm(string title, Control control, Form parent, Icon icon = null) : this()
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(title)) { throw new ArgumentNullException(nameof(title)); }
            if (control == null) { throw new ArgumentNullException(nameof(control)); }

            _modal = _modal || parent.Modal;

            if (icon != null)
            {
                this.Icon = icon;
            }
            InitForm(title, control);
        }

        #endregion Constructors


        #region Methods
        public void RefreshTitle()
        {
            this.Text = _control.Text;
        }

        public bool SaveNeeded()
        {
            if (_control is ECSEditUserControl)
            {
                return (_control as ECSEditUserControl).Memento.Changed;
            }

            return false;
        }

        public bool SaveIfPossible()
        {
            if (SaveNeeded())

            {
                if ((_control as ECSEditUserControl).IsNewElement)
                {
                    return false;
                }
                else
                {
                    return (_control as ECSEditUserControl).Save();
                }
            }

            return true;
        }

        public void SetIcon(string iconKey)
        {
            if (!String.IsNullOrWhiteSpace(iconKey))
            {
                Icon icon = ECSImageUtility.GetIcon(iconKey);
                if (icon != null)
                {
                    this.Icon = icon;
                }
            }
        }

        private void InitForm(string title, Control control)
        {
            // Safe design
            if (control == null) { throw new ArgumentNullException(nameof(control)); }

            this.Text = title;

            _control = control as ECSUserControl;
            if (_control != null)
            {
                _modal = _modal || _control.EcsDisplayAsDialog || ECSFormUtility.ModalContext;
            }


            if (_modal)
            {
                var _layout = new LayoutControl();
                _layout.AutoSize = true;
                this.Controls.Add(_layout);
                _layout.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(4);
                this.AutoSize = true;

                LayoutControlItem item = _layout.AddItem("", control);
                item.TextVisible = false;
                item.Padding = new DevExpress.XtraLayout.Utils.Padding(0);
                // Ajout de taille pour le padding
                int incHeight = 18;
                int incWidth = 8;
                if (control is ECSBarUserControl)
                {
                    incHeight += 30;
                    incWidth += 15;
                }

                _layout.MaximumSize = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width - incWidth, Screen.PrimaryScreen.Bounds.Height - incHeight - 20);

                // Faire disparaire la scroll hor bar si scroll bar vertical
                if (control.MinimumSize.Height + incHeight > _layout.MaximumSize.Height)
                {
                    incWidth += 20;
                }

                _layout.MinimumSize = new System.Drawing.Size(control.MinimumSize.Width + incWidth, control.MinimumSize.Height + incHeight);


                if (_layout.MinimumSize.Width > _layout.MaximumSize.Width || _layout.MinimumSize.Height > _layout.MaximumSize.Height)
                {
                    _layout.MinimumSize = new System.Drawing.Size(Math.Min(_layout.MinimumSize.Width, _layout.MaximumSize.Width), Math.Min(_layout.MinimumSize.Height, _layout.MaximumSize.Height));
                }
                _layout.AutoSize = true;

                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                //this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            }
            else
            {
                //_layout.Visible = false;
                this.Controls.Add(control);
                control.Dock = DockStyle.Fill;
                this.MdiParent = ECSFormUtility.MainMdiParent;
            }

            var ecsControl = control as ECSUserControl;
            if (ecsControl != null)
            {
                this.AcceptButton = ecsControl.AcceptButton;
                this.CancelButton = ecsControl.CancelButton;
            }
        }

        public void Display()
        {
            if (_modal)
            {
                if (SplashScreenManager.Default != null)
                {
                    SplashScreenManager.CloseForm();
                }
                ShowDialog();
            }
            else
            {
                Show();
            }
        }

        public override int GetHashCode()
        {
            return _control != null ? _control.GetHashCode() : base.GetHashCode();
        }


        public void AddShortCut(Keys key, Action action)
        {
            // Safe design
            if (action == null) { throw new ArgumentNullException(nameof(action)); }

            if (!_dictShortcut.ContainsKey(key))
            {
                _dictShortcut.Add(key, action);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_dictShortcut.ContainsKey(keyData))
            {
                _dictShortcut[keyData].Invoke();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region Events
        private void ECSChildForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var _editControl = _control as ECSEditUserControl;
            if (_editControl != null && _editControl.Memento.Changed)
            {
                if (_editControl.EcsAutoSaveOnClose && (_editControl.EcsAutoSaveNoQuestionOnClose || ECSMessageBox.ShowQuestion("Save modification ?") == DialogResult.Yes))
                {
                    new DialogWaitCommand(() =>
                    {
                        e.Cancel = !_editControl.Save();
                    }).Execute();
                }
                else
                {
                    _editControl.Memento.ResetChange();
                }
            }
        }
        #endregion
    }
}