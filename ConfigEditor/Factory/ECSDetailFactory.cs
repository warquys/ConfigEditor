using ConfigEditor.Editor;
using ConfigEditor.Controls;
using ConfigEditor.CustomClass;
using ConfigEditor.Elements;
using ConfigEditor.Interfaces;
using ConfigEditor.Utils;

namespace ConfigEditor.Factory
{
    public static class ECSDetailFactory
    {
        #region Methods - Global
        public static ECSUserControl CreateDetailControl(IWriteManager manager)
        {
            dynamic elem = manager.CurrentObject;
            return GetDetailControl(manager, elem);
        }

        private static ECSEditUserControl GetDetailControl(IWriteManager manager, object element)
        {
            ECSMessageBox.Show($"Detail for: {element}");
            return null;
        }
        #endregion
        #region private
        private static ECSEditUserControl GetDetailControl(IWriteManager manager, CustomSymlClass element)
        {
            return new CustomSymlClassEditUC(manager);
        }

        private static ECSEditUserControl GetDetailControl(IWriteManager manager, Completor element)
        {
            return new CompletorEditUC(manager);
        }


        private static ECSEditUserControl GetDetailControl(IWriteManager manager, CompletorValue element)
        {
            return new CompletorValueEditUC(manager);
        }
#endregion
    }
}
