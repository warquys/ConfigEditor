using System.Drawing;
using DevExpress.XtraEditors;
using ConfigtEditor.ConfigEditor;
using ConfigtEditor.CustomClass;
using ConfigtEditor.Elements;

namespace ConfigtEditor.Menus
{
    public partial class MasterMenuControl : XtraUserControl
    {
        #region Attributes & Properties
        #endregion


        #region Constructors & Destructor
        public MasterMenuControl()
        {
            InitializeComponent();

            _logoLayout.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

/*
            string dir = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

            if (System.IO.File.Exists($"{dir}\\Logo.png"))
            {

                Bitmap popupLogo = new Bitmap($"{dir}\\Logo.png");

                //Récupération de la couleur transparente sur le premier pixel
                popupLogo.MakeTransparent(popupLogo.GetPixel(1, 1));
                _logo.Image = popupLogo;// Image.FromFile($"{dir}\\Logo.png");
            }
            else
            {
                _logoLayout.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }*/
            CreateMenuTabs();
        }
        #endregion


        #region Methods
        private void CreateMenuTabs()
        {

            var menuSynapse = new ECSMenuControl("Synapse");
            var grpConfig = menuSynapse.AddGroup(new ECSMenuGroup("Common"));
            grpConfig.AddItem<SynapseConfigEditor>();
            grpConfig.AddItem<Config>();
            grpConfig.AddItem<SynapsePermissionEditor>(); 
            //grpClass.AddItem<CustomSynapseClass>();

            menuSynapse.Visible = true;
            this._tabControl.TabPages.Add(new ECSMenuTabPage(menuSynapse));

            //var menuExiled = new ECSMenuControl("Exiled");
            //menuExiled.Visible = true;
            //this._tabControl.TabPages.Add(new ECSMenuTabPage(menuExiled));

        }
        #endregion
    }
}
