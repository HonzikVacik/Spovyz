using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;

namespace Spovyz.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task<string[]?> GetTagNamesByProject(uint ProjectId)
        {
            return await _context.Project_tags
                .Include(pt => pt.Project)
                .Include(pt => pt.Tag)
                .Where(pt => pt.Project.Id == ProjectId)
                .Select(pt => pt.Tag.Name)
                .ToArrayAsync();
        }

        public async System.Threading.Tasks.Task<string[]?> GetTagNamesByTask(uint TaskId)
        {
            return await _context.Task_tags
                .Include(tt => tt.Task)
                .Include(tt => tt.Tag)
                .Where(tt => tt.Task.Id == TaskId)
                .Select(tt => tt.Tag.Name)
                .ToArrayAsync();
        }

        public async System.Threading.Tasks.Task<Tag> PostGetTag(string TagName)
        {
            Tag? tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == TagName);
            if(tag == null)
            {
                tag = new Tag { Name = TagName };
                _context.Tags.Add(tag);
                _context.SaveChanges();
                return await _context.Tags.FirstOrDefaultAsync(t => t.Name == TagName);
            }
            return tag;
        }

        public async System.Threading.Tasks.Task<Tag[]> PostGetTags(string[] TagNames)
        {
            List<Tag> tags = new List<Tag>();
            foreach (string tagName in TagNames)
            {
                Tag? tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName };
                    _context.Tags.Add(tag);
                    _context.SaveChanges();
                    tags.Add(await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName));
                }
                tags.Add(tag);
            }
            return tags.ToArray();
        }
    }
}
