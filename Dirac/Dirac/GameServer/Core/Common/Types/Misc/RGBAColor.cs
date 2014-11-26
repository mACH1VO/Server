using System.Text;
using Gibbed.IO;

using Dirac.GameServer.Network;

namespace Dirac.GameServer.Types
{
    public class RGBAColor
    {
        public byte Red;
        public byte Green;
        public byte Blue;
        public byte Alpha;

        public RGBAColor() { }

        /// <summary>
        /// Parses RGBAColor from given GameBitBuffer, used for Compositor!
        /// </summary>
        /// <param name="buffer">The GameBitBuffer to parse from.</param>
        public void Parse(GameBitBuffer buffer)
        {
            Red = (byte)buffer.ReadInt(8);
            Green = (byte)buffer.ReadInt(8);
            Blue = (byte)buffer.ReadInt(8);
            Alpha = (byte)buffer.ReadInt(8);
        }

        /// <summary>
        /// Encodes RGBAColor to given GameBitBuffer.
        /// </summary>
        /// <param name="buffer">The GameBitBuffer to write.</param>
        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(8, Red);
            buffer.WriteInt(8, Green);
            buffer.WriteInt(8, Blue);
            buffer.WriteInt(8, Alpha);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RGBAColor:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Reg: 0x" + Red.ToString("X2"));
            b.Append(' ', pad);
            b.AppendLine("Green: 0x" + Green.ToString("X2"));
            b.Append(' ', pad);
            b.AppendLine("Blue: 0x" + Blue.ToString("X2"));
            b.Append(' ', pad);
            b.AppendLine("Alpha: 0x" + Alpha.ToString("X2"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}