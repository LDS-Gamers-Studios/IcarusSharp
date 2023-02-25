using DSharpPlus.SlashCommands;

namespace Icarus.Discord.Commands
{
    [SlashCommandGroup("management", "Basic utilities", false)]
    public partial class Management : ApplicationCommandModule
    {
        ILogger Logger;
        IConfiguration Config;
        DataContext DataContext;

        public Management(ILogger logger, IConfiguration config, DataContext dbContext)
        {
            Logger = logger;
            Config = config;
            DataContext = dbContext;
        }
    }
}
