using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Icarus.Discord.Commands
{
    public partial class Admin
    {
        [SlashCommand("heartbeat", "He's dead, Jim!")]
        public async Task Heartbeat(InteractionContext ctx)
        {
            await ctx.DeferAsync();
            var e = ctx.IcarusEmbed()
                .WithTitle("Heartbeat")
                .WithColor(DiscordColor.DarkGreen)
                .AddField("Latency", $"{ctx.Client.Ping}ms", false)
                .AddField("Commands (Startup / Last Hour)", $"{BotService.CommandsSinceStart.Count}/{BotService.CommandsSinceStart.Where(c => c.Item2 > DateTime.Now.AddHours(-1)).Count()}", true)
                .AddField("Messages (Startup / Last Hour)", $"{BotService.MessagesSinceStart.Count}/{BotService.MessagesSinceStart.Where(c => c > DateTime.Now.AddHours(-1)).Count()}", true);
            await ctx.EditResponseAsync(e);
        }
    }
}
