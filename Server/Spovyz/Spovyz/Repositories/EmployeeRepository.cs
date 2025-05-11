using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;

namespace Spovyz.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
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
    }
}
