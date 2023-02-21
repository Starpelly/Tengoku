namespace Trinkit
{
    public class Random
    {
        public static float Range(float minInclusive, float maxInclusive)
        {
            var random = new System.Random();
            return random.NextSingle() * (maxInclusive - minInclusive) + minInclusive;
        }

        public static int Range(int minInclusive, int maxInclusive)
        {
            var random = new System.Random();
            return (int)(random.NextSingle() * (maxInclusive - minInclusive) + minInclusive);
        }
    }
}
