namespace Spovyz.Transport_models
{
    public class ProjectCardData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public uint Customer {  get; set; }
        public uint Status { get; set; }
        public DateOnly? Deadline { get; set; }
        public string WorkedOut { get; set; }
        public string WorkedByMe { get; set; }
        public string[]? Tags { get; set; }
        public uint[] Employees { get; set; }
        public NameBasic[]? Tasks { get; set; }
    }
}
