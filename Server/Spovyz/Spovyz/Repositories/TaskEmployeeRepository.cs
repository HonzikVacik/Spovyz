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

        public async System.Threading.Tasks.Task<Task_employee[]> GetTaskEmployeeByTask(Models.Task task)
        {
            return await _context.Task_employees
                    .Include(t => t.Task)
                    .Where(t => t.Task == task)
                    .ToArrayAsync();
        }
    }
}
