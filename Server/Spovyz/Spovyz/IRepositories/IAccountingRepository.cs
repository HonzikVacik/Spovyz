using Spovyz.Models;

namespace Spovyz.IRepositories
{
    public interface IAccountingRepository
    {
        System.Threading.Tasks.Task<Accounting?> GetAccounting(uint CompanyId, uint EmployeeId, DateOnly Date);
        System.Threading.Tasks.Task<string?> SetAccounting(uint CompanyId, uint EmployeeId, uint Salary);
    }
}
