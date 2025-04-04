using ConfigEditor.Elements;
using ConfigEditor.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ConfigEditor.Elements
{
    [Serializable]
    [XmlRoot("CONFIG")]
    public class SymlEditorConfig
    {
        [XmlIgnore]
        public static SymlEditorConfig Singleton = new SymlEditorConfig();

        [XmlArray("Completors")]
        [XmlArrayItem("Completor")]
        public List<Completor> Completors
        {
            get; set;
        }

        public SymlEditorConfig()
        {
            Completors = new List<Completor>();
        }

        public uint GetCompletorId() 
        {
            uint max = 0;
            if (Completors.Any())
            {
                max = Completors.Max(p => p.Id);
            }
            return max + 1 ;
        }

        public uint GetCompletorValueId()
        {
            uint nextId = 0;
            foreach (var completor in SymlEditorConfig.Singleton.Completors)
            {
                if (completor.ListValues.Any())
                {
                    var max = completor.ListValues.Max(p => p.Id);
                    if (max > nextId)
                    {
                        nextId = max;
                    }
                }
            }
            return nextId + 1;
        }

        #region Default Value
        public void Default()
        {
            var completorBool = new Completor();
            completorBool.Id = GetCompletorId();
            completorBool.Name = "Bool";
            Singleton.Completors.Add(completorBool);
            completorBool.ListValues.Add(new CompletorValue("true"));
            completorBool.ListValues.Add(new CompletorValue("false"));
            completorBool.CompletorType = CompletorType.ByValue;


            //Item
            var completorItem = new Completor();
            completorItem.Id = GetCompletorId();
            completorItem.Name = "Items";
            completorItem.ContainWord = "Item&Id";
            completorItem.CompletorType = CompletorType.ByName;
            Singleton.Completors.Add(completorItem);
            AddItemsToCompletor(completorItem);
            Singleton.Save();

            //Scp
            var completorScp = new Completor();
            completorScp.Id = GetCompletorId();
            completorScp.Name = "Scps";
            completorScp.ContainWord = "Scp&Id";
            completorScp.CaseSensitive = false;
            completorScp.CompletorType = CompletorType.ByName;
            Singleton.Completors.Add(completorScp);
            AddScpToCompletor(completorScp);
            Singleton.Save();

            //Role
            var completorRole = new Completor();
            completorRole.Id = GetCompletorId();
            completorRole.Name = "Roles";
            completorRole.ContainWord = "Role&Id|All&roles";
            completorRole.CaseSensitive = false;
            completorRole.CompletorType = CompletorType.ByName;
            Singleton.Completors.Add(completorRole);
            AddRolesToCompletor(completorRole);
            Singleton.Save();

            //Team
            var completorTeam = new Completor();
            completorTeam.Id = GetCompletorId();
            completorTeam.Name = "Teams";
            completorTeam.ContainWord = "Team";
            completorTeam.CompletorType = CompletorType.ByName;
            Singleton.Completors.Add(completorTeam);
            AddTeamsToCompletor(completorTeam);
            Singleton.Save();

            //Language
            var completorLanguage = new Completor();
            completorLanguage.Id = GetCompletorId();
            completorLanguage.Name = "Language";
            completorLanguage.ContainWord = "Language";
            completorLanguage.CompletorType = CompletorType.ByName;
            Singleton.Completors.Add(completorLanguage);
            AddLanguageToCompletor(completorLanguage);
            Singleton.Save();

            //TODO key
        }

        private void AddLanguageToCompletor(Completor completor)
        {
            completor.ListValues.Add(new CompletorValue("ENGLISH"));
            completor.ListValues.Add(new CompletorValue("GERMAN"));
            completor.ListValues.Add(new CompletorValue("FRENCH"));
        }

        private void AddScpToCompletor(Completor completor)
        {
            completor.ListValues.Add(GetCompletorValue("Scp-173", "0"));
            completor.ListValues.Add(GetCompletorValue("Scp-106", "3"));
            completor.ListValues.Add(GetCompletorValue("Scp-049", "5"));
            completor.ListValues.Add(GetCompletorValue("Scp-096", "9"));
            completor.ListValues.Add(GetCompletorValue("Scp-049-2", "10"));
            completor.ListValues.Add(GetCompletorValue("SCP-939-53", "16"));
            completor.ListValues.Add(GetCompletorValue("SCP-939-89", "17"));
            completor.ListValues.Add(GetCompletorValue("SCP-035", "35"));
            completor.ListValues.Add(GetCompletorValue("SCP-056", "56"));
            completor.ListValues.Add(GetCompletorValue("Scp-079-Robot", "79"));
            /*
            completor.ListValues.Add(GetCompletorValue("SCP-008", "122"));
            completor.ListValues.Add(GetCompletorValue("SCP-966", "123"));
            completor.ListValues.Add(GetCompletorValue("SCP-999", "124"));
            completor.ListValues.Add(GetCompletorValue("SCP-682", "125"));
            completor.ListValues.Add(GetCompletorValue("SCP-1048", "126"));
            completor.ListValues.Add(GetCompletorValue("SCP-1048A", "127"));
            completor.ListValues.Add(GetCompletorValue("SCP-1048B", "128"));
            completor.ListValues.Add(GetCompletorValue("SCP-1048C", "129"));
            completor.ListValues.Add(GetCompletorValue("SCP-1048D", "130"));
            completor.ListValues.Add(GetCompletorValue("SCP-953", "131"));
            *///Custom Class from VT
            completor.ListValues.Add(GetCompletorValue("SCP-682", "682"));
        }

        private void AddRolesToCompletor(Completor completor)
        {
            completor.ListValues.Add(GetCompletorValue("None", "-1"));
            completor.ListValues.Add(GetCompletorValue("Scp-173", "0"));
            completor.ListValues.Add(GetCompletorValue("ClassD", "1"));
            completor.ListValues.Add(GetCompletorValue("Spectator", "2"));
            completor.ListValues.Add(GetCompletorValue("Scp-106", "3"));
            completor.ListValues.Add(GetCompletorValue("NtfSpecialist", "4"));
            completor.ListValues.Add(GetCompletorValue("Scp-049", "5"));
            completor.ListValues.Add(GetCompletorValue("Scientist", "6"));
            completor.ListValues.Add(GetCompletorValue("Scp-079", "7"));
            completor.ListValues.Add(GetCompletorValue("ChaosConscript", "8"));
            completor.ListValues.Add(GetCompletorValue("Scp-096", "9"));
            completor.ListValues.Add(GetCompletorValue("Scp-049-2", "10"));
            completor.ListValues.Add(GetCompletorValue("NtfSergeant", "11"));
            completor.ListValues.Add(GetCompletorValue("NtfCaptain", "12"));
            completor.ListValues.Add(GetCompletorValue("NtfPrivate", "13"));
            completor.ListValues.Add(GetCompletorValue("Tutorial", "14"));
            completor.ListValues.Add(GetCompletorValue("Facility Guard", "15"));
            completor.ListValues.Add(GetCompletorValue("SCP-939-53", "16"));
            completor.ListValues.Add(GetCompletorValue("SCP-939-89", "17"));
            completor.ListValues.Add(GetCompletorValue("ChaosRifleman", "18"));
            completor.ListValues.Add(GetCompletorValue("ChaosRepressor", "19"));
            completor.ListValues.Add(GetCompletorValue("ChaosMarauder", "20"));
            completor.ListValues.Add(GetCompletorValue("GOC Member", "25"));
            completor.ListValues.Add(GetCompletorValue("Serpents Hand", "30"));
            completor.ListValues.Add(GetCompletorValue("SCP-035", "35"));
            completor.ListValues.Add(GetCompletorValue("SCP-056", "56"));
            completor.ListValues.Add(GetCompletorValue("Scp-079-Robot", "79"));
            /*
            completor.ListValues.Add(GetCompletorValue("Janitor", "100"));
            completor.ListValues.Add(GetCompletorValue("Guard Supervisor", "101"));
            completor.ListValues.Add(GetCompletorValue("Scientific Supervisor", "102"));
            completor.ListValues.Add(GetCompletorValue("Site Director", "103"));
            completor.ListValues.Add(GetCompletorValue("Technician", "104"));
            completor.ListValues.Add(GetCompletorValue("Ntf Lieutenant", "105"));
            completor.ListValues.Add(GetCompletorValue("Ntf Commander", "106"));
            completor.ListValues.Add(GetCompletorValue("Ntf Lieutenant Colonel", "107"));
            completor.ListValues.Add(GetCompletorValue("Ntf Recontainment Expert", "108"));
            completor.ListValues.Add(GetCompletorValue("Ntf Pyrotechnics Expert", "109"));
            completor.ListValues.Add(GetCompletorValue("Ntf Nurse", "110"));
            completor.ListValues.Add(GetCompletorValue("Ntf Virologist", "111"));
            completor.ListValues.Add(GetCompletorValue("Foundation UTR", "112"));
            completor.ListValues.Add(GetCompletorValue("Chaos Mastodon", "113"));
            completor.ListValues.Add(GetCompletorValue("Chaos Pyrotechnics Expert", "114"));
            completor.ListValues.Add(GetCompletorValue("Chaos Leader", "115"));
            completor.ListValues.Add(GetCompletorValue("Chaos Hacker", "116"));
            completor.ListValues.Add(GetCompletorValue("Chaos Kamikaze", "117"));
            completor.ListValues.Add(GetCompletorValue("Chaos Intruder", "118"));
            completor.ListValues.Add(GetCompletorValue("Chaos Spy", "119"));
            completor.ListValues.Add(GetCompletorValue("Chaos Nurse", "120"));
            completor.ListValues.Add(GetCompletorValue("SCP-008", "122"));
            completor.ListValues.Add(GetCompletorValue("SCP-966", "123"));
            completor.ListValues.Add(GetCompletorValue("SCP-999", "124"));
            completor.ListValues.Add(GetCompletorValue("SCP-682", "125"));
            completor.ListValues.Add(GetCompletorValue("SCP-1048", "126"));
            completor.ListValues.Add(GetCompletorValue("SCP-1048A", "127"));
            completor.ListValues.Add(GetCompletorValue("SCP-1048B", "128"));
            completor.ListValues.Add(GetCompletorValue("SCP-1048C", "129"));
            completor.ListValues.Add(GetCompletorValue("SCP-1048D", "130"));
            completor.ListValues.Add(GetCompletorValue("SCP-953", "131"));
            completor.ListValues.Add(GetCompletorValue("Anderson Heavy UTR ", "139"));
            completor.ListValues.Add(GetCompletorValue("Anderson Light UTR ", "140"));
            completor.ListValues.Add(GetCompletorValue("Anderson Engineer", "141"));
            completor.ListValues.Add(GetCompletorValue("Anderson Leader", "142"));
            completor.ListValues.Add(GetCompletorValue("HamerDown Cadet", "143"));
            completor.ListValues.Add(GetCompletorValue("HamerDown Lieutenant", "144"));
            completor.ListValues.Add(GetCompletorValue("HamerDown Commander", "145"));
            completor.ListValues.Add(GetCompletorValue("Jail Guard", "146"));
            completor.ListValues.Add(GetCompletorValue("Zone Manager", "147"));
            completor.ListValues.Add(GetCompletorValue("MTF UTR", "148"));
            completor.ListValues.Add(GetCompletorValue("UIU Agent", "149"));
            completor.ListValues.Add(GetCompletorValue("UIU Liaison Officer", "150"));
            completor.ListValues.Add(GetCompletorValue("Asimov General", "151"));
            completor.ListValues.Add(GetCompletorValue("Asimov Guardian", "152"));
            completor.ListValues.Add(GetCompletorValue("AlphaOne Agent", "153"));
            completor.ListValues.Add(GetCompletorValue("Staff", "199"));
            *///Custom Class from VT
            //completor.ListValues.Add(GetCompletorValue("Saphir Leader", "300"));
            //completor.ListValues.Add(GetCompletorValue("Saphir Private", "301"));
            
            completor.ListValues.Add(GetCompletorValue("SCP-682", "682"));

        }

        private void AddTeamsToCompletor(Completor completor)
        {
            completor.ListValues.Add(GetCompletorValue("None", "-1"));
            completor.ListValues.Add(GetCompletorValue("SCP", "0"));
            completor.ListValues.Add(GetCompletorValue("Nine-Tailed Fox", "1"));
            completor.ListValues.Add(GetCompletorValue("Choas Insugency", "2"));
            completor.ListValues.Add(GetCompletorValue("Research", "3"));
            completor.ListValues.Add(GetCompletorValue("Class D Personnel", "4"));
            completor.ListValues.Add(GetCompletorValue("Spectator", "5"));
            completor.ListValues.Add(GetCompletorValue("Tutoriel", "6"));
            completor.ListValues.Add(GetCompletorValue("Serpent's hand ", "7"));
            /*
            completor.ListValues.Add(GetCompletorValue("VIP", "9"));
            completor.ListValues.Add(GetCompletorValue("Hamer Down", "10"));
            completor.ListValues.Add(GetCompletorValue("Netral SCP", " 13"));
            completor.ListValues.Add(GetCompletorValue("Berserk SCP", "14"));
            completor.ListValues.Add(GetCompletorValue("Unusual Incidents Unit", "15"));
            completor.ListValues.Add(GetCompletorValue("Anderson Roblotic", "16"));
            completor.ListValues.Add(GetCompletorValue("Alpha One", "18"));
            completor.ListValues.Add(GetCompletorValue("Global Occult Coalition", "19"));
            *///Team for VT plugin
            //completor.ListValues.Add(GetCompletorValue("SAPHIR", "20")); 
            
        }

        private void AddItemsToCompletor(Completor completor)
        {
            completor.ListValues.Add(GetCompletorValue("None", "-1"));
            completor.ListValues.Add(GetCompletorValue("KeycardJanitor", "0"));
            completor.ListValues.Add(GetCompletorValue("KeycardScientist", "1"));
            completor.ListValues.Add(GetCompletorValue("KeycardResearchCoordinator", "2"));
            completor.ListValues.Add(GetCompletorValue("KeycardZoneManager", "3"));
            completor.ListValues.Add(GetCompletorValue("KeycardGuard", "4"));
            completor.ListValues.Add(GetCompletorValue("KeycardNTFOfficer", "5"));
            completor.ListValues.Add(GetCompletorValue("KeycardContainmentEngineer", "6"));
            completor.ListValues.Add(GetCompletorValue("KeycardNTFLieutenant", "7"));
            completor.ListValues.Add(GetCompletorValue("KeycardNTFCommander", "8"));
            completor.ListValues.Add(GetCompletorValue("KeycardFacilityManager", "9"));
            completor.ListValues.Add(GetCompletorValue("KeycardChaosInsurgency", "10"));
            completor.ListValues.Add(GetCompletorValue("KeycardO5", "11"));
            completor.ListValues.Add(GetCompletorValue("Radio", "12"));
            completor.ListValues.Add(GetCompletorValue("GunCOM15", "13"));
            completor.ListValues.Add(GetCompletorValue("Medkit", "14"));
            completor.ListValues.Add(GetCompletorValue("Flashlight", "15"));
            completor.ListValues.Add(GetCompletorValue("MicroHID", "16"));
            completor.ListValues.Add(GetCompletorValue("SCP500", "17"));
            completor.ListValues.Add(GetCompletorValue("SCP207", "18"));
            completor.ListValues.Add(GetCompletorValue("Ammo12gauge", "19"));
            completor.ListValues.Add(GetCompletorValue("GunE11SR", "20"));
            completor.ListValues.Add(GetCompletorValue("GunCrossvec", "21"));
            completor.ListValues.Add(GetCompletorValue("Ammo556x45", "22"));
            completor.ListValues.Add(GetCompletorValue("GunFSP9", "23"));
            completor.ListValues.Add(GetCompletorValue("GunLogicer", "24"));
            completor.ListValues.Add(GetCompletorValue("GrenadeHE", "25"));
            completor.ListValues.Add(GetCompletorValue("GrenadeFlash", "26"));
            completor.ListValues.Add(GetCompletorValue("Ammo44cal", "27"));
            completor.ListValues.Add(GetCompletorValue("Ammo762x39", "28"));
            completor.ListValues.Add(GetCompletorValue("Ammo9x19", "29"));
            completor.ListValues.Add(GetCompletorValue("GunCOM18", "30"));
            completor.ListValues.Add(GetCompletorValue("SCP018", "31"));
            completor.ListValues.Add(GetCompletorValue("SCP268", "32"));
            completor.ListValues.Add(GetCompletorValue("Adrenaline", "33"));
            completor.ListValues.Add(GetCompletorValue("Painkillers", "34"));
            completor.ListValues.Add(GetCompletorValue("Coin", "35"));
            completor.ListValues.Add(GetCompletorValue("ArmorLight", "36"));
            completor.ListValues.Add(GetCompletorValue("ArmorCombat", "37"));
            completor.ListValues.Add(GetCompletorValue("ArmorHeavy", "38"));
            completor.ListValues.Add(GetCompletorValue("GunRevolver", "39"));
            completor.ListValues.Add(GetCompletorValue("GunAK", "40"));
            completor.ListValues.Add(GetCompletorValue("GunShotgun", "41"));
            completor.ListValues.Add(GetCompletorValue("SCP330", "42"));
            completor.ListValues.Add(GetCompletorValue("SCP2176", "43"));
            completor.ListValues.Add(GetCompletorValue("Tranquilizer", "50"));
            completor.ListValues.Add(GetCompletorValue("GrenadeLauncher", "51"));
            completor.ListValues.Add(GetCompletorValue("Sniper", "53"));
            completor.ListValues.Add(GetCompletorValue("Scp127", "54"));
            completor.ListValues.Add(GetCompletorValue("XlMedkit", "55"));
            completor.ListValues.Add(GetCompletorValue("Scp1499", "56"));
            completor.ListValues.Add(GetCompletorValue("MedkitGun", "58"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_KeycardJanitor", "100"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_KeycardScientist", "101"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_KeycardResearchCoordinator", "102"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_KeycardZoneManager", "103"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_KeycardGuard", "104"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_KeycardNTFOfficer", "105"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_KeycardContainmentEngineer", "106"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_KeycardNTFLieutenant", "107"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_KeycardNTFCommander", "108"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_KeycardFacilityManager", "109"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_KeycardChaosInsurgency", "110"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_KeycardO5", "111"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_Radio", "112"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_GunCOM15", "113"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_Medkit", "114"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_Flashlight", "115"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_MicroHID", "116"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_SCP500", "117"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_SCP207", "118"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_Ammo12gauge", "119"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_GunE11SR", "120"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_GunCrossvec", "121"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_Ammo556x45", "122"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_GunFSP9", "123"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_GunLogicer", "124"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_GrenadeHE", "125"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_GrenadeFlash", "126"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_Ammo44cal", "127"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_Ammo762x39", "128"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_Ammo9x19", "129"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_GunCOM18", "130"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_SCP018", "131"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_SCP268", "132"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_Adrenaline", "133"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_Painkillers", "134"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_Coin", "135"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_ArmorLight", "136"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_ArmorCombat", "137"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_ArmorHeavy", "138"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_GunRevolver", "139"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_GunAK", "140"));
            completor.ListValues.Add(GetCompletorValue("Scp035_Item_GunShotgun", "141"));
        }

        private CompletorValue GetCompletorValue(string help, string value) => new CompletorValue(value, help);
        

        #endregion


        public string ToXml()
        {

            var nameSpace = new XmlSerializerNamespaces();
            // Pour ne pas avoir les xmlns sur la racine 
            nameSpace.Add("", "");

            var stringwriter = new StringWriter();
            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            var xmlWriter = XmlWriter.Create(stringwriter, settings);

            var xmlSerializer = new XmlSerializer(typeof(SymlEditorConfig));
            xmlSerializer.Serialize(xmlWriter, this, nameSpace);

            return stringwriter.ToString();
        }

        private static string FileName => $@"{Application.StartupPath}\EditorConfig.xml";

        public void Save()
        {
            File.WriteAllText(FileName, ToXml());
        }

        public static void Load()
        {

            if (File.Exists(FileName))
            {
                string xml = File.ReadAllText(FileName);
                if (!String.IsNullOrWhiteSpace(xml))
                {
                    var xDoc = XDocument.Parse(xml);
                    var xmlSerializer = new XmlSerializer(typeof(SymlEditorConfig));

                    Singleton = (SymlEditorConfig)xmlSerializer.Deserialize(xDoc.CreateReader());
                }
            }
            else
            {
                Singleton = new SymlEditorConfig();
                Singleton.Default();
            }
        }

        internal Completor GetCompletor(SymlContentItem symlContentItem)
        {
            return Completors.FirstOrDefault(p => p.IsItemCompletor(symlContentItem));
        }

    }
}
