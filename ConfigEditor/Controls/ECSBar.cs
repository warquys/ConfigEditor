using DevExpress.XtraBars;


namespace ConfigEditor.Controls
{
    public class ECSBar : Bar
    {
        #region Attributes & Properties
        /*public override bool Visible
		{
			get { return base.Visible && HasVisibleLinks(); }
			set { base.Visible = value; }
		}*/
        #endregion


        #region Constructors & Destructor
        public ECSBar() : base()
        {
            InitializeBar();
        }

        public ECSBar(BarManager manager, string name) : base(manager, name)
        {
            InitializeBar();
        }
        #endregion


        #region Methods
        private void InitializeBar()
        {
            CanDockStyle = BarCanDockStyle.Top;
            DockStyle = BarDockStyle.Top;
            //
            OptionsBar.UseWholeRow = true;
            OptionsBar.AllowDelete = false;
            OptionsBar.AllowQuickCustomization = false;
            OptionsBar.DrawDragBorder = false;
        }
        #endregion
    }
}