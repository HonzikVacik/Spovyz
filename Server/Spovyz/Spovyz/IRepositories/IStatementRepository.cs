using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.IRepositories
{
    public interface IStatementRepository
    {
        System.Threading.Tasks.Task AddStatement(byte statementType, DateOnly datum, byte pocetHodin, string? Description, Accounting accounting);
        System.Threading.Tasks.Task DeleteStatement(uint ActiveUserId, uint CompanyId, uint id);
        Task<StatementDataShort[]> GetDay(uint ActiveUserId, DateOnly datum);
        Task<StatementDataLong?> GetMonth(uint EmployeeId, byte Month, ushort Year, Accounting accounting);
    }
}
