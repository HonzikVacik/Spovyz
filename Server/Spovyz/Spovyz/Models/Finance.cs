namespace Spovyz.Models
{
    public class Finance
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public uint Cost { get; set; }
        public string Description { get; set; }
        public bool Income_expenditure { get; set; }
        public bool Current_planned { get; set; }
        public Employee EmployeeId { get; set; }
    }
}
