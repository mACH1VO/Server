using System;
using System.Linq;
using Dirac.Logging;
using Dirac.GameServer;

namespace Dirac.GameServer.Core
{
    public class DeathPayload : Payload
    {
        public DamageType DeathDamageType;
        public bool LootAndExp; //HACK: As we currently just give out random exp and loot, this is in to prevent giving this out for mobs that shouldn't give it.

        public DeathPayload(SkillContext context, DamageType deathDamageType, Actor target, bool grantsLootAndExp = true)
            : base(context, target)
        {
            this.LootAndExp = grantsLootAndExp;
            this.DeathDamageType = deathDamageType;
        }

        public override void Apply()
        {
            if (this.Target.World == null) return;

            if (this.Target is Player)
            {
                _DoSimplePlayerDeath();
                return;
            }
            
            // HACK: add to hackish list thats used to defer deleting actor and filter it from powers targetting
            //this.Target.World.PowerManager.AddDeletingActor(this.Target);

            // kill brain if living
            /*if (this.Target is Living)
            {
                Living actor = (Living)this.Target;
                if (actor.Brain != null)
                    actor.Brain.Kill();
            }*/

            // kill brain if monster
            /*if (this.Target is Monster)
            {
                Monster mon = (Monster)this.Target;
                if (mon.Brain != null)
                    mon.Brain.Kill();
            }*/

            // send this death payload to buffs
            //this.Target.World.BuffManager.SendTargetPayload(this.Target, this);

            // wtf is this?
            /*this.Target.World.BroadcastIfRevealed(new PlayEffectMessage()
            {
                ActorId = this.Target.DynamicID,
                Effect = Effect.Unknown12,
            }, this.Target);

            this.Target.World.BroadcastIfRevealed(new ANNDataMessage(Opcodes.ANNDataMessage13)
            {
                ActorID = this.Target.DynamicID
            }, this.Target);

            // play main death animation
            this.Target.PlayAnimation(11, _FindBestDeathAnimationSNO() , 1f, 2);

            this.Target.World.BroadcastIfRevealed(new ANNDataMessage(Opcodes.ANNDataMessage24)
            {
                ActorID = this.Target.DynamicID,
            }, this.Target);
            */
            // remove all buffs and running powers before deleting actor
            //this.Target.World.BuffManager.RemoveAllBuffs(this.Target);
            //this.Target.World.PowerManager.CancelAllPowers(this.Target);

            //estos 2 van si o si con un playanimationmessage antes sino no anda nada

            /*this.Target.Attributes[GameAttributeStaticList.Deleted_On_Server] = true;
            this.Target.Attributes[GameAttributeStaticList.Could_Have_Ragdolled] = true;*/

            this.Target.Attributes.BroadcastChangedIfRevealed();

            //Target.OnKill(this.Context);

            // Spawn Random item and give exp for each player in range
            /*List<Player> players = this.Target.GetPlayersInRange(26f);
            foreach (Player plr in players)
            {
                plr.UpdateExp(this.Target.Attributes[GameAttribute.Experience_Granted]);
                this.Target.World.SpawnRandomItemDrop(this.Target, plr);
            }*/

            /*if (LootAndExp)
            {
                if (this.Context.User is Player)
                {
                    Player player = (Player)this.Context.User;

                    player.ExpBonusData.Update(player.GBHandle.Type, this.Target.GBHandle.Type);
                    this.Target.World.SpawnGold(this.Target, player);
                    if (Mooege.Common.Helpers.Math.RandomHelper.Next(1, 100) < 20)
                        this.Target.World.SpawnHealthGlobe(this.Target, player, this.Target.Position);
                }
            }

            if (this.Target is Monster)
                (this.Target as Monster).PlayLore();*/

            // HACK: instead of deleting actor right here, its added to a list (near the top of this function)
            this.Target.Destroy();
        }

        private void _DoSimplePlayerDeath()
        {
            // HACK: simple death implementation
            /*this.Target.World.BuffManager.RemoveAllBuffs(this.Target);
            this.Target.World.PowerManager.CancelAllPowers(this.Target);

            this.Target.World.BuffManager.AddBuff(this.Target, this.Target, new Implementations.ActorGhostedBuff());

            Player player = (Player)this.Target;
            player.Teleport(player.CheckPointPosition);
            player.AddPercentageHP(100);*/
        }

    }
}
