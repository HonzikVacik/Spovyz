using Spovyz.IServices;
using Spovyz.Transport_models;

namespace Spovyz.Services
{
    public class FinanceService : IFinanceService
    {
        public Task<(ValidityControl.ResultStatus, string?)> AddFinance(string UserName, string Name, uint Cost, string? Description, bool Income_expenditure, bool Current_planned)
        {
            throw new NotImplementedException();
        }

        public Task<(ValidityControl.ResultStatus, string?)> DeleteFinance(string UserName, uint Id)
        {
            throw new NotImplementedException();
        }

        public Task<(List<NameBasic>, string?)> GetAllFinances(string UserName, bool Income_expenditure, bool Current_planned)
        {
            throw new NotImplementedException();
        }

        public Task<(FinanceData, string?)> GetFinanceById(string UserName, uint Id)
        {
            throw new NotImplementedException();
        }

        public Task<(ValidityControl.ResultStatus, string?)> UpdateFinance(string UserName, uint Id, string Name, uint Cost, string? Description, bool Income_expenditure, bool Current_planned)
        {
            throw new NotImplementedException();
        }
    }
}
