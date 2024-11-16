namespace Spovyz.Models
{
    public class Finance
    {
        public required uint Id { get; set; }
        public required string Name { get; set; }
        public required uint Cost { get; set; }
        public string? Description { get; set; }
        public required bool Income_expenditure { get; set; }
        public required bool Current_planned { get; set; }
        public required Employee EmployeeId { get; set; }
    }
}
