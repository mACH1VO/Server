

using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class PlayNonPositionalSoundMessage : GameMessage
    {
        public int /* sno */ Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayNonPositionalSoundMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}