using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;

namespace Spovyz.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task DeleteTask(Models.Task Task, Task_employee[] t_employees, Task_tag[] t_tags)
        {
            _context.RemoveRange(t_employees);
            _context.RemoveRange(t_tags);
            _context.Remove(Task);
            await _context.SaveChangesAsync();
        }

        public async Task<Models.Task?> GetTaskById(uint TaskId)
        {
            return await _context.Tasks
                .Where(t => t.Id == TaskId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Models.Task>> GetTaskList(uint ProjectId)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.Project.Id == ProjectId)
                .ToListAsync();
        }
    }
}
