

using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class ACDChangeActorVisibility : GameMessage
    {
        public int ActorID; 
        public bool Visible;

        public ACDChangeActorVisibility() : base(Opcodes.ACDChangeActorVisibility) { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorID = buffer.ReadInt(32);
            Visible = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ActorID);
            buffer.WriteBool(Visible);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDGroupMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorID.ToString("X8") + " (" + ActorID + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Visible + " (" + Visible + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
