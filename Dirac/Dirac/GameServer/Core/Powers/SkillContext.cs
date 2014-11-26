using System;
using System.Linq;
using Dirac.Logging;
using Dirac.GameServer.Types;
using Dirac.GameServer;
using System.Collections.Generic;
using Dirac.Extensions;
using Dirac.Math;

using Dirac.GameServer.Network;
using Dirac.GameServer.Network.Message;

namespace Dirac.GameServer.Core
{
    public abstract class SkillContext
    {
        public int PowerSNO;

        public Map World;

        public Player Player;

        public Actor TargetActor;

        public Vector3 DestinationUserClick;

        public Vector3 VectorDirector;

        public TargetMessage TargetMessageFromClient;

        public abstract void Run();

        public void WeaponDamage(Actor target, DamageType damageType)
        {
            AttackPayload AttackPayload = new AttackPayload(this);
            AttackPayload.SetSingleTarget(target);
            //payload.AddWeaponDamage(damageMultiplier, damageType);
            AttackPayload.Apply();
        }

        /*public EffectActor SpawnEffect(int actorSNO, Vector3 position, float angle = 0, TickTimer timeout = null)
        {
            if (angle == -1)
                angle = (float)(RandomHelper.NextDouble() * (System.Math.PI * 2));
            
            if (timeout == null)
            {
            }

            var actor = new EffectActor(this, actorSNO, position);
            actor.Timeout = timeout;
            //actor.Spawn(angle);
            return actor;
        }*/


        /*public TargetList GetEnemiesInRadius(Vector3D center, float radius, int maxCount = -1)
        {
            return _getTargetsInRadiusHelper(center, radius, maxCount);
        }*/

        /*private TargetList _getTargetsInRadiusHelper(Vector3D center, float radius, int maxCount)
        {
            // Query() needs to gather using circle-circle collision, until then just extend the search radius by the default
            // actor radius currently used.
            float actorRadiusCompensation = 1.5f;

            TargetList targets = new TargetList();
            List<Actor> actors = World.QuadTree.Query<Actor>(new Circle(center.X, center.Y, radius + actorRadiusCompensation));
            foreach (Actor actor in actors)
            {
                if ((actor.ActorType == ActorType.Monster) && !actor.Attributes[GameAttributeStaticList.Untargetable] 
                    && actor != Player && actor.Attributes[GameAttributeStaticList.TeamID] == 10 ) 
                {
                    targets.Actors.Add(actor);
                }
                else
                {
                    targets.ExtraActors.Add(actor);
                }
            }

            return targets;
        }*/
    }
}
