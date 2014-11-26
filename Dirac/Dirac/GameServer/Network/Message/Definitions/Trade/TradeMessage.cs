using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class TradeMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public long Field3;
        public int Field4;
        // MaxLength = 5
        public int[] Field5;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(4);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt64(64);
            Field4 = buffer.ReadInt(32);
            Field5 = new int[5];
            for (int i = 0; i < Field5.Length; i++) Field5[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt64(64, Field3);
            buffer.WriteInt(32, Field4);
            for (int i = 0; i < Field5.Length; i++) buffer.WriteInt(32, Field5[i]);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("TradeMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X16"));
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < Field5.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < Field5.Length; j++, i++) { b.Append("0x" + Field5[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }



        public TradeMessage(Opcodes opcodes)
        {
            // TODO: Complete member initialization
            this.opcodes = opcodes;
        }
    }
}