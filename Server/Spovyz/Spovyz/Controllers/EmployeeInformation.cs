using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spovyz.Models;
using Spovyz.Transport_models;
using System.Security.Cryptography;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeInformation : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private ValidityControl validityControl = new ValidityControl();

        public EmployeeInformation(ApplicationDbContext context)
        {
            this._context = context;
        }

        // GET: api/<EmployeeInformation>
        [HttpGet]
        [Authorize]
        public IEnumerable<AdminDashboardData> Get()
        {
            uint i = 0;
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());
            Employee[] employees = [.. _context.Employees.Include(e => e.Company).Where(e => e.Company.Id == activeUser.Company.Id)];
            List<AdminDashboardData> data = employees.Select(e => new AdminDashboardData{ Id = i++, Username = e.Username}).ToList();
            data.Add(employees.Select(e => new AdminDashboardData { Id = i++, Username = e.Username }).First());
            return data;
        }


        [HttpGet("GetSelf")]
        [Authorize]
        public IActionResult GetSelf()
        {
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());
            string supervisor = "";
            if(activeUser.Supervisor != null)
                supervisor = activeUser.Supervisor.Id.ToString();
            EmployeeInformationData data = new EmployeeInformationData()
            {
                Username = activeUser.Username,
                SecurityVerification = activeUser.SecurityVerification,
                First_name = activeUser.First_name,
                Surname = activeUser.Surname,
                Phone_number = activeUser.Phone_number,
                Email = activeUser.Email,
                Date_of_birth = activeUser.Date_of_birth,
                Sex = activeUser.Sex,
                Pronoun = activeUser.Pronoun,
                Country = activeUser.Country,
                City = activeUser.City,
                Zip_code = activeUser.Zip_code,
                Street = activeUser.Street,
                Descriptive_number = activeUser.Descriptive_number,
                Account_type = activeUser.Account_type,
                Supervisor = supervisor
            };
            return Ok(data);
        }

        [HttpGet("GetSelfId")]
        [Authorize]
        public IActionResult GetSelfId()
        {
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());
            return Ok(activeUser.Id);
        }

        // GET api/<EmployeeInformation>/5
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get(int id)
        {
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());
            Employee[] employees = [.. _context.Employees.Include(e => e.Company).Where(e => e.Company.Id == activeUser.Company.Id)];
            if (id < 0 || id >= employees.Length)
            {
                return NotFound($"Uživatel s id {id} neexistuje");
            }
            Employee result = employees[id];
            return Ok(result.Username);
        }

        // POST api/<EmployeeInformation>
        [HttpPost]
        [Authorize]
        public IActionResult Post(string username, string password, string securityVerification, string firstName, string surname, string phoneNumber, string email, DateOnly dateOfBirth, int sex, string pronoun, string country, string city, int zipCode, string street, uint decNumber, int accountType, int supervisorId)
        {
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());

            if (validityControl.AllFilledOut(username, password, securityVerification, firstName, surname, phoneNumber, email, dateOfBirth, sex, pronoun, country, city, zipCode, street, decNumber, accountType))
                return Ok("e1");

            string validityControlString = validityControl.NewUser(_context, activeUser.Company.Id, username, password, securityVerification, accountType, sex, email, phoneNumber, dateOfBirth);
            if (validityControlString != "a")
                return Ok(validityControlString);

            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            Employee employee = new Employee()
            {
                Username = username,
                Password = hashedPassword,
                Salt = salt,
                SecurityVerification = securityVerification,
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
                Pay = 0,
                Company = activeUser.Company
            };

            _context.Add(employee);
            _context.SaveChanges();

            return Ok("a");
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
