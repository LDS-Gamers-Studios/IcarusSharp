namespace Icarus.Discord
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DiscordEventHandler : Attribute
    {
        public string Event;

        public DiscordEventHandler(string e)
        {
            Event = e;
        }
    }
}
