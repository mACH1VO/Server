using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class WorldStatusMessage : GameMessage
    {
        public uint WorldID; // World's DynamicID
        public bool Field1;

        public override void Parse(GameBitBuffer buffer)
        {
            WorldID = buffer.ReadUInt(32);
            Field1 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, WorldID);
            buffer.WriteBool(Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("WorldStatusMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("WorldID: 0x" + WorldID.ToString("X8") + " (" + WorldID + ")");
            b.Append(' ', pad); b.AppendLine("Field1: " + (Field1 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
