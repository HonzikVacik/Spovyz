using Spovyz.Models;

namespace Spovyz.IRepositories
{
    public interface ITagRepository
    {
        Task<string[]?> GetTagNamesByProject(uint ProjectId);
        Task<string[]?> GetTagNamesByTask(uint TaskId);
        Task<Tag> PostGetTag(string TagName);
    }
}
