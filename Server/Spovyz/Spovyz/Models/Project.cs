namespace Spovyz.Models
{
    public class Project
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Customer Customer { get; set; }
        public DateOnly? Dead_line { get; set; }
        public Enums.ProjectStatus Status { get; set; }
    }
}
