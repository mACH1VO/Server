using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class ACDLookAtMessage : GameMessage
    {
        public int ActorID;
        public int TargetID;

        public ACDLookAtMessage() : base(Opcodes.ACDLookAtMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorID = buffer.ReadInt(32);
            TargetID = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ActorID);
            buffer.WriteInt(32, TargetID);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDLookAtMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorID.ToString("X8") + " (" + ActorID + ")");
            b.Append(' ', pad); b.AppendLine("TargetID: 0x" + TargetID.ToString("X8") + " (" + TargetID + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}