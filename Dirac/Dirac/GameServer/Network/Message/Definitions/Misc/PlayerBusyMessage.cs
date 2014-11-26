using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class PlayerBusyMessage : GameMessage, ISelfHandler
    {
        public bool Busy;

        public override void Parse(GameBitBuffer buffer)
        {
            Busy = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Busy);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerBusyMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Busy: " + (Busy ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

        public void Handle(GameClient client)
        {
            // TODO: PlayerBusyMessage - The status change is sent back to the client,
            // I am waiting for an autosyncing implementation of GameAttributes - farmy
            //client.Player.Attributes[GameAttribute.Busy] = this.Busy;
        }
    }
}
