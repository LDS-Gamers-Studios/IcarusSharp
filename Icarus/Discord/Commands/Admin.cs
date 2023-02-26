using DSharpPlus.SlashCommands;

namespace Icarus.Discord.Commands
{
    [SlashCommandGroup("admin", "Bot administrators only.", false)]
    public partial class Admin : ApplicationCommandModule
    {
        readonly ILogger Logger;

        public Admin(ILogger logger)
        {
            Logger = logger;
        }
    }
}
