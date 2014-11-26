using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;


namespace Dirac.Store.FileFormats
{
    [Serializable]
    public class PlayerData
    {
        public Header Header { get; set; }
        public String MeshFileName { get; set; }
        public List<String> ExtraMeshFileName { get; set; }

        public PlayerData()
        {
            this.ExtraMeshFileName = new List<String>();
        }
        
        public static void XMLSerialize(String filename, PlayerData arg)
        {
            filename = Store.BaseDirection + filename;
            XmlSerializer serializer = null;
            FileStream stream = null;
            serializer = new XmlSerializer(typeof(PlayerData));
            stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
            serializer.Serialize(stream, arg);
            if (stream != null)
                stream.Close();
        }
        
        public static PlayerData XMLDeserialize(String filename)
        {
            filename = Store.BaseDirection + filename;
            XmlSerializer serializer = null;
            FileStream stream = null;
            PlayerData emp = new PlayerData();
            serializer = new XmlSerializer(typeof(PlayerData));
            stream = new FileStream(filename, FileMode.Open);
            emp = (PlayerData)serializer.Deserialize(stream);
            if (stream != null)
                stream.Close();

            return emp;
        }
    }
}
