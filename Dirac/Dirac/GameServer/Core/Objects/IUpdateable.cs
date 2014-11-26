using System;

namespace Dirac.GameServer
{
    /// <summary>
    /// Defines an interface for objects that can update.
    /// </summary>
    public interface IUpdateable
    {
        /// <summary>
        /// Tells object to update itself and call it's IUpdateable childs if any.
        /// </summary>
        void Update(TimeSpan elapsed);
    }
}
