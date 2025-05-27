namespace Spovyz.Models
{
    public class Task_employee
    {
        public uint TaskId { get; set; }
        public Task Task { get; set; }
        public uint EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
