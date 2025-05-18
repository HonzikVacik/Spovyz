namespace Spovyz.Models
{
    public class MonthSalary
    {
        public uint Id { get; set; }
        public byte Month { get; set; }
        public ushort Year { get; set; }
        public decimal Salary { get; set; }
        public Employee Employee { get; set; } = null!;
    }
}
