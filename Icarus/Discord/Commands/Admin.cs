using DSharpPlus.SlashCommands;

namespace Icarus.Discord.Commands
{
    [SlashCommandGroup("admin", "Bot administrators only.", false)]
    public partial class Admin : ApplicationCommandModule
    {
        readonly ILogger Logger;
        readonly IConfiguration Config;
        readonly DataContext DataContext;
        readonly DiscordBotService BotService;

        public Admin(ILogger logger, IConfiguration config, DataContext context, DiscordBotService botService)
        {
            Logger = logger;
            Config = config;
            DataContext = context;
            BotService = botService;
        }
    }
}
