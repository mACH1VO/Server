using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class AttributesSetValuesMessage : GameMessage
    {
        public int ActorID; // Actor's DynamicID        
        public NetAttributeKeyValue[] atKeyVals; // MaxLength = 15

        public AttributesSetValuesMessage() : base(Opcodes.AttributesSetValuesMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorID = buffer.ReadInt(32);
            atKeyVals = new NetAttributeKeyValue[buffer.ReadInt(4)];

            for (int i = 0; i < atKeyVals.Length; i++) 
            { 
                atKeyVals[i] = new NetAttributeKeyValue(); 
                atKeyVals[i].Parse(buffer); 
            }

            for (int i = 0; i < atKeyVals.Length; i++) 
            { 
                atKeyVals[i].ParseValue(buffer); 
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ActorID);
            buffer.WriteInt(4, atKeyVals.Length);

            for (int i = 0; i < atKeyVals.Length; i++) 
            { 
                atKeyVals[i].Encode(buffer); 
            }

            for (int i = 0; i < atKeyVals.Length; i++) 
            { 
                atKeyVals[i].EncodeValue(buffer); 
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AttributesSetValuesMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorID.ToString("X8") + " (" + ActorID + ")");
            b.Append(' ', pad); b.AppendLine("atKeyVals:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < atKeyVals.Length; i++) { atKeyVals[i].AsText(b, pad + 1); b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
