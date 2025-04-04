using ConfigEditor.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigEditor.Editor
{
    public class AddListItemCommand : BaseCommand<SymlContentItem>
    {
        #region Attributes & Properties
        protected override bool CanExecuteValue => base.CanExecuteValue && (Parameter.IsList || Parameter.IsListItem);
        private SymlDetailManager _managerDetail;


        #endregion

        #region Constructors & Destructor
        public AddListItemCommand(SymlDetailManager managerDetail)
        {
            _managerDetail = managerDetail;
        }

        #endregion

        #region Methods
        protected override void ExecuteCommand()
        {
            _managerDetail.CreateListEntry(Parameter);
        }
        #endregion

    }
}
