using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Handler.Save;

namespace MainProgram
{
    class Program
    {
        static SaveHandler m_SaveHandler = null;

        static void Init()
        {
            m_SaveHandler = new SaveHandler();

            //
            //Create a generic XmlFile
            string genericLocation = @"..\GenericXmlFile.xml";
            //bool isGenericCreated = saveFactory.CreateFile<XmlFile>(generic, new XmlFile(), false);
            bool isGenericCreated = m_SaveHandler.CreateFile<XmlFile>(genericLocation, new XmlFile("Generic"), true);

            //
            // Create a custom XmlFile
            string customXmlFileLocation = @"..\CustomXmlFile.xml";
            m_SaveHandler.CreateFile<CustomXmlFile>(customXmlFileLocation, new CustomXmlFile(), true);

            //
            // Create a custom serializable file.
            string customFileLocation = @"..\CustomFile.xml";
            SampleFile customFile = new SampleFile();
            customFile.listItems.Add("Item1");
            customFile.listItems.Add("Item2");
            customFile.listItems.Add("Item3");

            SampleFile.KeyValuePair<string, string> KeyValue1 = new SampleFile.KeyValuePair<string, string>();
            KeyValue1.Key = "Key1";
            KeyValue1.Value = "Value1";
            customFile.listKeyValue.Add(KeyValue1);

            SampleFile.KeyValuePair<string, string> KeyValue2 = new SampleFile.KeyValuePair<string, string>();
            KeyValue2.Key = "Key2";
            KeyValue2.Value = "Value2";
            customFile.listKeyValue.Add(KeyValue2);

            m_SaveHandler.CreateFile<SampleFile>(customFileLocation, customFile, true);
        }

        static void Main(string[] args)
        {
            Init();
            // Console.ReadLine();
        }
    }

    [Serializable]
    public class CustomXmlFile : XmlFile
    {
        public CustomXmlFile()
        {
            Title = "CustomXmlFile";
            Element1 = 0;
            Element2 = "value";
        }

        [XmlElement]
        public short Element1;

        [XmlElement("Element2")]
        public string Element2;
    }

    [Serializable]
    public class SampleFile
    {

        [XmlType(TypeName = "KeyValues")]
        public struct KeyValuePair<K, V>
        {
            [XmlAttribute]
            public K Key { get; set; }

            [XmlAttribute]
            public V Value { get; set; }
        }

        public SampleFile()
        {
            listItems = new List<string>();
            listItemsArray = listItems;
            listKeyValue = new List<KeyValuePair<string, string>>();
        }

        ~SampleFile()
        {
            if (listItems != null)
                listItems = null;

            if (listItemsArray != null)
                listItemsArray = null;

            if (listKeyValue != null)
                listKeyValue = null;
        }

        [XmlElement]
        public List<string> listItems = null;

        [XmlArray]
        public List<string> listItemsArray = null;

        [XmlElement]
        public List<KeyValuePair<string, string>> listKeyValue = null;
    }
}
