using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ConfigEditor.Utils
{
    /// <summary>
    /// Class that keeps track of the changes happening to data bindings
    /// </summary>
    public class Memento
    {
        #region Internal Class

        private class State
        {
            public object Initial { get; set; }
            public object Current { get; set; }

            public bool Changed
            {
                get
                {
                    return !Object.Equals(Initial, Current);
                }
            }

            public State(object initial, object current)
            {
                Initial = initial;
                Current = current;
            }
        }

        #endregion Internal Class


        #region Attributes & Properties

        public event EventHandler SourceStateChanged;
        public event EventHandler SourceReinitialized;

        private readonly List<BindingSource> _bindingSources = new List<BindingSource>();
        private readonly Dictionary<Binding, State> _dicValues = new Dictionary<Binding, State>();

        private bool _changed;
        /// <summary>
        /// True when a binding on the UI has a new value
        /// </summary>
        public bool Changed
        {
            get { return _changed; }
            set
            {
                if (_changed != value)
                {
                    _changed = value;
                    SourceStateChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        #endregion


        #region Constructors & Destructor

        public Memento(BindingSource bindingSource)
        {
            AddDataSource(bindingSource);
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            foreach (BindingSource datasource in _bindingSources)
            {
                datasource.Refresh();
            }
        }

        public bool BindingSourceChanged(BindingSource source)
        {
            foreach (Binding binding in source.CurrencyManager.Bindings)
            {
                if (_dicValues.ContainsKey(binding) && _dicValues[binding].Changed)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddDataSource(BindingSource bindingSource)
        {
            _bindingSources.Add(bindingSource);
            bindingSource.CurrencyManager.Bindings.CollectionChanged += BindingSource_CollectionChanged;
            bindingSource.BindingComplete += BindingSource_BindingComplete;
            bindingSource.CurrentItemChanged += BindingSource_CurrentItemChanged;
        }

        public void ValidateChange()
        {
            _dicValues.Values.ToList<State>().ForEach(state => state.Initial = state.Current);
            Changed = false;
        }

        public void ValidateChange(string field)
        {
            Binding key = _dicValues.Keys.FirstOrDefault(k => k.BindingMemberInfo.BindingField == field);
            if (key != null)
            {
                State state = _dicValues[key];
                state.Initial = state.Current;
            }
            Changed = _dicValues.Values.Any(s => s.Changed);
        }

        public void InvalidateField(string field, object initialValue)
        {
            Binding key = _dicValues.Keys.FirstOrDefault(k => k.BindingMemberInfo.BindingField == field);
            if (key != null)
            {
                State state = _dicValues[key];
                state.Initial = initialValue;
            }
            Changed = _dicValues.Values.Any(s => s.Changed);
        }

        public bool UpdateFromSource()
        {
            bool result = false;
            foreach (BindingSource source in _bindingSources)
            {
                UpdateStateFromSource(source);
                result = true;
            }
            Changed = false;
            return result;
        }

        public bool ResetChange()
        {
            bool result = false;

            foreach (BindingSource source in _bindingSources)
            {
                ResetSource(source);
                result = true;
            }
            Changed = _dicValues.Values.Any(s => s.Changed);
            return result;
        }

        public bool PropertyChanged(string propertyName)
        {
            // Safe design
            if (propertyName == null) { throw new ArgumentNullException(nameof(propertyName)); }

            bool result = false;
            foreach (var key in _dicValues.Keys)
            {
                if (key.BindingMemberInfo.BindingField == propertyName)
                {
                    State valueInfo = _dicValues[key];
                    result = result || valueInfo.Changed;
                }
            }
            /*			if (_dicValues.ContainsKey(propertyName))
						{
							State valueInfo = _dicValues[propertyName];
							return valueInfo.Changed;
						}
						*/
            return result;
        }

        private void UpdateMemento(Binding binding)
        {
            if (GetCurrentValue(binding, out object currentValue))
            {
                string propertyName = binding.BindingMemberInfo.BindingField;
                State valueInfo = _dicValues[binding];
                _dicValues[binding] = new State(valueInfo.Initial, currentValue);
                Changed = _dicValues.Values.Any(state => state.Changed);
            }
        }

        private void UpdateStateFromSource(BindingSource source)
        {
            object element = source.Current;
            if (element != null)
            {
                source.CurrentItemChanged -= BindingSource_CurrentItemChanged;
                foreach (Binding binding in source.CurrencyManager.Bindings)
                {
                    string pptName = binding.BindingMemberInfo.BindingField;
                    Type modelType = element.GetType();
                    PropertyInfo modelProp = modelType.GetProperty(pptName);
                    if (modelProp != null && modelProp.CanRead)
                    {
                        _dicValues[binding].Initial = modelProp.GetValue(element);
                        _dicValues[binding].Current = _dicValues[binding].Initial;
                    }
                }

                source.ResetCurrentItem();
                source.CurrentItemChanged += BindingSource_CurrentItemChanged;
                SourceReinitialized?.Invoke(this, EventArgs.Empty);
            }
        }

        private void ResetSource(BindingSource source)
        {
            object element = source.Current;
            if (element != null)
            {
                source.CurrentItemChanged -= new EventHandler(BindingSource_CurrentItemChanged);
                foreach (Binding binding in source.CurrencyManager.Bindings)
                {
                    string pptName = binding.BindingMemberInfo.BindingField;
                    Type modelType = element.GetType();
                    PropertyInfo modelProp = modelType.GetProperty(pptName);
                    if (modelProp != null && modelProp.CanWrite)
                    {
                        modelProp.SetValue(element, _dicValues[binding].Initial);
                        _dicValues[binding].Current = _dicValues[binding].Initial;
                    }
                }

                source.ResetCurrentItem();
                source.CurrentItemChanged += new EventHandler(BindingSource_CurrentItemChanged);
                SourceReinitialized?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool GetCurrentValue(Binding binding, out object value)
        {
            if (binding.BindingManagerBase != null && binding.BindingManagerBase.Count > 0)
            {
                object element = binding.BindingManagerBase.Current;
                if (element != null)
                {
                    Type modelType = element.GetType();
                    string propertyName = binding.BindingMemberInfo.BindingField;
                    PropertyInfo modelProp = modelType.GetProperty(propertyName);
                    value = modelProp.GetValue(element);
                    return true;
                }
            }
            value = null;
            return false;
        }

        #endregion

        #region Event

        private void BindingSource_CurrentItemChanged(object sender, EventArgs e)
        {
            if (_dicValues.Count > 0)
            {
                foreach (BindingSource source in _bindingSources)
                {
                    foreach (Binding binding in source.CurrencyManager.Bindings)
                    {
                        UpdateMemento(binding);
                    }
                }
            }

        }

        private void BindingSource_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            if (e.Element is Binding binding && GetCurrentValue(binding, out object value))
            {
                if (!_dicValues.ContainsKey(binding))

                {
                    _dicValues[binding] = new State(value, value);
                }
            }
        }

        private void BindingSource_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteContext == BindingCompleteContext.DataSourceUpdate &&
                e.BindingCompleteState == BindingCompleteState.Success)
            {
                // la collection ne change plus correction du bug des changement de docking qui provoque un changement de collection dans les binding
                //bindingSource.CurrencyManager.Bindings.CollectionChanged -= BindingSource_CollectionChanged;
                UpdateMemento(e.Binding);
            }
        }

        #endregion

    }
}
