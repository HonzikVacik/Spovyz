namespace Spovyz.Models
{
    public class Project
    {
        public required uint Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required Customer CustomerId { get; set; }
        public DateOnly? Dead_line { get; set; }
    }
}
