namespace Spovyz.Models
{
    public class Project_employee
    {
        public uint ProjectId { get; set; }
        public Project Project { get; set; }
        public uint EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
