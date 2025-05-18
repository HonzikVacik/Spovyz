using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.IServices
{
    public interface IStatementService
    {
        Task<(ValidityControl.ResultStatus, string? error)> AddStatement(string UserName, byte StatementType, DateOnly Datum, byte PocetHodin, string? Description);
        Task<(ValidityControl.ResultStatus, string? error)> DeleteStatement(string UserName, uint Id);
        Task<(ValidityControl.ResultStatus, string? error, StatementDataShort[]?)> GetDay(string UserName, DateOnly Datum);
        Task<(ValidityControl.ResultStatus, string? error, StatementDataLong?)> GetMonth(string UserName, uint EmployeeId, byte Month, ushort Year);
    }
}
