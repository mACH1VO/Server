using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class ConnectionEstablishedMessage : GameMessage
    {
        public int PlayerIndex;
        public int Field1;
        public int SNOPackHash;

        public ConnectionEstablishedMessage():base(Opcodes.ConnectionEstablishedMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            PlayerIndex = buffer.ReadInt(3);
            Field1 = buffer.ReadInt(32);
            SNOPackHash = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, PlayerIndex);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, SNOPackHash);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ConnectionEstablishedMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("PlayerIndex: 0x" + PlayerIndex.ToString("X8") + " (" + PlayerIndex + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + SNOPackHash.ToString("X8") + " (" + SNOPackHash + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }

}