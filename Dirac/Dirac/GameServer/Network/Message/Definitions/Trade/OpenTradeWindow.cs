using System.Text;

namespace Dirac.GameServer.Network.Message
{
    /// <summary>
    /// Show a trade window. The inventory 0x14 (20) for an actor is
    /// shown as trade offerings 
    /// </summary>
    public class OpenTradeWindowMessage : GameMessage
    {
        public int ActorID;

        public OpenTradeWindowMessage() 
        {
            this.opcodes = Opcodes.OpenTradeWindow;
            this.Id = (int)Opcodes.OpenTradeWindow;
        }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorID = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ActorID);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("OpenTradeWindowMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorID.ToString("X8") + " (" + ActorID + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

    }
}