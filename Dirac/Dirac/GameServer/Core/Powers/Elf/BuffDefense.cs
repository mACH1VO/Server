using System;
using System.Linq;
using System.Collections.Generic;

using Dirac.Logging;
using Dirac.GameServer.Types;
using Dirac.GameServer;
using Dirac.Extensions;
using Dirac.Math;
using Dirac.GameServer.Network;
using Dirac.GameServer.Network.Message;
using Math = Dirac.Math.Vector3;

namespace Dirac.GameServer.Core
{
    public class BuffDefense : SkillContext
    {
        public BuffDefense()
        {

        }

        public override void Run()
        {
            PlayEffectMessage pem = new PlayEffectMessage()
            {
                ActorId = this.Player.DynamicID,
                EffectOpcode = EffectOpcode.BuffDefense,
                Position = this.Player.Position
            };
            this.Player.World.BroadcastIfRevealed(pem, this.Player); 
        }
    }
}
