
using ConfigEditor.Utils;
using System;

namespace ConfigEditor.Commands
{
    public class ElementResetCommand : BaseCommand
    {

        #region Attributes & Properties
        private Memento _memento;
        public event EventHandler BeforeReset;
        public event EventHandler AfterReset;
        #endregion
        #region Constructors & Destructor
        public ElementResetCommand(Memento memento)
        {
            // Safe design
            if (memento == null) { throw new ArgumentNullException(nameof(memento)); }
            _memento = memento;
            _memento.SourceStateChanged += Memento_SourceStateChanged;
        }
        #endregion
        #region Methods
        protected override void ExecuteCommand()
        {
            BeforeReset?.Invoke(this, EventArgs.Empty);
            _memento.ResetChange();
            AfterReset?.Invoke(this, EventArgs.Empty);
        }
        public override bool CanExecute(object parameter)
        {
            return _memento.Changed;
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
