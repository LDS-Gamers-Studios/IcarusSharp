namespace Icarus.Models
{
    public class Flag
    {
        public int FlagId { get; set; }
        public DateTime Time { get; set; }
        public int Type { get; set; }
        public Member FlaggedBy { get; set; }
        public ulong SourceMessageId { get; set; }
        public ulong SourceChannelId { get; set; }
        public ulong SourceUserId { get; set; }
        public Member ResolvedBy { get; set; }
        public DateTime ResolutionTime { get; set; }
        public int ResolutionPoints { get; set; }
        public string SystemMessage { get; set; }
    }
}
