using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.IRepositories
{
    public interface IAccountingRepository
    {
        System.Threading.Tasks.Task<Accounting?> GetAccounting(uint CompanyId, uint EmployeeId, DateOnly Date);
        System.Threading.Tasks.Task<Accounting?> GetAccountingById(uint AccountingId);
        System.Threading.Tasks.Task<string?> SetAccounting(uint CompanyId, uint EmployeeId, uint Salary);
        System.Threading.Tasks.Task<AccountingDataShort[]> Get(uint ActiveUserId);
        System.Threading.Tasks.Task<AccountingDataLong[]> GetAll(uint CompanyId);
    }
}
