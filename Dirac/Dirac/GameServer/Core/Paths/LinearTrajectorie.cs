using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dirac.Math;
using Dirac.Logging;
using Dirac.GameServer;

namespace Dirac.GameServer.Core
{
    public class LinearTrajectorie
    {
        public readonly Vector3 V0;
        public readonly Vector3 Destination;
        public readonly float PathLen;
        public readonly Vector3 Versor;
        public Vector3 Velocity;
        public Vector3 CurrentPosition;
        public Vector3 Direction;
        public float Speed;
        public float CurrentLen;

        public LinearTrajectorie(Vector3 from, Vector3 to)
        {
            //Logging.LogManager.DefaultLogger.Trace("Speed " + speed.ToString());
            V0 = from;
            Destination = to;
            Versor = (to - from).NormalizedCopy;
            Direction = Versor;
            PathLen = (to - from).Length;
            CurrentLen = 0;
            CurrentPosition = from;
        }

        public Vector3 Advance(long ticks, float speed)
        {
            //Logging.LogManager.DefaultLogger.Trace("Factor " + (ticks / 100000f).ToString());

            Speed = speed;
            Velocity = (Versor * speed); //actualize current velocity with speed sended

            Vector3 newPosition = CurrentPosition + Velocity * (ticks / 100000f);

            CurrentLen += (newPosition - CurrentPosition).Length; //( new - old ).len, o sea, suma de a pedacitos avanzados

            CurrentPosition = newPosition;
            return CurrentPosition;
        }

        public Boolean HasReachedPosition
        {
            get
            {
                if (CurrentLen >= PathLen)
                    return true;
                else return false;
            }
        }
    }
}
