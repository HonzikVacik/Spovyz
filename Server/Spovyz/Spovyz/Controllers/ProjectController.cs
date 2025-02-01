using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spovyz.Models;
using Spovyz.Transport_models;

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
        public IActionResult Delete(int id)
        {
            //e1 - bad id
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());
            Project[] projects = [.. _context.Project_employees
                .Include(te => te.Project)
                .Include(te => te.Employee)
                .Where(te => te.Employee.Id == activeUser.Id)
                .Select(te => te.Project)
                .ToArray()];

            if (id < 0 || id >= projects.Length)
                return Ok("e1");
            return Ok("a");
        }
    }
}
