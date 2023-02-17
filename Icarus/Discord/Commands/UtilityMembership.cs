using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;

namespace Icarus.Discord.Commands
{
    public partial class Utility : ApplicationCommandModule
    {
        [DiscordEventHandler("Ready")]
        public static async Task OnReady(DiscordClient sender, ReadyEventArgs e)
        {
            
        }
    }
}
