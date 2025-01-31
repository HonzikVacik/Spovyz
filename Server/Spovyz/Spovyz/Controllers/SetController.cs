using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spovyz.Models;
using System.Reflection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetController : ControllerBase
    {
        // GET: api/<SetController>
        [HttpGet("RoleEnum")]
        [Authorize]
        public IEnumerable<string> GetRole()
        {
            List<string> items = new List<string>();
            foreach (Enums.Role role in Enums.Role.GetValues(typeof(Enums.Role)))
            {
                RoleCeskyAttribute attribute = (RoleCeskyAttribute)role.GetType()
                    .GetField(role.ToString())
                    .GetCustomAttribute(typeof(RoleCeskyAttribute));

                if (attribute != null)
                {
                    items.Add(attribute.Nazev);
                }
            }
            return items;
        }

        [HttpGet("SexEnum")]
        [Authorize]
        public IEnumerable<string> GetSex()
        {
            List<string> items = new List<string>();
            foreach (Enums.Sex sex in Enums.Role.GetValues(typeof(Enums.Sex)))
            {
                RoleCeskyAttribute attribute = (RoleCeskyAttribute)sex.GetType()
                    .GetField(sex.ToString())
                    .GetCustomAttribute(typeof(RoleCeskyAttribute));

                if (attribute != null)
                {
                    items.Add(attribute.Nazev);
                }
            }
            return items;
        }

        // GET api/<SetController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SetController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SetController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SetController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
