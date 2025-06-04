using Spovyz.Models;

namespace Spovyz.IRepositories
{
    public interface ITagRepository
    {
        System.Threading.Tasks.Task<string[]?> GetTagNamesByProject(uint ProjectId);
        System.Threading.Tasks.Task<string[]?> GetTagNamesByTask(uint TaskId);
        System.Threading.Tasks.Task<Tag> PostGetTag(string TagName);
        System.Threading.Tasks.Task<Tag[]> PostGetTags(string[] TagNames);
    }
}
