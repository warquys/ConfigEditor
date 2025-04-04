using ConfigEditor.Elements;
using ConfigEditor.Interfaces;
using ConfigEditor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigEditor.Managers
{
    internal class CompletorValueManager : FixedListManager<CompletorValue>, IWriteManager
    {
        private Completor _completor;
        public object CurrentObject { get; set; }
        public CompletorValue Current => CurrentObject as CompletorValue;
        public void PrepareNew()
        {
            CurrentObject = new CompletorValue();
        }
        public CompletorValueManager()
        {

        }

        public override DelStatus Delete(CompletorValue element)
        {
            var result = base.Delete(element);
            SymlEditorConfig.Singleton.Save();
            return result;
        }
        public void LoadList(Completor completor)
        {
            _completor = completor;
            _results = completor is null ? new List<CompletorValue> () : completor.ListValues;
            LoadList();
        }

        public bool Save()
        {
            if (_completor != null)
            {
                if (Current.IsNew())
                {
                    Current.Id = SymlEditorConfig.Singleton.GetCompletorValueId();
                    _completor.ListValues.Add(Current);
                    LoadList();
                }
                SymlEditorConfig.Singleton.Save();
                OnElementListUpdated();
            }
            return (_completor != null);
        }
    }
}
