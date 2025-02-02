using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spovyz.Models;
using Spovyz.Transport_models;
using System.Reflection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<ProjectController>
        [HttpGet]
        [Authorize]
        public IEnumerable<EmployeeDashboardProject> Get()
        {
            // Testovací hodnoty
            List<EmployeeDashboardProject> datas = new List<EmployeeDashboardProject>();
            datas.Add(new EmployeeDashboardProject() { Id = 0, Name = "Project1" });
            datas.Add(new EmployeeDashboardProject() { Id = 1, Name = "Project2" });
            datas.Add(new EmployeeDashboardProject() { Id = 2, Name = "Project3" });
            return datas;

            uint i = 0;
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());

            Project[] projects = [.. _context.Project_employees
                .Include(te => te.Project)
                .Include(te => te.Employee)
                .Where(te => te.Employee.Id == activeUser.Id)
                .Select(te => te.Project)
                .ToArray()];

            List<EmployeeDashboardProject> data = projects.Select(t => new EmployeeDashboardProject() { Id = i++, Name = t.Name }).ToList();
            return data;
        }

        // GET api/<ProjectController>/5
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get(int id)
        {
            //e1 - bad id
            string error = null;
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());
            Project[] projects = [.. _context.Project_employees
                .Include(te => te.Project)
                .Include(te => te.Employee)
                .Where(te => te.Employee.Id == activeUser.Id)
                .Select(te => te.Project)
                .ToArray()];

            if (id < 0 || id >= projects.Length)
                return Ok(error = "e1");
            Project project = projects[id];


            string[] tagNames = [.. _context.Project_tags
                .Include(pt => pt.Tag)
                .Where(pt => pt.Project.Id == project.Id)
                .Select(pt => pt.Tag.Name)
                .ToArray()];
            List<NameBasic> p_tag = tagNames
                .Select((name, index) => new NameBasic { Id = index, Name = name })
                .ToList();


            uint[] employees = [.. _context.Project_employees
                .Include(e => e.Project)
                .Include(e => e.Employee)
                .Where(e => e.Project.Id == project.Id)
                .Select(e => e.Employee.Id)
                .ToArray()];


            string[] taskNames = [.. _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.Project.Id == project.Id)
                .Select(t => t.Name)
                .ToArray()];
            List<NameBasic> p_task = taskNames
                .Select((name, index) => new NameBasic { Id = index, Name = name })
                .ToList();


            ProjectCardData data = new ProjectCardData()
            {
                Name = project.Name,
                Description = project.Description,
                Customer = project.Customer.Id,
                Status = (uint)project.Status,
                Deathline = project.Dead_line,
                WorkedOut = "3 dny",
                WorkedByMe = "9 hodin",
                Tags = p_tag.ToArray(),
                Employees = employees,
                Tasks = p_task.ToArray()
            };
            return Ok("");
        }

        // POST api/<ProjectController>
        [HttpPost]
        public void Post(string name, string description, int customer, DateOnly? deathline, string[] tags, string employees)
        {
        }

        // PUT api/<ProjectController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProjectController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            //e1 - projekt neexistuje
            string error = "e1";
            string accept = "a";

            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());

            Project[] projects = [.. _context.Project_employees
                .Include(te => te.Project)
                .Include(te => te.Employee)
                .Where(te => te.Employee.Id == activeUser.Id)
                .Select(te => te.Project)
                .ToArray()];

            if (id >= 0 && id < projects.Length)
            {
                Project project = projects[id];
                Project_employee[] p_employees = [.. _context.Project_employees
                .Include(p => p.Project)
                .Where(p => p.Project == project)
                .ToArray()];
                Project_tag[] p_tag = [.. _context.Project_tags
                .Include(p => p.Project)
                .Where(p => p.Project == project)
                .ToArray()];
                Models.Task[] p_tasks = [.. _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.Project == project)
                .ToArray()];

                var result = p_tasks.Select(task => new
                {
                    t_employees = _context.Task_employees
                        .Include(t => t.Task)
                        .Where(t => t.Task == task)
                        .ToArray(),
                    t_tags = _context.Task_tags
                        .Include(t => t.Task == task)
                        .ToArray()
                });
                Task_employee[] t_employees = result.SelectMany(r => r.t_employees).ToArray();
                Task_tag[] t_tag = result.SelectMany(r => r.t_tags).ToArray();

                _context.RemoveRange(t_employees, t_tag);
                _context.RemoveRange(t_tag);
                _context.RemoveRange(p_tasks);
                _context.RemoveRange(p_tag);
                _context.RemoveRange(p_employees);
                _context.Remove(project);
                _context.SaveChanges();

                return Ok(accept);
            }
            else
                return Ok(error);
        }
    }
}
