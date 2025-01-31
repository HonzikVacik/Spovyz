using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public ChatController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ChatController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ChatController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ChatController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
