using DSharpPlus.SlashCommands;

namespace Icarus.Models
{
    public class Filter
    {
        public int FilterId { get; set; }
        public FilterType Type { get; set; }
        public string FilterText { get; set; }
        public string FilterTextConverted { get; set; }

        public List<FilterException> FilterExceptions { get; set; }
        public List<FilterChannelCondition> FilterChannelConditions { get; set; }
    }

    public enum FilterType
    {
        [ChoiceName("Flag Only")]
        FlagOnly = 0,
        [ChoiceName("Auto Warn")]
        AutoWarn = 1,
        [ChoiceName("Auto Mute")]
        AutoMute = 2,
    }
}
