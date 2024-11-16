namespace Spovyz.Models
{
    public class Message
    {
        public uint Id { get; set; }
        public TimeOnly Timestamp { get; set; }
        public string Text { get; set; }
        public Employee Employee { get; set; }
        public Project? Project { get; set; }
        public Task? Task { get; set; }
    }
}
