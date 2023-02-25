using DSharpPlus.SlashCommands.Attributes;
using DSharpPlus.SlashCommands;
using DSharpPlus;

namespace Icarus.Discord.Commands
{
    [SlashCommandGroup("admin", "Bot administrators only.", false)]
    public partial class Admin : ApplicationCommandModule
    {
        ILogger Logger;
        DataContext DataContext;

        public Admin(ILogger logger, DataContext dbContext)
        {
            Logger = logger;
            DataContext = dbContext;
        }
    }
}
