using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.IRepositories
{
    public interface ITaskRepository
    {
        Task<List<Models.Task>> GetTaskList(uint ProjectId, uint ActiveUserId);
        Task<NameBasic[]?> GetTaskNames(Project Project, uint ActiveUserId);
        Task<Models.Task?> GetTaskById(uint TaskId, uint ActiveUserId);
        System.Threading.Tasks.Task DeleteTask(Models.Task Task, Task_employee[] t_employees, Task_tag[] t_tags);
        System.Threading.Tasks.Task PostTask(string Name, string? Description, Project Project, DateOnly? DeadLine, Enums.Status Status, Employee[] Employees, uint[] TaskIds)
        Task<string?> PutTask(uint ActiveUserId, uint TaskId, string Name, string? Description, Project Project, DateOnly? DeadLine, Enums.Status Status, Employee[] Employees, uint[] TaskIds);
    }
}
