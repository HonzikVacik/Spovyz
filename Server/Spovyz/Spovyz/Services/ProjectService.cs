using Microsoft.EntityFrameworkCore;
using Spovyz.Repositories;
using Spovyz.IServices;
using Spovyz.Models;
using Spovyz.Transport_models;
using Spovyz.IRepositories;

namespace Spovyz.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectEmployeeRepository _projectEmployeeRepository;
        private readonly IProjectTagRepository _projectTagRepository;
        private readonly ITaskEmployeeRepository _taskEmployeeRepository;
        private readonly ITaskTagRepository _taskTagRepository;
        private readonly ITagRepository _tagRepository;

        public ProjectService(ApplicationDbContext context, IEmployeeRepository employeeRepository, ProjectRepository projectRepository, ITaskRepository taskRepository, IProjectEmployeeRepository projectEmployeeRepository, IProjectTagRepository projectTagRepository, ITaskEmployeeRepository taskEmployeeRepository, ITaskTagRepository taskTagRepository, ITagRepository tagRepository)
        {
            _context = context;
            _employeeRepository = employeeRepository;
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _projectEmployeeRepository = projectEmployeeRepository;
            _projectTagRepository = projectTagRepository;
            _taskEmployeeRepository = taskEmployeeRepository;
            _taskTagRepository = taskTagRepository;
            _tagRepository = tagRepository;
        }

        public async Task<string> DeleteProject(string UserName, uint ProjectId)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return "User not found";

            Project? project = await _projectRepository.GetProjectById(ProjectId, activeUser.Id);
            if (project == null)
                return "Project not found";

            Project_employee[] p_employees = await _projectEmployeeRepository.GetProjectEmployeeByProject(project);
            Project_tag[] p_tag = await _projectTagRepository.GetProjectTagByProject(project);
            List<Models.Task> p_tasks = await _taskRepository.GetTaskList(project.Id, activeUser.Id);

            var result = await System.Threading.Tasks.Task.WhenAll(p_tasks.Select(async task => new
            {
                t_employees = await _taskEmployeeRepository.GetTaskEmployeeByTask(task),
                t_tags = await _taskTagRepository.GetTaskTagByTask(task)
            }));

            Task_employee[] t_employees = result.SelectMany(r => r.t_employees).ToArray();
            Task_tag[] t_tag = result.SelectMany(r => r.t_tags).ToArray();

            await _projectRepository.DeleteProject(project, t_employees, t_tag, p_tasks.ToArray(), p_tag, p_employees);
            return "Accept";
        }

        public async Task<(ProjectCardData?, string?)> GetProjectById(string Username, uint ProjectId)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == Username);
            if (activeUser == null)
                return (null, "User not found");

            Project? project = await _projectRepository.GetProjectById(ProjectId, activeUser.Id);
            if (project == null)
                return (null, "Project not found");



            string[]? tagNames = await _tagRepository.GetTagNamesByProject(ProjectId);


            uint[]? employeesIds = await _employeeRepository.GetEmployeesIdsByProjectId(project.Id);


            NameBasic[]? taskNames = await _taskRepository.GetTaskNames(project, activeUser.Id);


            ProjectCardData data = new ProjectCardData()
            {
                Name = project.Name,
                Description = project.Description,
                Customer = project.Customer.Id,
                Status = (uint)project.Status,
                Deathline = project.Dead_line,
                WorkedOut = "3 dny",
                WorkedByMe = "9 hodin",
                Tags = tagNames,
                Employees = employeesIds,
                Tasks = taskNames
            };
            return (data, null);
        }

        public async Task<(List<EmployeeDashboardProject>?, string?)> GetProjectList(string UserName)
        {
            Employee? activeUser =  await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (null, "User not found");

            List<Project> projects = await _projectRepository.GetProjectList(activeUser.Id);
            List<EmployeeDashboardProject> data = projects.Select(t => new EmployeeDashboardProject() { Id = t.Id, Name = t.Name }).ToList();
            return (data, null);
        }

        public async System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string?)> AddProject(string UserName, string Name, string Description, uint CustomerId, DateOnly? Deadline, string[] Tags, uint[] Employees)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found");

            (ValidityControl.ResultStatus resultStatus, string? error) = await ValidityControl.Check_PI(_context, activeUser.Company.Id, Name, Description, CustomerId, Deadline, Employees, true);

            if (resultStatus != ValidityControl.ResultStatus.Ok)
                return (resultStatus, error);

            await _projectRepository.PostProject(Name, Description, await _context.Customers.FindAsync(CustomerId), Deadline, await _employeeRepository.GetEmployeesByIds(Employees), await _tagRepository.PostGetTags(Tags));

            return (ValidityControl.ResultStatus.Ok, null);
        }

        public async System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string?)> UpdateProject(string UserName, uint ProjectId, string Name, string Description, int CustomerId, DateOnly? DeadLine, int Status, string[] Tags, uint[] Employees)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found");

            return (ValidityControl.ResultStatus.Ok, null);
        }
    }
}
