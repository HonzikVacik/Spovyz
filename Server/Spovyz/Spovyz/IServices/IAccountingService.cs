using Spovyz.Transport_models;

namespace Spovyz.IServices
{
    public interface IAccountingService
    {
        System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string? error, AccountingDataShort[]?)> Get(string UserName);
        System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string? error, AccountingDataLong[]?)> GetAll(string UserName);
    }
}
