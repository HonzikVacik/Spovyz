using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;

namespace Spovyz.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<uint[]?> GetEmployeesIdsByProjectId(uint ProjectId)
        {
            return await _context.Project_employees
                .Where(pe => pe.Project.Id == ProjectId)
                .Select(pe => pe.Employee.Id)
                .ToArrayAsync();
        }
    }
}
