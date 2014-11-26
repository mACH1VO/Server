using System.Text;
using Dirac.Extensions;
using Dirac.Math;

namespace Dirac.GameServer.Network.Message
{
    public class PlayerMovementMessage : GameMessage
    {
        public int ActorId;
        public Vector3 Position;   
        public float? Angle;       
        public bool? Field3;        
        public float? Speed;        
        public int? Field5;
        public int? AnimationTag;
        public int? Field7;

        public override void Parse(GameBitBuffer buffer)
        {
            ActorId = buffer.ReadInt(32);
            if (buffer.ReadBool())
            {
                Position = new Vector3();
                Position = Position.Parse(buffer);
            }
            if (buffer.ReadBool())
            {
                Angle = buffer.ReadFloat32();
            }
            if (buffer.ReadBool())
            {
                Field3 = buffer.ReadBool();
            }
            if (buffer.ReadBool())
            {
                Speed = buffer.ReadFloat32();
            }
            if (buffer.ReadBool())
            {
                Field5 = buffer.ReadInt(25);
            }
            if (buffer.ReadBool())
            {
                AnimationTag = buffer.ReadInt(21) + (-1);
            }
            if (buffer.ReadBool())
            {
                Field7 = buffer.ReadInt(32);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ActorId);
            buffer.WriteBool(Position != null);
            if (Position != null)
            {
                Position.Encode(buffer);
            }
            buffer.WriteBool(Angle.HasValue);
            if (Angle.HasValue)
            {
                buffer.WriteFloat32(Angle.Value);
            }
            buffer.WriteBool(Field3.HasValue);
            if (Field3.HasValue)
            {
                buffer.WriteBool(Field3.Value);
            }
            buffer.WriteBool(Speed.HasValue);
            if (Speed.HasValue)
            {
                buffer.WriteFloat32(Speed.Value);
            }
            buffer.WriteBool(Field5.HasValue);
            if (Field5.HasValue)
            {
                buffer.WriteInt(25, Field5.Value);
            }
            buffer.WriteBool(AnimationTag.HasValue);
            if (AnimationTag.HasValue)
            {
                buffer.WriteInt(21, AnimationTag.Value - (-1));
            }
            buffer.WriteBool(Field7.HasValue);
            if (Field7.HasValue)
            {
                buffer.WriteInt(32, Field7.Value);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerMovementMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorId: 0x" + ActorId.ToString("X8"));
            if (Position != null)
            {
                Position.AsText(b, pad);
            }
            if (Angle.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Angle.Value: " + Angle.Value.ToString("G"));
            }
            if (Field3.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field3.Value: " + (Field3.Value ? "true" : "false"));
            }
            if (Speed.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Speed.Value: " + Speed.Value.ToString("G"));
            }
            if (Field5.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field5.Value: 0x" + Field5.Value.ToString("X8") + " (" + Field5.Value + ")");
            }
            if (AnimationTag.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("AnimationTag.Value: 0x" + AnimationTag.Value.ToString("X8") + " (" + AnimationTag.Value + ")");
            }
            if (Field7.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field7.Value: 0x" + Field7.Value.ToString("X8") + " (" + Field7.Value + ")");
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
