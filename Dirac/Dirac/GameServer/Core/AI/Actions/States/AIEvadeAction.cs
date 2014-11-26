using System;

namespace Dirac.GameServer.Core.AI.Actions.States
{
	/// <summary>
	/// NPC leaves combat and goes home
	/// </summary>
	public class AIEvadeAction : AIAction, IStrategy
	{
        public AIAction Strategy { get; set; }

        public AIEvadeAction(Monster owner)
			: base(owner)
		{
		}

		public override void Start()
		{
			m_owner.IsEvading = true;
			this.Target = null;
		}

        public override void Update(TimeSpan elapsed)
		{
		}

		public override void Stop()
		{
			m_owner.IsEvading = false;
            m_owner.StopMoving();
		}

	}
}