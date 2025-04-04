using Spovyz.Models;

namespace Spovyz.IRepositories
{
    public interface IProjectEmployeeRepository
    {
        Task<Project_employee[]> GetProjectEmployeeByProject(Project project);
    }
}
