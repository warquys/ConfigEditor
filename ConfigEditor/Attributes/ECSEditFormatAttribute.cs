using System;

namespace ConfigEditor.Attributes
{
    public enum ECSMaskType
    {
        None = 0,
        DateTime = 1,
        DateTimeAdvancingCaret = 2,
        Numeric = 3,
        RegEx = 4,
        Regular = 5,
        Simple = 6,
        Custom = 7,
        TimeSpan = 8,
        TimeSpanAdvancingCaret = 9
    }

    public class ECSEditFormatAttribute : Attribute
    {
        public ECSMaskType MaskType { get; set; }

        public string Mask { get; set; }

        public bool UseMaskAsDisplayFormat { get; set; }
    }
}
