using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;

using System.Net;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace Icarus.Discord.Commands
{
    public partial class Utility : ApplicationCommandModule
    {
        [DiscordEventHandler("MessageCreated")]
        public static async Task MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            var sentMessage = e.Message.Content;
            if (e.Author.IsBot || sentMessage is null || !sentMessage.StartsWith("!") || sentMessage.Length < 2)
            {
                return;
            }

            var parts = sentMessage[1..].Split(null);
            var cmd = parts[0];

            var mention = parts.Length > 1 ? Regex.IsMatch(parts[1], @"<@!*&*[0-9]+>") ? parts[1] : null : null;
            var remainder = parts.Length > 1 ? string.Join(' ', parts.Skip(1)) : null;

            var db = new IcarusDbContext(DiscordBotService.Configuration);
            var tag = db.Tag.FirstOrDefault(t => t.Name == cmd);

            if (tag is null) { return; }

            var msg = new DiscordMessageBuilder()
                .WithReply(e.Message.Id);

            HttpResponseMessage response = null;
            Stream streamToReadFrom = null;

            if (tag.Content.Contains("{mention}") && mention is null) { return; }
            if (tag.Content.Contains("{remainder}") && remainder is null) { return; }

            var content = tag.Content
                .Replace("{mention}", mention)
                .Replace("{remainder}", remainder)
                .Replace("{channel}", e.Channel.Mention)
                .Replace("{author}", e.Author.Mention);

            if (tag.IsEmbed)
            {
                var embed = e.Message.IcarusEmbed()
                    .WithDescription(content)
                    .WithImageUrl(tag.AttachmentURL);
                msg.Embed = embed;
            }
            else
            {
                msg.Content = content;

                if (tag.AttachmentURL is not null)
                {
                    if (!Directory.Exists("temp"))
                    {
                        Directory.CreateDirectory("temp");
                    }

                    var file = Extensions.RandomString(10);
                    using HttpClient client = new HttpClient();
                    response = await client.GetAsync(tag.AttachmentURL);
                    streamToReadFrom = await response.Content.ReadAsStreamAsync();
                    msg.AddFile(Path.GetFileName(tag.AttachmentURL), streamToReadFrom);
                }
            }

            await e.Channel.SendMessageAsync(msg);

            response?.Dispose();
            streamToReadFrom?.Dispose();
        }
    }
}
