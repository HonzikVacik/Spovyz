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

        public async System.Threading.Tasks.Task DeleteTask(Models.Task Task, Project_Tag[] t_employees, Task_tag[] t_tags)
        {
            _context.RemoveRange(t_employees);
            _context.RemoveRange(t_tags);
            _context.Remove(Task);
            _context.RemoveRange(await _context.Messages
                .Include(m => m.Task)
                .Where(m => m.Task == Task)
                .ToArrayAsync());
            await _context.SaveChangesAsync();
        }

        public async Task<Models.Task?> GetTaskById(uint TaskId, uint ActiveUserId)
        {
            return await _context.Task_employees
                .Include(t => t.Task)
                .Include(t => t.Emlployee)
                .Where(t => t.Task.Id == TaskId && t.Emlployee.Id == ActiveUserId)
                .Select(t => t.Task)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Models.Task>> GetTaskList(uint ProjectId, uint ActiveUserId)
        {
            return await _context.Task_employees
                .Include(t => t.Task)
                .Include(t => t.Emlployee)
                .Where(t => t.Task.Project.Id == ProjectId && t.Emlployee.Id == ActiveUserId)
                .Select(t => t.Task)
                .ToListAsync();
        }

        public async Task<NameBasic[]?> GetTaskNames(Project Project, uint ActiveUserId)
        {
            return await _context.Task_employees
                .Include(t => t.Task)
                .Include(t => t.Emlployee)
                .Where(t => t.Task.Project == Project && t.Emlployee.Id == ActiveUserId)
                .Select(t => new NameBasic { Id = t.Task.Id, Name = t.Task.Name })
                .ToArrayAsync();
        }

        public async System.Threading.Tasks.Task PostTask(string Name, string? Description, Project Project, DateOnly? DeadLine, Employee[] Employees, uint[] TagIds)
        {
            Models.Task task = new Models.Task
            {
                Name = Name,
                Description = Description,
                Project = Project,
                Dead_line = DeadLine,
                Status = Enums.Status.New
            };

            var taskTags = new List<Task_tag>();

            foreach (var tagId in TagIds)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(tt => tt.Id == tagId);
                if (tag != null)
                {
                    taskTags.Add(new Task_tag
                    {
                        Task = task,
                        Tag = tag
                    });
                }
            }

            _context.Tasks.Add(task);
            _context.Task_employees.AddRange(Employees.Select(e => new Project_Tag { Emlployee = e, Task = task }));
            _context.Task_tags.AddRange(taskTags);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task<string?> PutTask(uint ActiveUserId, uint TaskId, string Name, string? Description, Project Project, DateOnly? DeadLine, Enums.Status Status, Employee[] Employees, uint[] TaskIds)
        {
            Models.Task? task = await _context.Task_employees
                .Include(t => t.Task)
                .Include(t => t.Emlployee)
                .Where(t => t.Task.Id == TaskId && t.Emlployee.Id == ActiveUserId)
                .Select(t => t.Task)
                .FirstOrDefaultAsync();

            if (task == null)
                return "Task not found";

            task.Name = Name;
            task.Description = Description;
            task.Project = Project;
            task.Dead_line = DeadLine;
            task.Status = Status;

            return null;
        }
    }
}
