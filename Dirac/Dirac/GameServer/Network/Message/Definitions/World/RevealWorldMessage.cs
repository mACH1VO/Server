using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class RevealWorldMessage : GameMessage
    {
        public uint WorldID; // World's DynamicID
        public int /* sno */ WorldSNO;
        public int Field2;
        public int Field3;
        public int Field4;
        public int Field5;
        public int Field6;
        public int Field7;

        public RevealWorldMessage() : base(Opcodes.RevealWorldMessage) {}

        public override void Parse(GameBitBuffer buffer)
        {
            WorldID = buffer.ReadUInt(32);
            WorldSNO = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(32);
            Field7 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, WorldID);
            buffer.WriteInt(32, WorldSNO);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(32, Field5);
            buffer.WriteInt(32, Field6);
            buffer.WriteInt(32, Field7);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RevealWorldMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("WorldID: 0x" + WorldID.ToString("X8") + " (" + WorldID + ")");
            b.Append(' ', pad); b.AppendLine("WorldSNO: 0x" + WorldSNO.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad); b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
