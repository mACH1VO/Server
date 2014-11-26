using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dirac.GameServer.Core
{
    public static class ProbHelper
    {
        public static bool Prob(double percent)
        {
            double temp = (double)((percent) / 100);
            double ran = RandomHelper.NextDouble();
            if (ran < temp)
                return true;
            return false;
        }

        public static int Percent(int basenum, double percent)
        {
            return (int)((percent * basenum) / 100);
        }

        public static float Percent(float basenum, double percent)
        {
            return (float)((percent * basenum) / 100);
        }
    }
}
