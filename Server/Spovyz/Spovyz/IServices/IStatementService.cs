using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.IServices
{
    public interface IStatementService
    {
        System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string? error)> AddStatement(string UserName, byte StatementType, DateOnly Datum, byte PocetHodin, string? Description);
        System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string? error)> DeleteStatement(string UserName, uint Id);
        System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string? error, StatementDataShort[]?)> GetDay(string UserName, string EmployeeUserName, byte Day, uint AccountingId);
        System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string? error, StatementDataLong?)> GetMonth(string UserName, uint AccountingId);
    }
}
