using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [Authorize]

        public IActionResult Get()
        {
            var res = "This is a protected value";
            var res2 = User?.Identity?.Name;
            return Ok(new { res2 });
        }
    }
}
