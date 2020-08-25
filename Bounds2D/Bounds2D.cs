using System;
using System.Globalization;

namespace UnityEngine
{
    public struct Bounds2D : IEquatable<Bounds2D>
    {
        private Vector2 m_center;
        private Vector2 m_extents;

        /// <summary>
        /// The center of the bounding box.
        /// </summary>
        public Vector2 center
        {
            get
            {
                return m_center;
            }
            set
            {
                m_center = value;
            }
        }

        /// <summary>
        /// The total size of the box. This is always twice as large as the extents.
        /// </summary>
        public Vector2 size
        {
            get
            {
                return m_extents * 2f;
            }
            set
            {
                m_extents = value * 0.5f;
            }
        }

        /// <summary>
        /// The extents of the Bounding Box. This is always half of the size of the Bounds.
        /// </summary>
        public Vector2 extents
        {
            get
            {
                return m_extents;
            }
            set
            {
                m_extents = value;
            }
        }

        /// <summary>
        /// The minimal point of the box. This is always equal to center - extents.
        /// </summary>
        public Vector2 min
        {
            get
            {
                return center - extents;
            }
            set
            {
                SetMinMax(value, max);
            }
        }

        /// <summary>
        /// The maximal point of the box. This is always equal to center + extents.
        /// </summary>
        public Vector2 max
        {
            get
            {
                return center + extents;
            }
            set
            {
                SetMinMax(min, value);
            }
        }

        /// <summary>
        /// Creates a new Bounds 2D.
        /// </summary>
        /// <param name="center">The location of the origin of the Bounds.</param>
        /// <param name="size">The dimensions of the Bounds.</param>
        public Bounds2D(Vector2 center, Vector2 size)
        {
            m_center = center;
            m_extents = size * 0.5f;
        }

        public override int GetHashCode()
        {
            return center.GetHashCode() ^ (extents.GetHashCode() << 2);
        }

        public override bool Equals(object other)
        {
            if (!(other is Bounds2D))
                return false;
            return Equals((Bounds2D)other);
        }

        public bool Equals(Bounds2D other)
        {
            return center.Equals(other.center) && extents.Equals(other.extents);
        }

        public static bool operator ==(Bounds2D lhs, Bounds2D rhs)
        {
            return lhs.center == rhs.center && lhs.extents == rhs.extents;
        }

        public static bool operator !=(Bounds2D lhs, Bounds2D rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Sets the bounds to the min and max value of the box.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void SetMinMax(Vector2 min, Vector2 max)
        {
            extents = (max - min) * 0.5f;
            center = min + extents;
        }

        /// <summary>
        /// Grows the Bounds to include the point.
        /// </summary>
        /// <param name="point"></param>
        public void Encapsulate(Vector2 point)
        {
            SetMinMax(Vector2.Min(min, point), Vector2.Max(max, point));
        }

        /// <summary>
        /// Grow the bounds to encapsulate the bounds.
        /// </summary>
        /// <param name="bounds"></param>
        public void Encapsulate(Bounds2D bounds)
        {
            Encapsulate(bounds.center - bounds.extents);
            Encapsulate(bounds.center + bounds.extents);
        }

        /// <summary>
        /// Expand the bounds by increasing its size by amount along each side.
        /// </summary>
        /// <param name="amount"></param>
        public void Expand(float amount)
        {
            amount *= 0.5f;
            extents += new Vector2(amount, amount);
        }

        /// <summary>
        /// Expand the bounds by increasing its size by amount along each side.
        /// </summary>
        /// <param name="amount"></param>
        public void Expand(Vector2 amount)
        {
            extents += amount * 0.5f;
        }

        /// <summary>
        /// Does another bounding box intersect with this bounding box?
        /// </summary>
        /// <param name="bounds"></param>
        public bool Intersects(Bounds2D bounds)
        {
            return min.x <= bounds.max.x && max.x >= bounds.min.x && min.y <= bounds.max.y && max.y >= bounds.min.y;
        }

        /// <summary>
        /// Is point contained in the bounding box?
        /// </summary>
        /// <param name="position"></param>
        public bool Contains(Vector2 position)
        {
            return position.x >= min.x && position.y >= min.y && position.x <= max.x && position.y <= max.y;
        }

        /// <summary>
        /// Returns a nicely formatted string for the bounds.
        /// </summary>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture.NumberFormat, "Center: {0}, Extents: {1}", m_center, m_extents);
        }

        /// <summary>
        /// Returns a nicely formatted string for the bounds.
        /// </summary>
        /// <param name="format"></param>
        public string ToString(string format)
        {
            return string.Format(CultureInfo.InvariantCulture.NumberFormat, "Center: {0}, Extents: {1}", m_center.ToString(format), m_extents.ToString(format));
        }
    }
}