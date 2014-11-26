

using System.Text;

namespace Dirac.GameServer.Network.Message
{
    /// <summary>
    /// Sent by the client, when the player buys an item from a vendor
    /// </summary>
    public class InventoryRequestBuyItemMessage : GameMessage
    {
        public int ItemId;

        public InventoryRequestBuyItemMessage()
            : base(Opcodes.InventoryRequestBuyItemMessage)
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
            b.AppendLine("RequestBuyItemMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ItemActorId: 0x" + ItemId.ToString("X8") + " (" + ItemId + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

    }
}