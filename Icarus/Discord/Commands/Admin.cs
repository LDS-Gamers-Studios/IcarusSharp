using DSharpPlus.SlashCommands;

using Icarus.ServerSettings;

namespace Icarus.Discord.Commands
{
    [SlashCommandGroup("admin", "Bot administrators only.", false)]
    [ServerSettingRequired("Admin:My Testing Role", ServerSettingType.Role)]
    public partial class Admin : ApplicationCommandModule
    {
        readonly ILogger Logger;

        public Admin(ILogger logger)
        {
            Logger = logger;
        }
    }
}
