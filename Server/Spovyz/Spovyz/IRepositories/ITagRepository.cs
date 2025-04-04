namespace Spovyz.IRepositories
{
    public interface ITagRepository
    {
        Task<string[]?> GetTagNamesByProject(uint ProjectId);
    }
}
