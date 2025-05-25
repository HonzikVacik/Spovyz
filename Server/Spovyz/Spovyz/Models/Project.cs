namespace Spovyz.Models
{
    public class Project
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Customer Customer { get; set; }
        public DateOnly? Dead_line { get; set; }
        public Enums.Status Status { get; set; }
        public ICollection<Project_tag> Project_tags { get; set; }
        public ICollection<Project_employee> Project_employees { get; set; }
    }
}
