using System.Text;

namespace Dirac.GameServer.Network.Message
{
    public class PlayerSavedData
    {
        // MaxLength = 6
        public HotbarButtonData[] HotBarButtons;

        public void Parse(GameBitBuffer buffer)
        {
            HotBarButtons = new HotbarButtonData[6];
            for (int i = 0; i < HotBarButtons.Length; i++)
            {
                HotBarButtons[i] = new HotbarButtonData();
                HotBarButtons[i].Parse(buffer);
            }
        }

        public void Encode(GameBitBuffer buffer)
        {
            for (int i = 0; i < HotBarButtons.Length; i++)
            {
                HotBarButtons[i].Encode(buffer);
            }
        }

        public void AsText(StringBuilder b, int pad)
        {
            for (int i = 0; i < HotBarButtons.Length; i++)
            {
                HotBarButtons[i].AsText(b, pad + 1);
                b.AppendLine();
            }
        }


    }
}