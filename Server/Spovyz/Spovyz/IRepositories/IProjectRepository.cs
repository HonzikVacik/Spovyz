
using Spovyz.Models;

namespace Spovyz.IRepositories
{
    public interface IProjectRepository
    {
        Task<List<Models.Project>> GetProjectList(uint ActiveUserId);
        Task<Models.Project?> GetProjectById(uint ProjectId, uint activeUserId);
        System.Threading.Tasks.Task DeleteProject(Project project, Task_employee[] t_employees, Task_tag[] t_tag, Models.Task[] p_tasks, Project_tag[] p_tag, Project_employee[] p_employees);
    }
}
