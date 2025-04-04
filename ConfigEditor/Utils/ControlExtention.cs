using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ConfigEditor.Interfaces;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace ConfigEditor.Utils
{
    public static class ControlExtention
    {
        #region Methods

        public static void Fill(this PanelControl panelControl, Control control)
        {
            // Safe design
            if (panelControl == null) { throw new ArgumentNullException(nameof(panelControl)); }
            if (control == null) { throw new ArgumentNullException(nameof(control)); }

            panelControl.Controls.Clear();
            panelControl.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }

        public static void BindCommand(this ButtonEdit editButtonControl, ICommand command, ButtonPredefines kind = ButtonPredefines.Ellipsis)
        {
            // Safe design
            if (editButtonControl == null) { throw new ArgumentNullException(nameof(editButtonControl)); }

            BindCommand(editButtonControl, command, () => editButtonControl.EditValue, kind);
        }

        public static void BindCommand(this ButtonEdit editButtonControl, ICommand command, Image img)
        {
            // Safe design
            if (editButtonControl == null) { throw new ArgumentNullException(nameof(editButtonControl)); }

            BindCommand(editButtonControl, command, () => editButtonControl.EditValue, img);
        }

        /// <remarks>See more at <see cref="https://docs.devexpress.com/WindowsForms/DevExpress.XtraEditors.Controls.ButtonPredefines"/>ButtonPredefines.</remarks>
        /// <summary>
        /// Binds a command to the button with a command parameter and adds an Ellipsis button by default.
        /// When the edit button value changes, the command will update its state (Enabled | Disable) to indicate whether it can be executed
        /// </summary>
        public static void BindCommand(this ButtonEdit editButtonControl, ICommand command, Func<object> queryCommandParameter, ButtonPredefines kind = ButtonPredefines.Ellipsis)
        {
            // Safe design
            if (editButtonControl == null) { throw new ArgumentNullException(nameof(editButtonControl)); }
            if (command == null) { throw new ArgumentNullException(nameof(command)); }

            if (editButtonControl.Properties.Buttons.Any())
            {
                //editButtonControl.Properties.Buttons[0].BindCommand(command);
                editButtonControl.ClearButtons();
            }
            //	else
            {
                var button = new EditorButton(kind);
                button.BindCommand(command, queryCommandParameter);
                editButtonControl.Properties.Buttons.Add(button);
            }

            if (queryCommandParameter != null)
            {
                editButtonControl.TextChanged += (sender, e) =>
                {
                    command.CanExecute(queryCommandParameter.Invoke());
                };
                //editButtonControl.EditValueChanged += (sender, e) => command.CanExecute(queryCommandParameter.Invoke());
            }
        }


        public static void BindCommand(this ButtonEdit editButtonControl, ICommand command, Func<object> queryCommandParameter, Image img)
        {
            // Safe design
            if (editButtonControl == null) { throw new ArgumentNullException(nameof(editButtonControl)); }
            if (command == null) { throw new ArgumentNullException(nameof(command)); }

            if (editButtonControl.Properties.Buttons.Any())
            {
                //editButtonControl.Properties.Buttons[0].BindCommand(command);
                editButtonControl.ClearButtons();
            }
            //	else
            {
                var button = new EditorButton(ButtonPredefines.Glyph, img, null);
                button.BindCommand(command, queryCommandParameter);
                editButtonControl.Properties.Buttons.Add(button);
            }

            if (queryCommandParameter != null)
            {
                editButtonControl.EditValueChanged += (sender, e) => command.CanExecute(queryCommandParameter.Invoke());
            }
        }

        public static void ClearButtons(this ButtonEdit editButtonControl)
        {
            // Safe design
            if (editButtonControl == null) { throw new ArgumentNullException(nameof(editButtonControl)); }

            editButtonControl.Properties.Buttons.Clear();
        }

        #endregion
    }
}
