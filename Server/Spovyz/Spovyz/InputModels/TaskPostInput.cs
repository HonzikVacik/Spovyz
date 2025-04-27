using System.ComponentModel.DataAnnotations;

namespace Spovyz.InputModels
{
    public class TaskPostInput
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public uint ProjectId { get; set; }
        public DateOnly? DeadLine { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public string[] Tags { get; set; }
        [Required]
        public uint[] Employees { get; set; }
    }
}
