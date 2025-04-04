using System;


namespace ConfigEditor.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ECSDisplayColumnAttribute : ECSDisplayAttribute
    {
        #region Nested
        public enum TextAlign
        {
            Default = 0,
            Left = 1,
            Center = 2,
            Right = 3
        }
        #endregion


        #region Attributes & Properties
        public int Order { get; set; }
        public int Width { get; private set; }
        public TextAlign Align { get; private set; }
        #endregion


        #region Constructors & Destructor
        public ECSDisplayColumnAttribute(string displayName) : base(displayName)
        {

        }

        public ECSDisplayColumnAttribute(string displayName, TextAlign textAlign) : base(displayName)
        {
            this.Align = textAlign;
        }

        public ECSDisplayColumnAttribute(string displayName, int order) : base(displayName)
        {
            this.Order = order;
        }

        public ECSDisplayColumnAttribute(string displayName, int order, int width) : this(displayName, order, width, TextAlign.Default)
        {

        }

        public ECSDisplayColumnAttribute(string displayName, int order, int width, TextAlign textAlign) : base(displayName)
        {
            this.Order = order;
            this.Width = width;
            this.Align = textAlign;
        }
        #endregion


        #region Methods
        #endregion
    }
}