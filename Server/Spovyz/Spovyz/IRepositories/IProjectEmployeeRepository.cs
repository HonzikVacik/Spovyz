using Spovyz.Models;

namespace Spovyz.IRepositories
{
    public interface IProjectEmployeeRepository
    {
        System.Threading.Tasks.Task<Project_employee[]> GetProjectEmployeeByProject(Project project);
    }
}
