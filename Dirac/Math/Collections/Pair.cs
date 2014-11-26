namespace Dirac.Math.Collections
{
    /// <summary>
    /// 	A simple container class for returning a pair of objects from a method call
    /// 	(similar to std::pair, minus the templates).
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class Pair
    {
        public object first;
        public object second;

        public Pair(object first, object second)
        {
            this.first = first;
            this.second = second;
        }
    }
}