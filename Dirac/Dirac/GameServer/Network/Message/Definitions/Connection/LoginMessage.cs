using System;
using System.Text;
using Dirac.Logging;
using Dirac;


namespace Dirac.GameServer.Network.Message
{
    public class LoginMessage : GameMessage
    {
        public String Account;
        public String Password;

        public LoginMessage()
            : base(Opcodes.LoginMessage, Consumers.Login) { }

        public override void Parse(GameBitBuffer buffer)
        {
            Account = buffer.ReadCharArray(32);
            Password = buffer.ReadCharArray(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteCharArray(32, Account);
            buffer.WriteCharArray(32, Password);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("LoginMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Account: \"" + Account + "\"");
            b.Append(' ', pad); b.AppendLine("Password: \"" + Account + "\"");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
