using System;
using DevExpress.XtraBars;
using ConfigEditor.Interfaces;

namespace ConfigEditor.Controls
{
    public partial class ECSBarUserControl : ECSUserControl, ICommandRegistrator
    {
        #region Properties

        protected BarManager BarManager { get { return _barManager; } }

        // Commands

        private ECSCommandBar _commandBar;
        private ECSCommandBar CommandBar
        {
            get
            {
                if (_commandBar == null)
                {
                    _commandBar = new ECSCommandBar(_barManager);
                    _barManager.Bars.Add(_commandBar);
                }
                return _commandBar;
            }
        }

        #endregion Properties


        #region Constructors

        public ECSBarUserControl()
        {
            InitializeComponent();
        }

        #endregion Constructors


        #region Methods
        public void HideCommandeBar()
        {
            CommandBar.Visible = false;
        }

        public void Register(string commandKey, ICommand command, string caption, BarShortcut shortcut = null, BarItemLinkAlignment cmdAlignment = BarItemLinkAlignment.Left)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(commandKey)) { throw new ArgumentNullException(nameof(commandKey)); }
            if (command == null) { throw new ArgumentNullException(nameof(command)); }

            CommandBar.Register(commandKey, command, caption, shortcut, cmdAlignment);
            command.Key = commandKey;
        }

        public void ChangeCommandVisibility(string commandKey, BarItemVisibility visibility)
        {
            CommandBar.ChangeCommandVisibility(commandKey, visibility);
        }

        #endregion Methods
    }
}