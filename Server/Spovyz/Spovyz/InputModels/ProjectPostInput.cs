using System.ComponentModel.DataAnnotations;

namespace Spovyz.InputModels
{
    public class ProjectPostInput
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public DateOnly? DeadLine { get; set; }
        [Required]
        public string[] Tags { get; set; }
        [Required]
        public uint[] Employees { get; set; }
    }
}
