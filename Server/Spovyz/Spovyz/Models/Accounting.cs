namespace Spovyz.Models
{
    public class Accounting
    {
        public uint Id { get; set; }
        public Enums.Month Month { get; set; }
        public ushort Year { get; set; }
        public uint Salary { get; set; }
        public Employee Employee { get; set; }
    }
}
