using Microsoft.AspNetCore.Mvc;
using Spovyz.IServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatementController : ControllerBase
    {
        private readonly IStatementService _statementService;

        public StatementController(IStatementService statementService)
        {
            _statementService = statementService;
        }

        // GET: api/<Statement>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            return Ok();
        }

        // GET api/<Statement>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            return Ok();
        }

        // POST api/<Statement>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            return Ok();
        }

        // DELETE api/<Statement>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(uint id)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            return Ok();
        }
    }
}
