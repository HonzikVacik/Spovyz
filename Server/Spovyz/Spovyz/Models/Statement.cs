namespace Spovyz.Models
{
    public class Statement
    {
        public uint Id { get; set; }
        public Accounting AccountingId { get; set; }
        public byte Day { get; set; }
        public bool Statement_type { get; set; }
        public byte Number_of_hours { get; set; }
        public string Description { get; set; }
    }
}
