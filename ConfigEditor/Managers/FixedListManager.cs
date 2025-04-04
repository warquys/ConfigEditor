using ConfigEditor.Elements;
using ConfigEditor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigEditor.Managers

{
    public class FixedListManager<T> : IListManager<T>
    {

        #region Attributes & Properties
        protected List<T> _results;

        private readonly List<T> _elements = new List<T>();

        protected List<T> Elements { get { return _elements; } }

        public IEnumerable<T> ElementList { get { return _elements; } }

        public event EventHandler ElementListUpdated;

        #endregion

        #region Constructors & Destructor
        public FixedListManager()
        {
            _results = new List<T>();
        }

        public FixedListManager(List<T> list)
        {
            // Safe design
            if (list == null) { throw new ArgumentNullException(nameof(list)); }

            _results = list;
        }

        #endregion

        #region Methods
        public virtual DelStatus Delete(T element)
        {
            _results.Remove(element);
            LoadList();
            
            return DelStatus.Success;
        }

        public IDictionary<T, DelStatus> Delete(IEnumerable<T> element)
        {
            throw new NotImplementedException();
        }

        protected void OnElementListUpdated()
        {
            if (ElementListUpdated != null)
            {
                ElementListUpdated(this, EventArgs.Empty);
            }
        }

        public void LoadList()
        {
            Elements.Clear();

            if (_results != null)
            {
                if (_results.Count() > 0)
                {
                    Elements.AddRange(_results);
                }
                OnElementListUpdated();
            }
        }

        public void LoadList(List<T> elements)
        {
            _results = elements;
            LoadList();
        }

        public void UnloadList()
        {
            Elements.Clear();
            OnElementListUpdated();
        }

        public void UpdateElement(object sender)
        {
            OnElementListUpdated();
        }
        #endregion


    }
}
