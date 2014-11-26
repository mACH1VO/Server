using System.Text;

using Dirac.Math;
using Dirac.Extensions;


namespace Dirac.GameServer.Network.Message
{
    /// <summary>
    /// Sent to the client after a new actor is introduced with ACDEnterKnownMessage.
    /// This message adds the actor to the client's world.
    /// </summary>
    public class ACDEnterMapMessage : GameMessage
    {
        public int ActorId; // Actor's DynamicID
        public float Scale;
        public Vector3 Position;
        public Quaternion Orientation;
        public int MapId;

        public ACDEnterMapMessage()
            : base(Opcodes.ACDEnterMapMessage)
        { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorId = buffer.ReadInt(32);
            Scale = buffer.ReadFloat32();
            Position = Position.Parse(buffer);
            Orientation = Orientation.Parse(buffer);
            MapId = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ActorId);
            buffer.WriteFloat32(Scale);
            Position.Encode(buffer);
            Orientation.Encode(buffer);
            buffer.WriteInt(32, MapId);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDCreateActorMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorId.ToString("X8") + " (" + ActorId + ")");
            b.Append(' ', pad);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Scale: " + Scale.ToString("G"));
            Position.AsText(b, pad);
            Orientation.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("WorldID: 0x" + MapId.ToString("X8") + " (" + MapId + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}