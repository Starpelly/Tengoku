namespace Tengoku
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // account window height for menubar
            using (var rhythmTengoku = new Game("Rhythm Tengoku", 1280, 720 + 19))
            {
                rhythmTengoku.Run();
            }
        }
    }
}