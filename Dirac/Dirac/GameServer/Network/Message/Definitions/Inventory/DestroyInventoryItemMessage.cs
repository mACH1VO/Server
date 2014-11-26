using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Dirac.Logging;
using Dirac.GameServer;
using Dirac.GameServer.Core;

namespace Dirac.GameServer.Network.Message
{
    public class DestroyInventoryItemMessage : GameMessage
    {
        public int ItemID; // Item's DynamicID
        public int inventoryWindowId;
        public InventorySlot InventorySlot;
        public int SNO;

        public DestroyInventoryItemMessage()
            : base(Opcodes.DestroyInventoryItemMessage)
        {

        }

        public override void Parse(GameBitBuffer buffer)
        {
            ItemID = buffer.ReadInt(32);
            inventoryWindowId = buffer.ReadInt(32);
            InventorySlot = new InventorySlot();
            InventorySlot.Parse(buffer);
            SNO = buffer.ReadInt(32);

        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ItemID);
            buffer.WriteInt(32, inventoryWindowId);
            InventorySlot.Encode(buffer);
            buffer.WriteInt(32, SNO);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("DestroyInventoryItemMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ItemID: 0x" + this.ItemID.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("OwnerID: 0x" + this.inventoryWindowId.ToString("X8"));
            InventorySlot.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("SNO: 0x" + this.SNO.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
