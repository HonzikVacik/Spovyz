namespace Spovyz.Models
{
    public class Accounting
    {
        public uint Id { get; set; }
        public Enums.Month Month { get; set; }
        public Employee EmployeeId { get; set; }
        public uint Pay { get; set; }
    }
}
