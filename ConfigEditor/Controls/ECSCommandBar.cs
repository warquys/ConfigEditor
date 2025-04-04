using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraBars;
using ConfigEditor.Interfaces;
using ConfigEditor.Utils;

namespace ConfigEditor.Controls
{
    public class ECSCommandBar : ECSBar, ICommandRegistrator
    {
        #region Properties

        private Dictionary<string, BarItem> _commandDictionary = new Dictionary<string, BarItem>();
        public override bool Visible
        {
            get { return base.Visible && _commandDictionary.Values.Any(p => p.Visibility != BarItemVisibility.Never); }
            set { base.Visible = value; }
        }

        #endregion Properties


        #region Constructors

        public ECSCommandBar()
        { }

        public ECSCommandBar(BarManager manager) : base(manager, "Commands")
        {
            manager.AllowCustomization = false;
            manager.AllowQuickCustomization = false;
            manager.AllowShowToolbarsPopup = false;
        }

        public ECSCommandBar(BarManager manager, string name) : base(manager, name)
        { }

        #endregion Constructors


        #region Methods

        private BarItem CreateButton(string commandKey, ICommand command, string caption)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(commandKey)) { throw new ArgumentNullException(nameof(commandKey)); }
            if (command == null) { throw new ArgumentNullException(nameof(command)); }

            var button = new BarButtonItem(this.Manager, command.Caption);
            button.Caption = caption;
            // Icon
            button.ImageOptions.Image = ECSImageUtility.GetImage(commandKey, 24);

            // Command
            button.BindCommand(command);


            return button;
        }

        #endregion Methods


        #region Methods - ICommandRegistrator

        public void Register(string commandKey, ICommand command, string caption, BarShortcut shortcut = null, BarItemLinkAlignment cmdAlignment = BarItemLinkAlignment.Left)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(commandKey)) { throw new ArgumentNullException(nameof(commandKey)); }
            if (command == null) { throw new ArgumentNullException(nameof(command)); }

            if (_commandDictionary.ContainsKey(commandKey))
            {
                // Command already in the bar ... do nothing
                return;
            }

            this.BeginUpdate();
            //
            command.Key = commandKey;
            BarItem barItem = CreateButton(commandKey, command, caption);
            barItem.Alignment = cmdAlignment;
            if (shortcut != null)
            {
                barItem.ItemShortcut = shortcut;
            }
            if (barItem != null)
            {
                this.AddItem(barItem);

                _commandDictionary.Add(commandKey, barItem);
            }
            //
            this.EndUpdate();
        }

        public void ChangeCommandVisibility(string commandKey, BarItemVisibility visibility)
        {
            if (_commandDictionary.ContainsKey(commandKey))
            {
                _commandDictionary[commandKey].Visibility = visibility;
            }
        }

        #endregion Methods - ICommandRegistrator
    }
}