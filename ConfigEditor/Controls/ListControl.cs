using ConfigEditor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigEditor.Controls
{
    public partial class ListControl<TElement> : ECSGridUserControl<TElement>
    {
        #region Properties
        // TODO Repasser en protected
        public IListManager<TElement> Manager { get; private set; }

        public override ECSGridView GridView { get { return _gridView; } }

        /// <summary>
        /// Mechanism to display a different set of columns in the list view. 
        /// </summary>
        public Type DisplayView
        {
            get { return _gridView.DisplayView; }
            set { _gridView.DisplayView = value; }
        }

        public bool AllowSort
        {
            get
            {
                return GridView.OptionsCustomization.AllowSort;

            }
            set
            {
                GridView.OptionsCustomization.AllowSort = value;
            }
        }

        public List<string> InCellEditFields { get; set; }

        #endregion Properties


        #region Constructors

        public ListControl()
        {
            // Common
            InitializeComponent();
            InitializeGridUserControl();
        }

        public ListControl(IListManager<TElement> manager, bool forceLoad = true)
        {
            // Safe design
            if (manager == null) { throw new ArgumentNullException(nameof(manager)); }

            // Common
            InitializeComponent();
            InitializeGridUserControl();

            // Manager
            Manager = manager;
            Manager.ElementListUpdated += Manager_ElementListUpdated;

            // Grid
            _gridControl.DataSource = this.Manager.ElementList;

            // Load
            if (forceLoad)
            {
                new DialogWaitCommand(() => this.Manager.LoadList()).Execute();
            }
        }

        #endregion Constructors


        #region Events

        public void Manager_ElementListUpdated(object sender, EventArgs e)
        {
            _gridView.SaveSelection();
            //object item = _gridView.LastSelectedItem;
            _gridView.FocusInvalidRow();
            _gridView.RefreshData();
            _gridView.RestoreSelection();
            //_gridView.ReselectItem(item);
        }

        #endregion Events
    }
}