using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using ConfigEditor.Attributes;
using ConfigEditor.Interfaces;
using ConfigEditor.Utils;

namespace ConfigEditor.Controls
{
    public sealed class ECSGridView : GridView, ICommandRegistrator
    {
        #region Properties

        private ToolTipController _toolTipController = new ToolTipController { AutoPopDelay = 20000 };
        private bool _colPolutated = false;

        private int _nbMenuItemAtEnd = 0;
        private IDictionary<string, ECSDisplayAttribute> _displayColumnAttributes;
        private GridViewMenu _gridViewMenu;

        public event EventHandler SelectionRestored;


        public bool MultiSelect
        {
            get { return this.OptionsSelection.MultiSelect; }
            set
            {
                this.OptionsSelection.MultiSelect = value;
                this.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
                this.OptionsSelection.UseIndicatorForSelection = value;
            }
        }

        /// <summary>
        /// Set by the FocusedRowChanged event of the GridView
        /// </summary>
        public object LastSelectedItem { get; set; }

        protected override string ViewName
        {
            get { return nameof(ECSGridView); }
        }

        private DefaultBoolean _allowSort = DefaultBoolean.Default;
        public DefaultBoolean AllowSort
        {
            get { return _allowSort; }
            set
            {
                _allowSort = value;
                foreach (GridColumn column in Columns)
                {
                    column.OptionsColumn.AllowSort = _allowSort;
                }
            }
        }

        private Type _displayView;
        /// <summary>
        /// Gets or sets the Type that should be displayed in the GridView
        /// </summary>
        public Type DisplayView
        {
            get { return _displayView; }
            set
            {
                if (_displayView != value)
                {
                    _displayView = value;
                    if (this.Columns.Count > 0 || _colPolutated)
                    {
                        this.PopulateColumns();
                    }
                    DisplayViewChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Invoked when property DisplayView is set with a different value than existing
        /// </summary>
        public event EventHandler DisplayViewChanged;

        #endregion Properties


        #region Constructors

        public ECSGridView()
        {
            InitializeOptions();
        }

        public ECSGridView(GridControl ownerGrid) : base(ownerGrid)
        {
            InitializeOptions();
        }

        #endregion Properties


        #region Methods

        public T FocusedElement<T>()
        {
            return GetFocusedRow() == null ? default(T) : (T)GetFocusedRow();
        }

        public void FocusElement<T>(T element)
        {
            if (element != null)
            {
                this.BeginUpdate();
                int idx = FindRowHandleByObject(element);
                if (idx != InvalidRowHandle)
                {
                    FocusedRowHandle = idx;
                }
                this.EndUpdate();
            }

        }

        public void SelectElements<T>(IEnumerable<T> elements)
        {
            if (elements != null && elements.Any())
            {
                this.BeginUpdate();
                foreach (T item in elements)
                {
                    int idx = FindRowHandleByObject(item);
                    if (idx != InvalidRowHandle)
                    {
                        //int rowHandle = GetRowHandle(idx);
                        this.SelectRow(idx);
                    }
                }
                this.EndUpdate();
            }
        }


        public IEnumerable<T> SelectedElements<T>()
        {
            IList<T> result = SelectedRows<T>();
            if ((result == null || !result.Any()) && FocusedElement<T>() != null)
            {
                result = new List<T>();
                result.Add(FocusedElement<T>());
            }
            return result;
        }

        public IList<T> SelectedRows<T>()
        {
            int[] selectedHandle = GetSelectedRows();
            var lignes = new List<T>();
            foreach (int handle in selectedHandle)
            {
                object element = GetRow(handle);
                if (element != null && element is T)
                {
                    lignes.Add((T)element);
                }
            }
            return lignes;
        }

        private void InitializeOptions()
        {
            // Options
            InitGridOptions();
            this.PopupMenuShowing += GridView_PopupMenuShowing;
            // Events
            this.FocusedRowChanged += GridView_FocusedRowChanged;

            // Menu
            _gridViewMenu = new GridViewMenu(this);

        }

        public void SaveSelection()
        {
            _topRowIndex = this.TopRowIndex;
            _selectedLines = this.MultiSelect ? this.SelectedElements<object>() : null;
            _focusedLine = this.FocusedElement<object>();
        }

        public void RestoreSelection()
        {
            this.TopRowIndex = _topRowIndex;
            if (this.MultiSelect)
            {
                this.SelectElements<object>(_selectedLines);
            }
            //	this.FocusedRowHandle = InvalidRowHandle;
            this.FocusElement(_focusedLine);
            OnSelectionRestored();
        }

        private void RestoreOptions(List<bool> options)
        {
            this.OptionsBehavior.ReadOnly = options[0];
            this.OptionsBehavior.Editable = options[1];
            this.OptionsView.ShowGroupPanel = options[2];
            this.OptionsView.ColumnAutoWidth = options[3];
            this.OptionsView.ShowAutoFilterRow = options[4];
            this.OptionsView.EnableAppearanceEvenRow = options[5];
            this.OptionsView.EnableAppearanceOddRow = options[6];
            this.OptionsBehavior.AutoExpandAllGroups = options[7];
            this.OptionsView.ShowGroupedColumns = options[8];
        }

        private List<bool> SaveGridOptions()
        {
            List<bool> result = new List<bool>();
            result.Add(this.OptionsBehavior.ReadOnly);
            result.Add(this.OptionsBehavior.Editable);
            result.Add(this.OptionsView.ShowGroupPanel);
            result.Add(this.OptionsView.ColumnAutoWidth);
            result.Add(this.OptionsView.ShowAutoFilterRow);
            result.Add(this.OptionsView.EnableAppearanceEvenRow);
            result.Add(this.OptionsView.EnableAppearanceOddRow);
            result.Add(this.OptionsBehavior.AutoExpandAllGroups);
            result.Add(this.OptionsView.ShowGroupedColumns);
            return result;
        }

        private void InitGridOptions()
        {
            this.OptionsBehavior.ReadOnly = true;
            this.OptionsBehavior.Editable = false;
            this.OptionsView.ShowGroupPanel = false;
            this.OptionsView.ColumnAutoWidth = false;
            this.OptionsView.RowAutoHeight = true;
            this.OptionsView.ShowAutoFilterRow = false;
            this.OptionsView.EnableAppearanceEvenRow = true;
            this.OptionsView.EnableAppearanceOddRow = true;
            // Group - Options
            this.OptionsBehavior.AutoExpandAllGroups = true;
            this.OptionsView.ShowGroupedColumns = true;
            this.GroupSummary.Add(SummaryItemType.Count, String.Empty, null, "({0})");
        }

        public override void PopulateColumns()
        {
            bool multiSelect = this.MultiSelect;
            _colPolutated = true;

            this.GridControl.ToolTipController = _toolTipController;

            Type type = DisplayView ?? DataController.DataSource.GetType().GetGenericArguments()[0];
            _displayColumnAttributes = ECSDisplayAnalyzer.Analyze(type);

            base.PopulateColumns();

            if (DisplayView != ECSDisplayAnalyzer.GetDisplayType(type))
            {
                // Le vrai type derriere l'affichage
                _displayView = ECSDisplayAnalyzer.GetDisplayType(type);
                DisplayViewChanged?.Invoke(this, EventArgs.Empty);
            }


            // Desactive le hint DevExpress quand la collone est plus petit que le
            // Son titre
            this.OptionsHint.ShowColumnHeaderHints = false;
         }

        public event EventHandler LayoutResetEvent;

        private void OnLayoutReset()
        {
            LayoutResetEvent?.Invoke(this, new EventArgs());
        }


        protected override GridColumn PopulateColumn(DataColumnInfo columnInfo, int columnHandle, ref int columnIndex)
        {
            if (_displayColumnAttributes != null)
            {
                if (_displayColumnAttributes.ContainsKey(columnInfo.PropertyDescriptor.Name))
                {
                    ECSDisplayAttribute displayAttribute = _displayColumnAttributes[columnInfo.PropertyDescriptor.Name];
                    GridColumn column = base.PopulateColumn(columnInfo, columnHandle, ref columnIndex);
                    column.Caption = displayAttribute.DisplayName;
                    column.OptionsColumn.AllowSort = _allowSort;
                    ECSEditFormatAttribute attrDisplayFormat = columnInfo.PropertyDescriptor.Attributes[typeof(ECSEditFormatAttribute)] as ECSEditFormatAttribute;
                    if (attrDisplayFormat != null && attrDisplayFormat.MaskType == ECSMaskType.Numeric)
                    {
                        column.DisplayFormat.FormatType = FormatType.Numeric;
                        column.DisplayFormat.FormatString = attrDisplayFormat.Mask;
                    }
                    if (attrDisplayFormat != null && attrDisplayFormat.MaskType == ECSMaskType.DateTime)
                    {
                        column.DisplayFormat.FormatType = FormatType.DateTime;
                        column.DisplayFormat.FormatString = attrDisplayFormat.Mask;
                    }

                    if (displayAttribute is ECSDisplayColumnAttribute)
                    {
                        var columnDisplayAttribute = displayAttribute as ECSDisplayColumnAttribute;


                        // Order
                        columnIndex = columnDisplayAttribute.Order;
                        // Size
                        if (columnDisplayAttribute.Width > 0)
                        {
                            column.Width = columnDisplayAttribute.Width * 10;
                        }
                        else
                        {
                            column.BestFit();
                        }

                        // Align
                        if (columnDisplayAttribute.Align != ECSDisplayColumnAttribute.TextAlign.Default)
                        {
                            column.AppearanceHeader.Options.UseTextOptions = true;
                            column.AppearanceCell.Options.UseTextOptions = true;
                            column.AppearanceCell.TextOptions.HAlignment = (HorzAlignment)columnDisplayAttribute.Align;
                            column.AppearanceHeader.TextOptions.HAlignment = column.AppearanceCell.TextOptions.HAlignment;
                        }

                        // Group
                        var groupDisplayAttribute = columnDisplayAttribute as ECSDisplayColumnGroupAttribute;
                        if (groupDisplayAttribute != null)
                        {
                            column.GroupIndex = groupDisplayAttribute.GroupIndex;
                            column.OptionsColumn.AllowGroup = DefaultBoolean.True;
                            column.OptionsColumn.AllowSort = DefaultBoolean.True;
                            switch (groupDisplayAttribute.GroupDate)
                            {
                                case ECSDisplayColumnGroupAttribute.DateRange.Day:
                                    column.GroupInterval = ColumnGroupInterval.Date;
                                    column.SortOrder = (ColumnSortOrder)groupDisplayAttribute.GroupSort;
                                    break;

                                case ECSDisplayColumnGroupAttribute.DateRange.Month:
                                    column.GroupInterval = ColumnGroupInterval.DateMonth;
                                    column.SortOrder = (ColumnSortOrder)groupDisplayAttribute.GroupSort;
                                    break;

                                case ECSDisplayColumnGroupAttribute.DateRange.Year:
                                    column.GroupInterval = ColumnGroupInterval.DateYear;
                                    column.SortOrder = (ColumnSortOrder)groupDisplayAttribute.GroupSort;
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        column.BestFit();
                    }

                    return column;
                }
            }

            return null;
        }

        #endregion Methods


        #region Methods - ICommandRegistrator

        private void Register(string commandKey, ICommand command, string caption, GridViewMenu menu)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(commandKey)) { throw new ArgumentNullException(nameof(commandKey)); }
            if (command == null) { throw new ArgumentNullException(nameof(command)); }

            this.BeginUpdate();
            //
            command.Key = commandKey;
            DXMenuItem menuItem = new ECSCommandMenuItem(commandKey, command, caption);
            menuItem.ImageOptions.Image = ECSImageUtility.GetImage(commandKey, 16);
            menu.Items.Add(menuItem);
            //
            this.EndUpdate();
        }

        private void Register(string commandKey, ICommand command, string caption, bool groupe, bool atEnd)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(commandKey)) { throw new ArgumentNullException(nameof(commandKey)); }
            if (command == null) { throw new ArgumentNullException(nameof(command)); }

            this.BeginUpdate();
            //
            command.Key = commandKey;
            DXMenuItem menuItem = new ECSCommandMenuItem(commandKey, command, caption);
            menuItem.ImageOptions.Image = ECSImageUtility.GetImage(commandKey, 16);
            menuItem.BeginGroup = groupe;

            if (!atEnd)
            {
                if (_nbMenuItemAtEnd > 0)
                {
                    _gridViewMenu.Items.Insert(_gridViewMenu.Items.Count - _nbMenuItemAtEnd, menuItem);
                }
                else
                {
                    _gridViewMenu.Items.Add(menuItem);
                }
            }
            else
            {
                _gridViewMenu.Items.Add(menuItem);
                _nbMenuItemAtEnd++;
            }
            //
            this.EndUpdate();
        }

        private void GridView_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            // Safe design
            if (e == null) { throw new ArgumentNullException(nameof(e)); }


            switch (e.HitInfo)
            {
                case GridHitInfo hitInfo when hitInfo.InRow && !hitInfo.InGroupRow:
                    e.Menu = this._gridViewMenu;
                    e.Menu.ShowPopup(this.GridControl, e.Point);
                    e.Allow = true;
                    break;
            }
        }

        public void Register(string commandKey, ICommand command, string caption, BarShortcut shortcut = null, BarItemLinkAlignment cmdAlignment = BarItemLinkAlignment.Left)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(commandKey)) { throw new ArgumentNullException(nameof(commandKey)); }
            if (command == null) { throw new ArgumentNullException(nameof(command)); }
            Register(commandKey, command, caption, false, false);
        }

        private int FindRowHandleByObject(object row)
        {
            if (row != null)
            {
                for (int i = 0; i < this.DataRowCount; i++)
                {
                    if (row.Equals(this.GetRow(i)))
                    {
                        return i;
                    }
                }
            }
            return InvalidRowHandle;
        }

        public void ReselectItem(object item)
        {

            this.BeginSelection();
            if (item != null)
            {
                int idx = FindRowHandleByObject(item);
                if (idx != InvalidRowHandle)
                {
                    int rowHandle = GetRowHandle(idx);
                    FocusedRowHandle = rowHandle;
                }
            }
            this.EndSelection();
        }

        #endregion Methods - ICommandRegistrator


        #region Events

        private void GridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            LastSelectedItem = GetFocusedRow();
        }

        private static Dictionary<Type, object> _dicTypeDefaultValue = new Dictionary<Type, object>();
        private int _topRowIndex;
        private IEnumerable<object> _selectedLines;
        private object _focusedLine;

        private object GetDefaultValue(Type type)
        {
            if (!_dicTypeDefaultValue.ContainsKey(type))
            {
                if (type == typeof(DateTime))
                {
                    _dicTypeDefaultValue[type] = new DateTime(9999, 12, 31);
                }
                else
                {
                    _dicTypeDefaultValue[type] = Activator.CreateInstance(type);
                }
            }
            return _dicTypeDefaultValue[type];
        }

        private bool IsDefaultValue(object value)
        {
            Type type = value.GetType();

            object defValue = GetDefaultValue(type);

            return (value.Equals(defValue));
        }


        private void OnSelectionRestored()
        {
            if (SelectionRestored != null)
            {
                SelectionRestored(this, EventArgs.Empty);
            }
        }
        #endregion Events
    }
}