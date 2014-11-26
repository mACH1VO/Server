using System;
using System.Linq;
using System.Collections.Generic;
using Dirac.Logging;
using Dirac.GameServer.Types;
using Dirac.GameServer;
using Dirac.Extensions;
using Dirac.Math;

namespace Dirac.GameServer.Core
{
    public class Hand : SkillContext
    {
        public Hand()
        {
        }

        public override void Run()
        {
            if (this.TargetActor != null)
            {
                this.WeaponDamage(this.TargetActor, DamageType.Physical);
            }
            /*PlayEffectMessage pem = new PlayEffectMessage()
            {
                ActorId = this.Player.DynamicID,
                EffectOpcode = EffectOpcode.BuffDefense,
                Position = this.Player.Position
            };
            this.Player.World.BroadcastIfRevealed(pem, this.Player); */
        }
    }
}
