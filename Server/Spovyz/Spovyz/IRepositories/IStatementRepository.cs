using Spovyz.Transport_models;

namespace Spovyz.IRepositories
{
    public interface IStatementRepository
    {
        Task AddStatement(uint? ProjectId, uint? TaskId, byte statementType, DateOnly datum, byte pocetHodin, string? Description);
        Task DeleteStatement(uint ActiveUserId, uint CompanyId, uint id);
        Task<StatementDataShort?> GetDay(uint ActiveUserId, DateOnly datum);
        Task<StatementDataLong?> GetMonth(uint ActiveUserId, uint EmployeeId, byte Day, short Month);
    }
}
