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
        public uint CustomerId { get; set; }
        public DateOnly? DeadLine { get; set; }
        public string[] Tags { get; set; }
        [Required]
        public uint[] Employees { get; set; }
    }
}
