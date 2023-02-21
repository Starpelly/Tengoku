using System.Globalization;

namespace Trinkit
{
    public struct Vector3 : IEquatable<Vector3>, IFormattable
    {
        /// <summary>
        /// X component of the vector.
        /// </summary>
        public float x;
        /// <summary>
        /// Y component of the vector.
        /// </summary>
        public float y;
        /// <summary>
        /// Z component of the vector.
        /// </summary>
        public float z;

        /// <summary>
        /// Creates a new vector with given x, y, and z components.
        /// </summary>
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        /// <summary>
        /// Creates a new vector with given x, y components and sets z to zero.
        /// </summary>
        public Vector3(float x, float y) { this.x = x; this.y = y; this.z = 0f; }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Vector3(
                a.x + (b.x - a.x) * t,
                a.y + (b.y - a.y) * t,
                a.z + (b.z - a.z) * t
                );
        }

        public override bool Equals(object other)
        {
            if (!(other is Vector3)) return false;

            return Equals((Vector3)other);
        }

        public bool Equals(Vector3 other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "F2";
            if (formatProvider == null)
                formatProvider = CultureInfo.InvariantCulture.NumberFormat;
            return String.Format("({0}, {1}, {2})", x.ToString(format, formatProvider), y.ToString(format, formatProvider), z.ToString(format, formatProvider));
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        public static Vector3 operator +(Vector3 a, Vector3 b) { return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z); }
        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        public static Vector3 operator -(Vector3 a, Vector3 b) { return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z); }
        /// <summary>
        /// Mulitplies two vectors.
        /// </summary>
        public static Vector3 operator *(Vector3 a, Vector3 b) { return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z); }
        /// <summary>
        /// Divides two vectors.
        /// </summary>
        public static Vector3 operator /(Vector3 a, Vector3 b) { return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z); }
        /// <summary>
        /// Negates a vector.
        /// </summary>
        public static Vector3 operator -(Vector3 a) { return new Vector3(-a.x, -a.y, -a.z); }
        /// <summary>
        /// Adds a vector by a number.
        /// </summary>
        public static Vector3 operator +(Vector3 a, float d) { return new Vector3(a.x + d, a.y + d, a.z + d); }
        public static Vector3 operator +(float a, Vector3 d) { return new Vector3(a + d.x, a + d.y, a + d.z); }
        /// <summary>
        /// Subtracts a vector by a number.
        /// </summary>
        public static Vector3 operator -(Vector3 a, float d) { return new Vector3(a.x - d, a.y - d, a.z - d); }
        public static Vector3 operator -(float a, Vector3 d) { return new Vector3(a - d.x, a - d.y, a - d.z); }
        /// <summary>
        /// Multiplies a vector by a number.
        /// </summary>
        public static Vector3 operator *(Vector3 a, float d) { return new Vector3(a.x * d, a.y * d, a.z * d); }
        public static Vector3 operator *(float a, Vector3 d) { return new Vector3(a * d.x, a * d.y, a * d.z); }
        /// <summary>
        /// Divides a vector by a number.
        /// </summary>
        public static Vector3 operator /(Vector3 a, float d) { return new Vector3(a.x / d, a.y / d, a.z / d); }
        public static Vector3 operator /(float a, Vector3 d) { return new Vector3(a / d.x, a / d.y, a / d.z); }

        public static explicit operator Vector3(Vector2 v)
        {
            return new Vector3(v.x, v.y);
        }

        /// <summary>
        /// Converts Trinkit Vector2 to System.Numerics Vector2
        /// </summary>
        /// <returns></returns>
        public System.Numerics.Vector3 ToNumerics()
        {
            return new System.Numerics.Vector3(x, y, z);
        }
    }
}
