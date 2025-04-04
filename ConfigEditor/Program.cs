using ConfigEditor.Utils;
using ConfigEditor.Elements;
using ConfigEditor.Managers;
using DevExpress.Office.Crypto;
using DevExpress.Skins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConfigEditor
{
    static class Program
    {
        //public static Config Config;
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SymlEditorConfig.Load();
            Extension.ExtensionHandler.LoadDependencies();
            Extension.ExtensionHandler.LoadExtensions();
            SetTheme();
            ECSFormUtility.MainMdiParent = new MasterForm();
            Application.Run(ECSFormUtility.MainMdiParent);
        }

        static void SetTheme()
        {
            if (Theme.ShouldSystemUseDarkMode())
            {
                Theme.SetSkin(19);
            }
            else
            {
                Theme.SetSkin(18);
            }
        }
    }
}
