using System;
using System.Linq;
using ConfigEditor.Extension;
using ConfigEditor.Factory;
using ConfigEditor.Menus;
using ConfigEditor.Metadatas;
using DevExpress.Data.Helpers;
using DevExpress.Utils.Extensions;
using DevExpress.XtraTab;

namespace PermissionEditorExtension
{
    [ExtensionAttribute("Permission Editor", "VT")]
    internal class EntryPoint : IExtension
    {
        public void Initialize()
        {

        }

        public void EcsListRegisterHook(ECSListFactory ecsListFactory)
        {
            ecsListFactory.Register<SynapsePermissionEditor>(() =>
            {
                var ctrl = new PermissionConfigEditorUC();
                return ctrl;
            });
        }

        public void EcsMetaRegisterHook(ECSMetadataFactory ecsMetadataFactory)
        {
            ecsMetadataFactory.Register<SynapsePermissionEditor>("Permission Editor", "ICN_ACTION_ELEMENT_DROIT");
        }

        public void MasterMenuHook(MasterMenuControl masterMenu, XtraTabPageCollection tabPages)
        {
            foreach (ECSMenuControl control in from XtraTabPage tab in tabPages
                                    from object control in tab.Controls
                                    where control is ECSMenuControl
                                    select control)
            {
                var commun = control.Groups.FirstOrDefault(x => x.Caption == MasterMenuControl.CommonMenuControlCaption) as ECSMenuGroup;
                if (commun == null)
                    continue;

                commun.AddItem<SynapsePermissionEditor>();
            }
        }
    }
}
