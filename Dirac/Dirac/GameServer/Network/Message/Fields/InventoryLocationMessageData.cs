

using System.Text;
using Dirac.Extensions;

using Dirac.GameServer.Core;

namespace Dirac.GameServer.Network.Message
{
    public class InventoryLocationMessageData
    {
        public int OwnerID; // Player's DynamicID
        public int EquipmentSlot;
        public InventorySlot InventoryLocation; // Row, column

        public void Parse(GameBitBuffer buffer)
        {
            OwnerID = buffer.ReadInt(32);
            EquipmentSlot = buffer.ReadInt(5) + (-1);
            InventoryLocation = new InventorySlot();
            InventoryLocation.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, OwnerID);
            buffer.WriteInt(5, EquipmentSlot - (-1));
            InventoryLocation.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InventoryLocationMessageData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("OwnerID: 0x" + OwnerID.ToString("X8") + " (" + OwnerID + ")");
            b.Append(' ', pad);
            b.AppendLine("EquipmentSlot: 0x" + EquipmentSlot.ToString("X8") + " (" + EquipmentSlot + ")");
            InventoryLocation.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
