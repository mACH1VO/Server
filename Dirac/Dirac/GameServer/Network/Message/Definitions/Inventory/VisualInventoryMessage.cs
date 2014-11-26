using System.Text;

namespace Dirac.GameServer.Network.Message
{
    //Opcodes.VisualInventoryMessage)]
    public class VisualEquipmentMessage : GameMessage
    {
        public int ActorID; // Player's DynamicID
        public VisualItem[] Equipment;

        public VisualEquipmentMessage() : base(Opcodes.VisualEquipmentMessage) {}

        public override void Parse(GameBitBuffer buffer)
        {
            
            ActorID = buffer.ReadInt(32);
            Equipment = new VisualItem[9];
            for (int i = 0; i < Equipment.Length; i++)
            {
                Equipment[i] = new VisualItem();
                Equipment[i].Parse(buffer);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ActorID);
            for (int i = 0; i < Equipment.Length; i++)
            {
                Equipment[i].Encode(buffer);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("VisualEquipmentMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: " + " (" + ActorID + ")");
            b.Append(' ', pad);
            b.AppendLine("VisualEquipment:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < Equipment.Length; i++)
            {
                Equipment[i].AsText(b, pad + 1);
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
