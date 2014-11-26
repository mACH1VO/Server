﻿namespace Dirac.GameServer.Network.Message
 {
     public interface IMessageConsumer
     {
         void Consume(GameClient client, GameMessage message);
     }

    
 }
