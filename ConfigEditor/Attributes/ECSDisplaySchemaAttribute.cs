using System;


namespace ConfigEditor.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ECSDisplaySchemaAttribute : Attribute
    {
        #region Attributes & Properties
        public Type DisplayType { get; private set; }
        #endregion


        #region Constructors & Destructor
        public ECSDisplaySchemaAttribute(Type displayType) : base()
        {
            // Safe design
            if (displayType == null) { throw new ArgumentNullException(nameof(displayType)); }

            this.DisplayType = displayType;
        }
        #endregion


        #region Methods
        #endregion
    }
}
