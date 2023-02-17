using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Icarus.Discord.Commands
{
    public partial class Admin : ApplicationCommandModule
    {
        [SlashCommand("test", "Runs the test command")]
        [SlashRequireUserPermissions(Permissions.ManageGuild, false)]
        public async Task Test(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync("Members: " + IcarusDbContext.Member.Count());
        }
    }
}
