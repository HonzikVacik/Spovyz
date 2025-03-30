using Microsoft.EntityFrameworkCore;
using Spovyz.Repositories;
using Spovyz.IServices;
using Spovyz.Models;
using Spovyz.Transport_models;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Spovyz.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly ProjectRepository _projectRepository;

        public ProjectService(ApplicationDbContext context, ProjectRepository projectRepository)
        {
            _context = context;
            _projectRepository = projectRepository;
        }

        public async Task<string> DeleteProject(string UserName, uint ProjectId)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return "User not found";

            Project? project = await _projectRepository.GetProjectById(ProjectId, activeUser.Id);
            if (project == null)
                return "Project not found";

            Project_employee[] p_employees = [.. _context.Project_employees
                .Include(p => p.Project)
                .Where(p => p.Project == project)
                .ToArray()];
            Project_tag[] p_tag = [.. _context.Project_tags
                .Include(p => p.Project)
                .Where(p => p.Project == project)
                .ToArray()];
            Models.Task[] p_tasks = [.. _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.Project == project)
                .ToArray()];

            var result = p_tasks.Select(task => new
            {
                t_employees = _context.Task_employees
                    .Include(t => t.Task)
                    .Where(t => t.Task == task)
                    .ToArray(),
                t_tags = _context.Task_tags
                    .Include(t => t.Task == task)
                    .ToArray()
            });
            Task_employee[] t_employees = result.SelectMany(r => r.t_employees).ToArray();
            Task_tag[] t_tag = result.SelectMany(r => r.t_tags).ToArray();


            await _projectRepository.DeleteProject(project, t_employees, t_tag, p_tasks, p_tag, p_employees);
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



            string[] tagNames = [.. _context.Project_tags
                .Include(pt => pt.Tag)
                .Where(pt => pt.Project.Id == project.Id)
                .Select(pt => pt.Tag.Name)
                .ToArray()];
            List<NameBasic> p_tag = tagNames
                .Select((name, index) => new NameBasic { Id = index, Name = name })
                .ToList();


            uint[] employeesIds = [.. _context.Project_employees
                .Include(e => e.Project)
                .Include(e => e.Employee)
                .Where(e => e.Project.Id == project.Id)
                .Select(e => e.Employee.Id)
                .ToArray()];


            string[] taskNames = [.. _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.Project.Id == project.Id)
                .Select(t => t.Name)
                .ToArray()];
            List<NameBasic> p_task = taskNames
                .Select((name, index) => new NameBasic { Id = index, Name = name })
                .ToList();


            ProjectCardData data = new ProjectCardData()
            {
                Name = project.Name,
                Description = project.Description,
                Customer = project.Customer.Id,
                Status = (uint)project.Status,
                Deathline = project.Dead_line,
                WorkedOut = "3 dny",
                WorkedByMe = "9 hodin",
                Tags = p_tag.ToArray(),
                Employees = employeesIds,
                Tasks = p_task.ToArray()
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
    }
}
