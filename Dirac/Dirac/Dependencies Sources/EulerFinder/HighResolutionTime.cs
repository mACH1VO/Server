using System;
using System.Collections.Generic;
using System.Text;

namespace EulerFinder
{
    public static class HighResolutionTime
    {
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long perfcount);

        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long freq);

        private static long mStartCounter;
        private static long mFrequency;

        static HighResolutionTime()
        {
            QueryPerformanceFrequency(out mFrequency);
        }

        public static void Start()
        {
            QueryPerformanceCounter(out mStartCounter);
        }

        public static double GetTime()
        {
            long endCounter;
            QueryPerformanceCounter(out endCounter);
            long elapsed = endCounter - mStartCounter;
            return (double) elapsed / mFrequency;
        }
    }
}

