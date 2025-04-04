using ConfigEditor.Controls;
using ConfigEditor.Factory;
using ConfigEditor.Interfaces;
using ConfigEditor.Utils;
using System;
using System.Windows.Forms;

namespace ConfigEditor.Commands
{
    public class ElementNewCommand<T> : BaseCommand
        where T : new()
    {
        #region Attributes & Properties
        private IListManager _manager;
        protected Func<T> Constructor { get; set; }

        private Func<bool> _canExecute;

        #endregion


        #region Constructors

        public ElementNewCommand(IListManager manager, bool canExecute = true)
            : this(manager, () => new T(), () => canExecute)
        { }


        public ElementNewCommand(IListManager manager, Func<T> constructor, bool canExecute = true)
            : this(manager, constructor, () => canExecute)
        { }

        public ElementNewCommand(IListManager manager, Func<T> constructor, Func<bool> canExecute)
        {
            // Safe design
            if (manager == null) { throw new ArgumentNullException(nameof(manager)); }
            if (constructor == null) { throw new ArgumentNullException(nameof(constructor)); }

            _manager = manager;
            Constructor = constructor;
            _canExecute = canExecute;
        }

        #endregion Constructors


        #region Methods


        protected override bool CanExecuteValue
        {
            get { return _canExecute.Invoke() && base.CanExecuteValue; }
        }

        protected override void ExecuteCommand()
        {
            T newElement = Constructor.Invoke();
            new DialogWaitCommand(() => NewElement(newElement)).Execute();
        }

        protected virtual void NewElement(T data)
        {
            Form frm = ECSFormUtility.FindForm<T>(data);

            if (frm is null)
            {
                CreateForm(data);
            }
            else if (!ECSFormUtility.ModalContext)
            {
                ECSFormUtility.ActivateForm(frm);
            }
            else
            {
                ECSFormUtility.UnicityError();
            }
        }

        protected virtual ECSUserControl GetDetailControl(T data)
        {
            IWriteManager writeMgr = _manager as IWriteManager;
            writeMgr.PrepareNew();
            return ECSDetailFactory.CreateDetailControl(writeMgr);
        }

        private void CreateForm(T data)
        {
            // Safe design
            if (data == null) { throw new ArgumentNullException(nameof(data)); }

            ECSUserControl ctrlDetail = GetDetailControl(data);
            if (ctrlDetail != null)
            {
                if (ctrlDetail is ECSEditUserControl)
                {
                    (ctrlDetail as ECSEditUserControl).DataSourceSaved += (sender, e) =>
                    {
                        new DialogWaitCommand(() =>
                        {
                            _manager.UpdateElement(sender);
                        }).Execute();
                    };
                }
                ECSFormUtility.OpenForm<T>(ctrlDetail.Text, ctrlDetail);
            }
        }

        #endregion Methods
    }
}