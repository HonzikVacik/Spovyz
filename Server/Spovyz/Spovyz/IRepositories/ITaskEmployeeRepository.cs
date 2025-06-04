using Spovyz.Models;

namespace Spovyz.IRepositories
{
    public interface ITaskEmployeeRepository
    {
        System.Threading.Tasks.Task<Task_employee[]> GetTaskEmployeeByTask(Models.Task task);
    }
}
