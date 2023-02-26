using DSharpPlus.SlashCommands;

using Icarus.ServerSettings;

namespace Icarus.Discord.Commands
{
    [SlashCommandGroup("mod", "Moderation Tools")]
    [ServerSettingRequired("Moderation:Mod Logs", ServerSettingType.Channel)]
    [ServerSettingRequired("Moderation:Mod Flags", ServerSettingType.Channel)]
    [ServerSettingRequired("Moderation:Mod Discussion", ServerSettingType.Channel)]
    [ServerSettingRequired("Moderation:Mod Role", ServerSettingType.Role)]
    public partial class Mod : ApplicationCommandModule { }
}
