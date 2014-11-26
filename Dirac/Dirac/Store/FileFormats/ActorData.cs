using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Dirac.Store.FileFormats
{
    [Serializable]
    public class ActorData
    {
        public List<ActorSNOMap> ActorSNOMapList { get; set; }

        public ActorData()
        {
            this.ActorSNOMapList = new List<ActorSNOMap>();
        }

        public static void XMLSerialize(String filename, ActorData arg)
        {
            try
            {
                filename = Store.BaseDirection + filename;
                XmlSerializer serializer = null;
                FileStream stream = null;
                serializer = new XmlSerializer(typeof(ActorData));
                stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
                serializer.Serialize(stream, arg);
                if (stream != null)
                    stream.Close();
            }
            catch (Exception ex)
            {
                Logging.LogManager.DefaultLogger.Error(ex.Message);
                Logging.LogManager.DefaultLogger.Error(ex.InnerException.Message);
            }
        }
        public static ActorData XMLDeserialize(String filename)
        {
            try
            {
                filename = Store.BaseDirection + filename;
                XmlSerializer serializer = null;
                FileStream stream = null;
                ActorData emp = new ActorData();
                serializer = new XmlSerializer(typeof(ActorData));
                stream = new FileStream(filename, FileMode.Open);
                emp = (ActorData)serializer.Deserialize(stream);
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

    [Serializable]
    public class ActorSNOMap
    {
        public int SNO { get; set; }
        public String FileName { get; set; }
        public ActorSNOMap()
        {
        }
    }
}
