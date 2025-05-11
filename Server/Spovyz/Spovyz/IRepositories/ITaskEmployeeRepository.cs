using Spovyz.Models;

namespace Spovyz.IRepositories
{
    public interface ITaskEmployeeRepository
    {
        Task<Task_employee[]> GetTaskEmployeeByTask(Models.Task task);
    }
}
