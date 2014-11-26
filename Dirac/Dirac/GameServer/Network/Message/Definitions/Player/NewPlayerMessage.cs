using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class NewPlayerMessage : GameMessage
    {
        public int dynamicId;
        public string ToonName;
        public int Field3;
        public int Field4;
        public int /* sno */ snoActorPortrait;
        public int Level;
        //public HeroStateData StateData;
        public bool IsMainPlayer;
        public int Field9;
        public int SNO;
        public int ActorID; // Hero's DynamicID

        public NewPlayerMessage() : base(Opcodes.NewPlayerMessage) {}

        public override void Parse(GameBitBuffer buffer)
        {
            dynamicId = buffer.ReadInt(32);
            ToonName = buffer.ReadCharArray(49);
            Field3 = buffer.ReadInt(5) + (-1);
            Field4 = buffer.ReadInt(3) + (-1);
            snoActorPortrait = buffer.ReadInt(32);
            Level = buffer.ReadInt(7);
            //StateData = new HeroStateData();
            //StateData.Parse(buffer);
            IsMainPlayer = buffer.ReadBool();
            Field9 = buffer.ReadInt(32);
            SNO = buffer.ReadInt(32);
            ActorID = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, dynamicId);
            buffer.WriteCharArray(49, ToonName);
            buffer.WriteInt(5, Field3 - (-1));
            buffer.WriteInt(3, Field4 - (-1));
            buffer.WriteInt(32, snoActorPortrait);
            buffer.WriteInt(7, Level);
            //StateData.Encode(buffer);
            buffer.WriteBool(IsMainPlayer);
            buffer.WriteInt(32, Field9);
            buffer.WriteInt(32, SNO);
            buffer.WriteInt(32, ActorID);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("NewPlayerMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("dynamicId: 0x" + dynamicId.ToString("X8") + " (" + dynamicId + ")");
            b.Append(' ', pad); b.AppendLine("ToonName: \"" + ToonName + "\"");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("snoActorPortrait: 0x" + snoActorPortrait.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Level: 0x" + Level.ToString("X8") + " (" + Level + ")");
            //StateData.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("IsMainPlayer: " + (IsMainPlayer ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field9: 0x" + Field9.ToString("X8") + " (" + Field9 + ")");
            b.Append(' ', pad); b.AppendLine("SNO: 0x" + SNO.ToString("X8") + " (" + SNO + ")");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorID.ToString("X8") + " (" + ActorID + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
