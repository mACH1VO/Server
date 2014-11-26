using System;
using System.Linq;
using System.Text;


namespace Dirac.Extensions
{
    public static class StringHashHelper
    {
        public static uint HashIdentity(String input)
        {
            var bytes = Encoding.ASCII.GetBytes(input);
            return bytes.Aggregate(0x811C9DC5, (current, t) => 0x1000193 * (t ^ current));
        }

        public static int HashItemName(String input)
        {
            int hash = 0;
            input = input.ToLower();
            for (int i = 0; i < input.Length; ++i)
                hash = (hash << 5) + hash + input[i];
            return hash;
        }

        /// <summary>
        /// Hashes a string to an int. This hashing is CASE SENSITIVE
        /// </summary>
        public static int HashNormal(String input)
        {
            int hash = 0;
            for (int i = 0; i < input.Length; ++i)
                hash = (hash << 5) + hash + input[i];
            return hash;
        }

    }
}
