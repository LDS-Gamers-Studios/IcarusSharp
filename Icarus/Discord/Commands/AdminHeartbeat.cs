using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using System.Diagnostics;

namespace Icarus.Discord.Commands
{
    public partial class Admin
    {
        [SlashCommand("heartbeat", "He's dead, Jim!")]
        public async Task Heartbeat(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            var totalBytesOfMemoryUsed = currentProcess.WorkingSet64;
            var totalMBMemoryUsed = totalBytesOfMemoryUsed / 1024 / 1024;

            var e = ctx.IcarusEmbed()
                .WithTitle("Heartbeat")
                .WithColor(DiscordColor.DarkGreen)
                .AddField("Latency", $"{ctx.Client.Ping}ms", false)
                .AddField("Usage", $"{BotService.CommandsSinceStart.Count} commands\n{BotService.MessagesSinceStart.Count} messages", true)
                .AddField("Uptime", $"{GetReadableTimespan(DateTime.Now - BotService.StartTime)}", true)
                .AddField("Reach", $"{ctx.Client.Guilds.Count} Guilds\n{ctx.Client.Guilds.SelectMany(g => g.Value.Channels).Count()} Channels\n{ctx.Client.Guilds.SelectMany(g => g.Value.Members).Count()} Members", true)
                .AddField("Memory", $"{totalMBMemoryUsed} MB", true);
            await ctx.EditResponseAsync(e);
        }

        public string GetReadableTimespan(TimeSpan ts)
        {
            // formats and its cutoffs based on totalseconds
            var cutoff = new SortedList<long, string> {
               {59, "{3:S}" },
               {60, "{2:M}" },
               {60*60-1, "{2:M}, {3:S}"},
               {60*60, "{1:H}"},
               {24*60*60-1, "{1:H}, {2:M}"},
               {24*60*60, "{0:D}"},
               {Int64.MaxValue , "{0:D}, {1:H}"}
             };

            // find nearest best match
            var find = cutoff.Keys.ToList()
                          .BinarySearch((long)ts.TotalSeconds);
            // negative values indicate a nearest match
            var near = find < 0 ? Math.Abs(find) - 1 : find;
            // use custom formatter to get the string
            return String.Format(
                new HMSFormatter(),
                cutoff[cutoff.Keys[near]],
                ts.Days,
                ts.Hours,
                ts.Minutes,
                ts.Seconds);
        }

        public class HMSFormatter : ICustomFormatter, IFormatProvider
        {
            // list of Formats, with a P customformat for pluralization
            static Dictionary<string, string> timeformats = new Dictionary<string, string> {
                {"S", "{0:P:Seconds:Second}"},
                {"M", "{0:P:Minutes:Minute}"},
                {"H","{0:P:Hours:Hour}"},
                {"D", "{0:P:Days:Day}"}
            };

            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                return String.Format(new PluralFormatter(), timeformats[format], arg);
            }

            public object GetFormat(Type formatType)
            {
                return formatType == typeof(ICustomFormatter) ? this : null;
            }
        }

        // formats a numeric value based on a format P:Plural:Singular
        public class PluralFormatter : ICustomFormatter, IFormatProvider
        {

            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                if (arg != null)
                {
                    var parts = format.Split(':'); // ["P", "Plural", "Singular"]

                    if (parts[0] == "P") // correct format?
                    {
                        // which index postion to use
                        int partIndex = (arg.ToString() == "1") ? 2 : 1;
                        // pick string (safe guard for array bounds) and format
                        return String.Format("{0} {1}", arg, (parts.Length > partIndex ? parts[partIndex] : ""));
                    }
                }
                return String.Format(format, arg);
            }

            public object GetFormat(Type formatType)
            {
                return formatType == typeof(ICustomFormatter) ? this : null;
            }
        }
    }
}
