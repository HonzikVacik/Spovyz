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

        public async Task<(ValidityControl.ResultStatus status, string? error)> DeleteChatById(string UserName, uint ChatId)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found");

            await _chatRepository.DeleteChatById(activeUser.Id, ChatId);

            return (ValidityControl.ResultStatus.Ok, null);
        }

        public async Task<(ValidityControl.ResultStatus status, string? error, List<ChatData>? chatData)> GetChatsByProject(string UserName, uint ProjectId)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found", null);

            Project? project = await _projectRepository.GetProjectById(ProjectId, activeUser.Id);
            if (project == null)
                return (ValidityControl.ResultStatus.NotFound, "Project not found", null);

            List<ChatData> chatData = await _chatRepository.GetChatsByProject(activeUser.Company.Id, ProjectId);

            return (ValidityControl.ResultStatus.Ok, null, chatData);
        }

        public async Task<(ValidityControl.ResultStatus status, string? error, List<ChatData>? chatData)> GetChatsByTask(string UserName, uint TaskId)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found", null);

            Models.Task? task = await _taskRepository.GetTaskById(activeUser.Id, TaskId);
            if (task == null)
                return (ValidityControl.ResultStatus.NotFound, "Project not found", null);

            List<ChatData> chatData = await _chatRepository.GetChatsByTask(activeUser.Company.Id, TaskId);

            return (ValidityControl.ResultStatus.Ok, null, chatData);
        }

        public async Task<(ValidityControl.ResultStatus status, string? error)> PostChat(string UserName, uint? TaskId, uint? ProjectId, string Message)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found");

            Project? project = null;
            Models.Task? task = null;

            if (ProjectId != null)
            {
                project = await _projectRepository.GetProjectById((uint)ProjectId, activeUser.Id);
                if (project == null)
                    return (ValidityControl.ResultStatus.NotFound, "Project not found");
            }
            else if(TaskId != null)
            {
                task = await _taskRepository.GetTaskById(activeUser.Id, (uint)TaskId);
                if (task == null)
                    return (ValidityControl.ResultStatus.NotFound, "Task not found");
            }
            else
            {
                return (ValidityControl.ResultStatus.Error, "Either ProjectId or TaskId must be provided");
            }

            if(task == null && project == null)
            {
                return (ValidityControl.ResultStatus.Error, "Either ProjectId or TaskId must be provided");
            }
            else
            {
                await _chatRepository.PostChat(activeUser, task, project, Message);
                return (ValidityControl.ResultStatus.Ok, null);
            }
        }
    }
}
