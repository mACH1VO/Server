

using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class GameTickMessage : GameMessage
    {
        public int Tick;

        public GameTickMessage() : base(Opcodes.GameTickMessage) { }

        public GameTickMessage(int tick)
            : base(Opcodes.GameTickMessage)
        {
            this.Tick = tick;
        }

        public override void Parse(GameBitBuffer buffer)
        {
            Tick = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Tick);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GameTickMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Tick: 0x" + Tick.ToString("X8") + " (" + Tick + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
