
using Spovyz.Models;

namespace Spovyz.IRepositories
{
    public interface IProjectRepository
    {
        Task<List<Models.Project>> GetProjectList(uint ActiveUserId);
        Task<Models.Project?> GetProjectById(uint ProjectId, uint activeUserId);
        System.Threading.Tasks.Task DeleteProject(Project project, Project_Tag[] t_employees, Task_tag[] t_tag, Models.Task[] p_tasks, Project_tag[] p_tag, Project_employee[] p_employees);
        System.Threading.Tasks.Task PostProject(string Name, string? Description, Customer Customer, DateOnly? DeadLine, Employee[] Employees, Tag[] Tags);
        System.Threading.Tasks.Task<string?> PutProject(uint ActiveUserId, uint ProjectId, string Name, string? Description, Customer Customer, DateOnly? DeadLine, Enums.Status Status, Employee[] Employees, Tag[] Tags);
    }
}
