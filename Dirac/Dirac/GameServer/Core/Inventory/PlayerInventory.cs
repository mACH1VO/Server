using System;
using System.Collections.Generic;
using System.Linq;
using Dirac.Logging;
using Dirac.GameServer;

using Dirac.GameServer.Network.Message;
using Dirac.GameServer.Network;

namespace Dirac.GameServer.Core
{
    public partial class PlayerInventory : BaseInventory
    {
        public Player Owner { get; private set; }
        public Vault Vault { get; set; }

        private Equipment _equipment;

       
        public PlayerInventory(Player owner)
            :base(7, 12)
        {
            this.Owner = owner;
            this._equipment = new Equipment(owner);
            this.Vault = new Vault(owner);
        }
        public void Consume(GameClient client, GameMessage message)
        {
            switch (message.opcodes)
            {
                case Opcodes.InventoryRequestMoveMessage:
                    {
                        InventoryRequestMoveMessage msg = (InventoryRequestMoveMessage)message;
                        InventoryWindowsID dest_window = (InventoryWindowsID)msg.inventoryWindowId;
                        if (dest_window == InventoryWindowsID.Vault)
                        {
                            this.Vault.OnInventoryRequestMoveMessage(msg);
                        }
                        
                        if (dest_window == InventoryWindowsID.PlayerInventory)
                        {
                            this.OnInventoryRequestMoveMessage(msg);
                        }

                        break;
                    }
                case Opcodes.InventoryRequestBuyItemMessage:
                    {
                        InventoryRequestBuyItemMessage msg = (InventoryRequestBuyItemMessage)message;
                        if (!this.Owner.IsShopOpened)
                        {
                            Logging.LogManager.DefaultLogger.Error("Trying to buy but shop is not opened!");
                            break;
                        }

                        if (!this.Owner.OpenedNPC.ShopInventory.Contains(msg.ItemId))
                        {
                            Logging.LogManager.DefaultLogger.Error("Trying to buy an item that is not in the SHOP!");
                            break;
                        }

                        InventoryItem shop_item = (InventoryItem)this.Owner.OpenedNPC.ShopInventory.Items[msg.ItemId];
                        if (!this.hasFreeSpace(shop_item))
                        {
                            Logging.LogManager.DefaultLogger.Error("could not buy the item, inventory has not enough space!");
                            return;
                        }

                        InventoryItem new_item = new InventoryItem(shop_item.SNOId);
                        new_item.FillData(shop_item);
                        //fill new_item specific data from shop_item, this would be like cloning shop_item!

                        InventorySlot slot = this.findSlotForItem(new_item);
                        if (!this.canPutitemThere_checking_self_item(new_item, slot.R, slot.C))
                        {
                            Logging.LogManager.DefaultLogger.Error("could not put item there!");
                            return;
                        }
                        if (!this.addItemAtPosition(new_item, slot))
                        {
                            Logging.LogManager.DefaultLogger.Error("could not add item at position, buying item error!");
                            return;
                        }

                        CreateInventoryItemMessage create_item_msg = new CreateInventoryItemMessage();

                        create_item_msg.InventorySlot = slot;
                        create_item_msg.inventoryWindowsId = (int)InventoryWindowsID.PlayerInventory;
                        create_item_msg.SNO = new_item.SNOId;
                        create_item_msg.ItemID = new_item.DynamicID;

                        this.Owner.GameClient.SendMessage(create_item_msg);
                        new_item.Attributes.BroadcastAllAttributestoPlayer(this.Owner);

                        break;
                    }
                case Opcodes.InventoryRequestSellItemMessage:
                    {
                        InventoryRequestSellItemMessage msg = (InventoryRequestSellItemMessage)message;

                        InventoryItem inv_item;
                        if (!this.Contains(msg.ItemId))
                        {
                             Logging.LogManager.DefaultLogger.Error("Trying to SELL an item that is not in inventory!");
                             break;
                        }
                        else
                        {
                            inv_item = (InventoryItem)this.Items[msg.ItemId];
                        }


                        if (!this.removeItem(inv_item))
                        {
                             Logging.LogManager.DefaultLogger.Error("could not remove item!, sell item operation");
                             return;
                        }

                        DestroyInventoryItemMessage destroy_msg = new DestroyInventoryItemMessage();
                        destroy_msg.inventoryWindowId = (int)InventoryWindowsID.PlayerInventory;
                        destroy_msg.InventorySlot = inv_item.InventorySlot;
                        destroy_msg.ItemID = inv_item.DynamicID;
                        this.Owner.GameClient.SendMessage(destroy_msg);

                        break;
                    }
                case Opcodes.InventoryRequestQuickMoveMessage:
                    {
                        HandleInventoryRequestQuickMoveMessage(message as InventoryRequestQuickMoveMessage);
                        break;
                    }
                case Opcodes.InventoryDropItemMessage:
                    {
                        OnInventoryDropItemMessage(message as InventoryDropItemMessage);
                        break;
                    }
                case Opcodes.InventoryRequestJewelUse:
                    {
                        InventoryRequestJewelUse msg = (InventoryRequestJewelUse)message;
                        Logging.LogManager.DefaultLogger.Trace("Item Blessed!, id ", msg.destItemId);

                        InventoryItem source_jewel = (InventoryItem)this.Items[msg.sourceJewelId];
                        InventoryItem dest_item = (InventoryItem)this.Items[msg.destItemId];

                        DestroyInventoryItemMessage response = new DestroyInventoryItemMessage();
                        response.InventorySlot = source_jewel.InventorySlot;
                        response.inventoryWindowId = (int)InventoryWindowsID.PlayerInventory;
                        response.ItemID = source_jewel.DynamicID;
                        response.SNO = source_jewel.SNOId;
                        this.Owner.GameClient.SendMessage(response);

                        this.removeItem(source_jewel);

                        break;
                    }
                case Opcodes.InventoryCloseWindowMessage:
                    {
                        InventoryCloseWindowMessage msg = (InventoryCloseWindowMessage)message;

                        if (msg.windowId == InventoryWindowsID.Vault)
                        {
                            Player player = (this.Owner as Player);
                            player.Busy = false;
                            player.IsVaultOpened = false;

                            InventoryShowWindowMessage hide_vault_msg = new InventoryShowWindowMessage();
                            hide_vault_msg.windowId = InventoryWindowsID.Vault;
                            hide_vault_msg.visible = false;
                            player.GameClient.SendMessage(hide_vault_msg);

                            InventoryShowWindowMessage hide_invent_msg = new InventoryShowWindowMessage();
                            hide_invent_msg.windowId = InventoryWindowsID.PlayerInventory;
                            hide_invent_msg.visible = false;
                            player.GameClient.SendMessage(hide_invent_msg);
                        }

                        if (msg.windowId == InventoryWindowsID._SHOP)
                        {
                            Player player = (this.Owner as Player);
                            player.Busy = false;
                            player.IsShopOpened = false;

                            InventoryShowWindowMessage hide_shop_msg = new InventoryShowWindowMessage();
                            hide_shop_msg.windowId = InventoryWindowsID._SHOP;
                            hide_shop_msg.visible = false;
                            player.GameClient.SendMessage(hide_shop_msg);

                            InventoryShowWindowMessage hide_invent_msg = new InventoryShowWindowMessage();
                            hide_invent_msg.windowId = InventoryWindowsID.PlayerInventory;
                            hide_invent_msg.visible = false;
                            player.GameClient.SendMessage(hide_invent_msg);
                        }

                        if (msg.windowId == InventoryWindowsID.PlayerInventory)
                        {
                            Player player = (this.Owner as Player);
                            player.Busy = false;
                            

                            if (player.IsShopOpened)
                            {
                                player.IsShopOpened = false;
                                InventoryShowWindowMessage hide_shop_msg = new InventoryShowWindowMessage();
                                hide_shop_msg.windowId = InventoryWindowsID._SHOP;
                                hide_shop_msg.visible = false;
                                player.GameClient.SendMessage(hide_shop_msg);
                            }

                            if (player.IsVaultOpened)
                            {
                                player.IsVaultOpened = false;
                                InventoryShowWindowMessage hide_shop_msg = new InventoryShowWindowMessage();
                                hide_shop_msg.windowId = InventoryWindowsID.Vault;
                                hide_shop_msg.visible = false;
                                player.GameClient.SendMessage(hide_shop_msg);
                            }

                            InventoryShowWindowMessage hide_invent_msg = new InventoryShowWindowMessage();
                            hide_invent_msg.windowId = InventoryWindowsID.PlayerInventory;
                            hide_invent_msg.visible = false;
                            player.GameClient.SendMessage(hide_invent_msg);
                        }

                        break;
                    }
            }
        }

        /// <summary>
        /// Refreshes the visual appearance of the hero
        /// </summary>
        protected void broadcastVisualEquipment()
        {
            var message = new VisualEquipmentMessage()
                              {
                                  ActorID = this.Owner.DynamicID,
                                  Equipment = this._equipment.GetCurrentVisualEquipment()
                              };

            (this.Owner as Player).GameClient.SendMessage(message);
            //player.World.BroadcastGlobal(message);
        }

        protected void sendCreateInventoryItemMessage(InventoryItem item)
        {
            Player p = this.Owner as Player;
            p.GameClient.SendMessage(new CreateInventoryItemMessage()
            {
                ItemID = item.DynamicID,
                inventoryWindowsId = /*this.Owner.DynamicID*/(int)InventoryWindowsID.PlayerInventory,
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
            invItem.Attributes.BroadcastAllAttributestoPlayer(this.Owner);
            return true;
        }

        /// <summary>
        /// Used for equiping item after game starts
        /// TOOD: Needs rewrite
        /// </summary>
        /// <param name="item"></param>
        /// <param name="slot"></param>


        public void BuyItem(Item originalItem)
        {
            // TODO: Create a copy instead of random.
            //var newItem = ItemGenerator.CreateItem(_owner, originalItem.ItemDefinition);
           // _inventoryGrid.AddItem(newItem);
        }

        /// <summary>
        /// Handles a request to move an item within the inventory.
        /// This covers moving items within the backpack, from equipment
        /// slot to backpack and from backpack to equipment slot
        /// </summary>
        public void OnInventoryRequestMoveMessage(InventoryRequestMoveMessage request)
        {
            InventoryWindowsID sourceWindow = InventoryWindowsID.PlayerInventory;

            InventoryItem inv_item;
            if (!this.Contains(request.ItemID))
            {
                if (this._equipment.ItemsEquiped.ContainsKey(request.ItemID))
                {
                    inv_item = (InventoryItem)this._equipment.ItemsEquiped[request.ItemID];
                }
                else
                {
                    if (this.Vault.Contains(request.ItemID))
                    {
                        sourceWindow = InventoryWindowsID.Vault;
                        inv_item = (InventoryItem)this.Vault.Items[request.ItemID];
                    }
                    else
                    {
                        Logging.LogManager.DefaultLogger.Error("Trying to move an item that is not in inventory or equipment!");
                        return;
                    }
                }
            }
            else
            {
                inv_item = (InventoryItem)this.Items[request.ItemID];
            }

            EquipmentSlotId sourceEquip = inv_item.EquipmentSlot;
            EquipmentSlotId destEquip = (EquipmentSlotId)request.EquipmentSlot;
            if (sourceWindow == InventoryWindowsID.PlayerInventory)
            {
                if ((sourceEquip == EquipmentSlotId.Inventory) && (destEquip == EquipmentSlotId.Inventory))
                {
                    //inventory to inventory
                    if (!this.Contains(inv_item.DynamicID))
                    {
                        Logging.LogManager.DefaultLogger.Error("Trying to move an item that is not in inventory");
                        return;
                    }

                    if (!this.canPutitemThere_checking_self_item(inv_item, request.Row, request.Column))
                    {
                        Logging.LogManager.DefaultLogger.Error("Trying to put an item on a ocupped place (saltea el cliente check?)");
                        return;
                    }

                    if (!this.removeItem(inv_item))
                    {
                        Logging.LogManager.DefaultLogger.Error("could not remove item!, inventory to inventory operation");
                        return;
                    }

                    InventorySlot _slot = new InventorySlot(request.Row, request.Column);
                    if (!this.addItemAtPosition(inv_item, _slot))
                    {
                        Logging.LogManager.DefaultLogger.Error("could not add item at position, from");
                        return;
                    }
                    this.sendAcceptMoveRequest(inv_item, sourceWindow, InventoryWindowsID.PlayerInventory);
                    //this.sendVisualInventory(this._owner);
                }

                if ((sourceEquip == EquipmentSlotId.Inventory) && (destEquip != EquipmentSlotId.Inventory))
                {
                    //inventory to equipment!
                    if (!this.Contains(inv_item.DynamicID))
                    {
                        Logging.LogManager.DefaultLogger.Error("Trying to move an item from inventory to equipment but it is not in the inventory!");
                        return;
                    }

                    if (!this._equipment.IsSlotEmpty(destEquip))
                    {
                        Logging.LogManager.DefaultLogger.Error("Trying to move an item from inventory to equipment but equipment is occupied!");
                        return;
                    }

                    if (!this.removeItem(inv_item))
                    {
                        Logging.LogManager.DefaultLogger.Error("could not remove item!, inventory to equipment operation");
                        return;
                    }

                    this._equipment.EquipItem(inv_item, destEquip);
                    this.sendAcceptMoveRequest(inv_item, sourceWindow, InventoryWindowsID.PlayerInventory);
                    this.broadcastVisualEquipment();
                }

                if ((sourceEquip != EquipmentSlotId.Inventory) && (destEquip == EquipmentSlotId.Inventory))
                {
                    //equipment to inventory!

                    if (!this._equipment.IsItemEquipped(inv_item))
                    {
                        Logging.LogManager.DefaultLogger.Error("Trying to unequip an item (equipment to inventory) that is not in equipment");
                    }

                    if (!this.canPutitemThere_checking_self_item(inv_item, request.Row, request.Column))
                    {
                        Logging.LogManager.DefaultLogger.Error("Trying to put an item on a ocupped place (saltea el cliente check?)");
                        return;
                    }

                    this._equipment.UnequipItem(inv_item);

                    InventorySlot _slot = new InventorySlot(request.Row, request.Column);
                    if (!this.addItemAtPosition(inv_item, _slot))
                    {
                        Logging.LogManager.DefaultLogger.Error("could not add item at position, from");
                        return;
                    }
                    this.sendAcceptMoveRequest(inv_item, sourceWindow, InventoryWindowsID.PlayerInventory);
                    this.broadcastVisualEquipment();
                }

                if ((sourceEquip != EquipmentSlotId.Inventory) && (destEquip != EquipmentSlotId.Inventory))
                {
                    //equipment to equipment!
                }
            }

            if (sourceWindow == InventoryWindowsID.Vault)
            {
                //Vault to inventory
                if (!this.Vault.Contains(inv_item.DynamicID))
                {
                    Logging.LogManager.DefaultLogger.Error("Trying to move an item that is not in Vault!");
                    return;
                }

                if (!this.canPutitemThere_checking_self_item(inv_item, request.Row, request.Column))
                {
                    Logging.LogManager.DefaultLogger.Error("Trying to put an item on a ocupped place (saltea el cliente check?)");
                    return;
                }

                if (!this.Vault.removeItem(inv_item))
                {
                    Logging.LogManager.DefaultLogger.Error("could not remove item!, vault to inventory operation");
                    return;
                }

                InventorySlot _slot = new InventorySlot(request.Row, request.Column);
                if (!this.addItemAtPosition(inv_item, _slot))
                {
                    Logging.LogManager.DefaultLogger.Error("could not add item at position, from");
                    return;
                }
                this.sendAcceptMoveRequest(inv_item, sourceWindow, InventoryWindowsID.PlayerInventory);
            }
        }

        public void sendAcceptMoveRequest(InventoryItem item, InventoryWindowsID source, InventoryWindowsID dest)
        {
            InventoryResponseMoveMessage msg = new InventoryResponseMoveMessage();
            msg.Column = item.InventorySlot.C;
            msg.Row = item.InventorySlot.R;
            msg.originWindowsID = (int)source;
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

        private void OnInventoryDropItemMessage(InventoryDropItemMessage msg)
        {
            InventoryItem inv_item;
            if (!this.Contains(msg.ItemID))
            {
                if (this._equipment.ItemsEquiped.ContainsKey(msg.ItemID))
                {
                    inv_item = (InventoryItem)this._equipment.ItemsEquiped[msg.ItemID];
                }
                else
                {
                    Logging.LogManager.DefaultLogger.Error("Trying to DROP an item that is not in inventory or equipment!");
                    return;
                }
            }
            else
            {
                inv_item = (InventoryItem)this.Items[msg.ItemID];
            }


            if (inv_item.EquipmentSlot == EquipmentSlotId.Inventory)
            {
                //Inventory to ground!, so delete from inventory and drop item!
                if (!this.Contains(inv_item.DynamicID))
                {
                    Logging.LogManager.DefaultLogger.Error("Trying to DROP an item, from inventory to the ground but it is not in the inventory!");
                    return;
                }

                if (!this.removeItem(inv_item))
                {
                    Logging.LogManager.DefaultLogger.Error("could not remove item!, inventory to ground operation");
                    return;
                }

                Map currentPlayerMap = (this.Owner as Player).World;
                Item item = ItemFactory.CreateItem(inv_item.SNOId);
                currentPlayerMap.Enter(item, this.Owner.Position);

                DestroyInventoryItemMessage destroymsg = new DestroyInventoryItemMessage();
                destroymsg.InventorySlot = new InventorySlot(0, 0);
                destroymsg.ItemID = inv_item.DynamicID;
                destroymsg.inventoryWindowId = (int)InventoryWindowsID.PlayerInventory;
                destroymsg.SNO = inv_item.SNOId;
                (this.Owner as Player).GameClient.SendMessage(destroymsg);

                //this.broadcastVisualEquipment();
            }
            else
            {
                //Equipment to ground!, so delete from inventory and drop item!
            }
        }
    }
}