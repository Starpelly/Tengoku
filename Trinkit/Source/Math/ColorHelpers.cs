namespace Trinkit
{
    public static class ColorHelpers
    {
        public static Color ChangeAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        public static Color Hex2RGB(this string hex)
        {
            return new Color(hex);
        }
    }
}
