using System.ComponentModel.DataAnnotations;

namespace Spovyz.InputModels
{
    public class ProjectPutInput
    {
        [Required]
        public uint ProjectId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public uint CustomerId { get; set; }
        [Required]
        public DateOnly? DeadLine { get; set; }
        [Required]
        public int Status { get; set; }
        public string[] Tags { get; set; }
        [Required]
        public uint[] Employees { get; set; }
    }
}
