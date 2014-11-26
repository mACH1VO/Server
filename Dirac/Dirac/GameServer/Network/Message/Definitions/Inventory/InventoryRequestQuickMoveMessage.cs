

using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class InventoryRequestQuickMoveMessage : GameMessage
    {
        public int ItemID;
        public InventoryRequestQuickMoveMessage()
        {
            this.Consumer = Consumers.Inventory;
        }

        public override void Parse(GameBitBuffer buffer)
        {
            ItemID = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ItemID);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InventoryRequestQuickMoveMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + ItemID.ToString("X8") + " (" + ItemID + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
