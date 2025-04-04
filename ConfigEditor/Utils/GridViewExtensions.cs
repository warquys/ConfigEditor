using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConfigEditor.Utils
{
    public static class GridViewExtensions
    {
        public enum DrawType
        {
            Cell,
            Footer,
            Group
        }
        #region Attributes & Properties
        #endregion


        #region Constructors & Destructor
        #endregion


        #region Methods
        private static void UpdateEditMode(this GridView gridView)
        {
            // Safe design
            if (gridView == null) { throw new ArgumentNullException(nameof(gridView)); }

            gridView.BeginUpdate();
            //

            gridView.OptionsBehavior.ReadOnly = false;
            gridView.OptionsBehavior.Editable = true;

            foreach (GridColumn columnItem in gridView.Columns)
            {
                if (columnItem.ColumnEdit == null)
                {
                    columnItem.OptionsColumn.AllowEdit = false;
                }
            }

            //
            gridView.EndUpdate();
        }

        public static void SetFontColorFor<T>(this GridView gridView, string columnName, Func<T, Color?> fctColor)
            where T : class
        {
            gridView.CustomDrawCell += (sender, e) =>
            {
                if (e.Column.FieldName == columnName)
                {
                    var view = sender as GridView;
                    var cellViewInfo = e.Cell as GridCellInfo;
                    var data = view.GetRow(cellViewInfo.RowHandle) as T;

                    Color? color = fctColor.Invoke(data);
                    if (color != null)
                    {
                        e.Appearance.ForeColor = (Color)color;
                    }

                }
            };
        }

        public static void SetGroupeFontColorFor<T>(this GridView gridView, string columnName, Func<object, Color?> fctColor)
        {
            gridView.CustomDrawRowFooterCell += (sender, e) =>
            {
                if (e.Column.FieldName == columnName)
                {
                    Color? color = fctColor.Invoke((T)e.Info.Value);
                    if (color != null)
                    {
                        e.Appearance.ForeColor = (Color)color;
                    }

                }
            };
        }

        public static void SetFooterFontColorFor<T>(this GridView gridView, string columnName, Func<object, Color?> fctColor)
        {
            gridView.CustomDrawFooterCell += (sender, e) =>
            {
                if (e.Column.FieldName == columnName)
                {
                    Color? color = fctColor.Invoke((T)e.Info.Value);
                    if (color != null)
                    {
                        e.Appearance.ForeColor = (Color)color;
                    }

                }
            };
        }

        public static void SetImageFor<T>(this GridView gridView, string columnName, Func<T, string> fctImage)
            where T : class
        {
            gridView.CustomDrawCell += (sender, e) =>
            {
                if (e.Column.FieldName == columnName)
                {
                    var view = sender as GridView;
                    var cellViewInfo = e.Cell as GridCellInfo;
                    var info = cellViewInfo?.ViewInfo as TextEditViewInfo;
                    if (cellViewInfo != null)
                    {
                        var data = view?.GetRow(cellViewInfo.RowHandle) as T;
                        string imageName = data == null ? "" : fctImage.Invoke(data);
                        if (!String.IsNullOrWhiteSpace(imageName))
                        {
                            // TODO: If image is bigger than 16x16, it doesn't display in the UI! WHY?
                            if (info != null)
                            {
                                info.ContextImage = ECSImageUtility.GetImage(imageName, 16);
                                info.CalcViewInfo();
                                e.DefaultDraw();
                            }
                        }
                    }
                }
            };
        }

        public static void SetIndent<T>(this GridView gridView, Func<T, int> fctIndent)
            where T : class
        {
            gridView.CustomDrawCell += (sender, e) =>
            {
                if (e.Column.VisibleIndex == 0)
                {
                    GridView view = sender as GridView;
                    GridCellInfo cellViewInfo = e.Cell as GridCellInfo;

                    var data = view.GetRow(cellViewInfo.RowHandle) as T;

                    int dataIndent = data == null ? 0 : fctIndent.Invoke(data);
                    int indent = cellViewInfo.Bounds.X + dataIndent * 8;

                    cellViewInfo.CellValueRect.X = indent;
                    cellViewInfo.CellValueRect.Width = cellViewInfo.Bounds.Width - indent;
                }
            };
        }
        #endregion
    }
}
