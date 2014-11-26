

using System;
using System.Diagnostics;
using System.Windows;
using Dirac.GameServer.Types;
using Dirac.Math;
using Rect = System.Windows.Rect;

namespace Dirac.GameServer.Core
{
    /// <summary>
    /// An object that can be placed in world.
    /// </summary>
    public abstract class WorldObject : DynamicObject, IRevealable
    {
        private Object worldLocker = new Object();
        private Map _world;
        public Map World
        {
            get
            {
                lock (worldLocker)
                {
                    return _world;
                }
            }
            set
            {
                _world = value;
            }
        }

        protected Vector3 _position;

        /// <summary>
        /// The position of the object.
        /// </summary>
        public virtual Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                this.Bounds = new Rect(this.Position.x, this.Position.z, this.Size.Width, this.Size.Height); //asco
                var handler = PositionChanged;
                if (handler != null) 
                    handler(this, EventArgs.Empty);
            }
        }

        

        public virtual bool HasWorldLocation { get; set; }

        public Boolean IsAlreadyDestroyed { get; set; }

        /*public virtual void LookAt(Vector3 destPoint)
        {
            this.SceneNode.LookAt(destPoint, Node.TransformSpace.TS_PARENT, LocalDirectionVector);
            this.LastLookAtPoint = destPoint;
        }*/


        /*public SceneNode SceneNode;

        public virtual void LookAt(Vector3 destPoint)
        {
            this.SceneNode.LookAt(destPoint, Node.TransformSpace.TS_PARENT, LocalDirectionVector);
            this.LastLookAtPoint = destPoint;
        }*/

        public virtual Quaternion Orientation
        {
            get;
            set;
        }
        /// <summary>
        /// Event handler for position-change.
        /// </summary>
        public event EventHandler PositionChanged;

        /// <summary>
        /// Size of the object.
        /// </summary>
        public Size Size { get; protected set; }

        /// <summary>
        /// Automatically calculated bounds for object used by QuadTree.
        /// </summary>
        public Rect Bounds { get; set; }

        /// <summary>
        /// Scale of the object.
        /// </summary>
        public float Scale { get; set; }

        public Vector3 RotationAxis { get; set; }

        public float RotationW { get; set; }

        /// <summary>
        /// Creates a new world object.
        /// </summary>
        /// <param name="world">The world object belongs to.</param>
        /// <param name="dynamicID">The dynamicId of the object.</param>
        protected WorldObject(int dynamicID)
            : base(dynamicID)
        {
            this.Orientation = Dirac.Math.Quaternion.Identity;
            this.Size = new Size(1, 1); //QuadTree requeriment
            this.World = null;
            this.RotationAxis = new Vector3();
            this.Position = Vector3.Zero;
            this.IsAlreadyDestroyed = false;
        }

        /// <summary>
        /// Reveals the object to given player.
        /// </summary>
        /// <param name="player">The player to reveal the object.</param>
        /// <returns>true if the object was revealed or false if the object was already revealed.</returns>
        public abstract bool Reveal(Player player);

        /// <summary>
        /// Unreveals the object to given plaer.
        /// </summary>
        /// <param name="player">The player to unreveal the object.</param>
        /// <returns>true if the object was unrevealed or false if the object wasn't already revealed.</returns>
        public abstract bool Unreveal(Player player);

        /// <summary>
        /// Makes the object leave the world and then destroys it.
        /// </summary>
        public override void Destroy()
        {
            if (this is Actor)
                this.World.Leave(this as Actor);

            //Game.EndTracking(this); //nunca lo uso
            //this.World = null;
            this.IsAlreadyDestroyed = true;
        }
    }
}
