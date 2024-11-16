namespace Spovyz.Models
{
    public class Statement
    {
        public required uint Id { get; set; }
        public required Accounting AccountingId { get; set; }
        public required byte Day { get; set; }
        public required Enums.StatementType Statement_type { get; set; }
        public required byte Number_of_hours { get; set; }
        public string? Description { get; set; }
    }
}
