using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class WeatherOverrideMessage : GameMessage
    {
        public int SNOWorld;
        public float Field1;
        public float Field2;

        public override void Parse(GameBitBuffer buffer)
        {
            SNOWorld = buffer.ReadInt(32);
            Field1 = buffer.ReadFloat32();
            Field2 = buffer.ReadFloat32();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, SNOWorld);
            buffer.WriteFloat32(Field1);
            buffer.WriteFloat32(Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("WeatherOverrideMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("SNOWorld: 0x" + SNOWorld.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: " + Field1.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field2: " + Field2.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}