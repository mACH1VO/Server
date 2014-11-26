using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class ChatMessage : GameMessage
    {
        public int ActorDynamicID;
        public int Type;
        public int Lengh;
        public string Text;

        public ChatMessage()
            : base(Opcodes.ChatMessage)
        { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorDynamicID = buffer.ReadInt(32);
            Type = buffer.ReadInt(2);
            Lengh = buffer.ReadInt(4) + (-1);
            Text = buffer.ReadCharArray(512);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ActorDynamicID);
            buffer.WriteInt(2, Type);
            buffer.WriteInt(4, Lengh - (-1));
            buffer.WriteCharArray(512, Text);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ChatMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorDynamicID: 0x" + ActorDynamicID.ToString("X8") + " (" + ActorDynamicID + ")");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Type.ToString("X8") + " (" + Type + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Lengh.ToString("X8") + " (" + Lengh + ")");
            b.Append(' ', pad); b.AppendLine("Field2: \"" + Text + "\"");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}