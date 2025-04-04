using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ConfigEditor.Editor
{
    public class ContentLine
    {
        public ContentLine(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
        public ContentLine(string name)
        {
            this.Name = name;
        }
        public ContentLine()
        {
        }

        public string Name { get; set; }
        public string Value { get; set; }

    }
    public class ConfigSection
    {

        public ConfigSection() { }

        public ConfigSection(string section, string content)
        {
            Section = section;
            Content = content;
            ProcessContent();
        }

        public string Section { get; set; }
        public string Content { get; set; }

        public void ProcessContent()
        {
            _ContentList = new List<ContentLine>();
            var toParse = Content.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None).ToList();
            foreach (var line in toParse)
            {
                if (line.Contains(":"))
                {
                    var splt = line.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    _ContentList.Add(new ContentLine(splt[0], splt.Length > 1 ? splt[1] : ""));
                }
                else
                {
                    _ContentList.Add(new ContentLine(line));
                }
            }
        }

        private List<ContentLine> _ContentList;
        public List<ContentLine> ContentList
        {
            get
            {
                return _ContentList;
            }
        }
        public string Serialize()
        {
            return "[" + Section + "]" + "\n" + /*"{\n" + */Content
                .Replace("[", "::lcb::")
                .Replace("]", "::rcb::")
                .Replace("{", "::lsb::")
                .Replace("}", "::rsb::")
                .Trim() /*+ "\n}"*/+"\n";
        }
    }

}
