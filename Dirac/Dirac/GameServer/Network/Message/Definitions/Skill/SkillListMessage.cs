using System.Text;
using Dirac.GameServer.Core;

namespace Dirac.GameServer.Network.Message
{
    public class SkillListMessage : GameMessage
    {
        public SkillOpcode Primary;
        public SkillOpcode Secondary;

        public SkillOpcode _one;
        public SkillOpcode _two;
        public SkillOpcode _three;
        public SkillOpcode _four;

        public SkillListMessage()
            : base(Opcodes.SkillListMessage)
        {
            this.Consumer = Consumers.Player; 
        }
        public override void Parse(GameBitBuffer buffer)
        {
            //no need to parse this anyway, no client->server skilllistmsg is sended
            Primary = (SkillOpcode)buffer.ReadInt(32);
            Secondary = (SkillOpcode)buffer.ReadInt(32);

            _one = (SkillOpcode)buffer.ReadInt(32);
            _two = (SkillOpcode)buffer.ReadInt(32);
            _three = (SkillOpcode)buffer.ReadInt(32);
            _four = (SkillOpcode)buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, (int)Primary);
            buffer.WriteInt(32, (int)Secondary);

            buffer.WriteInt(32, (int)_one);
            buffer.WriteInt(32, (int)_two);
            buffer.WriteInt(32, (int)_three);
            buffer.WriteInt(32, (int)_four);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SkillListMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");

            b.Append(' ', pad); b.AppendLine("Primary: " + Primary.ToString());
            b.Append(' ', pad); b.AppendLine("Secondary: " + Secondary.ToString());

            b.Append(' ', pad); b.AppendLine("_one: " + _one.ToString());
            b.Append(' ', pad); b.AppendLine("_two: " + _two.ToString());
            b.Append(' ', pad); b.AppendLine("_three: " + _three.ToString());
            b.Append(' ', pad); b.AppendLine("_four: " + _four.ToString());

            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
