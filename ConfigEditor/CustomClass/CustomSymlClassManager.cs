using ConfigEditor.Elements;
using ConfigEditor.Interfaces;
using ConfigEditor.Managers;
using ConfigEditor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigEditor.CustomClass
{
    public class CustomSymlClassManager : FixedListManager<CustomSymlClass>, IWriteManager
    {

        #region Attributes & Properties
        public object CurrentObject { get; set; }
        public CustomSymlClass Current => CurrentObject as CustomSymlClass;

        private CustomSymlListClass liste;
        #endregion

        #region Constructors & Destructor
        public CustomSymlClassManager() : base()
        {
            // lire du fichier Xml
            LoadXmlFile();
        }
        #endregion

        #region Methods
        public void LoadXmlFile()
        {
            liste = new CustomSymlListClass();
            _results = liste.Elements;
            this.LoadList();

        }

        internal bool CheckIdInvalid()
        {
            return Elements.Any(p => p.IdClass == Current.IdClass && p.Id != Current.Id);
        }

        public void PrepareNew()
        {
            CurrentObject = new CustomSymlClass()
            {
                IdClass = 33
            };
        }
        public bool Save()
        {
            if (Current.IsNew())
            {
                Elements.Add(Current);
            }
            Current.Id = (uint)Current.IdClass;
            OnElementListUpdated();
            return true;
        }
        internal void CreateAllClass()
        {
            System.Windows.Forms.MessageBox.Show("Cree all class");
        }
        #endregion

    }
}
