using System;
using Dirac.Math;
using Dirac.GameServer.Core;

namespace Dirac.GameServer.Core.AI.Actions.States
{
	public class AIFollowAction : AIAction, IStrategy
	{
        public AIAction Strategy { get; set; }

		public AIFollowAction(Monster owner) 
            : base(owner)
		{
		}

		public override void Start()
		{
		}

        public override void Update(TimeSpan elapsed)
        {
            if (!Delay.TimedOut)
                return;
            
            if (this.Owner.GetActorsInRange(50).Contains(this.Target))
            {
                //Logging.LogManager.DefaultLogger.Trace("AIFollowAction: MoveToTarget");
                Vector3 director = (Target.Position - this.Owner.Position).NormalizedCopy;
                float distance = (Target.Position - this.Owner.Position).Length;
                if (distance - 10/*rad coll player obj*/ > 0)
                {
                    Vector3 newPositionToGo = this.Owner.Position + director * (distance - 2);
                    this.Owner.MoveTo(/*Target.Position*/newPositionToGo, 0);
                }
                
            }
            else
            {
                //lost visual range, back to roam!
                this.Owner.Brain.EnterState(Brains.BrainState.Roam);
            }

            Delay.Reset();

        }

        public override void Stop()
        {

        }
	}
}