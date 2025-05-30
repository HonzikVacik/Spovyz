using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.Repositories
{
    public class AccountingRepository : IAccountingRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AccountingDataShort[]> Get(uint ActiveUserId)
        {
            List<Accounting> accountings = await _context.Accountings
                .Include(a => a.Employee)
                .Where(a => a.Employee.Id == ActiveUserId)
                .OrderByDescending(a => a.Year)
                .ThenByDescending(a => a.Month)
                .ToListAsync();
            
            AccountingDataShort[] accountingDataShorts = new AccountingDataShort[accountings.Count];

            for (int i = 0; i < accountings.Count; i++)
            {
                accountingDataShorts[i] = new AccountingDataShort
                {
                    Id = accountings[i].Id,
                    Month = (byte)accountings[i].Month,
                    Year = accountings[i].Year,
                    Salary = accountings[i].Salary
                };
            }
            
            return accountingDataShorts;
        }

        public async Task<Accounting?> GetAccounting(uint CompanyId, uint EmployeeId, DateOnly Date)
        {
            return await _context.Accountings
                .Include(a => a.Employee)
                .ThenInclude(e => e.Company)
                .Where(a => a.Employee.Company.Id == CompanyId && a.Employee.Id == EmployeeId && a.Month == (Enums.Month)Date.Month && a.Year == Date.Year)
                .FirstOrDefaultAsync();
        }

        public async Task<Accounting?> GetAccountingById(uint AccountingId)
        {
            return await _context.Accountings
                .Include(a => a.Employee)
                .ThenInclude(e => e.Company)
                .Where(a => a.Id == AccountingId)
                .FirstOrDefaultAsync();
        }

        public async Task<AccountingDataLong[]> GetAll(uint CompanyId)
        {
            List<Accounting> accountings = await _context.Accountings
                .Include(a => a.Employee)
                .ThenInclude(e => e.Company)
                .Where(a => a.Employee.Company.Id == CompanyId)
                .OrderByDescending(a => a.Year)
                .ThenByDescending(a => a.Month)
                .ThenByDescending(a => a.Employee.Username)
                .ToListAsync();

            AccountingDataLong[] accountingDataLongs = new AccountingDataLong[accountings.Count];

            for (int i = 0; i < accountings.Count; i++)
            {
                accountingDataLongs[i] = new AccountingDataLong
                {
                    Id = accountings[i].Id,
                    Month = (byte)accountings[i].Month,
                    Year = accountings[i].Year,
                    Salary = accountings[i].Salary,
                    EmployeeName = accountings[i].Employee.Username
                };
            }

            return accountingDataLongs;
        }

        public async System.Threading.Tasks.Task<string?> SetAccounting(uint CompanyId, uint EmployeeId, uint Salary)
        {
            Employee? employee = await _context.Employees
                .Include(e => e.Company)
                .Where(e => e.Company.Id == CompanyId && e.Id == EmployeeId)
                .FirstOrDefaultAsync();
            if (employee == null)
                return "Employee not found";

            Accounting? accounting = await _context.Accountings
                .Include(ms => ms.Employee)
                .ThenInclude(e => e.Company)
                .Where(ms => ms.Employee.Company.Id == CompanyId && ms.Employee.Id == EmployeeId && ms.Month == (Enums.Month)DateTime.Now.Month && ms.Year == DateTime.Now.Year)
                .FirstOrDefaultAsync();
            
            if (accounting != null)
            {
                accounting.Salary = Salary;
                _context.Accountings.Update(accounting);
            }
            else
            {
                accounting = new Accounting
                {
                    Month = (Enums.Month)DateTime.Now.Month,
                    Year = (ushort)DateTime.Now.Year,
                    Salary = Salary,
                    Employee = employee
                };
                _context.Accountings.Add(accounting);
            }

            await _context.SaveChangesAsync();
            return null;
        }
    }
}
