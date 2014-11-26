using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class CreateHeroMessage : GameMessage
    {
        public string Name;
        public int /* gbid */ Field1;
        public int Field2;

        public override void Parse(GameBitBuffer buffer)
        {
            Name = buffer.ReadCharArray(49);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(30);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteCharArray(49, Name);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(30, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("CreateHeroMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Name: \"" + Name + "\"");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
