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
        private readonly ITaskEmployeeRepository _taskEmployeeRepository;
        private readonly ITaskTagRepository _taskTagRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public TaskService(ApplicationDbContext context, IProjectRepository projectRepository, ITaskRepository taskRepository, ITaskEmployeeRepository taskEmployeeRepository, ITaskTagRepository taskTagRepository, ITagRepository tagRepository, IEmployeeRepository employeeRepository)
        {
            _context = context;
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _taskEmployeeRepository = taskEmployeeRepository;
            _taskTagRepository = taskTagRepository;
            _taskTagRepository = taskTagRepository;
            _tagRepository = tagRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<string> DeleteTask(string UserName, uint TaskId)
        {
            Employee? activeUser = await _context.Employees.FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return "User not found";

            Models.Task? task = await _taskRepository.GetTaskById(TaskId, activeUser.Id);
            if(task == null)
                return "Task not found";

            Models.Task_employee[] t_employees = await _taskEmployeeRepository.GetTaskEmployeeByTask(task);
            Models.Task_tag[] t_tags = await _taskTagRepository.GetTaskTagByTask(task);

            await _taskRepository.DeleteTask(task, t_employees, t_tags);
            return "accept";
        }

        public async Task<(TaskCardData?, string?)> GetTaskById(string UserName, uint TaskId)
        {
            Employee? activeUser = await _context.Employees.FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (null, "User not found");

            Models.Task? task = await _taskRepository.GetTaskById(TaskId, activeUser.Id);
            if (task == null)
                return (null, "Task not found");

            TaskCardData result = new TaskCardData()
            {
                Name = task.Name,
                Description = task.Description,
                Status = (uint)task.Status,
                Deathline = task.Dead_line,
                WorkedOut = 0.ToString(),
                WorkedByMe = 0.ToString(),
                Tags = await _tagRepository.GetTagNamesByTask(task.Id),
                Employees = await _context.Task_employees
                    .Include(t => t.Task)
                    .Include(t => t.Employee)
                    .Where(t => t.Task.Id == task.Id)
                    .Select(t => t.Employee.Id)
                    .ToArrayAsync(),
            };

            return (result, null);
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

        public async Task<(ValidityControl.ResultStatus, string?)> AddTask(string UserName, string Name, string? Description, uint ProjectId, DateOnly? DeadLine, int Status, string[] Tags, uint[] Employees)
        {
            Employee? activeUser = await _context.Employees.FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found");

            Project? project = await _projectRepository.GetProjectById(ProjectId, activeUser.Id);
            if (project == null)
                return (ValidityControl.ResultStatus.NotFound, "Project not found");

            (ValidityControl.ResultStatus resultStatus, string? error) = await ValidityControl.Check_TI(_context, Name, ProjectId, DeadLine, Status, Employees, true);

            if (resultStatus != ValidityControl.ResultStatus.Ok)
                return (resultStatus, error);

            await _taskRepository.PostTask(Name, Description, project, DeadLine, await _employeeRepository.GetEmployeesByIds(Employees), await _tagRepository.PostGetTags(Tags));

            return (ValidityControl.ResultStatus.Ok, null);
        }

        public async Task<(ValidityControl.ResultStatus, string?)> UpdateTask(string UserName, uint TaskId, string Name, string? Description, uint ProjectId, DateOnly? DeadLine, int Status, string[] Tags, uint[] Employees)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found");

            Project? project = await _projectRepository.GetProjectById(ProjectId, activeUser.Id);
            if (project == null)
                return (ValidityControl.ResultStatus.NotFound, "Project not found");

            Models.Task? originalTask = await _taskRepository.GetTaskById(TaskId, activeUser.Id);
            if (originalTask == null)
                return (ValidityControl.ResultStatus.NotFound, "Task does not exist");

            (ValidityControl.ResultStatus resultStatus, string? error) = await ValidityControl.Check_TI(_context, Name, ProjectId, DeadLine, Status, Employees, originalTask.Name != Name);

            if (resultStatus != ValidityControl.ResultStatus.Ok)
                return (resultStatus, error);

            uint[] originalEmployees = await _employeeRepository.GetEmployeesIdsByTaskId(originalTask.Id);
            uint[] DelEmployees = originalEmployees.Except(Employees).ToArray();
            uint[] AddEmployees = Employees.Except(originalEmployees).ToArray();

            string[]? originalTags = await _tagRepository.GetTagNamesByTask(originalTask.Id);
            string[] DelTags = Array.Empty<string>();
            string[] AddTags = Array.Empty<string>();
            if(originalTags != null)
            {
                DelTags = originalTags.Except(Tags).ToArray();
                AddTags = Tags.Except(originalTags).ToArray();
            }
            else
            {
                AddTags = Tags;
            }

            await _taskRepository.PutTask(activeUser.Id, originalTask, Name, Description, project, DeadLine, (Enums.Status) Status, await _employeeRepository.GetEmployeesByIds(DelEmployees), await _employeeRepository.GetEmployeesByIds(AddEmployees), await _tagRepository.PostGetTags(DelTags), await _tagRepository.PostGetTags(AddTags));

            return (ValidityControl.ResultStatus.Ok, null);
        }
    }
}
