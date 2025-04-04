using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConfigEditor.Editor;
using ConfigEditor.Elements;
using ConfigEditor.Extension;
using DevExpress.XtraEditors;

namespace ConfigEditor.Menus
{
    public partial class MasterMenuControl : XtraUserControl
    {
        #region Constructors & Destructor
        public MasterMenuControl()
        {
            InitializeComponent();

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
            var ConfigSmyl = new ECSMenuControl("Syml");
            var grpConfig = ConfigSmyl.AddGroup(new ECSMenuGroup("Common"));
            grpConfig.AddItem<SymlConfigEditor>();
            grpConfig.AddItem<SymlEditorConfig>();

            ConfigSmyl.Visible = true;
            this._tabControl.TabPages.Add(new ECSMenuTabPage(ConfigSmyl));
            ExtensionHandler.CallMasterMenuHook(this, this._tabControl.TabPages);
        }
        #endregion
    }
}
