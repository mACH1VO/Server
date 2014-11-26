using System.Text;
using Dirac.Extensions;
using Dirac.Math;

namespace Dirac.GameServer.Network.Message
{
    public class PlayEffectMessage : GameMessage
    {
        public int ActorId;
        public EffectOpcode EffectOpcode;
        public Vector3 Position;

        public PlayEffectMessage() : base(Opcodes.PlayEffectMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorId = buffer.ReadInt(32);
            EffectOpcode = (EffectOpcode)buffer.ReadInt(7) + (-1);
            if (buffer.ReadBool())
            {
                Position = new Vector3();
                Position = Position.Parse(buffer);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ActorId);
            buffer.WriteInt(7, (int)EffectOpcode - (-1));
            buffer.WriteBool(Position != null);
            if (Position != null)
            {
                Position.Encode(buffer);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayEffectMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorId.ToString("X8") + " (" + ActorId + ")");
            b.Append(' ', pad); b.AppendLine("EffectOpcode: 0x" + ((int)EffectOpcode).ToString("X8") + " (" + EffectOpcode + ")");
            if (Position != null)
            {
                Position.AsText(b, pad);
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }

    public enum EffectOpcode : int
    {
        SimpleArrow = 1,
        IceArrowExplosion = 2,
        Cleave = 3,
        ElectricShield = 4,
        BuffDefense = 5,
        IceArmor = 6,
        BuffDMG = 7,
        EvilSpirit = 8,
        Inner = 9,
        Twisting = 10,
    }
}
