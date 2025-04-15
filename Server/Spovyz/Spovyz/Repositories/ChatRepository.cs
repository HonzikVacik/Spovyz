using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;
using Spovyz.Transport_models;
using Task = System.Threading.Tasks.Task;

namespace Spovyz.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteChatById(uint ActiveUserId, uint ChatId)
        {
            Message? message = await _context.Messages
                .Include(m => m.Employee)
                .Where(m => m.Employee.Id == ActiveUserId && m.Id == ChatId)
                .FirstOrDefaultAsync();
            
            if(message != null)
            {
                _context.Remove(message);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ChatData>> GetChatsByProject(uint ActiveUserId, uint ProjectId)
        {
            Message[] messages = await _context.Messages
                .Include(m => m.Employee)
                .Include(m => m.Project)
                .Where(m => m.Employee.Id == ActiveUserId && m.Project.Id == ProjectId)
                .ToArrayAsync();
            return messages.Select(m => new ChatData() { Id = m.Id, dateTime = m.DateTime, Descrition = m.Text, EmployeeName = m.Employee.Username }).ToList();
        }

        public async Task<List<ChatData>> GetChatsByTask(uint ActiveUserId, uint TaskId)
        {
            Message[] messages = await _context.Messages
                .Include(m => m.Employee)
                .Include(m => m.Task)
                .Where(m => m.Employee.Id == ActiveUserId && m.Task.Id == TaskId)
                .ToArrayAsync();
            return messages.Select(m => new ChatData() { Id = m.Id, dateTime = m.DateTime, Descrition = m.Text, EmployeeName = m.Employee.Username }).ToList();
        }

        public async Task PostChat(Employee ActiveUser, Models.Task Task, Project Project, string Message)
        {
            Message message = new Message()
            {
                DateTime = DateTime.Now,
                Text = Message,
                Employee = ActiveUser,
                Project = Project,
                Task = Task
            };
            _context.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
