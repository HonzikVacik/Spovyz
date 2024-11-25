using Microsoft.AspNetCore.Mvc;
using Spovyz.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeInformation : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeInformation(ApplicationDbContext context)
        {
            this._context = context;
        }

        // GET: api/<EmployeeInformation>
        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            Employee[] employees = [.. _context.Employees];
            //return new string[] { "value1", "value2" };
            return employees;
        }

        // GET api/<EmployeeInformation>/5
        [HttpGet("{id}")]
        public Employee Get(int id)
        {
            return _context.Employees.FirstOrDefault(e => e.Id == id);
        }

        // POST api/<EmployeeInformation>
        [HttpPost]
        public string Post([FromBody] int id, string username, string password, string firstName, string surname, string phoneNumber, string email, DateOnly dateOfBirth, int sex, string pronoun, string country, string city, int zipCode, string street, uint decNumber, int accountType, int supervisorId, uint pay, int companyId)
        {
            Company c = _context.Companies.FirstOrDefault();

            Employee employee = new Employee()
            {
                Username = username,
                Password = password,
                First_name = firstName,
                Surname = surname,
                Phone_number = phoneNumber,
                Email = email,
                Date_of_birth = dateOfBirth,
                Sex = (Enums.Sex)sex,
                Pronoun = pronoun,
                Country = country,
                City = city,
                Zip_code = zipCode,
                Street = street,
                Descriptive_number = decNumber,
                Account_type = (Enums.Role)accountType,
                Supervisor = null,
                Pay = pay,
                Company = c
            };
            return "Povedlo se";
        }

        // PUT api/<EmployeeInformation>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EmployeeInformation>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _context.Remove(_context.Employees.FirstOrDefault(e => e.Id == id));
        }
    }
}
