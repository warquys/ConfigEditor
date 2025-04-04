using DevExpress.XtraBars;


namespace ConfigEditor.Interfaces

{
    internal interface ICommandRegistrator
    {
        void Register(string commandKey, ICommand command, string caption, BarShortcut shortcut, BarItemLinkAlignment cmdAlignment);
    }
}