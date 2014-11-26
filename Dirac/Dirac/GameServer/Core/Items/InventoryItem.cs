using System.Collections.Generic;
using System;
using System.Linq;
using Dirac.Logging;

using Dirac.GameServer;
using Dirac.GameServer.Types;
using Dirac;

using Dirac.Store.FileFormats;
using Dirac.Extensions;
using Dirac.Math;

using Dirac.GameServer.Network.Message;

namespace Dirac.GameServer.Core
{
    public class InventoryItem : DynamicObject, IInventoriable
    {
        public int SNOId { get; set; }

        public Actor Owner { get; set; } 

        public InventorySlot InventorySlot { get; set; } // Column, row;

        public InventorySize InventorySize { get; set; }

        public ItemData Data { get; set; }

        public EquipmentSlotId EquipmentSlot { get; set; } // should be enum for types?

        public String FileData { get; set; }

        public GameAttributes Attributes { get; set; }

        public InventoryItem(Int32 _sNOId)
            : base(Game.NewActorID)
        {
            this.SNOId = _sNOId;
            this.FileData = Actor.SNOToFile[_sNOId];
            this.Data = new ItemData();
            this.Data = ItemData.XMLDeserialize(this.FileData + ".xml");

            this.InventorySize = this.Data.InventorySize;
            this.InventorySlot = new InventorySlot(); //hmmm nose si ponerle -1 -1 o que...
            this.Owner = null;
            this.Attributes = new GameAttributes(this);
            //this.Name = this.Data.Name;
            this.Attributes[GameAttributeStaticList.Level] = RandomHelper.Next(5, 20);
            this.Attributes[GameAttributeStaticList.Defense] = RandomHelper.Next(5, 150);
            this.Attributes[GameAttributeStaticList.Damage_Weapon_Min] = 25;
            this.Attributes[GameAttributeStaticList.Damage_Weapon_Max] = 30;
            this.Attributes[GameAttributeStaticList.Speed] = 25;
            this.Attributes[GameAttributeStaticList.Skill] = 2;
            this.Attributes[GameAttributeStaticList.Durability_Cur] = 58;
            this.Attributes[GameAttributeStaticList.Durability_Max] = 60;
            //this.CurrentState = ItemState.Normal;
        }

        public void FillData(Item item)
        {
            this.Attributes.FillDataFrom(item.Attributes);
        }

        public void FillData(InventoryItem inventoryitem)
        {
            this.Attributes.FillDataFrom(inventoryitem.Attributes);
        }

        public override void Destroy()
        {

        }

        public VisualItem GetVisualItem()
        {
            return new VisualItem()
            {
                snoId = this.SNOId,
                SlotID = (int)this.EquipmentSlot,
                Field2 = this.SNOId,
                Field3 = -1
            };
        }
    }
}
