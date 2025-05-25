using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spovyz.Models;
using Spovyz.Transport_models;
using Spovyz.IServices;
using System.Reflection;
using System.Threading.Tasks;
using Spovyz.InputModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProjectService _projectService;

        public ProjectController(ApplicationDbContext context, IProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            // Testovací hodnoty
            /*List<EmployeeDashboardProject> datas = new List<EmployeeDashboardProject>();
            datas.Add(new EmployeeDashboardProject() { Id = 0, Name = "Project1" });
            datas.Add(new EmployeeDashboardProject() { Id = 1, Name = "Project2" });
            datas.Add(new EmployeeDashboardProject() { Id = 2, Name = "Project3" });
            return Ok(datas);*/

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

        [HttpGet("{ProjectId}")]
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
        [Authorize]
        public async Task<IActionResult> Post(ProjectPostInput ProjectPostInput)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            (ValidityControl.ResultStatus ResultStatus, string? Description) result = await _projectService.AddProject(UserName, ProjectPostInput.Name, ProjectPostInput.Description, ProjectPostInput.CustomerId, ProjectPostInput.DeadLine, ProjectPostInput.Tags, ProjectPostInput.Employees);

            if (result.ResultStatus == ValidityControl.ResultStatus.Error)
                return BadRequest(result.Description);
            if (result.ResultStatus == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.Description);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put(ProjectPutInput projectPutInput)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            (ValidityControl.ResultStatus ResultStatus, string? Description) result = await _projectService.UpdateProject(UserName, projectPutInput.ProjectId, projectPutInput.Name, projectPutInput.Description, projectPutInput.CustomerId, projectPutInput.DeadLine, projectPutInput.Status, projectPutInput.Tags, projectPutInput.Employees);

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
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            string result = await _projectService.DeleteProject(UserName, id);
            if (result != "Accept")
                return NotFound(result);

            return Ok();
        }
    }
}
