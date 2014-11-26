using System;
using System.Linq;
using Dirac.Logging;
using Dirac.GameServer.Types;
using Dirac.GameServer;
using Dirac.Extensions;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Dirac.Math;
using Dirac.GameServer.Network;
using Dirac.GameServer.Network.Message;


namespace Dirac.GameServer.Core
{
    public static class SkillManager //esta clase deberia estar en Player o ser singleton, no me gusta static.
    {
        public static List<Projectile> Projectiles = new List<Projectile>();

        public static bool RunPower(Actor owner/*, Actor targetedActor*/, TargetMessage targetMessage)
        {

            if (targetMessage.PowerSlot == null)
                return false; //thrwo ex

            //targetPosition = targetedActor.Position;

            //user.Attributes[GameAttribute.Mana_Cur, (int)(user as Player).Toon.HeroTable.PrimaryResource] = 100 /*GetMaxResource((int)Toon.HeroTable.SecondaryResource)*/;
            //user.Attributes.BroadcastChangedIfRevealed();


            //return false;
            

            // find and run a power implementation
            SkillContext implementation = SkillManager.createInstance((owner as Player), (SkillSlot)targetMessage.PowerSlot);

            if (implementation == null)
                return false; //loghack //throw ex

            if (owner.World.Actors.ContainsKey(targetMessage.TargetID))
                implementation.TargetActor = owner.World.Actors[targetMessage.TargetID];
            implementation.TargetMessageFromClient = targetMessage;
            implementation.Player = (owner as Player);
            implementation.World = owner.World;
            implementation.DestinationUserClick = targetMessage.Position;
            implementation.VectorDirector = (implementation.DestinationUserClick - implementation.Player.Position).NormalizedCopy;

            implementation.Run();

            //Logging.LogManager.DefaultLogger.Trace("Skill SNO Used {0}", targetMessage.PowerSlot);

            return true;
        }

        private static SkillContext createInstance(Player player, SkillSlot opcode)
        {
            /*SkillContext ret = null; //powercontext base class for skill
            switch (this.SkillSlots[opcode])
            {
                default:
                    ret = null;  //throw ex? creo q no, handle el null despues
                    break;
            }
            return ret;*/

            return null;
        }
        
    }

}
