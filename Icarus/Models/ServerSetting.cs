using System.ComponentModel.DataAnnotations.Schema;

namespace Icarus.Models
{
    public class ServerSetting
    {
        public int ServerSettingId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Note { get; set; }
        [ForeignKey("SetByMemberId")]
        public Member SetBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
