using Newtonsoft.Json;

namespace Trinkit.Localization
{
    [Serializable]
    public class Language
    {
        public string? APPNAME { get; set; }
        public Dictionary<string, Game> GAMES { get; set; }

        public Language()
        {
            APPNAME = "Rhythm Tengoku";
            GAMES = new()
            {
                { "GAME_SPACEBALL", new("Spaceball", "Don't worry about the\ncamera's zooms! Hit the\nball with your heart!\nDon't blink!\nDon't wipe your tears!") }
            };
            // Console.WriteLine(JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public struct Game
        {
            public string NAME { get; set; }
            public string GAME_DESC { get; set; }

            public Game(string NAME, string GAME_DESC)
            {
                this.NAME = NAME;
                this.GAME_DESC = GAME_DESC;
            }
        }
    }
}
