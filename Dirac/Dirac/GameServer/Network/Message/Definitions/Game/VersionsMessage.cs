using System.Text;
using Dirac;

namespace Dirac.GameServer.Network.Message
{
    public class VersionsMessage : GameMessage
    {
        public int SNOPackHash;
        public int ProtocolHash;
        public string Version;

        public VersionsMessage(int snoPacketHash):base(Opcodes.VersionsMessage)
        {
            this.SNOPackHash = snoPacketHash;
            this.ProtocolHash = /*VersionInfo.Ingame.ProtocolHash*/10;
            this.Version = /*VersionInfo.Ingame.VersionString*/"10";
        }

        public VersionsMessage():base(Opcodes.VersionsMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            SNOPackHash = buffer.ReadInt(32);
            ProtocolHash = buffer.ReadInt(32);
            Version = buffer.ReadCharArray(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, SNOPackHash);
            buffer.WriteInt(32, ProtocolHash);
            buffer.WriteCharArray(32, Version);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("VersionsMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("SNOPackHash: 0x" + SNOPackHash.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("ProtocolHash: 0x" + ProtocolHash.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Version: \"" + Version + "\"");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}