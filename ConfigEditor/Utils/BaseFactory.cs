using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigEditor.Utils
{
    public abstract class BaseFactory<T>
    {
        #region Attributes & Properties

        private readonly Dictionary<Type, Func<T>> _factoryMap = new Dictionary<Type, Func<T>>();
        protected IReadOnlyDictionary<Type, Func<T>> FactoryMap { get { return _factoryMap; } }

        #endregion


        #region Constructors & Destructor

        protected BaseFactory() 
        { 
        
        }

        #endregion


        #region Methods

        public abstract T Create(Type type);


        protected void Register(Type type, Func<T> func)
        {
            // Safe design
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            if (func == null) { throw new ArgumentNullException(nameof(func)); }

            _factoryMap.Add(type, func);
        }
        #endregion


        #region Methods - Generic<TKey>()

        public T Create<TKey>()
        {
            return Create(typeof(TKey));
        }

        protected void Register<TKey>(Func<T> func)
        {
            // Safe design
            if (func == null) { throw new ArgumentNullException(nameof(func)); }

            _factoryMap.Add(typeof(TKey), func);
        }
        #endregion Methods - Generic<TKey>()
    }
}
