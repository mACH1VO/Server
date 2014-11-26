

using System.Text;
using Dirac.Logging;
using Dirac;

using Dirac.GameServer.Network;

namespace Dirac.GameServer.Network.Message
{
    public class LogoutComplete : GameMessage,ISelfHandler
    {
        public void Handle(GameClient client)
        {
            if (client.IsLoggingOut)
            {
                client.SendMessage(new QuitGameMessage() // should be sent to all players i guess.
                {
                   // PlayerIndex = client.Player.PlayerIndex,
                });

            }
        }

        public override void Parse(GameBitBuffer buffer)
        {
            
        }

        public override void Encode(GameBitBuffer buffer)
        {
            
        }

        public override void AsText(StringBuilder b, int pad)
        {
            
        }
    }
}
