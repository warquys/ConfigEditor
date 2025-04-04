using ConfigEditor.Editor;
using ConfigEditor.CustomClass;
using ConfigEditor.Elements;
using ConfigEditor.Utils;
using System;

namespace ConfigEditor.Metadatas
{
    public sealed class ECSMetadataFactorySingleton
    {
        #region Attributes & Properties
        public static ECSMetadataFactory Instance { get { return Singleton<ECSMetadataFactory>.Instance; } }
        #endregion
    }

    public class ECSMetadataFactory : BaseFactory<ECSMetadata>
    {
        #region Attributes & Properties
        #endregion


        #region Constructors & Destructor
        public ECSMetadataFactory()
        {
            RegisterAll();
        }
        #endregion


        #region Methods - BaseFactory

        public override ECSMetadata Create(Type type)
        {
            // Safe design
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            if (FactoryMap.ContainsKey(type))
            {
                Func<ECSMetadata> func = FactoryMap[type];
                ECSMetadata result = func.Invoke();

                return result;
            }
            else
            {
                return new ECSMetadata(type);
            }
        }

        private void Register<T>()
        {
            Type type = typeof(T);

            Register(type, () => new ECSMetadata(type, ExtractText(type)));
        }

        private void Register<T>(string iconName)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(iconName)) { throw new ArgumentNullException(nameof(iconName)); }

            Type type = typeof(T);

            Register(type, () => new ECSMetadata(type, ExtractText(type), iconName));
        }

        private void Register<T>(string text, string iconName, bool addControlText = false)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(text)) { throw new ArgumentNullException(nameof(text)); }
            //if (String.IsNullOrWhiteSpace(iconName)) { throw new ArgumentNullException(nameof(iconName)); }

            Register<T>(() => new ECSMetadata(typeof(T), text, iconName, addControlText));
        }
#endregion


        #region Methods
        private string ExtractText(Type type)
        {
            // Safe design
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            return type.ToString();
        }


        private void RegisterAll()
        {
            Register<CustomSymlClass>("Custom class", String.Empty); //For an next time 
            Register<SymlConfigEditor>("Config Editor", "ICN_PRODUCT_HIERARCHY");
            Register<SymlEditorConfig>("Config Editor Parameter", "ICN_CUSTOM_CLASS");
            Extension.ExtensionHandler.CallEcsMetaRegisterHook(this);
        }

        #endregion
    }
}