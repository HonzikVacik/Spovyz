using Spovyz.Transport_models;

namespace Spovyz.IServices
{
    public interface ITaskService
    {
        Task<(List<EmployeeDashboardTask>?, string?)> GetTaskList(string UserName, uint ProjectId);
        Task<(TaskCardData?, string?)> GetTaskById(string UserName, uint TaskId);
        Task<string> DeleteTask(string UserName, uint TaskId);
        System.Threading.Tasks.Task AddTask(string UserName, string Name, string? Description, uint ProjectId, DateOnly? DeadLine, int Status, string[] Tags, uint[] Employees);
        System.Threading.Tasks.Task UpdateTask(string UserName, uint TaskId, string Name, string? Description, uint ProjectId, DateOnly? DeadLine, int Status, string[] Tags, uint[] Employees);
    }
}
