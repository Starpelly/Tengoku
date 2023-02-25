using System.Globalization;
using System.Numerics;

namespace Trinkit
{
    public struct Color
    {
        /// <summary>
        /// Red component of the color.
        /// </summary>
        public float r;
        /// <summary>
        /// Green component of the color.
        /// </summary>
        public float g;
        /// <summary>
        /// Green component of the color.
        /// </summary>
        public float b;
        /// <summary>
        /// Alpha component of the color.
        /// </summary>
        public float a;

        /// <summary>
        /// Constructs a new Color with given r, g, b, and a components.
        /// </summary>
        public Color(float r, float g, float b, float a)
        {
            this.r = r; this.g = g; this.b = b; this.a = a;
        }

        /// <summary>
        /// Constructs a new Color with given r, g, b components and sets a to 1.
        /// </summary>
        public Color(float r, float g, float b)
        {
            this.r = r; this.g = g; this.b = b; this.a = 1.0f;
        }

        /// <summary>
        /// Constructs a new Color with given HEX
        /// </summary>
        public Color(string hex)
        {
            hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
            hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
            byte a = 255;//assume fully visible unless specified in hex
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            //Only use alpha if the string has enough characters
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            var col = (Color)new Color32(r, g, b, a);
            this.r = col.r;
            this.g = col.g;
            this.b = col.b;
            this.a = col.a;
        }

        /// <summary>
        /// Convert a color from RGB to HSV color space.
        /// </summary>
        public static void RGBToHSV(Color rgbCol, out float H, out float S, out float V)
        {
            // when blue is highest valued
            if ((rgbCol.b > rgbCol.g) && (rgbCol.b > rgbCol.r))
                RGBToHSVHelper((float)4, rgbCol.b, rgbCol.r, rgbCol.g, out H, out S, out V);
            //when green is highest valued
            else if (rgbCol.g > rgbCol.r)
                RGBToHSVHelper((float)2, rgbCol.g, rgbCol.b, rgbCol.r, out H, out S, out V);
            //when red is highest valued
            else
                RGBToHSVHelper((float)0, rgbCol.r, rgbCol.g, rgbCol.b, out H, out S, out V);
        }

        private static void RGBToHSVHelper(float offset, float dominantcolor, float colorone, float colortwo, out float H, out float S, out float V)
        {
            V = dominantcolor;
            //we need to find out which is the minimum color
            if (V != 0)
            {
                //we check which color is smallest
                float small = 0;
                if (colorone > colortwo) small = colortwo;
                else small = colorone;

                float diff = V - small;

                //if the two values are not the same, we compute the like this
                if (diff != 0)
                {
                    //S = max-min/max
                    S = diff / V;
                    //H = hue is offset by X, and is the difference between the two smallest colors
                    H = offset + ((colorone - colortwo) / diff);
                }
                else
                {
                    //S = 0 when the difference is zero
                    S = 0;
                    //H = 4 + (R-G) hue is offset by 4 when blue, and is the difference between the two smallest colors
                    H = offset + (colorone - colortwo);
                }

                H /= 6;

                //conversion values
                if (H < 0)
                    H += 1.0f;
            }
            else
            {
                S = 0;
                H = 0;
            }
        }

        public static Color HSVToRGB(float H, float S, float V)
        {
            return HSVToRGB(H, S, V, true);
        }

        /// <summary>
        /// Convert a set of HSV values to an RGB Color.
        /// </summary>
        public static Color HSVToRGB(float H, float S, float V, bool hdr)
        {
            Color retval = Color.white;
            if (S == 0)
            {
                retval.r = V;
                retval.g = V;
                retval.b = V;
            }
            else if (V == 0)
            {
                retval.r = 0;
                retval.g = 0;
                retval.b = 0;
            }
            else
            {
                retval.r = 0;
                retval.g = 0;
                retval.b = 0;

                //crazy hsv conversion
                float t_S, t_V, h_to_floor;

                t_S = S;
                t_V = V;
                h_to_floor = H * 6.0f;

                int temp = (int)Mathf.Floor(h_to_floor);
                float t = h_to_floor - ((float)temp);
                float var_1 = (t_V) * (1 - t_S);
                float var_2 = t_V * (1 - t_S * t);
                float var_3 = t_V * (1 - t_S * (1 - t));

                switch (temp)
                {
                    case 0:
                        retval.r = t_V;
                        retval.g = var_3;
                        retval.b = var_1;
                        break;

                    case 1:
                        retval.r = var_2;
                        retval.g = t_V;
                        retval.b = var_1;
                        break;

                    case 2:
                        retval.r = var_1;
                        retval.g = t_V;
                        retval.b = var_3;
                        break;

                    case 3:
                        retval.r = var_1;
                        retval.g = var_2;
                        retval.b = t_V;
                        break;

                    case 4:
                        retval.r = var_3;
                        retval.g = var_1;
                        retval.b = t_V;
                        break;

                    case 5:
                        retval.r = t_V;
                        retval.g = var_1;
                        retval.b = var_2;
                        break;

                    case 6:
                        retval.r = t_V;
                        retval.g = var_3;
                        retval.b = var_1;
                        break;

                    case -1:
                        retval.r = t_V;
                        retval.g = var_1;
                        retval.b = var_2;
                        break;
                }

                if (!hdr)
                {
                    retval.r = Mathf.Clamp(retval.r, 0.0f, 1.0f);
                    retval.g = Mathf.Clamp(retval.g, 0.0f, 1.0f);
                    retval.b = Mathf.Clamp(retval.b, 0.0f, 1.0f);
                }
            }
            return retval;
        }

        public override string ToString()
        {
            return ToString(null, null);
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "F3";
            if (formatProvider == null)
                formatProvider = CultureInfo.InvariantCulture.NumberFormat;
            return String.Format("RGBA({0}, {1}, {2}, {3})", r.ToString(format, formatProvider), g.ToString(format, formatProvider), b.ToString(format, formatProvider), a.ToString(format, formatProvider));
        }

        /// <summary>
        /// Interpolates between colors a and b by t.
        /// </summary>
        public static Color Lerp(Color a, Color b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Color(
                 a.r + (b.r - a.r) * t,
                 a.g + (b.g - a.g) * t,
                 a.b + (b.b - a.b) * t,
                 a.a + (b.a - a.a) * t
             );
        }

        public static implicit operator Raylib_CsLo.Color(Color c)
        {
            var co = (Color32)c;
            return new Raylib_CsLo.Color(
                co.r,
                co.g,
                co.b,
                co.a);
        }

        public static implicit operator System.Numerics.Vector4(Color c)
        {
            return new System.Numerics.Vector4(
                c.r,
                c.g,
                c.b,
                c.a);
        }

        public static implicit operator uint(Color c)
        {
            var c32 = (Color32)c;
            return (uint)(((int)c32.a << 24) | ((int)c32.b << 16) |
                          ((int)c32.g << 8) | ((int)c32.r << 0));
        }

        public static implicit operator Color(Vector4 c)
        {
            return new Color(c.x, c.y, c.z, c.w);
        }

        public static Color ChangeAlpha(Color c, float a)
        {
            return new Color(c.r, c.g, c.b, a);
        }

        /// <summary>
        /// Transparent Black. RGBA is (0, 0, 0, 0).
        /// </summary>
        public static Color transparent { get { return new Color(0f, 0f, 0f, 0f); } }
        /// <summary>
        /// Transparent White. RGBA is (1, 1, 1, 0).
        /// </summary>
        public static Color transparentWhite { get { return new Color(1f, 1f, 1f, 0f); } }
        /// <summary>
        /// Alice Blue. RGBA is (0.9411765, 0.972549, 1, 1).
        /// </summary>
        public static Color aliceBlue { get { return new Color(0.9411765f, 0.972549f, 1f, 1f); } }
        /// <summary>
        /// Antique White. RGBA is (0.98039216, 0.92156863, 0.84313726, 1).
        /// </summary>
        public static Color antiqueWhite { get { return new Color(0.98039216f, 0.92156863f, 0.84313726f, 1f); } }
        /// <summary>
        /// Aqua. RGBA is (0, 1, 1, 1).
        /// </summary>
        public static Color aqua { get { return new Color(0f, 1f, 1f, 1f); } }
        /// <summary>
        /// Aquamarine. RGBA is (0.49803922, 1, 0.83137256, 1).
        /// </summary>
        public static Color aquamarine { get { return new Color(0.49803922f, 1f, 0.83137256f, 1f); } }
        /// <summary>
        /// Azure. RGBA is (0.9411765, 1, 1, 1).
        /// </summary>
        public static Color azure { get { return new Color(0.9411765f, 1f, 1f, 1f); } }
        /// <summary>
        /// Beige. RGBA is (0.9607843, 0.9607843, 0.8627451, 1).
        /// </summary>
        public static Color beige { get { return new Color(0.9607843f, 0.9607843f, 0.8627451f, 1f); } }
        /// <summary>
        /// Bisque. RGBA is (1, 0.89411765, 0.76862746, 1).
        /// </summary>
        public static Color bisque { get { return new Color(1f, 0.89411765f, 0.76862746f, 1f); } }
        /// <summary>
        /// Black. RGBA is (0, 0, 0, 1).
        /// </summary>
        public static Color black { get { return new Color(0f, 0f, 0f, 1f); } }
        /// <summary>
        /// Blanched Almond. RGBA is (1, 0.92156863, 0.8039216, 1).
        /// </summary>
        public static Color blanchedAlmond { get { return new Color(1f, 0.92156863f, 0.8039216f, 1f); } }
        /// <summary>
        /// Blue. RGBA is (0, 0, 1, 1).
        /// </summary>
        public static Color blue { get { return new Color(0f, 0f, 1f, 1f); } }
        /// <summary>
        /// Blue Violet. RGBA is (0.5411765, 0.16862746, 0.8862745, 1).
        /// </summary>
        public static Color blueViolet { get { return new Color(0.5411765f, 0.16862746f, 0.8862745f, 1f); } }
        /// <summary>
        /// Brown. RGBA is (0.64705884, 0.16470589, 0.16470589, 1).
        /// </summary>
        public static Color brown { get { return new Color(0.64705884f, 0.16470589f, 0.16470589f, 1f); } }
        /// <summary>
        /// Burly Wood. RGBA is (0.87058824, 0.72156864, 0.5294118, 1).
        /// </summary>
        public static Color burlyWood { get { return new Color(0.87058824f, 0.72156864f, 0.5294118f, 1f); } }
        /// <summary>
        /// Cadet Blue. RGBA is (0.37254903, 0.61960787, 0.627451, 1).
        /// </summary>
        public static Color cadetBlue { get { return new Color(0.37254903f, 0.61960787f, 0.627451f, 1f); } }
        /// <summary>
        /// Chartreuse. RGBA is (0.49803922, 1, 0, 1).
        /// </summary>
        public static Color chartreuse { get { return new Color(0.49803922f, 1f, 0f, 1f); } }
        /// <summary>
        /// Chocolate. RGBA is (0.8235294, 0.4117647, 0.11764706, 1).
        /// </summary>
        public static Color chocolate { get { return new Color(0.8235294f, 0.4117647f, 0.11764706f, 1f); } }
        /// <summary>
        /// Coral. RGBA is (1, 0.49803922, 0.3137255, 1).
        /// </summary>
        public static Color coral { get { return new Color(1f, 0.49803922f, 0.3137255f, 1f); } }
        /// <summary>
        /// Cornflower Blue. RGBA is (0.39215687, 0.58431375, 0.92941177, 1).
        /// </summary>
        public static Color cornflowerBlue { get { return new Color(0.39215687f, 0.58431375f, 0.92941177f, 1f); } }
        /// <summary>
        /// Cornsilk. RGBA is (1, 0.972549, 0.8627451, 1).
        /// </summary>
        public static Color cornsilk { get { return new Color(1f, 0.972549f, 0.8627451f, 1f); } }
        /// <summary>
        /// Crimson. RGBA is (0.8627451, 0.078431375, 0.23529412, 1).
        /// </summary>
        public static Color crimson { get { return new Color(0.8627451f, 0.078431375f, 0.23529412f, 1f); } }
        /// <summary>
        /// Cyan. RGBA is (0, 1, 1, 1).
        /// </summary>
        public static Color cyan { get { return new Color(0f, 1f, 1f, 1f); } }
        /// <summary>
        /// Dark Blue. RGBA is (0, 0, 0.54509807, 1).
        /// </summary>
        public static Color darkBlue { get { return new Color(0f, 0f, 0.54509807f, 1f); } }
        /// <summary>
        /// Dark Cyan. RGBA is (0, 0.54509807, 0.54509807, 1).
        /// </summary>
        public static Color darkCyan { get { return new Color(0f, 0.54509807f, 0.54509807f, 1f); } }
        /// <summary>
        /// Dark Goldenrod. RGBA is (0.72156864, 0.5254902, 0.043137256, 1).
        /// </summary>
        public static Color darkGoldenrod { get { return new Color(0.72156864f, 0.5254902f, 0.043137256f, 1f); } }
        /// <summary>
        /// Dark Gray. RGBA is (0.6627451, 0.6627451, 0.6627451, 1).
        /// </summary>
        public static Color darkGray { get { return new Color(0.6627451f, 0.6627451f, 0.6627451f, 1f); } }
        /// <summary>
        /// Dark Green. RGBA is (0, 0.39215687, 0, 1).
        /// </summary>
        public static Color darkGreen { get { return new Color(0f, 0.39215687f, 0f, 1f); } }
        /// <summary>
        /// Dark Khaki. RGBA is (0.7411765, 0.7176471, 0.41960785, 1).
        /// </summary>
        public static Color darkKhaki { get { return new Color(0.7411765f, 0.7176471f, 0.41960785f, 1f); } }
        /// <summary>
        /// Dark Magenta. RGBA is (0.54509807, 0, 0.54509807, 1).
        /// </summary>
        public static Color darkMagenta { get { return new Color(0.54509807f, 0f, 0.54509807f, 1f); } }
        /// <summary>
        /// Dark Olive Green. RGBA is (0.33333334, 0.41960785, 0.18431373, 1).
        /// </summary>
        public static Color darkOliveGreen { get { return new Color(0.33333334f, 0.41960785f, 0.18431373f, 1f); } }
        /// <summary>
        /// Dark Orange. RGBA is (1, 0.54901963, 0, 1).
        /// </summary>
        public static Color darkOrange { get { return new Color(1f, 0.54901963f, 0f, 1f); } }
        /// <summary>
        /// Dark Orchid. RGBA is (0.6, 0.19607843, 0.8, 1).
        /// </summary>
        public static Color darkOrchid { get { return new Color(0.6f, 0.19607843f, 0.8f, 1f); } }
        /// <summary>
        /// Dark Red. RGBA is (0.54509807, 0, 0, 1).
        /// </summary>
        public static Color darkRed { get { return new Color(0.54509807f, 0f, 0f, 1f); } }
        /// <summary>
        /// Dark Salmon. RGBA is (0.9137255, 0.5882353, 0.47843137, 1).
        /// </summary>
        public static Color darkSalmon { get { return new Color(0.9137255f, 0.5882353f, 0.47843137f, 1f); } }
        /// <summary>
        /// Dark Sea Green. RGBA is (0.56078434, 0.7372549, 0.54509807, 1).
        /// </summary>
        public static Color darkSeaGreen { get { return new Color(0.56078434f, 0.7372549f, 0.54509807f, 1f); } }
        /// <summary>
        /// Dark Slate Blue. RGBA is (0.28235295, 0.23921569, 0.54509807, 1).
        /// </summary>
        public static Color darkSlateBlue { get { return new Color(0.28235295f, 0.23921569f, 0.54509807f, 1f); } }
        /// <summary>
        /// Dark Slate Gray. RGBA is (0.18431373, 0.30980393, 0.30980393, 1).
        /// </summary>
        public static Color darkSlateGray { get { return new Color(0.18431373f, 0.30980393f, 0.30980393f, 1f); } }
        /// <summary>
        /// Dark Turquoise. RGBA is (0, 0.80784315, 0.81960785, 1).
        /// </summary>
        public static Color darkTurquoise { get { return new Color(0f, 0.80784315f, 0.81960785f, 1f); } }
        /// <summary>
        /// Dark Violet. RGBA is (0.5803922, 0, 0.827451, 1).
        /// </summary>
        public static Color darkViolet { get { return new Color(0.5803922f, 0f, 0.827451f, 1f); } }
        /// <summary>
        /// Deep Pink. RGBA is (1, 0.078431375, 0.5764706, 1).
        /// </summary>
        public static Color deepPink { get { return new Color(1f, 0.078431375f, 0.5764706f, 1f); } }
        /// <summary>
        /// Deep Sky Blue. RGBA is (0, 0.7490196, 1, 1).
        /// </summary>
        public static Color deepSkyBlue { get { return new Color(0f, 0.7490196f, 1f, 1f); } }
        /// <summary>
        /// Dim Gray. RGBA is (0.4117647, 0.4117647, 0.4117647, 1).
        /// </summary>
        public static Color dimGray { get { return new Color(0.4117647f, 0.4117647f, 0.4117647f, 1f); } }
        /// <summary>
        /// Dodger Blue. RGBA is (0.11764706, 0.5647059, 1, 1).
        /// </summary>
        public static Color dodgerBlue { get { return new Color(0.11764706f, 0.5647059f, 1f, 1f); } }
        /// <summary>
        /// Firebrick. RGBA is (0.69803923, 0.13333334, 0.13333334, 1).
        /// </summary>
        public static Color firebrick { get { return new Color(0.69803923f, 0.13333334f, 0.13333334f, 1f); } }
        /// <summary>
        /// Floral White. RGBA is (1, 0.98039216, 0.9411765, 1).
        /// </summary>
        public static Color floralWhite { get { return new Color(1f, 0.98039216f, 0.9411765f, 1f); } }
        /// <summary>
        /// Forest Green. RGBA is (0.13333334, 0.54509807, 0.13333334, 1).
        /// </summary>
        public static Color forestGreen { get { return new Color(0.13333334f, 0.54509807f, 0.13333334f, 1f); } }
        /// <summary>
        /// Fuchsia. RGBA is (1, 0, 1, 1).
        /// </summary>
        public static Color fuchsia { get { return new Color(1f, 0f, 1f, 1f); } }
        /// <summary>
        /// Gainsboro. RGBA is (0.8627451, 0.8627451, 0.8627451, 1).
        /// </summary>
        public static Color gainsboro { get { return new Color(0.8627451f, 0.8627451f, 0.8627451f, 1f); } }
        /// <summary>
        /// Ghost White. RGBA is (0.972549, 0.972549, 1, 1).
        /// </summary>
        public static Color ghostWhite { get { return new Color(0.972549f, 0.972549f, 1f, 1f); } }
        /// <summary>
        /// Gold. RGBA is (1, 0.84313726, 0, 1).
        /// </summary>
        public static Color gold { get { return new Color(1f, 0.84313726f, 0f, 1f); } }
        /// <summary>
        /// Goldenrod. RGBA is (0.85490197, 0.64705884, 0.1254902, 1).
        /// </summary>
        public static Color goldenrod { get { return new Color(0.85490197f, 0.64705884f, 0.1254902f, 1f); } }
        /// <summary>
        /// Gray. RGBA is (0.5019608, 0.5019608, 0.5019608, 1).
        /// </summary>
        public static Color gray { get { return new Color(0.5019608f, 0.5019608f, 0.5019608f, 1f); } }
        /// <summary>
        /// Green. RGBA is (0, 0.5019608, 0, 1).
        /// </summary>
        public static Color green { get { return new Color(0f, 0.5019608f, 0f, 1f); } }
        /// <summary>
        /// Green Yellow. RGBA is (0.6784314, 1, 0.18431373, 1).
        /// </summary>
        public static Color greenYellow { get { return new Color(0.6784314f, 1f, 0.18431373f, 1f); } }
        /// <summary>
        /// Honeydew. RGBA is (0.9411765, 1, 0.9411765, 1).
        /// </summary>
        public static Color honeydew { get { return new Color(0.9411765f, 1f, 0.9411765f, 1f); } }
        /// <summary>
        /// Hot Pink. RGBA is (1, 0.4117647, 0.7058824, 1).
        /// </summary>
        public static Color hotPink { get { return new Color(1f, 0.4117647f, 0.7058824f, 1f); } }
        /// <summary>
        /// Indian Red. RGBA is (0.8039216, 0.36078432, 0.36078432, 1).
        /// </summary>
        public static Color indianRed { get { return new Color(0.8039216f, 0.36078432f, 0.36078432f, 1f); } }
        /// <summary>
        /// Indigo. RGBA is (0.29411766, 0, 0.50980395, 1).
        /// </summary>
        public static Color indigo { get { return new Color(0.29411766f, 0f, 0.50980395f, 1f); } }
        /// <summary>
        /// Ivory. RGBA is (1, 1, 0.9411765, 1).
        /// </summary>
        public static Color ivory { get { return new Color(1f, 1f, 0.9411765f, 1f); } }
        /// <summary>
        /// Khaki. RGBA is (0.9411765, 0.9019608, 0.54901963, 1).
        /// </summary>
        public static Color khaki { get { return new Color(0.9411765f, 0.9019608f, 0.54901963f, 1f); } }
        /// <summary>
        /// Lavender. RGBA is (0.9019608, 0.9019608, 0.98039216, 1).
        /// </summary>
        public static Color lavender { get { return new Color(0.9019608f, 0.9019608f, 0.98039216f, 1f); } }
        /// <summary>
        /// Lavender Blush. RGBA is (1, 0.9411765, 0.9607843, 1).
        /// </summary>
        public static Color lavenderBlush { get { return new Color(1f, 0.9411765f, 0.9607843f, 1f); } }
        /// <summary>
        /// Lawn Green. RGBA is (0.4862745, 0.9882353, 0, 1).
        /// </summary>
        public static Color lawnGreen { get { return new Color(0.4862745f, 0.9882353f, 0f, 1f); } }
        /// <summary>
        /// Lemon Chiffon. RGBA is (1, 0.98039216, 0.8039216, 1).
        /// </summary>
        public static Color lemonChiffon { get { return new Color(1f, 0.98039216f, 0.8039216f, 1f); } }
        /// <summary>
        /// Light Blue. RGBA is (0.6784314, 0.84705883, 0.9019608, 1).
        /// </summary>
        public static Color lightBlue { get { return new Color(0.6784314f, 0.84705883f, 0.9019608f, 1f); } }
        /// <summary>
        /// Light Coral. RGBA is (0.9411765, 0.5019608, 0.5019608, 1).
        /// </summary>
        public static Color lightCoral { get { return new Color(0.9411765f, 0.5019608f, 0.5019608f, 1f); } }
        /// <summary>
        /// Light Cyan. RGBA is (0.8784314, 1, 1, 1).
        /// </summary>
        public static Color lightCyan { get { return new Color(0.8784314f, 1f, 1f, 1f); } }
        /// <summary>
        /// Light Goldenrod Yellow. RGBA is (0.98039216, 0.98039216, 0.8235294, 1).
        /// </summary>
        public static Color lightGoldenrodYellow { get { return new Color(0.98039216f, 0.98039216f, 0.8235294f, 1f); } }
        /// <summary>
        /// Light Gray. RGBA is (0.827451, 0.827451, 0.827451, 1).
        /// </summary>
        public static Color lightGray { get { return new Color(0.827451f, 0.827451f, 0.827451f, 1f); } }
        /// <summary>
        /// Light Green. RGBA is (0.5647059, 0.93333334, 0.5647059, 1).
        /// </summary>
        public static Color lightGreen { get { return new Color(0.5647059f, 0.93333334f, 0.5647059f, 1f); } }
        /// <summary>
        /// Light Pink. RGBA is (1, 0.7137255, 0.75686276, 1).
        /// </summary>
        public static Color lightPink { get { return new Color(1f, 0.7137255f, 0.75686276f, 1f); } }
        /// <summary>
        /// Light Salmon. RGBA is (1, 0.627451, 0.47843137, 1).
        /// </summary>
        public static Color lightSalmon { get { return new Color(1f, 0.627451f, 0.47843137f, 1f); } }
        /// <summary>
        /// Light Sea Green. RGBA is (0.1254902, 0.69803923, 0.6666667, 1).
        /// </summary>
        public static Color lightSeaGreen { get { return new Color(0.1254902f, 0.69803923f, 0.6666667f, 1f); } }
        /// <summary>
        /// Light Sky Blue. RGBA is (0.5294118, 0.80784315, 0.98039216, 1).
        /// </summary>
        public static Color lightSkyBlue { get { return new Color(0.5294118f, 0.80784315f, 0.98039216f, 1f); } }
        /// <summary>
        /// Light Slate Gray. RGBA is (0.46666667, 0.53333336, 0.6, 1).
        /// </summary>
        public static Color lightSlateGray { get { return new Color(0.46666667f, 0.53333336f, 0.6f, 1f); } }
        /// <summary>
        /// Light Steel Blue. RGBA is (0.6901961, 0.76862746, 0.87058824, 1).
        /// </summary>
        public static Color lightSteelBlue { get { return new Color(0.6901961f, 0.76862746f, 0.87058824f, 1f); } }
        /// <summary>
        /// Light Yellow. RGBA is (1, 1, 0.8784314, 1).
        /// </summary>
        public static Color lightYellow { get { return new Color(1f, 1f, 0.8784314f, 1f); } }
        /// <summary>
        /// Lime. RGBA is (0, 1, 0, 1).
        /// </summary>
        public static Color lime { get { return new Color(0f, 1f, 0f, 1f); } }
        /// <summary>
        /// Lime Green. RGBA is (0.19607843, 0.8039216, 0.19607843, 1).
        /// </summary>
        public static Color limeGreen { get { return new Color(0.19607843f, 0.8039216f, 0.19607843f, 1f); } }
        /// <summary>
        /// Linen. RGBA is (0.98039216, 0.9411765, 0.9019608, 1).
        /// </summary>
        public static Color linen { get { return new Color(0.98039216f, 0.9411765f, 0.9019608f, 1f); } }
        /// <summary>
        /// Magenta. RGBA is (1, 0, 1, 1).
        /// </summary>
        public static Color magenta { get { return new Color(1f, 0f, 1f, 1f); } }
        /// <summary>
        /// Maroon. RGBA is (0.5019608, 0, 0, 1).
        /// </summary>
        public static Color maroon { get { return new Color(0.5019608f, 0f, 0f, 1f); } }
        /// <summary>
        /// Medium Aquamarine. RGBA is (0.4, 0.8039216, 0.6666667, 1).
        /// </summary>
        public static Color mediumAquamarine { get { return new Color(0.4f, 0.8039216f, 0.6666667f, 1f); } }
        /// <summary>
        /// Medium Blue. RGBA is (0, 0, 0.8039216, 1).
        /// </summary>
        public static Color mediumBlue { get { return new Color(0f, 0f, 0.8039216f, 1f); } }
        /// <summary>
        /// Medium Orchid. RGBA is (0.7294118, 0.33333334, 0.827451, 1).
        /// </summary>
        public static Color mediumOrchid { get { return new Color(0.7294118f, 0.33333334f, 0.827451f, 1f); } }
        /// <summary>
        /// Medium Purple. RGBA is (0.5764706, 0.4392157, 0.85882354, 1).
        /// </summary>
        public static Color mediumPurple { get { return new Color(0.5764706f, 0.4392157f, 0.85882354f, 1f); } }
        /// <summary>
        /// Medium Sea Green. RGBA is (0.23529412, 0.7019608, 0.44313726, 1).
        /// </summary>
        public static Color mediumSeaGreen { get { return new Color(0.23529412f, 0.7019608f, 0.44313726f, 1f); } }
        /// <summary>
        /// Medium Slate Blue. RGBA is (0.48235294, 0.40784314, 0.93333334, 1).
        /// </summary>
        public static Color mediumSlateBlue { get { return new Color(0.48235294f, 0.40784314f, 0.93333334f, 1f); } }
        /// <summary>
        /// Medium Spring Green. RGBA is (0, 0.98039216, 0.6039216, 1).
        /// </summary>
        public static Color mediumSpringGreen { get { return new Color(0f, 0.98039216f, 0.6039216f, 1f); } }
        /// <summary>
        /// Medium Turquoise. RGBA is (0.28235295, 0.81960785, 0.8, 1).
        /// </summary>
        public static Color mediumTurquoise { get { return new Color(0.28235295f, 0.81960785f, 0.8f, 1f); } }
        /// <summary>
        /// Medium Violet Red. RGBA is (0.78039217, 0.08235294, 0.52156866, 1).
        /// </summary>
        public static Color mediumVioletRed { get { return new Color(0.78039217f, 0.08235294f, 0.52156866f, 1f); } }
        /// <summary>
        /// Midnight Blue. RGBA is (0.09803922, 0.09803922, 0.4392157, 1).
        /// </summary>
        public static Color midnightBlue { get { return new Color(0.09803922f, 0.09803922f, 0.4392157f, 1f); } }
        /// <summary>
        /// Mint Cream. RGBA is (0.9607843, 1, 0.98039216, 1).
        /// </summary>
        public static Color mintCream { get { return new Color(0.9607843f, 1f, 0.98039216f, 1f); } }
        /// <summary>
        /// Misty Rose. RGBA is (1, 0.89411765, 0.88235295, 1).
        /// </summary>
        public static Color mistyRose { get { return new Color(1f, 0.89411765f, 0.88235295f, 1f); } }
        /// <summary>
        /// Moccasin. RGBA is (1, 0.89411765, 0.70980394, 1).
        /// </summary>
        public static Color moccasin { get { return new Color(1f, 0.89411765f, 0.70980394f, 1f); } }
        /// <summary>
        /// Mono Game Orange. RGBA is (0.90588236, 0.23529412, 0, 1).
        /// </summary>
        public static Color monoGameOrange { get { return new Color(0.90588236f, 0.23529412f, 0f, 1f); } }
        /// <summary>
        /// Navajo White. RGBA is (1, 0.87058824, 0.6784314, 1).
        /// </summary>
        public static Color navajoWhite { get { return new Color(1f, 0.87058824f, 0.6784314f, 1f); } }
        /// <summary>
        /// Navy. RGBA is (0, 0, 0.5019608, 1).
        /// </summary>
        public static Color navy { get { return new Color(0f, 0f, 0.5019608f, 1f); } }
        /// <summary>
        /// Old Lace. RGBA is (0.99215686, 0.9607843, 0.9019608, 1).
        /// </summary>
        public static Color oldLace { get { return new Color(0.99215686f, 0.9607843f, 0.9019608f, 1f); } }
        /// <summary>
        /// Olive. RGBA is (0.5019608, 0.5019608, 0, 1).
        /// </summary>
        public static Color olive { get { return new Color(0.5019608f, 0.5019608f, 0f, 1f); } }
        /// <summary>
        /// Olive Drab. RGBA is (0.41960785, 0.5568628, 0.13725491, 1).
        /// </summary>
        public static Color oliveDrab { get { return new Color(0.41960785f, 0.5568628f, 0.13725491f, 1f); } }
        /// <summary>
        /// Orange. RGBA is (1, 0.64705884, 0, 1).
        /// </summary>
        public static Color orange { get { return new Color(1f, 0.64705884f, 0f, 1f); } }
        /// <summary>
        /// Orange Red. RGBA is (1, 0.27058825, 0, 1).
        /// </summary>
        public static Color orangeRed { get { return new Color(1f, 0.27058825f, 0f, 1f); } }
        /// <summary>
        /// Orchid. RGBA is (0.85490197, 0.4392157, 0.8392157, 1).
        /// </summary>
        public static Color orchid { get { return new Color(0.85490197f, 0.4392157f, 0.8392157f, 1f); } }
        /// <summary>
        /// Pale Goldenrod. RGBA is (0.93333334, 0.9098039, 0.6666667, 1).
        /// </summary>
        public static Color paleGoldenrod { get { return new Color(0.93333334f, 0.9098039f, 0.6666667f, 1f); } }
        /// <summary>
        /// Pale Green. RGBA is (0.59607846, 0.9843137, 0.59607846, 1).
        /// </summary>
        public static Color paleGreen { get { return new Color(0.59607846f, 0.9843137f, 0.59607846f, 1f); } }
        /// <summary>
        /// Pale Turquoise. RGBA is (0.6862745, 0.93333334, 0.93333334, 1).
        /// </summary>
        public static Color paleTurquoise { get { return new Color(0.6862745f, 0.93333334f, 0.93333334f, 1f); } }
        /// <summary>
        /// Pale Violet Red. RGBA is (0.85882354, 0.4392157, 0.5764706, 1).
        /// </summary>
        public static Color paleVioletRed { get { return new Color(0.85882354f, 0.4392157f, 0.5764706f, 1f); } }
        /// <summary>
        /// Papaya Whip. RGBA is (1, 0.9372549, 0.8352941, 1).
        /// </summary>
        public static Color papayaWhip { get { return new Color(1f, 0.9372549f, 0.8352941f, 1f); } }
        /// <summary>
        /// Peach Puff. RGBA is (1, 0.85490197, 0.7254902, 1).
        /// </summary>
        public static Color peachPuff { get { return new Color(1f, 0.85490197f, 0.7254902f, 1f); } }
        /// <summary>
        /// Peru. RGBA is (0.8039216, 0.52156866, 0.24705882, 1).
        /// </summary>
        public static Color peru { get { return new Color(0.8039216f, 0.52156866f, 0.24705882f, 1f); } }
        /// <summary>
        /// Pink. RGBA is (1, 0.7529412, 0.79607844, 1).
        /// </summary>
        public static Color pink { get { return new Color(1f, 0.7529412f, 0.79607844f, 1f); } }
        /// <summary>
        /// Plum. RGBA is (0.8666667, 0.627451, 0.8666667, 1).
        /// </summary>
        public static Color plum { get { return new Color(0.8666667f, 0.627451f, 0.8666667f, 1f); } }
        /// <summary>
        /// Powder Blue. RGBA is (0.6901961, 0.8784314, 0.9019608, 1).
        /// </summary>
        public static Color powderBlue { get { return new Color(0.6901961f, 0.8784314f, 0.9019608f, 1f); } }
        /// <summary>
        /// Purple. RGBA is (0.5019608, 0, 0.5019608, 1).
        /// </summary>
        public static Color purple { get { return new Color(0.5019608f, 0f, 0.5019608f, 1f); } }
        /// <summary>
        /// Red. RGBA is (1, 0, 0, 1).
        /// </summary>
        public static Color red { get { return new Color(1f, 0f, 0f, 1f); } }
        /// <summary>
        /// Rosy Brown. RGBA is (0.7372549, 0.56078434, 0.56078434, 1).
        /// </summary>
        public static Color rosyBrown { get { return new Color(0.7372549f, 0.56078434f, 0.56078434f, 1f); } }
        /// <summary>
        /// Royal Blue. RGBA is (0.25490198, 0.4117647, 0.88235295, 1).
        /// </summary>
        public static Color royalBlue { get { return new Color(0.25490198f, 0.4117647f, 0.88235295f, 1f); } }
        /// <summary>
        /// Saddle Brown. RGBA is (0.54509807, 0.27058825, 0.07450981, 1).
        /// </summary>
        public static Color saddleBrown { get { return new Color(0.54509807f, 0.27058825f, 0.07450981f, 1f); } }
        /// <summary>
        /// Salmon. RGBA is (0.98039216, 0.5019608, 0.44705883, 1).
        /// </summary>
        public static Color salmon { get { return new Color(0.98039216f, 0.5019608f, 0.44705883f, 1f); } }
        /// <summary>
        /// Sandy Brown. RGBA is (0.95686275, 0.6431373, 0.3764706, 1).
        /// </summary>
        public static Color sandyBrown { get { return new Color(0.95686275f, 0.6431373f, 0.3764706f, 1f); } }
        /// <summary>
        /// Sea Green. RGBA is (0.18039216, 0.54509807, 0.34117648, 1).
        /// </summary>
        public static Color seaGreen { get { return new Color(0.18039216f, 0.54509807f, 0.34117648f, 1f); } }
        /// <summary>
        /// Sea Shell. RGBA is (1, 0.9607843, 0.93333334, 1).
        /// </summary>
        public static Color seaShell { get { return new Color(1f, 0.9607843f, 0.93333334f, 1f); } }
        /// <summary>
        /// Sienna. RGBA is (0.627451, 0.32156864, 0.1764706, 1).
        /// </summary>
        public static Color sienna { get { return new Color(0.627451f, 0.32156864f, 0.1764706f, 1f); } }
        /// <summary>
        /// Silver. RGBA is (0.7529412, 0.7529412, 0.7529412, 1).
        /// </summary>
        public static Color silver { get { return new Color(0.7529412f, 0.7529412f, 0.7529412f, 1f); } }
        /// <summary>
        /// Sky Blue. RGBA is (0.5294118, 0.80784315, 0.92156863, 1).
        /// </summary>
        public static Color skyBlue { get { return new Color(0.5294118f, 0.80784315f, 0.92156863f, 1f); } }
        /// <summary>
        /// Slate Blue. RGBA is (0.41568628, 0.3529412, 0.8039216, 1).
        /// </summary>
        public static Color slateBlue { get { return new Color(0.41568628f, 0.3529412f, 0.8039216f, 1f); } }
        /// <summary>
        /// Slate Gray. RGBA is (0.4392157, 0.5019608, 0.5647059, 1).
        /// </summary>
        public static Color slateGray { get { return new Color(0.4392157f, 0.5019608f, 0.5647059f, 1f); } }
        /// <summary>
        /// Snow. RGBA is (1, 0.98039216, 0.98039216, 1).
        /// </summary>
        public static Color snow { get { return new Color(1f, 0.98039216f, 0.98039216f, 1f); } }
        /// <summary>
        /// Spring Green. RGBA is (0, 1, 0.49803922, 1).
        /// </summary>
        public static Color springGreen { get { return new Color(0f, 1f, 0.49803922f, 1f); } }
        /// <summary>
        /// Steel Blue. RGBA is (0.27450982, 0.50980395, 0.7058824, 1).
        /// </summary>
        public static Color steelBlue { get { return new Color(0.27450982f, 0.50980395f, 0.7058824f, 1f); } }
        /// <summary>
        /// Tan. RGBA is (0.8235294, 0.7058824, 0.54901963, 1).
        /// </summary>
        public static Color tan { get { return new Color(0.8235294f, 0.7058824f, 0.54901963f, 1f); } }
        /// <summary>
        /// Teal. RGBA is (0, 0.5019608, 0.5019608, 1).
        /// </summary>
        public static Color teal { get { return new Color(0f, 0.5019608f, 0.5019608f, 1f); } }
        /// <summary>
        /// Thistle. RGBA is (0.84705883, 0.7490196, 0.84705883, 1).
        /// </summary>
        public static Color thistle { get { return new Color(0.84705883f, 0.7490196f, 0.84705883f, 1f); } }
        /// <summary>
        /// Tomato. RGBA is (1, 0.3882353, 0.2784314, 1).
        /// </summary>
        public static Color tomato { get { return new Color(1f, 0.3882353f, 0.2784314f, 1f); } }
        /// <summary>
        /// Turquoise. RGBA is (0.2509804, 0.8784314, 0.8156863, 1).
        /// </summary>
        public static Color turquoise { get { return new Color(0.2509804f, 0.8784314f, 0.8156863f, 1f); } }
        /// <summary>
        /// Violet. RGBA is (0.93333334, 0.50980395, 0.93333334, 1).
        /// </summary>
        public static Color violet { get { return new Color(0.93333334f, 0.50980395f, 0.93333334f, 1f); } }
        /// <summary>
        /// Wheat. RGBA is (0.9607843, 0.87058824, 0.7019608, 1).
        /// </summary>
        public static Color wheat { get { return new Color(0.9607843f, 0.87058824f, 0.7019608f, 1f); } }
        /// <summary>
        /// White. RGBA is (1, 1, 1, 1).
        /// </summary>
        public static Color white { get { return new Color(1f, 1f, 1f, 1f); } }
        /// <summary>
        /// White Smoke. RGBA is (0.9607843, 0.9607843, 0.9607843, 1).
        /// </summary>
        public static Color whiteSmoke { get { return new Color(0.9607843f, 0.9607843f, 0.9607843f, 1f); } }
        /// <summary>
        /// Yellow. RGBA is (1, 1, 0, 1).
        /// </summary>
        public static Color yellow { get { return new Color(1f, 1f, 0f, 1f); } }
        /// <summary>
        /// Yellow Green. RGBA is (0.6039216, 0.8039216, 0.19607843, 1).
        /// </summary>
        public static Color yellowGreen { get { return new Color(0.6039216f, 0.8039216f, 0.19607843f, 1f); } }

        //// | -- Was used to get all these colors, I wasn't about to write all that by hand --
        //// |
        //// |  var bindingFlags = BindingFlags.Instance |
        //// |  BindingFlags.NonPublic |
        //// |             BindingFlags.Public;
        //// |  var colorTest = new ColorTest();
        //// |  var fieldValues = colorTest.GetType().GetFields(bindingFlags).Select(c => c.GetValue(colorTest)).ToList();
        //// |  var fieldNames = colorTest.GetType().GetFields().Select(c => c.Name).ToList();
        //// |
        //// |  for (int i = 0; i < fieldValues.Count; i++)
        //// |  {
        //// |      var col = (Color)fieldValues[i];
        //// |      var name = fieldNames[i];
        //// |      var first = name[0].ToString().ToLower();
        //// |      name = $"{first}{name.Remove(0, 1)}";
        //// |              var nameUpper = Regex.Replace(fieldNames[i], @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
        //// |      var r = Mathf.Normalize(col.R, 0, 255);
        //// |      var g = Mathf.Normalize(col.G, 0, 255);
        //// |      var b = Mathf.Normalize(col.B, 0, 255);
        //// |      var a = Mathf.Normalize(col.A, 0, 255);
        //// |      Debug.Log($"/// <summary>");
        //// |      Debug.Log($"/// {nameUpper}. RGBA is ({r}, {g}, {b}, {a}).");
        //// |      Debug.Log($"/// </summary>");
        //// |      Debug.Log($"public static Color {name} " + "{ get { return new Color(" + r + "f, " + g + "f, " + b + "f, " + a + "f); } }");
        //// |  }
    }
}
