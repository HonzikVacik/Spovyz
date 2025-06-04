using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spovyz.InputModels;
using Spovyz.IServices;
using Spovyz.Models;
using Spovyz.Transport_models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITaskService _taskService;

        public TaskController(ApplicationDbContext context, ITaskService taskService)
        {
            this._context = context;
            this._taskService = taskService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetList(uint ProjectId)
        {
            //e1 - projekt neexistuje
            string? UserName = User.Identity.Name;
            if (UserName == null)
                return NotFound("A");
            
            (List<EmployeeDashboardTask>? data, string? error) = await _taskService.GetTaskList(UserName, ProjectId);
            if (error != null)
                return NotFound(error);
            return Ok(data);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            string? UserName = User.Identity.Name;
            if (UserName == null)
                return NotFound("Uživatel nenalezen");

            (TaskCardData? data, string? error) = await _taskService.GetTaskById(UserName, (uint)id);
            if (error != null)
                return NotFound(error);
            return Ok(data);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(TaskPostInput TaskPostInput)
        {
            string? UserName = User.Identity.Name;
            if (UserName == null)
                return NotFound("Uživatel nenalezen");
            (ValidityControl.ResultStatus ResultStatus, string? Description) result = await _taskService.AddTask(UserName, TaskPostInput.Name, TaskPostInput.Description, TaskPostInput.ProjectId, TaskPostInput.DeadLine, TaskPostInput.Status, TaskPostInput.Tags, TaskPostInput.Employees);

            if (result.ResultStatus == ValidityControl.ResultStatus.Error)
                return BadRequest(result.Description);
            if (result.ResultStatus == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.Description);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put(TaskPutInput TaskPutInput)
        {
            string? UserName = User.Identity.Name;
            if (UserName == null)
                return NotFound("Uživatel nenalezen");

            (ValidityControl.ResultStatus ResultStatus, string? Description) result = await _taskService.UpdateTask(UserName, TaskPutInput.TaskId, TaskPutInput.Name, TaskPutInput.Description, TaskPutInput.ProjectId, TaskPutInput.DeadLine, TaskPutInput.Status, TaskPutInput.Tags, TaskPutInput.Employees);

            if (result.ResultStatus == ValidityControl.ResultStatus.Error)
                return BadRequest(result.Description);
            if (result.ResultStatus == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.Description);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(uint id)
        {
            //e1 - projekt neexistuje
            string error = "e1";
            string accept = "accept";

            string? UserName = User.Identity.Name;
            if (UserName == null)
                return NotFound(error);

            string result = await _taskService.DeleteTask(UserName, id);
            if (result == accept)
                return Ok();
            else
                return NotFound(error);
        }
    }
}
