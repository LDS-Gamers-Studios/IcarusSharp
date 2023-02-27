using DSharpPlus.SlashCommands;

namespace Icarus.Discord.Commands
{
    [SlashCommandGroup("admin", "Bot administrators only.", false)]
    public partial class Admin : ApplicationCommandModule
    {
        readonly ILogger Logger;
        readonly IConfiguration Config;
        readonly DataContext DataContext;

        public Admin(ILogger logger, IConfiguration config, DataContext context)
        {
            Logger = logger;
            Config = config;
            DataContext = context;
        }
    }
}
