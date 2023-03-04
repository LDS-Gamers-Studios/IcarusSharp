using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

using Icarus.ServerSetting;

namespace Icarus.Discord.Commands
{
    public partial class Admin
    {
        [SlashCommand("test", "Runs the test command")]
        public async Task Test(InteractionContext ctx, [Option("a", "a")]string input)
        {
            await ctx.DeferAsync();

            var e = ctx.IcarusEmbed()
                .WithTitle("Cake Day Members");

            var members = DataContext.Config_Integer(ServerSettings.Management_CakeDayMembers);
            if (members is null)
            {
                e = e.WithDescription("No member count is set.")
                    .WithColor(DiscordColor.Yellow);
            }
            else
            {
                e = e.WithDescription($"Member count is set to {members}.");
            }

            await ctx.EditResponseAsync(e);
        }
    }
}
