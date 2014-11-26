using System;
using System.Linq;
using Dirac.Logging;
using System.Collections.Generic;
using Dirac.GameServer.Types;
using Dirac.GameServer;

namespace Dirac.GameServer.Core
{
    public class AttackPayload : Payload
    {
        public TargetList Targets;

        public AttackPayload(SkillContext context)
            : base(context, context.Player)
        {
        }

        public void SetSingleTarget(Actor target)
        {
            this.Targets = new TargetList();
            this.Targets.Actors.Add(target);
        }

        public void AddTarget(Actor target)
        {
            if (this.Target != null)
            {
                this.Targets.Actors.Add(target);
            }
        }


        public override void Apply()
        {
            if (this.Targets == null)
                this.Targets = new TargetList();
            
            // main targets
            foreach (Actor target in this.Targets.Actors)
            {
                // filter null and killed targets
                if (target == null  /*&& target.World.PowerManager.IsDeletingActor(target)*/)
                    continue;

                if (target.World == null)
                    continue;

                // TODO: calculate hit chance for monsters instead of always hitting
                bool docrit = _doCriticalHit(this.Context.Player, target);
                HitPayload hitpayload = new HitPayload(this, true, target);
                //payload.AutomaticHitEffects = this.AutomaticHitEffects;
                //payload.OnDeath = OnDeath;

                //foreach (Func<Buff> buffFactory in _hitBuffs)
                  //  this.Context.AddBuff(target, buffFactory());

                /*if (OnHit != null)
                    OnHit(hitpayload);*/

                hitpayload.Apply();
            }

            //aca hacer un foreach con this.Targets.ExtraActors?
        }

        private static bool _doCriticalHit(Actor user, Actor target)
        {
            //if (target.Attributes[GameAttribute.Ignores_Critical_Hits])
                //return false;
            // TODO: probably will calculate this based on GameAttribute.Crit_Percent_Base + GameAttribute.Crit_Percent_Bonus_Capped
            // right now those attributes aren't set though, so just do calc a generic 5% chance for now
            return RandomHelper.NextDouble() < 0.5;
        }
    }
}
