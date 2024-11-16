namespace Spovyz.Models
{
    public class Accounting
    {
        public required uint Id { get; set; }
        public required Enums.Month Month { get; set; }
        public required Employee EmployeeId { get; set; }
        public required uint Pay { get; set; }
    }
}
