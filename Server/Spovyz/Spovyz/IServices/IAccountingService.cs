using Spovyz.Transport_models;

namespace Spovyz.IServices
{
    public interface IAccountingService
    {
        Task<(ValidityControl.ResultStatus, string? error, AccountingDataShort[]?)> Get(string UserName);
        Task<(ValidityControl.ResultStatus, string? error, AccountingDataLong[]?)> GetAll(string UserName);
    }
}
