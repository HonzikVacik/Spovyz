namespace Spovyz.Models
{
    public class Message
    {
        public required uint Id { get; set; }
        public required TimeOnly Timestamp { get; set; }
        public required string Text { get; set; }
        public required Employee EmployeeId { get; set; }
        public required Project ProjectId { get; set; }
        public required Task TaskId { get; set; }
    }
}
