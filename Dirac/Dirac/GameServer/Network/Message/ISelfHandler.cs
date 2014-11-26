using Dirac.GameServer.Network;

namespace Dirac.GameServer.Network.Message
{
    public interface ISelfHandler
    {
        void Handle(GameClient client);
    }
}
