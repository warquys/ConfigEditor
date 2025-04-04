using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ConfigEditor.Factory;
using ConfigEditor.Menus;
using ConfigEditor.Metadatas;
using DevExpress.PivotGrid.OLAP.AdoWrappers;
using DevExpress.XtraTab;

namespace ConfigEditor.Extension
{
    public static class ExtensionHandler
    {
        public static readonly DirectoryInfo dependenciesDirectory;
        public static readonly DirectoryInfo extensionsDirectory;

        public static readonly List<ExtensionInfo> extensions = new List<ExtensionInfo>();

        static ExtensionHandler()
        {
            dependenciesDirectory = Directory.CreateDirectory("ExtensionDependencies");
            extensionsDirectory = Directory.CreateDirectory("Extensions");
        }

        internal static void LoadDependencies()
        {
            List<Exception> exceptions = new List<Exception>();
            foreach (var dll in dependenciesDirectory.GetFiles("*.dll"))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dll.FullName);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count != 0)

                File.WriteAllText($"dependencies-{DateTime.Now:dd-M--yyyy--HH-mm-ss}.log", string.Join(Environment.NewLine, exceptions.Select(e => e.Message)));
        }

        internal static void LoadExtensions()
        {
            List<Exception> exceptions = new List<Exception>();
            foreach (var dll in extensionsDirectory.GetFiles("*.dll"))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dll.FullName);
                    LoadExtension(assembly);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count != 0)
                File.WriteAllText($"extension-{DateTime.Now:dd-M--yyyy--HH-mm-ss}.log", string.Join(Environment.NewLine, exceptions.Select(e => e.Message)));
        }

        public static void LoadExtension(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                var attribut = type.GetCustomAttribute<ExtensionAttributeAttribute>();
                if (attribut != null && typeof(IExtension).IsAssignableFrom(type))
                {
                    var extension = (IExtension)Activator.CreateInstance(type);
                    extension.Initialize();
                    extensions.Add(new ExtensionInfo()
                    {
                        attribute = attribut,
                        instance = extension,
                        mainType = type
                    });
                }
            }
        }

 #region Event
#pragma warning disable CS0168 // La variable est déclarée mais jamais utilisée

        internal static void CallMasterMenuHook(MasterMenuControl masterMenu, XtraTabPageCollection tabPages)
        {
            foreach (var extension in extensions)
            {
                try
                {
                    extension.instance.MasterMenuHook(masterMenu, tabPages);
                }
                catch (Exception ex)
                {
#if DEBUG
                    throw;
#endif
                }
            }
        }
        
        internal static void CallEcsListRegisterHook(ECSListFactory ecsListFactory)
        {
            foreach (var extension in extensions)
            {
                try
                {
                    extension.instance.EcsListRegisterHook(ecsListFactory);
                }
                catch (Exception ex)
                {
#if DEBUG
                    throw;
#endif
                }
            }
        }
        
        internal static void CallEcsMetaRegisterHook(ECSMetadataFactory ecsMetadataFactory)
        {
            foreach (var extension in extensions)
            {
                try
                {
                    extension.instance.EcsMetaRegisterHook(ecsMetadataFactory);
                }
                catch (Exception ex)
                {
#if DEBUG
                    throw;
#endif
                }
            }
        }

#pragma warning restore CS0168 // La variable est déclarée mais jamais utilisée
#endregion

        public struct ExtensionInfo
        {
            public Type mainType;

            public IExtension instance;

            public ExtensionAttributeAttribute attribute;
        }
    }
}
