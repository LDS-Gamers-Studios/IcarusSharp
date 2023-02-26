namespace Icarus.ServerConfig
{
    public class ServerConfigTemplate
    {
        public string Name { get; set; }
        public ServerConfigType Type { get; set; }
    }

    public enum ServerConfigType
    {
        Channel,
        Role,
        User,
        Text,
        Decimal
    }
}
