

using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class VisualItem
    {
        public int /* gbid */ snoId;
        public int SlotID;
        public int Field2;
        public int Field3;

        public void Parse(GameBitBuffer buffer)
        {
            snoId = buffer.ReadInt(32);
            SlotID = buffer.ReadInt(5);
            Field2 = buffer.ReadInt(4);
            Field3 = buffer.ReadInt(5) + (-1);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoId);
            buffer.WriteInt(5, SlotID);
            buffer.WriteInt(4, Field2);
            buffer.WriteInt(5, Field3 - (-1));
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("VisualItem:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("snoId: 0x" + snoId.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("SlotID: 0x" + SlotID.ToString("X8") + " (" + SlotID + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}