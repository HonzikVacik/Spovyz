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

        public async Task<Models.Task?> GetTaskById(uint TaskId, uint ActiveUserId)
        {
            return await _context.Task_employees
                .Include(t => t.Task)
                .Include(t => t.Emlployee)
                .Where(t => t.Task.Id == TaskId && t.Emlployee.Id == ActiveUserId)
                .Select(t => t.Task)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Models.Task>> GetTaskList(uint ProjectId, uint ActiveUserId)
        {
            return await _context.Task_employees
                .Include(t => t.Task)
                .Include(t => t.Emlployee)
                .Where(t => t.Task.Project.Id == ProjectId && t.Emlployee.Id == ActiveUserId)
                .Select(t => t.Task)
                .ToListAsync();
        }

        public async Task<string[]?> GetTaskNames(uint ProjectId, uint ActiveUserId)
        {
            return await _context.Task_employees
                .Include(t => t.Task)
                .Include(t => t.Emlployee)
                .Where(t => t.Task.Project.Id == ProjectId && t.Emlployee.Id == ActiveUserId)
                .Select(t => t.Task.Name)
                .ToArrayAsync();
        }
    }
}
