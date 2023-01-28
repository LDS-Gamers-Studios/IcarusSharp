using DSharpPlus;

namespace Icarus.Discord
{
    public class DiscordBotService
    {
        DiscordClient Client;

        public DiscordBotService(ILogger<DiscordBotService> logger, IConfiguration config)
        {
            logger.LogInformation("Booting DSharpPlus...");

            Client = new DiscordClient(new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                LargeThreshold = 10000,
                LoggerFactory = new LoggerFactory(new List<ILoggerProvider> { new LogMessageDiverter<DiscordBotService>(logger) }),
                MessageCacheSize = 4096,
                Token = config[""]
            });
        }
    }
}
