using DSharpPlus.SlashCommands.Attributes;
using DSharpPlus.SlashCommands;
using DSharpPlus;

namespace Icarus.Discord.Commands
{
    [SlashCommandGroup("admin", "Bot administrators only.", false)]
    public partial class Admin : ApplicationCommandModule
    {
        ILogger Logger;
        IcarusDbContext IcarusDbContext;

        public Admin(ILogger logger, IcarusDbContext dbContext)
        {
            Logger = logger;
            IcarusDbContext = dbContext;
        }
    }
}
