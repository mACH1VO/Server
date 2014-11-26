using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Dirac.Store.FileFormats
{
    public class MonsterData
    {
        public Header Header { get; set; }
        public String MeshFileName { get; set; }
        public List<String> ExtraMeshFileName { get; set; }
        public float UpPosition { get; set; }
        public float Scale { get; set; }
        public float LocaldirVectorX { get; set; }
        public float LocaldirVectorY { get; set; }
        public float LocaldirVectorZ { get; set; }
        public List<SkeletalAnimInfoData> SkeletalAnimInfoDataList { get; set; }

        public MonsterData(String FileName)
        {
            this.Header = new Header();
            this.ExtraMeshFileName = new List<String>();
            this.SkeletalAnimInfoDataList = new List<SkeletalAnimInfoData>();
        }

        public MonsterData()
        {
        }

        public static void XMLSerialize(String filename, MonsterData arg)
        {
            filename = Store.MonsterBaseDirection + filename;
            XmlSerializer serializer = null;
            FileStream stream = null;
            serializer = new XmlSerializer(typeof(MonsterData));
            stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
            serializer.Serialize(stream, arg);
            if (stream != null)
                stream.Close();
        }
        public static MonsterData XMLDeserialize(String filename)
        {
            try
            {
                filename = Store.MonsterBaseDirection + filename;
                XmlSerializer serializer = null;
                FileStream stream = null;
                MonsterData emp = new MonsterData();
                serializer = new XmlSerializer(typeof(MonsterData));
                stream = new FileStream(filename, FileMode.Open);
                emp = (MonsterData)serializer.Deserialize(stream);
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
    public class SkeletalAnimInfoData
    {
        public AnimType animType { get; set; }
        public float initTimeSeconds { get; set; }
        public float endTimeSeconds { get; set; }

        public SkeletalAnimInfoData(AnimType animType, float init, float end)
        {
            this.animType = animType;
            this.initTimeSeconds = init;
            this.endTimeSeconds = end;
        }

        public SkeletalAnimInfoData(AnimType animType, int initFrame, int endFrame, int FPS)
        {
            this.initTimeSeconds = (float)initFrame / (float)FPS;
            this.endTimeSeconds = (float)endFrame / (float)FPS;
        }

        public SkeletalAnimInfoData()
        {
        }
    }
    public enum AnimType
    {
        Walk,
        Run,
        Idle,
        IdlePeace,
        IdleEmotion1,
        IdleEmotion2,
        Attack1,
        Attack2,
        Attack3,
        Attack4,
        Groggy1,
        Skill1,
        Skill2,
        GetHit1,
        Die1,
    }
}
