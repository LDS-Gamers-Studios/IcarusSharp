using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;

using System.Text;

namespace Icarus.Discord.Commands
{
    public partial class Utility : ApplicationCommandModule
    {
        static Dictionary<char, char> Matches;

        static Utility()
        {
            var f = File.ReadAllText("lookAlikes.txt", Encoding.UTF8).Split('\n');
            var pairs = f.SelectMany(line => line.ToCharArray().Select(t => new KeyValuePair<char, char>(t, line[0])));
            Matches = new Dictionary<char, char>();
            foreach (var pair in pairs)
            {
                if (!Matches.ContainsKey(pair.Key))
                {
                    Matches.Add(pair.Key, pair.Value);
                }
            }
        }

        public static string FilterConvert(string input)
        {
            var converted = string.Join("", input.ToCharArray().Select(c =>
            {
                if (Matches.ContainsKey(c))
                {
                    return Matches[c];
                }
                return c;
            }));

            return converted;
        }
    }
}
