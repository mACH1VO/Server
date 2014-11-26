

using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class UnassignSkillMessage : GameMessage
    {
        public int SkillIndex;

        public override void Parse(GameBitBuffer buffer)
        {
            SkillIndex = buffer.ReadInt(3);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(5, SkillIndex);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("UnassignSkillMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("SkillIndex: 0x" + SkillIndex.ToString("X8") + " (" + SkillIndex + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
