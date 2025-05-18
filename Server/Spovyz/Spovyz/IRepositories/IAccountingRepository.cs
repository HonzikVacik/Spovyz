namespace Spovyz.IRepositories
{
    public interface IAccountingRepository
    {
        System.Threading.Tasks.Task<string?> UpdateMonthSalary(uint CompanyId, uint EmployeeId, uint Salary);
    }
}
