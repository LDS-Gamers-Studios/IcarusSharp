namespace Icarus.ServerSettings
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ServerSettingRequiredAttribute : Attribute
    {
        public readonly ServerSettingTemplate Template;

        public ServerSettingRequiredAttribute(string name, ServerSettingType type)
        {
            Template = new ServerSettingTemplate()
            {
                Name = name,
                Type = type
            };
        }
    }
}
