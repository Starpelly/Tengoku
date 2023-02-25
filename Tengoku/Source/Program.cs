namespace Tengoku
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            using (var rhythmTengoku = new Game("Rhythm Tengoku (DEBUG)", 1280, 720+19, true))
            {
                rhythmTengoku.Run();
            }
#else
            using (var rhythmTengoku = new Game("Rhythm Tengoku", 1280, 720))
            {
                rhythmTengoku.Run();
            }
#endif
        }
    }
}