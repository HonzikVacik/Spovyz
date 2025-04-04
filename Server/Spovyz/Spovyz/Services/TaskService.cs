using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.IServices;
using Spovyz.Models;
using Spovyz.Transport_models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Spovyz.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;

        public TaskService(ApplicationDbContext context, IProjectRepository _projectRepository, ITaskRepository taskRepository)
        {
            _context = context;
            _taskRepository = taskRepository;
        }

        public async Task<string> DeleteTask(string UserName, uint TaskId)
        {
            Employee? activeUser = await _context.Employees.FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return "User not found";

            Models.Task? task = await _taskRepository.GetTaskById(TaskId, activeUser.Id);
            if(task == null)
                return "Task not found";

            Models.Task_employee[] t_employees = [.. _context.Task_employees
                    .Include(t => t.Task)
                    .Where(t => t.Task == task)
                    .ToArray()];
            Models.Task_tag[] t_tags = [.. _context.Task_tags
                    .Include(t => t.Task == task)
                    .ToArray()];

            await _taskRepository.DeleteTask(task, t_employees, t_tags);
            return "accept";
        }

        public Task<TaskCardData> GetTaskById(string UserName, uint TaskId)
        {
            throw new NotImplementedException();
        }

        public async Task<(List<EmployeeDashboardTask>?, string?)> GetTaskList(string UserName, uint ProjectId)
        {
            Employee? activeUser = await _context.Employees.FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (null, "User not found");

            Project? project = await _projectRepository.GetProjectById(ProjectId, activeUser.Id);
            if (project == null)
                return (null, "Project not found");

            List<Models.Task> tasks = await _taskRepository.GetTaskList(ProjectId, activeUser.Id);
            List<EmployeeDashboardTask> data = tasks.Select(t => new EmployeeDashboardTask() { Id = t.Id, Name = t.Name }).ToList();
            return (data, null);
        }
    }
}
