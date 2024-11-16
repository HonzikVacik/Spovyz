namespace Spovyz.Models
{
    public class Message
    {
        public uint Id { get; set; }
        public TimeOnly Timestamp { get; set; }
        public string Text { get; set; }
        public Employee EmployeeId { get; set; }
        public Project ProjectId { get; set; }
        public Task TaskId { get; set; }
    }
}
