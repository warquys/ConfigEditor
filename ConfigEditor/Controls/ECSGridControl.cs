using System;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Registrator;
using DevExpress.XtraGrid.Views.Base;


namespace ConfigEditor.Controls
{
    public class ECSGridControl : GridControl
    {
        #region Properties

        #endregion Properties


        #region Constructors

        public ECSGridControl() : base()
        { }

        #endregion Constructors

        #region Methods - Override

        protected override BaseView CreateDefaultView()
        {
            return (ECSGridView)base.CreateView(nameof(ECSGridView));
        }

        protected override void RegisterAvailableViewsCore(InfoCollection collection)
        {
            // Safe design
            if (collection == null) { throw new ArgumentNullException(nameof(collection)); }

            collection.Add(new ECSGridInfoRegistrator());
        }

        #endregion Methods - Override
    }
}