using DiscordRPC;

namespace Tengoku.Discord
{
    public static class DefaultPresences
    {
        public static RichPresence Idle = (new DefaultPresence() { State = "" }).ToRichPresence();
        public static RichPresence PlayingLevel = (new DefaultPresence() { State = "Playing a level", Details = "Spaceball" }).ToRichPresence();
    }

    public class DefaultPresence : BaseRichPresence
    {
        public DefaultPresence()
        {
            Assets = new Assets()
            {
                LargeImageKey = "default_icon"
            };
        }
    }
}
