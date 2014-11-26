using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Dirac.Store.FileFormats
{
    [Serializable]
    public class ProjectileData
    {
        public Header Header { get; set; }
        public String MeshFileName { get; set; }
        public List<String> ExtraMeshFileName { get; set; }
        public float Scale { get; set; }

        public ProjectileData(String FileName)
        {
            this.Header = new Header();
            this.ExtraMeshFileName = new List<String>();
        }

        public ProjectileData()
        {
        }

        public static void XMLSerialize(String filename, ProjectileData arg)
        {
            filename = Store.BaseDirection + filename;
            XmlSerializer serializer = null;
            FileStream stream = null;
            serializer = new XmlSerializer(typeof(ProjectileData));
            stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
            serializer.Serialize(stream, arg);
            if (stream != null)
                stream.Close();
        }

        public static ProjectileData XMLDeserialize(String filename)
        {
            filename = Store.BaseDirection + filename;
            XmlSerializer serializer = null;
            FileStream stream = null;
            ProjectileData emp = new ProjectileData();
            serializer = new XmlSerializer(typeof(ProjectileData));
            stream = new FileStream(filename, FileMode.Open);
            emp = (ProjectileData)serializer.Deserialize(stream);
            if (stream != null)
                stream.Close();

            return emp;
        }
    }
}
