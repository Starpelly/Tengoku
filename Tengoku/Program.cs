namespace Tengoku
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // account window height for menubar

#if DEBUG
            using (var rhythmTengoku = new Game("Rhythm Tengoku (DEBUG)", 1280, 720 + 19))
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