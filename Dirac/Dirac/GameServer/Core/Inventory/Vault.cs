using System;
using System.Collections.Generic;
using System.Linq;
using Dirac.Logging;
using Dirac.GameServer;

using Dirac.GameServer.Network.Message;
using Dirac.GameServer.Network;

namespace Dirac.GameServer.Core
{
    public class Vault : BaseInventory
    {
        public Player Owner { get; private set; }
        public Vault(Player owner)
            :base(16, 11)
        {
            this.Owner = owner;
        }

        protected void sendCreateInventoryItemMessage(InventoryItem item)
        {
            Player p = this.Owner as Player;
            p.GameClient.SendMessage(new CreateInventoryItemMessage()
            {
                ItemID = item.DynamicID,
                inventoryWindowsId = this.Owner.DynamicID,
                InventorySlot = new InventorySlot(item.InventorySlot.R, item.InventorySlot.C),
                SNO = item.SNOId
            });
        }

        /// <summary>
        /// Picks an item up after client request
        /// </summary>
        /// <returns>true if the item was picked up, or false if the player could not pick up the item.</returns>
        public bool TryPickUp(Item item)
        {
            InventoryItem invItem = new InventoryItem(item.SNOId);
            if (!this.hasFreeSpace(invItem))
            {
                Logging.LogManager.DefaultLogger.Trace("Inventory full, could not pick up item " + item.DynamicID);
                return false;
            }

            InventorySlot slot = this.findSlotForItem(invItem);
            this.addItemAtPosition(invItem, slot);

            invItem.Owner = this.Owner;
            this.sendCreateInventoryItemMessage(invItem);
            return true;
        }

        /// <summary>
        /// Handles a request to move an item within the inventory.
        /// This covers moving items within the backpack, from equipment
        /// slot to backpack and from backpack to equipment slot
        /// </summary>
        public void OnInventoryRequestMoveMessage(InventoryRequestMoveMessage request)
        {
            InventoryItem inv_item;
            InventoryWindowsID sourceWindow;
            if (!this.Contains(request.ItemID))
            {
                if (!(this.Owner as Player).Inventory.Contains(request.ItemID))
                {
                    Logging.LogManager.DefaultLogger.Error("[Vault] Trying to move an item that is not in vault or inventory!");
                    return;
                }
                else
                {
                    inv_item = (InventoryItem)(this.Owner as Player).Inventory.Items[request.ItemID];
                    sourceWindow = InventoryWindowsID.PlayerInventory;
                }
            }
            else
            {
                inv_item = (InventoryItem)this.Items[request.ItemID];
                sourceWindow = InventoryWindowsID.Vault;
            }


            if (sourceWindow == InventoryWindowsID.Vault) 
            {
                //inventory to inventory
                if (!this.Contains(inv_item.DynamicID))
                {
                    Logging.LogManager.DefaultLogger.Error("Trying to move an item that is not in vault");
                    return;
                }

                if (!this.canPutitemThere_checking_self_item(inv_item, request.Row, request.Column))
                {
                    Logging.LogManager.DefaultLogger.Error("Trying to put an item on a ocupped place (saltea el cliente check?)");
                    return;
                }

                if (!this.removeItem(inv_item))
                {
                    Logging.LogManager.DefaultLogger.Error("could not remove item!, vault to vault operation");
                    return;
                }

                InventorySlot _slot = new InventorySlot(request.Row, request.Column);
                if (!this.addItemAtPosition(inv_item, _slot))
                {
                    Logging.LogManager.DefaultLogger.Error("could not add item at position, from vault to vault!");
                    return;
                }
                this.sendAcceptMoveRequest(inv_item, sourceWindow, InventoryWindowsID.Vault);
            }

            if (sourceWindow == InventoryWindowsID.PlayerInventory)
            {
                PlayerInventory player_inventory = this.Owner.Inventory;
                //inventory to inventory
                if (!player_inventory.Contains(inv_item.DynamicID))
                {
                    Logging.LogManager.DefaultLogger.Error("Trying to move an item that is not in playerInventory!");
                    return;
                }

                if (!this.canPutitemThere_checking_self_item(inv_item, request.Row, request.Column))
                {
                    Logging.LogManager.DefaultLogger.Error("Trying to put an item on a ocupped place (saltea el cliente check?)");
                    return;
                }

                //this is different, remove from inventory!
                
                if (!player_inventory.removeItem(inv_item))
                {
                    Logging.LogManager.DefaultLogger.Error("could not remove item!, playerinventory to vault operation");
                    return;
                }

                InventorySlot _slot = new InventorySlot(request.Row, request.Column);
                if (!this.addItemAtPosition(inv_item, _slot))
                {
                    Logging.LogManager.DefaultLogger.Error("could not add item at position, from playerinventory to vault!");
                    return;
                }
                this.sendAcceptMoveRequest(inv_item, sourceWindow, InventoryWindowsID.Vault);
            }
        }

        public void sendAcceptMoveRequest(InventoryItem item, InventoryWindowsID origin, InventoryWindowsID dest)
        {
            InventoryResponseMoveMessage msg = new InventoryResponseMoveMessage();
            msg.Column = item.InventorySlot.C;
            msg.Row = item.InventorySlot.R;
            msg.originWindowsID = (int)origin;
            msg.destinationWindowsID = (int)dest;
            msg.ItemID = item.DynamicID;
            msg.EquipmentSlot = (int)item.EquipmentSlot;
            (this.Owner as Player).GameClient.SendMessage(msg);
        }
        /// <summary>
        /// Handles a request to move an item from stash the inventory and back
        /// </summary>
        public void HandleInventoryRequestQuickMoveMessage(InventoryRequestQuickMoveMessage request)
        {
            /*Item item = GetItem(request.ItemID);
            if (item == null || (request.DestEquipmentSlot != (int)EquipmentSlotId.Stash && request.DestEquipmentSlot != (int)EquipmentSlotId.Inventory))
                return;
            // Identify source and destination grids
            var destinationGrid = request.DestEquipmentSlot == 0 ? _inventoryGrid : _stashGrid;
            var sourceGrid = request.DestEquipmentSlot == 0 ? _stashGrid : _inventoryGrid;

            if (destinationGrid.HasFreeSpace(request.DestRowStart, request.DestRowEnd, item))
            {
                sourceGrid.RemoveItem(item);
                destinationGrid._addItemAtPosition(request.DestRowStart, request.DestRowEnd, item);
            }*/
        }
    }
}