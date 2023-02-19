using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Icarus.Discord.Commands
{
    public partial class Utility : ApplicationCommandModule
    {
        [SlashCommand("timestamp", "Generate a timestamp")]
        public async Task Timestamp(InteractionContext ctx, 
            [Option("time", "The time or relative time to generate")]string data, 
            [Option("format", "The format to display the timestamp")] TimestampFormatIcarus format)
        {
            data = data.Trim().ToLower();
            var time = DateTimeOffset.Now;
            
            if (data.StartsWith("in"))
            {
                if (data.Split(' ').Length != 3)
                {
                    var embedError = ctx.IcarusEmbed()
                        .WithColor(DiscordColor.Red)
                        .WithTitle("Failed To Generate Timestamp")
                        .WithDescription("Invalid number of arguments for 'in' mode.");

                    await ctx.CreateResponseAsync(embedError, false);
                    return;
                }

                var parts = data.Split(' ');
                
                if (!int.TryParse(parts[1], out var _))
                {
                    var embedError = ctx.IcarusEmbed()
                        .WithColor(DiscordColor.Red)
                        .WithTitle("Failed To Generate Timestamp")
                        .WithDescription("Invalid duration.");

                    await ctx.CreateResponseAsync(embedError, false);
                    return;
                }

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

                try
                {
                    var multiplier = mappings[unit];

                    time = time.AddSeconds(multiplier * number);
                } 
                catch (KeyNotFoundException)
                {
                    var embedError = ctx.IcarusEmbed()
                        .WithColor(DiscordColor.Red)
                        .WithTitle("Failed To Generate Timestamp")
                        .WithDescription("Invalid unit. Must be second(s), minute(s), hour(s), day(s), week(s), month(s), year(s).");

                    await ctx.CreateResponseAsync(embedError, false);
                    return;
                }
            }
            else
            {
                try
                {
                    time = DateTimeOffset.Parse(data);
                }
                catch
                {
                    var embedError = ctx.IcarusEmbed()
                        .WithColor(DiscordColor.Red)
                        .WithTitle("Failed To Generate Timestamp")
                        .WithDescription("I didn't understand your input.");

                    await ctx.CreateResponseAsync(embedError, false);
                    return;
                }
            }

            var t = Formatter.Timestamp(time, (TimestampFormat)format);

            var embed = ctx.IcarusEmbed()
                .WithTitle("Your Timestamp")
                .WithDescription(t + $" - `{t}`")
                .AddField("Input", data, true)
                .AddField("Format", ((TimestampFormat)format).ToString(), true);

            await ctx.CreateResponseAsync(embed, false);
        }

        public enum TimestampFormatIcarus : int
        {
            [ChoiceName("ShortDate (2/16/23)")]
            ShortDate = TimestampFormat.ShortDate,
            [ChoiceName("ShortDateTime (February 18, 2023 11:00 PM)")]
            ShortDateTime = TimestampFormat.ShortDateTime,
            [ChoiceName("ShortTime (11:05 PM)")]
            ShortTime = TimestampFormat.ShortTime,
            [ChoiceName("LongDate (February 18, 2023)")]
            LongDate = TimestampFormat.LongDate,
            [ChoiceName("LongDateTime (Saturday, February 18, 2023 11:08 PM)")]
            LongDateTime = TimestampFormat.LongDateTime,
            [ChoiceName("LongTime (11:08:37 PM)")]
            LongTime = TimestampFormat.LongTime,
            [ChoiceName("RelativeTime (in 32 years)")]
            RelativeTime = TimestampFormat.RelativeTime,
        }
    }
}
