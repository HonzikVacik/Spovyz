using Spovyz.Models;

namespace Spovyz.IRepositories
{
    public interface ITaskRepository
    {
        Task<List<Models.Task>> GetTaskList(uint ProjectId, uint ActiveUserId);
        Task<string[]?> GetTaskNames(uint ProjectId, uint ActiveUserId);
        Task<Models.Task?> GetTaskById(uint TaskId, uint ActiveUserId);
        System.Threading.Tasks.Task DeleteTask(Models.Task Task, Task_employee[] t_employees, Task_tag[] t_tags);
    }
}
