using System;
using DevExpress.Utils.Menu;
using ConfigEditor.Interfaces;

namespace ConfigEditor.Controls
{
    /// <summary>
    /// Sets the caption of a menu item and binds a command to it
    /// </summary>
    public class ECSCommandMenuItem : DXMenuItem
    {
        public ECSCommandMenuItem(string commandKey, ICommand command, string caption) : base()
        {
            // Safe design
            if (command == null) { throw new ArgumentNullException(nameof(command)); }

            this.Caption = caption;

            this.BindCommand(command);
        }
    }
}