using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Utils;
using ConfigtEditor.Commands;
using ConfigtEditor.ConfigEditor;
using ConfigtEditor.Controls;
using ConfigtEditor.CustomClass;
using ConfigtEditor.Elements;
using ConfigtEditor.Utils;

namespace ConfigtEditor.Factory
{
    public sealed class ECSListFactorySingleton : ECSListFactory
    {
        public static ECSListFactory Instance
        {
            get
            {
                return Singleton<ECSListFactory>.Instance;
            }
        }
    }


    public class ECSListFactory : BaseFactory<Control>
    {
        #region Constructors & Destructor

        public ECSListFactory()
        {
            RegisterAll();
        }

        #endregion


        #region Methods - BaseFactory

        public override Control Create(Type type)
        {
            // Safe design
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            if (FactoryMap.ContainsKey(type))
            {
                Func<Control> func = FactoryMap[type];

                return func.Invoke();
            }

            throw new NotImplementedException($"Cannot create : {type}");
        }

        #endregion


        #region Methods

        private void RegisterAll()
        {
            Register<CustomSynapseClass>(() =>
            {
                var manager = new CustomSynapseClassManager();
                var ctrl = new ListControl<CustomSynapseClass>(manager);

                InjectCommands(ctrl, true, true, true);
                ctrl.Register("ACN_GENERATE_ALL_CLASS", new ActionCommand(() => manager.CreateAllClass()), "Create ...");
                return ctrl;
            });
            Register<SynapseConfigEditor>(() =>
            {
                var ctrl = new SynapseConfigEditorUC();
                return ctrl;
            });
            Register<SynapsePermissionEditor>(() =>
            {
                var ctrl = new SynapseConfigEditorUC(true);
                return ctrl;
            });
            Register<Config>(() => new ConfigUC());
        }


        private void InjectCommands<T>(ListControl<T> control, bool addNewCmd = true, bool addEditCmd = true, bool addDeleteCmd = false)
           where T : class, new()
        {
            if (addNewCmd)
            {
                control.Register("ACN_ELEMENT_NEW", new ElementNewCommand<T>(control.Manager), "New", true, true);
            }

            if (addEditCmd)
            {
                control.Register("ACN_ELEMENT_EDIT", new ElementEditCommand<T>(control.Manager), "Edit", true, true, true);
            }

            if (addDeleteCmd)
            {
                control.Register("ACN_ELEMENT_DELETE", new ElementDeleteCommand<T>(control.Manager), "Delete", true, true);
            }
        }

#endregion
    }
}