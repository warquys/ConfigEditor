using ConfigEditor.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigEditor.Interfaces
{
    public interface IListManager<T> : IListManager
    {
        #region Attributes & Properties
        IEnumerable<T> ElementList { get; }
        event EventHandler ElementListUpdated;
        #endregion


        #region Methods
        DelStatus Delete(T element);
        IDictionary<T, DelStatus> Delete(IEnumerable<T> element);
        #endregion
    }

    public interface IListManager
    {
        #region Attributes & Properties
        #endregion


        #region Methods
        void LoadList();
        void UnloadList();
        void UpdateElement(object sender);
        #endregion
    }
}
