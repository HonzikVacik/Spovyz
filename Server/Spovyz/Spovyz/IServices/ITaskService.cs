using Spovyz.Transport_models;

namespace Spovyz.IServices
{
    public interface ITaskService
    {
        Task<(List<EmployeeDashboardTask>?, string?)> GetTaskList(string UserName, uint ProjectId);
        Task<TaskCardData> GetTaskById(string UserName, uint TaskId);
        Task<string> DeleteTask(string UserName, uint TaskId);
    }
}
