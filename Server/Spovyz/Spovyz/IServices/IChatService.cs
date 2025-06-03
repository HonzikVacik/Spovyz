using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.IServices
{
    public interface IChatService
    {
        System.Threading.Tasks.Task<(ValidityControl.ResultStatus status, string? error, List<ChatData>? chatData)> GetChatsByTask(string UserName, uint TaskId);
        System.Threading.Tasks.Task<(ValidityControl.ResultStatus status, string? error, List<ChatData>? chatData)> GetChatsByProject(string UserName, uint ProjectId);
        System.Threading.Tasks.Task<(ValidityControl.ResultStatus status, string? error)> DeleteChatById(string UserName, uint ChatId);
        System.Threading.Tasks.Task<(ValidityControl.ResultStatus status, string? error)> PostChat(string UserName, uint? TaskId, uint? ProjectId, string Message);
    }
}
