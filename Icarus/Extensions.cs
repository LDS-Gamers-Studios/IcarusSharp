using DSharpPlus;
using DSharpPlus.Entities;

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
    }
}
