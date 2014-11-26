
using System.Text;
using Dirac.Logging;
using Dirac.Extensions;
using Dirac.Math;

namespace Dirac.GameServer.Network.Message
{
    public class ACDClientTranslateMessage : GameMessage
    {
        public Vector3 Position;

        public ACDClientTranslateMessage()
            :base(Opcodes.ACDClientTranslateMessage)
        {
            this.Consumer = Consumers.Player;
        }

        public override void Parse(GameBitBuffer buffer)
        {
            Position = new Vector3();
            Position = Position.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Position.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDClientTranslateMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Position.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
