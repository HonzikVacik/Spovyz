using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.IRepositories
{
    public interface IStatementRepository
    {
        System.Threading.Tasks.Task AddStatement(byte statementType, DateOnly datum, byte pocetHodin, string? Description, Accounting accounting);
        System.Threading.Tasks.Task DeleteStatement(uint ActiveUserId, uint CompanyId, uint id);
        System.Threading.Tasks.Task<StatementDataShort[]> GetDay(uint UserId, DateOnly datum);
        System.Threading.Tasks.Task<StatementDataLong?> GetMonth(Accounting accounting);
    }
}
