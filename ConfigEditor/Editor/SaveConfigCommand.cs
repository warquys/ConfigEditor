using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraRichEdit;
using ConfigEditor.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConfigEditor.Editor
{
    public class SaveConfigCommand : BaseCommand
    {
        private SymlSectionManager _managerSection;
        protected override bool CanExecuteValue => base.CanExecuteValue && _managerSection.ElementList.Any();
        public SaveConfigCommand(SymlSectionManager managerSection)
        {
            _managerSection = managerSection;
        }

        protected override void ExecuteCommand()
        {
            _managerSection.Save();
        }
    }
}
