using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class ToonData
    {
        public int PlayerIndex;
        public int SNO;
        public int Level;
        public string ToonName;
        public VisualEquipment EquipmentList;

        public ToonData() { }

        public void Parse(GameBitBuffer buffer)
        {
            PlayerIndex = buffer.ReadInt(32);
            SNO = buffer.ReadInt(32);
            Level = buffer.ReadInt(32);
            EquipmentList = new VisualEquipment();
            EquipmentList.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, PlayerIndex);
            buffer.WriteInt(32, SNO);
            buffer.WriteInt(32, Level);
            EquipmentList.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ToonData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("PlayerIndex: 0x" + PlayerIndex.ToString("X8") + " (" + PlayerIndex + ")");
            b.Append(' ', pad); b.AppendLine("SNO: 0x" + SNO.ToString("X8") + " (" + SNO + ")");
            b.Append(' ', pad); b.AppendLine("Level: 0x" + Level.ToString("X8") + " (" + Level + ")");
            b.Append(' ', pad); b.AppendLine("ToonName:" + ToonName + " (" + ToonName + ")");
            EquipmentList.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
