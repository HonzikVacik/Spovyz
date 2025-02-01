namespace Spovyz.Models
{
    public class Task
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Project Project { get; set; }
        public DateOnly? Dead_line { get; set; }
        public Enums.Status Status { get; set; }
    }
}
