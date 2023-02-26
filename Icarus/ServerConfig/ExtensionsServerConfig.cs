using System.Reflection;

namespace Icarus.ServerConfig
{
    public static class ExtensionsServerConfig
    {
        public static readonly List<ServerConfigTemplate> Templates;

        static ExtensionsServerConfig()
        {
            Templates =
                Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .SelectMany(t => t.GetMembers())
                .Union(Assembly.GetExecutingAssembly().GetTypes())
                .Where(t => Attribute.IsDefined(t, typeof(ServerConfigRequiredAttribute)))
                .Select(t => t.GetCustomAttribute<ServerConfigRequiredAttribute>().Template)
                .ToList();
        }
    }
}
