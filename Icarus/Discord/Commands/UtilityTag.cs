using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;

using System.Net;
using System.Security.Policy;

namespace Icarus.Discord.Commands
{
    public partial class Utility : ApplicationCommandModule
    {
        [DiscordEventHandler("MessageCreated")]
        public static async Task MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            var content = e.Message.Content;
            if (e.Author.IsBot || content is null || !content.StartsWith("!") || content[1..].Length < 1)
            {
                return;
            }

            var parts = content[1..].Split(null);

            var cmd = parts[0];

            var db = new IcarusDbContext(DiscordBotService.Configuration);
            var tag = db.Tag.FirstOrDefault(t => t.Name == cmd);

            if (tag is null) { return; }

            var msg = new DiscordMessageBuilder()
                .WithReply(e.Message.Id);

            HttpResponseMessage response = null;
            Stream streamToReadFrom = null;

            if (tag.IsEmbed)
            {
                var embed = e.Message.IcarusEmbed()
                    .WithDescription(tag.Content)
                    .WithImageUrl(tag.AttachmentURL);
                msg.Embed = embed;
            }
            else
            {
                msg.Content = tag.Content;

                if (tag.AttachmentURL is not null)
                {
                    if (!Directory.Exists("temp"))
                    {
                        Directory.CreateDirectory("temp");
                    }

                    var file = Extensions.RandomString(10);
                    using (HttpClient client = new HttpClient())
                    {
                        response = await client.GetAsync(tag.AttachmentURL);
                        streamToReadFrom = await response.Content.ReadAsStreamAsync();
                        msg.AddFile(Path.GetFileName(tag.AttachmentURL), streamToReadFrom);
                    }
                }
            }

            await e.Channel.SendMessageAsync(msg);

            if (response is not null) { response.Dispose(); }
            if (streamToReadFrom is not null) { response.Dispose(); }
        }
    }
}
