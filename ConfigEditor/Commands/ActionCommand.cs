using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigEditor.Commands
{
    class ActionCommand : BaseCommand
    {
        private Action _action;
        public ActionCommand(Action action)
        {
            // Safe design
            if (action == null) { throw new ArgumentNullException(nameof(action)); }
            _action = action;
        }
        protected override void ExecuteCommand()
        {
            _action.Invoke();
        }
    }
}
