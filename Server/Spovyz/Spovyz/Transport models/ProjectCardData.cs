namespace Spovyz.Transport_models
{
    public class ProjectCardData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Customer {  get; set; }
        public int Status { get; set; }
        public DateOnly Deathline { get; set; }
        public string WorkedOut { get; set; }
        public string WorkedByMe { get; set; }
        public NameBasic[] Tags { get; set; }
        public int[] Employees { get; set; }
        public NameBasic Tasks { get; set; }
    }
}
