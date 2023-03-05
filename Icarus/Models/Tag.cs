namespace Icarus.Models
{
    [Serializable]
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string AttachmentURL { get; set; }
        public bool IsEmbed { get; set; }
    }
}
