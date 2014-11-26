using System;
using Dirac.GameServer.Core;
using Dirac.GameServer.Core.AI.Brains;

namespace Dirac.GameServer.Core.AI.Actions.Movement
{
	/// <summary>
	/// Moves to the Target and then enters Idle mode
	/// </summary>
	public class AIMoveToTargetAction : AIAction
	{
        public AIMoveToTargetAction(Monster owner)
			: base(owner)
		{
		}

        public AIMoveToTargetAction(Monster owner, Actor target)
            : base(owner)
        {
            this.Target = target;
        }

		public override void Start()
		{
			if (this.Target == null)
			{
				//m_owner.Say("I have no Target to follow.");
                Logging.LogManager.DefaultLogger.Warn("Monster id {0} has no target to follow", this.Owner.ToString());
				this.Owner.Brain.EnterDefaultState();
			}
			else
			{
                _target = this.Target;
			}
        }

        public override void Update(TimeSpan elapsed)
        {
            if (this.Owner.IsMoving)
            {
                this.Owner.Position = this.Owner.Path.Advance(elapsed.Ticks, this.Owner.TranslateSpeed);
                if (this.Owner.Path.HasReachedPosition)
                {
                    this.Owner.StopMoving();
                    //this.Owner.Brain.State = Brains.BrainState.Idle;
                }
            }
            else
            {
                this.Owner.MoveTo(this.Target.Position, 0);
            }
        }

		public override void Stop()
		{
			this.Target = null;
		}
	}
}