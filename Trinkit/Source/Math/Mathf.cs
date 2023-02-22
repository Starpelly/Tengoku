using System;

namespace Trinkit
{
    public static partial class Mathf
    {
        public const float PI = (float)Math.PI;
        public const float PIOver4 = (float)(Math.PI / 4.0);

        /// <summary>
        /// A representation of positive infinity.
        /// </summary>
        public const float Infinity = Single.PositiveInfinity;

        /// <summary>
        /// A representation of negative infinity.
        /// </summary>
        public const float NegativeInfinity = Single.NegativeInfinity;

        /// <summary>
        /// Degrees to radians conversion constant.
        /// </summary>
        public const float Deg2Rad = PI * 2f / 360f;

        /// <summary>
        /// Radians to degrees conversion constant.
        /// </summary>
        public const float Rad2Deg = 1f / Deg2Rad;

        /// <summary>
        /// Returns the sine of the angle (f) in radians.
        /// </summary>
        public static float Sin(float f) { return (float)Math.Sin(f); }

        /// <summary>
        /// Returns the cosine of the angle (f) in radians.
        /// </summary>
        public static float Cos(float f) { return (float)Math.Cos(f); }

        /// <summary>
        /// Returns the tangent of the angle (f) in radians.
        /// </summary>
        public static float Tan(float f) { return (float)Math.Tan(f); }

        /// <summary>
        /// Returns the absolute value of (f).
        /// </summary>
        public static float Abs(float f) { return (float)Math.Abs(f); }

        /// <summary>
        /// Returns the absolute value of (f).
        /// </summary>
        public static int Abs(int f) { return Math.Abs(f); }

        /// <summary>
        /// Returns the sign of (f).
        /// </summary>
        public static float Sign(float f) { return f >= 0F ? 1F : -1F; }

        /// <summary>
        /// Returns (f) raised to power of (p).
        /// </summary>
        public static float Pow(float f, float p) { return (float)Math.Pow(f, p); }

        /// <summary>
        /// Returns the smallest integer greater to or equal to (f).
        /// </summary>
        public static float Ceil(float f) { return (float)Math.Ceiling(f); }

        /// <summary>
        /// Returns the largest integer smaller to or equal to (f).
        /// </summary>
        public static float Floor(float f) { return (float)Math.Floor(f); }

        /// <summary>
        /// Returns (f) rounded to the nearest integer.
        /// </summary>
        public static float Round(float f) { return (float)Math.Round(f); }

        /// <summary>
        /// Clamps value between (min) and (max).
        /// </summary>
        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
        }

        /// <summary>
        /// Clamps value between (min) and (max).
        /// </summary>
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
        }

        /// <summary>
        /// Clamps (value) between 0 and 1.
        /// </summary>
        public static float Clamp01(float value)
        {
            if (value < 0F)
                return 0F;
            else if (value > 1f)
                return 1f;
            else
                return value;
        }

        /// <summary>
        /// Interpolates between (a) and (b) by (t).
        /// (t) is clamped between 0 and 1.
        /// </summary>
        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Clamp01(t);
        }

        /// <summary>
        /// Interpolates between (a) and (b) by (t).
        /// </summary>
        public static float LerpUnclamped(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        /// <summary>
        /// Converts two numbers to a range of 0 - 1.
        /// </summary>
        public static float Normalize(float val, float min, float max)
        {
            return (val - min) / (max - min);
        }

        /// <summary>
        /// Converts a normalized value to a set range from (min) - (max).
        /// </summary>
        public static float DeNormalize(float val, float min, float max)
        {
            return (val * (max - min) + min);
        }

        /// <summary>
        /// Loops the value (t), so that it is never larger than (length) and never smaller than 0.
        /// </summary>
        public static float Repeat(float t, float length)
        {
            return Clamp(t - Mathf.Floor(t / length) * length, 0.0f, length);
        }

        /// <summary>
        /// Rounds float to nearest interval.
        /// </summary>
        public static float Round2Nearest(float a, float interval)
        {
            return a - (a % interval);
        }

        /// <summary>
        /// Returns true if a value is within a certain range.
        /// </summary>
        public static bool IsWithin(this float val, float min, float max)
        {
            return val >= min && val <= max;
        }

        public static float Max(float a, float b) { return a > b ? a : b; }
        /// <summary>
        /// Returns largest of two or more values.
        /// </summary>
        public static float Max(params float[] values)
        {
            int len = values.Length;
            if (len == 0)
                return 0;
            float m = values[0];
            for (int i = 1; i < len; i++)
            {
                if (values[i] > m)
                    m = values[i];
            }
            return m;
        }

        public static int Max(int a, int b) { return a > b ? a : b; }
        /// <summary>
        /// Returns largest of two or more values.
        /// </summary>
        public static int Max(params int[] values)
        {
            int len = values.Length;
            if (len == 0)
                return 0;
            int m = values[0];
            for (int i = 1; i < len; i++)
            {
                if (values[i] > m)
                    m = values[i];
            }
            return m;
        }


        public static float Min(float a, float b) { return a < b ? a : b; }
        /// <summary>
        /// Returns smallest of two or more values.
        /// </summary>
        public static float Min(params float[] values)
        {
            int len = values.Length;
            if (len == 0)
                return 0;
            float m = values[0];
            for (int i = 1; i < len; i++)
            {
                if (values[i] < m)
                    m = values[i];
            }
            return m;
        }

        public static int Min(int a, int b) { return a < b ? a : b; }
        /// <summary>
        /// Returns smallest of two or more values.
        /// </summary>
        public static int Min(params int[] values)
        {
            int len = values.Length;
            if (len == 0)
                return 0;
            int m = values[0];
            for (int i = 1; i < len; i++)
            {
                if (values[i] < m)
                    m = values[i];
            }
            return m;
        }
    }
}
