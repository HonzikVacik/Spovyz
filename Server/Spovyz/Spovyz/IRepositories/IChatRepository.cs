using Spovyz.Models;
using Spovyz.Transport_models;
using Task = System.Threading.Tasks.Task;

namespace Spovyz.IRepositories
{
    public interface IChatRepository
    {
        System.Threading.Tasks.Task<List<ChatData>> GetChatsByTask(Employee ActiveUser, uint TaskId);
        System.Threading.Tasks.Task<List<ChatData>> GetChatsByProject(Employee ActiveUser, uint ProjectId);
        System.Threading.Tasks.Task DeleteChatById(uint ActiveUserId, uint ChatId);
        System.Threading.Tasks.Task PostChat(Employee ActiveUser, Models.Task? Task, Project? Project, string Message);
    }
}
