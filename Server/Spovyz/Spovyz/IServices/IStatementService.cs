using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.IServices
{
    public interface IStatementService
    {
        Task<(ValidityControl.ResultStatus, string? error)> AddStatement(string UserName, uint? ProjectId, uint? TaskId, byte statementType, DateOnly datum, byte pocetHodin, string? Description);
        Task<(ValidityControl.ResultStatus, string? error)> DeleteStatement(string UserName, uint id);
        Task<(ValidityControl.ResultStatus, string? error, StatementDataShort?)> GetDay(string UserName, DateOnly datum);
        Task<(ValidityControl.ResultStatus, string? error, StatementDataLong?)> GetMonth(string UserName, uint EmployeeId, byte Day, short Month);
    }
}
