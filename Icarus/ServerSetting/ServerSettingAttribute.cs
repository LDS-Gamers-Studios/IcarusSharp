namespace Icarus.ServerSetting
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ServerSettingAttribute : Attribute
    {
        public string Category { get; }
        public string Name { get; }
        public string Description { get; }
        public ServerSettingType Type { get; }

        public string Key => Category + ":" + Name;

        public ServerSettingAttribute(string category, string name, string description, ServerSettingType type)
        {
            Category = category;
            Name = name;
            Description = description;
            Type = type;
        }
    }
}
