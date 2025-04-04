using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigEditor.Factory;
using ConfigEditor.Menus;
using ConfigEditor.Metadatas;
using DevExpress.XtraTab;

namespace ConfigEditor.Extension
{
    public interface IExtension
    {
        void Initialize();

        void MasterMenuHook(MasterMenuControl masterMenu, XtraTabPageCollection tabPages);
        void EcsListRegisterHook(ECSListFactory ecsListFactory);
        void EcsMetaRegisterHook(ECSMetadataFactory ecsMetadataFactory);

    }
}