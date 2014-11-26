using System;

namespace Dirac.GameServer.Core.AI.Actions.States
{
	public class AIGuardMasterAction : AIAction, IStrategy
	{
        public AIAction Strategy { get; set; }

		public AIGuardMasterAction(Monster owner) 
            : base(owner)
		{
		}

        public override void Update(TimeSpan elapsed)
		{
			if (!m_owner.IsFighting)
			{

			}
		}

        public override void Stop()
        {
        }

        public override void Start()
        {
        }

	}
}