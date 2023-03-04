using System.ComponentModel.DataAnnotations.Schema;

namespace Icarus.Models
{
    public class Flag
    {
        public int FlagId { get; set; }
        public DateTime Time { get; set; }
        public FlagType Type { get; set; }
        public ulong FlaggedById { get; set; }
        public ulong SourceMessageId { get; set; }
        public ulong SourceChannelId { get; set; }
        public ulong SourceUserId { get; set; }
        public string SourcContent { get; set; }
        public string SourceMatches { get; set; }
        public ulong ResolvedById { get; set; }
        public DateTime ResolutionTime { get; set; }
        public FlagResolutionType ResolutionType { get; set; }
        public int ResolutionPoints { get; set; }
        public string SystemMessage { get; set; }
    }

    public enum FlagType
    {
        Message = 0,
        User = 1,
    }

    public enum FlagResolutionType
    {
        None = 0,
        Cleared = 1,
        Warned = 2,
        Muted = 3,
    }
}
