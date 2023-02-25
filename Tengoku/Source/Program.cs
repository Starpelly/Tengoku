namespace Tengoku
{
    internal class Program
    {
        private static string TITLE =
#if DEBUG
"Rhythm Tengoku (DEBUG)";
#else
"Rhythm Tengoku";
#endif

        static void Main(string[] args)
        {
            using (var rhythmTengoku = new Game(TITLE, 1280, 720, false))
            {
                rhythmTengoku.Run();
            }
        }
    }
}