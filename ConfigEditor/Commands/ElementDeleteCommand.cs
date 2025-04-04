using ConfigEditor.Elements;
using ConfigEditor.Interfaces;
using ConfigEditor.Utils;
using System;
using System.Windows.Forms;

namespace ConfigEditor.Commands
{
    public class ElementDeleteCommand<T> : BaseCommand<T>
    {
        #region Properties
        protected override bool CanExecuteValue
        {
            get { return Parameter != null && _canExecuteFunction.Invoke(Parameter); }
        }

        private readonly IListManager<T> _manager;
        //private readonly bool _canExecute;
        private Func<T, bool> _canExecuteFunction;

        private string _question;

        #endregion Properties


        #region Constructors

        public ElementDeleteCommand(IListManager<T> manager, bool canExecute = true, string question = "")
        {
            // Safe design
            if (manager == null) { throw new ArgumentNullException(nameof(manager)); }

            if (String.IsNullOrWhiteSpace(question))
            {
                _question = "Do you want to delete that item ?";
            }
            _manager = manager;
            _canExecuteFunction = (e) => canExecute;
        }

        public ElementDeleteCommand(IListManager<T> manager, Func<T, bool> canExecuteFunction, string question = "")
        {
            // Safe design
            if (manager == null) { throw new ArgumentNullException(nameof(manager)); }
            if (canExecuteFunction == null) { throw new ArgumentNullException(nameof(canExecuteFunction)); }
            if (String.IsNullOrWhiteSpace(question))
            {
                _question = "Do you want to delete that item ?";
            }

            _manager = manager;
            _canExecuteFunction = canExecuteFunction;
        }

        #endregion Constructors


        #region Methods

        protected override void ExecuteCommand()
        {
            if (CanExecuteValue && ECSMessageBox.ShowQuestion(_question) == DialogResult.Yes)
            {
                DeleteElement(Parameter);
            }
        }

        private void DeleteElement(T data)
        {
            // Safe design
            if (data == null) { throw new ArgumentNullException(nameof(data)); }
            DelStatus result = null;
            new DialogWaitCommand(() =>
            {
                result = _manager.Delete(data);
            }).Execute();
            if (result != null && result != DelStatus.Success)
            {

                ECSMessageBox.ShowError("That item cannot be deleted!");
            }
        }

        #endregion
    }
}
