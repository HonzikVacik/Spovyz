﻿namespace Spovyz.Models
{
    public class Task
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Project ProjectId { get; set; }
    }
}
