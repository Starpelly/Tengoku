using DiscordRPC;
using DiscordRPC.Logging;

namespace Tengoku.Discord
{
    public class DiscordRichPresence : IDisposable
    {
        private DiscordRpcClient? _client { get; set; }

        public DiscordRichPresence()
        {
            if (!Settings.DiscordEnabled) return;

            _client = new DiscordRpcClient(DiscordAppID.DISCORD_APP_ID);
            _client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            _client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            _client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
            };

            _client.Initialize();

            _client.SetPresence(DefaultPresences.Debugging);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
