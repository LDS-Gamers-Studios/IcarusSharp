using DSharpPlus.SlashCommands;

namespace Icarus.Discord.Commands
{
    [SlashCommandGroup("management", "Basic utilities", false)]
    public partial class Management : ApplicationCommandModule
    {
        ILogger Logger;
        IConfiguration Config;
        IcarusDbContext IcarusDbContext;

        public Management(ILogger logger, IConfiguration config, IcarusDbContext dbContext)
        {
            Logger = logger;
            Config = config;
            IcarusDbContext = dbContext;
        }
    }
}
