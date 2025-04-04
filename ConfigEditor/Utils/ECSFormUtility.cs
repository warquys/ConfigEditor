using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking2010.Views.Tabbed;
using ConfigEditor.Controls;
using ConfigEditor.Factory;
using ConfigEditor.Forms;
using ConfigEditor.Interfaces;
using ConfigEditor.Metadatas;

namespace ConfigEditor.Utils
{
    public static class ECSFormUtility
    {
        #region Properties

        public static Form MainMdiParent { get; set; }
        public static ECSMetadataFactory MetadataFactory { get { return ECSMetadataFactorySingleton.Instance; } }

        public static string ProductFullName => $"{Application.ProductName} {Application.ProductVersion}";

        #endregion Properties


        #region Methods

        public static void DockForm(Form form, Orientation orientation)
        {
            if (form != null)
            {
                Document document = (ECSFormUtility.MainMdiParent as MasterForm).DocumentManager.GetDocument(form) as Document;
                var view = (ECSFormUtility.MainMdiParent as MasterForm).DocumentManager.View as TabbedView;
                var group = new DocumentGroup();
                view.DocumentGroups.Add(group);
                group.DockTo(view.DocumentGroups[0], orientation);
                view.Controller.Dock(document, group);
            }
        }

        #endregion Methods

        #region Methods - No Check on Unicity 
        public static ECSChildForm CreateForm(Control control)
        {
            return CreateForm(Application.ProductName, control);
        }

        public static ECSChildForm CreateForm(Control control, bool dialog)
        {
            return CreateForm(Application.ProductName, control, dialog);
        }

        public static ECSChildForm CreateForm(string title, Control control)
        {
            return new ECSChildForm(title, control);
        }

        public static ECSChildForm CreateForm(string title, Control control, bool dialog)
        {
            return new ECSChildForm(title, control, dialog);
        }

        public static ECSChildForm CreateForm(string title, Control control, Form parent)
        {
            return new ECSChildForm(title, control, parent);
        }

        #endregion


        #region Methods - ECSMetadata

        private static Form GetOrCreateForm<T>(string title, Control control)
        {
            // Safe design
            if (control == null) { throw new ArgumentNullException(nameof(control)); }

            // Regarder si une form edit deja ce control
            Form frm = FindForm(control);

            if (frm == null)
            {
                ECSEditUserControl ctrlEdit = control as ECSEditUserControl;
                frm = CreateForm<T>(title, control);
                (frm as ECSChildForm).Display();
                // -------------------------------------
            }
            else if (!ModalContext)
            {
                ActivateForm(frm);
            }
            else if (!(control is ECSEditUserControl))
            {
                frm = CreateForm<T>(title, control);
                (frm as ECSChildForm).Display();
            }
            else
            {
                UnicityError();
            }
            return frm;
        }

        public static void UnicityError()
        {
            MessageBox.Show("That document is already open!");
        }

        private static ECSChildForm CreateForm<T>(string title, Control control)
        {
            // Safe design
            if (control == null) { throw new ArgumentNullException(nameof(control)); }

            ECSMetadata metadata = MetadataFactory.Create<T>();

            string newTitle = (metadata.HaveText && String.IsNullOrEmpty(title) ? (metadata.AddControlText ? metadata.Text + control.Text : metadata.Text) : title);

            Icon icon = null;
            if (metadata.HaveIcon)
            {
                icon = ECSImageUtility.GetIcon(metadata.IconKey);
            }

            var form = new ECSChildForm(newTitle, control, icon);

            var ecsControl = control as ECSUserControl;
            if (ecsControl != null)
            {
                form.AcceptButton = ecsControl.AcceptButton;
                form.CancelButton = ecsControl.CancelButton;
            }

            return form;
        }

        public static void OpenListForm<T>()
        {
            Control control = ECSListFactorySingleton.Instance.Create<T>();

            ECSFormUtility.OpenForm<T>("", control);
        }

        public static Form OpenForm<T>(string title, Control control)
        {
            // Safe design
            if (control == null) { throw new ArgumentNullException(nameof(control)); }

            return GetOrCreateForm<T>(title, control);
        }

        public static bool ModalContext
        {
            get
            {
                return (Form.ActiveForm != null && !(Form.ActiveForm is MasterForm) && !Form.ActiveForm.IsMdiChild);
            }
        }

        public static Form FindForm<T>(T element)
        {
            // Safe design
            if (element == null) { throw new ArgumentNullException(nameof(element)); }

            return FindForm<T>(element.GetHashCode());
        }

        public static Form FindForm<T>(int hashCode)
        {
            // on recherche une fenetre en edition sur l element
            // Grace a L'EditHashCode
            // Utiliser par les commande Edit et New
            foreach (Form form in Application.OpenForms)
            {
                if (form is ECSChildForm)
                {
                    if ((form as ECSChildForm).EditHashCode == hashCode)
                    {
                        return form;
                    }
                }
            }

            return null;
        }

        public static Form FindForm(Control element)
        {
            // Safe design
            if (element == null) { throw new ArgumentNullException(nameof(element)); }

            int hash = element.GetHashCode();
            if (element is IMultipleDisplay)
                return null;
            else
                return FindForm(hash);
        }
        public static Form FindForm(int hash)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is ECSChildForm)
                {
                    if ((form as ECSChildForm).GetHashCode() == hash)
                    {
                        return form;
                    }
                }
            }

            return null;
        }

        public static void ActivateForm(Form form)
        {
            MasterForm frmMain = Application.OpenForms.Cast<Form>().FirstOrDefault(frm => frm is MasterForm) as MasterForm;

            if (frmMain != null)
            {
                DevExpress.XtraBars.Docking2010.Views.BaseDocument document = frmMain.DocumentManager.View.Documents.FirstOrDefault(x => x.Control == form);
                if (document != null)
                {
                    (frmMain.DocumentManager.View as DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView).Controller.Select(document as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document);
                }
            }
        }
        #endregion
    }
}