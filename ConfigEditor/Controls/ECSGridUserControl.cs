using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using ConfigEditor.Interfaces;
using DevExpress.XtraBars;

namespace ConfigEditor.Controls
{
    public abstract class ECSGridUserControl<T> : ECSGridUserControl
    {
        #region Properties

        private readonly List<ICommand<T>> _elementCommands = new List<ICommand<T>>();
        private readonly List<ICommand<IEnumerable<T>>> _elementListCommands = new List<ICommand<IEnumerable<T>>>();

        public T FocusedElement
        {
            get { return GridView.FocusedElement<T>(); }
            set
            {
                GridView.ReselectItem(value);
            }
        }

        public IEnumerable<T> SelectedElements
        {
            get
            {
                return GridView.SelectedElements<T>();
            }
            set
            {
                GridView.SelectElements<T>(value);
            }
        }
        #endregion Properties


        #region Methods - ICommandRegistrator


        public override void Register(string commandKey, ICommand command, string caption, bool onBar, bool onMenu, bool onDoubleClick = false, BarShortcut shortcut = null)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(commandKey)) { throw new ArgumentNullException(nameof(commandKey)); }
            if (command == null) { throw new ArgumentNullException(nameof(command)); }

            base.Register(commandKey, command, caption, onBar, onMenu, onDoubleClick, shortcut);
            // Enregistrement des commandes nécessitant un paramètre T
            AddCommand(command);
        }

        #endregion Methods - ICommandRegistrator


        #region Methods

        protected override void InitializeGridUserControl()
        {
            if (GridView != null)
            {
                base.InitializeGridUserControl();
                //GridView.FocusedRowChanged += GridView_FocusedRowChanged;
                GridView.FocusedRowObjectChanged += GridView_FocusedRowObjectChanged;
                GridView.SelectionChanged += GridView_SelectionChanged;
                GridView.DisplayView = typeof(T);
                GridView.RowCountChanged += (s, e) =>
                {
                    // pour updater les parametres des commandes
                    // si on ajoute un element ou en supprime un
                    // correction d une erreur
                    RefreshCommand();
                };

                GridView.MouseDown += GridView_MouseDown;
                GridView.SelectionRestored += GridView_SelectionRestored;
            }
        }

        private void GridView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                GridView.TopRowIndex = 0; // Will scroll to top
            }
        }

        public void AddCommand(ICommand command)
        {
            if (command is ICommand<T>)
            {
                _elementCommands.Add(command as ICommand<T>);
                (command as ICommand<T>).Parameter = (T)GridView.GetFocusedRow();
                //				command.CanExecute(GridView.GetFocusedRow());
            }
            else if (command is ICommand<IEnumerable<T>>)
            {
                _elementListCommands.Add(command as ICommand<IEnumerable<T>>);
                (command as ICommand<IEnumerable<T>>).Parameter = GridView.SelectedElements<T>().ToList();
            }
        }

        public IList<T> SelectedRows()
        {
            return GridView.SelectedRows<T>();
        }

        public override int EditHashCode
        {
            get { return typeof(T).GetHashCode(); }
        }

        #endregion Methods


        #region Events

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshMultiLineCommands();
        }

        protected void RefreshMultiLineCommands()
        {
            if (_elementListCommands.Count > 0)
            {
                IList<T> selected = GridView.SelectedElements<T>().ToList();
                foreach (ICommand<IEnumerable<T>> item in _elementListCommands)
                {
                    item.Parameter = selected;
                }
            }
        }

        protected void RefreshCommand()
        {
            var currentElm = (T)GridView.GetFocusedRow();
            foreach (ICommand<T> item in _elementCommands)
            {
                // pour faire un changement d etat
                item.Parameter = currentElm;
                item.OnCanExecuteChanged();
                //item.Parameter = default(T);
                //item.Parameter = currentElm;
            }
        }

        // JMT Do not use FocusedRowChanged use FocusedRowObjectChanged
        private void GridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            RefreshCommand();
            RefreshMultiLineCommands();
        }

        private void GridView_FocusedRowObjectChanged(object sender, FocusedRowObjectChangedEventArgs e)
        {
            RefreshCommand();
            RefreshMultiLineCommands();
        }

        private void GridView_SelectionRestored(object sender, EventArgs e)
        {
            RefreshCommand();
            RefreshMultiLineCommands();
        }
        #endregion Events
    }


    public abstract class ECSGridUserControl : ECSBarUserControl, ISelectableControl
    {
        #region Attributes & Properties
        public object GetSelectedItem
        {
            get { return GridView?.GetFocusedRow(); }
        }
        public abstract ECSGridView GridView { get; }
        //
        private ICommand _doubleClickCommand = null;

        /// <summary>
        /// Gets or sets whether multiple rows can be selected
        /// </summary>
        public bool MultiSelect
        {
            get { return GridView.MultiSelect; }
            set
            {
                GridView.MultiSelect = value;
            }
        }

        #endregion


        #region Constructors & Destructor
        public ECSGridUserControl()
        {
            this.Load += ECSGridUserControl_Load;

        }

        private void ECSGridUserControl_Load(object sender, EventArgs ev)
        {
            this.Load -= ECSGridUserControl_Load;
        }

        protected virtual void InitializeGridUserControl()
        {
            if (GridView != null)
            {
                GridView.DoubleClick += GridView_DoubleClick;
            }
        }
        #endregion


        #region Methods - ICommandRegistrator

        public virtual void Register(string commandKey, ICommand command, string caption, bool onBar, bool onMenu, bool onDoubleClick = false, BarShortcut shortcut = null)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(commandKey)) { throw new ArgumentNullException(nameof(commandKey)); }
            if (command == null) { throw new ArgumentNullException(nameof(command)); }

            // Ajout à la bar de commande
            if (onBar)
            {
                base.Register(commandKey, command, caption, shortcut);
            }

            // Ajout au menu contextuel
            if (onMenu)
            {
                GridView.Register(commandKey, command, caption);
            }

            // Définition de la commande DoubleClick
            if (onDoubleClick)
            {
                _doubleClickCommand = command;
            }
        }

        #endregion


        #region Methods

        #endregion


        #region Events

        private void GridView_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)
            {
                if (_doubleClickCommand != null && _doubleClickCommand.CanExecute(GridView.GetFocusedRow()))
                {
                    _doubleClickCommand.Execute();
                }
            }
        }


        #endregion
    }
}