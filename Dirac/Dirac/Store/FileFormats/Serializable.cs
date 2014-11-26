using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Dirac.Store.FileFormats
{
    [Serializable]
    public abstract class Serializable
    {
        public void Serialize(String FileName)
        {
            XmlSerializer serializer = null;
            FileStream stream = null;
            Type type = this.GetType();
            serializer = new XmlSerializer(type);
            stream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite);
            serializer.Serialize(stream, this);
            if (stream != null)
                stream.Close();
        }

        public void Deserialize(String FileName)
        {
            XmlSerializer serializer = null;
            FileStream stream = null;
            Type type = this.GetType();
            serializer = new XmlSerializer(type);
            stream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite);
            object pepe = serializer.Deserialize(stream);
            if (stream != null)
                stream.Close();
        }
    }
}
