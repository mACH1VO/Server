using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class InventoryRequestSellItemMessage : GameMessage
    {
        public int ItemId;

        public InventoryRequestSellItemMessage()
            : base(Opcodes.InventoryRequestSellItemMessage)
        {
            this.Consumer = Consumers.Inventory;
        }

        public override void Parse(GameBitBuffer buffer)
        {
            ItemId = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ItemId);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SellItemMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ItemId: 0x" + ItemId.ToString("X8") + " (" + ItemId + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

    }
}