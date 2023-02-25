using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;

using Icarus.Models;

namespace Icarus
{
    public static class Extensions
    {
        public static List<DiscordActionRowComponent> CreateRows(this List<(string id, string text, ulong emote, ButtonStyle style)> buttons)
        {
            var buttonData = buttons.Count > 25 ? buttons.Take(25) : buttons;

            return buttonData
                .Select(btnData => new DiscordButtonComponent(btnData.style, btnData.id, btnData.text, false, new DiscordComponentEmoji(btnData.emote)))
                .Chunk(5)
                .Select(chunk => new DiscordActionRowComponent(chunk))
                .ToList();
        }

        public static Member IcarusMember(this DataContext db, BaseContext ctx)
        {
            return db.Member.FirstOrDefault(mem => mem.DiscordId == ctx.Member.Id);
        }

        public static DiscordEmbedBuilder IcarusEmbed(this InteractionContext ctx) =>
            new DiscordEmbedBuilder()
            .WithColor(new DiscordColor("ff8a00"))
            .WithAuthor(ctx.Member.Username, null, ctx.Member.GetAvatarUrl(ImageFormat.Png, 128))
            .WithTimestamp(DateTimeOffset.Now)
            .WithFooter("Icarus 6 Alpha");

        public static DiscordEmbedBuilder IcarusEmbed(this DiscordMessage ctx) =>
            new DiscordEmbedBuilder()
            .WithColor(new DiscordColor("ff8a00"))
            .WithAuthor(ctx.Author.Username, null, ctx.Author.GetAvatarUrl(ImageFormat.Png, 128))
            .WithTimestamp(DateTimeOffset.Now)
            .WithFooter("Icarus 6 Alpha");

        public static async Task Error(this InteractionContext ctx, string title, string desc, params string[] fields)
        {
            var e = ctx.IcarusEmbed()
                .WithTitle(title)
                .WithDescription(desc)
                .WithColor(DiscordColor.Red);

            fields.Chunk(2).ToList().ForEach(ch =>
            {
                e = e.AddField(ch[0], ch[1], true);
            });
            await ctx.EditResponseAsync(e);
        }

        public static async Task<bool> ConfirmAction(this InteractionContext ctx, string title, string desc)
        {
            var embed = ctx.IcarusEmbed()
                .WithColor(DiscordColor.Red)
                .WithTitle(title)
                .WithDescription(desc);

            var t = RandomString(10);
            var f = RandomString(10);

            var components = new List<DiscordComponent>()
            {
                new DiscordButtonComponent(ButtonStyle.Success, t.ToString(), "Confirm", emoji: new DiscordComponentEmoji("✅")),
                new DiscordButtonComponent(ButtonStyle.Danger, f.ToString(), "Cancel", emoji: new DiscordComponentEmoji("✖"))
            };

            var msg = await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed).AddComponents(components));

            var res = await msg.WaitForButtonAsync(i => i.Id == f.ToString() || i.Id == t.ToString());
            if (res.TimedOut)
            {
                embed.Title = "[CANCELED] " + embed.Title;
                embed.Color = DiscordColor.Gray;

                await res.Result.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().AddEmbed(embed));


                return false;
            }

            var c = res.Result.Id == t.ToString();

            embed.Title = "[" + (c ? "CONFIRMED" : "CANCELED") + "] " + embed.Title;
            embed.Color = c ? DiscordColor.DarkGreen : DiscordColor.Gray;
            await res.Result.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                new DiscordInteractionResponseBuilder().AddEmbed(embed)
            );

            return c;
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static Task<DiscordMessage> EditResponseAsync(this BaseContext ctx, string content) =>
            ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content));
        public static Task<DiscordMessage> EditResponseAsync(this BaseContext ctx, DiscordEmbedBuilder e) =>
            ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(e));
    }
}
