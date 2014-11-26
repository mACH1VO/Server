using System;
using System.Text;

namespace Dirac.GameServer.Network.Message
{
    //Opcodes.HeartbeatMessage)]
    public class HeartbeatMessage : GameMessage, ISelfHandler
    {
        public HeartbeatMessage()
            : base(Opcodes.HeartbeatMessage)
        { }

        public void Handle(GameClient client)
        {
            // Removes spam every 15 seconds for no handler
        }
        public override void Parse(GameBitBuffer buffer)
        {
        }

        public override void Encode(GameBitBuffer buffer)
        {
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HeartbeatMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
