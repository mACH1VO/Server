

using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class InventoryDropItemMessage : GameMessage
    {
        public int ItemID; // Item's DynamicID

        public InventoryDropItemMessage()
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
            b.AppendLine("InventoryDropItemMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ItemID: " + ItemID.ToString());
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
