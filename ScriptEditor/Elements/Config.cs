﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using ScriptEditor.ConfigEditor;

namespace ScriptEditor.Elements
{
    [Serializable]
    [XmlRoot("CONFIG")]
    public class Config
    {
        [XmlArray("Completors")]
        [XmlArrayItem("Completor")]
        public List<Completor> Completors
        {
            get; set;
        }

        [XmlArray("Rooms")]
        [XmlArrayItem("Room")]
        public List<string> ValideRooms
        {
            get; set;
        }

        public Config()
        {
            Completors = new List<Completor>();
        }

        public static Config Default()
        {
            var config = new Config();

            var completorBool = new Completor();
            completorBool.Name = "Bool";
            completorBool.ListValues.Add("true");
            completorBool.ListValues.Add("false");
            completorBool.CompletorType = CompletorType.ByValue;
            config.Completors.Add(completorBool);

            var completorRoom = new Completor();
            completorRoom.Name = "Room";
            completorRoom.ListValues.Add("LCZ_Toilets");
            completorRoom.ListValues.Add("LCZ_Cafe (15)");
            completorRoom.ListValues.Add("HCZ_EZ_Checkpoint");
            completorRoom.ContainWord = "room";
            completorRoom.CompletorType = CompletorType.ByName;
            config.Completors.Add(completorRoom);

            config.Save();
            return config;
        }

        public string ToXml()
        {

            var nameSpace = new XmlSerializerNamespaces();
            // Pour ne pas avoir les xmlns sur la racine 
            nameSpace.Add("", "");

            var stringwriter = new StringWriter();
            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            var xmlWriter = XmlWriter.Create(stringwriter, settings);

            var xmlSerializer = new XmlSerializer(typeof(Config));
            xmlSerializer.Serialize(xmlWriter, this, nameSpace);

            return stringwriter.ToString();
        }

        private static string FileName => $@"{Application.StartupPath}\EditorConfig.xml";

        public void Save()
        {
            File.WriteAllText(FileName, ToXml());
        }

        public static Config Load()
        {

            if (File.Exists(FileName))
            {
                string xml = File.ReadAllText(FileName);
                if (!String.IsNullOrWhiteSpace(xml))
                {
                    var xDoc = XDocument.Parse(xml);
                    var xmlSerializer = new XmlSerializer(typeof(Config));

                    return (Config)xmlSerializer.Deserialize(xDoc.CreateReader());
                }
            }

            return Config.Default();
        }

        internal Completor GetCompletor(SymlContentItem symlContentItem)
        {
            return Completors.FirstOrDefault(p => p.IsItemCompletor(symlContentItem));
        }

        internal void AddRoom(SymlContentItem item)
        {
            if (!ValideRooms.Contains(item.Value))
            {
                ValideRooms.Add(item.Value);
                Save();
            }
        }
    }
}
