using ConfigEditor.Interfaces;
using System;
using System.ComponentModel;

namespace ConfigEditor.Commands
{
    public abstract class BaseCommand<T, U> : BaseCommand<T>
    {
        #region Attributes & Properties
        private U _secondParameter;

        public U SecondParameter
        {
            get { return _secondParameter; }
            set
            {
                _secondParameter = value;
                OnCanExecuteChanged();
            }
        }

        protected override bool CanExecuteValue
        {
            get { return base.CanExecuteValue && _secondParameter != null; }
        }
        #endregion


        #region Constructors & Destructor
        protected BaseCommand() : base()
        { }
        #endregion


        #region Methods
        #endregion
    }


    public abstract class BaseCommand<T> : BaseCommand, ICommand<T>
    {
        #region Attributes & Properties
        private T _parameter;
        public virtual T Parameter
        {
            get { return _parameter; }
            set
            {
                bool canExecuteOld = CanExecuteValue;
                bool canExec = _parameter?.Equals(value) != true;
                _parameter = value;
                RefreshCommandState(canExec);// (canExecuteOld != CanExecuteValue);

                // WARNING: This implementation generates a StackOverflow (circular call between OnCanExecuteChanged and the setter of property Parameter)
                //_parameter = value;
                //OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// Can execute only if the parameter of the command is set
        /// </summary>
        protected override bool CanExecuteValue
        {
            get { return _parameter != null; }
        }

        #endregion


        #region Constructors & Destructor

        protected BaseCommand() : base()
        { }

        #endregion


        #region Methods

        /// <summary>
        /// The element in the UI (Ex: an icon) representing this command will become enabled or disabled based on the return value of CanExecuteValue
        /// </summary>
        protected virtual void RefreshCommandState(bool refresh)
        {
            if (refresh)
            {
                OnCanExecuteChanged();
            }
        }

        public sealed override void Execute(object parameter)
        {
            // Permet d'assigner la valeur de "Parameter" par celle issue du binding de la command (CommandParameter).
            if (parameter is T)
            {
                this.Parameter = (T)parameter;
            }
            Execute();
        }

        public sealed override bool CanExecute(object parameter)
        {
            // Permet d'assigner la valeur de "Parameter" par celle issue du binding de la command (CommandParameter).
            if (parameter is T)
            {
                this.Parameter = (T)parameter;
            }
            return CanExecuteValue;
        }

        #endregion
    }


    public abstract class BaseCommand : ICommand
    {
        #region Attributes & Properties
        public string Key { get; set; }
        public event EventHandler CaptionChanged;

        private string _caption;
        public string Caption
        {
            get { return _caption; }
            set
            {
                if (value != _caption)
                {
                    _caption = value;
                    OnCaptionChanged();
                }
            }
        }

        public event EventHandler<ExeCommandArgs> AfterExecute;
        public event EventHandler<ExeCommandArgs> BeforeExecute;
        /// <summary>
        /// Set this to true when the user cancels an action in the UI.
        /// </summary>
        protected bool UserCancelled { get; set; }

        /// <summary>
        /// By default, a command can execute
        /// </summary>
        protected virtual bool CanExecuteValue
        {
            get { return true; }
        }

        public event EventHandler CanExecuteChanged;
        #endregion


        #region Constructors & Destructor
        protected BaseCommand()
        { }
        #endregion


        #region Methods
        protected abstract void ExecuteCommand();

        public void Execute()
        {
            OnBeforeExecute();
            ExecuteCommand();
            OnAfterExecute();
        }

        public virtual void Execute(object parameter)
        {
            Execute();
        }

        public virtual bool CanExecute(object parameter)
        {
            return CanExecuteValue;
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnCaptionChanged()
        {
            CaptionChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnAfterExecute()
        {
            AfterExecute?.Invoke(this, new ExeCommandArgs { Cancel = UserCancelled });
        }

        protected virtual void OnBeforeExecute()
        {
            BeforeExecute?.Invoke(this, new ExeCommandArgs { Cancel = UserCancelled });
        }
        #endregion
    }

    /// <summary>
    /// Includes argument to specify if the user has cancelled the execution
    /// </summary>
    public class ExeCommandArgs : CancelEventArgs
    {
    
    }
}