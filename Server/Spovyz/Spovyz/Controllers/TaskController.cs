using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spovyz.Models;
using Spovyz.Transport_models;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // GET: api/<TaskController>
        [HttpGet]
        [Authorize]
        public IActionResult GetList(int ProjectId)
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
            Employee? activeUser = _context.Employees.FirstOrDefault(e => e.Username == User.Identity.Name.ToString());
            if (activeUser == null)
                return NotFound("A");

            Project? project = _context.Project_employees
                .Include(p => p.Project)
                .Include(p => p.Employee)
                .Where(p => p.Project.Id == ProjectId && p.Employee.Company.Id == activeUser.Company.Id)
                .Select(p => p.Project)
                .FirstOrDefault();
            if (project == null)
                return NotFound("B");

            Models.Task[] tasks = [.. _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.Project.Id == project.Id)
                .ToArray()];
            List<EmployeeDashboardTask> data = tasks.Select(t => new EmployeeDashboardTask() { Id = t.Id, Name = t.Name }).ToList();
            return Ok(data);
        }

        // GET api/<TaskController>/5
        [HttpGet("{id}")]
        public string Get(int id, int projectId)
        {
            return "value";
        }

        // POST api/<TaskController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TaskController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TaskController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            //e1 - projekt neexistuje
            string error = "e1";
            string accept = "a";

            Employee? activeUser = _context.Employees.FirstOrDefault(e => e.Username == User.Identity.Name.ToString());

            if(activeUser == null)
                return NotFound(error);

            Models.Task? task = _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.Id == id)
                .FirstOrDefault();

            if(task != null)
            {
                Models.Task_employee[] t_employees = [.. _context.Task_employees
                    .Include(t => t.Task)
                    .Where(t => t.Task == task)
                    .ToArray()];
                Models.Task_tag[] t_tags = [.. _context.Task_tags
                    .Include(t => t.Task == task)
                    .ToArray()];

                _context.RemoveRange(t_employees);
                _context.RemoveRange(t_tags);
                _context.Remove(task);
                _context.SaveChanges();

                return Ok(accept);
            }
            else
                return NotFound(error);
        }
    }
}
