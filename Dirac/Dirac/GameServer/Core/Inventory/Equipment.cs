

using System;
using System.Collections.Generic;
using System.Linq;
using Dirac.Logging;

using Dirac.GameServer;
using Dirac.GameServer.Network;
using Dirac.GameServer.Network.Message;

namespace Dirac.GameServer.Core
{
    public class Equipment //: IRevealable //dont know how to implement this yet!
    {

        public Dictionary<int, InventoryItem> ItemsEquiped { get; set; }
        public Dictionary<EquipmentSlotId, InventoryItem> itemsEquipedBySlot { get; set; }

        private readonly Player _owner; // Used, because most information is not in the item class but Actors managed by the world

        public InventoryItem Helm { get; set; }
        public InventoryItem Armor { get; set; }
        public InventoryItem Pants { get; set; }
        public InventoryItem Boots { get; set; }
        public InventoryItem Gloves { get; set; }
        public InventoryItem Weapon { get; set; }
        public InventoryItem Shield { get; set; }
        public InventoryItem Wing { get; set; }

        public Equipment(Player owner)
        {
            this._owner = owner;
            this.ItemsEquiped = new Dictionary<int, InventoryItem>();
            this.itemsEquipedBySlot = new Dictionary<EquipmentSlotId, InventoryItem>();
            foreach (EquipmentSlotId slots in Enum.GetValues(typeof(EquipmentSlotId)))
            {
                itemsEquipedBySlot.Add(slots, null);
            }
        }

        /// <summary>
        /// Equips an item in an equipment slot
        /// </summary>
        public void EquipItem(InventoryItem item, EquipmentSlotId slot)
        {
            ItemsEquiped.Add(item.DynamicID, item);
            item.Owner = _owner;
            //item.Attributes[GameAttributeStaticList.Item_Equipped] = true; // Probaly should be handled by Equipable class
            //item.Attributes.SendChangedMessage(_owner.GameClient);

            item.EquipmentSlot = slot;

            if (!itemsEquipedBySlot.ContainsKey(slot))
                itemsEquipedBySlot.Add(slot, item);
            else
                itemsEquipedBySlot[slot] = item;
        }

        /// <summary>
        /// Removes an item from the equipment slot it uses
        /// returns the used equipmentSlot
        /// </summary>
        public EquipmentSlotId UnequipItem(InventoryItem item)
        {
            ItemsEquiped.Remove(item.DynamicID);
            itemsEquipedBySlot[item.EquipmentSlot] = null;

            item.EquipmentSlot = EquipmentSlotId.Inventory;

            return item.EquipmentSlot;
        }

        public bool IsItemEquipped(InventoryItem item)
        {
            return this.ItemsEquiped.ContainsKey(item.DynamicID);
        }

        public bool IsSlotEmpty(EquipmentSlotId slot)
        {
            return (this.itemsEquipedBySlot[slot] == null);
        }

        public VisualItem[] GetCurrentVisualEquipment()
        {
            InventoryItem item;
            VisualItem empty_visualitem;
            VisualItem[] result = new VisualItem[9];

            item = this.itemsEquipedBySlot[EquipmentSlotId.Helm];
            if (item == null)
            {
                empty_visualitem = new VisualItem();
                empty_visualitem.SlotID = (int)EquipmentSlotId.Helm;
                empty_visualitem.Field2 = 0;
                empty_visualitem.Field3 = 0;
                empty_visualitem.snoId = 0;
                result[0] = empty_visualitem;
            }
            else
            {
                result[0] = item.GetVisualItem();
            }

            item = this.itemsEquipedBySlot[EquipmentSlotId.Armor];
            if (item == null)
            {
                empty_visualitem = new VisualItem();
                empty_visualitem.SlotID = (int)EquipmentSlotId.Armor;
                empty_visualitem.Field2 = 0;
                empty_visualitem.Field3 = 0;
                empty_visualitem.snoId = 0;
                result[1] = empty_visualitem;
            }
            else
            {
                result[1] = item.GetVisualItem();
            }

            item = this.itemsEquipedBySlot[EquipmentSlotId.Gloves];
            if (item == null)
            {
                empty_visualitem = new VisualItem();
                empty_visualitem.SlotID = (int)EquipmentSlotId.Gloves;
                empty_visualitem.Field2 = 0;
                empty_visualitem.Field3 = 0;
                empty_visualitem.snoId = 0;
                result[2] = empty_visualitem;
            }
            else
            {
                result[2] = item.GetVisualItem();
            }

            item = this.itemsEquipedBySlot[EquipmentSlotId.Pants];
            if (item == null)
            {
                empty_visualitem = new VisualItem();
                empty_visualitem.SlotID = (int)EquipmentSlotId.Pants;
                empty_visualitem.Field2 = 0;
                empty_visualitem.Field3 = 0;
                empty_visualitem.snoId = 0;
                result[3] = empty_visualitem;
            }
            else
            {
                result[3] = item.GetVisualItem();
            }

            item = this.itemsEquipedBySlot[EquipmentSlotId.Boots];
            if (item == null)
            {
                empty_visualitem = new VisualItem();
                empty_visualitem.SlotID = (int)EquipmentSlotId.Boots;
                empty_visualitem.Field2 = 0;
                empty_visualitem.Field3 = 0;
                empty_visualitem.snoId = 0;
                result[4] = empty_visualitem;
            }
            else
            {
                result[4] = item.GetVisualItem();
            }


            item = this.itemsEquipedBySlot[EquipmentSlotId.Weapon_L];
            if (item == null)
            {
                empty_visualitem = new VisualItem();
                empty_visualitem.SlotID = (int)EquipmentSlotId.Weapon_L;
                empty_visualitem.Field2 = 0;
                empty_visualitem.Field3 = 0;
                empty_visualitem.snoId = 0;
                result[5] = empty_visualitem;
            }
            else
            {
                result[5] = item.GetVisualItem();
            }

            item = this.itemsEquipedBySlot[EquipmentSlotId.Weapon_R];
            if (item == null)
            {
                empty_visualitem = new VisualItem();
                empty_visualitem.SlotID = (int)EquipmentSlotId.Weapon_R;
                empty_visualitem.Field2 = 0;
                empty_visualitem.Field3 = 0;
                empty_visualitem.snoId = 0;
                result[6] = empty_visualitem;
            }
            else
            {
                result[6] = item.GetVisualItem();
            }

            item = this.itemsEquipedBySlot[EquipmentSlotId.Wing];
            if (item == null)
            {
                empty_visualitem = new VisualItem();
                empty_visualitem.SlotID = (int)EquipmentSlotId.Wing;
                empty_visualitem.Field2 = 0;
                empty_visualitem.Field3 = 0;
                empty_visualitem.snoId = 0;
                result[7] = empty_visualitem;
            }
            else
            {
                result[7] = item.GetVisualItem();
            }


            item = this.itemsEquipedBySlot[EquipmentSlotId.Fairy];
            if (item == null)
            {
                empty_visualitem = new VisualItem();
                empty_visualitem.SlotID = (int)EquipmentSlotId.Fairy;
                empty_visualitem.Field2 = 0;
                empty_visualitem.Field3 = 0;
                empty_visualitem.snoId = 0;
                result[8] = empty_visualitem;
            }
            else
            {
                result[8] = item.GetVisualItem();
            }

            return result;
        }

        /*private VisualItem GetEquipmentItem(EquipmentSlotId equipSlot)
        {
            if (_equipment[(int)equipSlot] == 0)
            {
                return new VisualItem()
                {
                    GbId = -1, // 0 causes error logs on the client  - angerwin
                    Field1 = 0,
                    Field2 = 0,
                    Field3 = 0,
                };
            }

            return ItemsEquiped[(_equipment[(int)equipSlot])].CreateVisualItem();
        }*/

        /*private D3.Hero.VisualItem GetEquipmentItemForToon(EquipmentSlotId equipSlot)
        {
            if (_equipment[(int)equipSlot] == 0)
            {
                return D3.Hero.VisualItem.CreateBuilder()
                    .SetGbid(-1)
                    .SetDyeType(0)
                    .SetEffectLevel(0)
                    .SetItemEffectType(-1)
                    .Build();
            }

            return ItemsEquiped[(_equipment[(int)equipSlot])].GetVisualItem();
        }*/


        /*public bool Reveal(Player player)
        {
            foreach (var item in ItemsEquiped.Values)
            {
                item.Reveal(player);
            }
            return true;
        }

        public bool Unreveal(Player player)
        {
            foreach (var item in ItemsEquiped.Values)
            {
                item.Unreveal(player);
            }
            return true;
        }*/
    }

    public enum EquipmentSlotId : int
    {
        Inventory = 0,
        Helm = 1,
        Armor = 2,
        Weapon_L = 3,
        Weapon_R = 4,
        Gloves = 5,
        Necklace = 6,
        Boots = 7,
        Shoulders = 8,
        Pants = 9,
        Bracers = 10,
        Ring_right = 11,
        Ring_left = 12,
        Fairy = 13,
        Skills = 14,
        Stash = 15,
        Gold = 16,
        BackPack = 17,
        Wing = 18
    }
}
