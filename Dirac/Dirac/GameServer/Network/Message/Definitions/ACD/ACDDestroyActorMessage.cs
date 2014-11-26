using System.Text;

namespace Dirac.GameServer.Network.Message
{
    /// <summary>
    /// Sent to the client after a actor dies or gets out of sight.
    /// This message removes the actor from the client's world
    /// </summary>
    public class ACDDestroyActorMessage : GameMessage
    {
        public uint ActorId; // Actor's DynamicID

        public ACDDestroyActorMessage(uint actorId)
            : base(Opcodes.ACDDestroyActorMessage)
        {
            this.ActorId = actorId;
        }

        public ACDDestroyActorMessage()
            : base(Opcodes.ACDDestroyActorMessage)
        { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorId = (uint)buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, (int)ActorId);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDDestroyActorMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorId.ToString("X8") + " (" + ActorId + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}