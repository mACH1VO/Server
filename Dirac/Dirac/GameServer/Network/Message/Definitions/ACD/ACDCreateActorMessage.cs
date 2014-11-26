

using System.Text;
using System;

using Dirac.GameServer;
using Dirac.GameServer.Core;

namespace Dirac.GameServer.Network.Message
{
    [Serializable]
    public class ACDCreateActorMessage : GameMessage
    {
        public int DynamicID; // The actor's DynamicID
        public int ActorSNOId;
        public ActorType ActorType;
        public int Field2;
        public int Field3; 
        //public WorldLocationMessageData WorldLocation;
        //public InventoryLocationMessageData InventoryLocation;

        public ACDCreateActorMessage() : base(Opcodes.ACDCreateActorMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            DynamicID = buffer.ReadInt(32);
            ActorSNOId = buffer.ReadInt(32);
            ActorType = (ActorType)buffer.ReadInt(32);
            Field2 = buffer.ReadInt(6);
            Field3 = buffer.ReadInt(2) + (-1);
            /*if (buffer.ReadBool())
            {
                WorldLocation = new WorldLocationMessageData();
                WorldLocation.Parse(buffer);
            }
            Field7 = buffer.ReadInt(32);
            NameSNOId = buffer.ReadInt(32);
            Quality = buffer.ReadInt(4) + (-1);
            Field10 = (byte)buffer.ReadInt(8);
            if (buffer.ReadBool())
            {
                Field11 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                MarkerSetSNO = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                MarkerSetIndex = buffer.ReadInt(32);
            }*/
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, DynamicID);
            buffer.WriteInt(32, ActorSNOId);
            buffer.WriteInt(32, (int)ActorType);
            buffer.WriteInt(6, Field2);
            buffer.WriteInt(2, Field3 - (-1));
            /*buffer.WriteBool(WorldLocation != null);
            if (WorldLocation != null)
            {
                WorldLocation.Encode(buffer);
            }
            buffer.WriteBool(InventoryLocation != null);
            if (InventoryLocation != null)
            {
                InventoryLocation.Encode(buffer);
            }
            buffer.WriteInt(32, Field7);
            buffer.WriteInt(32, NameSNOId);
            buffer.WriteInt(4, Quality - (-1));
            buffer.WriteInt(8, Field10);
            buffer.WriteBool(Field11.HasValue);
            if (Field11.HasValue)
            {
                buffer.WriteInt(32, Field11.Value);
            }
            buffer.WriteBool(MarkerSetSNO.HasValue);
            if (MarkerSetSNO.HasValue)
            {
                buffer.WriteInt(32, MarkerSetSNO.Value);
            }
            buffer.WriteBool(MarkerSetIndex.HasValue);
            if (MarkerSetIndex.HasValue)
            {
                buffer.WriteInt(32, MarkerSetIndex.Value);
            }*/
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDCreateActorMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + DynamicID.ToString("X8") + " (" + DynamicID + ")");
            b.Append(' ', pad); b.AppendLine("ActorSNOId: 0x" + ActorSNOId.ToString("X8") +  "  (" + ActorSNOId + ")");
            b.Append(' ', pad); b.AppendLine("ActorType: 0x" + ActorType.ToString() + "  (" + (int)ActorType + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
