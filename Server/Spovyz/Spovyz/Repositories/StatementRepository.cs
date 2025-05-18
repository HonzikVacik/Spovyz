using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;
using Spovyz.Transport_models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Spovyz.Repositories
{
    public class StatementRepository : IStatementRepository
    {
        private readonly ApplicationDbContext _context;

        public StatementRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task AddStatement(byte statementType, DateOnly datum, byte pocetHodin, string? Description, Accounting accounting)
        {
            Statement statement = new Statement
            {
                Accounting = accounting,
                Day = (byte)datum.Day,
                Statement_type = (Enums.StatementType)statementType,
                Number_of_hours = pocetHodin,
                Description = Description
            };
            _context.Statements.Add(statement);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteStatement(uint ActiveUserId, uint CompanyId, uint id)
        {
            Statement? statement = await _context.Statements
                .Include(s => s.Accounting)
                .ThenInclude(a => a.Employee)
                .ThenInclude(e => e.Company)
                .Where(s => s.Id == id && s.Accounting.Employee.Company.Id == CompanyId && s.Accounting.Employee.Id == ActiveUserId)
                .FirstOrDefaultAsync();
            if (statement != null)
            {
                _context.Remove(statement);
                await _context.SaveChangesAsync();
            }
        }

        public async System.Threading.Tasks.Task<StatementDataShort[]> GetDay(uint ActiveUserId, DateOnly datum)
        {
            List<Statement>? statements = await _context.Statements
                .Include(s => s.Accounting)
                .ThenInclude(a => a.Employee)
                .ThenInclude(e => e.Company)
                .Where(s => s.Accounting.Employee.Id == ActiveUserId && s.Day == datum.Day && s.Accounting.Month == (Enums.Month)datum.Month && s.Accounting.Year == datum.Year)
                .ToListAsync();

            List<StatementDataShort> statementDataShorts = new List<StatementDataShort>();
            foreach (Statement statement in statements)
            {
                StatementDataShort statementDataShort = new StatementDataShort
                {
                    Datum = datum,
                    PocetHodin = statement.Number_of_hours,
                    Description = statement.Description,
                };
                statementDataShorts.Add(statementDataShort);
            }
            return statementDataShorts.ToArray();
        }

        public async System.Threading.Tasks.Task<StatementDataLong?> GetMonth(uint EmployeeId, byte Month, ushort Year, Accounting accounting)
        {
            uint NumberOfHours = 0;

            NumberOfHours += (uint) await _context.Statements
                .Include(s => s.Accounting)
                .ThenInclude(a => a.Employee)
                .ThenInclude(e => e.Company)
                .Where(s => s.Accounting.Employee.Id == EmployeeId && s.Accounting.Month == (Enums.Month)Month && s.Accounting.Year == Year)
                .SumAsync(s => s.Number_of_hours);

            byte[] days = await _context.Statements
                .Include(s => s.Accounting)
                .ThenInclude(a => a.Employee)
                .ThenInclude(e => e.Company)
                .Where(s => s.Accounting.Employee.Id == EmployeeId && s.Accounting.Month == (Enums.Month)Month && s.Accounting.Year == Year)
                .Select(s => s.Day)
                .Distinct()
                .ToArrayAsync();

            StatementDataLong statementDataLong = new StatementDataLong
            {
                EmployeeName = accounting.Employee.Username,
                EmployeeSalary = accounting.Salary,
                Den = days,
                Mesic = (byte)accounting.Month,
                Rok = accounting.Year,
                PocetHodin = NumberOfHours
            };

            return statementDataLong;
        }
    }
}
