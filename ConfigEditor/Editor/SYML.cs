using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigEditor.Editor
{
    public class SYML
    {
        private readonly string _path;

        public Dictionary<string, ConfigSection> Sections = new Dictionary<string, ConfigSection>();

        public SYML(string path)
        {
            this._path = path;
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
        }

        public void Load()
        {
            var text = File.ReadAllText(_path);
            Sections = ParseString(text);
        }

        public void Store()
        {
            var text = WriteSections(Sections);
            File.WriteAllText(_path, text);
        }

        public static Dictionary<string, ConfigSection> ParseString(string source)
        {
            var sections = new Dictionary<string, ConfigSection>();
            var str = source.Replace("[]", "::lcb::::rcb::");
            var split = str.Split(new string[] { "[", "]" }, StringSplitOptions.None);
            for (var i = 1; i < split.Length; i += 2)
            {
                var identifier = split[i];
                var content = split[i + 1];
                /*
                int lastBracket = content.Length - 1;
                int firstBracket = 0;
                for (var i1 = 0; i1 < content.Length; i1++)
                {
                    if (content[i1] == '{')
                    {
                        firstBracket = i1;
                        break;
                    }
                }
                for (var i1 = 0; i1 < content.Length; i1++)
                {
                    if (content[i1] == '}')
                    {
                        lastBracket = i1;
                    }
                }
                content = content.Substring(firstBracket + 1, lastBracket - firstBracket - 1);
                */
                content = content
                    .Replace("::lcb::", "[")
                    .Replace("::rcb::", "]")
                    .Replace("::lsb::", "{")
                    .Replace("::rsb::", "}")
                    ;

                sections.Add(identifier, new ConfigSection(identifier, content));
            }

            return sections;
        }

        public static string WriteSections(Dictionary<string, ConfigSection> sections)
        {
            string s = "";
            foreach (var value in sections.Values)
            {
                s += value.Serialize();
                s += "\n";
            }

            return s;
        }
    }

}
