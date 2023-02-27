using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus;
using Microsoft.CodeAnalysis;

namespace Icarus.Discord.Commands
{
    public class Fairies
    {
        [DiscordEventHandler("MessageCreated")]
        public static async Task MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (e.Message.Content == "^^VV<><>BA" && e.Guild is not null)
            {
                try
                {
                    await e.Message.DeleteAsync();
                }
                catch { }
                await e.Guild.Members[e.Author.Id].SendMessageAsync("Oh, hey! Nice job finding this. Don't tell anyone this exists! Here's a one time reward.");
                // Todo: Award ember here then log to DB for only one run
            }
        }
    }
}
