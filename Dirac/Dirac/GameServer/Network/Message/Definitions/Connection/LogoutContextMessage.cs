using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class LogoutContextMessage : GameMessage,ISelfHandler
    {
        public bool Field0;

        public void Handle(GameClient client)
        {
            client.IsLoggingOut = !client.IsLoggingOut;
        }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("LogoutContextMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + (Field0 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

        public LogoutContextMessage(Opcodes opcodes)
        {
            //................
            this.opcodes = opcodes;
        }
    }
}
