using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

using Google.Protobuf.WellKnownTypes;

namespace Icarus.Discord.Commands
{
    public partial class Admin : ApplicationCommandModule
    {
        [SlashCommand("test", "Runs the test command")]
        public async Task Test(InteractionContext ctx, [Option("a", "a")]string input)
        {
            var start = DateTime.Now;
            var f = Utility.FilterConvert(input);
            var seconds = (DateTime.Now - start).TotalSeconds;

            await ctx.CreateResponseAsync(seconds + " - " + f);
        }
    }
}
