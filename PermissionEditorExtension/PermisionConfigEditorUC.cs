using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigEditor.Commands;
using ConfigEditor.Editor;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.Parameters;

namespace PermissionEditorExtension
{
    public partial class PermissionConfigEditorUC : SymlConfigEditorUC
    {
        #region Nested
        // Changer cella en de la composition ? Et tous déplacer dans la factory ?
        protected class DelSectionCommand : BaseCommand<SymlSection>
        {
            #region Attributes & Properties
            protected override bool CanExecuteValue => Parameter != null && Parameter.Name != "Server";
            private SymlSectionManager _manager;


            #endregion

            #region Constructors & Destructor
            public DelSectionCommand(SymlSectionManager manager)
            {
                _manager = manager;
            }

            #endregion

            #region Methods
            protected override void ExecuteCommand()
            {
                _manager.Delete(Parameter);
            }
            #endregion

        }
        protected class AddSectionCommand : BaseCommand
        {
            #region Attributes & Properties
            protected override bool CanExecuteValue => _manager.ElementList.Any();
            private SymlSectionManager _manager;

            private const string emptyPermissionSection = @"[/NAME/]
{
# If Enabled this Group will be assigned to all players, which are in no other Group
default: false
# If Enabled this Group will be assigned to Northwood staff players, which are in no other Group
northwood: false
# If Enabled this Group has Acces to RemoteAdmin
remoteAdmin: false
# The Badge which will be displayed in game
badge: NONE
# The Color which the Badge has in game
color: NONE
# If Enabled The Badge of this Group will be displayed instead of the global Badge
cover: false
# If Enabled the Badge is Hidden by default
hidden: false
# The KickPower the group has
kickPower: 0
# The KickPower which is required to kick the group
requiredKickPower: 1
# The Permissions which the group has
permissions:
- synapse.command.help
- synapse.command.plugins
# Gives the Group the Permissions of all Groups in this List
inheritance: [ ]
# The UserID's of the Players in the Group
members: [ ]
}";

            #endregion

            #region Constructors & Destructor
            public AddSectionCommand(SymlSectionManager manager)
            {
                _manager = manager;
            }

            #endregion

            #region Methods
            protected override void ExecuteCommand()
            {
                string name = XtraInputBox.Show("Group name", "Section Name", "NewGroup");

                if (name == String.Empty)
                    return;

                _manager.CreateConfigSection(name, emptyPermissionSection);
            }
            #endregion

        }
        #endregion

        private AddSectionCommand addSectionCommand;
        private DelSectionCommand delSectionCommand;

        public PermissionConfigEditorUC() : base()
        {
            AddPermissionCommand();
        }

        private void InitWarning()
        {
            addSectionCommand.AfterExecute += (s, e) => Changed();
            delSectionCommand.AfterExecute += (s, e) => Changed();
        }

        private void InitCommand()
        {
            loadCommand.AfterExecute += (s, e) =>
            {
                addSectionCommand?.OnCanExecuteChanged();
            };
        }

        private void AddPermissionCommand()
        {
            addSectionCommand = new AddSectionCommand(_managerSection);
            delSectionCommand = new DelSectionCommand(_managerSection);
            _listSection.Register("Add", addSectionCommand, "Add Section", true, true, shortcut: new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            _listSection.Register("Del", delSectionCommand, "Del Section", true, true, shortcut: new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
        }

        

    }
}
