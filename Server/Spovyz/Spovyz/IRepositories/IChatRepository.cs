using Spovyz.Models;
using Spovyz.Transport_models;
using Task = System.Threading.Tasks.Task;

namespace Spovyz.IRepositories
{
    public interface IChatRepository
    {
        Task<List<ChatData>> GetChatsByTask(uint ActiveUserId, uint TaskId);
        Task<List<ChatData>> GetChatsByProject(uint ActiveUserId, uint ProjectId);
        Task DeleteChatById(uint ActiveUserId, uint ChatId);
        Task PostChat(Employee ActiveUser, Models.Task Task, Project Project, string Message);
    }
}
