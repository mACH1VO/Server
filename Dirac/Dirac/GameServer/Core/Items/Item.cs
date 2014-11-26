using System;
using System.Collections.Generic;
using System.Linq;
using Dirac.Logging;

using Dirac.GameServer;
using Dirac.GameServer.Types;
using Dirac;

using Dirac.Store.FileFormats;
using Dirac.Extensions;
using Dirac.Math;

using Dirac.GameServer.Network.Message;

// TODO: This entire namespace belongs in GS. Bnet only needs a certain representation of items whereas nearly everything here is GS-specific

namespace Dirac.GameServer.Core
{
    public class Item : Actor
    {
        public bool ItemHasChanges { get; set; }

        public override ActorType ActorType { get { return ActorType.Item; } }

        public Actor Owner { get; set; } 

        public ItemType ItemType { get; set; }

        public String Name { get; set; }

        public ItemData Data { get; set; }

        public String FileData { get; set; }

        public ItemState CurrentState { get; set; }


        // There are 2 VisualItemClasses... any way to use the builder to create a D3 Message?
        

        /*public bool IsStackable()
        {
            return ItemDefinition.MaxStackAmount > 1;
        }*/


        public Item(Int32 SNOId)
            : base(SNOId)
        {
            this.FileData = Actor.SNOToFile[SNOId];
            this.Data = new ItemData();
            this.Data = ItemData.XMLDeserialize(this.FileData + ".xml");

            /*this.InventorySize = this.Data.InventorySize;
            this.InventorySlot = new InventorySlot(); //hmmm nose si ponerle -1 -1 o que...
            this.Name = this.Data.Name;*/

            this.CurrentState = ItemState.Normal;
        }


        public override void OnTargeted(Player player, TargetMessage message)
        {
            Logging.LogManager.DefaultLogger.Trace("OnTargeted Item ID: {0}", message.TargetID);

            if (player.Inventory.TryPickUp(this))
            {
                World.Leave(this);
            }
        }

        public override bool Reveal(Player player)
        {
            if (!base.Reveal(player))
                return false;
            //this.Attributes.BroadcastAllAttributesIfRevealed(); //ya se hace antes de createactor adentro de base.reveal
            return true;
        }

        public override bool Unreveal(Player player)
        {
            return base.Unreveal(player);
        }
    }

    public enum ItemState
    {
        Normal,
        PickingUp,
        Dropping
    }

    public enum ItemType
    {
        Unknown,
        Helm,
        Armor,
        Belt,
        Pants,
        Gloves,
        Boots,
        Shield,
        Quiver,
        Orb,
        Axe_1H,
        Axe_2H,
        CombatStaff_2H,
        Staff,
        Scepter,
        Dagger,
        Mace_1H,
        Mace_2H,
        Sword_1H,
        Sword_2H,
        HandXbow,
        Crossbow,
        Bow,
        Spear,
        Wand,
        Gold,
        Ring,
        Amulet,
        Wing,
        Jewel
    }

    /*public enum ItemType
    {
        Unknown, Helm, Gloves, Boots, Belt, Shoulders, Pants, Bracers, Shield, Quiver, Orb,
        Axe_1H, Axe_2H, CombatStaff_2H, Staff, Dagger, Mace_1H, Mace_2H, Sword_1H, HandXbow,
        Mojo, CeremonialDagger_1H, MightyWeapon1H, MightyWeapon2H, WizardHat, VodooMask, Cloak,
        Sword_2H, Crossbow, Bow, Spear, Polearm, Wand, Ring, FistWeapon_1H, ChestArmor, BarbBelt,
        Dye, Gold1, Gold2, Gold3, Gold4, Gold5,
        Amethyst, Emarald, Ruby, Topaz, Skull, Backpack, Potion, Amulet,
        //CraftingReagent
        // Not working at the moment:
        // ThrownWeapon, ThrowingAxe - does not work because there are no snoId in Actors.txt. Do they actually drop in the D3 beta? /angerwin?
        // Diamond, Sapphire - I realised some days ago, that the Item type Diamond and Shappire (maybe not the only one) causes client crash and BAD GBID messages, although they actually have SNO IDs. /angerwin
    }*/

    public enum ItemQuality : int
    {
        Inferior = 0,
        Normal = 1,
        Superior = 2,
        Magic = 4,
        Rare1 = 6,
        Rare2 = 7,
        Rare3 = 8,
        Set = 9,
    }
}

