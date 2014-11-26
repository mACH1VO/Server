
using System;
using System.Linq;
using Dirac.Logging;
using Dirac.GameServer.Types;
using Dirac.GameServer;
using Dirac.Extensions;
using System.Collections.Generic;
using Dirac.Math;

namespace Dirac.GameServer.Core
{
    public class EffectActor : Actor, IUpdateable
    {
        public SkillContext Context;

        public TickTimer Timeout = null;
        public float UpdateDelay = 0f;

        public Action OnUpdate = null;
        public Action OnTimeout = null;

        public override ActorType ActorType { get { return ActorType.ClientEffect; } }

        private TickTimer _updateTimer;

        public EffectActor(SkillContext context, int actorSNO, Vector3 position)
            : base(actorSNO)
        {
            this.Context = context;

            //this.Field2 = 0x8;
            if (this.Scale == 0f)
                this.Scale = 1f;
            this.Position = position;

            // copy in important effect params from user
            /*this.Attributes[GameAttribute.Rune_A, context.PowerSNO] = context.User.Attributes[GameAttribute.Rune_A, context.PowerSNO];
            this.Attributes[GameAttribute.Rune_B, context.PowerSNO] = context.User.Attributes[GameAttribute.Rune_B, context.PowerSNO];
            this.Attributes[GameAttribute.Rune_C, context.PowerSNO] = context.User.Attributes[GameAttribute.Rune_C, context.PowerSNO];
            this.Attributes[GameAttribute.Rune_D, context.PowerSNO] = context.User.Attributes[GameAttribute.Rune_D, context.PowerSNO];
            this.Attributes[GameAttribute.Rune_E, context.PowerSNO] = context.User.Attributes[GameAttribute.Rune_E, context.PowerSNO];*/
        }

        public void Update()
        {
            /*if (Timeout != null && Timeout.TimedOut)
            {
                if (OnTimeout != null)
                    OnTimeout();

                this.Destroy();
            }
            else if (OnUpdate != null)
            {
                if (_updateTimer == null || _updateTimer.TimedOut)
                {
                    OnUpdate();
                    if (this.UpdateDelay > 0f)
                        _updateTimer = new SecondsTickTimer(this.Context.World.Game, this.UpdateDelay);
                    else
                        _updateTimer = null;
                }
            }*/
        }
    }
}
