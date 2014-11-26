using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class VisualEquipment
    {
        // MaxLength = 8
        public VisualItem[] Equipment;

        public void Parse(GameBitBuffer buffer)
        {
            Equipment = new VisualItem[8];
            for (int i = 0; i < Equipment.Length; i++)
            {
                Equipment[i] = new VisualItem();
                Equipment[i].Parse(buffer);
            }
        }

        public void Encode(GameBitBuffer buffer)
        {
            for (int i = 0; i < Equipment.Length; i++)
            {
                Equipment[i].Encode(buffer);
            }
        }

        public void AsText(StringBuilder b, int pad)
        {
            
        }


    }
}
