using System;
using System.ComponentModel.DataAnnotations;


namespace ConfigEditor.Attributes
{
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ECSRequiredAttribute : RequiredAttribute
    {
        #region Properties

        public bool Enabled { get; set; }
        //
        public string DefaultText { get; set; }

        #endregion Properties


        #region Constructors
        public ECSRequiredAttribute() : this(true)
        { }

        public ECSRequiredAttribute(bool enabled)
        {
            Enabled = enabled;
            DefaultText = "This data is required!";
        }

        public ECSRequiredAttribute(string defaultText)
        {
            Enabled = true;
            DefaultText = defaultText;
        }

        #endregion Constructors


        #region Methods

        public override bool IsValid(object value)
        {
            return (Enabled) ? base.IsValid(value) : true;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return (Enabled) ? base.IsValid(value, validationContext) : ValidationResult.Success;
        }

        #endregion Methods
    }
}