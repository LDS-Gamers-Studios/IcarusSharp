using DSharpPlus.Entities;

using Icarus.Discord;
using Icarus.Models;

using System.Reflection;

namespace Icarus.ServerSetting
{
    public static class ServerSettingExtensions
    {
        public static readonly List<ServerSettingAttribute> ServerSettings;
        public static readonly Dictionary<ServerSettings, string> SettingKeys;

        static ServerSettingExtensions()
        {
            ServerSettings = new List<ServerSettingAttribute>();
            SettingKeys = new Dictionary<ServerSettings, string>();

            var values = Enum.GetValues(typeof(ServerSettings));
            foreach (ServerSettings value in values)
            {
                var att = typeof(ServerSettings)
                    .GetFields()
                    .First(f => f.Name == value.ToString())
                    .GetCustomAttribute<ServerSettingAttribute>();

                ServerSettings.Add(att);
                SettingKeys.Add(value, att.Key);
            }
        }

        public static List<IGrouping<string /* Category */, (ServerSettingAttribute, ServerSettingValue, string /* Preview */)>> LoadConfigValues(DataContext data, DiscordBotService bot, IConfiguration config)
        {
            var stored = data.ServerSettingValue.ToList();
            var guild = bot.Client.Guilds[ulong.Parse(config["discord:guild"])];

            return ServerSettings
                .Select(t => (t, stored.FirstOrDefault(s => s.Key == t.Key)))
                .Select(t => (t.t, t.Item2, Preview(t.t.Key, t.Item2?.Value, guild)))
                .GroupBy(t => t.t.Category)
                .ToList();
        }

        public static string Preview(string key, string value, DiscordGuild guild)
        {
            if (value is null || value.Trim() == "") { return null; }

            var t = ServerSettings.First(t => t.Key == key);

            if (t.Type == ServerSettingType.Text)
            {
                return "";
            }
            else if (t.Type == ServerSettingType.Decimal)
            {
                if (!float.TryParse(value, out var f)) { return null; }
                return f.ToString();
            }
            else if (t.Type == ServerSettingType.Integer)
            {
                if (!int.TryParse(value, out var f)) { return null; }
                return f.ToString();
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
            else if (t.Type == ServerSettingType.ChannelCollection)
            {
                var ids = value.Split(',');
                var o = new List<string>();
                foreach (var id in ids)
                {
                    if (guild.Channels.Any(c => c.Key.ToString() == id))
                    {
                        o.Add("#" + guild.Channels.FirstOrDefault(c => c.Key.ToString() == id).Value.Name);
                    }
                }
                return string.Join(", ", o);
            }

            return null;
        }

        public static void Set(string key, string value, DiscordBotService bot, DataContext data, IConfiguration config)
        {
            var guild = bot.Client.Guilds[ulong.Parse(config["discord:guild"])];
            var existing = data.ServerSettingValue.FirstOrDefault(s => s.Key == key);

            if (value is null || value.Trim() == "")
            {
                if (existing is not null)
                {
                    data.ServerSettingValue.Remove(existing);
                    data.SaveChanges();
                }
                return;
            }

            var p = Preview(key, value, guild);
            if (p is null)
            {
                if (existing is not null)
                {
                    data.ServerSettingValue.Remove(existing);
                    data.SaveChanges();
                }
                return;
            }

            if (existing is not null)
            {
                data.ServerSettingValue.Remove(existing);
            }

            var setting = new ServerSettingValue()
            {
                Key = key,
                Value = value,
            };
            data.ServerSettingValue.Add(setting);
            data.SaveChanges();
        }

        public static DiscordChannel Config_Channel(this DataContext data, ServerSettings setting)
        {
            var currentValue = data.ServerSettingValue.FirstOrDefault(s => s.Key == SettingKeys[setting]);
            if (currentValue is null) { return null; }
            var guild = DiscordBotService.Instance.Client.Guilds[ulong.Parse(DiscordBotService.Configuration["discord:guild"])];
            return guild.Channels.Select(c => c.Value).FirstOrDefault(c => c.Id.ToString() == currentValue.Value);
        }

        public static DiscordRole Config_Role(this DataContext data, ServerSettings setting)
        {
            var currentValue = data.ServerSettingValue.FirstOrDefault(s => s.Key == SettingKeys[setting]);
            if (currentValue is null) { return null; }
            var guild = DiscordBotService.Instance.Client.Guilds[ulong.Parse(DiscordBotService.Configuration["discord:guild"])];
            return guild.Roles.Select(r => r.Value).FirstOrDefault(r => r.Id.ToString() == currentValue.Value);
        }

        public static DiscordMember Config_Member(this DataContext data, ServerSettings setting)
        {
            var currentValue = data.ServerSettingValue.FirstOrDefault(s => s.Key == SettingKeys[setting]);
            if (currentValue is null) { return null; }
            var guild = DiscordBotService.Instance.Client.Guilds[ulong.Parse(DiscordBotService.Configuration["discord:guild"])];
            return guild.Members.Select(m => m.Value).FirstOrDefault(m => m.Id.ToString() == currentValue.Value);
        }

        public static string Config_Text(this DataContext data, ServerSettings setting)
        {
            var currentValue = data.ServerSettingValue.FirstOrDefault(s => s.Key == SettingKeys[setting]);
            if (currentValue is null) { return null; }
            return currentValue.Value;
        }

        public static float? Config_Decimal(this DataContext data, ServerSettings setting)
        {
            var currentValue = data.ServerSettingValue.FirstOrDefault(s => s.Key == SettingKeys[setting]);
            if (currentValue is null) { return null; }
            return float.Parse(currentValue.Value);
        }

        public static int? Config_Integer(this DataContext data, ServerSettings setting)
        {
            var currentValue = data.ServerSettingValue.FirstOrDefault(s => s.Key == SettingKeys[setting]);
            if (currentValue is null) { return null; }
            return int.Parse(currentValue.Value);
        }

        public static List<DiscordChannel> Config_ChannelCollection(this DataContext data, ServerSettings setting)
        {
            var currentValue = data.ServerSettingValue.FirstOrDefault(s => s.Key == SettingKeys[setting]);
            if (currentValue is null) { return new(); }
            var guild = DiscordBotService.Instance.Client.Guilds[ulong.Parse(DiscordBotService.Configuration["discord:guild"])];
            return guild.Channels.Select(c => c.Value).Where(c => currentValue.Value.Split(',').Contains(c.Id.ToString())).ToList();
        }
    }
}
