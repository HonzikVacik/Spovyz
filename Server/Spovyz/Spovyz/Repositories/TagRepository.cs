using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;

namespace Spovyz.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string[]?> GetTagNamesByProject(uint ProjectId)
        {
            return await _context.Project_tags
                .Include(pt => pt.Project)
                .Include(pt => pt.Tag)
                .Where(pt => pt.Project.Id == ProjectId)
                .Select(pt => pt.Tag.Name)
                .ToArrayAsync();
        }

        public async Task<string[]?> GetTagNamesByTask(uint TaskId)
        {
            return await _context.Task_tags
                .Include(tt => tt.Task)
                .Include(tt => tt.Tag)
                .Where(tt => tt.Task.Id == TaskId)
                .Select(tt => tt.Tag.Name)
                .ToArrayAsync();
        }
    }
}
