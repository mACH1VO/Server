using System.Text;

namespace Dirac.GameServer.Network.Message
{
    //Opcodes.InventoryRequestUseMessage, Consumers.Inventory)]
    public class InventoryRequestUseMessage : GameMessage
    {
        public uint UsedItem;
        public int Field1;
        public uint UsedOnItem;
        public WorldPlace Location;

        public override void Parse(GameBitBuffer buffer)
        {
            UsedItem = buffer.ReadUInt(32);
            Field1 = buffer.ReadInt(3) + (-1);
            UsedOnItem = buffer.ReadUInt(32);
            Location = new WorldPlace();
            Location.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, UsedItem);
            buffer.WriteInt(3, Field1 - (-1));
            buffer.WriteUInt(32, UsedOnItem);
            Location.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InventoryRequestUseMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("UsedItem: 0x" + UsedItem.ToString("X8") + " (" + UsedItem + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("UsedOnItem: 0x" + UsedOnItem.ToString("X8") + " (" + UsedOnItem + ")");
            Location.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}