using Spovyz.Models;

namespace Spovyz.IRepositories
{
    public interface ITaskEmployeeRepository
    {
        Task<Project_Tag[]> GetTaskEmployeeByTask(Models.Task task);
    }
}
