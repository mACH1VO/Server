using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class AssignStatMessage : GameMessage
    {
        public int StatType;

        public AssignStatMessage()
            : base(Opcodes.AssignStatMessage)
        {
            this.Consumer = Consumers.Player;
        }
        public override void Parse(GameBitBuffer buffer)
        {
            StatType = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, StatType);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AssignStatMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("StatType: " + StatType.ToString());
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
