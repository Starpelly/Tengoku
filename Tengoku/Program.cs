namespace Tengoku
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var rhythmTengoku = new Game("Rhythm Tengoku", 1280, 720))
            {
                rhythmTengoku.Run();
            }
        }
    }
}