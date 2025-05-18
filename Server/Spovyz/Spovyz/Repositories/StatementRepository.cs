using Spovyz.IRepositories;
using Spovyz.Transport_models;

namespace Spovyz.Repositories
{
    public class StatementRepository : IStatementRepository
    {
        private readonly ApplicationDbContext _context;

        public StatementRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddStatement(uint? ProjectId, uint? TaskId, byte statementType, DateOnly datum, byte pocetHodin, string? Description)
        {
            throw new NotImplementedException();
        }

        public Task DeleteStatement(uint ActiveUserId, uint CompanyId, uint id)
        {
            throw new NotImplementedException();
        }

        public Task<StatementDataShort?> GetDay(uint ActiveUserId, DateOnly datum)
        {
            throw new NotImplementedException();
        }

        public Task<StatementDataLong?> GetMonth(uint ActiveUserId, uint EmployeeId, byte Day, short Month)
        {
            throw new NotImplementedException();
        }
    }
}
