using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;

using System.Text;

namespace Icarus.Discord.Commands
{
    public partial class Utility
    {
        static readonly Dictionary<char, char> CharacterMatches;

        static Utility()
        {
            var f = File.ReadAllText("lookAlikes.txt", Encoding.UTF8).Split('\n');
            var pairs = f.SelectMany(line => line.ToCharArray().Select(t => new KeyValuePair<char, char>(t, line[0])));
            CharacterMatches = new Dictionary<char, char>();
            foreach (var pair in pairs)
            {
                if (!CharacterMatches.ContainsKey(pair.Key))
                {
                    CharacterMatches.Add(pair.Key, pair.Value);
                }
            }
        }

        public static string FilterConvert(string input)
        {
            var converted = string.Join("", input.ToCharArray().Select(c =>
            {
                if (CharacterMatches.ContainsKey(c))
                {
                    return CharacterMatches[c];
                }
                return c;
            }));

            return converted;
        }
    }
}
