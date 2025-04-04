using System;
using System.ComponentModel;
using DevExpress.XtraNavBar;
using ConfigEditor.Metadatas;
using ConfigEditor.Utils;

namespace ConfigEditor.Menus
{
    public class ECSMenuGroup : NavBarGroup
    {
        #region Attributes & Properties
        #endregion


        #region Constructors & Destructor
        public ECSMenuGroup(string caption) : base(caption)
        {
            base.Expanded = true;
            //
            Visible = false;
            ItemLinks.CollectionChanged += ItemLinks_CollectionChanged;
        }
        #endregion


        #region Methods
        public void AddItem(NavBarItem item)
        {
            // Safe design
            if (item == null) { throw new ArgumentNullException(nameof(item)); }

            ItemLinks.Add(item);
        }


        public void AddItem<T>(Action action)
        {
            // Safe design
            if (action == null) { throw new ArgumentNullException(nameof(action)); }

            ECSMetadata metadata = ECSMetadataFactorySingleton.Instance.Create<T>();

            AddItem(new ECSMenuItem(metadata.Text, metadata.IconKey, action));
        }

        public void AddItem<T>()
        {
            AddItem<T>(() => { ECSFormUtility.OpenListForm<T>(); });
        }
        #endregion


        #region Events
        private void ItemLinks_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            Visible = (VisibleItemLinks.Count > 0);
        }
        #endregion
    }
}