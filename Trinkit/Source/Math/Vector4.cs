namespace Trinkit
{
    public struct Vector4 : IEquatable<Vector4>, IFormattable
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
        /// W component of the vector.
        /// </summary>
        public float w;

        /// <summary>
        /// Creates a new vector with given x, y, z, and w components.
        /// </summary>
        public Vector4(float x, float y, float z, float w) { this.x = x; this.y = y; this.z = z; this.w = w; }

        public override bool Equals(object? other)
        {
            if (!(other is Vector4)) return false;

            return Equals((Vector4)other);
        }

        public bool Equals(Vector4 other)
        {
            return x == other.x && y == other.y && z == other.z && w == other.w;
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
