using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dirac.GameServer
{
    public class TickTimer
    {
        public int TimeOutLong { get; private set; }
        public int ResetTimes { get; private set; }
        public int StartTick { get; private set; }

        public TickTimer(TimeSpan timespan)
        {
            StartTick = Environment.TickCount;
            this.TimeOutLong = (int)timespan.TotalMilliseconds;
        }


        public TickTimer(int timeOutLong)
        {
            StartTick = Environment.TickCount;
            this.TimeOutLong = timeOutLong;
        }

        public bool TimedOut
        {
            //lock? Environment.TickCount is not threadsafe.
            get { return Environment.TickCount >= (StartTick + TimeOutLong); }
        }

        public int Remain
        {
            get
            {
                if ((StartTick + TimeOutLong) - Environment.TickCount > 0)
                {
                    return (StartTick + TimeOutLong) - Environment.TickCount;
                }
                else
                {
                    return -1;
                }
            }
        }

        public void Reset()
        {
            this.StartTick = Environment.TickCount;
            ResetTimes++;
        }

        public void Reset(int newLong)
        {
            this.TimeOutLong = newLong;
            this.StartTick = Environment.TickCount;
            ResetTimes++;
        }
    }
}
