using DSharpPlus;

namespace Icarus.Discord
{
    public class DiscordBotService
    {
        DiscordClient Client;

        public DiscordBotService(ILogger<DiscordBotService> logger)
        {
            logger.LogInformation("Booting DSharpPlus...");

            Client = new DiscordClient(new DiscordConfiguration()
            {
                AlwaysCacheMembers= true,
                AutoReconnect = true,
                Intents = DiscordIntents.All,
                LargeThreshold = 10000,
                LoggerFactory = new LoggerFactory(new List<ILoggerProvider> { new LogMessageDiverter<DiscordBotService>(logger) })
            });
        }
    }
}
