using ConfigEditor.Controls;
using ConfigEditor.Interfaces;
using System;


namespace ConfigEditor.Commands
{
    public class ElementSaveCommand : BaseCommand
    {
        #region Properties

        private IWriteManager _manager;
        private ECSEditUserControl _editControl;

        #endregion Properties


        #region Constructors

        public ElementSaveCommand(IWriteManager manager)
        {
            // Safe design
            if (manager == null) { throw new ArgumentNullException(nameof(manager)); }
            _manager = manager;
            _editControl = null;
        }

        public ElementSaveCommand(IWriteManager manager, ECSEditUserControl editControl)
        {
            // Safe design
            if (manager == null) { throw new ArgumentNullException(nameof(manager)); }
            if (editControl == null) { throw new ArgumentNullException(nameof(editControl)); }
            _manager = manager;
            _editControl = editControl;
            _editControl.Memento.SourceStateChanged += Memento_SourceStateChanged;
        }

        #endregion Constructors

        #region Methods

        public override bool CanExecute(object parameter)
        {
            if (_editControl == null)
            {
                return true;
            }

            return _editControl.Memento.Changed;
        }

        protected override void ExecuteCommand()
        {
            if (_editControl == null)
            {
                SaveElement();
            }
            else
            {
                SaveElement(_editControl);
            }
        }

        private void SaveElement()
        {
            _manager.Save();
        }

        private void SaveElement(ECSEditUserControl editControl)
        {
            // Safe design
            if (editControl == null) { throw new ArgumentNullException(nameof(editControl)); }

            new DialogWaitCommand(() =>
            {
                editControl.Save();
            }
            ).Execute();
        }

        #endregion

        #region Events
        private void Memento_SourceStateChanged(object sender, EventArgs e)
        {
            OnCanExecuteChanged();
        }
        #endregion
    }
}
