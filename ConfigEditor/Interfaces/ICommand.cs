using System;
using IWinCommand = System.Windows.Input.ICommand;

namespace ConfigEditor.Interfaces
{
    public interface ICommand<T> : ICommand
    {
        #region Attributes & Properties
        T Parameter { get; set; }
        #endregion


        #region Methods
        #endregion
    }


    public interface ICommand : IWinCommand
    {
        #region Attributes & Properties
        string Caption { get; set; }
        string Key { get; set; }
        #endregion


        #region Methods
        void Execute();
        void OnCanExecuteChanged();

        event EventHandler CaptionChanged;
        #endregion
    }
}

