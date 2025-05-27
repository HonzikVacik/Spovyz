namespace Spovyz.Models
{
    public class Task_tag
    {
        public uint TaskId { get; set; }
        public Task Task { get; set; }
        public uint TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
