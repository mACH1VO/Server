

using System;
using System.Linq;
using System.Collections.Generic;

namespace Dirac.GameServer.Core
{
    public class RandomHelper
    {
        private readonly static Random _random;

        static RandomHelper()
        {
            _random = new Random();
        }

        public static int Next()
        {
            return _random.Next();
        }

        public static int Next(Int32 maxValue)
        {
            return _random.Next(maxValue);
        }

        public static int Next(Int32 minValue, Int32 maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
        public static float Nextf(float minValue, float maxValue)
        {
            float width = (maxValue - minValue);
            float rand = (float)_random.NextDouble();
            return (float)decimal.Round((decimal)(minValue + width*rand), 2); /*2 porque redondeo a 2 cifras*/
        }

        public static void NextBytes(byte[] buffer)
        {
            _random.NextBytes(buffer);
        }

        public static double NextDouble()
        {
            return _random.NextDouble();
        }

        
        public static TValue RandomValue<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        {
            List<TValue> values = Enumerable.ToList(dictionary.Values);
            int size = dictionary.Count;
            /*while (true)
            {
                yield return values[_random.Next(size)];
            }*/
            return values[_random.Next(size)];
        }

        public static T RandomItem<T>(IEnumerable<T> list, Func<T, float> probability)
        {
            int cumulative = (int)list.Select(x => probability(x)).Sum();

            int randomRoll = RandomHelper.Next(cumulative);
            float cumulativePercentages = 0;

            foreach (T element in list)
            {
                cumulativePercentages += probability(element);
                if (cumulativePercentages > randomRoll)
                    return element;
            }

            return list.First();
        }
    
    }

}
