using System;
using System.Collections.Generic;
using System.Linq;
using Dirac.Logging;
using Dirac.GameServer;

using Dirac.GameServer.Network.Message;
using Dirac.GameServer.Network;

namespace Dirac.GameServer.Core
{
    public class ShopInventory : BaseInventory
    {
        public NPC Owner { get; private set; }
        public ShopInventory(NPC owner)
            :base(16, 10)
        {
            this.Owner = owner;
        }

        protected void sendCreateInventoryItemMessage(InventoryItem item, Player player)
        {
            player.GameClient.SendMessage(new CreateInventoryItemMessage()
            {
                ItemID = item.DynamicID,
                inventoryWindowsId = (int)InventoryWindowsID._SHOP,
                InventorySlot = new InventorySlot(item.InventorySlot.R, item.InventorySlot.C),
                SNO = item.SNOId
            });
        }

        public void loadData(String fileName)
        {

            //CROSSBOWS
            for (int i = 0; i < 1; i++)
            {
                _addItemToInventory(101 + i);
            }

            //WINGS
            /*for (int i = 0; i < 1; i++)
            {
                _addItemToInventory(5033 + i);
            }

            //SETS
            for (int i = 0; i < 5; i++)
            {
                _addItemToInventory(1005 + i);
            }*/

            //MACES
            for (int i = 1; i < 5; i++)
            {
                _addItemToInventory(300 + i);
            }

            //AXES
            for (int i = 1; i < 9; i++)
            {
                _addItemToInventory(400 + i);
            }

            for (int i = 1; i < 4; i++)
            {
                _addItemToInventory(6000 + i);
            }

            for (int i = 2; i < 8; i++)
            {
                _addItemToInventory(6100 + i);
            }

            //DRAGON SET
            for (int i = 5; i < 10; i++)
            {
                _addItemToInventory(1000 + i);
            }

            for (int i = 38; i < 39; i++)
            {
                _addItemToInventory(5000 + i);
            }
            /*//AXES
            for (int i = 1; i < 2; i++)
            {
                _addItemToInventory(400 + i);
            }

            //BOW
            for (int i = 1; i < 8; i++)
            {
                _addItemToInventory(200 + i);
            }*/
           
        }

        private bool _addItemToInventory(int snoId)
        {
            InventoryItem invItem = new InventoryItem(snoId);

            if (!this.hasFreeSpace(invItem))
            {
                Logging.LogManager.DefaultLogger.Trace("[ShopInventory] full, could not find slot for this item in shop, item sno " + invItem.SNOId);
                return false;
            }

            InventorySlot slot = this.findSlotForItem(invItem);
            if (!this.addItemAtPosition(invItem, slot))
            {
                Logging.LogManager.DefaultLogger.Trace("[ShopInventory] could not find slot for this item in shop, item sno " + invItem.SNOId);
                return false;
            }
            invItem.Owner = this.Owner;
            return true;
        }

        public void RevealTo(Player player)
        {
            foreach (var item in this.Items.Values)
            {
                 this.sendCreateInventoryItemMessage((item as InventoryItem), player);
                 (item as InventoryItem).Attributes.BroadcastAllAttributestoPlayer(player);
            }
        }

    }
}