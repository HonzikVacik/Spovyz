namespace Spovyz.IRepositories
{
    public interface ITaskTagRepository
    {
        System.Threading.Tasks.Task<Models.Task_tag[]> GetTaskTagByTask(Models.Task task);
    }
}
