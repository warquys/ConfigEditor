using System;
using System.ComponentModel;


namespace ConfigEditor.Attributes
{
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ECSDisplayAttribute : DisplayNameAttribute
    {
        #region Attributes & Properties
        #endregion


        #region Constructors & Destructor
        public ECSDisplayAttribute(string displayName) : base(displayName)
        {
        }
        #endregion


        #region Methods
        #endregion
    }
}