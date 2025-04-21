using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;

namespace Spovyz.Repositories
{
    public class TaskEmployeeRepository : ITaskEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskEmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Project_Tag[]> GetTaskEmployeeByTask(Models.Task task)
        {
            return await _context.Task_employees
                    .Include(t => t.Task)
                    .Where(t => t.Task == task)
                    .ToArrayAsync();
        }
    }
}
