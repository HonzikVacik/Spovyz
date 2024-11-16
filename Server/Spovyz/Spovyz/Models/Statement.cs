namespace Spovyz.Models
{
    public class Statement
    {
        public uint Id { get; set; }
        public Accounting Accounting { get; set; }
        public byte Day { get; set; }
        public Enums.StatementType Statement_type { get; set; }
        public byte Number_of_hours { get; set; }
        public string? Description { get; set; }
    }
}
