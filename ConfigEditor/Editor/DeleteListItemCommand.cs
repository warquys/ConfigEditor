using ConfigEditor.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigEditor.Editor
{
    public class DeleteListItemCommand : BaseCommand<SymlContentItem>
    {
        #region Attributes & Properties
        protected override bool CanExecuteValue => base.CanExecuteValue && Parameter.IsListItem;
        private SymlDetailManager _managerDetail;


        #endregion

        #region Constructors & Destructor
        public DeleteListItemCommand(SymlDetailManager managerDetail)
        {
            _managerDetail = managerDetail;
        }

        #endregion

        #region Methods
        protected override void ExecuteCommand()
        {
            _managerDetail.DeleteListEntry(Parameter);
        }
        #endregion
    }
}
