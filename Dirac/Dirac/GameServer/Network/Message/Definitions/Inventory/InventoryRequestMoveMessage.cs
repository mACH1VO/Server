

using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class InventoryRequestMoveMessage : GameMessage
    {
        public int ItemID; // Item's DynamicID
        //public InvLoc Location;

        public int inventoryWindowId;
        public int EquipmentSlot;
        public int Column;
        public int Row;

        public InventoryRequestMoveMessage()
            : base(Opcodes.InventoryRequestMoveMessage)
        { }

        public override void Parse(GameBitBuffer buffer)
        {
            ItemID = buffer.ReadInt(32);
            inventoryWindowId = buffer.ReadInt(32);
            EquipmentSlot = buffer.ReadInt(5) + (-1);
            Column = buffer.ReadInt(32);
            Row = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ItemID);
            buffer.WriteInt(32, inventoryWindowId);
            buffer.WriteInt(5, EquipmentSlot - (-1));
            buffer.WriteInt(32, Column);
            buffer.WriteInt(32, Row);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InventoryRequestMoveMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ItemID: 0x" + ItemID.ToString("X8") + " (" + ItemID + ")");
            b.Append(' ', pad);
            b.AppendLine("InvLoc:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("inventoryWindowId: 0x" + inventoryWindowId.ToString("X8") + " (" + inventoryWindowId + ")");
            b.Append(' ', pad);
            b.AppendLine("EquipmentSlot: 0x" + EquipmentSlot.ToString("X8") + " (" + EquipmentSlot + ")");
            b.Append(' ', pad);
            b.AppendLine("Column: 0x" + Column.ToString("X8") + " (" + Column + ")");
            b.Append(' ', pad);
            b.AppendLine("Row: 0x" + Row.ToString("X8") + " (" + Row + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
