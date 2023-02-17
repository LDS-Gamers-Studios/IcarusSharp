using DSharpPlus;
using DSharpPlus.SlashCommands;

namespace Icarus.Discord.Commands
{
    [SlashCommandGroup("utility", "Basic utilities", false)]
    public partial class Utility : ApplicationCommandModule
    {
        ILogger Logger;

        public Utility(ILogger logger)
        {
            Logger = logger;
        }
    }
}
