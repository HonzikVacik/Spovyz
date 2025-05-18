namespace Spovyz.Transport_models
{
    public class StatementDataLong
    {
        public string EmployeeName { get; set; } = string.Empty;
        public decimal EmployeeSalary { get; set; }
        public byte[] Den { get; set; }
        public byte Mesic { get; set; }
        public ushort Rok { get; set; }
        public uint PocetHodin { get; set; }
    }
}
