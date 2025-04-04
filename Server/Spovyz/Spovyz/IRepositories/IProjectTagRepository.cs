using Spovyz.Models;

namespace Spovyz.IRepositories
{
    public interface IProjectTagRepository
    {
        Task<Project_tag[]> GetProjectTagByProject(Project project);
    }
}
