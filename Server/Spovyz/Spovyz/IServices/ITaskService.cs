using Spovyz.Transport_models;

namespace Spovyz.IServices
{
    public interface ITaskService
    {
        System.Threading.Tasks.Task<(List<EmployeeDashboardTask>?, string?)> GetTaskList(string UserName, uint ProjectId);
        System.Threading.Tasks.Task<(TaskCardData?, string?)> GetTaskById(string UserName, uint TaskId);
        System.Threading.Tasks.Task<string> DeleteTask(string UserName, uint TaskId);
        System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string?)> AddTask(string UserName, string Name, string? Description, uint ProjectId, DateOnly? DeadLine, int Status, string[] Tags, uint[] Employees);
        System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string?)> UpdateTask(string UserName, uint TaskId, string Name, string? Description, uint ProjectId, DateOnly? DeadLine, int Status, string[] Tags, uint[] Employees);
    }
}
