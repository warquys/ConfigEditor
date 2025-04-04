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
    internal class CompletorManager : FixedListManager<Completor>, IWriteManager
    {
        public object CurrentObject { get; set; }
        public Completor Current => CurrentObject as Completor;
        
        public void PrepareNew()
        {
            CurrentObject = new Completor();
            Current.CompletorType = CompletorType.ByValue;
        }

        public CompletorManager() : base(SymlEditorConfig.Singleton.Completors)
        {

        }

        public override DelStatus Delete(Completor element)
        {
            var result = base.Delete(element);
            SymlEditorConfig.Singleton.Save();
            return result;
        }
        
        public bool Save()
        {
            if (Current.IsNew())
            {
                Current.Id = SymlEditorConfig.Singleton.GetCompletorId();
                SymlEditorConfig.Singleton.Completors.Add(Current);
                LoadList(SymlEditorConfig.Singleton.Completors);
                OnElementListUpdated();
            }
            SymlEditorConfig.Singleton.Save();
            return true;
        }
    }
}
