

using System;

namespace Dirac.Extensions
{
    public static class DateTimeExtensions
    {
        public static int ToUnixTime(this DateTime time)
        {
            return (int)((time.ToUniversalTime().Ticks - 621355968000000000L) / 10000000L);
        }

        public static ulong ToExtendedEpoch(this DateTime time)
        {
            return (ulong)((time.ToUniversalTime().Ticks - 621355968000000000L) / 10L);
        }
    }
}
