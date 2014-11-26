using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class DisplayGameTextMessage : GameMessage
    {
        public string Field0;
        public int? Field1;
        public int? Field2;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadCharArray(512);
            if (buffer.ReadBool())
            {
                Field1 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field2 = buffer.ReadInt(32);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteCharArray(512, Field0);
            buffer.WriteBool(Field1.HasValue);
            if (Field1.HasValue)
            {
                buffer.WriteInt(32, Field1.Value);
            }
            buffer.WriteBool(Field2.HasValue);
            if (Field2.HasValue)
            {
                buffer.WriteInt(32, Field2.Value);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("DisplayGameTextMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: \"" + Field0 + "\"");
            if (Field1.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field1.Value: 0x" + Field1.Value.ToString("X8") + " (" + Field1.Value + ")");
            }
            if (Field2.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field2.Value: 0x" + Field2.Value.ToString("X8") + " (" + Field2.Value + ")");
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}