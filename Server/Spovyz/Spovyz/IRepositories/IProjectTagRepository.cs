using Spovyz.Models;

namespace Spovyz.IRepositories
{
    public interface IProjectTagRepository
    {
        System.Threading.Tasks.Task<Project_tag[]> GetProjectTagByProject(Project project);
    }
}
