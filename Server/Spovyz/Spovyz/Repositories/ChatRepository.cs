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

        public async Task<List<ChatData>> GetChatsByProject(Employee ActiveUser, uint ProjectId)
        {
            Message[] messages = await _context.Messages
                .Include(m => m.Employee)
                .ThenInclude(e => e.Company)
                .Include(m => m.Project)
                .Where(m => m.Employee.Company.Id == ActiveUser.Company.Id && m.Project.Id == ProjectId)
                .OrderByDescending(m => m.DateTime)
                .ToArrayAsync();
            return messages.Select(m => new ChatData() { Id = m.Id, dateTime = DateTime.Parse(DateTime.SpecifyKind(m.DateTime, DateTimeKind.Utc).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")), Description = m.Text, EmployeeName = m.Employee.Username, MyMessage = m.Employee.Id == ActiveUser.Id }).ToList();
        }

        public async Task<List<ChatData>> GetChatsByTask(Employee ActiveUser, uint TaskId)
        {
            Message[] messages = await _context.Messages
                .Include(m => m.Employee)
                .ThenInclude(e => e.Company)
                .Include(m => m.Task)
                .Where(m => m.Employee.Company.Id == ActiveUser.Company.Id && m.Task.Id == TaskId)
                .OrderByDescending(m => m.DateTime)
                .ToArrayAsync();
            return messages.Select(m => new ChatData() { Id = m.Id, dateTime = DateTime.Parse(DateTime.SpecifyKind(m.DateTime, DateTimeKind.Utc).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")), Description = m.Text, EmployeeName = m.Employee.Username, MyMessage = m.Employee.Id == ActiveUser.Id }).ToList();
        }

        public async Task PostChat(Employee ActiveUser, Models.Task? Task, Project? Project, string Message)
        {
            Message message = new Message()
            {
                DateTime = DateTime.Now.ToUniversalTime(),
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
