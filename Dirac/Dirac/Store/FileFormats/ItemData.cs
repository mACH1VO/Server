using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using Dirac.GameServer;
using Dirac.GameServer.Core;

namespace Dirac.Store.FileFormats
{
    [Serializable]
    public class ItemData
    {
        public Header Header { get; set; }
        public ClothingInfo ClothingInfo { get; set; }
        public String MeshFileName { get; set; }
        public List<String> ExtraMeshFileName { get; set; }
        public List<String> ItemEquipedMeshFileNames { get; set; }
        public String SkinName { get; set; }

        public String IdleAnim { get; set; }
        public String WalkAnim { get; set; }
        public String AttackAnim { get; set; }
        public String SkillAnim { get; set; }

        public float Scale3D { get; set; }
        public float ScaleTagPoint { get; set; }
        public int Hash { get; set; }
        public string Name { get; set; }
        public int SNOActor { get; set; }
        public String ItemType { get; set; }
        public int ItemLevel { get; set; }
        public int BaseGoldValue { get; set; }
        public int RequiredLevel { get; set; }
        public int DurabilityMin { get; set; }
        public int DurabilityDelta { get; set; }
        public float WeaponDamageMin { get; set; }
        public float WeaponDamageDelta { get; set; }
        public float ArmorValue { get; set; }
        public float AttacksPerSecond { get; set; }
        public ItemQuality Quality { get; set; }
        public InventorySize InventorySize { get; set; }

        public ItemData(String FileName)
        {
            this.Header = new Header();
            this.ClothingInfo = new ClothingInfo();
            this.InventorySize = new InventorySize();
            this.ExtraMeshFileName = new List<String>();
        }

        public ItemData()
        {
            /*this.Header = new Header();
            this.Header.FileName = "Bow24.xml";
            this.Header.SNOId = 1;
            this.Header.SnoType = 1;
            this.Hash = 5555;
            this.Name = "Slphyd Bow";
            this.SNOActor = 1;
            this.ItemType = 1;
            this.ItemLevel = 1;
            this.BaseGoldValue = 1;
            this.RequiredLevel = 1;
            this.DurabilityMin = 1;
            this.DurabilityDelta = 1;
            this.WeaponDamageDelta = 1;
            this.WeaponDamageMin = 1;
            this.ArmorValue = 50;
            this.AttacksPerSecond = 5;
            this.Quality = ItemQuality.Magic1;*/
        }

        public static void XMLSerialize(String filename, ItemData arg)
        {
            filename = Store.BaseDirection + filename;
            XmlSerializer serializer = null;
            FileStream stream = null;
            serializer = new XmlSerializer(typeof(ItemData));
            stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
            serializer.Serialize(stream, arg);
            if (stream != null)
                stream.Close();
        }
        public static ItemData XMLDeserialize(String filename)
        {
            try
            {
                filename = Store.BaseDirection + filename;
                XmlSerializer serializer = null;
                FileStream stream = null;
                ItemData emp = new ItemData();
                serializer = new XmlSerializer(typeof(ItemData));
                stream = new FileStream(filename, FileMode.Open);
                emp = (ItemData)serializer.Deserialize(stream);
                if (stream != null)
                    stream.Close();

                return emp;
            }
            catch (InvalidOperationException ex)
            {
                Logging.LogManager.DefaultLogger.Error(ex.Message);
                return null;
                //Logging.LogManager.DefaultLogger.Error(ex.Data);

            }
            
        }
    }

    public enum ItemQuality
    {
        Invalid = -1,
        Inferior,
        Normal,
        Superior,
        Magic1,
        Magic2,
        Magic3,
        Rare4,
        Rare5,
        Rare6,
        Legendary,
        Artifact,
    }


}
