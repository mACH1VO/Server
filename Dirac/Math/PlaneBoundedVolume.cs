using Dirac.Math.Collections;

namespace Dirac.Math
{
    /// <summary>
    ///		Represents a convex volume bounded by planes.
    /// </summary>
    public class PlaneBoundedVolume
    {
        #region Fields

        /// <summary>
        ///		Publicly accessible plane list, you can modify this direct.
        /// </summary>
        public PlaneList planes = new PlaneList();

        /// <summary>
        ///		Side of the plane to be considered 'outside'.
        /// </summary>
        public PlaneSide outside;

        #endregion Fields

        #region Constructors

        /// <summary>
        ///		Default constructor.
        /// </summary>
        public PlaneBoundedVolume()
            : this(PlaneSide.Negative) { }

        /// <summary>
        ///		Constructor.
        /// </summary>
        /// <param name="outside">Side of the plane to be considered 'outside'.</param>
        public PlaneBoundedVolume(PlaneSide outside)
        {
            this.outside = outside;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///		Intersection test with an <see cref="AxisAlignedBox"/>.
        /// </summary>
        /// <remarks>
        ///		May return false positives but will never miss an intersection.
        /// </remarks>
        /// <param name="box">Box to test.</param>
        /// <returns>True if interesecting, false otherwise.</returns>
        public bool Intersects(AxisAlignedBox box)
        {
            if (box.IsNull)
            {
                return false;
            }

            if (box.IsInfinite)
            {
                return true;
            }

            // Get centre of the box
            Vector3 center = box.Center;
            // Get the half-size of the box
            Vector3 halfSize = box.HalfSize;

            // If all points are on outside of any plane, we fail
            Vector3[] points = box.Corners;

            for (int i = 0; i < planes.Count; i++)
            {
                Plane plane = (Plane)planes[i];

                PlaneSide side = plane.GetSide(center, halfSize);
                if (side == outside)
                {
                    // Found a splitting plane therefore return not intersecting
                    return false;
                }
            }

            // couldn't find a splitting plane, assume intersecting
            return true;
        }

        /// <summary>
        ///		Intersection test with <see cref="Sphere"/>.
        /// </summary>
        /// <param name="sphere">Sphere to test.</param>
        /// <returns>True if the sphere intersects this volume, and false otherwise.</returns>
        public bool Intersects(Sphere sphere)
        {
            for (int i = 0; i < planes.Count; i++)
            {
                Plane plane = (Plane)planes[i];

                // Test which side of the plane the sphere is
                float d = plane.GetDistance(sphere.Center);

                // Negate d if planes point inwards
                if (outside == PlaneSide.Negative)
                {
                    d = -d;
                }

                if ((d - sphere.Radius) > 0)
                {
                    return false;
                }
            }

            // assume intersecting
            return true;
        }

        #endregion Methods
    }
}