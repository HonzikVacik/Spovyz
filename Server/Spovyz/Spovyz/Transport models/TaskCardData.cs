using System;

namespace Spovyz.Transport_models
{
    public class TaskCardData
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public uint ProjectId { get; set; }
        public uint Status { get; set; }
        public DateOnly? Deadline { get; set; }
        public uint? Remains { get; set; }
        public string[]? Tags { get; set; }
        public uint[] Employees { get; set; }
    }
}
