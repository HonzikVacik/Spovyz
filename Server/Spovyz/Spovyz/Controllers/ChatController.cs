using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spovyz.IServices;
using Spovyz.Transport_models;

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("GetChatsByProject")]
        [Authorize(Roles = "Worker,Supervisor,Manager")]
        public async Task<IActionResult> GetByProject(uint ProjectId)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound("Uživatel nenalezen");

            (ValidityControl.ResultStatus status, string? error, List<ChatData>? chatData) result = await _chatService.GetChatsByProject(UserName, ProjectId);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok(result.chatData);
        }

        [HttpGet("GetChatsByTask")]
        [Authorize(Roles = "Worker,Supervisor,Manager")]
        public async Task<IActionResult> GetByTask(uint TaskId)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound("Uživatel nenalezen");

            (ValidityControl.ResultStatus status, string? error, List<ChatData>? chatData) result = await _chatService.GetChatsByTask(UserName, TaskId);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok(result.chatData);
        }

        [HttpPost]
        [Authorize(Roles = "Worker,Supervisor,Manager")]
        public async Task<IActionResult> Post(uint? ProjectId, uint? TaskId, string Message)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound("Uživatel nenalezen");

            (ValidityControl.ResultStatus status, string? error) result = await _chatService.PostChat(UserName, TaskId, ProjectId, Message);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Worker,Supervisor,Manager")]
        public async Task<IActionResult> Delete(uint id)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound("Uživatel nenalezen");

            (ValidityControl.ResultStatus status, string? error) result = await _chatService.DeleteChatById(UserName, id);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok();
        }
    }
}
