using ConfigEditor.Managers;
using DevExpress.Office.Utils;
using DevExpress.XtraRichEdit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DevExpress.XtraEditors.Mask.Design.MaskSettingsForm.DesignInfo.MaskManagerInfo;

namespace ConfigEditor.Editor
{
    public class SymlDetailManager : FixedListManager<SymlContentItem>
    {
        private SymlSection _section;
        public void CreateListEntry(SymlContentItem element)
        {
            var list = _section.Contents();
            int idxRacine = list.IndexOf(element);
            int idxInsertAfter = idxRacine;
            SetIndexListStart(list, ref idxRacine);
            var parent = list[idxRacine];
            List<SymlContentItem> lstStructure = parent.StructureList().Select(p => p.Copy()).ToList();
            if (!string.IsNullOrEmpty(parent.Value))
            {
                parent.Value = string.Empty;
            }
            if (!lstStructure.Any())
            {
                lstStructure = new List<SymlContentItem>();
                var elem = element.Copy();
                elem.Name = "- ";
                elem.ParentListName     = parent.Name;
                elem.IsListItem         = true;
                elem.IsLastListItem     = true;
                elem.IsFirstListItem    = true;
                elem.IsLastListItem     = true;
                lstStructure.Add(elem);
            }
            while (!list[idxInsertAfter].IsLastListItem && !list[idxInsertAfter].IsList && list[idxInsertAfter].IsListItem)
            {
                idxInsertAfter++;
            }
            var toAdd = new List<SymlContentItem>();
            foreach(var item in lstStructure)
            {
                item.Value = "";
                var completor = item.GetCompletor;
                if (completor != null)
                {
                    if (completor.ListValues.Any())
                    {
                        item.Value = completor.ListValues.First().Value;
                    }
                }
                toAdd.Add(item);
            }
            list.InsertRange(idxInsertAfter + 1, toAdd);
            LoadList(list);
        }

        internal void DeleteListEntry(SymlContentItem element)
        {
            // a revoir 
            var list = _section.Contents();
            var listToDelete = new List<SymlContentItem>();
            int idxRacine = list.IndexOf(element);
            SetIndexLastItem(list, ref idxRacine);
            listToDelete.Add(list[idxRacine]);
            bool lastItem = CheckDifList(list, idxRacine + 1, element);
            int beforeDelet = idxRacine;
            while (!list[idxRacine].IsFirstListItem)
            {
                idxRacine--;
                listToDelete.Add(list[idxRacine]);
            }
            bool firstItem = CheckDifList(list, idxRacine - 1, element);
            list.RemoveAll(p => listToDelete.Contains(p));
            if (firstItem && lastItem)
            {
                list[--idxRacine].Value = " [ ]";
            }
            LoadList(list);
        }

        private void SetIndexLastItem(List<SymlContentItem> list, ref int index)
        {
            while (!list[index].IsLastListItem)
            {
                index++;
            }
        }

        private void SetIndexListStart(List<SymlContentItem> list, ref int index)
        {
            while (!list[index].IsList)
            {
                index--;
            }
        }

        private bool CheckDifList(List<SymlContentItem> list, int itemIndex, SymlContentItem curentItem)
        {
            if (TryGetItem(list, itemIndex, out var otherItem))
            {
                return otherItem.ParentListName != curentItem.ParentListName;
            }
            return true;
        }

        private bool TryGetItem(List<SymlContentItem> list, int itemIndex, out SymlContentItem item)
        {
            if (itemIndex < list.Count)
            {
                item = list[itemIndex];
                return true;
            }
            item = null;
            return false;
        }

        internal void LoadContent(SymlSection item)
        {
            _section = item;
            LoadList(item.Contents());
        }
    }
}
