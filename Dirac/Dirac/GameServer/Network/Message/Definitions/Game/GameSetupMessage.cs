using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class GameSetupMessage : GameMessage
    {
        public float Field0;

        public GameSetupMessage():base(Opcodes.GameSetupMessage) {}

        public override void Parse(GameBitBuffer buffer)
        {
            //Field0 = buffer.ReadInt(32);
            Field0 = buffer.ReadFloat32();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            //buffer.WriteInt(32, Field0);
            buffer.WriteFloat32(Field0);
        }


        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GameSetupMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }

}
