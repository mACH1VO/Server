using System.Text;

namespace Dirac.GameServer.Network.Message
{

    public class DeleteBuffIconMessage : GameMessage
    {
        public int slot;

        public DeleteBuffIconMessage(Opcodes id) : base(id) { }
        public DeleteBuffIconMessage() : base(Opcodes.DeleteBuffIconMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            slot = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, slot);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("DeleteBuffIconMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("slot: 0x" + slot.ToString("X8") + " (" + slot + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
