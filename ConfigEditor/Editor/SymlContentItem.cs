using ConfigEditor.Attributes;
using ConfigEditor.Elements;
using DevExpress.Utils.Serializing.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigEditor.Editor
{
    public class SymlContentItem : BaseUintElement
    {
        private static uint NextId = 0;
        private static uint GetId()
        {
            NextId++;
            return NextId - 1;
        }
        
        private uint howManyIndent(string s)
        {
            string cp = s;
            uint result = 0;
            while (cp.StartsWith("  "))
            {
                result++;
                cp = cp.Substring(2);
            }

            if (cp.StartsWith("- "))
            {
                result++;
                IsListItem = true;
                IsFirstListItem = true;
            }
            return result;
        }

        private string _multiLineStart = "";
        internal string RestoreMultiLine()
        {
            var result = Value;
            result = result.Replace("\r\n", "\n\n  ");
            result = $"{_multiLineStart}  {result}";
            result = result.Replace("\r", "");
            return result;
        }
        internal void ChangeMultiLine()
        {
            int po = Value.IndexOf(">-\r\n");
            _multiLineStart = Value.Substring(0, po + 4);
            Value = Value.Substring(po + 4);
        }

        internal void AddMultiLineValue(string line)
        {
            Value = $"{Value}\r\n{line.TrimStart()}";
        }


        public SymlContentItem(string name, string value) : this(name)
        {
            this.Value = value;
            this.IsMultiLine = value.Trim().StartsWith(">-");
        }
        public SymlContentItem(string name) : this()
        {
            this.Name = name;
            Indent = howManyIndent(name);
        }
        public SymlContentItem()
        {
            this.Id = GetId();
            ParentListName = "";
        }

        private List<SymlContentItem> structureList = new List<SymlContentItem>();

        public List<SymlContentItem> StructureList()
        {
            return structureList;
        }

        #region Prop
        [ECSDisplayColumn("Name", 1, 40)]
        public string Name { get; set; }
        [ECSDisplayColumn("Value", 2, 20)]
        public string Value { get; set; }
        public uint Reference => Id;
        //[ECSDisplayColumn("Parent Name", 10, 40)]
        public string ParentListName { get; set; }
        //[ECSDisplayColumn("Parent Comment", 10, 40)]
        public string ParentComment { get; set; }
        public uint Indent { get; set; }
        //[ECSDisplayColumn("Is List", 10, 40)]
        public bool IsList { get; set; }
        public bool IsListItem { get; set; }
        public bool IsFirstListItem { get; set; }
        public bool IsLastListItem { get; set; }
        public string Action { get; set; }


        //[ECSDisplayColumn("IsComment", 10, 40)]
        public bool IsComment { get; set; }
        public Completor GetCompletor
        {
            get
            {
                return SymlEditorConfig.Singleton.GetCompletor(this);
            }
        }

        [ECSDisplayColumn("Space Error", 3, 8)]
        public bool SpaceError
        {
            get
            {
                return !String.IsNullOrEmpty(Name) && (Name.TakeWhile(c => c == ' ').Count() % 2 != 0);
            }
        }

        public bool IsMultiLine { get; internal set; }
        #endregion

        public SymlContentItem Copy()
        {
            var elem = new SymlContentItem();
            elem.ParentListName = ParentListName;
            elem.Name = Name;
            elem.ParentComment = ParentComment;
            elem.Indent = Indent;
            elem.IsListItem = IsListItem;
            elem.IsFirstListItem = IsFirstListItem;
            elem.IsLastListItem = IsLastListItem;
            return elem;
        }
    }
}
