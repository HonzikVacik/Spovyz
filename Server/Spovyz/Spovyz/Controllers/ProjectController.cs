using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spovyz.Models;
using Spovyz.Transport_models;
using Spovyz.Services;
using System.Reflection;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ProjectService _projectService;

        public ProjectController(ApplicationDbContext context, ProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            /*// Testovací hodnoty
            List<EmployeeDashboardProject> datas = new List<EmployeeDashboardProject>();
            datas.Add(new EmployeeDashboardProject() { Id = 0, Name = "Project1" });
            datas.Add(new EmployeeDashboardProject() { Id = 1, Name = "Project2" });
            datas.Add(new EmployeeDashboardProject() { Id = 2, Name = "Project3" });
            return datas;*/

            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            (List<EmployeeDashboardProject>? data, string? error) = await _projectService.GetProjectList(UserName);
            
            if (error != null)
                return NotFound(error);
            return Ok(data);

            /*Employee? activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());

            if(activeUser == null)
                return NotFound();

            Project[] projects = [.. _context.Project_employees
                .Include(te => te.Project)
                .Include(te => te.Employee)
                .Where(te => te.Employee.Id == activeUser.Id)
                .Select(te => te.Project)
                .ToArray()];

            List<EmployeeDashboardProject> data = projects.Select(t => new EmployeeDashboardProject() { Id = t.Id, Name = t.Name }).ToList();
            return Ok(data);*/
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(uint ProjectId)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            (ProjectCardData? data, string? error) = await _projectService.GetProjectById(UserName, ProjectId);
            if (error != null)
                return NotFound(error);

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Post(string Name, string Description, int CustomerId, DateOnly? DeadLine, string[] Tags, uint[] Employees)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            await _projectService.AddProject(UserName, Name, Description, CustomerId, DeadLine, Tags, Employees);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(uint ProjectId, string Name, string Description, int CustomerId, DateOnly? DeadLine, int Status, string[] Tags, uint[] Employees)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            await _projectService.UpdateProject(UserName, ProjectId, Name, Description, CustomerId, DeadLine, Status, Tags, Employees);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(uint id)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            string result = await _projectService.DeleteProject(UserName, id);
            if (result != "Accept")
                return NotFound(result);

            return Ok(result);
        }
    }
}
