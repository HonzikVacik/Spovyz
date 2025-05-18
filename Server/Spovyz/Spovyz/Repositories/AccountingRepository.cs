using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;

namespace Spovyz.Repositories
{
    public class AccountingRepository : IAccountingRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task<string?> UpdateMonthSalary(uint CompanyId, uint EmployeeId, uint Salary)
        {
            Employee? employee = await _context.Employees
                .Include(e => e.Company)
                .Where(e => e.Company.Id == CompanyId && e.Id == EmployeeId)
                .FirstOrDefaultAsync();
            if (employee == null)
                return "Employee not found";

            Accounting? accounting = await _context.Accountings
                .Include(ms => ms.Employee)
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
