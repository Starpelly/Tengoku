using System.Globalization;

namespace Trinkit
{
    public struct Vector2 : IEquatable<Vector2>, IFormattable
    {
        public const float kEpsilon = 0.00001f;
        public const float kEpsilonNormalSqrt = 1e-15f;

        /// <summary>
        /// X component of the vector.
        /// </summary>
        public float x;
        /// <summary>
        /// Y component of the vector.
        /// </summary>
        public float y;

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2 index!");
                }
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2 index!");
                }
            }
        }

        /// <summary>
        /// Constructs a new vector with the same x and y component.
        /// </summary>
        public Vector2(float xy) { this.x = xy; this.y = xy; }

        /// <summary>
        /// Constructs a new vector with x and y components.
        /// </summary>
        public Vector2(float x, float y) { this.x = x; this.y = y; }

        /// <summary>
        /// Lineraly interpolates between two vectors.
        /// </summary>
        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            t = Mathf.Abs(t);

            if (Single.IsNaN(b.x) || (Single.IsNaN(b.y))) b = a;

            return new Vector2(
                a.x + (b.x - a.x) * t,
                a.y + (b.y - a.y) * t
            );
        }

        /// <summary>
        /// Dot Product of two vectors.
        /// </summary>
        public static float Dot(Vector2 lhs, Vector2 rhs) { return lhs.x * rhs.x + lhs.y * rhs.y; }

        /// <summary>
        /// Returns the length of this vector.
        /// </summary>
        public float magnitude { get { return (float)Math.Sqrt(x * x + y * y); } }

        /// <summary>
        /// Returns the squared length of this vector.
        /// </summary>
        public float sqrMagnitude { get { return x * x + y * y; } }

        /// <summary>
        /// Returns the angle in degrees between (from) and (to).
        /// </summary>
        public static float Angle(Vector2 from, Vector2 to)
        {
            // sqrt(a) * sqrt(b) = sqrt(a * b) -- valid for real numbers
            float denominator = (float)Math.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
            if (denominator < kEpsilonNormalSqrt)
                return 0F;

            float dot = Mathf.Clamp(Dot(from, to) / denominator, -1F, 1F);
            return (float)Math.Acos(dot) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Returns the signed angle in degress between (from) and (to).
        /// Always returns the smallest possible angle.
        /// </summary>
        public static float SignedAngle(Vector2 from, Vector2 to)
        {
            float unsigned_angle = Angle(from, to);
            float sign = Mathf.Sign(from.x * to.y - from.y * to.x);
            return unsigned_angle * sign;
        }

        public static float Distance(Vector2 from, Vector2 to)
        {
            float ast = Angle(from, to);
            float abt = Mathf.Sign(from.x * to.y - from.y * to.x);
            return ast * abt;
        }


        /// <summary>
        /// Returns a copy of (vector) with its magnitude clamped to (maxLength).
        /// </summary>
        public static Vector2 ClampMagnitude(Vector2 vector, float maxLength)
        {
            float sqrMagnitude = vector.sqrMagnitude;
            if (sqrMagnitude > maxLength * maxLength)
            {
                float mag = (float)Math.Sqrt(sqrMagnitude);

                //these intermediate variables force the intermediate result to be
                //of float precision. without this, the intermediate result can be of higher
                //precision, which changes behavior.
                float normalized_x = vector.x / mag;
                float normalized_y = vector.y / mag;
                return new Vector2(normalized_x * maxLength,
                    normalized_y * maxLength);
            }
            return vector;
        }

        public bool Equals(Vector2 other)
        {
            return x == other.x && y == other.y;
        }

        public override string ToString()
        {
            return ((IFormattable)this).ToString(null, null);
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "F2";
            if (formatProvider == null)
                formatProvider = CultureInfo.InvariantCulture.NumberFormat;
            return String.Format("({0}, {1})", x.ToString(format, formatProvider), y.ToString(format, formatProvider));
        }

        public static Vector2 zero { get { return new Vector2(0, 0); } }
        public static Vector2 one { get { return new Vector2(1, 1); } }
        public static Vector2 up { get { return new Vector2(0f, 1f); } }
        public static Vector2 down { get { return new Vector2(0f, -1f); } }
        public static Vector2 left { get { return new Vector2(-1f, 0f); } }
        public static Vector2 right { get { return new Vector2(1f, 0f); } }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        public static Vector2 operator +(Vector2 a, Vector2 b) { return new Vector2(a.x + b.x, a.y + b.y); }
        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        public static Vector2 operator -(Vector2 a, Vector2 b) { return new Vector2(a.x - b.x, a.y - b.y); }
        /// <summary>
        /// Mulitplies two vectors.
        /// </summary>
        public static Vector2 operator *(Vector2 a, Vector2 b) { return new Vector2(a.x * b.x, a.y * b.y); }
        /// <summary>
        /// Divides two vectors.
        /// </summary>
        public static Vector2 operator /(Vector2 a, Vector2 b) { return new Vector2(a.x / b.x, a.y / b.y); }
        /// <summary>
        /// Negates a vector.
        /// </summary>
        public static Vector2 operator -(Vector2 a) { return new Vector2(-a.x, -a.y); }
        /// <summary>
        /// Adds a vector by a number.
        /// </summary>
        public static Vector2 operator +(Vector2 a, float d) { return new Vector2(a.x + d, a.y + d); }
        public static Vector2 operator +(float a, Vector2 d) { return new Vector2(a + d.x, a + d.y); }
        /// <summary>
        /// Subtracts a vector by a number.
        /// </summary>
        public static Vector2 operator -(Vector2 a, float d) { return new Vector2(a.x - d, a.y - d); }
        public static Vector2 operator -(float a, Vector2 d) { return new Vector2(a - d.x, a - d.y); }
        /// <summary>
        /// Multiplies a vector by a number.
        /// </summary>
        public static Vector2 operator *(Vector2 a, float d) { return new Vector2(a.x * d, a.y * d); }
        public static Vector2 operator *(float a, Vector2 d) { return new Vector2(a * d.x, a * d.y); }
        /// <summary>
        /// Divides a vector by a number.
        /// </summary>
        public static Vector2 operator /(Vector2 a, float d) { return new Vector2(a.x / d, a.y / d); }
        public static Vector2 operator /(float a, Vector2 d) { return new Vector2(a / d.x, a / d.y); }

        public static explicit operator Vector2(System.Numerics.Vector2 v)
        {
            return new Vector2(v.X, v.Y);
        }

        public static explicit operator Vector2(Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }

        /// <summary>
        /// Converts Trinkit Vector2 to System.Numerics Vector2
        /// </summary>
        /// <returns></returns>
        public System.Numerics.Vector2 ToNumerics()
        {
            return new System.Numerics.Vector2(x, y);
        }
    }
}
