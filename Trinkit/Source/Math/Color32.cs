using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Trinkit
{
    public struct Color32
    {
        private int rgba;

        /// <summary>
        /// Red component of the color.
        /// </summary>
        public byte r;
        /// <summary>
        /// Green component of the color.
        /// </summary>
        public byte g;
        /// <summary>
        /// Green component of the color.
        /// </summary>
        public byte b;
        /// <summary>
        /// Alpha component of the color.
        /// </summary>
        public byte a;

        /// <summary>
        /// Constructs a new Color with given r, g, b, a components.
        /// </summary>
        public Color32(byte r, byte g, byte b, byte a)
        {
            rgba = 0; this.r = r; this.g = g; this.b = b; this.a = a;
        }

        /// <summary>
        /// Color32 can be implicitly converted to and from [[Color]].
        /// </summary>
        public static implicit operator Color32(Color c)
        {
            return new Color32((byte)(Mathf.Round((Mathf.Clamp01(c.r) * 255f))),
                (byte)(Mathf.Round((Mathf.Clamp01(c.g) * 255f))),
                (byte)(Mathf.Round((Mathf.Clamp01(c.b) * 255f))),
                (byte)(Mathf.Round((Mathf.Clamp01(c.a) * 255f))));
        }

        /// <summary>
        /// Color32 can be implicitly converted to and from [[Color]].
        /// </summary>
        public static implicit operator Color(Color32 c)
        {
            return new Color(c.r / 255f, c.g / 255f, c.b / 255f, c.a / 255f);
        }

        /// <summary>
        /// Interpolates between colors /a/ and /b/ by /t/.
        /// </summary>
        public static Color32 Lerp(Color32 a, Color32 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Color32(
                (byte)(a.r + (b.r - a.r) * t),
                (byte)(a.g + (b.g - a.g) * t),
                (byte)(a.b + (b.b - a.b) * t),
                (byte)(a.a + (b.a - a.a) * t)
            );
        }

        /// <summary>
        /// Interpolates between colors /a/ and /b/ by /t/ without clamping the interpolant
        /// </summary>
        public static Color32 LerpUnclamped(Color32 a, Color32 b, float t)
        {
            return new Color32(
                (byte)(a.r + (b.r - a.r) * t),
                (byte)(a.g + (b.g - a.g) * t),
                (byte)(a.b + (b.b - a.b) * t),
                (byte)(a.a + (b.a - a.a) * t)
            );
        }

        /// <summary>
        /// Access the r, g, b,a components using [0], [1], [2], [3] respectively.
        /// </summary>
        public byte this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return r;
                    case 1: return g;
                    case 2: return b;
                    case 3: return a;
                    default:
                        throw new IndexOutOfRangeException("Invalid Color32 index(" + index + ")!");
                }
            }

            set
            {
                switch (index)
                {
                    case 0: r = value; break;
                    case 1: g = value; break;
                    case 2: b = value; break;
                    case 3: a = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Color32 index(" + index + ")!");
                }
            }
        }


        internal bool InternalEquals(Color32 other)
        {
            return rgba == other.rgba;
        }


        public override string ToString()
        {
            return ToString(null, null);
        }


        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
                formatProvider = CultureInfo.InvariantCulture.NumberFormat;
            return String.Format("RGBA({0}, {1}, {2}, {3})", r.ToString(format, formatProvider), g.ToString(format, formatProvider), b.ToString(format, formatProvider), a.ToString(format, formatProvider));
        }
    }
}
