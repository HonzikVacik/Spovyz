using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeSalary[]> EmployeeSalary(uint CompanyId)
        {
            Transport_models.EmployeeSalary[] employeeSalaries = await _context.Employees
                .Where(e => e.Company.Id == CompanyId)
                .Select(e => new Transport_models.EmployeeSalary
                {
                    EmployeeId = e.Id,
                    EmployeeName = e.Username,
                    Salary = e.Pay
                }).ToArrayAsync();
            return employeeSalaries;
        }

        public async Task<Employee[]> GetAllEmployees(uint CompanyId)
        {
            return await _context.Employees
                .Where(e => e.Company.Id == CompanyId)
                .ToArrayAsync();
        }

        public async Task<Employee[]> GetEmployeesByIds(uint[] employees)
        {
            return await _context.Employees
                .Where(e => employees.Contains(e.Id))
                .ToArrayAsync();
        }

        public async Task<uint[]?> GetEmployeesIdsByProjectId(uint ProjectId)
        {
            return await _context.Project_employees
                .Where(pe => pe.Project.Id == ProjectId)
                .Select(pe => pe.Employee.Id)
                .ToArrayAsync();
        }

        public async Task<uint[]> GetEmployeesIdsByTaskId(uint TaskId)
        {
            return await _context.Task_employees
                .Where(te => te.Task.Id == TaskId)
                .Select(te => te.Employee.Id)
                .ToArrayAsync();
        }

        public async System.Threading.Tasks.Task UpdateEmployeeSalary(uint companyId, uint employeeId, uint salary)
        {
            await _context.Employees
                .Where(e => e.Company.Id == companyId && e.Id == employeeId)
                .ExecuteUpdateAsync(e => e.SetProperty(x => x.Pay, salary));
        }
    }
}
