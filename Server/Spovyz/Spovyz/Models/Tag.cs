namespace Spovyz.Models
{
    public class Tag
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public ICollection<Project_tag> Project_tags { get; set; }
    }
}
