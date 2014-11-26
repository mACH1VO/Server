

using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class PlayMusicMessage : GameMessage
    {
        public int /* sno */ snoMusic;

        public override void Parse(GameBitBuffer buffer)
        {
            snoMusic = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoMusic);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayMusicMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoMusic: 0x" + snoMusic.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}