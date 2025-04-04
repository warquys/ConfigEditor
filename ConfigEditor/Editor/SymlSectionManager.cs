using ConfigEditor.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigEditor.Editor
{
    public class SymlSectionManager : FixedListManager<SymlSection>
    {
        public SYML Syml { get; private set; }

        public void LoadSyml(string v)
        {
            Syml = new SYML(v);
            Syml.Load();
            LoadList(Syml.Sections.Keys.Select(p => new SymlSection(p, Syml.Sections[p].Content)).ToList());
        }

        public void Save()
        {
            foreach (var element in ElementList)
            {
                Syml.Sections[element.Name].Content = element.GetContentText();
            }
            Syml.Store();
        }


        /// <summary>
        /// Replacing any match of "/NALE/" inside <paramref name="section"/> by <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="section"></param>
        public void CreateConfigSection(string name, string section)
        {
            if (Syml != null)
            {
                if (!ElementList.Any(p => p.Name == name))
                {
                    var text = SYML.WriteSections(Syml.Sections);
                    string sectionWithName = section.Replace("/NAME/", name);
                    text += sectionWithName;
                    Syml.Sections = SYML.ParseString(text);
                    LoadList(Syml.Sections.Keys.ToList().Select(p => new SymlSection(p, Syml.Sections[p].Content)).ToList());
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("That group name is already here!");
                }
            }
        }
    }
}
