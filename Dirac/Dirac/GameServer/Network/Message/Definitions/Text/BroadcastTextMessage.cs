using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class BroadcastTextMessage : GameMessage
    {
        public string Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadCharArray(512);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteCharArray(512, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("BroadcastTextMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: \"" + Field0 + "\"");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}