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

        public async System.Threading.Tasks.Task DeleteProject(Project project, Project_Tag[] t_employees, Task_tag[] t_tag, Models.Task[] p_tasks, Project_tag[] p_tag, Project_employee[] p_employees)
        {
            _context.RemoveRange(t_employees);
            _context.RemoveRange(t_tag);
            _context.RemoveRange(p_tasks);
            _context.RemoveRange(p_tag);
            _context.RemoveRange(p_employees);
            _context.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<Project?> GetProjectById(uint ProjectId, uint activeUserId)
        {
            return await _context.Project_employees
                .Include(pe => pe.Project)
                .Include(pe => pe.Employee)
                .Where(pe => pe.Project.Id == ProjectId && pe.Employee.Id == activeUserId)
                .Select(pe => pe.Project)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Project>> GetProjectList(uint ActiveUserId)
        {
            return await _context.Project_employees
                .Include(te => te.Project)
                .Include(te => te.Employee)
                .Where(te => te.Employee.Id == ActiveUserId)
                .Select(te => te.Project)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task PostProject(string Name, string? Description, Customer Customer, DateOnly? DeadLine, Enums.Status Status, Employee[] Employees, Tag[] Tags)
        {
            Project project = new Project
            {
                Name = Name,
                Description = Description,
                Customer = Customer,
                Dead_line = DeadLine,
                Status = Status
            };
            _context.Project_tags.AddRange(Tags.Select(t => new Project_tag { Project = project, Tag = t}));
            _context.Project_employees.AddRange(Employees.Select(e => new Project_employee { Project = project, Employee = e }));
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task<string?> PutProject(uint ActiveUserId, uint ProjectId, string Name, string? Description, Customer Customer, DateOnly? DeadLine, Enums.Status Status, Employee[] Employees, Tag[] Tags)
        {
            Project? project = await _context.Project_employees
                .Include(pe => pe.Project)
                .Include(pe => pe.Employee)
                .Where(pe => pe.Project.Id == ProjectId && pe.Employee.Id == ActiveUserId)
                .Select(pe => pe.Project)
                .FirstOrDefaultAsync();

            if (project == null)
                return "Project not found.";

            project.Name = Name;
            project.Description = Description;
            project.Customer = Customer;
            project.Dead_line = DeadLine;
            project.Status = Status;

            return null;
        }
    }
}
