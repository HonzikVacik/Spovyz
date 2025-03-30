using Spovyz.Transport_models;

namespace Spovyz.IServices
{
    public interface IProjectService
    {
        Task<(List<EmployeeDashboardProject>?, string?)> GetProjectList(string UserName);
        Task<(Transport_models.ProjectCardData?, string?)> GetProjectById(string Username, uint ProjectId);
        Task<string> DeleteProject(string UserName, uint ProjectId);
    }
}
