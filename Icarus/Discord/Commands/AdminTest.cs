using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

using Icarus.ServerSettings;

namespace Icarus.Discord.Commands
{
    public partial class Admin
    {
        [SlashCommand("test", "Runs the test command")]
        [ServerSettingRequired("Admin:Channel To Post In", ServerSettingType.Channel)]
        public async Task Test(InteractionContext ctx, [Option("a", "a")]string input)
        {
            var c = ServerSettingExtensions.GetChannel(ctx.Client, DataContext, Config, "Admin:Channel To Post In");

            await ctx.DeferAsync();

            await ctx.EditResponseAsync(ctx.IcarusEmbed().WithDescription("Channel: " + (c?.Mention ?? "None")));
        }
    }
}
