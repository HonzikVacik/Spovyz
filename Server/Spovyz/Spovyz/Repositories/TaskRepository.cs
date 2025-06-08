using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task DeleteTask(Models.Task Task, Task_employee[] t_employees, Task_tag[] t_tags)
        {
            Message[]? messages = await _context.Messages
                .Include(m => m.Task)
                .Where(m => m.Task == Task)
                .ToArrayAsync();
            if (messages != null && messages.Length > 0)
                _context.Messages.RemoveRange(messages);

            _context.RemoveRange(t_employees);
            _context.RemoveRange(t_tags);
            _context.Remove(Task);
            _context.RemoveRange(await _context.Messages
                .Include(m => m.Task)
                .Where(m => m.Task == Task)
                .ToArrayAsync());
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task<Models.Task?> GetTaskById(uint TaskId, uint ActiveUserId)
        {
            return await _context.Task_employees
                .Include(t => t.Task.Project)
                .Include(t => t.Task)
                .Include(t => t.Employee)
                .Where(t => t.Task.Id == TaskId && t.Employee.Id == ActiveUserId)
                .Select(t => t.Task)
                .FirstOrDefaultAsync();
        }

        public async System.Threading.Tasks.Task<List<Models.Task>> GetTaskList(uint ProjectId, uint ActiveUserId)
        {
            return await _context.Task_employees
                .Include(t => t.Task)
                .Include(t => t.Employee)
                .Where(t => t.Task.Project.Id == ProjectId && t.Employee.Id == ActiveUserId)
                .Select(t => t.Task)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<NameBasic[]?> GetTaskNames(Project Project, uint ActiveUserId)
        {
            return await _context.Task_employees
                .Include(t => t.Task)
                .Include(t => t.Employee)
                .Where(t => t.Task.Project == Project && t.Employee.Id == ActiveUserId)
                .Select(t => new NameBasic { Id = t.Task.Id, Name = t.Task.Name })
                .ToArrayAsync();
        }

        public async System.Threading.Tasks.Task PostTask(string Name, string? Description, Project Project, DateOnly? DeadLine, Employee[] Employees, Tag[] Tags)
        {
            Models.Task task = new Models.Task
            {
                Name = Name,
                Description = Description,
                Project = Project,
                Dead_line = DeadLine,
                Status = Enums.Status.New
            };

            _context.Tasks.Add(task);
            _context.Task_employees.AddRange(Employees.Select(e => new Task_employee { Employee = e, Task = task }));
            _context.Task_tags.AddRange(Tags.Select(t => new Task_tag { Task = task, Tag = t }));
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task PutTask(Models.Task Task, string Name, string? Description, Project Project, DateOnly? DeadLine, Enums.Status Status, Employee[] DelEmployees, Employee[] AddEmployees, Tag[] DelTags, Tag[] AddTags)
        {
            Task.Name = Name;
            Task.Description = Description;
            Task.Project = Project;
            Task.Dead_line = DeadLine;
            Task.Status = Status;

            List<Task_employee> DelEmployeeList = new List<Task_employee>();
            List<Task_employee> AddEmployeeList = new List<Task_employee>();
            List<Task_tag> DelTagList = new List<Task_tag>();
            List<Task_tag> AddTagList = new List<Task_tag>();

            foreach (var employee in DelEmployees)
            {
                DelEmployeeList.Add(new Task_employee { Task = Task, Employee = employee });
            }
            foreach (var employee in AddEmployees)
            {
                AddEmployeeList.Add(new Task_employee { Task = Task, Employee = employee });
            }
            foreach (var tag in DelTags)
            {
                DelTagList.Add(new Task_tag { Task = Task, Tag = tag });
            }
            foreach(var tag in AddTags)
            {
                AddTagList.Add(new Task_tag { Task= Task, Tag = tag });
            }

            _context.Tasks.Update(Task);
            _context.Task_employees.RemoveRange(DelEmployeeList);
            _context.Task_employees.AddRange(AddEmployeeList);
            _context.Task_tags.RemoveRange(DelTagList);
            _context.Task_tags.AddRange(AddTagList);
            await _context.SaveChangesAsync();
        }
    }
}
