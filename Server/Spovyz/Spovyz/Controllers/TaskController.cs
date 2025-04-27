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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<TaskController>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetList(uint ProjectId)
        {
            /*// Testovací hodnoty
            List<EmployeeDashboardTask> datas = new List<EmployeeDashboardTask>();
            switch(ProjectId)
            {
                case 0:
                    {
                        datas.Add(new EmployeeDashboardTask() { Id = 0, Name = "Task1" });
                        datas.Add(new EmployeeDashboardTask() { Id = 1, Name = "Task2" });
                        datas.Add(new EmployeeDashboardTask() { Id = 2, Name = "Task3" });
                    }
                    break;
                case 1:
                    {
                        datas.Add(new EmployeeDashboardTask() { Id = 0, Name = "Task4" });
                        datas.Add(new EmployeeDashboardTask() { Id = 1, Name = "Task5" });
                        datas.Add(new EmployeeDashboardTask() { Id = 2, Name = "Task6" });
                    }
                    break;
                case 2:
                    {
                        datas.Add(new EmployeeDashboardTask() { Id = 0, Name = "Task7" });
                        datas.Add(new EmployeeDashboardTask() { Id = 1, Name = "Task8" });
                        datas.Add(new EmployeeDashboardTask() { Id = 2, Name = "Task9" });
                    }
                    break;
            }
            return Ok(datas);*/

            //e1 - projekt neexistuje
            string? UserName = User.Identity.Name;
            if (UserName == null)
                return NotFound("A");
            
            (List<EmployeeDashboardTask>? data, string? error) = await _taskService.GetTaskList(UserName, ProjectId);
            if (error != null)
                return NotFound(error);
            return Ok(data);
        }

        // GET api/<TaskController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            string? UserName = User.Identity.Name;
            if (UserName == null)
                return NotFound("User not found");
            
            (TaskCardData? data, string? error) = await _taskService.GetTaskById(UserName, (uint)id);
            if (error != null)
                return NotFound(error);
            return Ok(data);
        }

        // POST api/<TaskController>
        [HttpPost]
        public async Task<IActionResult> Post(TaskPostInput TaskPostInput)
        {
            string? UserName = User.Identity.Name;
            if (UserName == null)
                return NotFound("User not found");

            await _taskService.AddTask(UserName, TaskPostInput.Name, TaskPostInput.Description, TaskPostInput.ProjectId, TaskPostInput.DeadLine, TaskPostInput.Status, TaskPostInput.Tags, TaskPostInput.Employees);
            return Ok();
        }

        // PUT api/<TaskController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(TaskPutInput TaskPutInput)
        {
            string? UserName = User.Identity.Name;
            if (UserName == null)
                return NotFound("User not found");

            await _taskService.UpdateTask(UserName, TaskPutInput.TaskId, TaskPutInput.Name, TaskPutInput.Description, TaskPutInput.ProjectId, TaskPutInput.DeadLine, TaskPutInput.Status, TaskPutInput.Tags, TaskPutInput.Employees);
            return Ok();
        }

        // DELETE api/<TaskController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(uint id)
        {
            //e1 - projekt neexistuje
            string error = "e1";
            string accept = "a";

            string? UserName = User.Identity.Name;
            if (UserName == null)
                return NotFound(error);

            string result = await _taskService.DeleteTask(UserName, id);
            if (result == accept)
                return Ok(accept);
            else
                return NotFound(error);
        }
    }
}
