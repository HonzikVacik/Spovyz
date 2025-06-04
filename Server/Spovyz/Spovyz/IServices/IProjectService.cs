using Spovyz.Transport_models;

namespace Spovyz.IServices
{
    public interface IProjectService
    {
        System.Threading.Tasks.Task<(List<EmployeeDashboardProject>?, string?)> GetProjectList(string UserName);
        System.Threading.Tasks.Task<(Transport_models.ProjectCardData?, string?)> GetProjectById(string Username, uint ProjectId);
        System.Threading.Tasks.Task<string> DeleteProject(string UserName, uint ProjectId);
        System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string?)> AddProject(string UserName, string Name, string Description, uint CustomerId, DateOnly? Deadline, string[] Tags, uint[] Employees);
        System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string?)> UpdateProject(string UserName, uint ProjectId, string Name, string Description, uint CustomerId, DateOnly? DeadLine, int Status, string[] Tags, uint[] Employees);
    }
}
