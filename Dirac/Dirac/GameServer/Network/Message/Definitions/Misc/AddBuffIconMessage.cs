using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class AddBuffIconMessage : GameMessage
    {
        public AddBuffIconMessage() : base(Opcodes.AddBuffIconMessage) { }

        public int slot;
        public int BuffIconOpcode;

        public override void Parse(GameBitBuffer buffer)
        {
            slot = buffer.ReadInt(32);
            BuffIconOpcode = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, slot);
            buffer.WriteInt(32, BuffIconOpcode);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AddBuffIconMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("slot: 0x" + slot.ToString("X8") + " (" + slot + ")");
            b.Append(' ', pad); b.AppendLine("opcode: 0x" + BuffIconOpcode.ToString("X8") + " (" + BuffIconOpcode + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}