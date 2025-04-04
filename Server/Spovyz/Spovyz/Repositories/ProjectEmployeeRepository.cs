using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;

namespace Spovyz.Repositories
{
    public class ProjectEmployeeRepository : IProjectEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectEmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Project_employee[]> GetProjectEmployeeByProject(Project project)
        {
            return await _context.Project_employees
                .Include(p => p.Project)
                .Where(p => p.Project == project)
                .ToArrayAsync();
        }
    }
}
