using DSharpPlus.EventArgs;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Text.RegularExpressions;
using Icarus.Models;

namespace Icarus.Discord.Commands
{
    public partial class Mod
    {
        public class FilterMatch
        {
            public string FilterText;
            public int Index;
            public bool InLink;
        }

        [DiscordEventHandler("MessageCreated")]
        public static async Task MessageCreated(DiscordClient sender, MessageCreateEventArgs e) =>
            await HandleMessage(sender, e.Message);
        [DiscordEventHandler("MessageEdited")]
        public static async Task MessageUpdated(DiscordClient sender, MessageUpdateEventArgs e) =>
            await HandleMessage(sender, e.Message);

        private static async Task HandleMessage(DiscordClient client, DiscordMessage msg)
        {
            if (msg.Author.Id == client.CurrentUser.Id) { return; }
            var flagIgnored = DiscordBotService.Configuration.GetSection("discord:flagIgnored").Get<string[]>();
            if (flagIgnored.Contains(msg.Author.Id.ToString())) { return; }

            var context = new DataContext(DiscordBotService.Configuration);
            context.FilterChannelCondition.ToList();
            context.FilterException.ToList();

            var searchableContent = 
                (msg.Content + "\n" + 
                string.Join(
                    "\n", 
                    msg.Embeds.Select(e => 
                        e.Description + "\n" +
                        e.Title + "\n" +
                        e.Url + "\n" +
                        e.Footer + "\n" +
                        e.Author?.Name + "\n" +
                        (e.Fields is null || e.Fields.Count == 0 ? "" : string.Join("\n", e.Fields?.Select(f => f.Name + "\n" + f.Value)))
                    )
                )).ToLower();

            var linkRegex = new Regex(@"(http|ftp|https):\/\/([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:\/~+#-]*[\w@?^=%&\/~+#-])", RegexOptions.IgnoreCase);
            var links = linkRegex.Matches(searchableContent).ToList();

            var contentConverted = Utility.FilterConvert(searchableContent);



            foreach (var filter in context.Filter)
            {
                if (!(filter.FilterChannelConditions is null) && filter.FilterChannelConditions.Any(c => c.ChannelId == msg.ChannelId)) { continue; }

                var positions = contentConverted.AllIndexesOf(filter.FilterTextConverted);

                var index = contentConverted.IndexOf(filter.FilterTextConverted);
                if (index != -1)
                {
                    await msg.Channel.SendMessageAsync($"Found match {filter.FilterText} at index {index}");
                }
            }
        }
    }
}
