using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;

namespace Spovyz.Repositories
{
    public class TaskTagRepository : ITaskTagRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskTagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Task_tag[]> GetTaskTagByTask(Models.Task task)
        {
            return await _context.Task_tags
                .Include(t => t.Task)
                .Where(t => t.Task == task)
                .ToArrayAsync();
        }
    }
}
