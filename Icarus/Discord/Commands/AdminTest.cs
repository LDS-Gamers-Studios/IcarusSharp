using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Icarus.Discord.Commands
{
    public partial class Admin : ApplicationCommandModule
    {
        [SlashCommand("test", "Runs the test command")]
        public async Task Test(InteractionContext ctx, [Option("a", "a")]string input)
        {
            await ctx.DeferAsync();

            await ctx.Error("Failed To Run Error Test", "Task failed successfully??", "f1", "v1", "f2", "v2");
        }
    }
}
