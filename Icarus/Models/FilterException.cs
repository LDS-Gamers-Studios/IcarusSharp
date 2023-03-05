namespace Icarus.Models
{
    public class FilterException
    {
        public int FilterExceptionId { get; set; }
        public Filter Filter { get; set; }
        public string ExceptionText { get; set; }
    }
}
