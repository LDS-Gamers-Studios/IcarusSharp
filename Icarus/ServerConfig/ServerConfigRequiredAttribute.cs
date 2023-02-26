namespace Icarus.ServerConfig
{
    [AttributeUsage(AttributeTargets.All)]
    public class ServerConfigRequiredAttribute : Attribute
    {
        public readonly ServerConfigTemplate Template;

        public ServerConfigRequiredAttribute(ServerConfigTemplate template)
        {
            Template = template;
        }
    }
}
