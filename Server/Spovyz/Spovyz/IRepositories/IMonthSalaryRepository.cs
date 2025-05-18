namespace Spovyz.IRepositories
{
    public interface IMonthSalaryRepository
    {
        System.Threading.Tasks.Task<string?> UpdateMonthSalary(uint CompanyId, uint EmployeeId, uint Salary);
    }
}
