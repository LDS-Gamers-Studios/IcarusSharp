namespace Icarus.Models
{
    [Serializable]
    public class Member
    {
        public int MemberId { get; set; }
        public ulong DiscordId { get; set; }
        public string GuildedId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
