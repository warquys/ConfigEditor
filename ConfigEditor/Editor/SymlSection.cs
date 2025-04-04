using ConfigEditor.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConfigEditor.Editor
{
    public class SymlSection
    {
        private const string CommentIndicator = "#";
        private const string ListItemIndicator = "- ";
        private const string ValueSepartor = ": ";
        private const string ListIndicator = ":";

        [ECSDisplayColumn("Name", 1, 20)]
        public string Name { get; set; }

        private List<SymlContentItem> ContentList { get; set; }

        public SymlSection(string name, string content)
        {
            Name = name;
            LoadConfig(content);
        }

        public List<SymlContentItem> Contents()
        {
            return ContentList;
        }

        public string GetContentText()
        {
            string result = "";
            foreach (var item in ContentList)
            {
                string toAdd = item.Value;
                if (item.IsMultiLine)
                {
                    toAdd = item.RestoreMultiLine();
                }
                if (!String.IsNullOrWhiteSpace(result))
                {
                    result += "\n";
                }
                result += $"{item.Name}{toAdd}";
            }
            return result;
        }
        public void LoadConfig(string content)
        {
            ContentList = new List<SymlContentItem>();
            var toParse = content.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None).ToList();
            SymlContentItem item = null;
            foreach (var line in toParse)
            {
                if (line.StartsWith(CommentIndicator))
                {
                    item = new SymlContentItem(line);
                    item.IsComment = true;
                    AddItemIfNeeded(item);
                }
                else if (line.Contains(ValueSepartor))
                {
                    var splt = line.Split(new string[] { ValueSepartor }, StringSplitOptions.None);
                    item = new SymlContentItem(splt[0] + ValueSepartor, Join(splt, ValueSepartor));
                    if (IsEmptyList(splt[1]))
                        item.IsList = true;
                    AddItemIfNeeded(item);
                }
                else
                {
                    if (line.Contains(ListItemIndicator))
                    {
                        var splt = line.Split(new string[] { ListItemIndicator }, StringSplitOptions.None);
                        item = new SymlContentItem(splt[0] + ListItemIndicator, Join(splt, ListItemIndicator));
                        AddItemIfNeeded(item);
                    }
                    else
                    {
                        if (item?.IsMultiLine == true)//!String.IsNullOrEmpty(item?.Value))
                        {
                            if (!String.IsNullOrWhiteSpace(line))
                            {
                                item.AddMultiLineValue(line);
                            }
                            AddItemIfNeeded(item, false);
                        }
                        else
                        {
                            item = new SymlContentItem(line);
                            if (line.EndsWith(ListIndicator))
                            {
                                item.IsList = true;
                            }
                            AddItemIfNeeded(item);
                        }
                    }
                }
            }

            // Add the structure item to the List entry
            /*
            for (int i = 0; i < ContentList.Count; i++)
            {
                SymlContentItem elem = ContentList[i];
                if (elem.Name.StartsWith("#"))
                {
                    elem.IsComment = true;
                    for (int j = i + 1; j < ContentList.Count && elem.IsComment; j++)
                    {
                        elem = ContentList[j];
                        elem.IsComment = !elem.Name.Contains(":");
                    }
                }
            }*/
            foreach (var elem in ContentList)
            {
                if (elem.IsMultiLine)
                {
                    elem.ChangeMultiLine();
                }
            }
            // Add the Comment item 
            for (int i = 0; i < ContentList.Count; i++)
            {
                SymlContentItem elem = ContentList[i];
                if (!elem.IsComment)
                {
                    elem.ParentComment = GetComment(i - 1);
                }
            }

            var list = ContentList.Where(p => !p.IsListItem && ContentList.Any(p2 => p2.Id == p.Id + 1 && p2.IsListItem));
            foreach (SymlContentItem elem in list)
            {
                int idx = ContentList.IndexOf(elem);

                // Mark as a list
                elem.IsList = true;
                idx++;
                var max = ContentList.Count;
                while (idx < max && elem.Indent < ContentList[idx].Indent)
                {
                    var subItem = ContentList[idx];
                    subItem.ParentListName = elem.Name;
                    subItem.ParentComment = elem.ParentComment;
                    idx++;
                }
                idx = ContentList.IndexOf(elem);
                // Copy structure in the list entry for Add
                SymlContentItem toAdd;
                do
                {
                    idx++;
                    toAdd = ContentList[idx];
                    elem.StructureList().Add(toAdd.Copy());
                } while (!toAdd.IsLastListItem);
            }


            // Add Parent list name

        }

        private bool IsEmptyList(string value)
        {
            if (Regex.IsMatch(value, pattern: @"( *\[( *)\])$"))
                return true;
            return false;
        }


        private void AddItemIfNeeded(SymlContentItem item, bool needed = true)
        {
            if (ContentList.Any())
            {
                // Check start and end list item
                var previous = ContentList.Last();
                if ((previous.IsListItem && item.IsListItem) || (previous.IsListItem && previous.Indent != item.Indent))
                {
                    previous.IsLastListItem = true;
                }
                else if (previous.IsListItem && !item.IsListItem && previous.IsListItem && previous.Indent == item.Indent)
                {
                    item.IsListItem = true;
                }
            }
            if (needed)
            {
                ContentList.Add(item);
            }
        }

        private string GetComment(int idx)
        {
            string result = "";
            while (idx > 0 && ContentList[idx].IsComment)
            {
                result = ContentList[idx].Name + result;
                idx--;
            }

            return result;
        }
        private string Join(string[] splt, string v)
        {
            if (splt.Length > 1)
            {
                string result = "";
                for (int i = 1; i < splt.Length; i++)
                {
                    if (!String.IsNullOrWhiteSpace(result))
                    {
                        result += v;
                    }
                    result += splt[i];
                }

                return result;
            }
            else
            {
                return "";
            }
        }

    }
}
