using DSharpPlus.Entities;

using Icarus.Discord;
using Icarus.Models;

using System.Reflection;

namespace Icarus.ServerSettings
{
    public static class ExtensionsServerConfig
    {
        public static readonly List<ServerSettingTemplate> Templates;

        static ExtensionsServerConfig()
        {
            Templates =
                Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .SelectMany(t => t.GetMembers())
                .Union(Assembly.GetExecutingAssembly().GetTypes())
                .Where(t => Attribute.IsDefined(t, typeof(ServerSettingRequiredAttribute)))
                .SelectMany(t => t.GetCustomAttributes<ServerSettingRequiredAttribute>().Select(a => a.Template))
                .ToList();
        }

        public static List<IGrouping<string, (ServerSettingTemplate, ServerSetting, string)>> LoadConfigValues(DataContext data, DiscordBotService bot, IConfiguration config)
        {
            var stored = data.ServerSetting.ToList();
            var guild = bot.Client.Guilds[ulong.Parse(config["discord:guild"])];

            return Templates
                .Select(t => (t, stored.FirstOrDefault(s => s.Key == t.Name)))
                .Select(t => (t.t, t.Item2, Preview(t.t.Name, t.Item2?.Value, guild)))
                .GroupBy(t => t.t.Name.Split(':')[0])
                .ToList();
        }

        public static string Preview(string key, string value, DiscordGuild guild)
        {
            if (value is null) { return null; }

            var t = Templates.First(t => t.Name == key);

            if (t.Type == ServerSettingType.Text || t.Type == ServerSettingType.Decimal)
            {
                return value;
            }
            else if (t.Type == ServerSettingType.Channel)
            {
                if (!guild.Channels.Any(c => c.Key.ToString() == value))
                {
                    return null;
                }

                return "#" + guild.Channels.FirstOrDefault(c => c.Key.ToString() == value).Value.Name;
            }
            else if (t.Type == ServerSettingType.Role)
            {
                if (!guild.Roles.Any(c => c.Key.ToString() == value))
                {
                    return null;
                }

                return "@" + guild.Roles.FirstOrDefault(c => c.Key.ToString() == value).Value.Name;
            }
            else if (t.Type == ServerSettingType.User)
            {
                if (!guild.Members.Any(c => c.Key.ToString() == value))
                {
                    return null;
                }

                return "@" + guild.Members.FirstOrDefault(c => c.Key.ToString() == value).Value.Username;
            }

            return null;
        }

        public static void Set(string key, string value, DiscordBotService bot, DataContext data, IConfiguration config)
        {
            var guild = bot.Client.Guilds[ulong.Parse(config["discord:guild"])];
            var existing = data.ServerSetting.FirstOrDefault(s => s.Key == key);

            if (value is null || value.Trim() == "")
            {
                if (existing is not null)
                {
                    data.ServerSetting.Remove(existing);
                    data.SaveChanges();
                }
                return;
            }

            var p = Preview(key, value, guild);
            if (p is null)
            {
                if (existing is not null)
                {
                    data.ServerSetting.Remove(existing);
                    data.SaveChanges();
                }
                return;
            }

            if (existing is not null)
            {
                data.ServerSetting.Remove(existing);
            }

            var setting = new ServerSetting()
            {
                Key = key,
                Value = value,
                SetBy = data.Member.First(),
                UpdatedAt = DateTime.Now,
                Note = "",
            };
            data.ServerSetting.Add(setting);
            data.SaveChanges();
        }
    }
}
