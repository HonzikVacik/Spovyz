namespace Spovyz.IRepositories
{
    public interface ITaskTagRepository
    {
        Task<Models.Task_tag[]> GetTaskTagByTask(Models.Task task);
    }
}
