

using System.Windows;
using Gibbed.IO;
using Dirac.Math;
using Rect = System.Windows.Rect;

namespace Dirac.GameServer.Types
{
    public struct Circle
    {
        /// <summary> 
        /// Center position of the circle. 
        /// </summary> 
        public Vector2 Center;

        /// <summary> 
        /// Radius of the circle. 
        /// </summary> 
        public float Radius;

        /// <summary> 
        /// Constructs a new circle. 
        /// </summary> 
        public Circle(Vector2 position, float radius)
        {
            this.Center = position;
            this.Radius = radius;
        }

        /// <summary> 
        /// Constructs a new circle. 
        /// </summary> 
        public Circle(float x, float y, float radius)
            : this(new Vector2(x, y), radius)
        { }

        /// <summary> 
        /// Determines if a circle intersects a rectangle. 
        /// </summary> 
        /// <returns>True if the circle and rectangle overlap. False otherwise.</returns> 
        public bool Intersects(Rect rectangle)
        {
            // Find the closest point to the circle within the rectangle
            float closestX = Clamp(this.Center.x, (float)rectangle.Left, (float)rectangle.Right);
            float closestY = Clamp(this.Center.y, (float)rectangle.Top, (float)rectangle.Bottom);

            // Calculate the distance between the circle's center and this closest point
            float distanceX = this.Center.x - closestX;
            float distanceY = this.Center.y - closestY;

            // If the distance is less than the circle's radius, an intersection occurs
            float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
            return distanceSquared < (this.Radius * this.Radius);
        }

        public static float Clamp(float value, float min, float max)
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;
            return value;
        }
    }
}
