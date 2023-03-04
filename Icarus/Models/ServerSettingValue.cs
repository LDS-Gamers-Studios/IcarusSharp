using System.ComponentModel.DataAnnotations.Schema;

namespace Icarus.Models
{
    public class ServerSettingValue
    {
        public int ServerSettingValueId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Note { get; set; }
        public Member SetBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
