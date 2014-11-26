using System.Text;
using Dirac.Extensions;
using Dirac.Math;

namespace Dirac.GameServer.Network.Message
{
    public class WorldPlace
    {
        public Vector3 Position;
        public uint WorldID; // World's DynamicID

        public void Parse(GameBitBuffer buffer)
        {
            Position = new Vector3();
            Position = Position.Parse(buffer);
            WorldID = buffer.ReadUInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            Position.Encode(buffer);
            buffer.WriteUInt(32, WorldID);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("WorldPlace:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Position.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("WorldID: 0x" + WorldID.ToString("X8") + " (" + WorldID + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
