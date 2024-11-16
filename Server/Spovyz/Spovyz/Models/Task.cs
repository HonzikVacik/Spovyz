namespace Spovyz.Models
{
    public class Task
    {
        public required uint Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required Project ProjectId { get; set; }
    }
}
