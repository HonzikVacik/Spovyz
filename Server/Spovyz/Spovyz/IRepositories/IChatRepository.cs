using Spovyz.Models;
using Spovyz.Transport_models;
using Task = System.Threading.Tasks.Task;

namespace Spovyz.IRepositories
{
    public interface IChatRepository
    {
        Task<List<ChatData>> GetChatsByTask(Employee ActiveUser, uint TaskId);
        Task<List<ChatData>> GetChatsByProject(Employee ActiveUser, uint ProjectId);
        Task DeleteChatById(uint ActiveUserId, uint ChatId);
        Task PostChat(Employee ActiveUser, Models.Task? Task, Project? Project, string Message);
    }
}
