using System;
using System.Text;

using Dirac.GameServer.Core;

using Attribute = Dirac.GameServer.Core.Attribute;

namespace Dirac.GameServer.Network.Message
{
    public class NetAttributeKeyValue
    {
        public Attribute Attribute;
        public int Index;
        public int Int;
        public float Float;

        public void Parse(GameBitBuffer buffer)
        {
            Index = buffer.ReadInt(10) & 0xFFF;

            //hijo de puta, como crasheas eh..............
            Attribute = Dirac.GameServer.Core.GameAttributeStaticList.AttributesByID[Index]; 
            //set attribute by index
        }

        public void ParseValue(GameBitBuffer buffer)
        {
            switch (Attribute.EncodingType)
            {
                case AttributeEncoding.Int:
                    Int = buffer.ReadInt(Attribute.BitCount);
                    break;
                case AttributeEncoding.IntMinMax:
                    Int = buffer.ReadInt(Attribute.BitCount) + Attribute.Min.Value;
                    break;
                case AttributeEncoding.Float16:
                    Float = buffer.ReadFloat16();
                    break;
                case AttributeEncoding.Float16Or32:
                    Float = buffer.ReadBool() ? buffer.ReadFloat16() : buffer.ReadFloat32();
                    break;
                case AttributeEncoding.Float32:
                    Float = buffer.ReadFloat32();
                    break;
                default:
                    throw new Exception("bad, gg");
            }
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(10, Attribute.Id);
        }

        public void EncodeValue(GameBitBuffer buffer)
        {
            switch (Attribute.EncodingType)
            {
                case AttributeEncoding.Int:
                    buffer.WriteInt(Attribute.BitCount, Int);
                    break;
                case AttributeEncoding.IntMinMax:
                    buffer.WriteInt(Attribute.BitCount, Int - Attribute.Min.Value);
                    break;
                case AttributeEncoding.Float16:
                    buffer.WriteFloat16(Float);
                    break;
                case AttributeEncoding.Float16Or32:
                    if (Float >= 65536.0f || -65536.0f >= Float)
                    {
                        buffer.WriteBool(false);
                        buffer.WriteFloat32(Float);
                    }
                    else
                    {
                        buffer.WriteBool(true);
                        buffer.WriteFloat16(Float);
                    }
                    break;
                case AttributeEncoding.Float32:
                    buffer.WriteFloat32(Float);
                    break;
                default:
                    throw new Exception("bad, gg");
            }
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("NetAttributeKeyValue:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.Append(Attribute.Name);
            b.Append(" (" + Attribute.Id + "): ");

            if (Attribute.IsInteger)
                b.AppendLine("0x" + Int.ToString("X8") + " (" + Int + ")" + " I");
            else
                b.AppendLine(Float.ToString("G") + " f");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}