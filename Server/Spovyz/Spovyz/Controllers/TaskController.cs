using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spovyz.Models;
using Spovyz.Transport_models;
using System.ComponentModel.DataAnnotations;

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
            // Testovací hodnoty
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
            return Ok(datas);

            //e1 - projekt neexistuje

            string error = "e1";
            uint i = 0;
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());
            Project_employee[] pe = [.. _context.Project_employees
                .Include(pe => pe.Employee)
                .Include(pe => pe.Project)
                .Where(pe => pe.Employee.Id == activeUser.Id)
                .ToArray()];
            if (ProjectId < 0 || ProjectId >= pe.Length)
                return Ok(error);
            uint activeProjectId = pe[ProjectId].Project.Id;
            Models.Task[] tasks = [.. _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.Project.Id == activeProjectId)
                .ToArray()];
            List<EmployeeDashboardTask> data = tasks.Select(t => new EmployeeDashboardTask() { Id = i++, Name = t.Name }).ToList();
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
        public IActionResult Delete(int id, [Required]int projectId)
        {
            //e1 - projekt neexistuje
            string error = "e1";
            string accept = "a";

            Employee activeUser = _context.Employees.FirstOrDefault(e => e.Username == User.Identity.Name.ToString());

            Models.Task[] tasks = _context.Tasks
                .Include(t => t.Project)
                .ToArray();

            if(id >= 0 && projectId < tasks.Length)
            {
                Models.Task task = tasks[id];
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
                return Ok(error);
        }
    }
}
