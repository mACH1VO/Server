using System;

namespace Dirac.GameServer.Core.AI.Actions.States
{
	/// <summary>
	/// An action of NPCs that selects targets, according to Threat and other factors and then
	/// updates the given <see cref="Strategy"/> to kill the current target.
	/// </summary>
	public class AICombatAction : AIAction , IStrategy
	{
		/// <summary>
		/// Only check for a Threat update every 20 ticks
		/// </summary>
		public static int ReevaluateThreatTicks = 20;

        public AIAction Strategy { get; set; }
		private bool m_init;

		public AICombatAction(Monster owner)
			: base(owner)
		{
		}

        public AICombatAction(Monster owner, AIAction combatAction)
			: base(owner)
		{
			Strategy = combatAction;
		}

		/// <summary>
		/// If true, the owner wants to retreat from combat and go back to its AttractionPoint
		/// due to too big distance and not being hit or hitting itself
		/// </summary>
		/*public bool WantsToRetreat
		{
			get
			{
				return
                    m_owner is Monster &&
                    ((Monster)m_owner).CanEvade &&
					!m_owner.IsInRadiusSq(m_owner.Brain.SourcePoint, NPCMgr.DefaultMaxHomeDistanceInCombatSq) &&
					   ((((NPC)m_owner).Entry.Rank >= CreatureRank.RareElite) ||
						m_owner.MillisSinceLastCombatAction > NPCMgr.GiveUpCombatDelay);
			}
		}*/

		public override void Start()
		{
			m_init = false;
			//m_owner.Movement.MoveType = AIMoveType.Run;
		}

        public override void Update(TimeSpan elapsed)
		{

            if (!this.Owner.GetActorsInRange(50).Contains(this.Target))
            {
                this.Owner.Brain.EnterDefaultState();
            }
            else
            {
                Actor target = this.Target;
                if (target != null)
                {
                    Logging.LogManager.DefaultLogger.Warn(target.ToString());
                }
            }
			/*if (!m_owner.CanDoHarm)
			{
				// busy
				return;
			}*/

			/*if (WantsToRetreat)
			{
				m_owner.Brain.State = BrainState.Evade;
			}
			else
			{
				if (owner.Target == null ||
					!m_owner.CanBeAggroedBy(owner.Target) ||
					owner.CheckTicks(ReevaluateThreatTicks))
				{
					Unit target;
					while ((target = owner.ThreatCollection.CurrentAggressor) != null)
					{
						// if target is dead or gone, check for other targets or retreat
						if (!m_owner.CanBeAggroedBy(target))
						{
							// remove dead and invalid targets from aggro list
							owner.ThreatCollection.Remove(target);
						}
						else
						{
							if (m_Strategy == null)
							{
								// no action set - must not happen
								Log.Error("Executing " + GetType().Name + " without having a Strategy set.");
							}
							else
							{
								if (owner.Target != target || !m_init)
								{
									// change target and start Action again
									var oldTarget = owner.Target;
									owner.Target = target;
									StartEngagingCurrentTarget(oldTarget);
								}
								else
								{
									m_Strategy.Update();
								}
								return;
							}
						}
					}
				}
				else
				{
					if (!m_init)
					{
						StartEngagingCurrentTarget(null);
					}
					else
					{
						m_Strategy.Update();
					}
					return;
				}

				// no one left to attack
				if (owner.CanEvade)
				{
					// evade
					if (owner.MillisSinceLastCombatAction > NPCMgr.CombatEvadeDelay)
					{
						// check if something came up again
						if (!m_owner.Brain.CheckCombat())
						{
							// run back
							owner.Brain.ClearCombat(BrainState.Evade);
						}
					}
				}
				else
				{
					// cannot evade -> Just go back to default if there are no more targets
					if (!owner.Brain.CheckCombat())
					{
						// go back to what we did before
						owner.Brain.ClearCombat(owner.Brain.DefaultState);
					}
				}
			}*/
		}

		public override void Stop()
		{
			if (Strategy != null)
			{
				Strategy.Stop();
			}

			/*if (m_init && this.Target != null)
			{
				Disengage(this.Target);
			}*/

			Target = null;
			//m_owner.MarkUpdate(UnitFields.DYNAMIC_FLAGS);
		}

		/// <summary>
		/// Start attacking a new target
		/// </summary>
        private void StartEngagingCurrentTarget(Monster oldTarget)
		{
			if (m_init)
			{
				if (oldTarget != null)
				{
					// had a previous target
					//Disengage(oldTarget);
				}
			}
			else
			{
				m_init = true;
			}
			m_owner.IsFighting = true;
			Strategy.Start();
		}
	}
}