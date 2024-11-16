namespace Spovyz.Models
{
    public class Accounting
    {
        public uint Id { get; set; }
        public Enums.Month Month { get; set; }
        public Employee Employee { get; set; }
        public uint Pay { get; set; }
    }
}
