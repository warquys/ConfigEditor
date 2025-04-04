using DevExpress.XtraBars;
using System;


namespace ConfigEditor.Interfaces
{
    public interface ISelectableControl : IDisposable
    {
        object GetSelectedItem { get; }
        void Register(string commandKey, ICommand command, string caption, bool onBar, bool onMenu, bool onDoubleClick = false, BarShortcut shortcut = null);
    }
}
