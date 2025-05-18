using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.IServices;
using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.Services
{
    public class StatementService : IStatementService
    {
        private readonly ApplicationDbContext _context;
        private readonly IStatementRepository _statementRepository;

        public StatementService(ApplicationDbContext context, IStatementRepository statementRepository)
        {
            _context = context;
            _statementRepository = statementRepository;
        }

        public async Task<(ValidityControl.ResultStatus, string? error)> AddStatement(string UserName, uint? ProjectId, uint? TaskId, byte statementType, DateOnly datum, byte pocetHodin, string? Description)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found");

            throw new NotImplementedException();
        }

        public async Task<(ValidityControl.ResultStatus, string? error)> DeleteStatement(string UserName, uint id)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found");

            throw new NotImplementedException();
        }

        public async Task<(ValidityControl.ResultStatus, string? error, StatementDataShort?)> GetDay(string UserName, DateOnly datum)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found", null);

            throw new NotImplementedException();
        }

        public async Task<(ValidityControl.ResultStatus, string? error, StatementDataLong?)> GetMonth(string UserName, uint EmployeeId, byte Day, short Month)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found", null);

            throw new NotImplementedException();
        }
    }
}
