using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.IRepositories
{
    public interface IFinanceRepository
    {
        System.Threading.Tasks.Task<FinanceResult> GetFinanceResult(uint CompanyId, bool Current_planned);
        System.Threading.Tasks.Task<List<NameBasic>?> GetAllFinances(uint CompanyId, bool Income_expenditure, bool Current_planned);
        System.Threading.Tasks.Task<Finance?> GetFinanceById(uint CompanyId, uint Id);
        System.Threading.Tasks.Task DeleteFinance(Finance finance);
        System.Threading.Tasks.Task AddTask(Employee ActiveUser, string Name, uint Cost, string? Description, bool Income_expenditure, bool Current_planned);
        System.Threading.Tasks.Task UpdateTask(Finance finance, string Name, uint Cost, string? Description, bool Income_expenditure, bool Current_planned);
    }
}
