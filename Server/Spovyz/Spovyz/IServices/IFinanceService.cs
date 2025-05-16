using Spovyz.Transport_models;

namespace Spovyz.IServices
{
    public interface IFinanceService
    {
        Task<(FinanceData, string?)> GetFinanceById(string UserName, uint Id);
        Task<(List<NameBasic>, string?)> GetAllFinances(string UserName, bool Income_expenditure, bool Current_planned);
        Task<(ValidityControl.ResultStatus, string?)> AddFinance(string UserName, string Name, uint Cost, string? Description, bool Income_expenditure, bool Current_planned);
        Task<(ValidityControl.ResultStatus, string?)> UpdateFinance(string UserName, uint Id, string Name, uint Cost, string? Description, bool Income_expenditure, bool Current_planned);
        Task<(ValidityControl.ResultStatus, string?)> DeleteFinance(string UserName, uint Id);
    }
}
