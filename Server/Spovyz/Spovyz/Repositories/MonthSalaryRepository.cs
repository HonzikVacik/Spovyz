using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;

namespace Spovyz.Repositories
{
    public class MonthSalaryRepository : IMonthSalaryRepository
    {
        private readonly ApplicationDbContext _context;

        public MonthSalaryRepository(ApplicationDbContext context)
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

            MonthSalary? monthSalary = await _context.MonthSalaries
                .Include(ms => ms.Employee)
                .Where(ms => ms.Employee.Company.Id == CompanyId && ms.Employee.Id == EmployeeId && ms.Month == DateTime.Now.Month && ms.Year == DateTime.Now.Year)
                .FirstOrDefaultAsync();
            
            if (monthSalary != null)
            {
                monthSalary.Salary = Salary;
                _context.MonthSalaries.Update(monthSalary);
            }
            else
            {
                monthSalary = new MonthSalary
                {
                    Month = (byte)DateTime.Now.Month,
                    Year = (ushort)DateTime.Now.Year,
                    Salary = Salary,
                    Employee = employee
                };
                _context.MonthSalaries.Add(monthSalary);
            }

            await _context.SaveChangesAsync();
            return null;
        }
    }
}
