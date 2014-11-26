

namespace Dirac.GameServer
{
    /// <summary>
    /// A dynamic object that can have a dynamicId
    /// </summary>
    public abstract class DynamicObject
    {
        /// <summary>
        /// The dynamic unique runtime ID for the actor.
        /// </summary>
        public readonly int DynamicID;

        /// <summary>
        /// Initialization constructor.
        /// </summary>
        /// <param name="dynamicID">The dynamic ID to initialize with.</param>
        protected DynamicObject(int dynamicID)
        {
            this.DynamicID = dynamicID;
        }

        /// <summary>
        /// Destroy the object. This should remove any references to the object throughout GS.
        /// </summary>
        public abstract void Destroy();
    }
}
