using System.Text;
using Dirac.Extensions;
using Dirac.Math;

namespace Dirac.GameServer.Network.Message
{
    public class ACDTranslateNormalMessage : GameMessage
    {
        public int ActorId;
        public Vector3 Position;
        public float? Angle;        
        public bool? TurnImmediately;        
        public float? Speed;        
        public int? Field5;
        public int? AnimationTag;   
        public int? Field7;
        public int? Field8;
        public int? Field9;

        public ACDTranslateNormalMessage() : base(Opcodes.ACDTranslateNormalMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorId = buffer.ReadInt(32);
            if (buffer.ReadBool())
            {
                Position = new Vector3();
                Position.Parse(buffer);
            }
            if (buffer.ReadBool())
            {
                Angle = buffer.ReadFloat32();
            }
            if (buffer.ReadBool())
            {
                TurnImmediately = buffer.ReadBool();
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
            if (buffer.ReadBool())
            {
                Field8 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field9 = buffer.ReadInt(16);
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
            buffer.WriteBool(TurnImmediately.HasValue);
            if (TurnImmediately.HasValue)
            {
                buffer.WriteBool(TurnImmediately.Value);
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
            buffer.WriteBool(Field8.HasValue);
            if (Field8.HasValue)
            {
                buffer.WriteInt(32, Field8.Value);
            }
            buffer.WriteBool(Field9.HasValue);
            if (Field9.HasValue)
            {
                buffer.WriteInt(16, Field9.Value);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateNormalMessage:");
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
            if (TurnImmediately.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field3.Value: " + (TurnImmediately.Value ? "true" : "false"));
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
            if (Field8.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field8.Value: 0x" + Field8.Value.ToString("X8") + " (" + Field8.Value + ")");
            }
            if (Field9.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field9.Value: 0x" + Field9.Value.ToString("X8") + " (" + Field9.Value + ")");
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
