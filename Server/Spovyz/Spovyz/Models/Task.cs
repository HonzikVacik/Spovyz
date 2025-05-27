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
        public ICollection<Task_tag> Task_tags { get; set; }
        public ICollection<Task_employee> Task_employees { get; set; }
    }
}
