using DSharpPlus;
using DSharpPlus.SlashCommands;

namespace Icarus.Discord.Commands
{
    [SlashCommandGroup("utility", "Basic utilities")]
    public partial class Utility : ApplicationCommandModule
    {
        ILogger Logger;
        IConfiguration Config;

        public Utility(ILogger logger, IConfiguration config)
        {
            Logger = logger;
            Config = config;
        }
    }
}
