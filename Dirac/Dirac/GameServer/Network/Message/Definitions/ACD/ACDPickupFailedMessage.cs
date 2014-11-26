using System.Text;

namespace Dirac.GameServer.Network.Message
{
    //Opcodes.ACDPickupFailedMessage)]
    public class ACDPickupFailedMessage : GameMessage
    {
        public enum Reasons : int
        {
            InventoryFull = 0,                  //and 1, 2, 5, 6, 7  <-- ?
            ItemBelongingToSomeoneElse = 3,
            OnlyOneItemAllowed = 4
        }

        public uint ItemID; // Item's DynamicID
        public Reasons Reason;

        public ACDPickupFailedMessage() : base(Opcodes.ACDPickupFailedMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            ItemID = (uint)buffer.ReadInt(32);
            Reason = (Reasons)buffer.ReadInt(3);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, (int)ItemID);
            buffer.WriteInt(3, (int)Reason);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDPickupFailedMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ItemID: 0x" + ItemID.ToString("X8") + " (" + ItemID + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + ((int)(Reason)).ToString("X8") + " (" + Reason + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
