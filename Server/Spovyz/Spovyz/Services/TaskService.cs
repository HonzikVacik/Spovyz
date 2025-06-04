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

        public async System.Threading.Tasks.Task<string> DeleteTask(string UserName, uint TaskId)
        {
            Employee? activeUser = await _context.Employees.FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return "Uživatel nenalezen";

            Models.Task? task = await _taskRepository.GetTaskById(TaskId, activeUser.Id);
            if(task == null)
                return "Task nenalezen";

            Models.Task_employee[] t_employees = await _taskEmployeeRepository.GetTaskEmployeeByTask(task);
            Models.Task_tag[] t_tags = await _taskTagRepository.GetTaskTagByTask(task);

            await _taskRepository.DeleteTask(task, t_employees, t_tags);
            return "accept";
        }

        public async System.Threading.Tasks.Task<(TaskCardData?, string?)> GetTaskById(string UserName, uint TaskId)
        {
            Employee? activeUser = await _context.Employees.FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (null, "Uživatel nenalezen");

            Models.Task? task = await _taskRepository.GetTaskById(TaskId, activeUser.Id);
            if (task == null)
                return (null, "Task nenalezen");

            string[]? tags = await _tagRepository.GetTagNamesByTask(task.Id);
            
            uint[] employees = await _context.Task_employees
                    .Include(t => t.Task)
                    .Include(t => t.Employee)
                    .Where(t => t.Task.Id == task.Id)
                    .Select(t => t.Employee.Id)
                    .ToArrayAsync();

            uint? remains = null;
            if (task.Dead_line != null)
            {
                remains = (uint)(((DateOnly)task.Dead_line).ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days;
            }

            TaskCardData result = new TaskCardData()
            {
                Name = task.Name,
                Description = task.Description,
                ProjectId = task.Project.Id,
                Status = (uint)task.Status,
                Deadline = task.Dead_line,
                Remains = remains,
                Tags = tags,
                Employees = employees
            };

            return (result, null);
        }

        public async System.Threading.Tasks.Task<(List<EmployeeDashboardTask>?, string?)> GetTaskList(string UserName, uint ProjectId)
        {
            Employee? activeUser = await _context.Employees.FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (null, "Uživatel nenalezen");

            Project? project = await _projectRepository.GetProjectById(ProjectId, activeUser.Id);
            if (project == null)
                return (null, "Projekt nenalezen");

            List<Models.Task> tasks = await _taskRepository.GetTaskList(ProjectId, activeUser.Id);
            List<EmployeeDashboardTask> data = tasks.Select(t => new EmployeeDashboardTask() { Id = t.Id, Name = t.Name }).ToList();
            return (data, null);
        }

        public async System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string?)> AddTask(string UserName, string Name, string? Description, uint ProjectId, DateOnly? DeadLine, int Status, string[] Tags, uint[] Employees)
        {
            Employee? activeUser = await _context.Employees.FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "Uživatel nenalezen");

            Project? project = await _projectRepository.GetProjectById(ProjectId, activeUser.Id);
            if (project == null)
                return (ValidityControl.ResultStatus.NotFound, "Projekt nenalezen");

            (ValidityControl.ResultStatus resultStatus, string? error) = await ValidityControl.Check_TI(_context, Name, ProjectId, DeadLine, Status, Employees, true);

            if (resultStatus != ValidityControl.ResultStatus.Ok)
                return (resultStatus, error);

            await _taskRepository.PostTask(Name, Description, project, DeadLine, await _employeeRepository.GetEmployeesByIds(Employees), await _tagRepository.PostGetTags(Tags));

            return (ValidityControl.ResultStatus.Ok, null);
        }

        public async System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string?)> UpdateTask(string UserName, uint TaskId, string Name, string? Description, uint ProjectId, DateOnly? DeadLine, int Status, string[] Tags, uint[] Employees)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "Uživatel nenalezen");

            Project? project = await _projectRepository.GetProjectById(ProjectId, activeUser.Id);
            if (project == null)
                return (ValidityControl.ResultStatus.NotFound, "Projekt nenalezen");

            Models.Task? originalTask = await _taskRepository.GetTaskById(TaskId, activeUser.Id);
            if (originalTask == null)
                return (ValidityControl.ResultStatus.NotFound, "Tento task neexistuje");

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

            await _taskRepository.PutTask(originalTask, Name, Description, project, DeadLine, (Enums.Status) Status, await _employeeRepository.GetEmployeesByIds(DelEmployees), await _employeeRepository.GetEmployeesByIds(AddEmployees), await _tagRepository.PostGetTags(DelTags), await _tagRepository.PostGetTags(AddTags));

            return (ValidityControl.ResultStatus.Ok, null);
        }
    }
}
