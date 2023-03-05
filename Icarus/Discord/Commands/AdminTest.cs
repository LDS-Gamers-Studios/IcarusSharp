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
                .WithTitle("RP Allowed Channels");

            var channels = DataContext.Config_ChannelCollection(ServerSettings.Terra_RPAllowedChannels);
            if (channels.Count == 0)
            {
                e = e.WithDescription("No channels RP allowed.")
                    .WithColor(DiscordColor.Yellow);
            }
            else
            {
                e = e.WithDescription($"RP allowed channels: {string.Join(",", channels.Select(c => c.Mention))}");
            }

            await ctx.EditResponseAsync(e);
        }
    }
}
