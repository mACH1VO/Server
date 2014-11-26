using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Dirac.Store.FileFormats
{
    public class NPCData
    {
        public Header Header { get; set; }
        public String MeshFileName { get; set; }
        public List<String> ExtraMeshFileName { get; set; }
        public int NPCType { get; set; }
        public float Scale { get; set; }
        public float LocaldirVectorX { get; set; }
        public float LocaldirVectorY { get; set; }
        public float LocaldirVectorZ { get; set; }
        public List<SkeletalAnimInfoData> SkeletalAnimInfoDataList { get; set; }

        public NPCData(String FileName)
        {
            this.Header = new Header();
            this.ExtraMeshFileName = new List<String>();
            this.SkeletalAnimInfoDataList = new List<SkeletalAnimInfoData>();
        }

        public NPCData()
        {
        }

        public static void XMLSerialize(String filename, NPCData arg)
        {
            filename = Store.NPCBaseDirection + filename;
            XmlSerializer serializer = null;
            FileStream stream = null;
            serializer = new XmlSerializer(typeof(NPCData));
            stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
            serializer.Serialize(stream, arg);
            if (stream != null)
                stream.Close();
        }
        public static NPCData XMLDeserialize(String filename)
        {
            try
            {
                filename = Store.NPCBaseDirection + filename;
                XmlSerializer serializer = null;
                FileStream stream = null;
                NPCData emp = new NPCData();
                serializer = new XmlSerializer(typeof(NPCData));
                stream = new FileStream(filename, FileMode.Open);
                emp = (NPCData)serializer.Deserialize(stream);
                if (stream != null)
                    stream.Close();

                return emp;
            }
            catch (Exception ex)
            {
                Logging.LogManager.DefaultLogger.Error(ex.Message);
                return null;
            }
        }
    }
}
