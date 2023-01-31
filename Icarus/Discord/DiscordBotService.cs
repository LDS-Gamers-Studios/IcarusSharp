using DSharpPlus;
using DSharpPlus.SlashCommands;

using System.Reflection;

namespace Icarus.Discord
{
    public class DiscordBotService
    {
        DiscordClient Client;

        public DiscordBotService(ILogger<DiscordBotService> logger, IConfiguration config)
        {
            logger.LogInformation("Booting DSharpPlus... - ");

            Client = new DiscordClient(new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                LargeThreshold = 10000,
                LoggerFactory = new LoggerFactory(new List<ILoggerProvider> { new LogMessageDiverter<DiscordBotService>(logger) }),
                MessageCacheSize = 4096,
                Token = config["discord:token"],
            });

            var slash = Client.UseSlashCommands(new SlashCommandsConfiguration()
            {
                Services = new ServiceCollection()
                    .AddSingleton(config)
                    .AddSingleton<ILogger>(logger)
                    .AddScoped<IcarusDbContext>()
                    .AddLogging(a => a.AddProvider(new LogMessageDiverter<DiscordBotService>(logger)))
                    .BuildServiceProvider()
            });

            slash.RegisterCommands(Assembly.GetExecutingAssembly(), ulong.Parse(config["discord:ldsg"]));

            Client.ConnectAsync().Wait();
        }
    }
}
