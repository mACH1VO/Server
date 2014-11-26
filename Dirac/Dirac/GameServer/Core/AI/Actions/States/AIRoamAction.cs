using System;
using System.Collections.Generic;
using System.Windows;
using Dirac.Math;

using Dirac.GameServer.Core.AI.Brains;
using Dirac.GameServer.Core.AI.Actions.States;

namespace Dirac.GameServer.Core.AI.Actions.States
{
	/// <summary>
	/// AI movemement action for roaming
	/// </summary>
	public class AIRoamAction : AIAction , IStrategy
	{
        public AIAction Strategy { get; set; }

		public AIRoamAction(Monster owner)
			: base(owner)
		{
            
		}

        public AIRoamAction(Monster owner, AIAction roamActionStrategy) 
            :base(owner)
		{
			Strategy = roamActionStrategy;
		}

		public override void Start()
		{
			// make sure we don't have Target nor Attacker
            this.Delay.Reset(5000); //newLong
			//m_owner.FirstAttacker = null;
			this.Target = null;
			//Strategy.Start();
		}

        public override void Update(TimeSpan elapsed)
        {
            if (this.Owner.World == null) //sould be if owner.spawned?
                return;
            if (!Delay.TimedOut)
                return;
            this.Delay.Reset(RandomHelper.Next(5000, 10000));
            //Logging.LogManager.DefaultLogger.Trace("[Update RoamAction] " + this.Owner.ToString());
            if (!this.Owner.IsMoving)
            {
                List<Player> RangedPlayers = this.Owner.GetPlayersInRange(20);
                if (RangedPlayers.Count != 0)
                {
                    this.Owner.Brain.EnterState(BrainState.Follow, RangedPlayers[0]);
                }
                else
                {
                    //roam
                    float uno = RandomHelper.Next(-100, 100);
                    float dos = RandomHelper.Next(-100, 100);
                    Vector3 destinPos = new Vector3(uno, -0f, dos);
                    Vector3 destinPos2 = this.Owner.Position + destinPos;
                    if ((this.Owner.Position - destinPos2).Length < 3)
                        return;
                    this.Owner.MoveTo(destinPos, 1f);
                }
            }

            Delay.Reset();

        }

		public override void Stop()
		{
			//Strategy.Stop();
		}

		/*/// <summary>
		/// Tries to cast a Spell that is ready and allowed in the current context.
		/// </summary>
		/// <returns></returns>
		protected bool TryCastSpell()
		{
			Monster owner = m_owner;

			foreach (var spell in owner.NPCSpells.ReadySpells)
			{
				if (!spell.HasHarmfulEffects && spell.CanCast(owner))
				{
					return m_owner.SpellCast.Start(spell) == SpellFailedReason.Ok;
				}
			}
			return false;
		}*/
	}
}