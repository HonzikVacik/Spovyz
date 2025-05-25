namespace Spovyz.Models
{
    public class Project_tag
    {
        public uint ProjectId { get; set; }
        public Project Project { get; set; }
        public uint TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
