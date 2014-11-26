using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class HeroStateData
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public int PlayerFlags;
        public PlayerSavedData PlayerSavedData;
        public int QuestRewardHistoryEntriesCount;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            PlayerFlags = buffer.ReadInt(30);
            PlayerSavedData = new PlayerSavedData();
            PlayerSavedData.Parse(buffer);
            QuestRewardHistoryEntriesCount = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(30, PlayerFlags);
            PlayerSavedData.Encode(buffer);
            buffer.WriteInt(32, QuestRewardHistoryEntriesCount);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HeroStateData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad);
            b.AppendLine("PlayerFlags: 0x" + PlayerFlags.ToString("X8") + " (" + PlayerFlags + ")");
            PlayerSavedData.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("QuestRewardHistoryEntriesCount: 0x" + QuestRewardHistoryEntriesCount.ToString("X8") + " (" + QuestRewardHistoryEntriesCount + ")");
            b.Append(' ', pad);
            b.AppendLine("tQuestRewardHistory:");
            b.Append(' ', pad);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
