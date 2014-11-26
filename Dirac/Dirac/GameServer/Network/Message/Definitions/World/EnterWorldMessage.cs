using System.Text;
using Dirac.Extensions;
using Dirac.Math;

namespace Dirac.GameServer.Network.Message
{
    public class EnterWorldMessage : GameMessage
    {
        public Vector3 Position;
        public uint WorldID; // World's DynamicID
        public int /* sno */ WorldSNO;

        public EnterWorldMessage() : base(Opcodes.EnterWorldMessage) {}

        public override void Parse(GameBitBuffer buffer)
        {
            Position = new Vector3();
            Position = Position.Parse(buffer);
            WorldID = buffer.ReadUInt(32);
            WorldSNO = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Position.Encode(buffer);
            buffer.WriteUInt(32, WorldID);
            buffer.WriteInt(32, WorldSNO);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("EnterWorldMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Position.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("WorldID: 0x" + WorldID.ToString("X8") + " (" + WorldID + ")");
            b.Append(' ', pad); b.AppendLine("WorldSNO: 0x" + WorldSNO.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
