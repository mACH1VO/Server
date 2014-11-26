using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class GameChangeSceneMessage : GameMessage
    {
        public int /* sno */ NewSceneId;
        public int /* sno */ Field1;

        public GameChangeSceneMessage()
            : base(Opcodes.GameChangeSceneMessage, Consumers.Login) { }
        public override void Parse(GameBitBuffer buffer)
        {
            NewSceneId = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, NewSceneId);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GameChangeSceneMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("NewSceneId: 0x" + NewSceneId.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}