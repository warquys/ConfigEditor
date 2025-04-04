using System;


namespace ConfigEditor.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ECSDisplayColumnGroupAttribute : ECSDisplayColumnAttribute
    {
        #region Nested
        public enum SortOrder
        {
            Default = 0,
            Asc = 1,
            Desc = 2
        }

        public enum DateRange
        {
            Default = 1,
            Day = 2,
            Month = 3,
            Year = 4
        }
        #endregion


        #region Attributes & Properties
        //private int _groupIndex = -1;
        public int GroupIndex { get; private set; }
        //{
        //	get { return _groupIndex; }
        //	set { _groupIndex = value; }
        //}

        public DateRange GroupDate { get; private set; }
        public SortOrder GroupSort { get; private set; }
        #endregion


        #region Constructors & Destructor
        public ECSDisplayColumnGroupAttribute(string displayName) : base(displayName)
        {
            GroupIndex = -1;
        }

        public ECSDisplayColumnGroupAttribute(string displayName, int index) : base(displayName)
        {
            GroupIndex = index;
        }

        public ECSDisplayColumnGroupAttribute(string displayName, int index, DateRange dateRange, SortOrder dateSort) : base(displayName)
        {
            GroupDate = dateRange;
            GroupSort = dateSort;
        }
        #endregion


        #region Methods
        #endregion
    }
}