using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;

namespace Spovyz.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task DeleteProject(Project project, Task_employee[] t_employees, Task_tag[] t_tag, Models.Task[] p_tasks, Project_tag[] p_tag, Project_employee[] p_employees)
        {
            _context.RemoveRange(t_employees);
            _context.RemoveRange(t_tag);
            _context.RemoveRange(p_tasks);
            _context.RemoveRange(p_tag);
            _context.RemoveRange(p_employees);
            _context.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<Project?> GetProjectById(uint ProjectId, uint activeUserId)
        {
            return await _context.Project_employees
                .Include(te => te.Project)
                .Include(te => te.Employee)
                .Where(te => te.Employee.Id == activeUserId)
                .Select(te => te.Project)
                .Where(p => p.Id == ProjectId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Project>> GetProjectList(uint ActiveUserId)
        {
            return await _context.Project_employees
                .Include(te => te.Project)
                .Include(te => te.Employee)
                .Where(te => te.Employee.Id == ActiveUserId)
                .Select(te => te.Project)
                .ToListAsync();
        }
    }
}
