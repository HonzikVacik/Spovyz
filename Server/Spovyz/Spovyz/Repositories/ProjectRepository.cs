using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;
using System.Threading.Tasks;

namespace Spovyz.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task DeleteProject(Project project, Task_employee[] t_employees, Task_tag[] t_tag, Models.Task[] p_tasks, Project_tag[] p_tag, Project_employee[] p_employees)
        {
            Message[]? messages = await _context.Messages
                .Include(m => m.Project)
                .Where(m => m.Project == project)
                .ToArrayAsync();
            if (messages != null && messages.Length > 0)
                _context.Messages.RemoveRange(messages);

            _context.RemoveRange(t_employees);
            _context.RemoveRange(t_tag);
            _context.RemoveRange(p_tasks);
            _context.RemoveRange(p_tag);
            _context.RemoveRange(p_employees);
            _context.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task<Project?> GetProjectById(uint ProjectId, uint activeUserId)
        {
            return await _context.Project_employees
                .Include(pe => pe.Project)
                .ThenInclude(p => p.Customer)
                .Include(pe => pe.Employee)
                .Where(pe => pe.Project.Id == ProjectId && pe.Employee.Id == activeUserId)
                .Select(pe => pe.Project)
                .FirstOrDefaultAsync();
        }

        public async System.Threading.Tasks.Task<List<Project>> GetProjectList(uint ActiveUserId)
        {
            return await _context.Project_employees
                .Include(te => te.Project)
                .Include(te => te.Employee)
                .Where(te => te.Employee.Id == ActiveUserId)
                .Select(te => te.Project)
                .OrderBy(te => te.Name)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task PostProject(string Name, string? Description, Customer Customer, DateOnly? DeadLine, Employee[] Employees, Tag[] Tags)
        {
            Project project = new Project
            {
                Name = Name,
                Description = Description,
                Customer = Customer,
                Dead_line = DeadLine,
                Status = Enums.Status.New
            };
            _context.Project_tags.AddRange(Tags.Select(t => new Project_tag { Project = project, Tag = t}));
            _context.Project_employees.AddRange(Employees.Select(e => new Project_employee { Project = project, Employee = e }));
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task PutProject(Project Project, string Name, string? Description, Customer Customer, DateOnly? DeadLine, Enums.Status Status, Employee[] DelEmployees, Employee[] AddEmployees, Tag[] DelTags, Tag[] AddTags)
        {
            Project.Name = Name;
            Project.Description = Description;
            Project.Customer = Customer;
            Project.Dead_line = DeadLine;
            Project.Status = Status;

            List<Project_employee> DelEmployeeList = new List<Project_employee>();
            List<Project_employee> AddEmployeeList = new List<Project_employee>();
            List<Project_tag> DelTagList = new List<Project_tag>();
            List<Project_tag> AddTagList = new List<Project_tag>();

            foreach (var employee in DelEmployees)
            {
                DelEmployeeList.Add(new Project_employee { Project = Project, Employee = employee });
            }
            foreach (var employee in AddEmployees)
            {
                AddEmployeeList.Add(new Project_employee { Project = Project, Employee = employee });
            }
            foreach (var tag in DelTags)
            {
                DelTagList.Add(new Project_tag { Project = Project, Tag = tag });
            }
            foreach (var tag in AddTags)
            {
                AddTagList.Add(new Project_tag { Project = Project, Tag = tag });
            }

            _context.Projects.Update(Project);
            _context.Project_employees.RemoveRange(DelEmployeeList);
            _context.Project_employees.AddRange(AddEmployeeList);
            _context.Project_tags.RemoveRange(DelTagList);
            _context.Project_tags.AddRange(AddTagList);
            await _context.SaveChangesAsync();
        }
    }
}
