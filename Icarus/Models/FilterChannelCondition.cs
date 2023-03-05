namespace Icarus.Models
{
    public class FilterChannelCondition
    {
        public int FilterChannelConditionId { get; set; }
        public Filter Filter { get; set; }
        public ulong ChannelId { get; set; }
    }
}
