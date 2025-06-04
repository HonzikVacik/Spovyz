using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spovyz.Models;
using Spovyz.Transport_models;
using Spovyz.IServices;
using System.Reflection;
using System.Threading.Tasks;
using Spovyz.InputModels;

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound("Uživatel nenalezen");

            (List<EmployeeDashboardProject>? data, string? error) = await _projectService.GetProjectList(UserName);
            
            if (error != null)
                return NotFound(error);
            return Ok(data);
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
                return NotFound("Uživatel nenalezen");

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
                return NotFound("Uživatel nenalezen");

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
                return NotFound("Uživatel nenalezen");

            string result = await _projectService.DeleteProject(UserName, id);
            if (result != "Accept")
                return NotFound(result);

            return Ok();
        }
    }
}
