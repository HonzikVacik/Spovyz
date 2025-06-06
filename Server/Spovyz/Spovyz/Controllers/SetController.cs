using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spovyz.Models;
using Spovyz.Transport_models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SetController(ApplicationDbContext context)
        {
            this._context = context;
        }

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

        [HttpGet("StatementTypeEnum")]
        [Authorize]
        public IEnumerable<string> GetStatementType()
        {
            List<string> items = new List<string>();
            foreach (Enums.StatementType statementType in Enums.StatementType.GetValues(typeof(Enums.StatementType)))
            {
                RoleCeskyAttribute attribute = (RoleCeskyAttribute)statementType.GetType()
                    .GetField(statementType.ToString())
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

        [HttpGet("StatusEnum")]
        [Authorize(Roles = "Worker,Supervisor,Manager")]
        public IEnumerable<string> GetStatus()
        {
            List<string> items = new List<string>();
            foreach (Enums.Status status in Enums.Role.GetValues(typeof(Enums.Status)))
            {
                RoleCeskyAttribute attribute = (RoleCeskyAttribute)status.GetType()
                    .GetField(status.ToString())
                    .GetCustomAttribute(typeof(RoleCeskyAttribute));

                if (attribute != null)
                {
                    items.Add(attribute.Nazev);
                }
            }
            return items;
        }


        [HttpGet("Junior")]
        [Authorize]
        public IEnumerable<NameBasic> GetJunior([Required] bool project_task)
        {
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());

            //get direct workers
            Employee[] employees = [.. _context.Employees
                .Include(e => e.Company)
                .Include(e => e.Supervisor)
                .Where(e => e.Company == activeUser.Company && e.Supervisor == activeUser && (e.Account_type == Enums.Role.Manager || e.Account_type == Enums.Role.Supervisor || e.Account_type == Enums.Role.Worker))
                .ToArray()];

            //get other workers
            Employee[] employees1 = GetJuniorChild(employees, activeUser);
            employees1 = employees1.OrderBy(e => e.Account_type).ToArray();

            Employee[] ee2 = employees.Concat(employees1).ToArray();
            List<NameBasic> ee3 = new List<NameBasic>();
            foreach (var employee in ee2.Select((value, index) => new { index, value }))
            {
                ee3.Add(new NameBasic() { Id = (uint)employee.index, Name = employee.value.Username });
            }
            return ee3;
        }


        private Employee[] GetJuniorChild(Employee[] employees, Employee activeUser)
        {
            if(employees.Length == 0)
                return new Employee[0];
            else
            {

                Employee[] employees1 = [.. _context.Employees
                    .Include(e => e.Company)
                    .Include(e => e.Supervisor)
                    .Where(e => e.Company == activeUser.Company && employees.Contains(e.Supervisor) && (e.Account_type == Enums.Role.Manager || e.Account_type == Enums.Role.Supervisor || e.Account_type == Enums.Role.Worker))
                    .ToArray()];
                return employees1.Concat(GetJuniorChild(employees1 , activeUser)).ToArray();
            }
        }
    }
}
