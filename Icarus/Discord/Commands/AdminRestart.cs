using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Icarus.Discord.Commands
{
    public partial class Admin : ApplicationCommandModule
    {
        [SlashCommand("restart", "Restarts the bot.")]
        [SlashRequireUserPermissions(Permissions.ManageGuild, false)]
        public async Task RestartCommand(InteractionContext ctx)
        {
            Logger.LogWarning("Disconnecting bot on slash command.");
            await ctx.CreateResponseAsync("Restarting bot...", true);
            await ctx.Client.DisconnectAsync();
            ctx.Client.Dispose();
            Thread.Sleep(2000);
            Environment.Exit(0);
        }
    }
}
