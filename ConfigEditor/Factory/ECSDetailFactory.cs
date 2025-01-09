using ConfigtEditor.ConfigEditor;
using ConfigtEditor.Controls;
using ConfigtEditor.CustomClass;
using ConfigtEditor.Elements;
using ConfigtEditor.Interfaces;
using ConfigtEditor.Utils;

namespace ConfigtEditor.Factory
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
        private static ECSEditUserControl GetDetailControl(IWriteManager manager, CustomSynapseClass element)
        {
            return new CustomSynapseClassEditUC(manager);
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
