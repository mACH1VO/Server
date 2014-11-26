using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class HeroStateMessage : GameMessage
    {
        public HeroStateData State;

        public HeroStateMessage() : base(Opcodes.HeroStateMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            State = new HeroStateData();
            State.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            State.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HeroStateMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            State.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}