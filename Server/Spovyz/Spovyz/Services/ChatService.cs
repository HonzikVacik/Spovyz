using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.IServices;
using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.Services
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IChatRepository _chatRepository;

        public ChatService(ApplicationDbContext context, IProjectRepository projectRepository, ITaskRepository taskRepository, IChatRepository chatRepository)
        {
            _context = context;
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _chatRepository = chatRepository;
        }

        public async System.Threading.Tasks.Task<(ValidityControl.ResultStatus status, string? error)> DeleteChatById(string UserName, uint ChatId)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "Uživatel nenalezen");

            await _chatRepository.DeleteChatById(activeUser.Id, ChatId);

            return (ValidityControl.ResultStatus.Ok, null);
        }

        public async System.Threading.Tasks.Task<(ValidityControl.ResultStatus status, string? error, List<ChatData>? chatData)> GetChatsByProject(string UserName, uint ProjectId)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "Uživatel nenalezen", null);

            Project? project = await _projectRepository.GetProjectById(ProjectId, activeUser.Id);
            if (project == null)
                return (ValidityControl.ResultStatus.NotFound, "Projekt nenalezen", null);

            List<ChatData> chatData = await _chatRepository.GetChatsByProject(activeUser, ProjectId);

            return (ValidityControl.ResultStatus.Ok, null, chatData);
        }

        public async System.Threading.Tasks.Task<(ValidityControl.ResultStatus status, string? error, List<ChatData>? chatData)> GetChatsByTask(string UserName, uint TaskId)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "Uživatel nenalezen", null);

            Models.Task? task = await _taskRepository.GetTaskById(TaskId, activeUser.Id);
            if (task == null)
                return (ValidityControl.ResultStatus.NotFound, "Projekt nenalezen", null);

            List<ChatData> chatData = await _chatRepository.GetChatsByTask(activeUser, task.Id);

            return (ValidityControl.ResultStatus.Ok, null, chatData);
        }

        public async System.Threading.Tasks.Task<(ValidityControl.ResultStatus status, string? error)> PostChat(string UserName, uint? TaskId, uint? ProjectId, string Message)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "Uživatel nenalezen");

            Project? project = null;
            Models.Task? task = null;

            if (ProjectId != null)
            {
                project = await _projectRepository.GetProjectById((uint)ProjectId, activeUser.Id);
                if (project == null)
                    return (ValidityControl.ResultStatus.NotFound, "Projekt nenalezen");
            }
            else if(TaskId != null)
            {
                task = await _taskRepository.GetTaskById((uint)TaskId, activeUser.Id);
                if (task == null)
                    return (ValidityControl.ResultStatus.NotFound, "Task nenalezen");
            }
            else
            {
                return (ValidityControl.ResultStatus.Error, "Musí být zadán Task nebo Projekt");
            }

            if(task == null && project == null)
            {
                return (ValidityControl.ResultStatus.Error, "Musí být zadán Task nebo Projekt");
            }
            else
            {
                await _chatRepository.PostChat(activeUser, task, project, Message);
                return (ValidityControl.ResultStatus.Ok, null);
            }
        }
    }
}
