using System;

namespace Dirac.GameServer.Core.AI.Actions.States
{
	public class AIIdleAction : AIAction
	{

		public AIIdleAction(Monster owner) 
            : base(owner) 
        {
        }

		public override void Start()
		{
		}

        public override void Update(TimeSpan elapsed)
		{
		}

		public override void Stop()
		{
		}

	}
}