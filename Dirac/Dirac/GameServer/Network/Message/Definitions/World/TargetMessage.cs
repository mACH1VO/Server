using System.Text;
using Dirac.Math;
using Dirac.Extensions;

namespace Dirac.GameServer.Network.Message
{
    public class TargetMessage : GameMessage
    {
        public int Field0;
        public int TargetID; // Targeted actor's DynamicID
        public Vector3 Position;
        public int PowerSlot; //slot
        public int Field4;
        public int Field5;

        public TargetMessage() : base(Opcodes.TargetMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3) + (-1);
            TargetID = buffer.ReadInt(32);
            Position = new Vector3();
            Position = Position.Parse(buffer);
            PowerSlot = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(2);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0 - (-1));
            buffer.WriteInt(32, TargetID);
            Position.Encode(buffer);
            buffer.WriteInt(32, PowerSlot);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(2, Field5);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("TargetMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("TargetID: 0x" + TargetID.ToString("X8") + " (" + TargetID + ")");
            Position.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("PowerSNO: 0x" + PowerSlot.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }

}
