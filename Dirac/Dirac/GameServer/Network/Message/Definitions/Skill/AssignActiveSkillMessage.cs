using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class AssignActiveSkillMessage : GameMessage
    {
        public int /* sno */ SNOSkill;
        public int RuneIndex;
        public int SkillIndex;

        public override void Parse(GameBitBuffer buffer)
        {
            SNOSkill = buffer.ReadInt(32);
            RuneIndex = buffer.ReadInt(3) + (-1);
            SkillIndex = buffer.ReadInt(3);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, SNOSkill);
            buffer.WriteInt(3, RuneIndex - (-1));
            buffer.WriteInt(5, SkillIndex);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AssignActiveSkillMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("SNOSkill: 0x" + SNOSkill.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("RuneIndex: 0x" + RuneIndex.ToString("X8") + " (" + RuneIndex + ")");
            b.Append(' ', pad); b.AppendLine("SkillIndex: 0x" + SkillIndex.ToString("X8") + " (" + SkillIndex + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
