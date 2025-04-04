using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;

namespace Spovyz.Repositories
{
    public class ProjectTagRepository : IProjectTagRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectTagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Project_tag[]> GetProjectTagByProject(Project project)
        {
            return await _context.Project_tags
                .Include(p => p.Project)
                .Where(p => p.Project == project)
                .ToArrayAsync();
        }
    }
}
