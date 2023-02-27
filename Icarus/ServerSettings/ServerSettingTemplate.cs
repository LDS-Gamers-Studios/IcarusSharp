using Icarus.Discord;

namespace Icarus.ServerSettings
{
    public class ServerSettingTemplate
    {
        public string Name { get; set; }
        public ServerSettingType Type { get; set; }
    }

    public enum ServerSettingType
    {
        Channel,
        Role,
        User,
        Text,
        Decimal
    }
}
