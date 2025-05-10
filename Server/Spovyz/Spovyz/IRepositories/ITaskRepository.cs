using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.IRepositories
{
    public interface ITaskRepository
    {
        Task<List<Models.Task>> GetTaskList(uint ProjectId, uint ActiveUserId);
        Task<NameBasic[]?> GetTaskNames(Project Project, uint ActiveUserId);
        Task<Models.Task?> GetTaskById(uint TaskId, uint ActiveUserId);
        System.Threading.Tasks.Task DeleteTask(Models.Task Task, Project_Tag[] t_employees, Task_tag[] t_tags);
        System.Threading.Tasks.Task PostTask(string Name, string? Description, Project Project, DateOnly? DeadLine, Employee[] Employees, Tag[] Tags);
        Task<string?> PutTask(uint ActiveUserId, Models.Task Task, string Name, string? Description, Project Project, DateOnly? DeadLine, Enums.Status Status, Employee[] Employees, Tag[] Tags);
    }
}
