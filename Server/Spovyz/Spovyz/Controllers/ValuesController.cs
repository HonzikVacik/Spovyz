using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

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
            var claimsPrincipal = User as ClaimsPrincipal;
            if (claimsPrincipal != null)
            {
                var roleClaim = claimsPrincipal.FindFirst(ClaimTypes.Role);
                if (roleClaim != null)
                {
                    var role = roleClaim.Value;
                    return Ok(new { res2, role });
                }
            }
            return Ok(new { res2 });
        }
    }
}
