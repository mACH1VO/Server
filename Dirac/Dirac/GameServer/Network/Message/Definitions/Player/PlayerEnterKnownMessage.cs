using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class PlayerEnterKnownMessage : GameMessage
    {
        public int PlayerIndex;
        public uint ActorId; // Player's DynamicID

        public PlayerEnterKnownMessage() : base(Opcodes.PlayerEnterKnownMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            PlayerIndex = buffer.ReadInt(3);
            ActorId = buffer.ReadUInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, PlayerIndex);
            buffer.WriteUInt(32, ActorId);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerEnterKnownMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("PlayerIndex: 0x" + PlayerIndex.ToString("X8") + " (" + PlayerIndex + ")");
            b.Append(' ', pad); b.AppendLine("ActorId: 0x" + ActorId.ToString("X8") + " (" + ActorId + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
