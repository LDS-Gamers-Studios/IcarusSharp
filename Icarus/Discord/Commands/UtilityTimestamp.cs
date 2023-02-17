using DSharpPlus;
using DSharpPlus.SlashCommands;

namespace Icarus.Discord.Commands
{
    public partial class Utility : ApplicationCommandModule
    {
        [SlashCommand("timestamp", "Generate a timestamp")]
        public async Task Timestamp(InteractionContext ctx, 
            [Option("Data", "data??")]string data, 
            [Option("Format", "The format to display the timestamp")] TimestampFormatIcarus format)
        {
            data = data.Trim().ToLower();
            var time = DateTimeOffset.Now;
            
            if (data.StartsWith("in"))
            {
                if (data.Split(' ').Length != 3)
                {
                    await ctx.CreateResponseAsync("Invalid number of arguments", true);
                    return;
                }

                var parts = data.Split(' ');
                var number = int.Parse(parts[1]);
                var unit = parts[2];

                var mappings = new Dictionary<string, int>()
                {
                    { "second", 1 },
                    { "seconds", 1 },
                    { "minute", 60 },
                    { "minutes", 60 },
                    { "hour", 3600 },
                    { "hours", 3600 },
                    { "day", 86400 },
                    { "days", 86400 },
                    { "week", 604800 },
                    { "weeks", 604800 },
                    { "month", 2592000 },
                    { "months", 2592000 },
                    { "year", 31536000 },
                    { "years", 31536000 },
                };

                time = time.AddSeconds(mappings[unit] * number);
            }
            else
            {
                time = DateTimeOffset.Parse(data);
            }

            var t = Formatter.Timestamp(time, (TimestampFormat)format);
            await ctx.CreateResponseAsync($"{t}");
        }

        public enum TimestampFormatIcarus : int
        {
            [ChoiceName("Short Date (2/16/23)")]
            ShortDate = TimestampFormat.ShortDate,
            ShortDateTime = TimestampFormat.ShortDateTime,
            ShortTime = TimestampFormat.ShortTime,
            LongDate = TimestampFormat.LongDate,
            LongDateTime = TimestampFormat.LongDateTime,
            LongTime = TimestampFormat.LongTime,
            RelativeTime = TimestampFormat.RelativeTime,
        }
    }
}
