using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ConfigEditor.Commands;
using ConfigEditor.Elements;
using ConfigEditor.Interfaces;
using ConfigEditor.Utils;

namespace ConfigEditor.Controls
{
    public partial class ECSEditUserControl : ECSBarUserControl
    {
        #region VS - Browsable

        /// <summary>
        /// True = The control will save then close without giving the user the choice to cancel
        /// False (default) = The control will ask the user to confirm before saving and then closing
        /// </summary>
        [Browsable(true)]
        public bool EcsAutoSaveNoQuestionOnClose { get; set; }

        [Browsable(true)]
        public bool EcsAutoSaveOnClose { get; set; }

        [Browsable(true)]
        public bool EcsCloseOnSave { get; set; }

        [Browsable(true)]
        public bool EcsDontCheckRight { get; set; }

        #endregion VS - Browsable


        #region Properties
        [Browsable(false)]
        public bool ValidateChangeAfterLoad { get; set; }

        [Browsable(false)]
        public bool IsNewElement
        {
            get
            {
                return BindingDataSource != null
                    && BindingDataSource.DataSource != null
                    && ((BindingDataSource.DataSource is BaseUintElement
                        && (BindingDataSource.DataSource as BaseUintElement).IsNew())
                        || (BindingDataSource.DataSource is BaseStringElement
                        && (BindingDataSource.DataSource as BaseStringElement).IsNew()));
            }
        }

        protected ElementResetCommand ResetCommand;
        protected ElementSaveCommand SaveCommand;

        public override int EditHashCode
        {
            get
            {
                if (BindingDataSource != null && BindingDataSource.DataSource != null)
                {
                    if (BindingDataSource.DataSource is Type)
                    {
                        return 0;
                    }
                    return BindingDataSource.DataSource.GetHashCode();
                }
                return 0;
            }
        }

        protected event EventHandler ParentClosing;

        public Memento Memento { get; set; }

        protected IWriteManager Manager { get; set; }

        public override string Text
        {
            get { return GetTitle(); }
            set { base.Text = value; }
        }

        #endregion Properties


        #region Constructors

        public ECSEditUserControl(IWriteManager manager, bool btnSaveAndCancel = true) : this()
        {
            // Safe design
            if (manager == null) { throw new ArgumentNullException(nameof(manager)); }
            Manager = manager;

            if (btnSaveAndCancel)
            {
                ResetCommand = new ElementResetCommand(this.Memento);
                SaveCommand = new ElementSaveCommand(Manager, this);

                RegisterSaveCommand();
            }

            this.Load += LoadEvent;
        }


        public ECSEditUserControl()
        {
            InitializeComponent();
            this.Load += Loading;
            EcsAutoSaveOnClose = true;
            Memento = new Memento(BindingDataSource);
            Memento.SourceReinitialized += MementoReinitialized;
        }

        #endregion Constructors


        #region Methods

        protected virtual void RegisterSaveCommand()
        {
            this.Register("ACN_ELEMENT_SAVE", SaveCommand, "Save");
            this.Register("ACN_ELEMENT_CANCEL", ResetCommand, "Cancel");
        }

        protected void HideCancelCommand()
        {
            ChangeCommandVisibility("ACN_ELEMENT_CANCEL", DevExpress.XtraBars.BarItemVisibility.Never);
        }

        protected void HideSaveCommand()
        {
            ChangeCommandVisibility("ACN_ELEMENT_SAVE", DevExpress.XtraBars.BarItemVisibility.Never);
        }

        public bool Save()
        {
            if (this.BeforeSave())
            {
                if (this.ManagerSave())
                {
                    this.AfterSave();
                    return true;
                }

                ECSMessageBox.ShowError("Error during save command!");
            }

            return false;
        }


        public virtual bool BeforeSave()
        {
            ResetValidation();
            var validationContext = new ValidationContext(BindingDataSource.DataSource);
            //validationContext.Items
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(BindingDataSource.DataSource, validationContext, results, true))
            {
                return this.ValidateChildren();
            }

            this.ValidateChildren();
            return false;
        }

        protected virtual bool ManagerSave()
        {
            if (Manager != null)
            {
                return Manager.Save();
            }
            return false;
        }

        public virtual void AfterSave()
        {
            this.ValidateChildren();
            Memento.ValidateChange();
            OnDataSourceSaved();

            Form form = this.FindForm();
            if (form != null)
            {
                form.Text = this.Text;
                if (EcsCloseOnSave)
                {
                    form.Close();
                }
            }
        }

        protected void OnDataSourceSaved(object sender, EventArgs e)
        {
            if (DataSourceSaved != null)
            {
                DataSourceSaved(sender, e);
            }
        }

        protected bool ChildrenValidated()
        {
            return ChildrenValidated(this.Controls);
        }

        protected void ResetValidation()
        {
            ResetValidation(this.Controls);
        }

        protected void OnDataSourceSaved()
        {
            DataSourceSaved?.Invoke(BindingDataSource.DataSource, EventArgs.Empty);
        }

        private bool ChildrenValidated(ControlCollection controls)
        {
            bool result = true;
            foreach (Control ctrl in controls)
            {
                if (ctrl is BaseEdit && !String.IsNullOrWhiteSpace((ctrl as BaseEdit).ErrorText))
                { return false; }
                else
                {
                    result = result && ChildrenValidated(ctrl.Controls);
                }
            }
            return result;
        }

        private void ResetValidation(ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl is BaseEdit && !String.IsNullOrWhiteSpace((ctrl as BaseEdit).ErrorText))
                {
                    (ctrl as BaseEdit).ErrorText = "";
                    (ctrl as BaseEdit).ErrorIcon = null;
                }
                else
                {
                    ResetValidation(ctrl.Controls);
                }
            }
        }

        private string GetTitle()
        {
            if (Manager != null)
            {
                string title = Manager.CurrentObject?.ToString();
                if (String.IsNullOrWhiteSpace(title) || IsNewElement)
                {
                    title = "New";
                }

                return title;
            }

            if (BindingDataSource.DataSource != null)
            {
                string title = BindingDataSource.DataSource.ToString();
                if (String.IsNullOrWhiteSpace(title))
                {
                    title = "New";
                }

                return title;
            }

            return String.IsNullOrWhiteSpace(base.Text) ? "ERREUR TITRE" : base.Text;
        }

        #endregion Methods


        #region Events

        /// <summary>
        /// Lets other controls (UI elements) know that this control's data has been changed
        /// </summary>
        public event EventHandler DataSourceSaved;

        private void ECSEditUserControl_Load(object sender, EventArgs e)
        {
        }

        protected void MementoReinitialized(object sender, EventArgs e)
        {
            this.ResetValidation();
        }

        private void Loading(object sender, EventArgs e)
        {
            Form frm = this.FindForm();
            if (frm != null)
            {
                frm.FormClosing += (se, args) =>
                {
                    if (Memento.Changed && !args.Cancel)
                    {
                        Memento.ResetChange();
                    }

                    ParentClosing?.Invoke(this, null);
                };
            }
        }
        private void LoadEvent(object sender, EventArgs e)
        {
            if (ValidateChangeAfterLoad)
            {
                this.FindForm().Activated += FormActivated;
            }
            this.Load -= LoadEvent;
        }

        private void FormActivated(object sender, EventArgs e)
        {
            if (ValidateChangeAfterLoad)
            {
                this.Memento.ValidateChange();
            }
            this.FindForm().Activated -= FormActivated;
        }


        #endregion Events
    }
}
