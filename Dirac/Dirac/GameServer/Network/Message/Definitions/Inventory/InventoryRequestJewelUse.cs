

using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class InventoryRequestJewelUse : GameMessage
    {
        public int sourceJewelId; // Item's DynamicID
        public int destItemId;

        public InventoryRequestJewelUse()
            : base(Opcodes.InventoryRequestJewelUse)
        {
            this.Consumer = Consumers.Inventory;
        }

        public override void Parse(GameBitBuffer buffer)
        {
            sourceJewelId = buffer.ReadInt(32);
            destItemId = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, sourceJewelId);
            buffer.WriteInt(32, destItemId);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InventoryRequestJewelUse:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("sourceJewelId: " + sourceJewelId.ToString());
            b.Append(' ', pad); b.AppendLine("destItemId: " + destItemId.ToString());
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
